// ------------------------------------------------------------------------------------------------
// <copyright file="KeycloakUserSeederTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using ErrorOr;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Data;
    using Mycelium.Forge.Services;

    [TestFixture]
    public class KeycloakUserSeederTestFixture
    {
        private Mock<IKeycloakAdminClient> keycloakAdminClientMock;
        private Mock<ILogger<KeycloakUserSeeder>> loggerMock;
        private IOptions<KeycloakOptions> options;
        private KeycloakUserSeeder seeder;

        [SetUp]
        public void SetUp()
        {
            this.keycloakAdminClientMock = new Mock<IKeycloakAdminClient>();
            this.loggerMock = new Mock<ILogger<KeycloakUserSeeder>>();

            this.options = Options.Create(new KeycloakOptions
            {
                BaseUrl = "http://localhost:8080/",
                Realm = "forge",
                AdminUsername = "admin",
                AdminPassword = "adminpassword",
                SeedUserPassword = "userpassword"
            });

            this.keycloakAdminClientMock.Setup(x => x.DeleteAllUsersAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);
            this.keycloakAdminClientMock.Setup(x => x.PartialImportUsersAsync(It.IsAny<IEnumerable<object>>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);
            this.keycloakAdminClientMock.Setup(x => x.AssignRealmRolesAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

            this.seeder = new KeycloakUserSeeder(this.keycloakAdminClientMock.Object, this.options, this.loggerMock.Object);
        }

        [Test]
        public async Task VerifySeedAsync()
        {
            // Case 1: Keycloak successfully deletes users, imports realm, and assigns roles (success)
            var seedSuccess = await this.seeder.SeedAsync(CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(seedSuccess, Is.True);
                this.keycloakAdminClientMock.Verify(x => x.DeleteAllUsersAsync(CancellationToken.None), Times.Once);
                this.keycloakAdminClientMock.Verify(x => x.PartialImportUsersAsync(It.Is<IEnumerable<object>>(users => users.Count() == SeedData.Accounts.Count), CancellationToken.None), Times.Once);
                this.keycloakAdminClientMock.Verify(x => x.AssignRealmRolesAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<string>>(), CancellationToken.None), Times.Exactly(SeedData.Accounts.Count));
            }

            // Case 2: Failure during DeleteAllUsersAsync (fail)
            this.keycloakAdminClientMock.Setup(x => x.DeleteAllUsersAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Error.Failure("Keycloak.SyncFailed", "Delete failed"));
            Assert.That(await this.seeder.SeedAsync(CancellationToken.None), Is.False);

            // Case 3: Failure during PartialImportUsersAsync (fail)
            this.keycloakAdminClientMock.Setup(x => x.DeleteAllUsersAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);
            this.keycloakAdminClientMock.Setup(x => x.PartialImportUsersAsync(It.IsAny<IEnumerable<object>>(), It.IsAny<CancellationToken>())).ReturnsAsync(Error.Failure("Keycloak.SyncFailed", "Import failed"));
            Assert.That(await this.seeder.SeedAsync(CancellationToken.None), Is.False);

            // Case 4: Failure during AssignRealmRolesAsync (fail)
            this.keycloakAdminClientMock.Setup(x => x.PartialImportUsersAsync(It.IsAny<IEnumerable<object>>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);
            this.keycloakAdminClientMock.Setup(x => x.AssignRealmRolesAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>())).ReturnsAsync(Error.Failure("Keycloak.SyncFailed", "Role assignment failed"));
            Assert.That(await this.seeder.SeedAsync(CancellationToken.None), Is.False);

            // Case 5: Exception thrown during seeding (fail)
            this.keycloakAdminClientMock.Setup(x => x.DeleteAllUsersAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestException("Connection refused"));
            Assert.That(await this.seeder.SeedAsync(CancellationToken.None), Is.False);
        }
    }
}
