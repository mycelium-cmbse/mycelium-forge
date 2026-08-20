// ------------------------------------------------------------------------------------------------
// <copyright file="AdminAccountsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels
{
    using Mycelium.Forge.Data;
    using Mycelium.Forge.Models;

    /// <summary>
    /// Provides view model state and operations for the installation accounts administration page.
    /// </summary>
    public class AdminAccountsViewModel : IAdminAccountsViewModel
    {
        /// <summary>
        /// The list of available status filter options.
        /// </summary>
        private static readonly IReadOnlyList<string> AvailableStatusFilters =
        [
            "All",
            "Active",
            "Suspended"
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
        /// Gets or sets the collection of all accounts.
        /// </summary>
        public List<AdminAccountModel> Accounts { get; set; } = [];

        /// <summary>
        /// Gets or sets the filtered collection of accounts based on search query, status, and verification filters.
        /// </summary>
        public List<AdminAccountModel> FilteredAccounts { get; set; } = [];

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
        /// Initializes the view model state and loads initial seed accounts.
        /// </summary>
        public void InitializeViewModel()
        {
            this.Accounts = [.. SeedData.AdminAccounts];
            this.ApplyFilters();
        }

        /// <summary>
        /// Applies the current search query, status filter, and verification filter to the accounts collection.
        /// </summary>
        public void ApplyFilters()
        {
            var filtered = this.Accounts.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(this.SearchQuery))
            {
                var query = this.SearchQuery.Trim();

                filtered = filtered.Where(account =>
                    account.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    account.Username.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    account.Email.Contains(query, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.Equals(this.SelectedStatusFilter, "All", StringComparison.OrdinalIgnoreCase))
            {
                filtered = filtered.Where(account =>
                    string.Equals(account.Status, this.SelectedStatusFilter, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.Equals(this.SelectedVerificationFilter, "All", StringComparison.OrdinalIgnoreCase))
            {
                filtered = filtered.Where(account =>
                    string.Equals(account.VerificationStatus, this.SelectedVerificationFilter, StringComparison.OrdinalIgnoreCase));
            }

            this.FilteredAccounts = [.. filtered];
        }

        /// <summary>
        /// Handles initiating an account ownership transfer action.
        /// </summary>
        public void TransferOwnership()
        {
        }
    }
}
