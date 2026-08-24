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
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using FluentResults;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.PackageDetails;
    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.Models.Package;
    using Mycelium.Forge.Models.Validation;
    using Mycelium.Forge.Services;
    using Mycelium.Forge.ViewModels.PackageDetails;

    [TestFixture]
    public class PackageDetailsTestFixture
    {
        private BunitContext context;
        private Mock<IPackageDetailsViewModel> viewModelMock;
        private Mock<IJsInterop> jsInteropMock;
        private DialogService dialogService;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;

            this.viewModelMock = new Mock<IPackageDetailsViewModel>();
            this.jsInteropMock = new Mock<IJsInterop>();

            var packageModel = new PackageModel(
                new Package { Name = "ECSS-MM-PWR", ShortName = "ecss-mm-pwr", Visibility = VisibilityKind.PUBLIC },
                "Starion Group",
                "1.3.0",
                description: "Power subsystem model.")
            {
                License = "Apache-2.0",
                Versions =
                [
                    new PackageVersionModel { Version = "1.3.0" }
                ]
            };

            var packageDetails = new PackageDetailsModel(packageModel)
            {
                InstallCommands = new Dictionary<string, string>
                {
                    { "Forge CLI", "forge add @starion/ECSS-MM-PWR@1.3.0" },
                    { "SysML v2 import", "import @starion/ECSS-MM-PWR;" }
                },
                Elements =
                [
                    new PackageElementModel("PowerSubsystem", "«part def»", "Parts", "Attributes")
                ],
                Dependencies =
                [
                    new PackageRelationshipModel("@esa/CoreTypes", "/packages/@esa/coretypes", "^1.0.0")
                ],
                Dependents =
                [
                    new PackageRelationshipModel("@starion/ECSS-Mission", "/packages/@starion/ecss-mission", "^1.0.0")
                ],
                ValidationReport = new PackageValidationReportModel(
                    "Automated release validation",
                    "All checks passed",
                    "5 / 5",
                    true,
                    [
                        new ValidationCheckModel("Schema check", "Passed")
                    ])
            };

            this.viewModelMock.SetupGet(x => x.Package).Returns(packageDetails);
            this.viewModelMock.SetupGet(x => x.IsUserAdmin).Returns(true);

            this.context.Services.AddSingleton(this.viewModelMock.Object);
            this.context.Services.AddSingleton(this.jsInteropMock.Object);
            this.dialogService = this.context.Services.GetRequiredService<DialogService>();
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyGetCurrentInstallCommand()
        {
            var packageDetailsPage = this.context.Render<PackageDetails>();

            var command = packageDetailsPage.Instance.GetCurrentInstallCommand();
            packageDetailsPage.Instance.SelectInstallTab("purl");
            var purlCommand = packageDetailsPage.Instance.GetCurrentInstallCommand();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(command, Is.EqualTo("forge add @starion/ECSS-MM-PWR@1.3.0"));
                Assert.That(purlCommand, Is.EqualTo(string.Empty));
            }
        }

        [Test]
        public void VerifyGetVisibleContentTabs()
        {
            var packageDetailsPage = this.context.Render<PackageDetails>();
            var tabs = packageDetailsPage.Instance.GetVisibleContentTabs();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(tabs, Has.Count.EqualTo(6));
                Assert.That(tabs, Contains.Item("Overview"));
                Assert.That(tabs, Contains.Item("Contents"));
                Assert.That(tabs, Contains.Item("Dependencies"));
                Assert.That(tabs, Contains.Item("Dependents"));
                Assert.That(tabs, Contains.Item("Versions"));
                Assert.That(tabs, Contains.Item("Validation"));
            }
        }

        [Test]
        public void VerifyHandleAddDependency()
        {
            var packageDetailsPage = this.context.Render<PackageDetails>();
            var result = new AddToProjectResult { ProjectName = "Project A", VersionConstraint = "^1.0.0" };

            Assert.DoesNotThrow(() => packageDetailsPage.Instance.HandleAddDependency(result));
        }

        [Test]
        public void VerifyHandleMigrateInBloom()
        {
            var result = new MigrateInBloomResult { ProjectName = "Project A", VersionConstraint = "1.3.0" };
            this.viewModelMock.Setup(x => x.MigrateInBloom(result)).Returns(Result.Ok());

            var packageDetailsPage = this.context.Render<PackageDetails>();
            var handleResult = packageDetailsPage.Instance.HandleMigrateInBloom(result);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(handleResult.IsSuccess, Is.True);
                this.viewModelMock.Verify(x => x.MigrateInBloom(result), Times.Once);
            }
        }

        [Test]
        public void VerifyOnParametersSet()
        {
            var packageDetailsPage = this.context.Render<PackageDetails>(parameters => parameters
                .Add(p => p.Organization, "@starion")
                .Add(p => p.PackageName, "ECSS-MM-PWR"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(packageDetailsPage.Instance, Is.Not.Null);
                Assert.That(packageDetailsPage.Instance.IsUserAdmin, Is.True);
                this.viewModelMock.Verify(x => x.InitializeViewModel("ECSS-MM-PWR", "@starion"), Times.Once);
            }
        }

        [Test]
        public void VerifyOpenAddToProjectDialog()
        {
            var packageDetailsPage = this.context.Render<PackageDetails>();
            var addToProjectButton = packageDetailsPage.Find("#package-details-add-to-project-button");

            _ = packageDetailsPage.InvokeAsync(() => addToProjectButton.ClickAsync());

            Assert.That(this.dialogService.Dialogs, Has.Count.EqualTo(1));
        }

        [Test]
        public void VerifyOpenMigrateInBloomDialog()
        {
            var packageDetailsPage = this.context.Render<PackageDetails>();
            var migrateButton = packageDetailsPage.Find("#package-details-migrate-in-bloom-button");

            _ = packageDetailsPage.InvokeAsync(() => migrateButton.ClickAsync());

            Assert.That(this.dialogService.Dialogs, Has.Count.EqualTo(1));
        }

        [Test]
        public void VerifySelectTabsAndClasses()
        {
            var packageDetailsPage = this.context.Render<PackageDetails>();

            packageDetailsPage.Instance.SelectInstallTab("SysML v2 import");
            packageDetailsPage.Instance.SelectContentTab("Contents");

            var installClassActive = packageDetailsPage.Instance.GetInstallTabClass("SysML v2 import");
            var installClassInactive = packageDetailsPage.Instance.GetInstallTabClass("purl");
            var contentClassActive = packageDetailsPage.Instance.GetContentTabClass("Contents");
            var contentClassInactive = packageDetailsPage.Instance.GetContentTabClass("Overview");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(packageDetailsPage.Instance.SelectedInstallTab, Is.EqualTo("SysML v2 import"));
                Assert.That(packageDetailsPage.Instance.SelectedContentTab, Is.EqualTo("Contents"));
                Assert.That(installClassActive, Does.Contain("font-semibold text-primary"));
                Assert.That(installClassInactive, Does.Contain("text-muted-foreground"));
                Assert.That(contentClassActive, Does.Contain("bg-primary/10"));
                Assert.That(contentClassInactive, Does.Contain("text-muted-foreground"));
            }
        }
    }
}
