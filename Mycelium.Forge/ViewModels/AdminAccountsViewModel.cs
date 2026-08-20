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
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        /// The master collection of seed account records.
        /// </summary>
        private readonly List<AdminAccountModel> seedAccounts =
        [
            new("1", "R. André", "@r.andre", "RA", "regis.andre@starion.eu", true, "Verified", "@starion (Admin), @esa", "Active"),
            new("2", "S. Kramer", "@s.kramer", "SK", "s.kramer@starion.eu", false, "Verified", "@starion (Admin)", "Active"),
            new("3", "J. Klein", "@j.klein", "JK", "j.klein@starion.eu", false, "Verified", "@starion (Admin)", "Active"),
            new("4", "E. Weber", "@e.weber", "EW", "e.weber@starion.eu", false, "Pending", "@starion (Admin)", "Active"),
            new("5", "M. Blanc", "@m.blanc", "MB", "m.blanc@starion.eu", false, "Verified", "@starion", "Suspended"),
            new("6", "A. Novak", "@a.novak", "AN", "a.novak@esa.int", false, "Pending", "@starion (Admin)", "Active")
        ];

        /// <summary>
        /// Gets or sets the collection of all accounts.
        /// </summary>
        public IReadOnlyList<AdminAccountModel> Accounts { get; set; } = [];

        /// <summary>
        /// Gets or sets the filtered collection of accounts based on search query, status, and verification filters.
        /// </summary>
        public IReadOnlyList<AdminAccountModel> FilteredAccounts { get; set; } = [];

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
            this.Accounts = [.. this.seedAccounts];
            this.ApplyFilters();
        }

        /// <summary>
        /// Applies the current search query, status filter, and verification filter to the accounts collection.
        /// </summary>
        public void ApplyFilters()
        {
            var filtered = this.seedAccounts.AsEnumerable();

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

            this.FilteredAccounts = filtered.ToList();
        }

        /// <summary>
        /// Handles initiating an account ownership transfer action.
        /// </summary>
        public void TransferOwnership()
        {
        }
    }
}
