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
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    using ErrorOr;

    using Microsoft.Extensions.Logging;

    using Moq;

    using Mycelium.Forge.Dal.Events;
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
            this.handler = new KeycloakAccountSyncOutboxHandler(this.keycloakAdminClientMock.Object, this.loggerMock.Object);
        }

        [Test]
        public async Task VerifyHandleAsync()
        {
            Assert.That(this.handler.EventType, Is.EqualTo(AccountProfileChanged.EventType));

            var accountId = Guid.NewGuid();
            var validPayload = JsonSerializer.Serialize(new AccountProfileChanged(accountId, "alex@example.com", "Alex", "Rivera"));
            var validMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = accountId, EventType = AccountProfileChanged.EventType, Payload = validPayload };

            // Case 1: Valid payload dispatches to KeycloakAdminClient.UpdateUserAsync
            this.keycloakAdminClientMock.Setup(x => x.UpdateUserAsync(accountId, "alex@example.com", "Alex", "Rivera", CancellationToken.None)).ReturnsAsync(Result.Success);

            var validResult = await this.handler.HandleAsync(validMessage, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validResult.IsError, Is.False);
                this.keycloakAdminClientMock.Verify(x => x.UpdateUserAsync(accountId, "alex@example.com", "Alex", "Rivera", CancellationToken.None), Times.Once());
            }

            // Case 2: Null firstName/lastName values pass through cleanly
            var blankNamePayload = JsonSerializer.Serialize(new AccountProfileChanged(accountId, "alex@example.com", null!, null!));
            var blankNameMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = accountId, EventType = AccountProfileChanged.EventType, Payload = blankNamePayload };
            this.keycloakAdminClientMock.Setup(x => x.UpdateUserAsync(accountId, "alex@example.com", null!, null!, CancellationToken.None)).ReturnsAsync(Result.Success);

            var blankNameResult = await this.handler.HandleAsync(blankNameMessage, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(blankNameResult.IsError, Is.False);
                this.keycloakAdminClientMock.Verify(x => x.UpdateUserAsync(accountId, "alex@example.com", null!, null!, CancellationToken.None), Times.Once());
            }

            // Case 3: Invalid JSON payload returns validation error without calling KeycloakAdminClient
            var invalidJsonMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = accountId, EventType = AccountProfileChanged.EventType, Payload = "{not-valid-json" };

            var invalidJsonResult = await this.handler.HandleAsync(invalidJsonMessage, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(invalidJsonResult.IsError, Is.True);
                Assert.That(invalidJsonResult.FirstError.Type, Is.EqualTo(ErrorType.Validation));
                Assert.That(invalidJsonResult.FirstError.Code, Is.EqualTo("Outbox.InvalidPayload"));
            }

            // Case 4: Null or empty AccountId payload returns validation error without calling KeycloakAdminClient
            var nullPayloadMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = accountId, EventType = AccountProfileChanged.EventType, Payload = "null" };
            var emptyAccountPayload = JsonSerializer.Serialize(new AccountProfileChanged(Guid.Empty, "alex@example.com", "Alex", "Rivera"));
            var emptyAccountMessage = new OutboxMessage { Id = Guid.NewGuid(), AggregateId = accountId, EventType = AccountProfileChanged.EventType, Payload = emptyAccountPayload };

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

            // Case 5: Downstream Keycloak errors propagate directly without in-process retries
            this.keycloakAdminClientMock.Invocations.Clear();
            var downstreamError = Error.Unauthorized(KeycloakAdminClient.SyncFailedErrorCode, "Unauthorized");
            this.keycloakAdminClientMock.Setup(x => x.UpdateUserAsync(accountId, "alex@example.com", "Alex", "Rivera", It.IsAny<CancellationToken>())).ReturnsAsync(downstreamError);

            var downstreamResult = await this.handler.HandleAsync(validMessage, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(downstreamResult.IsError, Is.True);
                Assert.That(downstreamResult.FirstError.Type, Is.EqualTo(ErrorType.Unauthorized));
                Assert.That(downstreamResult.FirstError.Code, Is.EqualTo(KeycloakAdminClient.SyncFailedErrorCode));
                this.keycloakAdminClientMock.Verify(x => x.UpdateUserAsync(accountId, "alex@example.com", "Alex", "Rivera", It.IsAny<CancellationToken>()), Times.Once());
            }
        }
    }
}
