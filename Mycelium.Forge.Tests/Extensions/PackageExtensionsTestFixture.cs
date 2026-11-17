// ------------------------------------------------------------------------------------------------
// <copyright file="PackageExtensionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Extensions
{
    using System;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extensions;

    [TestFixture]
    public class PackageExtensionsTestFixture
    {
        [Test]
        public void VerifyCanManageTeam()
        {
            var scopeOwnerContext = new UserContext { AccountId = Guid.NewGuid(), CurrentRoles = [RoleKind.Account] };
            var packageOwnerContext = new UserContext { AccountId = Guid.NewGuid(), CurrentRoles = [RoleKind.Account] };
            var maintainerContext = new UserContext { AccountId = Guid.NewGuid(), CurrentRoles = [RoleKind.Account] };
            var organizationAdminContext = new UserContext { AccountId = Guid.NewGuid(), CurrentRoles = [RoleKind.Account] };
            var organizationMemberContext = new UserContext { AccountId = Guid.NewGuid(), CurrentRoles = [RoleKind.Account] };
            var outsiderAdminContext = new UserContext { AccountId = Guid.NewGuid(), CurrentRoles = [RoleKind.Account, RoleKind.InstallationAdministrator] };
            var unauthenticatedContext = new UserContext { AccountId = null, CurrentRoles = [RoleKind.Anonymous] };

            var organization = new Organization
            {
                Id = Guid.NewGuid(),
                Administrator = [organizationAdminContext.AccountId.Value],
                Member = [organizationAdminContext.AccountId.Value, organizationMemberContext.AccountId.Value]
            };

            var otherOrganization = new Organization
            {
                Id = Guid.NewGuid(),
                Administrator = [organizationAdminContext.AccountId.Value]
            };

            var package = new Package
            {
                Owner = scopeOwnerContext.AccountId.Value,
                PackageOwner = [packageOwnerContext.AccountId.Value],
                PackageMaintainer = [maintainerContext.AccountId.Value]
            };

            var orgPackage = new Package
            {
                Owner = organization.Id,
                PackageOwner = [packageOwnerContext.AccountId.Value],
                PackageMaintainer = [maintainerContext.AccountId.Value]
            };

            using (Assert.EnterMultipleScope())
            {
                Assert.That(((IPackage)null!).CanManageTeam(scopeOwnerContext), Is.False);
                Assert.That(package.CanManageTeam(null!), Is.False);
                Assert.That(package.CanManageTeam(unauthenticatedContext), Is.False);
                Assert.That(package.CanManageTeam(scopeOwnerContext), Is.False);
                Assert.That(package.CanManageTeam(packageOwnerContext), Is.True);
                Assert.That(package.CanManageTeam(maintainerContext), Is.False);
                Assert.That(package.CanManageTeam(outsiderAdminContext), Is.True);
                Assert.That(orgPackage.CanManageTeam(organizationAdminContext, organization), Is.True);
                Assert.That(orgPackage.CanManageTeam(organizationAdminContext), Is.False);
                Assert.That(orgPackage.CanManageTeam(organizationAdminContext, otherOrganization), Is.False);
                Assert.That(orgPackage.CanManageTeam(organizationMemberContext, organization), Is.False);
            }
        }

        [Test]
        public void VerifyCanManageVersions()
        {
            var scopeOwnerContext = new UserContext { AccountId = Guid.NewGuid(), CurrentRoles = [RoleKind.Account] };
            var packageOwnerContext = new UserContext { AccountId = Guid.NewGuid(), CurrentRoles = [RoleKind.Account] };
            var maintainerContext = new UserContext { AccountId = Guid.NewGuid(), CurrentRoles = [RoleKind.Account] };
            var organizationAdminContext = new UserContext { AccountId = Guid.NewGuid(), CurrentRoles = [RoleKind.Account] };
            var organizationMemberContext = new UserContext { AccountId = Guid.NewGuid(), CurrentRoles = [RoleKind.Account] };
            var organizationAdminWithoutPermissionContext = new UserContext { AccountId = organizationAdminContext.AccountId, CurrentRoles = [RoleKind.Anonymous] };
            var outsiderAdminContext = new UserContext { AccountId = Guid.NewGuid(), CurrentRoles = [RoleKind.Account, RoleKind.InstallationAdministrator] };
            var memberWithoutPermissionContext = new UserContext { AccountId = maintainerContext.AccountId, CurrentRoles = [RoleKind.Anonymous] };
            var unauthenticatedContext = new UserContext { AccountId = null, CurrentRoles = [RoleKind.Account] };

            var organization = new Organization
            {
                Id = Guid.NewGuid(),
                Administrator = [organizationAdminContext.AccountId.Value],
                Member = [organizationAdminContext.AccountId.Value, organizationMemberContext.AccountId.Value]
            };

            var otherOrganization = new Organization
            {
                Id = Guid.NewGuid(),
                Administrator = [organizationAdminContext.AccountId.Value]
            };

            var package = new Package
            {
                Owner = scopeOwnerContext.AccountId.Value,
                PackageOwner = [packageOwnerContext.AccountId.Value],
                PackageMaintainer = [maintainerContext.AccountId.Value]
            };

            var orgPackage = new Package
            {
                Owner = organization.Id,
                PackageOwner = [packageOwnerContext.AccountId.Value],
                PackageMaintainer = [maintainerContext.AccountId.Value]
            };

            using (Assert.EnterMultipleScope())
            {
                Assert.That(((IPackage)null!).CanManageVersions(scopeOwnerContext), Is.False);
                Assert.That(package.CanManageVersions(null!), Is.False);
                Assert.That(package.CanManageVersions(unauthenticatedContext), Is.False);
                Assert.That(package.CanManageVersions(scopeOwnerContext), Is.False);
                Assert.That(package.CanManageVersions(packageOwnerContext), Is.True);
                Assert.That(package.CanManageVersions(maintainerContext), Is.True);
                Assert.That(package.CanManageVersions(outsiderAdminContext), Is.False);
                Assert.That(package.CanManageVersions(memberWithoutPermissionContext), Is.False);
                Assert.That(orgPackage.CanManageVersions(organizationAdminContext, organization), Is.True);
                Assert.That(orgPackage.CanManageVersions(organizationAdminContext), Is.False);
                Assert.That(orgPackage.CanManageVersions(organizationMemberContext, organization), Is.False);
                Assert.That(orgPackage.CanManageVersions(organizationAdminContext, otherOrganization), Is.False);
                Assert.That(orgPackage.CanManageVersions(organizationAdminWithoutPermissionContext, organization), Is.False);
            }
        }

        [Test]
        public void VerifyGetFullName()
        {
            var package = new Package
            {
                Name = "ECSS-MM-PWR"
            };

            var organization = new Organization
            {
                ShortName = "starion"
            };

            var withOrg = package.GetFullName(organization);
            var withString = package.GetFullName("@starion");
            var withNullOrg = package.GetFullName((IOrganization)null!);
            var withNullString = package.GetFullName((string)null!);
            var nullPackage = ((IPackage)null!).GetFullName(organization);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(withOrg, Is.EqualTo("@starion/ECSS-MM-PWR"));
                Assert.That(withString, Is.EqualTo("@starion/ECSS-MM-PWR"));
                Assert.That(withNullOrg, Is.EqualTo(string.Empty));
                Assert.That(withNullString, Is.EqualTo(string.Empty));
                Assert.That(nullPackage, Is.EqualTo(string.Empty));
            }
        }

        [Test]
        public void VerifyIsOrganizationAdministrator()
        {
            var adminId = Guid.NewGuid();
            var memberId = Guid.NewGuid();

            var organization = new Organization { Id = Guid.NewGuid(), Administrator = [adminId], Member = [adminId, memberId] };
            var otherOrganization = new Organization { Id = Guid.NewGuid(), Administrator = [adminId] };
            var package = new Package { Owner = organization.Id };
            var accountScope = new Account { Id = organization.Id };

            using (Assert.EnterMultipleScope())
            {
                Assert.That(((IPackage)null!).IsOrganizationAdministrator(adminId, organization), Is.False);
                Assert.That(package.IsOrganizationAdministrator(adminId, null!), Is.False);
                Assert.That(package.IsOrganizationAdministrator(adminId, accountScope), Is.False);
                Assert.That(package.IsOrganizationAdministrator(adminId, otherOrganization), Is.False);
                Assert.That(package.IsOrganizationAdministrator(memberId, organization), Is.False);
                Assert.That(package.IsOrganizationAdministrator(adminId, organization), Is.True);
            }
        }
    }
}
