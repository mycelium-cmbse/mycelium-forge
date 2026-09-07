// ------------------------------------------------------------------------------------------------
// <copyright file="APIKeyPermissionServiceTestFixture.cs" company="Starion Group S.A.">
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
    /// Test fixture for <see cref="APIKeyPermissionService" />.
    /// </summary>
    [TestFixture]
    public class APIKeyPermissionServiceTestFixture
    {
        private APIKeyPermissionService permissionService;
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
            this.permissionService = new APIKeyPermissionService();
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
        /// Verifies the <see cref="APIKeyPermissionService.IsAllowedToDelete" /> method.
        /// </summary>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        [Test]
        public async Task VerifyIsAllowedToDelete()
        {
            var apiKey = new APIKey
            {
                Id = Guid.NewGuid(),
                Owner = this.userId,
                Name = "CI Token"
            };

            var unauthResult = await this.permissionService.IsAllowedToDelete(this.anonymousUserContext, apiKey);
            var ownerResult = await this.permissionService.IsAllowedToDelete(this.ownerUserContext, apiKey);
            var otherResult = await this.permissionService.IsAllowedToDelete(this.otherUserContext, apiKey);
            var adminResult = await this.permissionService.IsAllowedToDelete(this.adminUserContext, apiKey);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(unauthResult.IsFailed, Is.True);
                Assert.That(ownerResult.IsSuccess, Is.True);
                Assert.That(otherResult.IsFailed, Is.True);
                Assert.That(adminResult.IsSuccess, Is.True);
            }
        }
    }
}
