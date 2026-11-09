// ------------------------------------------------------------------------------------------------
// <copyright file="PublishReviewStepTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.Publish.Steps
{
    using System;
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
    public class PublishReviewStepTestFixture
    {
        private BunitContext context;
        private Mock<IPublishViewModel> viewModelMock;
        private Package package;
        private PackageVersion packageVersion;
        private PackageMetaData packageMetaData;
        private Organization scope;
        private List<ValidationCheckModel> validationChecks;
        private PublishArtefactModel stagedArtefact;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();
            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.package = new Package { Name = "test-pkg", ShortName = "test-pkg", Description = "A test package", License = "MIT", Tags = ["tag1", "tag2"] };
            this.packageVersion = new PackageVersion { Version = "1.0.0" };
            this.packageMetaData = new PackageMetaData { Dependencies = [("dep1", ">=1.0.0", true)] };
            this.scope = new Organization { Id = Guid.NewGuid(), Name = "Org", ShortName = "org" };
            this.validationChecks = [];
            this.stagedArtefact = new PublishArtefactModel { FileName = "pkg.kpar", Format = "SysML v2", SizeInBytes = 1024 };

            this.viewModelMock = new Mock<IPublishViewModel>();
            this.viewModelMock.Setup(x => x.Package).Returns(this.package);
            this.viewModelMock.Setup(x => x.PackageVersion).Returns(this.packageVersion);
            this.viewModelMock.Setup(x => x.PackageMetaData).Returns(this.packageMetaData);
            this.viewModelMock.Setup(x => x.SelectedScope).Returns(this.scope);
            this.viewModelMock.Setup(x => x.ValidationChecks).Returns(this.validationChecks);
            this.viewModelMock.Setup(x => x.StagedArtefact).Returns(this.stagedArtefact);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyDisplayHelpers()
        {
            var renderedStep = this.context.Render<PublishReviewStep>(p => p.Add(x => x.ViewModel, this.viewModelMock.Object));
            var step = renderedStep.Instance;

            // Case 1: Populated fields produce correct display coordinates and summaries
            using (Assert.EnterMultipleScope())
            {
                Assert.That(step.GetScopePackageCoordinate(), Is.EqualTo("@org/test-pkg"));
                Assert.That(step.GetLicenseText(), Is.EqualTo("MIT"));
                Assert.That(step.GetTagsText(), Is.EqualTo("tag1, tag2"));
                Assert.That(step.GetDependenciesText(), Is.EqualTo("dep1 >=1.0.0"));
                Assert.That(step.GetValidationStatusText(), Is.EqualTo("Passed all validation checks"));
            }

            // Case 2: Empty metadata fields fall back to None
            this.package.License = string.Empty;
            this.package.Tags = [];
            this.packageMetaData.Dependencies = [];

            using (Assert.EnterMultipleScope())
            {
                Assert.That(step.GetLicenseText(), Is.EqualTo("None"));
                Assert.That(step.GetTagsText(), Is.EqualTo("None"));
                Assert.That(step.GetDependenciesText(), Is.EqualTo("None"));
            }

            // Case 3: Validation warnings and errors affect review status text
            this.validationChecks.Add(new ValidationCheckModel(ValidationTypes.License, ValidationStatus.Warning, "License warning"));
            Assert.That(step.GetValidationStatusText(), Does.Contain("Passed with 1 warning"));

            this.validationChecks.Add(new ValidationCheckModel(ValidationTypes.License, ValidationStatus.Missing, "License missing"));
            Assert.That(step.GetValidationStatusText(), Does.Contain("Passed with 2 warnings"));

            this.validationChecks.Add(new ValidationCheckModel(ValidationTypes.SchemaValidation, ValidationStatus.Fail, "Schema fail"));
            Assert.That(step.GetValidationStatusText(), Does.Contain("Failed with 1 error"));

            this.validationChecks.Add(new ValidationCheckModel(ValidationTypes.PackageVersion, ValidationStatus.Fail, "Version fail"));
            Assert.That(step.GetValidationStatusText(), Does.Contain("Failed with 2 errors"));
        }

        [Test]
        public async Task VerifyHandlePublish()
        {
            var published = false;
            var onPublish = new EventCallbackFactory().Create(this, () => { published = true; });

            var renderedStep = this.context.Render<PublishReviewStep>(parameters =>
            {
                parameters.Add(x => x.ViewModel, this.viewModelMock.Object);
                parameters.Add(x => x.OnPublish, onPublish);
            });

            // Case 1: Publishing without terms acknowledgment fails validation
            var publishButton = renderedStep.Find("#publish-submit-button");
            await publishButton.ClickAsync(new MouseEventArgs());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(published, Is.False);
                Assert.That(renderedStep.Instance.ValidationManager.HasError(nameof(PublishReviewStep.IsAcknowledged)), Is.True);
            }

            // Case 2: Acknowledging terms permits publishing callback invocation
            renderedStep.Instance.OnAcknowledgmentChanged(true);
            await publishButton.ClickAsync(new MouseEventArgs());

            Assert.That(published, Is.True);

            // Case 3: When already publishing, button click does not invoke callback
            published = false;
            this.viewModelMock.Setup(x => x.IsPublishing).Returns(true);
            await publishButton.ClickAsync(new MouseEventArgs());

            Assert.That(published, Is.False);
        }

        [Test]
        public async Task VerifyNavigationCallbacks()
        {
            var replaced = false;
            var navigatedStep = -1;
            var onReplace = new EventCallbackFactory().Create(this, () => { replaced = true; });
            var onNavigate = new EventCallbackFactory().Create(this, (int step) => { navigatedStep = step; });

            var renderedStep = this.context.Render<PublishReviewStep>(parameters =>
            {
                parameters.Add(x => x.ViewModel, this.viewModelMock.Object);
                parameters.Add(x => x.OnReplaceArtefact, onReplace);
                parameters.Add(x => x.OnNavigateToStep, onNavigate);
            });

            // Case 1: HandleReplaceArtefact triggers OnReplaceArtefact callback
            await renderedStep.Instance.HandleReplaceArtefact();
            Assert.That(replaced, Is.True);

            // Case 2: HandleEditMetadata navigates to metadata step (1)
            await renderedStep.Instance.HandleEditMetadata();
            Assert.That(navigatedStep, Is.EqualTo(1));

            // Case 3: HandleViewValidationDetails navigates to validation step (2)
            await renderedStep.Instance.HandleViewValidationDetails();
            Assert.That(navigatedStep, Is.EqualTo(2));
        }
    }
}
