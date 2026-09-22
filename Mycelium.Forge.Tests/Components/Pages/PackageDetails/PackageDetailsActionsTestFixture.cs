// ------------------------------------------------------------------------------------------------
// <copyright file="PackageDetailsActionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.PackageDetails
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.PackageDetails;
    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.Services;
    using Mycelium.Forge.ViewModels.PackageDetails;

    /// <summary>
    /// Test fixture for <see cref="PackageDetailsActions" /> component.
    /// </summary>
    [TestFixture]
    public class PackageDetailsActionsTestFixture
    {
        private BunitContext context;
        private Mock<IPackageDetailsActionsViewModel> viewModelMock;
        private DialogService dialogService;
        private Package package;
        private Organization organization;
        private PackageVersion packageVersion;

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

            this.viewModelMock = new Mock<IPackageDetailsActionsViewModel>();

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
                Owner = this.organization.Id
            };

            this.packageVersion = new PackageVersion
            {
                Id = Guid.NewGuid(),
                Owner = this.package.Id,
                Version = "1.3.0"
            };

            this.viewModelMock.SetupGet(x => x.Package).Returns(this.package);
            this.viewModelMock.SetupGet(x => x.Owner).Returns(this.organization);
            this.viewModelMock.SetupGet(x => x.SelectedVersion).Returns(this.packageVersion);
            this.viewModelMock.SetupGet(x => x.IsUserAdmin).Returns(true);

            this.viewModelMock.SetupGet(x => x.InstallCommands).Returns(new Dictionary<string, string>
            {
                { InstallCommandConstants.ForgeCli, "forge add @starion/ecss-mm-pwr@1.3.0" },
                { InstallCommandConstants.SysMlV2Import, "import @starion/ecss-mm-pwr;" }
            });

            this.context.Services.AddSingleton(this.viewModelMock.Object);
            this.context.Services.AddSingleton(new Mock<IJsInterop>().Object);
            this.dialogService = this.context.Services.GetRequiredService<DialogService>();
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
        /// Verifies that <see cref="PackageDetailsActions.GetCurrentInstallCommand" /> resolves the active command.
        /// </summary>
        [Test]
        public void VerifyGetCurrentInstallCommand()
        {
            var component = this.context.Render<PackageDetailsActions>(parameters => parameters
                .Add(p => p.Package, this.package)
                .Add(p => p.Owner, this.organization)
                .Add(p => p.SelectedVersion, this.packageVersion)
                .Add(p => p.IsUserAdmin, true));

            var forgeCliCommand = component.Instance.GetCurrentInstallCommand();
            component.Instance.SelectInstallTab(InstallCommandConstants.SysMlV2Import);
            var sysmlCommand = component.Instance.GetCurrentInstallCommand();
            component.Instance.SelectInstallTab(InstallCommandConstants.Purl);
            var purlCommand = component.Instance.GetCurrentInstallCommand();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(forgeCliCommand, Is.EqualTo("forge add @starion/ecss-mm-pwr@1.3.0"));
                Assert.That(sysmlCommand, Is.EqualTo("import @starion/ecss-mm-pwr;"));
                Assert.That(purlCommand, Is.EqualTo(string.Empty));
            }

            this.viewModelMock.SetupGet(x => x.InstallCommands).Returns((IReadOnlyDictionary<string, string>)null!);
            var nullCommand = component.Instance.GetCurrentInstallCommand();
            Assert.That(nullCommand, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Verifies that <see cref="PackageDetailsActions.GetInstallTabClass(string)" /> computes correct styling classes.
        /// </summary>
        [Test]
        public void VerifyGetInstallTabClass()
        {
            var component = this.context.Render<PackageDetailsActions>(parameters => parameters
                .Add(p => p.Package, this.package)
                .Add(p => p.Owner, this.organization)
                .Add(p => p.SelectedVersion, this.packageVersion));

            component.Instance.SelectInstallTab(InstallCommandConstants.ForgeCli);
            var activeClass = component.Instance.GetInstallTabClass(InstallCommandConstants.ForgeCli);
            var inactiveClass = component.Instance.GetInstallTabClass(InstallCommandConstants.SysMlV2Import);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(activeClass, Does.Contain("font-semibold text-primary"));
                Assert.That(inactiveClass, Does.Contain("text-muted-foreground"));
            }
        }

        /// <summary>
        /// Verifies that <see cref="PackageDetailsActions.HandleAddDependency(AddToProjectResult)" /> does not throw.
        /// </summary>
        [Test]
        public void VerifyHandleAddDependency()
        {
            var component = this.context.Render<PackageDetailsActions>();

            var result = new AddToProjectResult
            {
                ProjectName = "Project A",
                VersionConstraint = "^1.0.0"
            };

            Assert.DoesNotThrow(() => component.Instance.HandleAddDependency(result));
        }

        /// <summary>
        /// Verifies that <see cref="PackageDetailsActions.HandleMigrateInBloom(MigrateInBloomResult)" /> calls view model.
        /// </summary>
        [Test]
        public void VerifyHandleMigrateInBloom()
        {
            var component = this.context.Render<PackageDetailsActions>();

            var result = new MigrateInBloomResult
            {
                ProjectName = "Project A",
                VersionConstraint = "^1.0.0"
            };

            component.Instance.HandleMigrateInBloom(result);

            this.viewModelMock.Verify(x => x.MigrateInBloom(result), Times.Once);
        }

        /// <summary>
        /// Verifies that <see cref="PackageDetailsActions.OpenAddToProjectDialog" /> opens the dialog.
        /// </summary>
        [Test]
        public void VerifyOpenAddToProjectDialog()
        {
            var component = this.context.Render<PackageDetailsActions>(parameters => parameters
                .Add(p => p.Package, this.package)
                .Add(p => p.Owner, this.organization)
                .Add(p => p.SelectedVersion, this.packageVersion));

            _ = component.Instance.OpenAddToProjectDialog();

            Assert.That(this.dialogService.Dialogs, Has.Count.EqualTo(1));

            this.viewModelMock.SetupGet(x => x.Package).Returns((IPackage)null!);
            _ = component.Instance.OpenAddToProjectDialog();

            Assert.That(this.dialogService.Dialogs, Has.Count.EqualTo(1));
        }

        /// <summary>
        /// Verifies that <see cref="PackageDetailsActions.OpenMigrateInBloomDialog" /> opens the dialog.
        /// </summary>
        [Test]
        public void VerifyOpenMigrateInBloomDialog()
        {
            var component = this.context.Render<PackageDetailsActions>(parameters => parameters
                .Add(p => p.Package, this.package)
                .Add(p => p.Owner, this.organization)
                .Add(p => p.SelectedVersion, this.packageVersion));

            _ = component.Instance.OpenMigrateInBloomDialog();

            Assert.That(this.dialogService.Dialogs, Has.Count.EqualTo(1));

            this.viewModelMock.SetupGet(x => x.SelectedVersion).Returns((IPackageVersion)null!);
            _ = component.Instance.OpenMigrateInBloomDialog();

            Assert.That(this.dialogService.Dialogs, Has.Count.EqualTo(1));
        }

        /// <summary>
        /// Verifies that <see cref="PackageDetailsActions.SelectInstallTab(string)" /> updates the selected install tab.
        /// </summary>
        [Test]
        public void VerifySelectInstallTab()
        {
            var component = this.context.Render<PackageDetailsActions>(parameters => parameters
                .Add(p => p.Package, this.package)
                .Add(p => p.Owner, this.organization)
                .Add(p => p.SelectedVersion, this.packageVersion));

            component.Instance.SelectInstallTab(InstallCommandConstants.Manifest);

            Assert.That(component.Instance.SelectedInstallTab, Is.EqualTo(InstallCommandConstants.Manifest));
        }
    }
}
