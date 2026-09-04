// ------------------------------------------------------------------------------------------------
// <copyright file="IService.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Threading;
    using System.Threading.Tasks;

    using FluentResults;

    using Mycelium.Forge.Common;

    using Npgsql;

    /// <summary>
    /// Base interface for domain entity services bringing validation, permission evaluation, and DAO operations together.
    /// </summary>
    /// <typeparam name="T">The domain entity type implementing <see cref="IThing"/>.</typeparam>
    public interface IService<T> where T : IThing
    {
        /// <summary>
        /// Asynchronously creates (persists) instances of <typeparamref name="T"/> within an existing transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="transaction">The current <see cref="NpgsqlTransaction" /> to the database.</param>
        /// <param name="token">The <see cref="CancellationToken"/> used to cancel the operation.</param>
        /// <param name="dtos">The collection of <typeparamref name="T"/> instances to create.</param>
        /// <returns>A <see cref="Result"/> indicating success or failure.</returns>
        Task<Result> CreateAsync(IUserContext userContext, NpgsqlTransaction transaction, CancellationToken token, IEnumerable<T> dtos);

        /// <summary>
        /// Asynchronously creates (persists) instances of <typeparamref name="T"/> managing its own connection and transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="token">The <see cref="CancellationToken"/> used to cancel the operation.</param>
        /// <param name="dtos">The collection of <typeparamref name="T"/> instances to create.</param>
        /// <returns>A <see cref="Result"/> indicating success or failure.</returns>
        Task<Result> CreateAsync(IUserContext userContext, CancellationToken token, IEnumerable<T> dtos);

        /// <summary>
        /// Asynchronously reads instances of <typeparamref name="T"/> within an existing transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="transaction">The current <see cref="NpgsqlTransaction" /> to the database.</param>
        /// <param name="token">The <see cref="CancellationToken"/> used to cancel the operation.</param>
        /// <param name="iids">An optional array of unique identifiers to read.</param>
        /// <returns>A <see cref="Result{TValue}"/> containing an <see cref="ImmutableList{T}"/> of permitted instances.</returns>
        Task<Result<ImmutableList<T>>> ReadAsync(IUserContext userContext, NpgsqlTransaction transaction, CancellationToken token, Guid[] iids = null);

        /// <summary>
        /// Asynchronously reads instances of <typeparamref name="T"/> managing its own connection and transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="token">The <see cref="CancellationToken"/> used to cancel the operation.</param>
        /// <param name="iids">An optional array of unique identifiers to read.</param>
        /// <returns>A <see cref="Result{TValue}"/> containing an <see cref="ImmutableList{T}"/> of permitted instances.</returns>
        Task<Result<ImmutableList<T>>> ReadAsync(IUserContext userContext, CancellationToken token, Guid[] iids = null);

        /// <summary>
        /// Asynchronously updates instances of <typeparamref name="T"/> within an existing transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="transaction">The current <see cref="NpgsqlTransaction" /> to the database.</param>
        /// <param name="token">The <see cref="CancellationToken"/> used to cancel the operation.</param>
        /// <param name="dtos">The collection of <typeparamref name="T"/> instances to update.</param>
        /// <returns>A <see cref="Result"/> indicating success or failure.</returns>
        Task<Result> UpdateAsync(IUserContext userContext, NpgsqlTransaction transaction, CancellationToken token, IEnumerable<T> dtos);

        /// <summary>
        /// Asynchronously updates instances of <typeparamref name="T"/> managing its own connection and transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="token">The <see cref="CancellationToken"/> used to cancel the operation.</param>
        /// <param name="dtos">The collection of <typeparamref name="T"/> instances to update.</param>
        /// <returns>A <see cref="Result"/> indicating success or failure.</returns>
        Task<Result> UpdateAsync(IUserContext userContext, CancellationToken token, IEnumerable<T> dtos);

        /// <summary>
        /// Asynchronously deletes instances of <typeparamref name="T"/> within an existing transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="transaction">The current <see cref="NpgsqlTransaction" /> to the database.</param>
        /// <param name="token">The <see cref="CancellationToken"/> used to cancel the operation.</param>
        /// <param name="iids">The collection of unique identifiers of instances to delete.</param>
        /// <returns>A <see cref="Result"/> indicating success or failure.</returns>
        Task<Result> DeleteAsync(IUserContext userContext, NpgsqlTransaction transaction, CancellationToken token, IEnumerable<Guid> iids);

        /// <summary>
        /// Asynchronously deletes instances of <typeparamref name="T"/> managing its own connection and transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="token">The <see cref="CancellationToken"/> used to cancel the operation.</param>
        /// <param name="iids">The collection of unique identifiers of instances to delete.</param>
        /// <returns>A <see cref="Result"/> indicating success or failure.</returns>
        Task<Result> DeleteAsync(IUserContext userContext, CancellationToken token, IEnumerable<Guid> iids);
    }
}
