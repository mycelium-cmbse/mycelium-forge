// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationDetailsViewModelTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.ViewModels.OrganizationDetails
{
    using System;

    using Mycelium.Forge.ViewModels.OrganizationDetails;

    [TestFixture]
    public class OrganizationDetailsViewModelTestFixture
    {
        private OrganizationDetailsViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            this.viewModel = new OrganizationDetailsViewModel();
        }

        [Test]
        public void VerifyInitializeViewModel()
        {
            var orgId = Guid.NewGuid();
            this.viewModel.InitializeViewModel(orgId);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.Organization, Is.Not.Null);
                Assert.That(this.viewModel.Packages, Has.Count.GreaterThan(0));
            }
        }
    }
}
