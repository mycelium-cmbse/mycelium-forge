// ------------------------------------------------------------------------------------------------
// <copyright file="PackageVersionsTabTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.PackageDetails.Tabs
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.PackageDetails.Tabs;

    /// <summary>
    /// Test fixture for <see cref="PackageVersionsTab" /> component.
    /// </summary>
    [TestFixture]
    public class PackageVersionsTabTestFixture
    {
        private BunitContext context;
        private Organization organization;
        private Package package;

        /// <summary>
        /// Sets up the test context before each test execution.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.organization = new Organization
            {
                Id = Guid.NewGuid(),
                Name = "Starion Group",
                ShortName = "starion"
            };

            this.package = new Package
            {
                Id = Guid.NewGuid(),
                Name = "ECSS-MM-PWR",
                ShortName = "ecss-mm-pwr",
                Owner = this.organization.Id
            };
        }

        /// <summary>
        /// Tears down the test context after each test execution.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous tear down.</returns>
        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        /// <summary>
        /// Verifies that <see cref="PackageVersionsTab" /> computes helpers and renders version states.
        /// </summary>
        [Test]
        public void VerifyPackageVersionsTabRendering()
        {
            // Empty list renders nothing
            var emptyTab = this.context.Render<PackageVersionsTab>(parameters =>
            {
                parameters.Add(x => x.Versions, []);
                parameters.Add(x => x.Owner, this.organization);
                parameters.Add(x => x.Package, this.package);
            });

            var emptyMarkup = emptyTab.Markup;

            var versionLatest = new PackageVersion
            {
                Id = Guid.NewGuid(),
                Version = "1.3.0",
                Listed = true,
                SizeInBytes = 2048,
                PublicationDate = DateTime.UtcNow.AddDays(-1)
            };

            var versionUnlisted = new PackageVersion
            {
                Id = Guid.NewGuid(),
                Version = "1.2.0",
                Listed = false,
                SizeInBytes = 1024,
                PublicationDate = DateTime.UtcNow.AddDays(-10)
            };

            var versions = new List<IPackageVersion> { versionLatest, versionUnlisted };
            var qualityChecks = new List<(string Title, string Detail, bool Passed)> { ("Check 1", "Detail", true) };

            var populatedTab = this.context.Render<PackageVersionsTab>(parameters =>
            {
                parameters.Add(x => x.Versions, versions);
                parameters.Add(x => x.QualityChecks, qualityChecks);
                parameters.Add(x => x.Owner, this.organization);
                parameters.Add(x => x.Package, this.package);
            });

            var isLatest = populatedTab.Instance.IsLatestVersion(versionLatest);
            var isNotLatest = populatedTab.Instance.IsLatestVersion(versionUnlisted);
            var isValidated = populatedTab.Instance.IsValidated(versionLatest);
            var isNotValidated = emptyTab.Instance.IsValidated(versionUnlisted);
            var downloadUrl = populatedTab.Instance.GetDownloadUrl(versionLatest);
            var markup = populatedTab.Markup;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(emptyMarkup.Trim(), Has.Length.LessThan(5));
                Assert.That(isLatest, Is.True);
                Assert.That(isNotLatest, Is.False);
                Assert.That(isValidated, Is.True);
                Assert.That(isNotValidated, Is.False);
                Assert.That(downloadUrl, Does.Contain("/api/packages/starion/ecss-mm-pwr/1.3.0/download"));
                Assert.That(markup, Does.Contain("Latest"));
                Assert.That(markup, Does.Contain("Unlisted"));
                Assert.That(markup, Does.Contain("Validated"));
            }
        }
    }
}
