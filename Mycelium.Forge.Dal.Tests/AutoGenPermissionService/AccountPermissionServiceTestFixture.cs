// ------------------------------------------------------------------------------------------------
// <copyright file="AccountPermissionServiceTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.Tests.AutoGenPermissionService
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.AutoGenPermissionService;

    using NUnit.Framework;

    /// <summary>
    /// Test fixture for <see cref="AccountPermissionService" />.
    /// </summary>
    [TestFixture]
    public class AccountPermissionServiceTestFixture
    {
        private AccountPermissionService permissionService;
        private Guid userId;
        private Guid otherUserId;
        private UserContext ownerUserContext;
        private UserContext otherUserContext;
        private UserContext adminUserContext;
        private UserContext anonymousUserContext;

        /// <summary>
        /// Sets up mock dependencies and test context before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.permissionService = new AccountPermissionService();
            this.userId = Guid.NewGuid();
            this.otherUserId = Guid.NewGuid();

            this.ownerUserContext = new UserContext
            {
                AccountId = this.userId,
                Username = "ownerUser",
                CurrentRoles = [RoleKind.Account]
            };

            this.otherUserContext = new UserContext
            {
                AccountId = this.otherUserId,
                Username = "otherUser",
                CurrentRoles = [RoleKind.Account]
            };

            this.adminUserContext = new UserContext
            {
                AccountId = this.userId,
                Username = "adminUser",
                CurrentRoles = [RoleKind.Account, RoleKind.InstallationAdministrator]
            };

            this.anonymousUserContext = new UserContext
            {
                AccountId = null,
                Username = string.Empty,
                CurrentRoles = [RoleKind.Anonymous]
            };
        }

        /// <summary>
        /// Verifies the <see cref="AccountPermissionService.IsAllowedToUpdate" /> method.
        /// </summary>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        [Test]
        public async Task VerifyIsAllowedToUpdate()
        {
            var existingAccount = new Account
            {
                Id = this.userId,
                Name = "Alice",
                Email = "alice@example.com"
            };

            var updatedAccount = new Account
            {
                Id = this.userId,
                Name = "Alice Updated",
                Email = "alice@example.com"
            };

            var otherAccount = new Account
            {
                Id = this.otherUserId,
                Name = "Bob",
                Email = "bob@example.com"
            };

            var unauthenticatedResult = await this.permissionService.IsAllowedToUpdate(this.anonymousUserContext, existingAccount, updatedAccount);
            var ownerResult = await this.permissionService.IsAllowedToUpdate(this.ownerUserContext, existingAccount, updatedAccount);
            var nonOwnerResult = await this.permissionService.IsAllowedToUpdate(this.otherUserContext, existingAccount, updatedAccount);
            var adminResult = await this.permissionService.IsAllowedToUpdate(this.adminUserContext, otherAccount, updatedAccount);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(unauthenticatedResult.IsError, Is.True);
                Assert.That(ownerResult.IsError, Is.False);
                Assert.That(nonOwnerResult.IsError, Is.True);
                Assert.That(adminResult.IsError, Is.False);
            }
        }
    }
}
