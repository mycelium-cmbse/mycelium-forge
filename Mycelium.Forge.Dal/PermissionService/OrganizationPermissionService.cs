// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationPermissionService.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.AutoGenPermissionService
{
    using FluentResults;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Domain-specific authorization and membership logic for <see cref="IOrganization" />.
    /// </summary>
    public partial class OrganizationPermissionService
    {
        /// <summary>
        /// Determines whether the user described by <paramref name="userContext" /> is allowed to update the specified
        /// organization,
        /// evaluating diffs between existing and updated entity states.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="existingThing">The existing persisted organization entity state.</param>
        /// <param name="updatedThing">The updated organization entity state.</param>
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether updating is permitted.</returns>
        public override Task<Result> IsAllowedToUpdate(IUserContext userContext, IOrganization existingThing, IOrganization updatedThing)
        {
            if (userContext is not { IsAuthenticated: true } || !userContext.AccountId.HasValue)
            {
                return Task.FromResult(Result.Fail("Unauthenticated user cannot update an organization."));
            }

            if (existingThing == null || updatedThing == null)
            {
                return Task.FromResult(Result.Fail("Organization cannot be null."));
            }

            if (PermissionGuard.HasPermission(userContext, PermissionKind.ManageOrganizations))
            {
                return Task.FromResult(Result.Ok());
            }

            if (!existingThing.Administrator.Contains(userContext.AccountId.Value))
            {
                return Task.FromResult(Result.Fail("Access denied: only current organization administrators can update the organization."));
            }

            // 1. Roles / Administrator Membership change
            if (!existingThing.Administrator.SequenceEqual(updatedThing.Administrator))
            {
                var guard = PermissionGuard.GuardPermission(userContext, PermissionKind.TransferOrganizationAdministration);

                if (guard.IsFailed)
                {
                    return Task.FromResult(guard);
                }
            }

            // 2. Member change
            if (!existingThing.Member.SequenceEqual(updatedThing.Member))
            {
                var guard = PermissionGuard.GuardPermission(userContext, PermissionKind.InviteOrganizationMembers);

                if (guard.IsFailed)
                {
                    return Task.FromResult(guard);
                }
            }

            // 3. Default visibility change
            if (existingThing.DefaultPackageVisibility != updatedThing.DefaultPackageVisibility)
            {
                var guard = PermissionGuard.GuardPermission(userContext, PermissionKind.ConfigureDefaultPackageVisibility);

                if (guard.IsFailed)
                {
                    return Task.FromResult(guard);
                }
            }

            // 4. General Settings Update
            return Task.FromResult(PermissionGuard.GuardPermission(userContext, PermissionKind.ManageOrganizationSettings));
        }
    }
}
