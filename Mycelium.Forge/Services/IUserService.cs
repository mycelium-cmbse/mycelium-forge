// ------------------------------------------------------------------------------------------------
// <copyright file="IUserService.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    using Mycelium.Forge.Common;

    /// <summary>
    /// Provides access to the identity, profile details, and assigned roles of the currently authenticated user within the
    /// scope of a Blazor circuit.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets or sets the application and domain roles currently assigned to the user.
        /// </summary>
        /// <remarks>This will likely be removed when IAccount roles is implemented.</remarks>
        IReadOnlyList<RoleKind> CurrentRoles { get; set; }

        /// <summary>
        /// Gets the current user account, optionally forcing a reload from the data source.
        /// </summary>
        /// <param name="forceLoad">The value indicating whether to force a reload.</param>
        /// <returns>The current user account.</returns>
        IAccount GetCurrentUser(bool forceLoad = false);

        /// <summary>
        /// Gets the user context, which includes the account ID, username, and current roles of the authenticated user.
        /// </summary>
        /// <returns>The user context.</returns>
        IUserContext GetUserContext();
    }
}
