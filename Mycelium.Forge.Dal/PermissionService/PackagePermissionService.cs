// ------------------------------------------------------------------------------------------------
// <copyright file="PackagePermissionService.cs" company="Starion Group S.A.">
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
    /// Domain-specific authorization logic for <see cref="IPackage" />.
    /// </summary>
    public partial class PackagePermissionService
    {
        /// <summary>
        /// Determines whether the user described by <paramref name="userContext" /> is allowed to update the specified package,
        /// evaluating diffs between existing and updated entity states.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="existingThing">The existing persisted package entity state.</param>
        /// <param name="updatedThing">The updated package entity state.</param>
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether updating is permitted.</returns>
        public override Task<Result> IsAllowedToUpdate(IUserContext userContext, IPackage existingThing, IPackage updatedThing)
        {
            if (userContext is not { IsAuthenticated: true } || !userContext.AccountId.HasValue)
            {
                return Task.FromResult(Result.Fail("Unauthenticated user cannot update a package."));
            }

            if (existingThing == null || updatedThing == null)
            {
                return Task.FromResult(Result.Fail("Package cannot be null."));
            }

            if (PermissionGuard.HasPermission(userContext, PermissionKind.DeletePackage))
            {
                return Task.FromResult(Result.Ok());
            }

            if (!existingThing.PackageOwner.Contains(userContext.AccountId.Value))
            {
                return Task.FromResult(Result.Fail("Access denied: only current package owners can update a package."));
            }

            // 1. Check Ownership Transfer
            if (existingThing.Owner != updatedThing.Owner)
            {
                if (existingThing.Owner != userContext.AccountId.Value &&
                    !existingThing.PackageOwner.Contains(userContext.AccountId.Value))
                {
                    return Task.FromResult(Result.Fail("Access denied: only the current package owner can transfer ownership."));
                }

                var guard = PermissionGuard.GuardPermission(userContext, PermissionKind.TransferPackageOwnership);

                if (guard.IsFailed)
                {
                    return Task.FromResult(guard);
                }
            }

            // 2. Check Team Modification
            if (!existingThing.PackageMaintainer.SequenceEqual(updatedThing.PackageMaintainer) ||
                !existingThing.PackageOwner.SequenceEqual(updatedThing.PackageOwner))
            {
                var guard = PermissionGuard.GuardPermission(userContext, PermissionKind.ManagePackageTeam);

                if (guard.IsFailed)
                {
                    return Task.FromResult(guard);
                }
            }

            // 3. Check Visibility Change
            if (existingThing.Visibility != updatedThing.Visibility)
            {
                var guard = PermissionGuard.GuardPermission(userContext, PermissionKind.SetPackageVisibility);

                if (guard.IsFailed)
                {
                    return Task.FromResult(guard);
                }
            }

            // 4. General Settings Update
            return Task.FromResult(PermissionGuard.GuardPermission(userContext, PermissionKind.ManagePackageSettings));
        }
    }
}
