// ------------------------------------------------------------------------------------------------
// <copyright file="PackageVisibilityOptionCardTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.PackageSettings
{
    using System.Threading.Tasks;

    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Mycelium.Forge.Components.Pages.PackageSettings;

    [TestFixture]
    public class PackageVisibilityOptionCardTestFixture
    {
        private BunitContext context;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyRendering()
        {
            var currentCard = this.context.Render<PackageVisibilityOptionCard>(parameters => parameters
                .Add(x => x.IsCurrent, true)
                .Add(x => x.Title, "Private")
                .Add(x => x.Description, "Only maintainers can see it."));

            var nonCurrentCard = this.context.Render<PackageVisibilityOptionCard>(parameters => parameters
                .Add(x => x.IsCurrent, false)
                .Add(x => x.Title, "Public")
                .Add(x => x.Description, "Anyone can see it."));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(currentCard.Markup, Does.Contain("Private"));
                Assert.That(currentCard.Markup, Does.Contain("Only maintainers can see it."));
                Assert.That(currentCard.Markup, Does.Contain("Current"));

                Assert.That(nonCurrentCard.Markup, Does.Contain("Public"));
                Assert.That(nonCurrentCard.Markup, Does.Contain("Anyone can see it."));
                Assert.That(nonCurrentCard.Markup, Does.Not.Contain("Current"));
            }
        }
    }
}
