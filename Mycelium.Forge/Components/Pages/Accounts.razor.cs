// ------------------------------------------------------------------------------------------------
// <copyright file="Accounts.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extensions;
    using Mycelium.Forge.Models;
    using Mycelium.Forge.ViewModels;

    /// <summary>
    /// Code-behind logic for the installation administration accounts management page.
    /// </summary>
    public partial class Accounts : ComponentBase
    {
        /// <summary>
        /// The list of available status filter options.
        /// </summary>
        private static readonly IReadOnlyList<string> AvailableStatusFilters =
        [
            "All",
            nameof(ScopeStatusKind.ACTIVE).ToUpperCaseFirst(),
            nameof(ScopeStatusKind.DEACTIVATED).ToUpperCaseFirst()
        ];

        /// <summary>
        /// The list of available verification filter options.
        /// </summary>
        private static readonly IReadOnlyList<string> AvailableVerificationFilters =
        [
            "All",
            "Verified",
            "Pending"
        ];

        /// <summary>
        /// Gets or sets the view model managing installation administration accounts state.
        /// </summary>
        [Inject]
        public IAdminAccountsViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the search filter query string.
        /// </summary>
        public string SearchQuery { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the selected account status filter.
        /// </summary>
        public string SelectedStatusFilter { get; set; } = "All";

        /// <summary>
        /// Gets or sets the selected verification status filter.
        /// </summary>
        public string SelectedVerificationFilter { get; set; } = "All";

        /// <summary>
        /// Gets the available status filter options.
        /// </summary>
        public IReadOnlyList<string> StatusFilterOptions => AvailableStatusFilters;

        /// <summary>
        /// Gets the available verification filter options.
        /// </summary>
        public IReadOnlyList<string> VerificationFilterOptions => AvailableVerificationFilters;

        /// <summary>
        /// Initializes the component and loads initial view model state.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.ViewModel.InitializeViewModel(this.SearchQuery, this.SelectedStatusFilter, this.SelectedVerificationFilter);
        }

        /// <summary>
        /// Handles search input query changes and triggers account filtering.
        /// </summary>
        /// <param name="query">The new search query value.</param>
        private void OnSearchInputChanged(string query)
        {
            this.SearchQuery = query ?? string.Empty;
            this.ViewModel.ApplyFilters(this.SearchQuery, this.SelectedStatusFilter, this.SelectedVerificationFilter);
        }

        /// <summary>
        /// Handles status filter dropdown selection changes.
        /// </summary>
        /// <param name="status">The selected status filter.</param>
        private void OnStatusFilterChanged(string status)
        {
            this.SelectedStatusFilter = status ?? "All";
            this.ViewModel.ApplyFilters(this.SearchQuery, this.SelectedStatusFilter, this.SelectedVerificationFilter);
        }

        /// <summary>
        /// Handles verification status filter dropdown selection changes.
        /// </summary>
        /// <param name="verification">The selected verification status filter.</param>
        private void OnVerificationFilterChanged(string verification)
        {
            this.SelectedVerificationFilter = verification ?? "All";
            this.ViewModel.ApplyFilters(this.SearchQuery, this.SelectedStatusFilter, this.SelectedVerificationFilter);
        }

        /// <summary>
        /// Handles initiating an account ownership transfer action.
        /// </summary>
        private void OnTransfer()
        {
            this.ViewModel.TransferOwnership();
        }

        /// <summary>
        /// Handles opening the contextual options menu for the specified account.
        /// </summary>
        /// <param name="account">The <see cref="AdminAccountModel" /> instance whose menu was triggered.</param>
        private void OnAccountMenu(AdminAccountModel account)
        {
        }

        /// <summary>
        /// Computes the human-readable summary count text for the accounts list.
        /// </summary>
        /// <returns>A formatted string indicating the number of accounts.</returns>
        private string GetAccountsCountText()
        {
            var count = this.ViewModel.FilteredAccounts.Count;
            return count == 1 ? "1 account" : $"{count} accounts";
        }
    }
}
