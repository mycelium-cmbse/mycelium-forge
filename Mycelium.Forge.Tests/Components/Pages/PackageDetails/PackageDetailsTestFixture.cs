// ------------------------------------------------------------------------------------------------
// <copyright file="PackageDetailsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.PackageDetails
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.PackageDetails;
    using Mycelium.Forge.Services;
    using Mycelium.Forge.Tests.TestHelpers;
    using Mycelium.Forge.ViewModels.PackageDetails;

    [TestFixture]
    public class PackageDetailsTestFixture
    {
        private BunitContext context;
        private Mock<IPackageDetailsViewModel> viewModelMock;
        private Mock<IPackageDetailsActionsViewModel> actionsViewModelMock;
        private Package package;
        private Organization organization;
        private PackageVersion packageVersion;
        private PackageMetaData packageMetaData;
        private PackageType packageType;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IPackageDetailsViewModel>();
            this.actionsViewModelMock = new Mock<IPackageDetailsActionsViewModel>();

            this.organization = new Organization
            {
                Id = Guid.NewGuid(),
                Name = "Starion Group",
                ShortName = "starion"
            };

            this.package = new Package
            {
                Id = Guid.NewGuid(),
                Name = "ECSS-MM-PWR",
                ShortName = "ecss-mm-pwr",
                Description = "Power subsystem metamodel",
                License = "Apache-2.0",
                Owner = this.organization.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-30)
            };

            this.packageMetaData = new PackageMetaData
            {
                Id = Guid.NewGuid(),
                Owner = Guid.NewGuid(),
                MetadataSource = MetadataSource.DeclaredByArtefact
            };

            this.packageVersion = new PackageVersion
            {
                Id = Guid.NewGuid(),
                Owner = this.package.Id,
                Version = "1.3.0",
                PublicationDate = DateTime.UtcNow.AddDays(-5),
                Listed = true,
                MetaData = this.packageMetaData.Id
            };

            this.packageMetaData.Owner = this.packageVersion.Id;

            this.packageType = new PackageType
            {
                Id = Guid.NewGuid(),
                Name = "SysML v2"
            };

            this.viewModelMock.SetupGet(x => x.Package).Returns(this.package);
            this.viewModelMock.SetupGet(x => x.Owner).Returns(this.organization);
            this.viewModelMock.SetupGet(x => x.PackageType).Returns(this.packageType);
            this.viewModelMock.SetupGet(x => x.CurrentVersion).Returns(this.packageVersion);
            this.viewModelMock.SetupGet(x => x.Versions).Returns([this.packageVersion]);
            this.viewModelMock.SetupGet(x => x.Maintainers).Returns([]);
            this.viewModelMock.SetupGet(x => x.MetaDatas).Returns([this.packageMetaData]);
            this.viewModelMock.SetupGet(x => x.CurrentMetaData).Returns(this.packageMetaData);
            this.viewModelMock.SetupGet(x => x.Elements).Returns([]);
            this.viewModelMock.SetupGet(x => x.Dependencies).Returns([]);
            this.viewModelMock.SetupGet(x => x.Dependents).Returns([]);

            this.viewModelMock.SetupGet(x => x.QualityChecks).Returns(
            [
                ("Schema check", "Passed", true),
                ("Dependency check", "Passed", true)
            ]);

            this.SetupAccessFlags(true, false);

            this.context.Services.AddSingleton(this.viewModelMock.Object);
            this.context.Services.AddSingleton(this.actionsViewModelMock.Object);
            this.context.Services.AddSingleton(new Mock<IJsInterop>().Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyGetContentTabClass()
        {
            var packageDetailsPage = this.context.Render<PackageDetails>();

            packageDetailsPage.Instance.SelectedContentTab = PackageTabConstants.Overview;
            var activeClass = packageDetailsPage.Instance.GetContentTabClass(PackageTabConstants.Overview);
            var inactiveClass = packageDetailsPage.Instance.GetContentTabClass(PackageTabConstants.Contents);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(activeClass, Does.Contain("bg-primary/10"));
                Assert.That(activeClass, Does.Contain("text-primary font-semibold"));
                Assert.That(inactiveClass, Does.Contain("text-muted-foreground"));
            }
        }

        [Test]
        public void VerifyGetPackageUrl()
        {
            var packageDetailsPage = this.context.Render<PackageDetails>();
            var purl = packageDetailsPage.Instance.GetPackageUrl();

            Assert.That(purl, Is.EqualTo("pkg:forge/starion/ecss-mm-pwr@1.3.0"));
        }

        [Test]
        public void VerifyGetPublishedAgo()
        {
            var packageDetailsPage = this.context.Render<PackageDetails>();
            var publishedAgo = packageDetailsPage.Instance.GetPublishedAgo();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(publishedAgo, Is.Not.Null);
                Assert.That(publishedAgo, Is.Not.Empty);
            }
        }

        [Test]
        public void VerifyGetQualityScore()
        {
            var packageDetailsPage = this.context.Render<PackageDetails>();
            var score = packageDetailsPage.Instance.GetQualityScore();

            this.viewModelMock.SetupGet(x => x.QualityChecks).Returns([]);
            var emptyScore = packageDetailsPage.Instance.GetQualityScore();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(score, Is.EqualTo("2/2 checks"));
                Assert.That(emptyScore, Is.Empty);
            }
        }

        [Test]
        public void VerifyGetReleaseStatus()
        {
            var packageDetailsPage = this.context.Render<PackageDetails>();
            var status = packageDetailsPage.Instance.GetReleaseStatus();

            this.packageVersion.IsDeprecated = true;
            var deprecatedStatus = packageDetailsPage.Instance.GetReleaseStatus();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(status, Is.EqualTo("Latest stable"));
                Assert.That(deprecatedStatus, Is.EqualTo("Deprecated"));
            }
        }

        [Test]
        public void VerifyOnInitializedAsync()
        {
            var packageDetailsPage = this.context.Render<PackageDetails>(parameters =>
            {
                parameters.Add(p => p.Scope, "starion");
                parameters.Add(p => p.PackageShortname, "ecss-mm-pwr");
                parameters.Add(p => p.Tab, PackageTabConstants.Contents);
            });

            var defaultPackageDetailsPage = this.context.Render<PackageDetails>(parameters =>
            {
                parameters.Add(p => p.Scope, "starion");
                parameters.Add(p => p.PackageShortname, "ecss-mm-pwr");
            });

            using (Assert.EnterMultipleScope())
            {
                Assert.That(packageDetailsPage.Instance.SelectedContentTab, Is.EqualTo(PackageTabConstants.Contents));
                this.viewModelMock.Verify(x => x.InitializeViewModel("ecss-mm-pwr", "starion", PackageTabConstants.Contents), Times.Once);
                Assert.That(defaultPackageDetailsPage.Instance.SelectedContentTab, Is.EqualTo(PackageTabConstants.Overview));
                this.viewModelMock.Verify(x => x.InitializeViewModel("ecss-mm-pwr", "starion", PackageTabConstants.Overview), Times.Once);
            }
        }

        [Test]
        public void VerifyRendering()
        {
            this.viewModelMock.SetupGet(x => x.Package).Returns((IPackage)null!);
            var nullPackageDetailsPage = this.context.Render<PackageDetails>();
            Assert.That(nullPackageDetailsPage.Markup.Trim(), Has.Length.LessThan(5));

            this.viewModelMock.SetupGet(x => x.Package).Returns(this.package);
            var maintainer = new Account { Id = Guid.NewGuid(), Name = "R. Andre", ShortName = "r-andre", IsVerified = true };
            this.viewModelMock.SetupGet(x => x.Maintainers).Returns([maintainer]);
            this.SetupAccessFlags(true, false);
            var activePackageDetailsPage = this.context.Render<PackageDetails>();
            var activeSettingsButton = activePackageDetailsPage.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("package-details-settings-button"));

            this.viewModelMock.SetupGet(x => x.Owner).Returns(maintainer);
            var accountOwnerComponent = this.context.Render<PackageDetails>();
            this.viewModelMock.SetupGet(x => x.Owner).Returns(this.organization);

            var maintainerLinks = activePackageDetailsPage.FindAll($"a[href='{PageRoutes.GetAccountRoute("r-andre")}']");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(activePackageDetailsPage.Markup, Does.Contain("ECSS-MM-PWR"));
                Assert.That(activePackageDetailsPage.Markup, Does.Contain("Power subsystem metamodel"));
                Assert.That(activePackageDetailsPage.FindAll($"a[href='{PageRoutes.GetOrganizationRoute("starion")}']"), Has.Count.GreaterThan(0));
                Assert.That(maintainerLinks, Has.Count.EqualTo(1));
                Assert.That(maintainerLinks[0].TextContent.Trim(), Is.EqualTo("R. Andre"));
                Assert.That(accountOwnerComponent.FindAll($"a[href='{PageRoutes.GetAccountRoute("r-andre")}']"), Has.Count.GreaterThan(0));
                Assert.That(activeSettingsButton, Is.Not.Null);
            }

            this.SetupAccessFlags(false, true);
            var maintainerPackageDetailsPage = this.context.Render<PackageDetails>();
            var maintainerSettingsButton = maintainerPackageDetailsPage.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("package-details-settings-button"));
            var maintainerMigrateButton = maintainerPackageDetailsPage.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("package-details-migrate-in-bloom-button"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(maintainerSettingsButton, Is.Not.Null);
                Assert.That(maintainerMigrateButton, Is.Null);
            }

            this.SetupAccessFlags(false, false, true);
            var teamManagerDetailsPage = this.context.Render<PackageDetails>();
            var teamManagerSettingsButton = teamManagerDetailsPage.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("package-details-settings-button"));
            Assert.That(teamManagerSettingsButton, Is.Not.Null);

            this.SetupAccessFlags(false, false);
            var readerPackageDetailsPage = this.context.Render<PackageDetails>();
            var readerSettingsButton = readerPackageDetailsPage.FindComponents<BbButton>().FirstOrDefault(x => x.Instance.HasId("package-details-settings-button"));
            Assert.That(readerSettingsButton, Is.Null);
        }

        [Test]
        public void VerifySelectContentTab()
        {
            var navigationManager = this.context.Services.GetRequiredService<NavigationManager>();

            var packageDetailsPage = this.context.Render<PackageDetails>(parameters =>
            {
                parameters.Add(p => p.Scope, "starion");
                parameters.Add(p => p.PackageShortname, "ecss-mm-pwr");
            });

            packageDetailsPage.Instance.SelectContentTab(PackageTabConstants.Dependencies);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(packageDetailsPage.Instance.SelectedContentTab, Is.EqualTo(PackageTabConstants.Dependencies));
                Assert.That(navigationManager.Uri, Does.Contain("/packages/starion/ecss-mm-pwr/dependencies"));
            }
        }

        private void SetupAccessFlags(bool isUserAdmin, bool canManageVersions, bool canManagePackageTeam = false)
        {
            this.viewModelMock.SetupGet(x => x.IsUserAdmin).Returns(isUserAdmin);
            this.viewModelMock.SetupGet(x => x.CanManageVersions).Returns(canManageVersions);
            this.viewModelMock.SetupGet(x => x.CanManagePackageTeam).Returns(canManagePackageTeam);
        }
    }
}
