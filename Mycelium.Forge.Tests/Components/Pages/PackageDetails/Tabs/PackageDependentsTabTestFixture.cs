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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Icons.Lucide.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Mycelium.Forge.Components.Pages.PackageDetails.Tabs;

    /// <summary>
    /// Test fixture for <see cref="PackageDependentsTab" /> component.
    /// </summary>
    [TestFixture]
    public class PackageDependentsTabTestFixture
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
        /// Verifies that <see cref="PackageDependentsTab" /> renders correctly for various dependent entries.
        /// </summary>
        [Test]
        public void VerifyPackageDependentsTabRendering()
        {
            // Empty list renders nothing
            var emptyTab = this.context.Render<PackageDependentsTab>(parameters => { parameters.Add(x => x.Dependents, []); });
            var emptyMarkup = emptyTab.Markup;

            // IsProject=true -> folder icon
            var projectTab = this.context.Render<PackageDependentsTab>(parameters =>
            {
                parameters.Add(x => x.Dependents, new List<(string Name, string Summary, bool IsProject, bool IsVerified)>
                {
                    ("@starion/Spacecraft", "Mission project", true, false)
                });
            });

            // IsProject=false -> box icon
            var packageTab = this.context.Render<PackageDependentsTab>(parameters =>
            {
                parameters.Add(x => x.Dependents, new List<(string Name, string Summary, bool IsProject, bool IsVerified)>
                {
                    ("@starion/ECSS-Mission", "Mission package", false, false)
                });
            });

            // IsVerified=true -> badge-check
            var verifiedTab = this.context.Render<PackageDependentsTab>(parameters =>
            {
                parameters.Add(x => x.Dependents, new List<(string Name, string Summary, bool IsProject, bool IsVerified)>
                {
                    ("@starion/ECSS-Verified", "Verified package", false, true)
                });
            });

            using (Assert.EnterMultipleScope())
            {
                Assert.That(emptyMarkup.Trim(), Has.Length.LessThan(5));
                Assert.That(projectTab.FindComponents<LucideIcon>().Any(x => x.Instance.Name == "folder"), Is.True);
                Assert.That(packageTab.FindComponents<LucideIcon>().Any(x => x.Instance.Name == "box"), Is.True);
                Assert.That(verifiedTab.FindComponents<LucideIcon>().Any(x => x.Instance.Name == "badge-check"), Is.True);
            }
        }
    }
}
