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
        /// <summary>
        /// The permission service under test.
        /// </summary>
        private AccountPermissionService permissionService;

        /// <summary>
        /// The test account identifier.
        /// </summary>
        private Guid userId;

        /// <summary>
        /// A second test account identifier.
        /// </summary>
        private Guid otherUserId;

        /// <summary>
        /// The user context for the account owner.
        /// </summary>
        private UserContext ownerUserContext;

        /// <summary>
        /// The user context for another account user.
        /// </summary>
        private UserContext otherUserContext;

        /// <summary>
        /// The user context for an installation administrator.
        /// </summary>
        private UserContext adminUserContext;

        /// <summary>
        /// The user context for an unauthenticated user.
        /// </summary>
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
            var nullExistingResult = await this.permissionService.IsAllowedToUpdate(this.ownerUserContext, null, updatedAccount);
            var nullUpdatedResult = await this.permissionService.IsAllowedToUpdate(this.ownerUserContext, existingAccount, null);
            var ownerResult = await this.permissionService.IsAllowedToUpdate(this.ownerUserContext, existingAccount, updatedAccount);
            var nonOwnerResult = await this.permissionService.IsAllowedToUpdate(this.otherUserContext, existingAccount, updatedAccount);
            var adminResult = await this.permissionService.IsAllowedToUpdate(this.adminUserContext, otherAccount, updatedAccount);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(unauthenticatedResult.IsFailed, Is.True);
                Assert.That(nullExistingResult.IsFailed, Is.True);
                Assert.That(nullUpdatedResult.IsFailed, Is.True);
                Assert.That(ownerResult.IsSuccess, Is.True);
                Assert.That(nonOwnerResult.IsFailed, Is.True);
                Assert.That(adminResult.IsSuccess, Is.True);
            }
        }
    }
}
