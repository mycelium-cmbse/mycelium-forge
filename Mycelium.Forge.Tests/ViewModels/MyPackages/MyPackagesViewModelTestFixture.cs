// ------------------------------------------------------------------------------------------------
// <copyright file="MyPackagesViewModelTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.ViewModels.MyPackages
{
    using Mycelium.Forge.ViewModels.MyPackages;

    [TestFixture]
    public class MyPackagesViewModelTestFixture
    {
        private MyPackagesViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            this.viewModel = new MyPackagesViewModel();
        }

        [Test]
        public void VerifyInitializeViewModel()
        {
            this.viewModel.InitializeViewModel();

            Assert.That(this.viewModel.Packages, Has.Count.GreaterThan(0));
        }
    }
}
