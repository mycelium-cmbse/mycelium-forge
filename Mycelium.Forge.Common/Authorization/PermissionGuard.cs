// ------------------------------------------------------------------------------------------------
// <copyright file="PermissionGuard.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    using System.Linq;

    using FluentResults;

    /// <summary>
    /// Provides static permission checking and operation guarding methods.
    /// </summary>
    public static class PermissionGuard
    {
        /// <summary>
        /// Determines whether the user described by <paramref name="userContext" /> has the specified
        /// <see cref="PermissionKind" />.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="permission">The permission to check.</param>
        /// <returns><c>true</c> if the user has the permission; otherwise, <c>false</c>.</returns>
        public static bool HasPermission(IUserContext userContext, PermissionKind permission)
        {
            if (userContext == null || userContext.CurrentRoles.Count == 0)
            {
                return false;
            }

            foreach (var role in userContext.CurrentRoles)
            {
                if (!RolePermissionMap.RoleToPermissions.TryGetValue(role, out var rolePermissions))
                {
                    continue;
                }

                if (rolePermissions.Contains(permission))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Guards an operation by verifying that the user described by <paramref name="userContext" />
        /// has the specified <see cref="PermissionKind" />.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="permission">The permission required for the operation.</param>
        /// <returns>A <see cref="Result" /> indicating whether the permission is granted.</returns>
        public static Result GuardPermission(IUserContext userContext, PermissionKind permission)
        {
            if (HasPermission(userContext, permission))
            {
                return Result.Ok();
            }

            var username = userContext != null ? userContext.Username : "unknown";
            var roles = userContext != null ? string.Join(", ", userContext.CurrentRoles.Select(r => r.ToString())) : string.Empty;

            return Result.Fail($"Access denied: user '{username}' with roles [{roles}] does not have the '{permission}' permission.");
        }
    }
}
