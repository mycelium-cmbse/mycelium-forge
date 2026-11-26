// ------------------------------------------------------------------------------------------------
// <copyright file="ArtifactManifestExtensionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Extensions
{
    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extensions;
    using Mycelium.Forge.Extractors.Models;
    using Mycelium.Forge.Extractors.Models.Capella;
    using Mycelium.Forge.Extractors.Models.EcssC3;
    using Mycelium.Forge.Extractors.Models.Kpar;

    [TestFixture]
    public class ArtifactManifestExtensionsTestFixture
    {
        [Test]
        public void VerifyCapellaApplyTo()
        {
            var packageVersion = new PackageVersion();

            // Case 1: Apply Capella manifest with major/minor numbers and description
            var capellaPackage = new Package();

            var capellaManifest = new CapellaManifest
            {
                Name = "Capella Model",
                Description = "A Capella model description",
                MajorVersionNumber = 2,
                MinorVersionNumber = 1
            };

            capellaManifest.ApplyTo(capellaPackage, packageVersion, false);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(capellaPackage.Name, Is.EqualTo("Capella Model"));
                Assert.That(capellaPackage.ShortName, Is.EqualTo("capella-model"));
                Assert.That(capellaPackage.Description, Is.EqualTo("A Capella model description"));
                Assert.That(packageVersion.Version, Is.EqualTo("2.1.0"));
                Assert.That(capellaManifest.GetDependencies(), Is.Empty);
            }

            // Case 2: Apply Capella manifest with summary fallback and no version numbers
            var capellaSummaryManifest = new CapellaManifest { Name = "Capella Summary", Summary = "Summary text" };
            var capellaSummaryPackage = new Package();
            capellaSummaryManifest.ApplyTo(capellaSummaryPackage, packageVersion, false);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(capellaSummaryPackage.Description, Is.EqualTo("Summary text"));
                Assert.That(packageVersion.Version, Is.Empty);
            }
        }

        [Test]
        public void VerifyEcssC3ApplyTo()
        {
            var packageVersion = new PackageVersion();

            var ecssPackage = new Package();
            var ecssManifest = new EcssC3Manifest { Remark = "ECSS Remark", Copyright = "ESA" };
            ecssManifest.ApplyTo(ecssPackage, packageVersion, false);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(ecssPackage.Name, Is.Empty);
                Assert.That(ecssPackage.ShortName, Is.Empty);
                Assert.That(ecssPackage.Description, Is.EqualTo("ECSS Remark"));
                Assert.That(packageVersion.Version, Is.Empty);
                Assert.That(ecssManifest.GetDependencies(), Is.Empty);
            }
        }

        [Test]
        public void VerifyKparApplyTo()
        {
            var package = new Package();
            var packageVersion = new PackageVersion();

            var kparManifest = new KparManifest
            {
                Name = "sysml-core",
                Description = "SysML v2 Core Library",
                License = "MIT",
                Website = "https://example.com/sysml",
                Topics = ["sysml", "modeling"],
                Version = "1.2.3",
                Dependencies = [new KparDependency { Resource = "dep-1", VersionConstraint = ">=1.0.0" }]
            };

            // Case 1: Apply to new package copies metadata and extracts dependencies
            kparManifest.ApplyTo(package, packageVersion, false);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(package.Name, Is.EqualTo("sysml-core"));
                Assert.That(package.ShortName, Is.EqualTo("sysml-core"));
                Assert.That(package.Description, Is.EqualTo("SysML v2 Core Library"));
                Assert.That(package.HomepageUrl, Is.EqualTo("https://example.com/sysml"));
                Assert.That(packageVersion.Version, Is.EqualTo("1.2.3"));
                Assert.That(kparManifest.GetDependencies(), Has.Count.EqualTo(1));
            }

            // Case 2: Apply to existing package preserves existing name and description
            var existingPackage = new Package { Name = "original-name", Description = "original-desc" };
            kparManifest.ApplyTo(existingPackage, packageVersion, true);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(existingPackage.Name, Is.EqualTo("original-name"));
                Assert.That(existingPackage.Description, Is.EqualTo("original-desc"));
            }
        }

        [Test]
        public void VerifyUnknownManifestApplyTo()
        {
            var package = new Package { Name = "Existing Name" };
            var packageVersion = new PackageVersion();

            var unknownManifestMock = new Mock<IArtifactManifest>();
            unknownManifestMock.Object.ApplyTo(package, packageVersion, false);

            Assert.That(package.Name, Is.EqualTo("Existing Name"));
        }
    }
}
