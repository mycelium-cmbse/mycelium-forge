// ------------------------------------------------------------------------------------------------
// <copyright file="PackageValidationTabTestFixture.cs" company="Starion Group S.A.">
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
    /// Test fixture for <see cref="PackageValidationTab" /> component.
    /// </summary>
    [TestFixture]
    public class PackageValidationTabTestFixture
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
        /// Verifies that <see cref="PackageValidationTab" /> renders validation checks and status.
        /// </summary>
        [Test]
        public void VerifyPackageValidationTabRendering()
        {
            var emptyTab = this.context.Render<PackageValidationTab>(parameters => { parameters.Add(x => x.Checks, []); });

            var emptyMarkup = emptyTab.Markup;

            var version = new PackageVersion
            {
                Id = Guid.NewGuid(),
                Version = "1.3.0"
            };

            var allPassedChecks = new List<(string Title, string Detail, bool Passed)>
            {
                ("Schema check", "Valid SysML v2 syntax", true),
                ("Metamodel check", "Conforms to standard", true)
            };

            var passedTab = this.context.Render<PackageValidationTab>(parameters =>
            {
                parameters.Add(x => x.Checks, allPassedChecks);
                parameters.Add(x => x.Version, version);
            });

            var passedMarkup = passedTab.Markup;

            var mixedChecks = new List<(string Title, string Detail, bool Passed)>
            {
                ("Schema check", "Valid SysML v2 syntax", true),
                ("Security check", "Vulnerability found", false)
            };

            var mixedTab = this.context.Render<PackageValidationTab>(parameters =>
            {
                parameters.Add(x => x.Checks, mixedChecks);
                parameters.Add(x => x.Version, version);
            });

            var mixedMarkup = mixedTab.Markup;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(emptyMarkup.Trim(), Has.Length.LessThan(5));
                Assert.That(passedMarkup, Does.Contain("Release validation passed"));
                Assert.That(passedMarkup, Does.Contain("2 / 2"));
                Assert.That(mixedMarkup, Does.Contain("Release validation issues"));
                Assert.That(mixedMarkup, Does.Contain("1 / 2"));
            }
        }
    }
}
