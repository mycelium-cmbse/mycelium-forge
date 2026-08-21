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
    using FluentResults;

    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.Models.Organization;
    using Mycelium.Forge.Models.Profile;

    /// <summary>
    /// Defines the view model contract for the user account settings and profile page.
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
        List<AccountOrganizationMembershipModel> Organizations { get; set; }

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
        /// Creates a new organization associated with the user account.
        /// </summary>
        /// <param name="result">The organization creation data.</param>
        /// <returns>A <see cref="Result" /> indicating the success or failure of the operation.</returns>
        Result CreateOrganization(CreateOrganizationResult result);

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
