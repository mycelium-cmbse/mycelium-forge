// ------------------------------------------------------------------------------------------------
// <copyright file="PublishTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.Publish
{
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using FluentResults;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.Publish;
    using Mycelium.Forge.Models.Publish;
    using Mycelium.Forge.Models.Validation;
    using Mycelium.Forge.ViewModels.Publish;

    [TestFixture]
    public class PublishTestFixture
    {
        private BunitContext context;
        private Mock<IPublishViewModel> viewModelMock;
        private DialogService dialogService;
        private ToastService toastService;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IPublishViewModel>();

            this.viewModelMock.Setup(x => x.Metadata).Returns(new PublishPackageMetadataModel
            {
                Scope = "@starion",
                PackageName = "ECSS-MM-PWR",
                Version = "1.3.0",
                IsScopeVerified = true,
                ArtefactFileName = "ECSS-MM-PWR-1.3.0.kpar",
                ArtefactFormat = "SysML v2 (kpar)",
                License = "Apache-2.0",
                Visibility = VisibilityKind.PUBLIC,
                Metamodel = "SysML v2 (2025-02)",
                Tags = "mission-model, power",
                Description = "ECSS mission model: Power subsystem."
            });

            this.viewModelMock.Setup(x => x.Steps).Returns(
            [
                new PublishStepModel(1, "Package metadata", true),
                new PublishStepModel(2, "Validation"),
                new PublishStepModel(3, "Review & publish")
            ]);

            this.viewModelMock.Setup(x => x.ValidationChecks).Returns(
            [
                new ValidationCheckModel("Schema validation"),
                new ValidationCheckModel("License file", status: ValidationStatus.Warning)
            ]);

            this.viewModelMock.Setup(x => x.ScopeOptions).Returns(["@starion", "@esa"]);
            this.viewModelMock.Setup(x => x.LicenseOptions).Returns(["Apache-2.0", "MIT"]);
            this.viewModelMock.Setup(x => x.VisibilityOptions).Returns([VisibilityKind.PUBLIC, VisibilityKind.PRIVATE]);
            this.viewModelMock.Setup(x => x.MetamodelOptions).Returns(["SysML v2 (2025-02)"]);

            this.context.Services.AddSingleton(this.viewModelMock.Object);

            // Use the dialog and toast services injected by blazorblueprint helper methods
            this.dialogService = this.context.Services.GetRequiredService<DialogService>();
            this.toastService = this.context.Services.GetRequiredService<ToastService>();
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyOnInitialized()
        {
            var publishPage = this.context.Render<Publish>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(publishPage.Instance, Is.Not.Null);
                this.viewModelMock.Verify(x => x.InitializeViewModel(), Times.Once);
            }
        }

        [Test]
        public void VerifyOnMetadataChanged()
        {
            var publishPage = this.context.Render<Publish>();
            var metadata = publishPage.Instance.ViewModel.Metadata;

            publishPage.Instance.OnScopeChanged("@esa");
            publishPage.Instance.OnPackageNameChanged("my-package");
            publishPage.Instance.OnVersionChanged("2.0.0");
            publishPage.Instance.OnDescriptionChanged("A new description.");
            publishPage.Instance.OnLicenseChanged("MIT");
            publishPage.Instance.OnVisibilityChanged(VisibilityKind.PRIVATE);
            publishPage.Instance.OnMetamodelChanged("SysML v1.6");
            publishPage.Instance.OnTagsChanged("power, thermal");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(metadata.Scope, Is.EqualTo("@esa"));
                Assert.That(metadata.PackageName, Is.EqualTo("my-package"));
                Assert.That(metadata.Version, Is.EqualTo("2.0.0"));
                Assert.That(metadata.Description, Is.EqualTo("A new description."));
                Assert.That(metadata.License, Is.EqualTo("MIT"));
                Assert.That(metadata.Visibility, Is.EqualTo(VisibilityKind.PRIVATE));
                Assert.That(metadata.Metamodel, Is.EqualTo("SysML v1.6"));
                Assert.That(metadata.Tags, Is.EqualTo("power, thermal"));
            }

            publishPage.Instance.OnScopeChanged(null);
            publishPage.Instance.OnPackageNameChanged(null);
            publishPage.Instance.OnVersionChanged(null);
            publishPage.Instance.OnDescriptionChanged(null);
            publishPage.Instance.OnLicenseChanged(null);
            publishPage.Instance.OnMetamodelChanged(null);
            publishPage.Instance.OnTagsChanged(null);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(metadata.Scope, Is.EqualTo(string.Empty));
                Assert.That(metadata.PackageName, Is.EqualTo(string.Empty));
                Assert.That(metadata.Version, Is.EqualTo(string.Empty));
                Assert.That(metadata.Description, Is.EqualTo(string.Empty));
                Assert.That(metadata.License, Is.EqualTo(string.Empty));
                Assert.That(metadata.Metamodel, Is.EqualTo(string.Empty));
                Assert.That(metadata.Tags, Is.EqualTo(string.Empty));
            }
        }

        [Test]
        public void VerifyOnReplaceArtefact()
        {
            var publishPage = this.context.Render<Publish>();
            publishPage.Instance.OnReplaceArtefact();
            this.viewModelMock.Verify(x => x.InitializeViewModel(), Times.Exactly(2));
        }

        [Test]
        public async Task VerifyPublishPackage()
        {
            this.viewModelMock.Setup(x => x.Publish()).Returns(Result.Ok());

            var publishPage = this.context.Render<Publish>();

            var publishButton = publishPage.Find("#publish-submit-button");
            Assert.That(publishButton, Is.Not.Null);

            _ = publishPage.InvokeAsync(() => publishButton.ClickAsync());
            Assert.That(this.dialogService.Dialogs, Has.Count.EqualTo(1));

            var failedResult = Result.Fail("Package already exists.");
            this.viewModelMock.Setup(x => x.Publish()).Returns(failedResult);

            await publishPage.InvokeAsync(() => publishButton.ClickAsync());
            Assert.That(this.toastService.Toasts, Has.Count.EqualTo(1));
        }
    }
}
