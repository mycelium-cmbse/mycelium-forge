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
    using Mycelium.Forge.ViewModels.PackageDetails;

    /// <summary>
    /// Test fixture for <see cref="PackageDetails" /> component.
    /// </summary>
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

        /// <summary>
        /// Sets up the test context and mocks before each test execution.
        /// </summary>
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

            this.viewModelMock.SetupGet(x => x.IsUserAdmin).Returns(true);

            this.context.Services.AddSingleton(this.viewModelMock.Object);
            this.context.Services.AddSingleton(this.actionsViewModelMock.Object);
            this.context.Services.AddSingleton(new Mock<IJsInterop>().Object);
        }

        /// <summary>
        /// Tears down the test context after each test execution.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous tear down.</returns>
        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        /// <summary>
        /// Verifies that <see cref="PackageDetails.GetContentTabClass(string)" /> computes correct styling classes.
        /// </summary>
        [Test]
        public void VerifyGetContentTabClass()
        {
            var component = this.context.Render<PackageDetails>();

            component.Instance.SelectedContentTab = PackageTabConstants.Overview;
            var activeClass = component.Instance.GetContentTabClass(PackageTabConstants.Overview);
            var inactiveClass = component.Instance.GetContentTabClass(PackageTabConstants.Contents);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(activeClass, Does.Contain("bg-primary/10"));
                Assert.That(activeClass, Does.Contain("text-primary font-semibold"));
                Assert.That(inactiveClass, Does.Contain("text-muted-foreground"));
            }
        }

        /// <summary>
        /// Verifies that <see cref="PackageDetails.GetPackageUrl" /> generates a purl route identifier.
        /// </summary>
        [Test]
        public void VerifyGetPackageUrl()
        {
            var component = this.context.Render<PackageDetails>();
            var purl = component.Instance.GetPackageUrl();

            Assert.That(purl, Is.EqualTo("pkg:forge/starion/ecss-mm-pwr@1.3.0"));
        }

        /// <summary>
        /// Verifies that <see cref="PackageDetails.GetPublishedAgo" /> formats publication date correctly.
        /// </summary>
        [Test]
        public void VerifyGetPublishedAgo()
        {
            var component = this.context.Render<PackageDetails>();
            var publishedAgo = component.Instance.GetPublishedAgo();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(publishedAgo, Is.Not.Null);
                Assert.That(publishedAgo, Is.Not.Empty);
            }
        }

        /// <summary>
        /// Verifies that <see cref="PackageDetails.GetQualityScore" /> formats quality score correctly.
        /// </summary>
        [Test]
        public void VerifyGetQualityScore()
        {
            var component = this.context.Render<PackageDetails>();
            var score = component.Instance.GetQualityScore();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(score, Is.EqualTo("2/2 checks"));
            }

            this.viewModelMock.SetupGet(x => x.QualityChecks).Returns([]);
            var emptyScore = component.Instance.GetQualityScore();

            Assert.That(emptyScore, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Verifies that <see cref="PackageDetails.GetReleaseStatus" /> returns correct status for active and deprecated packages.
        /// </summary>
        [Test]
        public void VerifyGetReleaseStatus()
        {
            var component = this.context.Render<PackageDetails>();
            var status = component.Instance.GetReleaseStatus();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(status, Is.EqualTo("Latest stable"));
            }

            this.package.IsDeprecated = true;
            var deprecatedStatus = component.Instance.GetReleaseStatus();

            Assert.That(deprecatedStatus, Is.EqualTo("Deprecated"));
        }

        /// <summary>
        /// Verifies that <see cref="PackageDetails.OnInitializedAsync" /> initializes the view model.
        /// </summary>
        [Test]
        public void VerifyOnInitializedAsync()
        {
            var component = this.context.Render<PackageDetails>(parameters => parameters
                .Add(p => p.Scope, "starion")
                .Add(p => p.PackageName, "ecss-mm-pwr")
                .Add(p => p.Tab, PackageTabConstants.Contents));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(component.Instance.SelectedContentTab, Is.EqualTo(PackageTabConstants.Contents));
                this.viewModelMock.Verify(x => x.InitializeViewModel("ecss-mm-pwr", "starion", PackageTabConstants.Contents), Times.Once);
            }

            var defaultComponent = this.context.Render<PackageDetails>(parameters => parameters
                .Add(p => p.Scope, "starion")
                .Add(p => p.PackageName, "ecss-mm-pwr"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(defaultComponent.Instance.SelectedContentTab, Is.EqualTo(PackageTabConstants.Overview));
            }
        }

        /// <summary>
        /// Verifies rendering behavior when package is null or populated.
        /// </summary>
        [Test]
        public void VerifyRendering()
        {
            this.viewModelMock.SetupGet(x => x.Package).Returns((IPackage)null!);
            var nullComponent = this.context.Render<PackageDetails>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(nullComponent.Markup.Trim(), Has.Length.LessThan(5));
            }

            this.viewModelMock.SetupGet(x => x.Package).Returns(this.package);
            var activeComponent = this.context.Render<PackageDetails>();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(activeComponent.Markup, Does.Contain("ECSS-MM-PWR"));
                Assert.That(activeComponent.Markup, Does.Contain("Power subsystem metamodel"));
            }
        }

        /// <summary>
        /// Verifies that <see cref="PackageDetails.SelectContentTab(string)" /> navigates to tab route.
        /// </summary>
        [Test]
        public void VerifySelectContentTab()
        {
            var navigationManager = this.context.Services.GetRequiredService<NavigationManager>();

            var component = this.context.Render<PackageDetails>(parameters => parameters
                .Add(p => p.Scope, "starion")
                .Add(p => p.PackageName, "ecss-mm-pwr"));

            component.Instance.SelectContentTab(PackageTabConstants.Dependencies);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(component.Instance.SelectedContentTab, Is.EqualTo(PackageTabConstants.Dependencies));
                Assert.That(navigationManager.Uri, Does.Contain("/packages/starion/ecss-mm-pwr/dependencies"));
            }
        }
    }
}
