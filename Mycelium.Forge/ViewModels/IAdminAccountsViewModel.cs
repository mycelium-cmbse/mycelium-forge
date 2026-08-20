// ------------------------------------------------------------------------------------------------
// <copyright file="IAdminAccountsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels
{
    using Mycelium.Forge.Models;

    /// <summary>
    /// Defines the contract for the view model managing installation administration accounts.
    /// </summary>
    public interface IAdminAccountsViewModel
    {
        /// <summary>
        /// Gets or sets the collection of all accounts.
        /// </summary>
        IReadOnlyList<AdminAccountModel> Accounts { get; set; }

        /// <summary>
        /// Gets or sets the filtered collection of accounts based on search query, status, and verification filters.
        /// </summary>
        IReadOnlyList<AdminAccountModel> FilteredAccounts { get; set; }

        /// <summary>
        /// Gets or sets the search filter query string.
        /// </summary>
        string SearchQuery { get; set; }

        /// <summary>
        /// Gets or sets the selected account status filter.
        /// </summary>
        string SelectedStatusFilter { get; set; }

        /// <summary>
        /// Gets or sets the selected verification status filter.
        /// </summary>
        string SelectedVerificationFilter { get; set; }

        /// <summary>
        /// Gets the available status filter options.
        /// </summary>
        IReadOnlyList<string> StatusFilterOptions { get; }

        /// <summary>
        /// Gets the available verification filter options.
        /// </summary>
        IReadOnlyList<string> VerificationFilterOptions { get; }

        /// <summary>
        /// Initializes the view model state and loads initial seed accounts.
        /// </summary>
        void InitializeViewModel();

        /// <summary>
        /// Applies the current search query, status filter, and verification filter to the accounts collection.
        /// </summary>
        void ApplyFilters();

        /// <summary>
        /// Handles initiating an account ownership transfer action.
        /// </summary>
        void TransferOwnership();
    }
}
