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
    using FluentResults;

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
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether creation is permitted.</returns>
        public virtual Task<Result> IsAllowedToCreate(IUserContext userContext, TThing toCreate)
        {
            return this.IsAllowedToCreateImplementation(userContext, toCreate);
        }

        /// <summary>
        /// Determines whether the user described by <paramref name="userContext" /> is allowed to read the specified entity.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="thing">The entity to read.</param>
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether reading is permitted.</returns>
        public virtual Task<Result> IsAllowedToRead(IUserContext userContext, TThing thing)
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
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether updating is permitted.</returns>
        public virtual Task<Result> IsAllowedToUpdate(IUserContext userContext, TThing existingThing, TThing updatedThing)
        {
            return this.IsAllowedToUpdateImplementation(userContext, existingThing, updatedThing);
        }

        /// <summary>
        /// Determines whether the user described by <paramref name="userContext" /> is allowed to delete the specified entity.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="thing">The entity to delete.</param>
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether deletion is permitted.</returns>
        public virtual Task<Result> IsAllowedToDelete(IUserContext userContext, TThing thing)
        {
            return this.IsAllowedToDeleteImplementation(userContext, thing);
        }

        /// <summary>
        /// Core implementation hook for verifying create permissions on <paramref name="toCreate" />.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="toCreate">The entity to create.</param>
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether creation is permitted.</returns>
        protected virtual Task<Result> IsAllowedToCreateImplementation(IUserContext userContext, TThing toCreate)
        {
            return Task.FromResult(Result.Ok());
        }

        /// <summary>
        /// Core implementation hook for verifying read permissions on <paramref name="thing" />.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="thing">The entity to read.</param>
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether reading is permitted.</returns>
        protected virtual Task<Result> IsAllowedToReadImplementation(IUserContext userContext, TThing thing)
        {
            return Task.FromResult(Result.Ok());
        }

        /// <summary>
        /// Core implementation hook for verifying update permissions comparing <paramref name="existingThing" /> and
        /// <paramref name="updatedThing" />.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="existingThing">The existing persisted entity state.</param>
        /// <param name="updatedThing">The updated entity state.</param>
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether updating is permitted.</returns>
        protected virtual Task<Result> IsAllowedToUpdateImplementation(IUserContext userContext, TThing existingThing, TThing updatedThing)
        {
            return Task.FromResult(Result.Ok());
        }

        /// <summary>
        /// Core implementation hook for verifying delete permissions on <paramref name="thing" />.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="thing">The entity to delete.</param>
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether deletion is permitted.</returns>
        protected virtual Task<Result> IsAllowedToDeleteImplementation(IUserContext userContext, TThing thing)
        {
            return Task.FromResult(Result.Ok());
        }
    }
}
