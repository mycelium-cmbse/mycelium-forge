// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationInvitationPermissionService.cs" company="Starion Group S.A.">
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
    /// Domain-specific authorization logic for <see cref="IOrganizationInvitation" />.
    /// </summary>
    public partial class OrganizationInvitationPermissionService
    {
        /// <summary>
        /// Determines whether the user described by <paramref name="userContext" /> is allowed to read the specified invitation.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="thing">The invitation entity to read.</param>
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether reading is permitted.</returns>
        public override Task<Result> IsAllowedToRead(IUserContext userContext, IOrganizationInvitation thing)
        {
            if (thing == null)
            {
                return Task.FromResult(Result.Fail("Invitation cannot be null."));
            }

            if (userContext is { AccountId: not null })
            {
                if (thing.Target == userContext.AccountId.Value || thing.Owner == userContext.AccountId.Value)
                {
                    return Task.FromResult(Result.Ok());
                }
            }

            if (PermissionGuard.HasPermission(userContext, PermissionKind.ManageOrganizations) ||
                PermissionGuard.HasPermission(userContext, PermissionKind.ViewOrganizationMemberList))
            {
                return Task.FromResult(Result.Ok());
            }

            return Task.FromResult(Result.Fail("Access denied: cannot view this organization invitation."));
        }

        /// <summary>
        /// Determines whether the user described by <paramref name="userContext" /> is allowed to update the specified invitation,
        /// evaluating the transition from <paramref name="existingThing" /> to <paramref name="updatedThing" />.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="existingThing">The existing persisted invitation entity state.</param>
        /// <param name="updatedThing">The updated invitation entity state.</param>
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether updating is permitted.</returns>
        public override Task<Result> IsAllowedToUpdate(IUserContext userContext, IOrganizationInvitation existingThing, IOrganizationInvitation updatedThing)
        {
            if (userContext is not { IsAuthenticated: true } || !userContext.AccountId.HasValue)
            {
                return Task.FromResult(Result.Fail("Unauthenticated user cannot update an invitation."));
            }

            if (existingThing == null || updatedThing == null)
            {
                return Task.FromResult(Result.Fail("Invitation cannot be null."));
            }

            if (existingThing.Target == userContext.AccountId.Value)
            {
                return Task.FromResult(PermissionGuard.GuardPermission(userContext, PermissionKind.AcceptOrganizationInvitation));
            }

            if (existingThing.Owner == userContext.AccountId.Value ||
                PermissionGuard.HasPermission(userContext, PermissionKind.ManageOrganizations))
            {
                return Task.FromResult(Result.Ok());
            }

            return Task.FromResult(Result.Fail("Access denied: cannot update organization invitation."));
        }
    }
}
