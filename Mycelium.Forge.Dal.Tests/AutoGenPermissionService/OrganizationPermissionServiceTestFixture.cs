// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationPermissionServiceTestFixture.cs" company="Starion Group S.A.">
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
    /// Test fixture for <see cref="OrganizationPermissionService" />.
    /// </summary>
    [TestFixture]
    public class OrganizationPermissionServiceTestFixture
    {
        private OrganizationPermissionService permissionService;
        private Guid userId;
        private Guid otherUserId;
        private UserContext orgAdminUserContext;
        private UserContext orgMemberUserContext;
        private UserContext accountUserContext;
        private UserContext installationAdminUserContext;
        private UserContext anonymousUserContext;

        /// <summary>
        /// Sets up the test fixture before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.permissionService = new OrganizationPermissionService();
            this.userId = Guid.NewGuid();
            this.otherUserId = Guid.NewGuid();

            this.orgAdminUserContext = new UserContext
            {
                AccountId = this.userId,
                Username = "orgAdmin",
                CurrentRoles = [RoleKind.Account, RoleKind.OrganizationAdministrator]
            };

            this.orgMemberUserContext = new UserContext
            {
                AccountId = this.userId,
                Username = "orgMember",
                CurrentRoles = [RoleKind.Account, RoleKind.OrganizationMember]
            };

            this.accountUserContext = new UserContext
            {
                AccountId = this.userId,
                Username = "regularUser",
                CurrentRoles = [RoleKind.Account]
            };

            this.installationAdminUserContext = new UserContext
            {
                AccountId = this.userId,
                Username = "superAdmin",
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
        /// Verifies the <see cref="OrganizationPermissionService.IsAllowedToCreate" /> method.
        /// </summary>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        [Test]
        public async Task VerifyIsAllowedToCreate()
        {
            var organization = new Organization
            {
                Id = Guid.NewGuid(),
                Name = "StarionOrg",
                ShortName = "starion",
                Administrator = [this.userId],
                Member = [this.userId]
            };

            var accountResult = await this.permissionService.IsAllowedToCreate(this.accountUserContext, organization);
            var adminResult = await this.permissionService.IsAllowedToCreate(this.orgAdminUserContext, organization);
            var anonymousResult = await this.permissionService.IsAllowedToCreate(this.anonymousUserContext, organization);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(accountResult.IsSuccess, Is.True);
                Assert.That(adminResult.IsSuccess, Is.True);
                Assert.That(anonymousResult.IsFailed, Is.True);
            }
        }

        /// <summary>
        /// Verifies the IsAllowedToDelete method.
        /// </summary>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        [Test]
        public async Task VerifyIsAllowedToDelete()
        {
            var orgId = Guid.NewGuid();

            var superAdminResult = await this.permissionService.IsAllowedToDelete(this.installationAdminUserContext, orgId);
            var orgAdminResult = await this.permissionService.IsAllowedToDelete(this.orgAdminUserContext, orgId);
            var accountResult = await this.permissionService.IsAllowedToDelete(this.accountUserContext, orgId);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(superAdminResult.IsSuccess, Is.True);
                Assert.That(orgAdminResult.IsFailed, Is.True);
                Assert.That(accountResult.IsFailed, Is.True);
            }
        }

        /// <summary>
        /// Verifies the <see cref="OrganizationPermissionService.IsAllowedToRead" /> method.
        /// </summary>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        [Test]
        public async Task VerifyIsAllowedToRead()
        {
            var organization = new Organization
            {
                Id = Guid.NewGuid(),
                Name = "StarionOrg",
                ShortName = "starion",
                Administrator = [this.userId],
                Member = [this.userId]
            };

            var nonMemberOrg = new Organization
            {
                Id = Guid.NewGuid(),
                Name = "OtherOrg",
                ShortName = "other",
                Administrator = [this.otherUserId],
                Member = [this.otherUserId]
            };

            var adminResult = await this.permissionService.IsAllowedToRead(this.orgAdminUserContext, organization);
            var memberResult = await this.permissionService.IsAllowedToRead(this.orgMemberUserContext, organization);

            var nonMemberAccountResult = await this.permissionService.IsAllowedToRead(this.accountUserContext, nonMemberOrg);
            var nonMemberSuperAdminResult = await this.permissionService.IsAllowedToRead(this.installationAdminUserContext, nonMemberOrg);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(adminResult.IsSuccess, Is.True);
                Assert.That(memberResult.IsSuccess, Is.True);
                Assert.That(nonMemberAccountResult.IsFailed, Is.True);
                Assert.That(nonMemberSuperAdminResult.IsSuccess, Is.True);
            }
        }

        /// <summary>
        /// Verifies the <see cref="OrganizationPermissionService.IsAllowedToUpdate" /> method.
        /// </summary>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        [Test]
        public async Task VerifyIsAllowedToUpdate()
        {
            var existingOrg = new Organization
            {
                Id = Guid.NewGuid(),
                Name = "StarionOrg",
                ShortName = "starion",
                Administrator = [this.userId],
                Member = [this.userId, this.otherUserId],
                DefaultPackageVisibility = VisibilityKind.PRIVATE
            };

            var updatedSettingsOrg = new Organization
            {
                Id = existingOrg.Id,
                Name = "UpdatedStarionOrg",
                ShortName = "starion",
                Administrator = [this.userId],
                Member = [this.userId, this.otherUserId],
                DefaultPackageVisibility = VisibilityKind.PRIVATE
            };

            var updatedAdminOrg = new Organization
            {
                Id = existingOrg.Id,
                Name = "StarionOrg",
                ShortName = "starion",
                Administrator = [this.otherUserId],
                Member = [this.userId, this.otherUserId],
                DefaultPackageVisibility = VisibilityKind.PRIVATE
            };

            var updatedVisibilityOrg = new Organization
            {
                Id = existingOrg.Id,
                Name = "StarionOrg",
                ShortName = "starion",
                Administrator = [this.userId],
                Member = [this.userId, this.otherUserId],
                DefaultPackageVisibility = VisibilityKind.INTERNAL
            };

            var settingsAdminResult = await this.permissionService.IsAllowedToUpdate(this.orgAdminUserContext, existingOrg, updatedSettingsOrg);
            var settingsMemberResult = await this.permissionService.IsAllowedToUpdate(this.orgMemberUserContext, existingOrg, updatedSettingsOrg);

            var adminTransferResult = await this.permissionService.IsAllowedToUpdate(this.orgAdminUserContext, existingOrg, updatedAdminOrg);
            var memberTransferResult = await this.permissionService.IsAllowedToUpdate(this.orgMemberUserContext, existingOrg, updatedAdminOrg);

            var visibilityAdminResult = await this.permissionService.IsAllowedToUpdate(this.orgAdminUserContext, existingOrg, updatedVisibilityOrg);
            var visibilityMemberResult = await this.permissionService.IsAllowedToUpdate(this.orgMemberUserContext, existingOrg, updatedVisibilityOrg);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(settingsAdminResult.IsSuccess, Is.True);
                Assert.That(settingsMemberResult.IsFailed, Is.True);
                Assert.That(adminTransferResult.IsSuccess, Is.True);
                Assert.That(memberTransferResult.IsFailed, Is.True);
                Assert.That(visibilityAdminResult.IsSuccess, Is.True);
                Assert.That(visibilityMemberResult.IsFailed, Is.True);
            }
        }
    }
}
