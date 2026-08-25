// ------------------------------------------------------------------------------------------------
// <copyright file="AdminAccountsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.AdminAccounts
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Data;
    using Mycelium.Forge.Enums;
    using Mycelium.Forge.Models.Admin;

    /// <summary>
    /// Provides view model state and operations for the installation accounts administration page.
    /// </summary>
    public class AdminAccountsViewModel : IAdminAccountsViewModel
    {
        /// <summary>
        /// Represents the filter value used to include all account statuses without filtering.
        /// </summary>
        public const string AllStatusFilter = "All";

        /// <summary>
        /// Gets or sets the collection of all accounts.
        /// </summary>
        public List<AdminAccountModel> Accounts { get; set; } = [];

        /// <summary>
        /// Gets or sets the filtered collection of accounts based on search query, status, and verification filters.
        /// </summary>
        public List<AdminAccountModel> FilteredAccounts { get; set; } = [];

        /// <summary>
        /// Initializes the view model state and loads initial seed accounts.
        /// </summary>
        /// <param name="searchQuery">The initial search filter query string.</param>
        /// <param name="statusFilter">The initial status filter.</param>
        /// <param name="verificationFilter">The initial verification filter option.</param>
        public void InitializeViewModel(string searchQuery = "", string statusFilter = AllStatusFilter, VerificationFilterOption verificationFilter = VerificationFilterOption.All)
        {
            this.Accounts = [.. SeedData.AdminAccounts];
            this.ApplyFilters(searchQuery, statusFilter, verificationFilter);
        }

        /// <summary>
        /// Applies the current search query, status filter, and verification filter to the accounts collection.
        /// </summary>
        /// <param name="searchQuery">The search query text filter.</param>
        /// <param name="statusFilter">The status filter.</param>
        /// <param name="verificationFilter">The verification filter option.</param>
        public void ApplyFilters(string searchQuery, string statusFilter, VerificationFilterOption verificationFilter)
        {
            var filtered = this.Accounts.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var query = searchQuery.Trim();

                filtered = filtered.Where(account =>
                    account.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    account.Username.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    account.Email.Contains(query, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.Equals(statusFilter, AllStatusFilter, StringComparison.OrdinalIgnoreCase) &&
                Enum.TryParse<ScopeStatusKind>(statusFilter, true, out var statusKind))
            {
                filtered = filtered.Where(account => account.Status == statusKind);
            }

            if (verificationFilter != VerificationFilterOption.All)
            {
                var filterName = verificationFilter.ToString();

                filtered = filtered.Where(account =>
                    string.Equals(account.VerificationStatus, filterName, StringComparison.OrdinalIgnoreCase));
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
