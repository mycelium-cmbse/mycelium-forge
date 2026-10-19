// ------------------------------------------------------------------------------------------------
// <copyright file="IDao.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Orm
{
    using System.Collections.Immutable;

    using ErrorOr;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Orm.Filters;

    using Npgsql;

    /// <summary>
    /// Defines generic data access operations for domain entities implementing <see cref="IThing" />.
    /// </summary>
    /// <typeparam name="T">The domain entity type implementing <see cref="IThing" />.</typeparam>
    public interface IDao<T> where T : IThing
    {
        /// <summary>
        /// Asynchronously creates (persists) a new instance of <typeparamref name="T" /> in the database.
        /// </summary>
        /// <param name="transaction">The current <see cref="NpgsqlTransaction" /> to the database.</param>
        /// <param name="item">The entity that is to be persisted.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the Create operation.</param>
        /// <returns>
        /// Returns a successful <see cref="Result.Created" /> when persisted successfully, or a failed
        /// <see cref="Error" /> if not successful.
        /// </returns>
        Task<ErrorOr<Created>> CreateAsync(NpgsqlTransaction transaction, T item, CancellationToken token);

        /// <summary>
        /// Asynchronously reads <typeparamref name="T" /> instances from the database.
        /// </summary>
        /// <param name="transaction">The current <see cref="NpgsqlTransaction" /> to the database.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the Read operation.</param>
        /// <param name="include">An array of unique identifiers of instances that are to be read.</param>
        /// <param name="exclude">An array of unique identifiers of instances that are to be excluded from the read results.</param>
        /// <param name="filter">An optional <see cref="ISqlFilter" /> to apply to the read query.</param>
        /// <returns>An <see cref="ImmutableList{T}" /> of permitted instances.</returns>
        Task<ErrorOr<ImmutableList<T>>> ReadAsync(NpgsqlTransaction transaction, CancellationToken token, Guid[] include = null, Guid[] exclude = null, ISqlFilter filter = null);

        /// <summary>
        /// Reads the unique identifiers of all <typeparamref name="T" /> objects from the database.
        /// </summary>
        /// <param name="transaction">The current <see cref="NpgsqlTransaction" /> to the database.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the Read operation.</param>
        /// <param name="filter">An optional <see cref="ISqlFilter" /> to apply to the read query.</param>
        /// <returns>The list of identifiers of the objects that exist in the database.</returns>
        Task<List<Guid>> ReadIdentifiersAsync(NpgsqlTransaction transaction, CancellationToken token, ISqlFilter filter = null);

        /// <summary>
        /// Asynchronously updates (persists) an instance of <typeparamref name="T" /> in the database.
        /// </summary>
        /// <param name="transaction">The current <see cref="NpgsqlTransaction" /> to the database.</param>
        /// <param name="item">The entity that is to be updated.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the Update operation.</param>
        /// <returns>
        /// Returns a successful <see cref="Result.Updated" /> when updated successfully, or a failed <see cref="Error" />
        /// if not successful.
        /// </returns>
        Task<ErrorOr<Updated>> UpdateAsync(NpgsqlTransaction transaction, T item, CancellationToken token);

        /// <summary>
        /// Asynchronously deletes the provided <typeparamref name="T" /> instances from the database.
        /// </summary>
        /// <param name="transaction">The current <see cref="NpgsqlTransaction" /> to the database.</param>
        /// <param name="dtos">The collection of entities that are to be deleted from the database.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>
        /// Returns a successful <see cref="Result.Deleted" /> when deleted successfully, or a failed <see cref="Error" />
        /// if not successful.
        /// </returns>
        Task<ErrorOr<Deleted>> DeleteAsync(NpgsqlTransaction transaction, IEnumerable<T> dtos, CancellationToken token);
    }
}
