// ------------------------------------------------------------------------------------------------
// <copyright file="PackageSettingsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.PackageSettings
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.PackageSettings;
    using Mycelium.Forge.ViewModels.PackageSettings;

    [TestFixture]
    public class PackageSettingsTestFixture
    {
        private BunitContext context;
        private Mock<IPackageSettingsViewModel> viewModelMock;
        private DialogService dialogService;
        private Package testPackage;
        private Organization testOwner;
        private Account ownerAccount;
        private Account maintainerAccount;
        private PackageVersion activeVersion;
        private PackageVersion unlistedVersion;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IPackageSettingsViewModel>();

            var ownerId = Guid.NewGuid();
            var packageId = Guid.NewGuid();
            var ownerAccountId = Guid.NewGuid();
            var maintainerAccountId = Guid.NewGuid();

            this.activeVersion = new PackageVersion
            {
                Id = Guid.NewGuid(),
                Version = "1.3.0",
                Owner = packageId,
                Listed = true,
                IsDeprecated = false,
                PublicationDate = DateTime.UtcNow.AddDays(-1)
            };

            this.unlistedVersion = new PackageVersion
            {
                Id = Guid.NewGuid(),
                Version = "1.0.0",
                Owner = packageId,
                Listed = false,
                IsDeprecated = false,
                PublicationDate = DateTime.UtcNow.AddMonths(-1)
            };

            this.testOwner = new Organization
            {
                Id = ownerId,
                Name = "Starion Group",
                ShortName = "starion"
            };

            this.testPackage = new Package
            {
                Id = packageId,
                Name = "ECSS-MM-PWR",
                ShortName = "ecss-mm-pwr",
                Visibility = VisibilityKind.PUBLIC,
                Owner = ownerId,
                PackageOwner = [ownerAccountId],
                PackageMaintainer = [maintainerAccountId],
                Version = [this.activeVersion.Id, this.unlistedVersion.Id]
            };

            this.ownerAccount = new Account
            {
                Id = ownerAccountId,
                Name = "Alex Rivera",
                IsVerified = true
            };

            this.maintainerAccount = new Account
            {
                Id = maintainerAccountId,
                Name = "Sam Developer"
            };

            this.viewModelMock.Setup(x => x.Package).Returns(this.testPackage);
            this.viewModelMock.Setup(x => x.Owner).Returns(this.testOwner);
            this.viewModelMock.Setup(x => x.Owners).Returns([this.ownerAccount]);
            this.viewModelMock.Setup(x => x.Maintainers).Returns([this.maintainerAccount]);
            this.viewModelMock.Setup(x => x.Versions).Returns([this.activeVersion, this.unlistedVersion]);
            this.viewModelMock.Setup(x => x.CanManagePackage).Returns(true);
            this.viewModelMock.Setup(x => x.CanDeletePackage).Returns(true);

            this.context.Services.AddSingleton(this.viewModelMock.Object);
            this.dialogService = this.context.Services.GetRequiredService<DialogService>();
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public async Task VerifyDeprecateVersion()
        {
            var packageSettingsPage = this.context.Render<PackageSettings>();
            var deprecateBtn = packageSettingsPage.Find(".package-deprecate-version-button");

            await packageSettingsPage.InvokeAsync(() => deprecateBtn.ClickAsync());

            this.viewModelMock.Verify(x => x.DeprecateVersion(this.activeVersion, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void VerifyGetBreadcrumbItems()
        {
            var packageSettingsPage = this.context.Render<PackageSettings>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(packageSettingsPage.Markup, Does.Contain("@starion"));
                Assert.That(packageSettingsPage.Markup, Does.Contain("ECSS-MM-PWR"));
                Assert.That(packageSettingsPage.Markup, Does.Contain("Settings"));
            }
        }

        [Test]
        public void VerifyOnDeletePackage()
        {
            var packageSettingsPage = this.context.Render<PackageSettings>();
            var deleteButton = packageSettingsPage.Find("#package-settings-delete-button");

            _ = packageSettingsPage.InvokeAsync(() => deleteButton.ClickAsync());

            Assert.That(this.dialogService.Dialogs, Has.Count.EqualTo(1));
        }

        [Test]
        public void VerifyOnParametersSet()
        {
            var packageSettingsPage = this.context.Render<PackageSettings>(parameters => parameters
                .Add(p => p.Scope, "@starion")
                .Add(p => p.PackageName, "ECSS-MM-PWR"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(packageSettingsPage.Instance, Is.Not.Null);
                this.viewModelMock.Verify(x => x.InitializeViewModel("ECSS-MM-PWR", "@starion"), Times.Once);
            }
        }

        [Test]
        public async Task VerifyOnSelectVisibility()
        {
            var packageSettingsPage = this.context.Render<PackageSettings>();
            var radioGroup = packageSettingsPage.FindComponent<BlazorBlueprint.Primitives.RadioGroup.BbRadioGroup<VisibilityKind>>();

            await packageSettingsPage.InvokeAsync(() => radioGroup.Instance.ValueChanged.InvokeAsync(VisibilityKind.PRIVATE));

            this.viewModelMock.Verify(x => x.SetVisibility(VisibilityKind.PRIVATE, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void VerifyOnTransferOwnership()
        {
            var packageSettingsPage = this.context.Render<PackageSettings>();
            var transferButton = packageSettingsPage.Find("#package-settings-transfer-button");

            _ = packageSettingsPage.InvokeAsync(() => transferButton.ClickAsync());

            Assert.That(this.dialogService.Dialogs, Has.Count.EqualTo(1));
        }

        [Test]
        public void VerifyRenderingWhenNullOrDeprecatedOrInternal()
        {
            this.viewModelMock.Setup(x => x.Package).Returns((IPackage)null!);
            var nullPage = this.context.Render<PackageSettings>();

            this.viewModelMock.Setup(x => x.Package).Returns(this.testPackage);
            this.viewModelMock.Setup(x => x.CanDeletePackage).Returns(false);
            var customPage = this.context.Render<PackageSettings>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(nullPage.Markup.Trim(), Has.Length.LessThan(5));
                Assert.That(customPage.Markup, Does.Contain("Deleting a package"));
                Assert.That(customPage.Markup, Does.Contain("Organization"));
            }
        }

        [Test]
        public void VerifyStubMethods()
        {
            var packageSettingsPage = this.context.Render<PackageSettings>();

            var addMaintainerBtn = packageSettingsPage.Find("#package-settings-add-maintainer-button");
            var maintainerMenuBtn = packageSettingsPage.Find(".package-maintainer-menu-button");

            Assert.That(async () =>
            {
                await packageSettingsPage.InvokeAsync(() => addMaintainerBtn.ClickAsync());
                await packageSettingsPage.InvokeAsync(() => maintainerMenuBtn.ClickAsync());
            }, Throws.Nothing);
        }

        [Test]
        public async Task VerifyUnlistAndRelistVersion()
        {
            var packageSettingsPage = this.context.Render<PackageSettings>();
            var unlistBtn = packageSettingsPage.Find(".package-unlist-version-button");
            var relistBtn = packageSettingsPage.Find(".package-relist-version-button");

            _ = packageSettingsPage.InvokeAsync(() => unlistBtn.ClickAsync());
            await packageSettingsPage.InvokeAsync(() => relistBtn.ClickAsync());

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.dialogService.Dialogs, Has.Count.EqualTo(1));
                this.viewModelMock.Verify(x => x.RelistVersion(this.unlistedVersion, It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        [Test]
        public void VerifyVersionRendering()
        {
            var packageSettingsPage = this.context.Render<PackageSettings>();
            var badges = packageSettingsPage.FindAll(".badge-version-state");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(badges, Has.Count.EqualTo(2));
                Assert.That(badges[0].TextContent.Trim(), Is.EqualTo("Latest"));
                Assert.That(badges[1].TextContent.Trim(), Is.EqualTo("Unlisted"));
            }
        }
    }
}
