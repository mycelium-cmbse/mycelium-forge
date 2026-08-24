// ------------------------------------------------------------------------------------------------
// <copyright file="PackageDetailsViewModelTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.ViewModels.PackageDetails
{
    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.ViewModels.PackageDetails;

    [TestFixture]
    public class PackageDetailsViewModelTestFixture
    {
        private PackageDetailsViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            this.viewModel = new PackageDetailsViewModel();
        }

        [Test]
        public void VerifyInitializeViewModel()
        {
            this.viewModel.InitializeViewModel("ECSS-MM-PWR", "@starion");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.Package, Is.Not.Null);
                Assert.That(this.viewModel.Package.Package.Name, Is.EqualTo("ECSS-MM-PWR"));
                Assert.That(this.viewModel.Package.Package.Publisher, Is.EqualTo("@starion"));
                Assert.That(this.viewModel.Package.InstallCommands, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.Package.Elements, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.Package.Dependencies, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.Package.Dependents, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.Package.Package.Versions, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.Package.ValidationReport, Is.Not.Null);
                Assert.That(this.viewModel.IsUserAdmin, Is.True);
            }

            this.viewModel.InitializeViewModel(string.Empty, string.Empty);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.Package.Package.Name, Is.EqualTo("ECSS-MM-PWR"));
                Assert.That(this.viewModel.Package.Package.Publisher, Is.EqualTo("@starion"));
            }

            this.viewModel.InitializeViewModel("MyPack", "starion");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.Package.Package.Name, Is.EqualTo("MyPack"));
                Assert.That(this.viewModel.Package.Package.Publisher, Is.EqualTo("@starion"));
            }
        }

        [Test]
        public void VerifyMigrateInBloom()
        {
            var result = new MigrateInBloomResult
            {
                ProjectName = "Spacecraft Mission",
                VersionConstraint = "^1.2.0"
            };

            var operationResult = this.viewModel.MigrateInBloom(result);

            Assert.That(operationResult.IsSuccess, Is.True);
        }
    }
}
