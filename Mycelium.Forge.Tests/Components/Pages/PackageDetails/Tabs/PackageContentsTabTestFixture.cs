// ------------------------------------------------------------------------------------------------
// <copyright file="PackageContentsTabTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.PackageDetails.Tabs
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Mycelium.Forge.Components.Pages.PackageDetails.Tabs;
    using Mycelium.Forge.Models.Package;

    [TestFixture]
    public class PackageContentsTabTestFixture
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
        public void VerifyGetFilteredElements()
        {
            var elements = new List<PackageElementModel>
            {
                new("PowerSubsystem", "«part def»", "Parts", "3 parts, 2 attributes"),
                new("Voltage", "«attribute def»", "Attributes", "Real"),
                new("Current", "«attribute def»", "Attributes", "Real")
            };

            var tab = this.context.Render<PackageContentsTab>(parameters => { parameters.Add(x => x.Elements, elements); });

            tab.Instance.SelectKindTab("Parts");
            var parts = tab.Instance.GetFilteredElements();

            tab.Instance.SelectKindTab("Attributes");
            var attributes = tab.Instance.GetFilteredElements();

            tab.Instance.SelectKindTab("NonExistent");
            var fallback = tab.Instance.GetFilteredElements();

            var emptyTab = this.context.Render<PackageContentsTab>(parameters => { parameters.Add(x => x.Elements, []); });

            var empty = emptyTab.Instance.GetFilteredElements();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(parts, Has.Count.EqualTo(1));
                Assert.That(parts[0].Name, Is.EqualTo("PowerSubsystem"));
                Assert.That(attributes, Has.Count.EqualTo(2));
                Assert.That(fallback, Has.Count.EqualTo(3));
                Assert.That(empty, Has.Count.EqualTo(0));
            }
        }

        [Test]
        public void VerifyGetKindButtonClass()
        {
            var elements = new List<PackageElementModel>
            {
                new("PowerSubsystem", "«part def»", "Parts", "3 parts")
            };

            var tab = this.context.Render<PackageContentsTab>(parameters => { parameters.Add(x => x.Elements, elements); });

            var selectedClass = tab.Instance.GetKindButtonClass("Parts");
            var unselectedClass = tab.Instance.GetKindButtonClass("Attributes");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(selectedClass, Does.Contain("bg-primary/10"));
                Assert.That(unselectedClass, Is.EqualTo("rounded-full"));
            }
        }

        [Test]
        public void VerifyPackageContentsTabRendering()
        {
            var emptyTab = this.context.Render<PackageContentsTab>(parameters => { parameters.Add(x => x.Elements, []); });

            var elements = new List<PackageElementModel>
            {
                new("PowerSubsystem", "«part def»", "Parts", "3 parts, 2 attributes"),
                new("Voltage", "«attribute def»", "Attributes", "Real")
            };

            var tab = this.context.Render<PackageContentsTab>(parameters => { parameters.Add(x => x.Elements, elements); });

            var markup = tab.Markup;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(emptyTab.Markup.Trim(), Has.Length.LessThan(5));
                Assert.That(markup, Does.Contain("Contents"));
                Assert.That(markup, Does.Contain("Read-only"));
                Assert.That(markup, Does.Contain("PowerSubsystem"));
                Assert.That(markup, Does.Contain("«part def»"));
                Assert.That(tab.Instance.KindTabs, Has.Count.EqualTo(6));
            }
        }

        [Test]
        public void VerifySelectKindTab()
        {
            var elements = new List<PackageElementModel>
            {
                new("PowerSubsystem", "«part def»", "Parts", "3 parts")
            };

            var tab = this.context.Render<PackageContentsTab>(parameters => { parameters.Add(x => x.Elements, elements); });

            tab.Instance.SelectKindTab("Units");

            Assert.That(tab.Instance.SelectedKindTab, Is.EqualTo("Units"));
        }
    }
}
