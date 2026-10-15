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
        IAccount Profile { get; set; }

        /// <summary>
        /// Gets or sets the collection of organizations associated with the user account.
        /// </summary>
        IReadOnlyList<IOrganization> Organizations { get; set; }

        /// <summary>
        /// Gets or sets the primary address location string.
        /// </summary>
        string Location { get; set; }

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
