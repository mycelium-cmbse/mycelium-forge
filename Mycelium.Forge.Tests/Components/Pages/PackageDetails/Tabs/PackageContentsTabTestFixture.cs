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

    /// <summary>
    /// Test fixture for <see cref="PackageContentsTab" /> component.
    /// </summary>
    [TestFixture]
    public class PackageContentsTabTestFixture
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
        /// Verifies that <see cref="PackageContentsTab.GetFilteredElements" /> filters elements by category or returns all.
        /// </summary>
        [Test]
        public void VerifyGetFilteredElements()
        {
            var elements = new List<(string Name, string Kind, string Category, string AttributeSummary)>
            {
                ("PowerSubsystem", "«part def»", "Parts", "3 parts, 2 attributes"),
                ("Voltage", "«attribute def»", "Attributes", "Real"),
                ("Current", "«attribute def»", "Attributes", "Real")
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

        /// <summary>
        /// Verifies that <see cref="PackageContentsTab.GetKindButtonClass(string)" /> computes active and inactive button styling.
        /// </summary>
        [Test]
        public void VerifyGetKindButtonClass()
        {
            var elements = new List<(string Name, string Kind, string Category, string AttributeSummary)>
            {
                ("PowerSubsystem", "«part def»", "Parts", "3 parts")
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

        /// <summary>
        /// Verifies that <see cref="PackageContentsTab" /> renders correctly for populated and empty elements.
        /// </summary>
        [Test]
        public void VerifyPackageContentsTabRendering()
        {
            var emptyTab = this.context.Render<PackageContentsTab>(parameters => { parameters.Add(x => x.Elements, []); });

            var elements = new List<(string Name, string Kind, string Category, string AttributeSummary)>
            {
                ("PowerSubsystem", "«part def»", "Parts", "3 parts, 2 attributes"),
                ("Voltage", "«attribute def»", "Attributes", "Real")
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

        /// <summary>
        /// Verifies that <see cref="PackageContentsTab.SelectKindTab(string)" /> updates the selected kind tab.
        /// </summary>
        [Test]
        public void VerifySelectKindTab()
        {
            var elements = new List<(string Name, string Kind, string Category, string AttributeSummary)>
            {
                ("PowerSubsystem", "«part def»", "Parts", "3 parts")
            };

            var tab = this.context.Render<PackageContentsTab>(parameters => { parameters.Add(x => x.Elements, elements); });

            tab.Instance.SelectKindTab("Units");

            Assert.That(tab.Instance.SelectedKindTab, Is.EqualTo("Units"));
        }
    }
}
