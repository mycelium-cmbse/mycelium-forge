// ------------------------------------------------------------------------------------------------
// <copyright file="PublishValidationStepTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.Publish.Steps
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Web;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.Publish.Steps;
    using Mycelium.Forge.Enums;
    using Mycelium.Forge.Models.Publish;
    using Mycelium.Forge.Models.Validation;
    using Mycelium.Forge.ViewModels.Publish;

    [TestFixture]
    public class PublishValidationStepTestFixture
    {
        private BunitContext context;
        private Mock<IPublishViewModel> viewModelMock;
        private List<ValidationCheckModel> validationChecks;
        private PublishArtefactModel stagedArtefact;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();
            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.validationChecks = [];
            this.stagedArtefact = new PublishArtefactModel { StagingToken = "token", IsManifestValid = true };

            this.viewModelMock = new Mock<IPublishViewModel>();
            this.viewModelMock.Setup(x => x.ValidationChecks).Returns(this.validationChecks);
            this.viewModelMock.Setup(x => x.StagedArtefact).Returns(this.stagedArtefact);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public async Task VerifyHandleContinue()
        {
            var continued = false;
            var onContinue = new EventCallbackFactory().Create(this, () => { continued = true; });

            this.validationChecks.Add(new ValidationCheckModel(ValidationTypes.SchemaValidation, ValidationStatus.Fail, "Failure"));

            var renderedStep = this.context.Render<PublishValidationStep>(parameters =>
            {
                parameters.Add(x => x.ViewModel, this.viewModelMock.Object);
                parameters.Add(x => x.OnContinue, onContinue);
            });

            // Case 1: Clicking continue with validation failures does not advance
            var continueButton = renderedStep.Find("#publish-continue-to-review-button");
            await continueButton.ClickAsync(new MouseEventArgs());

            Assert.That(continued, Is.False);

            // Case 2: Resolving failures allows continuing to review
            this.validationChecks.Clear();
            this.validationChecks.Add(new ValidationCheckModel(ValidationTypes.SchemaValidation, ValidationStatus.Pass, "Success"));
            renderedStep.Render();

            await continueButton.ClickAsync(new MouseEventArgs());
            Assert.That(continued, Is.True);
        }

        [Test]
        public void VerifyValidationPropertiesAndFormatting()
        {
            var renderedStep = this.context.Render<PublishValidationStep>(p => p.Add(x => x.ViewModel, this.viewModelMock.Object));
            var step = renderedStep.Instance;

            // Case 1: Initial state with all checks passing
            using (Assert.EnterMultipleScope())
            {
                Assert.That(step.HasFailures, Is.False);
                Assert.That(step.WarningCount, Is.Zero);
                Assert.That(step.GetBackText(), Is.EqualTo("Back to metadata"));
                Assert.That(step.GetFootnoteText(), Is.EqualTo("All checks passed. Ready to proceed."));
                Assert.That(step.GetValidationSubText(), Is.EqualTo("All pre-publish checks completed successfully."));
            }

            // Case 2: Validation warning does not block publishing
            this.validationChecks.Add(new ValidationCheckModel(ValidationTypes.License, ValidationStatus.Warning, "Missing license"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(step.WarningCount, Is.EqualTo(1));
                Assert.That(step.GetWarningsTitle(), Is.EqualTo("1 warning"));
                Assert.That(step.GetFootnoteText(), Does.Contain("do not block publishing"));
                Assert.That(step.GetValidationCountsText(), Does.Contain("1 warning"));
            }

            // Case 3: Multiple warnings title pluralization
            this.validationChecks.Add(new ValidationCheckModel(ValidationTypes.License, ValidationStatus.Warning, "No license"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(step.WarningCount, Is.EqualTo(2));
                Assert.That(step.GetWarningsTitle(), Is.EqualTo("2 warnings"));
            }

            // Case 4: Validation failure blocks progression
            this.validationChecks.Add(new ValidationCheckModel(ValidationTypes.SchemaValidation, ValidationStatus.Fail, "Invalid format"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(step.HasFailures, Is.True);
                Assert.That(step.GetFootnoteText(), Is.EqualTo("Resolve all blocking errors before proceeding."));
                Assert.That(step.GetValidationCountsText(), Does.Contain("1 error"));
                Assert.That(step.GetValidationSubText(), Does.Contain("cannot be published until all blocking errors are resolved"));
            }

            // Case 5: Invalid manifest returns navigation to upload step
            this.stagedArtefact.IsManifestValid = false;
            Assert.That(step.GetBackText(), Is.EqualTo("Back to upload"));
        }
    }
}
