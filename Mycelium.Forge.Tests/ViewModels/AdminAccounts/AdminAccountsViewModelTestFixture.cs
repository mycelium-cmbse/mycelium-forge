// ------------------------------------------------------------------------------------------------
// <copyright file="AdminAccountsViewModelTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.ViewModels.AdminAccounts
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.ViewModels.AdminAccounts;

    [TestFixture]
    public class AdminAccountsViewModelTestFixture
    {
        private AdminAccountsViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            this.viewModel = new AdminAccountsViewModel();
        }

        [Test]
        public void VerifyApplyFilters()
        {
            this.viewModel.InitializeViewModel();

            this.viewModel.ApplyFilters("r.andre", "All", "All");
            var nameCount = this.viewModel.FilteredAccounts.Count;

            this.viewModel.ApplyFilters(string.Empty, nameof(ScopeStatusKind.ACTIVE), "All");
            var activeCount = this.viewModel.FilteredAccounts.Count;

            this.viewModel.ApplyFilters(string.Empty, "All", "Verified");
            var verifiedCount = this.viewModel.FilteredAccounts.Count;

            this.viewModel.ApplyFilters("nonexistentuser12345", "All", "All");
            var emptyCount = this.viewModel.FilteredAccounts.Count;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(nameCount, Is.GreaterThanOrEqualTo(1));
                Assert.That(activeCount, Is.GreaterThanOrEqualTo(1));
                Assert.That(verifiedCount, Is.GreaterThanOrEqualTo(1));
                Assert.That(emptyCount, Is.Zero);
            }
        }

        [Test]
        public void VerifyInitializeViewModel()
        {
            this.viewModel.InitializeViewModel();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.viewModel.Accounts, Has.Count.GreaterThan(0));
                Assert.That(this.viewModel.FilteredAccounts, Has.Count.EqualTo(this.viewModel.Accounts.Count));
            }
        }

        [Test]
        public void VerifyTransferOwnership()
        {
            Assert.DoesNotThrow(() => this.viewModel.TransferOwnership());
        }
    }
}
