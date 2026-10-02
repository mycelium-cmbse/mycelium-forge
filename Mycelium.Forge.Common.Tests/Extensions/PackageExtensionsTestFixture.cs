// ------------------------------------------------------------------------------------------------
// <copyright file="PackageExtensionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.Tests.Extensions
{
    using System;
    using System.Collections.Generic;

    using Moq;

    using Mycelium.Forge.Common.Extensions;

    [TestFixture]
    public class PackageExtensionsTestFixture
    {
        [Test]
        public void VerifyComputeDownloadCount()
        {
            var packageId = Guid.NewGuid();
            var otherPackageId = Guid.NewGuid();

            var package = new Package
            {
                Id = packageId
            };

            var version1 = new PackageVersion
            {
                Id = Guid.NewGuid(),
                Owner = packageId,
                DownloadCount = 150
            };

            var version2 = new PackageVersion
            {
                Id = Guid.NewGuid(),
                Owner = packageId,
                DownloadCount = 350
            };

            var otherVersion = new PackageVersion
            {
                Id = Guid.NewGuid(),
                Owner = otherPackageId,
                DownloadCount = 1000
            };

            var versions = new List<IPackageVersion> { version1, version2, otherVersion };

            package.ComputeDownloadCount(versions);

            var emptyVersionsPackage = new Package
            {
                Id = Guid.NewGuid()
            };

            emptyVersionsPackage.ComputeDownloadCount([]);

            var nullVersionsPackage = new Package
            {
                Id = Guid.NewGuid()
            };

            nullVersionsPackage.ComputeDownloadCount(null!);

            IPackage nullPackage = null!;
            nullPackage.ComputeDownloadCount(versions);

            var mockPackage = new Mock<IPackage>();
            mockPackage.Object.ComputeDownloadCount(versions);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(package.downloadCount, Is.EqualTo(500));
                Assert.That(emptyVersionsPackage.downloadCount, Is.Zero);
                Assert.That(nullVersionsPackage.downloadCount, Is.Zero);
            }
        }
    }
}
