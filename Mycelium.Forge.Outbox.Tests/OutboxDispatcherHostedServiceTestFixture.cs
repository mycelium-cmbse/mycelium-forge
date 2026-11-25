// ------------------------------------------------------------------------------------------------
// <copyright file="OutboxDispatcherHostedServiceTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Outbox.Tests
{
    using System.Diagnostics.CodeAnalysis;

    using ErrorOr;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Moq;

    using Mycelium.Forge.Dal.DatabaseSource;
    using Mycelium.Forge.Orm.Outbox;

    using Npgsql;

    using NUnit.Framework;

    [TestFixture]
    [SuppressMessage("Performance", "CA1873:Avoid potentially expensive logging", Justification = "Moq expression tree in unit test.")]
    public class OutboxDispatcherHostedServiceTestFixture
    {
        private const string TestEventType = "Account.ProfileChanged";

        private Mock<IDatabaseSource> databaseSourceMock;
        private Mock<IOutboxDao> outboxDaoMock;
        private Mock<IOutboxEventHandler> eventHandlerMock;
        private Mock<IOutboxEventHandler> secondaryHandlerMock;
        private Mock<ILogger<OutboxDispatcherHostedService>> loggerMock;
        private ServiceProvider serviceProvider;
        private IServiceScopeFactory serviceScopeFactory;
        private OutboxOptions options;

        [SetUp]
        public void SetUp()
        {
            this.databaseSourceMock = new Mock<IDatabaseSource>();
            this.outboxDaoMock = new Mock<IOutboxDao>();
            this.eventHandlerMock = new Mock<IOutboxEventHandler>();
            this.secondaryHandlerMock = new Mock<IOutboxEventHandler>();
            this.loggerMock = new Mock<ILogger<OutboxDispatcherHostedService>>();

            this.eventHandlerMock.Setup(x => x.EventType).Returns(TestEventType);
            this.secondaryHandlerMock.Setup(x => x.EventType).Returns("Secondary.Only");

            var services = new ServiceCollection();
            services.AddScoped(_ => this.eventHandlerMock.Object);
            services.AddScoped(_ => this.secondaryHandlerMock.Object);
            this.serviceProvider = services.BuildServiceProvider();
            this.serviceScopeFactory = this.serviceProvider.GetRequiredService<IServiceScopeFactory>();

            this.options = new OutboxOptions
            {
                Enabled = true,
                PollingIntervalSeconds = 1,
                BatchSize = 20,
                MaxAttempts = 5,
                LeaseDurationSeconds = 30,
                BaseRetryDelaySeconds = 10,
                MaxRetryDelaySeconds = 300
            };

            this.ConfigureTransactionPassThrough();
        }

        [TearDown]
        public void TearDown()
        {
            this.serviceProvider.Dispose();
        }

        [Test]
        [CancelAfter(10000)]
        public async Task VerifyStartAsync()
        {
            // Case 1: Disabled outbox exits immediately without claiming batches
            using var disabledService = new OutboxDispatcherHostedService(this.databaseSourceMock.Object, this.outboxDaoMock.Object, this.serviceScopeFactory, Options.Create(new OutboxOptions { Enabled = false }), this.loggerMock.Object);
            await disabledService.StartAsync(CancellationToken.None);
            await disabledService.ExecuteTask!;

            this.outboxDaoMock.Verify(x => x.ClaimBatchAsync(It.IsAny<NpgsqlTransaction>(), It.IsAny<int>(), It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()), Times.Never());

            // Case 2: Empty batch does not dispatch or mutate outbox state
            IReadOnlyList<OutboxMessage> emptyBatch = [];
            var emptyBatchCompleted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            this.outboxDaoMock.Setup(x => x.ClaimBatchAsync(It.IsAny<NpgsqlTransaction>(), 20, TimeSpan.FromSeconds(30), It.IsAny<CancellationToken>())).Callback(() => emptyBatchCompleted.TrySetResult()).ReturnsAsync(ErrorOrFactory.From(emptyBatch));

            using var emptyBatchService = new OutboxDispatcherHostedService(this.databaseSourceMock.Object, this.outboxDaoMock.Object, this.serviceScopeFactory, Options.Create(this.options), this.loggerMock.Object);
            await emptyBatchService.StartAsync(CancellationToken.None);
            await emptyBatchCompleted.Task;
            await emptyBatchService.StopAsync(CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                this.eventHandlerMock.Verify(x => x.HandleAsync(It.IsAny<OutboxMessage>(), It.IsAny<CancellationToken>()), Times.Never());
                this.outboxDaoMock.Verify(x => x.DeleteAsync(It.IsAny<NpgsqlTransaction>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never());
            }

            // Case 3: ClaimBatchAsync error logs warning and continues polling safely
            var claimErrorCompleted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            this.outboxDaoMock.Setup(x => x.ClaimBatchAsync(It.IsAny<NpgsqlTransaction>(), 20, TimeSpan.FromSeconds(30), It.IsAny<CancellationToken>())).Callback(() => claimErrorCompleted.TrySetResult()).ReturnsAsync(Error.Failure("Outbox.ClaimError", "Claim failed"));

            using var claimErrorService = new OutboxDispatcherHostedService(this.databaseSourceMock.Object, this.outboxDaoMock.Object, this.serviceScopeFactory, Options.Create(this.options), this.loggerMock.Object);
            await claimErrorService.StartAsync(CancellationToken.None);
            await claimErrorCompleted.Task;
            await claimErrorService.StopAsync(CancellationToken.None);

            this.loggerMock.Verify(x => x.Log(LogLevel.Warning, It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => true), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.AtLeastOnce());

            // Case 4: Matching handler succeeds -> deletes message using its claim token
            this.ResetMocks();
            var successClaimToken = Guid.NewGuid();
            var successMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = Guid.NewGuid(), EventType = TestEventType, Payload = "{}", Attempts = 1, ClaimToken = successClaimToken };
            var deleteCompleted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            IReadOnlyList<OutboxMessage> successBatch = [successMessage];
            IReadOnlyList<OutboxMessage> emptyBatch2 = [];

            this.outboxDaoMock.SetupSequence(x => x.ClaimBatchAsync(It.IsAny<NpgsqlTransaction>(), 20, TimeSpan.FromSeconds(30), It.IsAny<CancellationToken>())).ReturnsAsync(ErrorOrFactory.From(successBatch)).ReturnsAsync(ErrorOrFactory.From(emptyBatch2));

            this.eventHandlerMock.Setup(x => x.HandleAsync(successMessage, It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);
            this.outboxDaoMock.Setup(x => x.DeleteAsync(It.IsAny<NpgsqlTransaction>(), successMessage.Id, successClaimToken, It.IsAny<CancellationToken>())).Callback(() => deleteCompleted.TrySetResult()).ReturnsAsync(Result.Deleted);

            using var successService = new OutboxDispatcherHostedService(this.databaseSourceMock.Object, this.outboxDaoMock.Object, this.serviceScopeFactory, Options.Create(this.options), this.loggerMock.Object);
            await successService.StartAsync(CancellationToken.None);
            await deleteCompleted.Task;
            await successService.StopAsync(CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                this.eventHandlerMock.Verify(x => x.HandleAsync(successMessage, It.IsAny<CancellationToken>()), Times.Once());
                this.outboxDaoMock.Verify(x => x.DeleteAsync(It.IsAny<NpgsqlTransaction>(), successMessage.Id, successClaimToken, It.IsAny<CancellationToken>()), Times.Once());
                this.outboxDaoMock.Verify(x => x.RescheduleAsync(It.IsAny<NpgsqlTransaction>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateTimeOffset>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never());
                this.outboxDaoMock.Verify(x => x.MarkFailedAsync(It.IsAny<NpgsqlTransaction>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never());
            }

            // Case 5: Multiple handlers for the same event type fan out; if one fails transiently, message is rescheduled
            this.ResetMocks();
            this.secondaryHandlerMock.Setup(x => x.EventType).Returns(TestEventType);
            var fanOutToken = Guid.NewGuid();
            var fanOutMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = Guid.NewGuid(), EventType = TestEventType, Payload = "{}", Attempts = 1, ClaimToken = fanOutToken };
            var fanOutCompleted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            IReadOnlyList<OutboxMessage> fanOutBatch = [fanOutMessage];

            this.outboxDaoMock.SetupSequence(x => x.ClaimBatchAsync(It.IsAny<NpgsqlTransaction>(), 20, TimeSpan.FromSeconds(30), It.IsAny<CancellationToken>())).ReturnsAsync(ErrorOrFactory.From(fanOutBatch)).ReturnsAsync(ErrorOrFactory.From(emptyBatch2));
            this.eventHandlerMock.Setup(x => x.HandleAsync(fanOutMessage, It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);
            this.secondaryHandlerMock.Setup(x => x.HandleAsync(fanOutMessage, It.IsAny<CancellationToken>())).ReturnsAsync(Error.Unauthorized("Keycloak.SyncFailed", "Unauthorized"));
            this.outboxDaoMock.Setup(x => x.RescheduleAsync(It.IsAny<NpgsqlTransaction>(), fanOutMessage.Id, fanOutToken, It.IsAny<DateTimeOffset>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Callback(() => fanOutCompleted.TrySetResult()).ReturnsAsync(Result.Updated);

            using var fanOutService = new OutboxDispatcherHostedService(this.databaseSourceMock.Object, this.outboxDaoMock.Object, this.serviceScopeFactory, Options.Create(this.options), this.loggerMock.Object);
            await fanOutService.StartAsync(CancellationToken.None);
            await fanOutCompleted.Task;
            await fanOutService.StopAsync(CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                this.eventHandlerMock.Verify(x => x.HandleAsync(fanOutMessage, It.IsAny<CancellationToken>()), Times.Once());
                this.secondaryHandlerMock.Verify(x => x.HandleAsync(fanOutMessage, It.IsAny<CancellationToken>()), Times.Once());
                this.outboxDaoMock.Verify(x => x.RescheduleAsync(It.IsAny<NpgsqlTransaction>(), fanOutMessage.Id, fanOutToken, It.IsAny<DateTimeOffset>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
                this.outboxDaoMock.Verify(x => x.DeleteAsync(It.IsAny<NpgsqlTransaction>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never());
            }

            // Case 6: Unknown event type -> marks message as permanently failed
            this.ResetMocks();
            var unknownClaimToken = Guid.NewGuid();
            var unknownMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = Guid.NewGuid(), EventType = "Unknown.Event", Payload = "{}", Attempts = 1, ClaimToken = unknownClaimToken };
            var unknownFailedCompleted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var capturedUnknownError = string.Empty;

            IReadOnlyList<OutboxMessage> unknownBatch = [unknownMessage];

            this.outboxDaoMock.SetupSequence(x => x.ClaimBatchAsync(It.IsAny<NpgsqlTransaction>(), 20, TimeSpan.FromSeconds(30), It.IsAny<CancellationToken>())).ReturnsAsync(ErrorOrFactory.From(unknownBatch)).ReturnsAsync(ErrorOrFactory.From(emptyBatch2));

            this.outboxDaoMock.Setup(x => x.MarkFailedAsync(It.IsAny<NpgsqlTransaction>(), unknownMessage.Id, unknownClaimToken, It.IsAny<string>(), It.IsAny<CancellationToken>())).Callback<NpgsqlTransaction, Guid, Guid, string, CancellationToken>((_, _, _, error, _) =>
            {
                capturedUnknownError = error;
                unknownFailedCompleted.TrySetResult();
            }).ReturnsAsync(Result.Updated);

            using var unknownEventService = new OutboxDispatcherHostedService(this.databaseSourceMock.Object, this.outboxDaoMock.Object, this.serviceScopeFactory, Options.Create(this.options), this.loggerMock.Object);
            await unknownEventService.StartAsync(CancellationToken.None);
            await unknownFailedCompleted.Task;
            await unknownEventService.StopAsync(CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(capturedUnknownError, Does.Contain("Unknown.Event"));
                this.outboxDaoMock.Verify(x => x.MarkFailedAsync(It.IsAny<NpgsqlTransaction>(), unknownMessage.Id, unknownClaimToken, It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
                this.eventHandlerMock.Verify(x => x.HandleAsync(It.IsAny<OutboxMessage>(), It.IsAny<CancellationToken>()), Times.Never());
            }

            // Case 7: Transient error below MaxAttempts -> reschedules with exponential backoff + jitter
            this.ResetMocks();
            var transientClaimToken = Guid.NewGuid();
            var transientMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = Guid.NewGuid(), EventType = TestEventType, Payload = "{}", Attempts = 3, ClaimToken = transientClaimToken };
            var rescheduleCompleted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var beforeDispatch = DateTimeOffset.UtcNow;
            DateTimeOffset capturedNextAttemptAt = default;
            var capturedRescheduleError = string.Empty;

            IReadOnlyList<OutboxMessage> transientBatch = [transientMessage];

            this.outboxDaoMock.SetupSequence(x => x.ClaimBatchAsync(It.IsAny<NpgsqlTransaction>(), 20, TimeSpan.FromSeconds(30), It.IsAny<CancellationToken>())).ReturnsAsync(ErrorOrFactory.From(transientBatch)).ReturnsAsync(ErrorOrFactory.From(emptyBatch2));

            this.eventHandlerMock.Setup(x => x.HandleAsync(transientMessage, It.IsAny<CancellationToken>())).ReturnsAsync(Error.Failure("Keycloak.SyncFailed", "Keycloak 503"));

            this.outboxDaoMock.Setup(x => x.RescheduleAsync(It.IsAny<NpgsqlTransaction>(), transientMessage.Id, transientClaimToken, It.IsAny<DateTimeOffset>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Callback<NpgsqlTransaction, Guid, Guid, DateTimeOffset, string, CancellationToken>((_, _, _, nextAttemptAt, error, _) =>
            {
                capturedNextAttemptAt = nextAttemptAt;
                capturedRescheduleError = error;
                rescheduleCompleted.TrySetResult();
            }).ReturnsAsync(Result.Updated);

            using var rescheduleService = new OutboxDispatcherHostedService(this.databaseSourceMock.Object, this.outboxDaoMock.Object, this.serviceScopeFactory, Options.Create(this.options), this.loggerMock.Object);
            await rescheduleService.StartAsync(CancellationToken.None);
            await rescheduleCompleted.Task;
            await rescheduleService.StopAsync(CancellationToken.None);

            // Attempt 3 with BaseRetryDelaySeconds=10 => base*2^(3-1)=40s, jitter [0.8, 1.2] => [32s, 48s]
            var delay = capturedNextAttemptAt - beforeDispatch;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(delay.TotalSeconds, Is.GreaterThanOrEqualTo(31.0));
                Assert.That(delay.TotalSeconds, Is.LessThanOrEqualTo(50.0));
                Assert.That(capturedRescheduleError, Does.Contain("Keycloak 503"));
                this.outboxDaoMock.Verify(x => x.MarkFailedAsync(It.IsAny<NpgsqlTransaction>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never());
            }

            // Case 8: Permanent Validation error below MaxAttempts -> marks failed immediately without rescheduling
            this.ResetMocks();
            var permanentClaimToken = Guid.NewGuid();
            var permanentMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = Guid.NewGuid(), EventType = TestEventType, Payload = "{}", Attempts = 1, ClaimToken = permanentClaimToken };
            var permanentCompleted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            IReadOnlyList<OutboxMessage> permanentBatch = [permanentMessage];

            this.outboxDaoMock.SetupSequence(x => x.ClaimBatchAsync(It.IsAny<NpgsqlTransaction>(), 20, TimeSpan.FromSeconds(30), It.IsAny<CancellationToken>())).ReturnsAsync(ErrorOrFactory.From(permanentBatch)).ReturnsAsync(ErrorOrFactory.From(emptyBatch2));

            this.eventHandlerMock.Setup(x => x.HandleAsync(permanentMessage, It.IsAny<CancellationToken>())).ReturnsAsync(Error.Validation("Keycloak.SyncFailed", "Bad request"));
            this.outboxDaoMock.Setup(x => x.MarkFailedAsync(It.IsAny<NpgsqlTransaction>(), permanentMessage.Id, permanentClaimToken, It.IsAny<string>(), It.IsAny<CancellationToken>())).Callback(() => permanentCompleted.TrySetResult()).ReturnsAsync(Result.Updated);

            using var permanentService = new OutboxDispatcherHostedService(this.databaseSourceMock.Object, this.outboxDaoMock.Object, this.serviceScopeFactory, Options.Create(this.options), this.loggerMock.Object);
            await permanentService.StartAsync(CancellationToken.None);
            await permanentCompleted.Task;
            await permanentService.StopAsync(CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                this.outboxDaoMock.Verify(x => x.MarkFailedAsync(It.IsAny<NpgsqlTransaction>(), permanentMessage.Id, permanentClaimToken, It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
                this.outboxDaoMock.Verify(x => x.RescheduleAsync(It.IsAny<NpgsqlTransaction>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateTimeOffset>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never());
            }

            // Case 9: Transient error reaching MaxAttempts -> marks failed immediately
            this.ResetMocks();
            var exhaustedClaimToken = Guid.NewGuid();
            var exhaustedMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = Guid.NewGuid(), EventType = TestEventType, Payload = "{}", Attempts = 5, ClaimToken = exhaustedClaimToken };
            var exhaustedCompleted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            IReadOnlyList<OutboxMessage> exhaustedBatch = [exhaustedMessage];

            this.outboxDaoMock.SetupSequence(x => x.ClaimBatchAsync(It.IsAny<NpgsqlTransaction>(), 20, TimeSpan.FromSeconds(30), It.IsAny<CancellationToken>())).ReturnsAsync(ErrorOrFactory.From(exhaustedBatch)).ReturnsAsync(ErrorOrFactory.From(emptyBatch2));

            this.eventHandlerMock.Setup(x => x.HandleAsync(exhaustedMessage, It.IsAny<CancellationToken>())).ReturnsAsync(Error.Failure("Keycloak.SyncFailed", "Timeout"));
            this.outboxDaoMock.Setup(x => x.MarkFailedAsync(It.IsAny<NpgsqlTransaction>(), exhaustedMessage.Id, exhaustedClaimToken, It.IsAny<string>(), It.IsAny<CancellationToken>())).Callback(() => exhaustedCompleted.TrySetResult()).ReturnsAsync(Result.Updated);

            using var exhaustedService = new OutboxDispatcherHostedService(this.databaseSourceMock.Object, this.outboxDaoMock.Object, this.serviceScopeFactory, Options.Create(this.options), this.loggerMock.Object);
            await exhaustedService.StartAsync(CancellationToken.None);
            await exhaustedCompleted.Task;
            await exhaustedService.StopAsync(CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                this.outboxDaoMock.Verify(x => x.MarkFailedAsync(It.IsAny<NpgsqlTransaction>(), exhaustedMessage.Id, exhaustedClaimToken, It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
                this.outboxDaoMock.Verify(x => x.RescheduleAsync(It.IsAny<NpgsqlTransaction>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateTimeOffset>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never());
            }

            // Case 10: Unexpected handler exception is caught and rescheduled when below MaxAttempts
            this.ResetMocks();
            var throwingClaimToken = Guid.NewGuid();
            var throwingMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = Guid.NewGuid(), EventType = TestEventType, Payload = "{}", Attempts = 1, ClaimToken = throwingClaimToken };
            var handlerExceptionCompleted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            IReadOnlyList<OutboxMessage> throwingBatch = [throwingMessage];

            this.outboxDaoMock.SetupSequence(x => x.ClaimBatchAsync(It.IsAny<NpgsqlTransaction>(), 20, TimeSpan.FromSeconds(30), It.IsAny<CancellationToken>())).ReturnsAsync(ErrorOrFactory.From(throwingBatch)).ReturnsAsync(ErrorOrFactory.From(emptyBatch2));

            this.eventHandlerMock.Setup(x => x.HandleAsync(throwingMessage, It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException("Handler crashed"));
            this.outboxDaoMock.Setup(x => x.RescheduleAsync(It.IsAny<NpgsqlTransaction>(), throwingMessage.Id, throwingClaimToken, It.IsAny<DateTimeOffset>(), It.Is<string>(error => error.Contains("Handler crashed")), It.IsAny<CancellationToken>())).Callback(() => handlerExceptionCompleted.TrySetResult()).ReturnsAsync(Result.Updated);

            using var throwingHandlerService = new OutboxDispatcherHostedService(this.databaseSourceMock.Object, this.outboxDaoMock.Object, this.serviceScopeFactory, Options.Create(this.options), this.loggerMock.Object);
            await throwingHandlerService.StartAsync(CancellationToken.None);
            await handlerExceptionCompleted.Task;
            await throwingHandlerService.StopAsync(CancellationToken.None);

            this.outboxDaoMock.Verify(x => x.RescheduleAsync(It.IsAny<NpgsqlTransaction>(), throwingMessage.Id, throwingClaimToken, It.IsAny<DateTimeOffset>(), It.Is<string>(error => error.Contains("Handler crashed")), It.IsAny<CancellationToken>()), Times.Once());

            // Case 11: Lease ownership conflict during delete logs warning and does not throw
            this.ResetMocks();
            var conflictClaimToken = Guid.NewGuid();
            var conflictMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = Guid.NewGuid(), EventType = TestEventType, Payload = "{}", Attempts = 1, ClaimToken = conflictClaimToken };
            var conflictCompleted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            IReadOnlyList<OutboxMessage> conflictBatch = [conflictMessage];

            this.outboxDaoMock.SetupSequence(x => x.ClaimBatchAsync(It.IsAny<NpgsqlTransaction>(), 20, TimeSpan.FromSeconds(30), It.IsAny<CancellationToken>())).ReturnsAsync(ErrorOrFactory.From(conflictBatch)).ReturnsAsync(ErrorOrFactory.From(emptyBatch2));
            this.eventHandlerMock.Setup(x => x.HandleAsync(conflictMessage, It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);
            this.outboxDaoMock.Setup(x => x.DeleteAsync(It.IsAny<NpgsqlTransaction>(), conflictMessage.Id, conflictClaimToken, It.IsAny<CancellationToken>())).Callback(() => conflictCompleted.TrySetResult()).ReturnsAsync(Error.Conflict("Outbox.Conflict", "Ownership lost"));

            using var conflictService = new OutboxDispatcherHostedService(this.databaseSourceMock.Object, this.outboxDaoMock.Object, this.serviceScopeFactory, Options.Create(this.options), this.loggerMock.Object);
            await conflictService.StartAsync(CancellationToken.None);
            await conflictCompleted.Task;
            await conflictService.StopAsync(CancellationToken.None);

            this.loggerMock.Verify(x => x.Log(LogLevel.Warning, It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => true), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.AtLeastOnce());

            // Case 12: Unexpected database exception during polling is caught and logged without crashing background loop
            this.ResetMocks();
            var pollExceptionCompleted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            this.databaseSourceMock.Setup(x => x.ExecuteInTransactionAsync(It.IsAny<Func<NpgsqlTransaction, Task<ErrorOr<IReadOnlyList<OutboxMessage>>>>>(), It.IsAny<CancellationToken>())).Callback(() => pollExceptionCompleted.TrySetResult()).ThrowsAsync(new InvalidOperationException("Database temporarily unavailable"));

            using var resilientService = new OutboxDispatcherHostedService(this.databaseSourceMock.Object, this.outboxDaoMock.Object, this.serviceScopeFactory, Options.Create(this.options), this.loggerMock.Object);
            await resilientService.StartAsync(CancellationToken.None);
            await pollExceptionCompleted.Task;
            await resilientService.StopAsync(CancellationToken.None);

            this.loggerMock.Verify(x => x.Log(LogLevel.Warning, It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => true), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.AtLeastOnce());
        }

        private void ConfigureTransactionPassThrough()
        {
            this.databaseSourceMock.Setup(x => x.ExecuteInTransactionAsync(It.IsAny<Func<NpgsqlTransaction, Task<ErrorOr<IReadOnlyList<OutboxMessage>>>>>(), It.IsAny<CancellationToken>())).Returns<Func<NpgsqlTransaction, Task<ErrorOr<IReadOnlyList<OutboxMessage>>>>, CancellationToken>((operation, _) => operation(null!));
            this.databaseSourceMock.Setup(x => x.ExecuteInTransactionAsync(It.IsAny<Func<NpgsqlTransaction, Task<ErrorOr<Deleted>>>>(), It.IsAny<CancellationToken>())).Returns<Func<NpgsqlTransaction, Task<ErrorOr<Deleted>>>, CancellationToken>((operation, _) => operation(null!));
            this.databaseSourceMock.Setup(x => x.ExecuteInTransactionAsync(It.IsAny<Func<NpgsqlTransaction, Task<ErrorOr<Updated>>>>(), It.IsAny<CancellationToken>())).Returns<Func<NpgsqlTransaction, Task<ErrorOr<Updated>>>, CancellationToken>((operation, _) => operation(null!));
        }

        private void ResetMocks()
        {
            this.databaseSourceMock.Reset();
            this.outboxDaoMock.Reset();
            this.eventHandlerMock.Reset();
            this.secondaryHandlerMock.Reset();
            this.loggerMock.Reset();
            this.eventHandlerMock.Setup(x => x.EventType).Returns(TestEventType);
            this.secondaryHandlerMock.Setup(x => x.EventType).Returns("Secondary.Only");
            this.ConfigureTransactionPassThrough();
        }
    }
}
