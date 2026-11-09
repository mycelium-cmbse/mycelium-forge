// ------------------------------------------------------------------------------------------------
// <copyright file="PublishStepBaseTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.Publish.Steps
{
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Mycelium.Forge.Components.Pages.Publish.Steps;
    using Mycelium.Forge.Models.Publish;

    [TestFixture]
    public class PublishStepBaseTestFixture
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
        public void VerifyRendering()
        {
            var infoCard = new InformationCardModel
            {
                Title = "GUIDE TITLE",
                Items = ["Point 1", "Point 2"],
                HelpText = "Need help? Contact support.",
                LinkText = "Documentation",
                LinkHref = "/docs"
            };

            var stepBase = this.context.Render<PublishStepBase>(parameters =>
            {
                parameters.Add(x => x.InformationCard, infoCard);
                parameters.Add(x => x.Body, builder => builder.AddMarkupContent(0, "<div id=\"custom-body\">Body content</div>"));
                parameters.Add(x => x.Footer, builder => builder.AddMarkupContent(0, "<div id=\"custom-footer\">Footer content</div>"));
            });

            using (Assert.EnterMultipleScope())
            {
                Assert.That(stepBase.Find("#custom-body"), Is.Not.Null);
                Assert.That(stepBase.Find("#custom-footer"), Is.Not.Null);
                Assert.That(stepBase.Markup, Does.Contain("GUIDE TITLE"));
                Assert.That(stepBase.Markup, Does.Contain("Point 1"));
                Assert.That(stepBase.Markup, Does.Contain("Need help? Contact support."));
            }
        }
    }
}
