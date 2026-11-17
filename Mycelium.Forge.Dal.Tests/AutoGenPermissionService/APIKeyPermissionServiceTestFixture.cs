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
    using ErrorOr;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.AutoGenPermissionService;

    using Npgsql;

    using NUnit.Framework;

    [TestFixture]
    public class APIKeyPermissionServiceTestFixture
    {
        private APIKeyPermissionService permissionService;
        private NpgsqlTransaction transaction;
        private Guid userId;
        private Guid otherUserId;
        private UserContext ownerUserContext;
        private UserContext otherUserContext;
        private UserContext adminUserContext;
        private UserContext anonymousUserContext;

        [SetUp]
        public void SetUp()
        {
            this.transaction = null;
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

        [Test]
        public async Task VerifyIsAllowedToCreate()
        {
            var ownApiKey = new APIKey { Id = Guid.NewGuid(), Owner = this.userId, Name = "Own Token" };
            var otherApiKey = new APIKey { Id = Guid.NewGuid(), Owner = this.otherUserId, Name = "Other Token" };
            var platformOperatorContext = new UserContext { AccountId = this.userId, Username = "operator", CurrentRoles = [RoleKind.PlatformOperator] };
            var nonOwnerAdminContext = new UserContext { AccountId = Guid.NewGuid(), Username = "nonOwnerAdmin", CurrentRoles = [RoleKind.Account, RoleKind.InstallationAdministrator] };

            var unauthResult = await this.permissionService.IsAllowedToCreate(this.anonymousUserContext, ownApiKey, this.transaction);
            var operatorResult = await this.permissionService.IsAllowedToCreate(platformOperatorContext, ownApiKey, this.transaction);
            var ownerResult = await this.permissionService.IsAllowedToCreate(this.ownerUserContext, ownApiKey, this.transaction);
            var forOtherResult = await this.permissionService.IsAllowedToCreate(this.ownerUserContext, otherApiKey, this.transaction);
            var adminForOtherResult = await this.permissionService.IsAllowedToCreate(nonOwnerAdminContext, otherApiKey, this.transaction);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(unauthResult.IsError, Is.True);
                Assert.That(unauthResult.FirstError.Type, Is.EqualTo(ErrorType.Unauthorized));
                Assert.That(operatorResult.IsError, Is.True);
                Assert.That(operatorResult.FirstError.Type, Is.EqualTo(ErrorType.Forbidden));
                Assert.That(ownerResult.IsError, Is.False);
                Assert.That(forOtherResult.IsError, Is.True);
                Assert.That(forOtherResult.FirstError.Type, Is.EqualTo(ErrorType.Forbidden));
                Assert.That(adminForOtherResult.IsError, Is.False);
            }
        }

        [Test]
        public async Task VerifyIsAllowedToDelete()
        {
            var apiKey = new APIKey
            {
                Id = Guid.NewGuid(),
                Owner = this.userId,
                Name = "CI Token"
            };

            var unauthResult = await this.permissionService.IsAllowedToDelete(this.anonymousUserContext, apiKey, this.transaction);
            var ownerResult = await this.permissionService.IsAllowedToDelete(this.ownerUserContext, apiKey, this.transaction);
            var otherResult = await this.permissionService.IsAllowedToDelete(this.otherUserContext, apiKey, this.transaction);
            var adminResult = await this.permissionService.IsAllowedToDelete(this.adminUserContext, apiKey, this.transaction);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(unauthResult.IsError, Is.True);
                Assert.That(ownerResult.IsError, Is.False);
                Assert.That(otherResult.IsError, Is.True);
                Assert.That(adminResult.IsError, Is.False);
            }
        }

        [Test]
        public async Task VerifyIsAllowedToRead()
        {
            var apiKey = new APIKey { Id = Guid.NewGuid(), Owner = this.userId, Name = "CI Token" };
            var nonOwnerAdminContext = new UserContext { AccountId = Guid.NewGuid(), Username = "nonOwnerAdmin", CurrentRoles = [RoleKind.Account, RoleKind.InstallationAdministrator] };

            var unauthResult = await this.permissionService.IsAllowedToRead(this.anonymousUserContext, apiKey, this.transaction);
            var ownerResult = await this.permissionService.IsAllowedToRead(this.ownerUserContext, apiKey, this.transaction);
            var otherResult = await this.permissionService.IsAllowedToRead(this.otherUserContext, apiKey, this.transaction);
            var adminResult = await this.permissionService.IsAllowedToRead(nonOwnerAdminContext, apiKey, this.transaction);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(unauthResult.IsError, Is.True);
                Assert.That(ownerResult.IsError, Is.False);
                Assert.That(otherResult.IsError, Is.True);
                Assert.That(adminResult.IsError, Is.False);
            }
        }

        [Test]
        public async Task VerifyIsAllowedToUpdate()
        {
            var existingApiKey = new APIKey { Id = Guid.NewGuid(), Owner = this.userId, Name = "CI Token" };
            var updatedApiKey = new APIKey { Id = existingApiKey.Id, Owner = this.userId, Name = "Renamed Token" };
            var reassignedApiKey = new APIKey { Id = existingApiKey.Id, Owner = this.otherUserId, Name = "Reassigned Token" };
            var nonOwnerAdminContext = new UserContext { AccountId = Guid.NewGuid(), Username = "nonOwnerAdmin", CurrentRoles = [RoleKind.Account, RoleKind.InstallationAdministrator] };

            var unauthResult = await this.permissionService.IsAllowedToUpdate(this.anonymousUserContext, existingApiKey, updatedApiKey, this.transaction);
            var ownerResult = await this.permissionService.IsAllowedToUpdate(this.ownerUserContext, existingApiKey, updatedApiKey, this.transaction);
            var ownerReassignResult = await this.permissionService.IsAllowedToUpdate(this.ownerUserContext, existingApiKey, reassignedApiKey, this.transaction);
            var otherResult = await this.permissionService.IsAllowedToUpdate(this.otherUserContext, existingApiKey, updatedApiKey, this.transaction);
            var adminResult = await this.permissionService.IsAllowedToUpdate(nonOwnerAdminContext, existingApiKey, updatedApiKey, this.transaction);
            var adminReassignResult = await this.permissionService.IsAllowedToUpdate(nonOwnerAdminContext, existingApiKey, reassignedApiKey, this.transaction);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(unauthResult.IsError, Is.True);
                Assert.That(ownerResult.IsError, Is.False);
                Assert.That(ownerReassignResult.IsError, Is.True);
                Assert.That(ownerReassignResult.FirstError.Type, Is.EqualTo(ErrorType.Forbidden));
                Assert.That(otherResult.IsError, Is.True);
                Assert.That(adminResult.IsError, Is.False);
                Assert.That(adminReassignResult.IsError, Is.False);
            }
        }
    }
}
