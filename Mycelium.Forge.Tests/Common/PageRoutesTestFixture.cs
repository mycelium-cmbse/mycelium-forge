// ------------------------------------------------------------------------------------------------
// <copyright file="PageRoutesTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Common
{
    using Mycelium.Forge.Common;

    [TestFixture]
    public class PageRoutesTestFixture
    {
        [Test]
        public void VerifyGetAccountRoute()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(PageRoutes.GetAccountRoute("r-andre"), Is.EqualTo("/accounts/r-andre"));
                Assert.That(PageRoutes.GetAccountRoute("@R-Andre "), Is.EqualTo("/accounts/r-andre"));
            }
        }

        [Test]
        public void VerifyGetScopeRoute()
        {
            var account = new Account { ShortName = "@R-Andre" };
            var organization = new Organization { ShortName = "@Starion" };

            using (Assert.EnterMultipleScope())
            {
                Assert.That(PageRoutes.GetScopeRoute(account), Is.EqualTo("/accounts/r-andre"));
                Assert.That(PageRoutes.GetScopeRoute(organization), Is.EqualTo("/organizations/starion"));
                Assert.That(PageRoutes.GetScopeRoute(null), Is.EqualTo(string.Empty));
            }
        }

        [Test]
        public void VerifyGetPackageRoute()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(PageRoutes.GetPackageRoute("@Starion", "ecss-mm-pwr"), Is.EqualTo("/packages/starion/ecss-mm-pwr"));
                Assert.That(PageRoutes.GetPackageRoute("starion", "ecss-mm-pwr", PackageTabConstants.Licenses), Is.EqualTo("/packages/starion/ecss-mm-pwr/licenses"));
                Assert.That(PageRoutes.GetPackageRoute("starion", "ecss-mm-pwr", PackageTabConstants.Licenses, "Apache-2.0"), Is.EqualTo("/packages/starion/ecss-mm-pwr/licenses#Apache-2.0"));
                Assert.That(PageRoutes.GetPackageRoute("starion", "ecss-mm-pwr", PackageTabConstants.Licenses, "#MIT"), Is.EqualTo("/packages/starion/ecss-mm-pwr/licenses#MIT"));
                Assert.That(PageRoutes.GetPackageRoute("starion", "ecss-mm-pwr", fragment: "Apache-2.0"), Is.EqualTo("/packages/starion/ecss-mm-pwr#Apache-2.0"));
                Assert.That(PageRoutes.GetPackageRoute("starion", "ecss-mm-pwr", PackageTabConstants.Licenses, "   "), Is.EqualTo("/packages/starion/ecss-mm-pwr/licenses"));
            }
        }
    }
}
