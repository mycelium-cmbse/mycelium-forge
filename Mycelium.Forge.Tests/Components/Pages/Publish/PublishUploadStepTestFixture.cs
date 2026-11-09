// ------------------------------------------------------------------------------------------------
// <copyright file="PublishUploadStepTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.Publish
{
    using System;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;
    using Microsoft.AspNetCore.Components.Web;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.Publish.Steps;
    using Mycelium.Forge.Enums;
    using Mycelium.Forge.Models.Publish;
    using Mycelium.Forge.Services;
    using Mycelium.Forge.ViewModels.Publish;

    [TestFixture]
    public class PublishUploadStepTestFixture
    {
        private BunitContext context;
        private Mock<IPublishViewModel> viewModelMock;
        private Mock<IJsInterop> jsInteropMock;
        private Mock<INotificationService> notificationServiceMock;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();
            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            var configurationManager = new ConfigurationManager();
            configurationManager[ConfigurationConstants.MaximumUploadFileSizeInMb] = "200";
            this.context.Services.AddSingleton<IConfiguration>(configurationManager);

            this.jsInteropMock = new Mock<IJsInterop>();
            this.context.Services.AddSingleton(this.jsInteropMock.Object);

            this.notificationServiceMock = new Mock<INotificationService>();
            this.context.Services.AddSingleton(this.notificationServiceMock.Object);

            this.viewModelMock = new Mock<IPublishViewModel>();
            this.viewModelMock.Setup(x => x.ExtractManifest(It.IsAny<PublishArtefactModel>())).ReturnsAsync(true);
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

            this.jsInteropMock.Setup(x => x.UploadLargeFile(It.IsAny<string>(), ApiRoutes.UploadFileForStage)).ReturnsAsync("token-uploaded");

            var uploadStep = this.context.Render<PublishUploadStep>(parameters =>
            {
                parameters.Add(x => x.ViewModel, this.viewModelMock.Object);
                parameters.Add(x => x.OnContinue, onContinue);
            });

            // Case 1: Attempting to continue without a file selected does not proceed
            var continueButton = uploadStep.Find("#publish-continue-button");
            await continueButton.ClickAsync(new MouseEventArgs());

            Assert.That(continued, Is.False);

            // Case 2: Selecting a file and clicking continue stages the file and extracts manifest
            var inputFile = uploadStep.FindComponent<InputFile>();
            inputFile.UploadFiles(InputFileContent.CreateFromText("kpar data", "library.kpar"));

            await continueButton.ClickAsync(new MouseEventArgs());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(continued, Is.True);
                this.jsInteropMock.Verify(x => x.UploadLargeFile("publish-package-file-input", ApiRoutes.UploadFileForStage), Times.Once);
                this.viewModelMock.Verify(x => x.ExtractManifest(It.Is<PublishArtefactModel>(a => a.StagingToken == "token-uploaded")), Times.Once);
            }

            // Case 3: Upload failure displays notification and prevents continuing
            this.jsInteropMock.Setup(x => x.UploadLargeFile(It.IsAny<string>(), ApiRoutes.UploadFileForStage)).ThrowsAsync(new InvalidOperationException("Upload aborted"));
            continued = false;

            await continueButton.ClickAsync(new MouseEventArgs());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(continued, Is.False);
                this.notificationServiceMock.Verify(x => x.AddNotification(It.IsAny<string>(), It.IsAny<string>(), NotificationType.Error), Times.Once);
            }
        }

        [Test]
        public void VerifyHandleFilesSelected()
        {
            var uploadStep = this.context.Render<PublishUploadStep>(parameters => { parameters.Add(x => x.ViewModel, this.viewModelMock.Object); });

            // Case 1: Uploading a .kpar archive automatically identifies the SysML v2 format
            var inputFile = uploadStep.FindComponent<InputFile>();
            inputFile.UploadFiles(InputFileContent.CreateFromText("kpar payload", "archive.kpar"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(uploadStep.Find(".publish-file-name").TextContent, Does.Contain("archive.kpar"));
                Assert.That(uploadStep.Find(".publish-file-format").TextContent, Does.Contain(PackageFormatConstants.SysMlV2));
            }

            // Case 2: Uploading a .zip archive requires selecting model type
            inputFile.UploadFiles(InputFileContent.CreateFromText("zip payload", "archive.zip"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(uploadStep.Find(".publish-file-name").TextContent, Does.Contain("archive.zip"));
                Assert.That(uploadStep.Find(".publish-model-type-toggle"), Is.Not.Null);
            }

            // Case 3: Existing package locks expected format
            this.viewModelMock.Setup(x => x.ExpectedFormat).Returns(SupportedPackageFormat.All[0]);
            var existingPackageStep = this.context.Render<PublishUploadStep>(parameters => { parameters.Add(x => x.ViewModel, this.viewModelMock.Object); });

            var existingInputFile = existingPackageStep.FindComponent<InputFile>();
            existingInputFile.UploadFiles(InputFileContent.CreateFromText("kpar payload", "model.kpar"));

            Assert.That(existingPackageStep.Find(".publish-file-name").TextContent, Does.Contain("model.kpar"));
        }
    }
}
