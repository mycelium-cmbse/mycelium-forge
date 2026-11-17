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

            var accountContext = new UserContext
            {
                AccountId = Guid.NewGuid(),
                Username = "account-user",
                CurrentRoles = [RoleKind.Account]
            };

            var anonymousContext = UserContext.CreateAnonymous();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(PermissionGuard.HasRole(installationAdministratorContext, RoleKind.InstallationAdministrator), Is.True);
                Assert.That(PermissionGuard.HasRole(installationAdministratorContext, RoleKind.Account), Is.True);
                Assert.That(PermissionGuard.HasRole(accountContext, RoleKind.InstallationAdministrator), Is.False);
                Assert.That(PermissionGuard.HasRole(accountContext, RoleKind.Account), Is.True);
                Assert.That(PermissionGuard.HasRole(anonymousContext, RoleKind.InstallationAdministrator), Is.False);
                Assert.That(PermissionGuard.HasRole(null, RoleKind.InstallationAdministrator), Is.False);
            }
        }

        [Test]
        public void VerifyHasPermission()
        {
            var installationAdministratorContext = new UserContext { AccountId = Guid.NewGuid(), Username = "admin", CurrentRoles = [RoleKind.Account, RoleKind.InstallationAdministrator] };
            var accountContext = new UserContext { AccountId = Guid.NewGuid(), Username = "account-user", CurrentRoles = [RoleKind.Account] };
            var operatorContext = new UserContext { AccountId = Guid.NewGuid(), Username = "operator-user", CurrentRoles = [RoleKind.PlatformOperator] };
            var anonymousContext = UserContext.CreateAnonymous();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(PermissionGuard.HasPermission(installationAdministratorContext, PermissionKind.PublishPackage), Is.True);
                Assert.That(PermissionGuard.HasPermission(accountContext, PermissionKind.PublishPackage), Is.True);
                Assert.That(PermissionGuard.HasPermission(operatorContext, PermissionKind.PublishPackage), Is.False);
                Assert.That(PermissionGuard.HasPermission(anonymousContext, PermissionKind.PublishPackage), Is.False);
                Assert.That(PermissionGuard.HasPermission(null, PermissionKind.PublishPackage), Is.False);
            }
        }
    }
}
