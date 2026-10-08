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
    using ErrorOr;

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
        /// Asynchronously executes an operation within an isolated connection and database transaction,
        /// committing on success or rolling back on error or unhandled exception.
        /// </summary>
        /// <typeparam name="TValue">The return value type wrapped by <see cref="ErrorOr{TValue}" />.</typeparam>
        /// <param name="action">The asynchronous operation to execute within the transaction.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
        /// <returns>A <see cref="Task" /> representing the result of the operation or an error.</returns>
        public async Task<ErrorOr<TValue>> ExecuteInTransactionAsync<TValue>(
            Func<NpgsqlTransaction, Task<ErrorOr<TValue>>> action,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(action);

            try
            {
                await using var connection = await this.OpenNewConnectionAsync(cancellationToken);
                await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

                var result = await action.Invoke(transaction);

                if (result.IsError)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return result.Errors;
                }

                await transaction.CommitAsync(cancellationToken);
                return result;
            }
            catch (Exception exception)
            {
                return Error.Failure(description: exception.Message);
            }
        }

        /// <summary>
        /// Asynchronously defers all deferrable foreign key constraints until transaction commit.
        /// </summary>
        /// <param name="transaction">The active <see cref="NpgsqlTransaction" />.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task DeferConstraintsAsync(NpgsqlTransaction transaction, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(transaction);

            await using var deferCommand = new NpgsqlCommand("SET CONSTRAINTS ALL DEFERRED;", transaction.Connection, transaction);
            await deferCommand.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
