// ------------------------------------------------------------------------------------------------
// <copyright file="PermissionServiceBase.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.PermissionService
{
    using ErrorOr;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Abstract base class providing common permission resolution and guard logic for domain entities.
    /// </summary>
    /// <typeparam name="TThing">The domain entity type implementing <see cref="IThing" />.</typeparam>
    public abstract class PermissionServiceBase<TThing> : IPermissionService<TThing> where TThing : IThing
    {
        /// <summary>
        /// Determines whether the user described by <paramref name="userContext" /> is allowed to create the specified entity.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="toCreate">The entity to create.</param>
        /// <returns>An awaitable <see cref="Task{ErrorOr}" /> indicating whether creation is permitted.</returns>
        public virtual Task<ErrorOr<Success>> IsAllowedToCreate(IUserContext userContext, TThing toCreate)
        {
            return this.IsAllowedToCreateImplementation(userContext, toCreate);
        }

        /// <summary>
        /// Determines whether the user described by <paramref name="userContext" /> is allowed to read the specified entity.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="thing">The entity to read.</param>
        /// <returns>An awaitable <see cref="Task{ErrorOr}" /> indicating whether reading is permitted.</returns>
        public virtual Task<ErrorOr<Success>> IsAllowedToRead(IUserContext userContext, TThing thing)
        {
            return this.IsAllowedToReadImplementation(userContext, thing);
        }

        /// <summary>
        /// Determines whether the user described by <paramref name="userContext" /> is allowed to update the specified entity,
        /// evaluating the transition from <paramref name="existingThing" /> to <paramref name="updatedThing" />.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="existingThing">The existing persisted entity state.</param>
        /// <param name="updatedThing">The updated entity state.</param>
        /// <returns>An awaitable <see cref="Task{ErrorOr}" /> indicating whether updating is permitted.</returns>
        public virtual Task<ErrorOr<Success>> IsAllowedToUpdate(IUserContext userContext, TThing existingThing, TThing updatedThing)
        {
            return this.IsAllowedToUpdateImplementation(userContext, existingThing, updatedThing);
        }

        /// <summary>
        /// Determines whether the user described by <paramref name="userContext" /> is allowed to delete the specified entity.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="thing">The entity to delete.</param>
        /// <returns>An awaitable <see cref="Task{ErrorOr}" /> indicating whether deletion is permitted.</returns>
        public virtual Task<ErrorOr<Success>> IsAllowedToDelete(IUserContext userContext, TThing thing)
        {
            return this.IsAllowedToDeleteImplementation(userContext, thing);
        }

        /// <summary>
        /// Core implementation hook for verifying create permissions on <paramref name="toCreate" />.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="toCreate">The entity to create.</param>
        /// <returns>An awaitable <see cref="Task{ErrorOr}" /> indicating whether creation is permitted.</returns>
        protected virtual Task<ErrorOr<Success>> IsAllowedToCreateImplementation(IUserContext userContext, TThing toCreate)
        {
            return Task.FromResult<ErrorOr<Success>>(Result.Success);
        }

        /// <summary>
        /// Core implementation hook for verifying read permissions on <paramref name="thing" />.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="thing">The entity to read.</param>
        /// <returns>An awaitable <see cref="Task{ErrorOr}" /> indicating whether reading is permitted.</returns>
        protected virtual Task<ErrorOr<Success>> IsAllowedToReadImplementation(IUserContext userContext, TThing thing)
        {
            return Task.FromResult<ErrorOr<Success>>(Result.Success);
        }

        /// <summary>
        /// Core implementation hook for verifying update permissions comparing <paramref name="existingThing" /> and
        /// <paramref name="updatedThing" />.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="existingThing">The existing persisted entity state.</param>
        /// <param name="updatedThing">The updated entity state.</param>
        /// <returns>An awaitable <see cref="Task{ErrorOr}" /> indicating whether updating is permitted.</returns>
        protected virtual Task<ErrorOr<Success>> IsAllowedToUpdateImplementation(IUserContext userContext, TThing existingThing, TThing updatedThing)
        {
            return Task.FromResult<ErrorOr<Success>>(Result.Success);
        }

        /// <summary>
        /// Core implementation hook for verifying delete permissions on <paramref name="thing" />.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="thing">The entity to delete.</param>
        /// <returns>An awaitable <see cref="Task{ErrorOr}" /> indicating whether deletion is permitted.</returns>
        protected virtual Task<ErrorOr<Success>> IsAllowedToDeleteImplementation(IUserContext userContext, TThing thing)
        {
            return Task.FromResult<ErrorOr<Success>>(Result.Success);
        }
    }
}
