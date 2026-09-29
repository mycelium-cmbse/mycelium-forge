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
        /// Asynchronously executes an action within a database transaction, committing on success or rolling back on failure.
        /// </summary>
        /// <param name="action">The asynchronous action to execute within the transaction, returning <see langword="true" /> to commit or <see langword="false" /> to roll back.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task resolving to <see langword="true" /> if the operation was executed and committed successfully; otherwise <see langword="false" />.</returns>
        Task<bool> ExecuteInTransactionAsync(Func<NpgsqlTransaction, Task<bool>> action, CancellationToken cancellationToken = default);
    }
}
