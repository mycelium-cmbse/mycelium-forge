// ------------------------------------------------------------------------------------------------
// <copyright file="UserService.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Data;

    /// <summary>
    /// Scoped service that holds the identity, profile details, and assigned roles of the currently authenticated user.
    /// One instance per Blazor circuit ensures concurrent users are isolated.
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// Gets or sets the underlying account entity of the currently authenticated user, if available.
        /// </summary>
        private readonly IAccount currentUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService" /> class, populated with initial mock/seed user data.
        /// </summary>
        public UserService()
        {
            this.currentUser = SeedData.RegisAccount;
            this.CurrentRoles = [RoleKind.InstallationAdministrator, RoleKind.OrganizationAdministrator, RoleKind.Account];
        }

        /// <summary>
        /// Gets or sets the application and domain roles currently assigned to the user.
        /// </summary>
        /// <remarks>This will likely be removed when IAccount roles is implemented.</remarks>
        public IReadOnlyList<RoleKind> CurrentRoles { get; set; }

        /// <summary>
        /// Gets a value indicating whether the user is authenticated.
        /// </summary>
        public bool IsAuthenticated => this.currentUser != null && !this.CurrentRoles.Contains(RoleKind.Anonymous);

        /// <summary>
        /// Gets the current user account, optionally forcing a reload from the data source.
        /// </summary>
        /// <param name="forceLoad">The value indicating whether to force a reload.</param>
        /// <returns>The current user account.</returns>
        public IAccount GetCurrentUser(bool forceLoad = false)
        {
            return this.currentUser;
        }
    }
}
