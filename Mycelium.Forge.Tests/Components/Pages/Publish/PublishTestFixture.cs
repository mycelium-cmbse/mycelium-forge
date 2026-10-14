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

    using ErrorOr;

    using Microsoft.AspNetCore.Components.Forms;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.Publish;
    using Mycelium.Forge.Enums;
    using Mycelium.Forge.Models.Publish;
    using Mycelium.Forge.Models.Validation;
    using Mycelium.Forge.Services;
    using Mycelium.Forge.ViewModels.Publish;

    [TestFixture]
    public class PublishTestFixture
    {
        private BunitContext context;
        private Mock<IPublishViewModel> viewModelMock;
        private Mock<IFileUploadService> fileUploadServiceMock;
        private DialogService dialogService;
        private NotificationService notificationService;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            var configurationManager = new ConfigurationManager();
            configurationManager["MaximumUploadFileSizeInMB"] = "100";
            this.context.Services.AddSingleton<IConfiguration>(configurationManager);

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

            this.fileUploadServiceMock = new Mock<IFileUploadService>();
            this.context.Services.AddSingleton(this.fileUploadServiceMock.Object);

            this.notificationService = new NotificationService();
            this.context.Services.AddSingleton<INotificationService>(this.notificationService);

            this.dialogService = this.context.Services.GetRequiredService<DialogService>();
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyCanOpenStep()
        {
            var publishPage = this.context.Render<Publish>();
            var steps = publishPage.FindComponents<BbStep>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(steps[0].Instance.Disabled, Is.False);
                Assert.That(steps[1].Instance.Disabled, Is.True);
                Assert.That(steps[2].Instance.Disabled, Is.True);
                Assert.That(steps[3].Instance.Disabled, Is.True);
            }

            var fileMock = new Mock<IBrowserFile>();
            fileMock.Setup(x => x.Name).Returns("test.kpar");
            publishPage.Instance.OnUploadStepContinued(fileMock.Object);
            publishPage.Render();

            steps = publishPage.FindComponents<BbStep>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(steps[0].Instance.Disabled, Is.False);
                Assert.That(steps[1].Instance.Disabled, Is.False);
                Assert.That(steps[2].Instance.Disabled, Is.True);
                Assert.That(steps[3].Instance.Disabled, Is.True);
            }
        }

        [Test]
        public void VerifyGetStepClass()
        {
            var publishPage = this.context.Render<Publish>();

            var activeClass = publishPage.Instance.GetStepClass(0);
            var inactiveClass = publishPage.Instance.GetStepClass(1);
            var customActiveClass = publishPage.Instance.GetStepClass(0, "custom-class");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(activeClass, Is.EqualTo("w-full"));
                Assert.That(inactiveClass, Is.EqualTo("hidden"));
                Assert.That(customActiveClass, Is.EqualTo("custom-class"));
            }
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
            publishPage.Instance.ActiveStep = 1;
            publishPage.Instance.OnReplaceArtefact();

            using (Assert.EnterMultipleScope())
            {
                this.viewModelMock.Verify(x => x.InitializeViewModel(), Times.Exactly(2));
                Assert.That(publishPage.Instance.ActiveStep, Is.Zero);
            }
        }

        [Test]
        public void VerifyOnUploadStepContinued()
        {
            var publishPage = this.context.Render<Publish>();
            var fileMock = new Mock<IBrowserFile>();
            fileMock.Setup(x => x.Name).Returns("test-file.kpar");

            publishPage.Instance.OnUploadStepContinued(fileMock.Object);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(publishPage.Instance.ViewModel.Metadata.ArtefactFileName, Is.EqualTo("test-file.kpar"));
                Assert.That(publishPage.Instance.ActiveStep, Is.EqualTo(1));
            }
        }

        [Test]
        public async Task VerifyPublishPackage()
        {
            this.viewModelMock.Setup(x => x.Publish()).Returns(Result.Success);

            var publishPage = this.context.Render<Publish>();
            publishPage.Instance.ActiveStep = 1;
            publishPage.Render();

            var publishButton = publishPage.Find("#publish-submit-button");
            Assert.That(publishButton, Is.Not.Null);

            _ = publishPage.InvokeAsync(() => publishButton.ClickAsync());
            Assert.That(this.dialogService.Dialogs, Has.Count.EqualTo(1));

            var failedResult = Error.Failure(description: "Package already exists.");
            this.viewModelMock.Setup(x => x.Publish()).Returns(failedResult);

            await publishPage.InvokeAsync(() => publishButton.ClickAsync());
            Assert.That(this.notificationService.Results.Items, Has.Count.EqualTo(1));
        }
    }
}
