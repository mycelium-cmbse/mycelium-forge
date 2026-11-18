// ------------------------------------------------------------------------------------------------
// <copyright file="KeycloakAccountSyncHandlerTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using ErrorOr;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Services;

    [TestFixture]
    public class KeycloakAccountSyncHandlerTestFixture
    {
        private Mock<IKeycloakAdminClient> keycloakAdminClientMock;
        private KeycloakAccountSyncHandler handler;

        [SetUp]
        public void SetUp()
        {
            this.keycloakAdminClientMock = new Mock<IKeycloakAdminClient>();
            this.handler = new KeycloakAccountSyncHandler(this.keycloakAdminClientMock.Object);
        }

        [Test]
        public async Task VerifySyncAccountAsync()
        {
            var existing = new Account { Id = Guid.NewGuid(), Name = "Old Name", Email = "old@example.com" };

            // Case 1: Multi-word name splits into firstName and lastName and delegates to UpdateUserAsync
            var updatedMultiWord = new Account { Id = existing.Id, Name = "Alex Rivera", Email = "alex@example.com" };
            this.keycloakAdminClientMock.Setup(x => x.UpdateUserAsync(updatedMultiWord.Id, "alex@example.com", "Alex", "Rivera", CancellationToken.None)).ReturnsAsync(Result.Success);

            var multiWordResult = await this.handler.SyncAccountAsync(existing, updatedMultiWord, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(multiWordResult.IsError, Is.False);
                this.keycloakAdminClientMock.Verify(x => x.UpdateUserAsync(updatedMultiWord.Id, "alex@example.com", "Alex", "Rivera", CancellationToken.None), Times.Once);
            }

            // Case 3: Single-word name passes null lastName
            var updatedSingleWord = new Account { Id = existing.Id, Name = "Alex", Email = "alex@example.com" };
            this.keycloakAdminClientMock.Setup(x => x.UpdateUserAsync(updatedSingleWord.Id, "alex@example.com", "Alex", null, CancellationToken.None)).ReturnsAsync(Result.Success);

            var singleWordResult = await this.handler.SyncAccountAsync(existing, updatedSingleWord, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(singleWordResult.IsError, Is.False);
                this.keycloakAdminClientMock.Verify(x => x.UpdateUserAsync(updatedSingleWord.Id, "alex@example.com", "Alex", null, CancellationToken.None), Times.Once);
            }

            // Case 4: Error from UpdateUserAsync propagates
            this.keycloakAdminClientMock.Setup(x => x.UpdateUserAsync(updatedMultiWord.Id, "alex@example.com", "Alex", "Rivera", CancellationToken.None)).ReturnsAsync(Error.Failure("Keycloak.SyncFailed", "Keycloak unreachable"));

            var errorResult = await this.handler.SyncAccountAsync(existing, updatedMultiWord, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(errorResult.IsError, Is.True);
                Assert.That(errorResult.FirstError.Code, Is.EqualTo("Keycloak.SyncFailed"));
            }
        }
    }
}
