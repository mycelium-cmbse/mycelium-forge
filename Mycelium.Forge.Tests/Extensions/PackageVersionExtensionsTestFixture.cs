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

    /// <summary>
    /// Test fixture for <see cref="PackageVersionExtensions" />.
    /// </summary>
    [TestFixture]
    public class PackageVersionExtensionsTestFixture
    {
        /// <summary>
        /// Verifies that <see cref="PackageVersionExtensions.FormatFileSize(long)" /> formats file sizes correctly.
        /// </summary>
        [Test]
        public void VerifyFormatFileSize()
        {
            var bytes = PackageVersionExtensions.FormatFileSize(500);
            var kb = PackageVersionExtensions.FormatFileSize(2048);
            var mb = PackageVersionExtensions.FormatFileSize(2 * 1024 * 1024);
            var gb = PackageVersionExtensions.FormatFileSize(3L * 1024 * 1024 * 1024);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(bytes, Is.EqualTo("500 B"));
                Assert.That(kb, Is.EqualTo("2 KB"));
                Assert.That(mb, Is.EqualTo("2 MB"));
                Assert.That(gb, Is.EqualTo("3 GB"));
            }
        }

        /// <summary>
        /// Verifies that <see cref="PackageVersionExtensions.GetFormattedSize(IPackageVersion)" /> formats version size.
        /// </summary>
        [Test]
        public void VerifyGetFormattedSize()
        {
            var version = new PackageVersion
            {
                SizeInBytes = 1024 * 1024 * 4
            };

            var formatted = version.GetFormattedSize();

            Assert.That(formatted, Is.EqualTo("4 MB"));
        }

        /// <summary>
        /// Verifies that <see cref="PackageVersionExtensions.GetVersion(IPackageVersion)" /> returns version string.
        /// </summary>
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
