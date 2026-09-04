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
        private Guid thirdPartyUserId;
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
            this.thirdPartyUserId = Guid.NewGuid();

            this.orgAdminUserContext = new UserContext
            {
                AccountId = this.userId,
                Username = "orgAdmin",
                CurrentRoles = [RoleKind.Account, RoleKind.OrganizationAdministrator]
            };

            this.orgMemberUserContext = new UserContext
            {
                AccountId = this.otherUserId,
                Username = "orgMember",
                CurrentRoles = [RoleKind.Account, RoleKind.OrganizationMember]
            };

            this.accountUserContext = new UserContext
            {
                AccountId = this.thirdPartyUserId,
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
            var organization = new Organization
            {
                Id = Guid.NewGuid(),
                Administrator = [this.otherUserId],
                Member = [this.otherUserId]
            };

            var superAdminResult = await this.permissionService.IsAllowedToDelete(this.installationAdminUserContext, organization);
            var orgAdminResult = await this.permissionService.IsAllowedToDelete(this.orgAdminUserContext, organization);
            var accountResult = await this.permissionService.IsAllowedToDelete(this.accountUserContext, organization);

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
                Member = [this.userId, this.otherUserId]
            };

            var nonMemberOrg = new Organization
            {
                Id = Guid.NewGuid(),
                Name = "OtherOrg",
                ShortName = "other",
                Administrator = [this.otherUserId],
                Member = [this.otherUserId]
            };

            var publicOrg = new Organization
            {
                Id = Guid.NewGuid(),
                Name = "PublicOrg",
                ShortName = "public-org",
                DefaultPackageVisibility = VisibilityKind.PUBLIC,
                Administrator = [this.otherUserId],
                Member = [this.otherUserId]
            };

            var internalOrg = new Organization
            {
                Id = Guid.NewGuid(),
                Name = "InternalOrg",
                ShortName = "internal-org",
                DefaultPackageVisibility = VisibilityKind.INTERNAL,
                Administrator = [this.userId],
                Member = [this.userId, this.otherUserId]
            };

            var privateOrg = new Organization
            {
                Id = Guid.NewGuid(),
                Name = "PrivateOrg",
                ShortName = "private-org",
                DefaultPackageVisibility = VisibilityKind.PRIVATE,
                Administrator = [this.userId],
                Member = [this.userId, this.otherUserId]
            };

            var adminResult = await this.permissionService.IsAllowedToRead(this.orgAdminUserContext, organization);
            var memberResult = await this.permissionService.IsAllowedToRead(this.orgMemberUserContext, organization);

            var nonMemberAccountResult = await this.permissionService.IsAllowedToRead(this.accountUserContext, nonMemberOrg);
            var nonMemberSuperAdminResult = await this.permissionService.IsAllowedToRead(this.installationAdminUserContext, nonMemberOrg);

            var anonymousPublicResult = await this.permissionService.IsAllowedToRead(this.anonymousUserContext, publicOrg);
            var nonMemberPublicResult = await this.permissionService.IsAllowedToRead(this.accountUserContext, publicOrg);

            var internalAdminResult = await this.permissionService.IsAllowedToRead(this.orgAdminUserContext, internalOrg);
            var internalMemberResult = await this.permissionService.IsAllowedToRead(this.orgMemberUserContext, internalOrg);
            var internalNonMemberResult = await this.permissionService.IsAllowedToRead(this.accountUserContext, internalOrg);
            var internalSuperAdminResult = await this.permissionService.IsAllowedToRead(this.installationAdminUserContext, internalOrg);
            var internalAnonResult = await this.permissionService.IsAllowedToRead(this.anonymousUserContext, internalOrg);

            var privateAdminResult = await this.permissionService.IsAllowedToRead(this.orgAdminUserContext, privateOrg);
            var privateMemberResult = await this.permissionService.IsAllowedToRead(this.orgMemberUserContext, privateOrg);
            var privateNonMemberResult = await this.permissionService.IsAllowedToRead(this.accountUserContext, privateOrg);
            var privateSuperAdminResult = await this.permissionService.IsAllowedToRead(this.installationAdminUserContext, privateOrg);
            var privateAnonResult = await this.permissionService.IsAllowedToRead(this.anonymousUserContext, privateOrg);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(adminResult.IsSuccess, Is.True);
                Assert.That(memberResult.IsSuccess, Is.True);
                Assert.That(nonMemberAccountResult.IsFailed, Is.True);
                Assert.That(nonMemberSuperAdminResult.IsSuccess, Is.True);
                Assert.That(anonymousPublicResult.IsSuccess, Is.True);
                Assert.That(nonMemberPublicResult.IsSuccess, Is.True);
                Assert.That(internalAdminResult.IsSuccess, Is.True);
                Assert.That(internalMemberResult.IsSuccess, Is.True);
                Assert.That(internalNonMemberResult.IsFailed, Is.True);
                Assert.That(internalSuperAdminResult.IsSuccess, Is.True);
                Assert.That(internalAnonResult.IsFailed, Is.True);
                Assert.That(privateAdminResult.IsSuccess, Is.True);
                Assert.That(privateMemberResult.IsSuccess, Is.True);
                Assert.That(privateNonMemberResult.IsFailed, Is.True);
                Assert.That(privateSuperAdminResult.IsSuccess, Is.True);
                Assert.That(privateAnonResult.IsFailed, Is.True);
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
