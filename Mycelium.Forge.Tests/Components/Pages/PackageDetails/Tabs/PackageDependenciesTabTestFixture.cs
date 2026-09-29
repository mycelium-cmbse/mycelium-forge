// ------------------------------------------------------------------------------------------------
// <copyright file="PackageDependenciesTabTestFixture.cs" company="Starion Group S.A.">
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
    /// Test fixture for <see cref="PackageDependenciesTab" /> component.
    /// </summary>
    [TestFixture]
    public class PackageDependenciesTabTestFixture
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
        /// Verifies that <see cref="PackageDependenciesTab" /> renders correctly for empty and populated dependencies.
        /// </summary>
        [Test]
        public void VerifyPackageDependenciesTabRendering()
        {
            var emptyTab = this.context.Render<PackageDependenciesTab>(parameters => { parameters.Add(x => x.Dependencies, []); });

            var emptyMarkup = emptyTab.Markup;

            var dependencies = new List<(string Name, string Summary, bool IsVerified)>
            {
                ("@esa/CoreTypes", "Core types library", true),
                ("@starion/Units", "SI unit definitions", false)
            };

            var populatedTab = this.context.Render<PackageDependenciesTab>(parameters => { parameters.Add(x => x.Dependencies, dependencies); });

            var markup = populatedTab.Markup;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(emptyMarkup.Trim(), Has.Length.LessThan(5));
                Assert.That(markup, Does.Contain("Dependencies"));
                Assert.That(markup, Does.Contain("@esa/CoreTypes"));
                Assert.That(markup, Does.Contain("@starion/Units"));
                Assert.That(markup, Does.Contain("Core types library"));
            }
        }
    }
}
