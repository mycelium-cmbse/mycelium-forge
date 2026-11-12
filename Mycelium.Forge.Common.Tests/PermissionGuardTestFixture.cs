// ------------------------------------------------------------------------------------------------
// <copyright file="PermissionGuardTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.Tests
{
    using System;

    [TestFixture]
    public class PermissionGuardTestFixture
    {
        [Test]
        public void VerifyHasRole()
        {
            var installationAdministratorContext = new UserContext
            {
                AccountId = Guid.NewGuid(),
                Username = "admin",
                CurrentRoles = [RoleKind.Account, RoleKind.InstallationAdministrator]
            };

            var organizationAdministratorContext = new UserContext
            {
                AccountId = Guid.NewGuid(),
                Username = "organization-admin",
                CurrentRoles = [RoleKind.Account, RoleKind.OrganizationAdministrator]
            };

            var anonymousContext = UserContext.CreateAnonymous();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(PermissionGuard.HasRole(installationAdministratorContext, RoleKind.InstallationAdministrator), Is.True);
                Assert.That(PermissionGuard.HasRole(installationAdministratorContext, RoleKind.OrganizationAdministrator), Is.False);
                Assert.That(PermissionGuard.HasRole(organizationAdministratorContext, RoleKind.InstallationAdministrator), Is.False);
                Assert.That(PermissionGuard.HasRole(organizationAdministratorContext, RoleKind.OrganizationAdministrator), Is.True);
                Assert.That(PermissionGuard.HasRole(anonymousContext, RoleKind.InstallationAdministrator), Is.False);
                Assert.That(PermissionGuard.HasRole(null, RoleKind.InstallationAdministrator), Is.False);
            }
        }
    }
}
