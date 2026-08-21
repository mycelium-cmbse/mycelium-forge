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
    using FluentResults;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Data;
    using Mycelium.Forge.Models;
    using Mycelium.Forge.Models.DialogResults;

    /// <summary>
    /// Provides view model state and operations for the user account settings and profile management page.
    /// </summary>
    public class AccountSettingsViewModel : IAccountSettingsViewModel
    {
        /// <summary>
        /// Gets or sets the user profile details.
        /// </summary>
        public UserProfileModel Profile { get; set; }

        /// <summary>
        /// Gets or sets the collection of organization memberships associated with the user account.
        /// </summary>
        public List<AccountOrganizationMembershipModel> Organizations { get; set; } = [];

        /// <summary>
        /// Initializes the view model state and populates initial user profile and organization data.
        /// </summary>
        public void InitializeViewModel()
        {
            this.Profile = new UserProfileModel(
                SeedData.RegisAccount,
                "Starion Group",
                "Systems engineer, concurrent design.");

            this.Organizations = [.. SeedData.RegisOrganizationMemberships];
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
        /// Creates a new organization with the specified creation details.
        /// </summary>
        /// <param name="result">The organization creation data.</param>
        /// <returns>A <see cref="Result" /> indicating the success or failure of the operation.</returns>
        public Result CreateOrganization(CreateOrganizationResult result)
        {
            if (result == null || string.IsNullOrWhiteSpace(result.OrganizationName))
            {
                return Result.Fail("Organization name is required.");
            }

            var cleanScope = result.Scope?.TrimStart('@').ToLowerInvariant() ?? string.Empty;

            var org = new Organization
            {
                Name = result.OrganizationName,
                ShortName = cleanScope,
                BillingEmail = result.BillingEmail
            };

            var membership = new AccountOrganizationMembershipModel(org, OrganizationInvitationKind.ADMINISTRATOR);
            this.Organizations.Add(membership);

            return Result.Ok();
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
