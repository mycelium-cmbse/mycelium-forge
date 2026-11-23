// ------------------------------------------------------------------------------------------------
// <copyright file="KeycloakAccountSyncOutboxHandlerTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    using ErrorOr;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.Services;
    using Mycelium.Forge.Orm.Outbox;
    using Mycelium.Forge.Services;

    [TestFixture]
    [SuppressMessage("Performance", "CA1873:Avoid potentially expensive logging", Justification = "Moq expression tree in unit test.")]
    public class KeycloakAccountSyncOutboxHandlerTestFixture
    {
        private Mock<IKeycloakAdminClient> keycloakAdminClientMock;
        private Mock<ILogger<KeycloakAccountSyncOutboxHandler>> loggerMock;
        private KeycloakAccountSyncOutboxHandler handler;

        [SetUp]
        public void SetUp()
        {
            this.keycloakAdminClientMock = new Mock<IKeycloakAdminClient>();
            this.loggerMock = new Mock<ILogger<KeycloakAccountSyncOutboxHandler>>();

            var options = Options.Create(new OutboxOptions { ImmediateRetryCount = 2 });
            this.handler = new KeycloakAccountSyncOutboxHandler(this.keycloakAdminClientMock.Object, options, this.loggerMock.Object);
        }

        [Test]
        public async Task VerifyHandleAsync()
        {
            Assert.That(this.handler.EventType, Is.EqualTo(OutboxEventTypes.AccountKeycloakSync));

            var accountId = Guid.NewGuid();
            var validPayload = JsonSerializer.Serialize(new KeycloakAccountSyncPayload(accountId, "alex@example.com", "Alex", "Rivera"));
            var validMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = accountId, EventType = OutboxEventTypes.AccountKeycloakSync, Payload = validPayload };

            // Case 1: Valid payload dispatches to KeycloakAdminClient.UpdateUserAsync
            this.keycloakAdminClientMock.Setup(x => x.UpdateUserAsync(accountId, "alex@example.com", "Alex", "Rivera", CancellationToken.None)).ReturnsAsync(Result.Success);

            var validResult = await this.handler.HandleAsync(validMessage, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validResult.IsError, Is.False);
                this.keycloakAdminClientMock.Verify(x => x.UpdateUserAsync(accountId, "alex@example.com", "Alex", "Rivera", CancellationToken.None), Times.Once());
            }

            // Case 2: Null firstName/lastName values pass through cleanly
            var blankNamePayload = JsonSerializer.Serialize(new KeycloakAccountSyncPayload(accountId, "alex@example.com", null!, null!));
            var blankNameMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = accountId, EventType = OutboxEventTypes.AccountKeycloakSync, Payload = blankNamePayload };
            this.keycloakAdminClientMock.Setup(x => x.UpdateUserAsync(accountId, "alex@example.com", null!, null!, CancellationToken.None)).ReturnsAsync(Result.Success);

            var blankNameResult = await this.handler.HandleAsync(blankNameMessage, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(blankNameResult.IsError, Is.False);
                this.keycloakAdminClientMock.Verify(x => x.UpdateUserAsync(accountId, "alex@example.com", null!, null!, CancellationToken.None), Times.Once());
            }

            // Case 3: Invalid JSON payload returns validation error without calling KeycloakAdminClient
            var invalidJsonMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = accountId, EventType = OutboxEventTypes.AccountKeycloakSync, Payload = "{not-valid-json" };

            var invalidJsonResult = await this.handler.HandleAsync(invalidJsonMessage, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(invalidJsonResult.IsError, Is.True);
                Assert.That(invalidJsonResult.FirstError.Type, Is.EqualTo(ErrorType.Validation));
                Assert.That(invalidJsonResult.FirstError.Code, Is.EqualTo("Outbox.InvalidPayload"));
            }

            // Case 4: Null or empty AccountId payload returns validation error without calling KeycloakAdminClient
            var nullPayloadMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = accountId, EventType = OutboxEventTypes.AccountKeycloakSync, Payload = "null" };
            var emptyAccountPayload = JsonSerializer.Serialize(new KeycloakAccountSyncPayload(Guid.Empty, "alex@example.com", "Alex", "Rivera"));
            var emptyAccountMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = accountId, EventType = OutboxEventTypes.AccountKeycloakSync, Payload = emptyAccountPayload };

            var nullPayloadResult = await this.handler.HandleAsync(nullPayloadMessage, CancellationToken.None);
            var emptyAccountResult = await this.handler.HandleAsync(emptyAccountMessage, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(nullPayloadResult.IsError, Is.True);
                Assert.That(nullPayloadResult.FirstError.Type, Is.EqualTo(ErrorType.Validation));
                Assert.That(nullPayloadResult.FirstError.Code, Is.EqualTo("Outbox.InvalidPayload"));
                Assert.That(emptyAccountResult.IsError, Is.True);
                Assert.That(emptyAccountResult.FirstError.Type, Is.EqualTo(ErrorType.Validation));
                Assert.That(emptyAccountResult.FirstError.Code, Is.EqualTo("Outbox.InvalidPayload"));
            }

            // Case 5: Permanent HTTP 4xx error is not retried by Polly
            this.keycloakAdminClientMock.Invocations.Clear();
            var permanentError = Error.Failure(KeycloakAdminClient.SyncFailedErrorCode, "Bad Request", new Dictionary<string, object> { { OutboxErrorClassifier.StatusCodeMetadataKey, 400 } });
            this.keycloakAdminClientMock.Setup(x => x.UpdateUserAsync(accountId, "alex@example.com", "Alex", "Rivera", It.IsAny<CancellationToken>())).ReturnsAsync(permanentError);

            var permanentResult = await this.handler.HandleAsync(validMessage, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(permanentResult.IsError, Is.True);
                Assert.That(permanentResult.FirstError.Code, Is.EqualTo(KeycloakAdminClient.SyncFailedErrorCode));
                this.keycloakAdminClientMock.Verify(x => x.UpdateUserAsync(accountId, "alex@example.com", "Alex", "Rivera", It.IsAny<CancellationToken>()), Times.Once());
            }

            // Case 6: Transient error retries via Polly and succeeds when subsequent attempt succeeds
            this.keycloakAdminClientMock.Invocations.Clear();
            var transientError = Error.Failure(KeycloakAdminClient.SyncFailedErrorCode, "Service Unavailable", new Dictionary<string, object> { { OutboxErrorClassifier.StatusCodeMetadataKey, 503 } });

            this.keycloakAdminClientMock.SetupSequence(x => x.UpdateUserAsync(accountId, "alex@example.com", "Alex", "Rivera", It.IsAny<CancellationToken>())).ReturnsAsync(transientError).ReturnsAsync(Result.Success);

            var transientRecoveredResult = await this.handler.HandleAsync(validMessage, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(transientRecoveredResult.IsError, Is.False);
                this.keycloakAdminClientMock.Verify(x => x.UpdateUserAsync(accountId, "alex@example.com", "Alex", "Rivera", It.IsAny<CancellationToken>()), Times.Exactly(2));
            }

            // Case 7: Transient HttpRequestException retries up to ImmediateRetryCount and succeeds on final retry
            this.keycloakAdminClientMock.Invocations.Clear();

            this.keycloakAdminClientMock.SetupSequence(x => x.UpdateUserAsync(accountId, "alex@example.com", "Alex", "Rivera", It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestException("Temporary network error")).ThrowsAsync(new TimeoutException("Timed out")).ReturnsAsync(Result.Success);

            var exceptionRecoveredResult = await this.handler.HandleAsync(validMessage, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(exceptionRecoveredResult.IsError, Is.False);
                this.keycloakAdminClientMock.Verify(x => x.UpdateUserAsync(accountId, "alex@example.com", "Alex", "Rivera", It.IsAny<CancellationToken>()), Times.Exactly(3));
            }

            // Case 8: Exhausted retries return the transient error after 1 + ImmediateRetryCount attempts
            this.keycloakAdminClientMock.Invocations.Clear();
            this.keycloakAdminClientMock.Setup(x => x.UpdateUserAsync(accountId, "alex@example.com", "Alex", "Rivera", It.IsAny<CancellationToken>())).ReturnsAsync(transientError);

            var exhaustedResult = await this.handler.HandleAsync(validMessage, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(exhaustedResult.IsError, Is.True);
                Assert.That(exhaustedResult.FirstError.Code, Is.EqualTo(KeycloakAdminClient.SyncFailedErrorCode));
                this.keycloakAdminClientMock.Verify(x => x.UpdateUserAsync(accountId, "alex@example.com", "Alex", "Rivera", It.IsAny<CancellationToken>()), Times.Exactly(3));
            }

            // Case 9: Zero ImmediateRetryCount uses empty pipeline without retries
            var noRetryHandler = new KeycloakAccountSyncOutboxHandler(this.keycloakAdminClientMock.Object, Options.Create(new OutboxOptions { ImmediateRetryCount = 0 }), this.loggerMock.Object);
            this.keycloakAdminClientMock.Invocations.Clear();

            var noRetryResult = await noRetryHandler.HandleAsync(validMessage, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(noRetryResult.IsError, Is.True);
                this.keycloakAdminClientMock.Verify(x => x.UpdateUserAsync(accountId, "alex@example.com", "Alex", "Rivera", It.IsAny<CancellationToken>()), Times.Once());
            }
        }
    }
}
