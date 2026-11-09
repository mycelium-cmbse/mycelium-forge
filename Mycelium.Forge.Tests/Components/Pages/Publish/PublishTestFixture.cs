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
    using System.Threading;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using ErrorOr;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.Publish;
    using Mycelium.Forge.Enums;
    using Mycelium.Forge.Models.Publish;
    using Mycelium.Forge.Services;
    using Mycelium.Forge.ViewModels.Publish;

    [TestFixture]
    public class PublishTestFixture
    {
        private BunitContext context;
        private Mock<IPublishViewModel> viewModelMock;
        private DialogService dialogService;
        private Mock<INotificationService> notificationServiceMock;
        private PublishArtefactModel stagedArtefact;

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
            this.context.Services.AddSingleton(new Mock<IJsInterop>().Object);

            this.stagedArtefact = new PublishArtefactModel { StagingToken = "token-1", IsManifestValid = true };

            this.viewModelMock = new Mock<IPublishViewModel>();
            this.viewModelMock.Setup(x => x.SelectedScope).Returns(new Organization { ShortName = "starion" });
            this.viewModelMock.Setup(x => x.Package).Returns(new Package { ShortName = "my-pkg" });
            this.viewModelMock.Setup(x => x.PackageVersion).Returns(new PackageVersion { Version = "1.0.0" });
            this.viewModelMock.Setup(x => x.PackageMetaData).Returns(new PackageMetaData());
            this.viewModelMock.Setup(x => x.StagedArtefact).Returns(this.stagedArtefact);
            this.viewModelMock.Setup(x => x.ValidationChecks).Returns([]);

            this.context.Services.AddSingleton(this.viewModelMock.Object);

            this.notificationServiceMock = new Mock<INotificationService>();
            this.context.Services.AddSingleton(this.notificationServiceMock.Object);

            this.dialogService = this.context.Services.GetRequiredService<DialogService>();
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyIsUnauthorizedExistingPackage()
        {
            var publishPage = this.context.Render<Publish>();

            // Case 1: New package is never unauthorized
            this.viewModelMock.Setup(x => x.IsExistingPackage).Returns(false);
            Assert.That(publishPage.Instance.IsUnauthorizedExistingPackage, Is.False);

            // Case 2: Existing package where user cannot release is unauthorized
            this.viewModelMock.Setup(x => x.IsExistingPackage).Returns(true);
            this.viewModelMock.Setup(x => x.CanUserReleaseExistingPackageVersion).Returns(false);
            Assert.That(publishPage.Instance.IsUnauthorizedExistingPackage, Is.True);

            // Case 3: Existing package where user can release is authorized
            this.viewModelMock.Setup(x => x.CanUserReleaseExistingPackageVersion).Returns(true);
            Assert.That(publishPage.Instance.IsUnauthorizedExistingPackage, Is.False);

            // Case 4: Null package reference is unauthorized
            this.viewModelMock.Setup(x => x.Package).Returns((IPackage)null!);
            Assert.That(publishPage.Instance.IsUnauthorizedExistingPackage, Is.True);
        }

        [Test]
        public void VerifyOnBack()
        {
            var publishPage = this.context.Render<Publish>();

            // Case 1: Stepping back from review (3) goes to validation (2)
            publishPage.Instance.ActiveStep = 3;
            publishPage.Instance.OnBack();
            Assert.That(publishPage.Instance.ActiveStep, Is.EqualTo(2));

            // Case 2: Stepping back with valid manifest goes to metadata (1)
            this.stagedArtefact.IsManifestValid = true;
            publishPage.Instance.OnBack();
            Assert.That(publishPage.Instance.ActiveStep, Is.EqualTo(1));

            // Case 3: Stepping back from metadata goes to upload (0)
            publishPage.Instance.OnBack();
            Assert.That(publishPage.Instance.ActiveStep, Is.Zero);

            // Case 4: Stepping back from upload remains at 0
            publishPage.Instance.OnBack();
            Assert.That(publishPage.Instance.ActiveStep, Is.Zero);

            // Case 5: Stepping back from validation with invalid manifest skips metadata back to upload (0)
            this.stagedArtefact.IsManifestValid = false;
            publishPage.Instance.ActiveStep = 2;
            publishPage.Instance.OnBack();
            Assert.That(publishPage.Instance.ActiveStep, Is.Zero);
        }

        [Test]
        public void VerifyOnInitializedAsync()
        {
            // Case 1: Parameterless route initializes default view model
            var publishPage = this.context.Render<Publish>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(publishPage.Instance, Is.Not.Null);
                this.viewModelMock.Verify(x => x.InitializeViewModel(), Times.Once);
            }

            // Case 2: Parameterized scope and package route initializes existing package view model
            var parameterizedPage = this.context.Render<Publish>(parameters =>
            {
                parameters.Add(x => x.Scope, "starion");
                parameters.Add(x => x.Package, "my-pkg");
            });

            using (Assert.EnterMultipleScope())
            {
                Assert.That(parameterizedPage.Instance, Is.Not.Null);
                this.viewModelMock.Verify(x => x.InitializeViewModel("starion", "my-pkg"), Times.Once);
            }
        }

        [Test]
        public async Task VerifyOnNavigateToStep()
        {
            var publishPage = this.context.Render<Publish>();

            // Case 1: Navigate to metadata step (1)
            await publishPage.Instance.OnNavigateToStep(1);
            Assert.That(publishPage.Instance.ActiveStep, Is.EqualTo(1));

            // Case 2: Navigate to validation step (2) triggers Validate()
            await publishPage.Instance.OnNavigateToStep(2);

            using (Assert.EnterMultipleScope())
            {
                this.viewModelMock.Verify(x => x.Validate(null), Times.Once);
                Assert.That(publishPage.Instance.ActiveStep, Is.EqualTo(2));
            }
        }

        [Test]
        public async Task VerifyOnNext()
        {
            var publishPage = this.context.Render<Publish>();

            // Case 1: Advancing from upload (step 0) to metadata (step 1)
            publishPage.Instance.ActiveStep = 0;
            await publishPage.Instance.OnNext();
            Assert.That(publishPage.Instance.ActiveStep, Is.EqualTo(1));

            // Case 2: Advancing from metadata (step 1) to validation (step 2) triggers Validate()
            await publishPage.Instance.OnNext();

            using (Assert.EnterMultipleScope())
            {
                this.viewModelMock.Verify(x => x.Validate(null), Times.Once);
                Assert.That(publishPage.Instance.ActiveStep, Is.EqualTo(2));
            }

            // Case 3: Advancing from validation (step 2) to review (step 3)
            await publishPage.Instance.OnNext();
            Assert.That(publishPage.Instance.ActiveStep, Is.EqualTo(3));

            // Case 4: Skipping metadata when manifest is invalid moves directly to validation
            this.stagedArtefact.IsManifestValid = false;
            publishPage.Instance.ActiveStep = 0;
            await publishPage.Instance.OnNext();
            Assert.That(publishPage.Instance.ActiveStep, Is.EqualTo(2));
        }

        [Test]
        public void VerifyOnReplaceArtefact()
        {
            var publishPage = this.context.Render<Publish>();
            publishPage.Instance.ActiveStep = 2;

            publishPage.Instance.OnReplaceArtefact();

            using (Assert.EnterMultipleScope())
            {
                this.viewModelMock.Verify(x => x.DiscardStagedFile(), Times.Once);
                Assert.That(publishPage.Instance.ActiveStep, Is.Zero);
            }
        }

        [Test]
        public async Task VerifyPublishPackage()
        {
            // Case 1: Successful publication opens the PublishedToForge dialog
            this.viewModelMock.Setup(x => x.Publish(It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

            var publishPage = this.context.Render<Publish>();
            _ = publishPage.Instance.PublishPackage();

            Assert.That(this.dialogService.Dialogs, Has.Count.EqualTo(1));

            // Case 2: Publication failure displays error notification
            this.viewModelMock.Setup(x => x.Publish(It.IsAny<CancellationToken>())).ReturnsAsync(Error.Failure(description: "Publish failed"));
            await publishPage.Instance.PublishPackage();

            this.notificationServiceMock.Verify(x => x.AddNotification(It.IsAny<string>(), It.IsAny<string>(), NotificationType.Error), Times.Once);
        }
    }
}
