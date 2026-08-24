// ------------------------------------------------------------------------------------------------
// <copyright file="AccountSettingsViewModelTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.ViewModels.AccountSettings
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.Models.Profile;
    using Mycelium.Forge.ViewModels.AccountSettings;

    [TestFixture]
    public class AccountSettingsViewModelTestFixture
    {
        private AccountSettingsViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            this.viewModel = new AccountSettingsViewModel();
        }

        [Test]
        public void VerifyCreateOrganization()
        {
            this.viewModel.InitializeViewModel();
            var initialCount = this.viewModel.Organizations.Count;

            var result = new CreateOrganizationResult
            {
                OrganizationName = "New Org",
                Scope = "@neworg",
                BillingEmail = "billing@neworg.com"
            };

            var operationResult = this.viewModel.CreateOrganization(result);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(operationResult.IsSuccess, Is.True);
                Assert.That(this.viewModel.Organizations, Has.Count.EqualTo(initialCount + 1));
                Assert.That(this.viewModel.Organizations[^1].Name, Is.EqualTo("New Org"));
                Assert.That(this.viewModel.Organizations[^1].Scope, Is.EqualTo("@neworg"));
            }
        }

        [Test]
        public void VerifyDeactivateAccount()
        {
            Assert.DoesNotThrow(() => this.viewModel.DeactivateAccount());
        }

        [Test]
        public void VerifyDeleteAccount()
        {
            Assert.DoesNotThrow(() => this.viewModel.DeleteAccount());
        }

        [Test]
        public void VerifyInitializeViewModel()
        {
            this.viewModel.InitializeViewModel();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.Profile, Is.Not.Null);
                Assert.That(this.viewModel.Organizations, Has.Count.GreaterThan(0));
            }
        }

        [Test]
        public void VerifyUpdateProfile()
        {
            this.viewModel.InitializeViewModel();
            var newProfile = new UserProfileModel(new Account { Name = "New Name" });

            this.viewModel.UpdateProfile(newProfile);

            Assert.That(this.viewModel.Profile, Is.SameAs(newProfile));
        }
    }
}
