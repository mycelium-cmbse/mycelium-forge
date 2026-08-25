// ------------------------------------------------------------------------------------------------
// <copyright file="PublishViewModelTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.ViewModels.Publish
{
    using Mycelium.Forge.ViewModels.Publish;

    [TestFixture]
    public class PublishViewModelTestFixture
    {
        private PublishViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            this.viewModel = new PublishViewModel();
        }

        [Test]
        public void VerifyInitializeViewModel()
        {
            this.viewModel.IsPublishing = true;
            this.viewModel.InitializeViewModel();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.Metadata, Is.Not.Null);
                Assert.That(this.viewModel.Metadata.PackageName, Is.EqualTo("ECSS-MM-PWR"));
                Assert.That(this.viewModel.Steps, Has.Count.EqualTo(3));
                Assert.That(this.viewModel.ValidationChecks, Has.Count.EqualTo(6));
                Assert.That(this.viewModel.ScopeOptions, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.LicenseOptions, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.VisibilityOptions, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.MetamodelOptions, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.IsPublishing, Is.False);
            }
        }

        [Test]
        public void VerifyPublish()
        {
            this.viewModel.InitializeViewModel();
            var result = this.viewModel.Publish();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(this.viewModel.IsPublishing, Is.False);
            }
        }
    }
}
