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
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.PackageSettings;
    using Mycelium.Forge.Models.Package;
    using Mycelium.Forge.ViewModels.PackageSettings;

    [TestFixture]
    public class PackageSettingsTestFixture
    {
        private BunitContext context;
        private Mock<IPackageSettingsViewModel> viewModelMock;
        private PackageVersionModel unlistedVersion;
        private PackageVersionModel activeVersion;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IPackageSettingsViewModel>();

            this.activeVersion = new PackageVersionModel { Version = "1.3.0", IsLatest = true, IsUnlisted = false, PublishedAgo = "1 day ago" };
            this.unlistedVersion = new PackageVersionModel { Version = "1.0.0", IsUnlisted = true, PublishedAgo = "1 month ago" };

            var packageModel = new PackageModel(
                new Package { Name = "ECSS-MM-PWR", ShortName = "ecss-mm-pwr", Visibility = VisibilityKind.PUBLIC },
                "Starion Group")
            {
                Maintainers =
                [
                    new PackageMaintainerModel("Alex Rivera", "AR")
                ],
                Versions =
                [
                    this.activeVersion,
                    this.unlistedVersion
                ]
            };

            this.viewModelMock.Setup(x => x.Package).Returns(packageModel);

            this.context.Services.AddSingleton(this.viewModelMock.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyDeprecateVersion()
        {
            var packageSettingsPage = this.context.Render<PackageSettings>();

            packageSettingsPage.Instance.OnDeprecateVersion(this.activeVersion);
            packageSettingsPage.Instance.OnDeprecateVersion(null);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.activeVersion.IsDeprecated, Is.True);
                this.viewModelMock.Verify(x => x.SavePackage(), Times.Once);
            }
        }

        [Test]
        public void VerifyOnParametersSet()
        {
            var packageSettingsPage = this.context.Render<PackageSettings>(parameters => parameters
                .Add(p => p.Organization, "@starion")
                .Add(p => p.PackageName, "ECSS-MM-PWR"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(packageSettingsPage.Instance, Is.Not.Null);
                this.viewModelMock.Verify(x => x.InitializeViewModel("ECSS-MM-PWR", "@starion"), Times.Once);
            }
        }

        [Test]
        public void VerifyOnSelectVisibility()
        {
            var packageSettingsPage = this.context.Render<PackageSettings>();

            packageSettingsPage.Instance.OnSelectVisibility(VisibilityKind.PRIVATE);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModelMock.Object.Package.Visibility, Is.EqualTo(VisibilityKind.PRIVATE));
                this.viewModelMock.Verify(x => x.SavePackage(), Times.Once);
            }
        }

        [Test]
        public void VerifyStubMethods()
        {
            var packageSettingsPage = this.context.Render<PackageSettings>();

            var addMaintainerBtn = packageSettingsPage.Find("#package-settings-add-maintainer-button");
            var maintainerMenuBtn = packageSettingsPage.Find(".package-maintainer-menu-button");
            var transferBtn = packageSettingsPage.Find("#package-settings-transfer-button");

            Assert.That(async () =>
            {
                await packageSettingsPage.InvokeAsync(() => addMaintainerBtn.ClickAsync());
                await packageSettingsPage.InvokeAsync(() => maintainerMenuBtn.ClickAsync());
                await packageSettingsPage.InvokeAsync(() => transferBtn.ClickAsync());
            }, Throws.Nothing);
        }

        [Test]
        public void VerifyUnlistAndRelistVersion()
        {
            var packageSettingsPage = this.context.Render<PackageSettings>();

            packageSettingsPage.Instance.OnUnlistVersion(this.activeVersion);
            packageSettingsPage.Instance.OnRelistVersion(this.unlistedVersion);
            packageSettingsPage.Instance.OnUnlistVersion(null);
            packageSettingsPage.Instance.OnRelistVersion(null);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.activeVersion.IsUnlisted, Is.True);
                Assert.That(this.unlistedVersion.IsUnlisted, Is.False);
                this.viewModelMock.Verify(x => x.SavePackage(), Times.Exactly(2));
            }
        }
    }
}
