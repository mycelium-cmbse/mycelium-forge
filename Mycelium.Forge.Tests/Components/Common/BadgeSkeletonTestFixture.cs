// ------------------------------------------------------------------------------------------------
// <copyright file="BadgeSkeletonTestFixture.cs" company="Starion Group S.A.">
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
    /// Test fixture for <see cref="BadgeSkeleton" />.
    /// </summary>
    [TestFixture]
    public class BadgeSkeletonTestFixture
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
        /// Verifies rendering of <see cref="BadgeSkeleton" /> with default and custom parameters.
        /// </summary>
        [Test]
        public void VerifyBadgeSkeletonRendering()
        {
            var defaultSkeletonComponent = this.context.Render<BadgeSkeleton>();
            var defaultItems = defaultSkeletonComponent.FindComponents<BbSkeleton>();

            var customSkeletonComponent = this.context.Render<BadgeSkeleton>(parameters => parameters
                .Add(p => p.Count, 5)
                .Add(p => p.HeightClass, "h-8")
                .Add(p => p.WidthClass, "w-20")
                .Add(p => p.ShapeClass, "rounded-full")
                .Add(p => p.GapClass, "gap-4")
                .Add(p => p.Class, "custom-badge-skeleton")
                .Add(p => p.AdditionalAttributes, new Dictionary<string, object>
                {
                    { "id", "test-badge-skeleton" },
                    { "data-testid", "badge-skeleton" }
                }));

            var customItems = customSkeletonComponent.FindComponents<BbSkeleton>();
            var customElement = customSkeletonComponent.Find("#test-badge-skeleton");

            var emptySkeletonComponent = this.context.Render<BadgeSkeleton>(parameters => parameters.Add(p => p.Count, 0));
            var emptyItems = emptySkeletonComponent.FindComponents<BbSkeleton>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(defaultSkeletonComponent.Instance, Is.Not.Null);
                Assert.That(defaultItems, Has.Count.EqualTo(3));

                Assert.That(customSkeletonComponent.Instance, Is.Not.Null);
                Assert.That(customItems, Has.Count.EqualTo(5));
                Assert.That(customElement.ClassList, Does.Contain("custom-badge-skeleton"));
                Assert.That(customElement.GetAttribute("data-testid"), Is.EqualTo("badge-skeleton"));

                Assert.That(emptySkeletonComponent.Instance, Is.Not.Null);
                Assert.That(emptyItems, Has.Count.EqualTo(0));
            }
        }
    }
}
