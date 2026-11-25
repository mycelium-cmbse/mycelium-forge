// ------------------------------------------------------------------------------------------------
// <copyright file="IAccountSettingsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.AccountSettings
{
    using DynamicData;

    using ErrorOr;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Models.DialogResults;

    /// <summary>
    /// Defines the view model contract for the user account settings and profile page.
    /// </summary>
    public interface IAccountSettingsViewModel
    {
        /// <summary>
        /// Gets or sets the user profile account DTO.
        /// </summary>
        Account Profile { get; set; }

        /// <summary>
        /// Gets the collection of organizations associated with the user account.
        /// </summary>
        SourceList<IOrganization> Organizations { get; }

        /// <summary>
        /// Gets the view model for managing pending requests.
        /// </summary>
        IPendingRequestsManagerViewModel PendingRequestsManagerViewModel { get; }

        /// <summary>
        /// Gets the view model for managing the addresses.
        /// </summary>
        IAddressesManagerViewModel AddressesManagerViewModel { get; }

        /// <summary>
        /// Initializes the view model state and populates initial user profile and organization data asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous initialization.</returns>
        Task InitializeViewModel();

        /// <summary>
        /// Creates a new organization associated with the user account asynchronously.
        /// </summary>
        /// <param name="result">The organization creation data.</param>
        /// <returns>A task indicating the success or failure of the operation.</returns>
        Task<ErrorOr<Success>> CreateOrganization(CreateOrganizationResult result);

        /// <summary>
        /// Updates the user profile account and synchronizes identity changes asynchronously.
        /// </summary>
        /// <param name="updatedAccount">The updated <see cref="Account" /> DTO.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating the success or failure of the operation.</returns>
        Task<ErrorOr<Success>> UpdateProfile(Account updatedAccount);

        /// <summary>
        /// Handles the deactivation of the current user account asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task DeactivateAccount();

        /// <summary>
        /// Handles the deletion of the current user account asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task DeleteAccount();
    }
}
