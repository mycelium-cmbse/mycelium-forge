// ------------------------------------------------------------------------------------------------
// <copyright file="PackageOverviewTabTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.PackageDetails.Tabs
{
    using System;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.PackageDetails.Tabs;

    /// <summary>
    /// Test fixture for <see cref="PackageOverviewTab" /> component.
    /// </summary>
    [TestFixture]
    public class PackageOverviewTabTestFixture
    {
        private BunitContext context;

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
        /// Verifies that <see cref="PackageOverviewTab" /> renders package information, README contents, and usage.
        /// </summary>
        [Test]
        public void VerifyPackageOverviewTabRendering()
        {
            var package = new Package
            {
                Id = Guid.NewGuid(),
                Name = "ECSS-MM-PWR",
                ShortName = "ecss-mm-pwr"
            };

            var version = new PackageVersion
            {
                Id = Guid.NewGuid(),
                Owner = package.Id,
                Version = "1.3.0",
                Readme = "# Overview\n\nPower subsystem library.\n\n## Installation\n\nHow to install.\n\n## Usage\n\nHow to use."
            };

            var component = this.context.Render<PackageOverviewTab>(parameters =>
            {
                parameters.Add(x => x.Package, package);
                parameters.Add(x => x.Version, version);
            });

            var readmeContents = component.Instance.GetReadmeContents();
            var import = component.Instance.CodeUsageImport;
            var markup = component.Markup;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(readmeContents, Does.Contain("Overview"));
                Assert.That(readmeContents, Does.Contain("Installation"));
                Assert.That(readmeContents, Does.Contain("Usage"));
                Assert.That(import, Is.EqualTo("import ecss_mm_pwr::*;"));
                Assert.That(markup, Does.Contain("ECSS-MM-PWR"));
                Assert.That(markup, Does.Contain("Usage"));
            }

            version.Readme = null;
            var emptyContents = component.Instance.GetReadmeContents();
            Assert.That(emptyContents, Is.EqualTo(string.Empty));
        }
    }
}
