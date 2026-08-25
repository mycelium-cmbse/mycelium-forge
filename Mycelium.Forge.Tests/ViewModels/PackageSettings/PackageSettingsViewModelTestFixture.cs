// ------------------------------------------------------------------------------------------------
// <copyright file="PackageSettingsViewModelTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.ViewModels.PackageSettings
{
    using Mycelium.Forge.ViewModels.PackageSettings;

    [TestFixture]
    public class PackageSettingsViewModelTestFixture
    {
        private PackageSettingsViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            this.viewModel = new PackageSettingsViewModel();
        }

        [Test]
        public void VerifyInitializeViewModel()
        {
            this.viewModel.InitializeViewModel("ECSS-MM-PWR", "@starion");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.Package, Is.Not.Null);
                Assert.That(this.viewModel.Package.Name, Is.EqualTo("ECSS-MM-PWR"));
                Assert.That(this.viewModel.Package.Publisher, Is.EqualTo("@starion"));
                Assert.That(this.viewModel.Package.Maintainers, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.Package.Versions, Has.Count.GreaterThan(0));
            }

            this.viewModel.InitializeViewModel(string.Empty, string.Empty);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.Package.Name, Is.EqualTo("ECSS-MM-PWR"));
                Assert.That(this.viewModel.Package.Publisher, Is.EqualTo("@starion"));
            }

            this.viewModel.InitializeViewModel("MyPack", "starion");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.Package.Name, Is.EqualTo("MyPack"));
                Assert.That(this.viewModel.Package.Publisher, Is.EqualTo("@starion"));
            }
        }

        [Test]
        public void VerifySavePackage()
        {
            Assert.DoesNotThrow(() => this.viewModel.SavePackage());
        }
    }
}
