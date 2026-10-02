// ------------------------------------------------------------------------------------------------
// <copyright file="TableSkeletonTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Common
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Mycelium.Forge.Components.Common;

    /// <summary>
    /// Test fixture for <see cref="TableSkeleton" />.
    /// </summary>
    [TestFixture]
    public class TableSkeletonTestFixture
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
        /// Tears down the BUnit context after each test.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        /// <summary>
        /// Verifies rendering of <see cref="TableSkeleton" /> with default and custom parameters.
        /// </summary>
        [Test]
        public void VerifyTableSkeletonRendering()
        {
            var defaultSkeletonComponent = this.context.Render<TableSkeleton>();
            var defaultSkeletons = defaultSkeletonComponent.FindComponents<BbSkeleton>();
            var defaultSeparators = defaultSkeletonComponent.FindComponents<BbSeparator>();

            var customSkeletonComponent = this.context.Render<TableSkeleton>(parameters => parameters
                .Add(p => p.ColumnCount, 2)
                .Add(p => p.RowCount, 3)
                .Add(p => p.HeaderHeightClass, "h-10")
                .Add(p => p.RowHeightClass, "h-4")
                .Add(p => p.GapClass, "gap-2")
                .Add(p => p.Class, "custom-table-skeleton")
                .Add(p => p.AdditionalAttributes, new Dictionary<string, object>
                {
                    { "id", "test-table-skeleton" },
                    { "data-testid", "table-skeleton" }
                }));

            var customSkeletons = customSkeletonComponent.FindComponents<BbSkeleton>();
            var customElement = customSkeletonComponent.Find("#test-table-skeleton");

            var emptySkeletonComponent = this.context.Render<TableSkeleton>(parameters => parameters
                .Add(p => p.ColumnCount, 0)
                .Add(p => p.RowCount, 0));

            var emptySkeletons = emptySkeletonComponent.FindComponents<BbSkeleton>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(defaultSkeletonComponent.Instance, Is.Not.Null);

                // Default: 4 header column skeletons + (5 body rows * 4 column skeletons) = 24
                Assert.That(defaultSkeletons, Has.Count.EqualTo(4 + 5 * 4));
                Assert.That(defaultSeparators, Has.Count.EqualTo(1));

                Assert.That(customSkeletonComponent.Instance, Is.Not.Null);

                // Custom: 2 header column skeletons + (3 body rows * 2 column skeletons) = 8
                Assert.That(customSkeletons, Has.Count.EqualTo(2 + 3 * 2));
                Assert.That(customElement.ClassList, Does.Contain("custom-table-skeleton"));
                Assert.That(customElement.GetAttribute("data-testid"), Is.EqualTo("table-skeleton"));

                Assert.That(emptySkeletonComponent.Instance, Is.Not.Null);
                Assert.That(emptySkeletons, Has.Count.EqualTo(0));
            }
        }
    }
}
