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

            var package = new Package
            {
                Id = packageId,
                Version = [version1.Id, version2.Id]
            };

            var versions = new List<IPackageVersion> { version1, version2, otherVersion };

            var emptyVersionsPackage = new Package
            {
                Id = Guid.NewGuid(),
                Version = []
            };

            IPackage nullPackage = null!;

            var mockPackage = new Mock<IPackage>();
            mockPackage.Setup(p => p.Version).Returns([version1.Id, version2.Id]);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(package.ComputeDownloadCount(versions), Is.EqualTo(500));
                Assert.That(emptyVersionsPackage.ComputeDownloadCount([]), Is.Zero);
                Assert.That(nullPackage.ComputeDownloadCount(versions), Is.Zero);
                Assert.That(mockPackage.Object.ComputeDownloadCount(versions), Is.EqualTo(500));
            }
        }
    }
}
