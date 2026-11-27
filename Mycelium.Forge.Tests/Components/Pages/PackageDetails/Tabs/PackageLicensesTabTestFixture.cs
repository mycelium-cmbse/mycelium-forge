// ------------------------------------------------------------------------------------------------
// <copyright file="PackageLicensesTabTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.PackageDetails.Tabs
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.PackageDetails.Tabs;

    [TestFixture]
    public class PackageLicensesTabTestFixture
    {
        private BunitContext context;
        private License apacheLicense;
        private License mitLicense;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.apacheLicense = new License
            {
                Id = Guid.NewGuid(),
                Name = "Apache License 2.0",
                SpdxIdentifier = "Apache-2.0",
                LicenseText = "Apache License 2.0 full text"
            };

            this.mitLicense = new License
            {
                Id = Guid.NewGuid(),
                Name = "MIT License",
                SpdxIdentifier = "MIT",
                LicenseText = "MIT License full text"
            };
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyPackageLicensesTabRendering()
        {
            var nullLicensesTab = this.context.Render<PackageLicensesTab>(parameters => parameters.Add(x => x.Licenses, null!));
            var emptyLicensesTab = this.context.Render<PackageLicensesTab>(parameters => parameters.Add(x => x.Licenses, []));
            var singleLicenseTab = this.context.Render<PackageLicensesTab>(parameters => parameters.Add(x => x.Licenses, [this.apacheLicense]));
            var multipleLicensesTab = this.context.Render<PackageLicensesTab>(parameters => parameters.Add(x => x.Licenses, [this.apacheLicense, this.mitLicense]));

            var singleBadges = singleLicenseTab.FindComponents<BbBadge>();
            var multipleBadges = multipleLicensesTab.FindComponents<BbBadge>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(nullLicensesTab.Markup, Does.Contain("No licenses declared for this release."));
                Assert.That(emptyLicensesTab.Markup, Does.Contain("No licenses declared for this release."));
                Assert.That(singleLicenseTab.Markup, Does.Contain("1 license declared for this release."));
                Assert.That(singleLicenseTab.Markup, Does.Contain("Apache License 2.0"));
                Assert.That(singleLicenseTab.Markup, Does.Contain("Apache-2.0"));
                Assert.That(singleLicenseTab.Markup, Does.Contain("Apache License 2.0 full text"));
                Assert.That(singleBadges, Has.Count.EqualTo(1));
                Assert.That(multipleLicensesTab.Markup, Does.Contain("2 licenses declared for this release."));
                Assert.That(multipleLicensesTab.Markup, Does.Contain("Apache License 2.0"));
                Assert.That(multipleLicensesTab.Markup, Does.Contain("MIT License"));
                Assert.That(multipleLicensesTab.Markup, Does.Contain("MIT License full text"));
                Assert.That(multipleLicensesTab.FindAll("[id='Apache-2.0']"), Has.Count.EqualTo(1));
                Assert.That(multipleLicensesTab.FindAll("[id='MIT']"), Has.Count.EqualTo(1));
                Assert.That(multipleBadges, Has.Count.EqualTo(2));
                Assert.That(multipleLicensesTab.FindAll("pre").Select(pre => pre.TextContent), Is.EquivalentTo((string[])["Apache License 2.0 full text", "MIT License full text"]));
            }
        }
    }
}
