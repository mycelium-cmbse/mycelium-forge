// ------------------------------------------------------------------------------------------------
// <copyright file="HomeViewModelTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.ViewModels.Home
{
    using Mycelium.Forge.ViewModels.Home;

    [TestFixture]
    public class HomeViewModelTestFixture
    {
        private HomeViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            this.viewModel = new HomeViewModel();
        }

        [Test]
        public void VerifyInitializeViewModel()
        {
            this.viewModel.InitializeViewModel();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.PackageCount, Is.Not.Empty);
                Assert.That(this.viewModel.VersionCount, Is.Not.Empty);
                Assert.That(this.viewModel.PublisherCount, Is.Not.Empty);
                Assert.That(this.viewModel.ImportCount, Is.Not.Empty);
                Assert.That(this.viewModel.StandardLibraries, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.RecentlyUpdated, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.MostUsed, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.ModelsFromOtherMbseTools, Has.Count.GreaterThan(0));
            }
        }
    }
}
