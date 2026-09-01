// ------------------------------------------------------------------------------------------------
// <copyright file="IPermissionService.cs" company="Starion Group S.A.">
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
    /// Provides permission evaluation and authorization guard operations for entities of type <typeparamref name="TThing" />.
    /// </summary>
    /// <typeparam name="TThing">The type of the domain entity, which must implement <see cref="IThing" />.</typeparam>
    public interface IPermissionService<in TThing> where TThing : IThing
    {
        /// <summary>
        /// Determines whether the user described by <paramref name="userContext" /> is allowed to create the specified entity.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="toCreate">The entity to create.</param>
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether creation is permitted.</returns>
        Task<Result> IsAllowedToCreate(IUserContext userContext, TThing toCreate);

        /// <summary>
        /// Determines whether the user described by <paramref name="userContext" /> is allowed to read the specified entity.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="thing">The entity to read.</param>
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether reading is permitted.</returns>
        Task<Result> IsAllowedToRead(IUserContext userContext, TThing thing);

        /// <summary>
        /// Determines whether the user described by <paramref name="userContext" /> is allowed to update the specified entity,
        /// evaluating the transition from <paramref name="existingThing" /> to <paramref name="updatedThing" />.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="existingThing">The existing persisted entity state.</param>
        /// <param name="updatedThing">The updated entity state.</param>
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether updating is permitted.</returns>
        Task<Result> IsAllowedToUpdate(IUserContext userContext, TThing existingThing, TThing updatedThing);

        /// <summary>
        /// Determines whether the user described by <paramref name="userContext" /> is allowed to delete the entity with the
        /// specified identifier.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="id">The unique identifier of the entity to delete.</param>
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether deletion is permitted.</returns>
        Task<Result> IsAllowedToDelete(IUserContext userContext, Guid id);

        /// <summary>
        /// Determines whether the user described by <paramref name="userContext" /> is allowed to delete the specified entity.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="thing">The entity to delete.</param>
        /// <returns>An awaitable <see cref="Task{Result}" /> indicating whether deletion is permitted.</returns>
        Task<Result> IsAllowedToDelete(IUserContext userContext, TThing thing);
    }
}
