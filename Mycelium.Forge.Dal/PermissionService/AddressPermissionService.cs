// ------------------------------------------------------------------------------------------------
// <copyright file="AddressPermissionService.cs" company="Starion Group S.A.">
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
    /// Domain-specific authorization logic for <see cref="IAddress" /> supporting Account and Organization scopes.
    /// </summary>
    public partial class AddressPermissionService
    {
        /// <summary>
        /// Determines whether the user described by <paramref name="userContext" /> is allowed to create the specified address.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="toCreate">The address entity to create.</param>
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether creation is permitted.</returns>
        public override Task<Result> IsAllowedToCreate(IUserContext userContext, IAddress toCreate)
        {
            if (userContext is not { IsAuthenticated: true } || !userContext.AccountId.HasValue)
            {
                return Task.FromResult(Result.Fail("Unauthenticated user cannot create an address."));
            }

            if (toCreate == null)
            {
                return Task.FromResult(Result.Fail("Address cannot be null."));
            }

            if (toCreate.Owner == userContext.AccountId.Value &&
                PermissionGuard.HasPermission(userContext, PermissionKind.ManageOwnProfile))
            {
                return Task.FromResult(Result.Ok());
            }

            if (PermissionGuard.HasPermission(userContext, PermissionKind.ManageOrganizationSettings) ||
                PermissionGuard.HasPermission(userContext, PermissionKind.ManageOrganizations))
            {
                return Task.FromResult(Result.Ok());
            }

            return Task.FromResult(Result.Fail("Access denied: cannot create address for this scope."));
        }

        /// <summary>
        /// Determines whether the user described by <paramref name="userContext" /> is allowed to read the specified address.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="thing">The address entity to read.</param>
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether reading is permitted.</returns>
        public override Task<Result> IsAllowedToRead(IUserContext userContext, IAddress thing)
        {
            if (thing == null)
            {
                return Task.FromResult(Result.Fail("Address cannot be null."));
            }

            if (userContext is { AccountId: not null } && thing.Owner == userContext.AccountId.Value)
            {
                return Task.FromResult(Result.Ok());
            }

            if (PermissionGuard.HasPermission(userContext, PermissionKind.ViewOrganizationMemberList) ||
                PermissionGuard.HasPermission(userContext, PermissionKind.ViewAllOrganizations) ||
                PermissionGuard.HasPermission(userContext, PermissionKind.ViewAllAccounts))
            {
                return Task.FromResult(Result.Ok());
            }

            return Task.FromResult(Result.Fail("Access denied: cannot view this address."));
        }

        /// <summary>
        /// Determines whether the user described by <paramref name="userContext" /> is allowed to update the specified address,
        /// evaluating the transition from <paramref name="existingThing" /> to <paramref name="updatedThing" />.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="existingThing">The existing persisted address entity state.</param>
        /// <param name="updatedThing">The updated address entity state.</param>
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether updating is permitted.</returns>
        public override Task<Result> IsAllowedToUpdate(IUserContext userContext, IAddress existingThing, IAddress updatedThing)
        {
            if (userContext is not { IsAuthenticated: true } || !userContext.AccountId.HasValue)
            {
                return Task.FromResult(Result.Fail("Unauthenticated user cannot update an address."));
            }

            if (existingThing == null || updatedThing == null)
            {
                return Task.FromResult(Result.Fail("Address cannot be null."));
            }

            if (existingThing.Owner == userContext.AccountId.Value &&
                PermissionGuard.HasPermission(userContext, PermissionKind.ManageOwnProfile))
            {
                return Task.FromResult(Result.Ok());
            }

            if (PermissionGuard.HasPermission(userContext, PermissionKind.ManageOrganizationSettings) ||
                PermissionGuard.HasPermission(userContext, PermissionKind.ManageOrganizations))
            {
                return Task.FromResult(Result.Ok());
            }

            return Task.FromResult(Result.Fail("Access denied: cannot update address for this scope."));
        }
    }
}
