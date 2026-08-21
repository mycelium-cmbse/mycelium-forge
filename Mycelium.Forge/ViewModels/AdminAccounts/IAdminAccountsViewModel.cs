// ------------------------------------------------------------------------------------------------
// <copyright file="IAdminAccountsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.AdminAccounts
{
    using Mycelium.Forge.Models.Admin;

    /// <summary>
    /// Defines the view model contract for the installation accounts administration page.
    /// </summary>
    public interface IAdminAccountsViewModel
    {
        /// <summary>
        /// Gets or sets the collection of all accounts.
        /// </summary>
        List<AdminAccountModel> Accounts { get; set; }

        /// <summary>
        /// Gets or sets the filtered collection of accounts based on active filters.
        /// </summary>
        List<AdminAccountModel> FilteredAccounts { get; set; }

        /// <summary>
        /// Initializes the view model state and loads accounts.
        /// </summary>
        /// <param name="searchQuery">The initial search filter query string.</param>
        /// <param name="statusFilter">The initial status filter.</param>
        /// <param name="verificationFilter">The initial verification filter.</param>
        void InitializeViewModel(string searchQuery = "", string statusFilter = "All", string verificationFilter = "All");

        /// <summary>
        /// Applies the current search query, status filter, and verification filter to the accounts collection.
        /// </summary>
        /// <param name="searchQuery">The search query text filter.</param>
        /// <param name="statusFilter">The status filter.</param>
        /// <param name="verificationFilter">The verification filter.</param>
        void ApplyFilters(string searchQuery, string statusFilter, string verificationFilter);

        /// <summary>
        /// Handles initiating an account ownership transfer action.
        /// </summary>
        void TransferOwnership();
    }
}
