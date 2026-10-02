// ------------------------------------------------------------------------------------------------
// <copyright file="UserContextTestFixture.cs" company="Starion Group S.A.">
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
    public class UserContextTestFixture
    {
        [Test]
        public void VerifyHasPermission()
        {
            var adminContext = new UserContext
            {
                AccountId = Guid.NewGuid(),
                Username = "admin",
                CurrentRoles = [RoleKind.InstallationAdministrator]
            };

            var regularContext = new UserContext
            {
                AccountId = Guid.NewGuid(),
                Username = "regular",
                CurrentRoles = [RoleKind.Account]
            };

            var anonymousContext = UserContext.CreateAnonymous();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(adminContext.HasPermission(PermissionKind.ManageOrganizations), Is.True);
                Assert.That(regularContext.HasPermission(PermissionKind.ManageOrganizations), Is.False);
                Assert.That(regularContext.HasPermission(PermissionKind.CreateOrganization), Is.True);
                Assert.That(anonymousContext.HasPermission(PermissionKind.ManageOrganizations), Is.False);
                Assert.That(anonymousContext.HasPermission(PermissionKind.ReadCountry), Is.True);
                Assert.That(adminContext.IsAuthenticated, Is.True);
                Assert.That(anonymousContext.IsAuthenticated, Is.False);
            }
        }
    }
}
