// ------------------------------------------------------------------------------------------------
// <copyright file="PackageVersionExtensionsTestFixture.cs" company="Starion Group S.A.">
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

    [TestFixture]
    public class PackageVersionExtensionsTestFixture
    {
        [Test]
        public void VerifyGetVersion()
        {
            var version = new PackageVersion
            {
                Version = "1.3.0"
            };

            var resolvedVersion = version.GetVersion();
            var nullVersion = ((IPackageVersion)null!).GetVersion();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(resolvedVersion, Is.EqualTo("1.3.0"));
                Assert.That(nullVersion, Is.EqualTo(string.Empty));
            }
        }
    }
}
