// ------------------------------------------------------------------------------------------------
// <copyright file="UserContext.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Default implementation of <see cref="IUserContext" /> carrying identity and assigned roles.
    /// </summary>
    public class UserContext : IUserContext
    {
        /// <summary>
        /// Gets or sets the unique identifier of the authenticated user account, if applicable.
        /// </summary>
        public Guid? AccountId { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the application and domain roles currently assigned to the user.
        /// </summary>
        public IReadOnlyList<RoleKind> CurrentRoles { get; set; } = [];

        /// <summary>
        /// Gets a value indicating whether the user is authenticated.
        /// </summary>
        public bool IsAuthenticated => this.AccountId.HasValue && !this.CurrentRoles.Contains(RoleKind.Anonymous);

        /// <summary>
        /// Determines whether the user context has the specified permission based on its current roles.
        /// </summary>
        /// <param name="permission">The permission to check.</param>
        /// <returns><c>true</c> if the user context has the permission; otherwise, <c>false</c>.</returns>
        public bool HasPermission(PermissionKind permission)
        {
            foreach (var role in this.CurrentRoles)
            {
                if (RolePermissionMap.RoleToPermissions.TryGetValue(role, out var permissions) && permissions.Contains(permission))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates an anonymous user context.
        /// </summary>
        /// <returns>An anonymous <see cref="IUserContext" /> with the <see cref="RoleKind.Anonymous" /> role.</returns>
        public static IUserContext CreateAnonymous()
        {
            return new UserContext
            {
                AccountId = null,
                Username = "anonymous",
                CurrentRoles = [RoleKind.Anonymous]
            };
        }
    }
}
