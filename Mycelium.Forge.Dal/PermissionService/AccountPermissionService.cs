// ------------------------------------------------------------------------------------------------
// <copyright file="AccountPermissionService.cs" company="Starion Group S.A.">
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
    /// Domain-specific authorization logic for <see cref="IAccount" />.
    /// </summary>
    public partial class AccountPermissionService
    {
        /// <summary>
        /// Determines whether the user described by <paramref name="userContext" /> is allowed to read the specified account
        /// profile.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="thing">The account entity to read.</param>
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether reading is permitted.</returns>
        public override Task<Result> IsAllowedToRead(IUserContext userContext, IAccount thing)
        {
            if (thing == null)
            {
                return Task.FromResult(Result.Fail("Account cannot be null."));
            }

            // Public account profile is universally crawlable and readable per SSS-FG-REG-W9J
            return Task.FromResult(Result.Ok());
        }

        /// <summary>
        /// Determines whether the user described by <paramref name="userContext" /> is allowed to update the specified account,
        /// evaluating the transition from <paramref name="existingThing" /> to <paramref name="updatedThing" />.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="existingThing">The existing persisted account entity state.</param>
        /// <param name="updatedThing">The updated account entity state.</param>
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether updating is permitted.</returns>
        public override Task<Result> IsAllowedToUpdate(IUserContext userContext, IAccount existingThing, IAccount updatedThing)
        {
            if (userContext is not { IsAuthenticated: true } || !userContext.AccountId.HasValue)
            {
                return Task.FromResult(Result.Fail("Unauthenticated user cannot update an account."));
            }

            if (existingThing == null || updatedThing == null)
            {
                return Task.FromResult(Result.Fail("Account cannot be null."));
            }

            if (existingThing.Id == userContext.AccountId.Value &&
                PermissionGuard.HasPermission(userContext, PermissionKind.ManageOwnProfile))
            {
                return Task.FromResult(Result.Ok());
            }

            if (PermissionGuard.HasPermission(userContext, PermissionKind.ManageAccounts))
            {
                return Task.FromResult(Result.Ok());
            }

            return Task.FromResult(Result.Fail("Access denied: cannot update account profile."));
        }
    }
}
