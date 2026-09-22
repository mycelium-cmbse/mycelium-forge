// ------------------------------------------------------------------------------------------------
// <copyright file="IReadService.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.Services
{
    using System.Collections.Immutable;

    using ErrorOr;

    using Mycelium.Forge.Common;

    using Npgsql;

    /// <summary>
    /// Base interface for domain entity services that support read operations.
    /// </summary>
    /// <typeparam name="T">The domain entity type implementing <see cref="IThing" />.</typeparam>
    public interface IReadService<T> where T : IThing
    {
        /// <summary>
        /// Asynchronously reads instances of <typeparamref name="T" /> within an existing transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="transaction">The current <see cref="NpgsqlTransaction" /> to the database.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <param name="iids">An optional array of unique identifiers to read.</param>
        /// <returns>A <see cref="ErrorOr{TValue}" /> containing an <see cref="ImmutableList{T}" /> of permitted instances.</returns>
        Task<ErrorOr<ImmutableList<T>>> ReadAsync(IUserContext userContext, NpgsqlTransaction transaction, CancellationToken token, Guid[] iids = null);

        /// <summary>
        /// Asynchronously reads instances of <typeparamref name="T" /> managing its own connection and transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <param name="iids">An optional array of unique identifiers to read.</param>
        /// <returns>A <see cref="ErrorOr{TValue}" /> containing an <see cref="ImmutableList{T}" /> of permitted instances.</returns>
        Task<ErrorOr<ImmutableList<T>>> ReadAsync(IUserContext userContext, CancellationToken token, Guid[] iids = null);
    }
}
