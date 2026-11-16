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
        public void VerifyCanManageVersions()
        {
            var ownerAccountId = Guid.NewGuid();
            var packageOwnerId = Guid.NewGuid();
            var maintainerId = Guid.NewGuid();
            var outsiderId = Guid.NewGuid();

            var package = new Package
            {
                Owner = ownerAccountId,
                PackageOwner = [packageOwnerId],
                PackageMaintainer = [maintainerId]
            };

            var scopeOwnerContext = new UserContext { AccountId = ownerAccountId, CurrentRoles = [RoleKind.Account] };
            var packageOwnerContext = new UserContext { AccountId = packageOwnerId, CurrentRoles = [RoleKind.Account] };
            var maintainerContext = new UserContext { AccountId = maintainerId, CurrentRoles = [RoleKind.Account] };
            var outsiderAdminContext = new UserContext { AccountId = outsiderId, CurrentRoles = [RoleKind.Account, RoleKind.InstallationAdministrator] };
            var memberWithoutPermissionContext = new UserContext { AccountId = maintainerId, CurrentRoles = [RoleKind.Anonymous] };
            var unauthenticatedContext = new UserContext { AccountId = null, CurrentRoles = [RoleKind.Account] };

            using (Assert.EnterMultipleScope())
            {
                Assert.That(((IPackage)null!).CanManageVersions(scopeOwnerContext), Is.False);
                Assert.That(package.CanManageVersions(null!), Is.False);
                Assert.That(package.CanManageVersions(unauthenticatedContext), Is.False);
                Assert.That(package.CanManageVersions(scopeOwnerContext), Is.True);
                Assert.That(package.CanManageVersions(packageOwnerContext), Is.True);
                Assert.That(package.CanManageVersions(maintainerContext), Is.True);
                Assert.That(package.CanManageVersions(outsiderAdminContext), Is.False);
                Assert.That(package.CanManageVersions(memberWithoutPermissionContext), Is.False);
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
    }
}
