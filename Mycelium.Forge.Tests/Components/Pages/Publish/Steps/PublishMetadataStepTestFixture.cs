// ------------------------------------------------------------------------------------------------
// <copyright file="PublishMetadataStepTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.Publish.Steps
{
    using System;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Web;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.Publish.Steps;
    using Mycelium.Forge.Extractors.Enums;
    using Mycelium.Forge.Models.Publish;
    using Mycelium.Forge.ViewModels.Publish;

    [TestFixture]
    public class PublishMetadataStepTestFixture
    {
        private BunitContext context;
        private Mock<IPublishViewModel> viewModelMock;
        private Package package;
        private PackageVersion packageVersion;
        private Organization organizationScope;
        private Account accountScope;
        private PublishArtefactModel stagedArtefact;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();
            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.package = new Package { Name = "test-pkg", ShortName = "test-pkg", Description = "A description" };
            this.packageVersion = new PackageVersion { Version = "1.0.0" };
            this.organizationScope = new Organization { Id = Guid.NewGuid(), Name = "Org", ShortName = "org", DefaultPackageVisibility = VisibilityKind.INTERNAL };
            this.accountScope = new Account { Id = Guid.NewGuid(), Name = "User", ShortName = "user", IsVerified = false, DefaultPackageVisibility = VisibilityKind.PRIVATE };
            this.stagedArtefact = new PublishArtefactModel { Kind = ArtifactKind.Kpar, StagingToken = "token-kpar" };

            this.viewModelMock = new Mock<IPublishViewModel>();
            this.viewModelMock.Setup(x => x.Package).Returns(this.package);
            this.viewModelMock.Setup(x => x.PackageVersion).Returns(this.packageVersion);
            this.viewModelMock.SetupProperty(x => x.SelectedScope, this.organizationScope);
            this.viewModelMock.Setup(x => x.StagedArtefact).Returns(this.stagedArtefact);
            this.viewModelMock.Setup(x => x.ScopeOptions).Returns([this.organizationScope, this.accountScope]);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyChangeHandlers()
        {
            var renderedStep = this.context.Render<PublishMetadataStep>(p => p.Add(x => x.ViewModel, this.viewModelMock.Object));
            var step = renderedStep.Instance;

            // Case 1: Changing scope updates selected scope and synchronizes package defaults
            step.OnScopeChanged(this.accountScope);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModelMock.Object.SelectedScope, Is.EqualTo(this.accountScope));
                Assert.That(this.package.Visibility, Is.EqualTo(VisibilityKind.PRIVATE));
                Assert.That(this.package.IsVerified, Is.False);
            }

            // Case 2: Changing metadata fields updates underlying models
            step.OnPackageNameChanged("New Name");
            step.OnPackageShortNameChanged("new-short-name");
            step.OnVersionChanged("2.3.4");
            step.OnReleaseNotesChanged("Notes");
            step.OnDescriptionChanged("New Description");
            step.OnLicenseChanged("MIT");
            step.OnVisibilityChanged(VisibilityKind.PUBLIC);
            step.OnTagsChanged(["tag1", "tag2"]);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.package.Name, Is.EqualTo("New Name"));
                Assert.That(this.package.ShortName, Is.EqualTo("new-short-name"));
                Assert.That(this.packageVersion.Version, Is.EqualTo("2.3.4"));
                Assert.That(this.packageVersion.ReleaseNotes, Is.EqualTo("Notes"));
                Assert.That(this.package.Description, Is.EqualTo("New Description"));
                Assert.That(this.package.License, Is.EqualTo("MIT"));
                Assert.That(this.package.Visibility, Is.EqualTo(VisibilityKind.PUBLIC));
                Assert.That(this.package.Tags, Has.Count.EqualTo(2));
            }
        }

        [Test]
        public async Task VerifyHandleContinue()
        {
            var continued = false;
            var onContinue = new EventCallbackFactory().Create(this, () => { continued = true; });

            var renderedStep = this.context.Render<PublishMetadataStep>(parameters =>
            {
                parameters.Add(x => x.ViewModel, this.viewModelMock.Object);
                parameters.Add(x => x.OnContinue, onContinue);
                parameters.Add(x => x.OnBack, EventCallback.Empty);
            });

            var continueButton = renderedStep.Find("#publish-continue-to-validation-button");

            // Case 1: Valid initial state continues to validation step
            await continueButton.ClickAsync(new MouseEventArgs());

            Assert.That(continued, Is.True);

            // Case 2: Missing package name prevents continuation
            continued = false;
            this.package.Name = string.Empty;
            await continueButton.ClickAsync(new MouseEventArgs());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(continued, Is.False);
                Assert.That(renderedStep.Instance.ValidationManager.HasError(nameof(IPackage.Name)), Is.True);
            }

            // Case 3: Invalid package short name prevents continuation
            this.package.Name = "Valid Name";
            this.package.ShortName = "Invalid_ShortName";
            await continueButton.ClickAsync(new MouseEventArgs());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(continued, Is.False);
                Assert.That(renderedStep.Instance.ValidationManager.HasError(nameof(IPackage.ShortName)), Is.True);
            }

            // Case 4: Invalid semantic version prevents continuation
            this.package.ShortName = "valid-short-name";
            this.packageVersion.Version = "not-semver";
            await continueButton.ClickAsync(new MouseEventArgs());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(continued, Is.False);
                Assert.That(renderedStep.Instance.ValidationManager.HasError(nameof(IPackageVersion.Version)), Is.True);
            }

            // Case 5: Empty description prevents continuation
            this.packageVersion.Version = "1.0.0";
            this.package.Description = string.Empty;
            await continueButton.ClickAsync(new MouseEventArgs());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(continued, Is.False);
                Assert.That(renderedStep.Instance.ValidationManager.HasError(nameof(IPackage.Description)), Is.True);
            }

            // Case 6: Existing package only validates version number
            this.package.Description = "Valid Description";
            this.viewModelMock.Setup(x => x.IsExistingPackage).Returns(true);
            this.packageVersion.Version = "invalid-ver";

            var existingStep = this.context.Render<PublishMetadataStep>(parameters =>
            {
                parameters.Add(x => x.ViewModel, this.viewModelMock.Object);
                parameters.Add(x => x.OnContinue, onContinue);
                parameters.Add(x => x.OnBack, EventCallback.Empty);
            });

            var existingContinueButton = existingStep.Find("#publish-continue-to-validation-button");
            await existingContinueButton.ClickAsync(new MouseEventArgs());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(continued, Is.False);
                Assert.That(existingStep.Instance.ValidationManager.HasError(nameof(IPackageVersion.Version)), Is.True);
            }

            this.packageVersion.Version = "2.0.0";
            await existingContinueButton.ClickAsync(new MouseEventArgs());
            Assert.That(continued, Is.True);
        }

        [Test]
        public void VerifyPropertiesAndHelpers()
        {
            var renderedStep = this.context.Render<PublishMetadataStep>(p => p.Add(x => x.ViewModel, this.viewModelMock.Object));
            var step = renderedStep.Instance;

            // Case 1: Organization scope verified properties and visibility constraints
            using (Assert.EnterMultipleScope())
            {
                Assert.That(step.IsKpar, Is.True);
                Assert.That(step.IsPackageNameDisabled, Is.True);
                Assert.That(step.IsSelectedScopeVerified, Is.True);
                Assert.That(step.IsScopeOrganization, Is.True);
                Assert.That(step.GetScopePackageCoordinate(), Is.EqualTo("@org/test-pkg"));
                Assert.That(step.IsVisibilityItemDisabled(VisibilityKind.INTERNAL), Is.False);
            }

            // Case 2: Account scope unverified properties and internal visibility disabled
            this.viewModelMock.Setup(x => x.SelectedScope).Returns(this.accountScope);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(step.IsSelectedScopeVerified, Is.False);
                Assert.That(step.IsScopeOrganization, Is.False);
                Assert.That(step.IsVisibilityItemDisabled(VisibilityKind.INTERNAL), Is.True);
                Assert.That(step.IsVisibilityItemDisabled(VisibilityKind.PUBLIC), Is.False);
            }
        }
    }
}
