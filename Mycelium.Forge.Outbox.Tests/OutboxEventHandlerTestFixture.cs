// ------------------------------------------------------------------------------------------------
// <copyright file="OutboxEventHandlerTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Outbox.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;

    using ErrorOr;

    using Microsoft.Extensions.Logging;

    using Moq;

    using Mycelium.Forge.Dal.Events;
    using Mycelium.Forge.Orm.Outbox;

    using NUnit.Framework;

    [TestFixture]
    [SuppressMessage("Performance", "CA1873:Avoid potentially expensive logging", Justification = "Moq expression tree in unit test.")]
    public class OutboxEventHandlerTestFixture
    {
        private Mock<ILogger> loggerMock;
        private TestAccountProfileChangedHandler handler;

        [SetUp]
        public void SetUp()
        {
            this.loggerMock = new Mock<ILogger>();
            this.handler = new TestAccountProfileChangedHandler(this.loggerMock.Object);
        }

        [Test]
        public async Task VerifyHandleAsync()
        {
            Assert.That(this.handler.EventType, Is.EqualTo(AccountProfileChanged.EventType));

            var aggregateId = Guid.NewGuid();
            var expectedEvent = new AccountProfileChanged(aggregateId, "alex@example.com", "Alex", "Rivera");
            var validPayload = JsonSerializer.Serialize(expectedEvent);
            var validMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = aggregateId, EventType = AccountProfileChanged.EventType, Payload = validPayload };

            // Case 1: Valid payload deserializes and delegates to typed HandleAsync
            var validResult = await this.handler.HandleAsync(validMessage, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validResult.IsError, Is.False);
                Assert.That(this.handler.LastHandledEvent, Is.EqualTo(expectedEvent));
            }

            // Case 2: Invalid JSON returns permanent Validation error without invoking typed HandleAsync
            this.handler.LastHandledEvent = null;
            var invalidJsonMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = aggregateId, EventType = AccountProfileChanged.EventType, Payload = "{not-valid" };
            var invalidJsonResult = await this.handler.HandleAsync(invalidJsonMessage, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(invalidJsonResult.IsError, Is.True);
                Assert.That(invalidJsonResult.FirstError.Type, Is.EqualTo(ErrorType.Validation));
                Assert.That(invalidJsonResult.FirstError.Code, Is.EqualTo("Outbox.InvalidPayload"));
                Assert.That(this.handler.LastHandledEvent, Is.Null);
            }

            // Case 3: Null JSON or empty AggregateId returns permanent Validation error
            var nullMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = aggregateId, EventType = AccountProfileChanged.EventType, Payload = "null" };
            var emptyAggregatePayload = JsonSerializer.Serialize(new AccountProfileChanged(Guid.Empty, "alex@example.com", "Alex", "Rivera"));
            var emptyAggregateMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = aggregateId, EventType = AccountProfileChanged.EventType, Payload = emptyAggregatePayload };

            var nullResult = await this.handler.HandleAsync(nullMessage, CancellationToken.None);
            var emptyAggregateResult = await this.handler.HandleAsync(emptyAggregateMessage, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(nullResult.IsError, Is.True);
                Assert.That(nullResult.FirstError.Type, Is.EqualTo(ErrorType.Validation));
                Assert.That(emptyAggregateResult.IsError, Is.True);
                Assert.That(emptyAggregateResult.FirstError.Type, Is.EqualTo(ErrorType.Validation));
                Assert.That(this.handler.LastHandledEvent, Is.Null);
            }
        }

        private sealed class TestAccountProfileChangedHandler(ILogger logger) : OutboxEventHandler<AccountProfileChanged>(logger)
        {
            public AccountProfileChanged LastHandledEvent { get; set; }

            protected override Task<ErrorOr<Success>> HandleAsync(AccountProfileChanged outboxEvent, CancellationToken token)
            {
                this.LastHandledEvent = outboxEvent;
                return Task.FromResult<ErrorOr<Success>>(Result.Success);
            }
        }
    }
}
