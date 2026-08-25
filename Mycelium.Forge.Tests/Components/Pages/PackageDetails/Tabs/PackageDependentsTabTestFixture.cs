// ------------------------------------------------------------------------------------------------
// <copyright file="PackageDependentsTabTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.PackageDetails.Tabs
{
    using System.Linq;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Icons.Lucide.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Mycelium.Forge.Components.Pages.PackageDetails.Tabs;
    using Mycelium.Forge.Models.Package;

    [TestFixture]
    public class PackageDependentsTabTestFixture
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
        public void VerifyPackageDependentsTabRendering()
        {
            // Empty list renders nothing
            var emptyTab = this.context.Render<PackageDependentsTab>(parameters => { parameters.Add(x => x.Dependents, []); });

            var emptyMarkup = emptyTab.Markup;

            // IsProject=true -> folder icon
            var projectTab = this.context.Render<PackageDependentsTab>(parameters => { parameters.Add(x => x.Dependents, [new PackageRelationshipModel { Name = "MyProject", IsProject = true }]); });

            // IsProject=false -> box icon
            var packageTab = this.context.Render<PackageDependentsTab>(parameters => { parameters.Add(x => x.Dependents, [new PackageRelationshipModel { Name = "MyPackage", IsProject = false }]); });

            // Has Href -> anchor
            var hrefTab = this.context.Render<PackageDependentsTab>(parameters => { parameters.Add(x => x.Dependents, [new PackageRelationshipModel { Name = "LinkedPackage", Href = "/packages/@org/pkg" }]); });

            var hrefMarkup = hrefTab.Markup;

            // No Href -> no anchor for name
            var noHrefTab = this.context.Render<PackageDependentsTab>(parameters => { parameters.Add(x => x.Dependents, [new PackageRelationshipModel { Name = "NoLinkPackage", Href = string.Empty }]); });

            var noHrefMarkup = noHrefTab.Markup;

            // IsVerified=true -> badge-check
            var verifiedTab = this.context.Render<PackageDependentsTab>(parameters => { parameters.Add(x => x.Dependents, [new PackageRelationshipModel { Name = "VerifiedPkg", IsVerified = true }]); });

            using (Assert.EnterMultipleScope())
            {
                Assert.That(emptyMarkup.Trim(), Has.Length.LessThan(5));
                Assert.That(projectTab.FindComponents<LucideIcon>().Any(x => x.Instance.Name == "folder"), Is.True);
                Assert.That(packageTab.FindComponents<LucideIcon>().Any(x => x.Instance.Name == "box"), Is.True);
                Assert.That(hrefMarkup, Does.Contain("href").And.Contain("/packages/@org/pkg"));
                Assert.That(noHrefMarkup, Does.Not.Contain("<a "));
                Assert.That(verifiedTab.FindComponents<LucideIcon>().Any(x => x.Instance.Name == "badge-check"), Is.True);
            }
        }
    }
}
