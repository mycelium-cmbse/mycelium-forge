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
    using System;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Models;
    using Mycelium.Forge.ViewModels;

    /// <summary>
    /// Code-behind logic for the installation administration accounts management page.
    /// </summary>
    public partial class Accounts : ComponentBase
    {
        /// <summary>
        /// Gets or sets the view model managing installation administration accounts state.
        /// </summary>
        [Inject]
        public IAdminAccountsViewModel ViewModel { get; set; }

        /// <summary>
        /// Initializes the component and loads initial view model state.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.ViewModel.InitializeViewModel();
        }

        /// <summary>
        /// Handles search input query changes and triggers account filtering.
        /// </summary>
        /// <param name="eventArgs">The change event arguments containing the new search value.</param>
        private void OnSearchInputChanged(ChangeEventArgs eventArgs)
        {
            this.ViewModel.SearchQuery = eventArgs.Value?.ToString() ?? string.Empty;
            this.ViewModel.ApplyFilters();
        }

        /// <summary>
        /// Handles status filter dropdown selection changes.
        /// </summary>
        /// <param name="eventArgs">The change event arguments containing the selected status.</param>
        private void OnStatusFilterChanged(ChangeEventArgs eventArgs)
        {
            this.ViewModel.SelectedStatusFilter = eventArgs.Value?.ToString() ?? "All";
            this.ViewModel.ApplyFilters();
        }

        /// <summary>
        /// Handles verification status filter dropdown selection changes.
        /// </summary>
        /// <param name="eventArgs">The change event arguments containing the selected verification filter.</param>
        private void OnVerificationFilterChanged(ChangeEventArgs eventArgs)
        {
            this.ViewModel.SelectedVerificationFilter = eventArgs.Value?.ToString() ?? "All";
            this.ViewModel.ApplyFilters();
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
