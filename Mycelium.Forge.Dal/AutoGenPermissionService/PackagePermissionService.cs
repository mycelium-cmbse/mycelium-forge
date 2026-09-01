// ------------------------------------------------------------------------------------------------
// <copyright file="PackagePermissionService.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.AutoGenPermissionService
{
    using System.CodeDom.Compiler;

    using FluentResults;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.PermissionService;

    /// <summary>
    /// Default permission service for <see cref="IPackage"/>.
    /// </summary>
    [GeneratedCode("Mycelium.Forge.Generator", "1.0.0")]
    public partial class PackagePermissionService : PermissionServiceBase<IPackage>, IPackagePermissionService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackagePermissionService"/> class.
        /// </summary>
        public PackagePermissionService()
        {
        }

        /// <summary>
        /// Core implementation hook for verifying create permissions on <paramref name="toCreate"/>.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="toCreate">The entity to create.</param>
        /// <returns>An awaitable <see cref="Task{Result}"/> indicating whether creation is permitted.</returns>
        protected override Task<Result> IsAllowedToCreateImplementation(IUserContext userContext, IPackage toCreate)
        {
            return Task.FromResult(PermissionGuard.GuardPermission(userContext, PermissionKind.PublishPackageToPersonalScope));
        }

        /// <summary>
        /// Core implementation hook for verifying read permissions on <paramref name="thing"/>.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="thing">The entity to read.</param>
        /// <returns>An awaitable <see cref="Task{Result}"/> indicating whether reading is permitted.</returns>
        protected override Task<Result> IsAllowedToReadImplementation(IUserContext userContext, IPackage thing)
        {
            if (userContext.AccountId.HasValue && thing.PackageOwner.Contains(userContext.AccountId.Value))
            {
                return Task.FromResult(Result.Ok());
            }

            if (userContext.AccountId.HasValue && thing.PackageMaintainer.Contains(userContext.AccountId.Value))
            {
                return Task.FromResult(Result.Ok());
            }

            if (thing.Visibility == VisibilityKind.PUBLIC)
            {
                return Task.FromResult(PermissionGuard.GuardPermission(userContext, PermissionKind.ReadPublicPackage));
            }

            if (thing.Visibility == VisibilityKind.INTERNAL)
            {
                return Task.FromResult(PermissionGuard.GuardPermission(userContext, PermissionKind.ReadOrganizationVisiblePackage));
            }

            if (thing.Visibility == VisibilityKind.PRIVATE)
            {
                return Task.FromResult(PermissionGuard.GuardPermission(userContext, PermissionKind.ReadPrivatePackage));
            }

            return Task.FromResult(PermissionGuard.GuardPermission(userContext, PermissionKind.ReadPublicPackage));
        }

        /// <summary>
        /// Core implementation hook for verifying update permissions comparing <paramref name="existingThing"/> and <paramref name="updatedThing"/>.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="existingThing">The existing persisted entity state.</param>
        /// <param name="updatedThing">The updated entity state.</param>
        /// <returns>An awaitable <see cref="Task{Result}"/> indicating whether updating is permitted.</returns>
        protected override Task<Result> IsAllowedToUpdateImplementation(IUserContext userContext, IPackage existingThing, IPackage updatedThing)
        {
            if (userContext.AccountId.HasValue && existingThing.PackageOwner.Contains(userContext.AccountId.Value))
            {
                return Task.FromResult(Result.Ok());
            }

            return Task.FromResult(PermissionGuard.GuardPermission(userContext, PermissionKind.ManagePackageSettings));
        }

        /// <summary>
        /// Core implementation hook for verifying delete permissions on an entity by its <paramref name="id"/>.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="id">The unique identifier of the entity to delete.</param>
        /// <returns>An awaitable <see cref="Task{Result}"/> indicating whether deletion is permitted.</returns>
        protected override Task<Result> IsAllowedToDeleteImplementation(IUserContext userContext, Guid id)
        {
            return Task.FromResult(PermissionGuard.GuardPermission(userContext, PermissionKind.DeletePackage));
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
