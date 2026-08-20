// ------------------------------------------------------------------------------------------------
// <copyright file="IAccountSettingsViewModel.cs" company="Starion Group S.A.">
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
    /// Defines the view model contract for the user account settings and profile management page.
    /// </summary>
    public interface IAccountSettingsViewModel
    {
        /// <summary>
        /// Gets or sets the user profile details.
        /// </summary>
        UserProfileModel Profile { get; set; }

        /// <summary>
        /// Gets or sets the collection of organization memberships associated with the user account.
        /// </summary>
        IReadOnlyList<AccountOrganizationMembershipModel> Organizations { get; set; }

        /// <summary>
        /// Initializes the view model state and populates initial user profile and organization data.
        /// </summary>
        void InitializeViewModel();

        /// <summary>
        /// Updates the user profile with the specified profile details.
        /// </summary>
        /// <param name="profile">The updated <see cref="UserProfileModel" /> data.</param>
        void UpdateProfile(UserProfileModel profile);

        /// <summary>
        /// Handles the deactivation of the current user account.
        /// </summary>
        void DeactivateAccount();

        /// <summary>
        /// Handles the deletion of the current user account.
        /// </summary>
        void DeleteAccount();
    }
}
