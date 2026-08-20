// ------------------------------------------------------------------------------------------------
// <copyright file="AccountSettingsViewModel.cs" company="Starion Group S.A.">
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
    /// Provides view model state and operations for the user account settings and profile management page.
    /// </summary>
    public class AccountSettingsViewModel : IAccountSettingsViewModel
    {
        /// <summary>
        /// The initial seed organization memberships.
        /// </summary>
        private readonly List<AccountOrganizationMembershipModel> seedOrganizations =
        [
            new("@starion", "Starion Group", "SG", "Organization Administrator", true),
            new("@esa", "European Space Agency", "ES", "Organization Administrator")
        ];

        /// <summary>
        /// The initial seed profile information.
        /// </summary>
        private readonly UserProfileModel seedProfile = new(
            "r.andre",
            "regis.andre@starion.eu",
            true,
            "R. André",
            "Starion Group",
            "Toulouse, France",
            "stariongroup.eu",
            "Systems engineer, concurrent design.");

        /// <summary>
        /// Gets or sets the user profile details.
        /// </summary>
        public UserProfileModel Profile { get; set; }

        /// <summary>
        /// Gets or sets the collection of organization memberships associated with the user account.
        /// </summary>
        public IReadOnlyList<AccountOrganizationMembershipModel> Organizations { get; set; } = [];

        /// <summary>
        /// Initializes the view model state and populates initial user profile and organization data.
        /// </summary>
        public void InitializeViewModel()
        {
            this.Profile = this.seedProfile;
            this.Organizations = [.. this.seedOrganizations];
        }

        /// <summary>
        /// Updates the user profile with the specified profile details.
        /// </summary>
        /// <param name="profile">The updated <see cref="UserProfileModel" /> data.</param>
        public void UpdateProfile(UserProfileModel profile)
        {
            this.Profile = profile;
        }

        /// <summary>
        /// Handles the deactivation of the current user account.
        /// </summary>
        public void DeactivateAccount()
        {
        }

        /// <summary>
        /// Handles the deletion of the current user account.
        /// </summary>
        public void DeleteAccount()
        {
        }
    }
}
