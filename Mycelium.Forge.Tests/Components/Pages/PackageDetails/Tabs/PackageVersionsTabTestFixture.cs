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
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Mycelium.Forge.Components.Pages.PackageDetails.Tabs;
    using Mycelium.Forge.Models.Package;

    [TestFixture]
    public class PackageVersionsTabTestFixture
    {
        private BunitContext context;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyPackageVersionsTabRendering()
        {
            // Empty list renders nothing
            var emptyTab = this.context.Render<PackageVersionsTab>(parameters => { parameters.Add(x => x.Versions, []); });

            var emptyMarkup = emptyTab.Markup;

            // IsLatest
            var latestTab = this.context.Render<PackageVersionsTab>(parameters => { parameters.Add(x => x.Versions, [new PackageVersionModel { Version = "1.0.0", IsLatest = true }]); });

            var latestMarkup = latestTab.Markup;

            // IsUnlisted
            var unlistedTab = this.context.Render<PackageVersionsTab>(parameters => { parameters.Add(x => x.Versions, [new PackageVersionModel { Version = "0.9.0", IsUnlisted = true }]); });

            var unlistedMarkup = unlistedTab.Markup;

            // IsDeprecated
            var deprecatedTab = this.context.Render<PackageVersionsTab>(parameters => { parameters.Add(x => x.Versions, [new PackageVersionModel { Version = "0.8.0", IsDeprecated = true }]); });

            var deprecatedMarkup = deprecatedTab.Markup;

            // IsValidated
            var validatedTab = this.context.Render<PackageVersionsTab>(parameters => { parameters.Add(x => x.Versions, [new PackageVersionModel { Version = "1.0.0", IsValidated = true }]); });

            var validatedMarkup = validatedTab.Markup;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(emptyMarkup.Trim(), Has.Length.LessThan(5));
                Assert.That(latestMarkup, Does.Contain("Latest"));
                Assert.That(unlistedMarkup, Does.Contain("Unlisted"));
                Assert.That(deprecatedMarkup, Does.Contain("Deprecated"));
                Assert.That(validatedMarkup, Does.Contain("Validated"));
            }
        }
    }
}
