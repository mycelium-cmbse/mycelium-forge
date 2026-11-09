// ------------------------------------------------------------------------------------------------
// <copyright file="PublishStepFooterTestFixture.cs" company="Starion Group S.A.">
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

    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Web;

    using Mycelium.Forge.Components.Pages.Publish.Steps;

    [TestFixture]
    public class PublishStepFooterTestFixture
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
        public async Task VerifyButtonsAndCallbacks()
        {
            var backed = false;
            var continued = false;
            var onBack = new EventCallbackFactory().Create(this, () => { backed = true; });
            var onContinue = new EventCallbackFactory().Create(this, () => { continued = true; });

            // Case 1: Custom back and continue buttons render and invoke callbacks when clicked
            var footer = this.context.Render<PublishStepFooter>(parameters =>
            {
                parameters.Add(x => x.FootnoteText, "Sample footnote text");
                parameters.Add(x => x.BackText, "Go Back");
                parameters.Add(x => x.BackId, "btn-back");
                parameters.Add(x => x.ContinueText, "Go Forward");
                parameters.Add(x => x.ContinueId, "btn-continue");
                parameters.Add(x => x.OnBack, onBack);
                parameters.Add(x => x.OnContinue, onContinue);
            });

            var backButton = footer.Find("#btn-back");
            var continueButton = footer.Find("#btn-continue");

            await backButton.ClickAsync(new MouseEventArgs());
            await continueButton.ClickAsync(new MouseEventArgs());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(backed, Is.True);
                Assert.That(continued, Is.True);
            }

            // Case 2: Disabled continue button sets disabled attribute
            var disabledFooter = this.context.Render<PublishStepFooter>(parameters =>
            {
                parameters.Add(x => x.ContinueId, "btn-disabled");
                parameters.Add(x => x.IsContinueDisabled, true);
                parameters.Add(x => x.OnContinue, onContinue);
            });

            var disabledButton = disabledFooter.Find("#btn-disabled");
            Assert.That(disabledButton.HasAttribute("disabled"), Is.True);
        }
    }
}
