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
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extensions;

    /// <summary>
    /// Test fixture for <see cref="PackageExtensions" />.
    /// </summary>
    [TestFixture]
    public class PackageExtensionsTestFixture
    {
        /// <summary>
        /// Verifies that <see cref="PackageExtensions.GetFullName(IPackage, IOrganization)" /> computes full package name.
        /// </summary>
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
