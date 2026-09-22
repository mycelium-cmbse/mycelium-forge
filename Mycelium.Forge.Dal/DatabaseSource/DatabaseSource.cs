// ------------------------------------------------------------------------------------------------
// <copyright file="DatabaseSource.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.DatabaseSource
{
    using Mycelium.Forge.Orm;

    using Npgsql;

    /// <summary>
    /// Implements <see cref="IDatabaseSource" /> to open new PostgreSQL database connections using
    /// <see cref="DatabaseConfig" />.
    /// </summary>
    public class DatabaseSource : IDatabaseSource
    {
        /// <summary>
        /// The database configuration options.
        /// </summary>
        private readonly DatabaseConfig databaseConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseSource" /> class.
        /// </summary>
        /// <param name="databaseConfig">The <see cref="DatabaseConfig" /> used to build connection strings.</param>
        public DatabaseSource(DatabaseConfig databaseConfig)
        {
            ArgumentNullException.ThrowIfNull(databaseConfig);

            this.databaseConfig = databaseConfig;
        }

        /// <summary>
        /// Asynchronously opens a new database connection.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A new open <see cref="NpgsqlConnection" />.</returns>
        public async Task<NpgsqlConnection> OpenNewConnectionAsync(CancellationToken cancellationToken = default)
        {
            var connection = new NpgsqlConnection(this.databaseConfig.BuildConnectionString());
            await connection.OpenAsync(cancellationToken);
            return connection;
        }

        /// <summary>
        /// Asynchronously executes an action within a database transaction, committing on success or rolling back on failure.
        /// </summary>
        /// <param name="action">
        /// The asynchronous action to execute within the transaction, returning <see langword="true" /> to
        /// commit or <see langword="false" /> to roll back.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task resolving to <see langword="true" /> if the operation was executed and committed successfully;
        /// otherwise <see langword="false" />.
        /// </returns>
        public async Task<bool> ExecuteInTransactionAsync(Func<NpgsqlTransaction, Task<bool>> action, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(action);

            await using var connection = await this.OpenNewConnectionAsync(cancellationToken);
            await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

            var success = await action.Invoke(transaction);

            if (success)
            {
                await transaction.CommitAsync(cancellationToken);
                return true;
            }

            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }
}
