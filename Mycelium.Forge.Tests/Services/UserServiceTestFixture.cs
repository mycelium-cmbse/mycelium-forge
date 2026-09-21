// ------------------------------------------------------------------------------------------------
// <copyright file="UserServiceTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Services
{
    using System.Threading.Tasks;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Data;
    using Mycelium.Forge.Services;

    [TestFixture]
    public class UserServiceTestFixture
    {
        private UserService userService;

        [SetUp]
        public void SetUp()
        {
            this.userService = new UserService();
        }

        [Test]
        public void VerifyCurrentRoles()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.userService.CurrentRoles, Has.Count.EqualTo(3));
                Assert.That(this.userService.CurrentRoles, Contains.Item(RoleKind.InstallationAdministrator));
                Assert.That(this.userService.CurrentRoles, Contains.Item(RoleKind.OrganizationAdministrator));
                Assert.That(this.userService.CurrentRoles, Contains.Item(RoleKind.Account));
            }

            this.userService.CurrentRoles = [RoleKind.Account];

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.userService.CurrentRoles, Has.Count.EqualTo(1));
                Assert.That(this.userService.CurrentRoles, Contains.Item(RoleKind.Account));
            }
        }

        [Test]
        public async Task VerifyGetCurrentUser()
        {
            var user = await this.userService.GetCurrentUser();
            var userForceLoaded = await this.userService.GetCurrentUser(true);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(user, Is.Not.Null);
                Assert.That(user.Id, Is.EqualTo(SeedData.RegisAccount.Id));
                Assert.That(user.Name, Is.EqualTo(SeedData.RegisAccount.Name));
                Assert.That(user.ShortName, Is.EqualTo(SeedData.RegisAccount.ShortName));
                Assert.That(userForceLoaded, Is.SameAs(user));
            }
        }

        [Test]
        public void VerifyIsAuthenticated()
        {
            Assert.That(this.userService.IsAuthenticated, Is.True);

            this.userService.CurrentRoles = [RoleKind.Anonymous];
            Assert.That(this.userService.IsAuthenticated, Is.False);
        }
    }
}
