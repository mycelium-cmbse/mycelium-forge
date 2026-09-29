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

    using Npgsql;

    using NUnit.Framework;

    /// <summary>
    /// Test fixture for <see cref="OrganizationPermissionService" />.
    /// </summary>
    [TestFixture]
    public class OrganizationPermissionServiceTestFixture
    {
        private OrganizationPermissionService permissionService;
        private NpgsqlTransaction transaction;
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
            this.transaction = null;
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

            var accountResult = await this.permissionService.IsAllowedToCreate(this.accountUserContext, organization, this.transaction);
            var adminResult = await this.permissionService.IsAllowedToCreate(this.orgAdminUserContext, organization, this.transaction);
            var anonymousResult = await this.permissionService.IsAllowedToCreate(this.anonymousUserContext, organization, this.transaction);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(accountResult.IsError, Is.False);
                Assert.That(adminResult.IsError, Is.False);
                Assert.That(anonymousResult.IsError, Is.True);
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

            var superAdminResult = await this.permissionService.IsAllowedToDelete(this.installationAdminUserContext, organization, this.transaction);
            var orgAdminResult = await this.permissionService.IsAllowedToDelete(this.orgAdminUserContext, organization, this.transaction);
            var accountResult = await this.permissionService.IsAllowedToDelete(this.accountUserContext, organization, this.transaction);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(superAdminResult.IsError, Is.False);
                Assert.That(orgAdminResult.IsError, Is.True);
                Assert.That(accountResult.IsError, Is.True);
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

            var adminResult = await this.permissionService.IsAllowedToRead(this.orgAdminUserContext, organization, this.transaction);
            var memberResult = await this.permissionService.IsAllowedToRead(this.orgMemberUserContext, organization, this.transaction);

            var nonMemberAccountResult = await this.permissionService.IsAllowedToRead(this.accountUserContext, nonMemberOrg, this.transaction);
            var nonMemberSuperAdminResult = await this.permissionService.IsAllowedToRead(this.installationAdminUserContext, nonMemberOrg, this.transaction);

            var anonymousPublicResult = await this.permissionService.IsAllowedToRead(this.anonymousUserContext, publicOrg, this.transaction);
            var nonMemberPublicResult = await this.permissionService.IsAllowedToRead(this.accountUserContext, publicOrg, this.transaction);

            var internalAdminResult = await this.permissionService.IsAllowedToRead(this.orgAdminUserContext, internalOrg, this.transaction);
            var internalMemberResult = await this.permissionService.IsAllowedToRead(this.orgMemberUserContext, internalOrg, this.transaction);
            var internalNonMemberResult = await this.permissionService.IsAllowedToRead(this.accountUserContext, internalOrg, this.transaction);
            var internalSuperAdminResult = await this.permissionService.IsAllowedToRead(this.installationAdminUserContext, internalOrg, this.transaction);
            var internalAnonResult = await this.permissionService.IsAllowedToRead(this.anonymousUserContext, internalOrg, this.transaction);

            var privateAdminResult = await this.permissionService.IsAllowedToRead(this.orgAdminUserContext, privateOrg, this.transaction);
            var privateMemberResult = await this.permissionService.IsAllowedToRead(this.orgMemberUserContext, privateOrg, this.transaction);
            var privateNonMemberResult = await this.permissionService.IsAllowedToRead(this.accountUserContext, privateOrg, this.transaction);
            var privateSuperAdminResult = await this.permissionService.IsAllowedToRead(this.installationAdminUserContext, privateOrg, this.transaction);
            var privateAnonResult = await this.permissionService.IsAllowedToRead(this.anonymousUserContext, privateOrg, this.transaction);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(adminResult.IsSuccess, Is.True);
                Assert.That(memberResult.IsSuccess, Is.True);
                Assert.That(nonMemberAccountResult.IsSuccess, Is.True);
                Assert.That(nonMemberSuperAdminResult.IsSuccess, Is.True);
                Assert.That(anonymousPublicResult.IsSuccess, Is.True);
                Assert.That(nonMemberPublicResult.IsSuccess, Is.True);
                Assert.That(internalAdminResult.IsSuccess, Is.True);
                Assert.That(internalMemberResult.IsSuccess, Is.True);
                Assert.That(internalNonMemberResult.IsSuccess, Is.True);
                Assert.That(internalSuperAdminResult.IsSuccess, Is.True);
                Assert.That(internalAnonResult.IsSuccess, Is.True);
                Assert.That(privateAdminResult.IsSuccess, Is.True);
                Assert.That(privateMemberResult.IsSuccess, Is.True);
                Assert.That(privateNonMemberResult.IsSuccess, Is.True);
                Assert.That(privateSuperAdminResult.IsSuccess, Is.True);
                Assert.That(privateAnonResult.IsSuccess, Is.True);
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

            var settingsAdminResult = await this.permissionService.IsAllowedToUpdate(this.orgAdminUserContext, existingOrg, updatedSettingsOrg, this.transaction);
            var settingsMemberResult = await this.permissionService.IsAllowedToUpdate(this.orgMemberUserContext, existingOrg, updatedSettingsOrg, this.transaction);

            var adminTransferResult = await this.permissionService.IsAllowedToUpdate(this.orgAdminUserContext, existingOrg, updatedAdminOrg, this.transaction);
            var memberTransferResult = await this.permissionService.IsAllowedToUpdate(this.orgMemberUserContext, existingOrg, updatedAdminOrg, this.transaction);

            var visibilityAdminResult = await this.permissionService.IsAllowedToUpdate(this.orgAdminUserContext, existingOrg, updatedVisibilityOrg, this.transaction);
            var visibilityMemberResult = await this.permissionService.IsAllowedToUpdate(this.orgMemberUserContext, existingOrg, updatedVisibilityOrg, this.transaction);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(settingsAdminResult.IsError, Is.False);
                Assert.That(settingsMemberResult.IsError, Is.True);
                Assert.That(adminTransferResult.IsError, Is.False);
                Assert.That(memberTransferResult.IsError, Is.True);
                Assert.That(visibilityAdminResult.IsError, Is.False);
                Assert.That(visibilityMemberResult.IsError, Is.True);
            }
        }
    }
}
