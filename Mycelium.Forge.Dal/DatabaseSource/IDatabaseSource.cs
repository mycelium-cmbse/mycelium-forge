// ------------------------------------------------------------------------------------------------
// <copyright file="IDatabaseSource.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.DatabaseSource
{
    using ErrorOr;

    using Npgsql;

    /// <summary>
    /// Provides an abstraction for opening database connections.
    /// </summary>
    public interface IDatabaseSource
    {
        /// <summary>
        /// Asynchronously opens a new database connection.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A new open <see cref="NpgsqlConnection" />.</returns>
        Task<NpgsqlConnection> OpenNewConnectionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously executes an operation within an isolated connection and database transaction,
        /// committing on success or rolling back on error or unhandled exception.
        /// </summary>
        /// <typeparam name="TValue">The return value type wrapped by <see cref="ErrorOr{TValue}" />.</typeparam>
        /// <param name="action">The asynchronous operation to execute within the transaction.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
        /// <returns>A <see cref="Task" /> representing the result of the operation or an error.</returns>
        Task<ErrorOr<TValue>> ExecuteInTransactionAsync<TValue>(Func<NpgsqlTransaction, Task<ErrorOr<TValue>>> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously defers all deferrable foreign key constraints until transaction commit.
        /// </summary>
        /// <param name="transaction">The active <see cref="NpgsqlTransaction" />.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task DeferConstraintsAsync(NpgsqlTransaction transaction, CancellationToken cancellationToken = default);
    }
}
