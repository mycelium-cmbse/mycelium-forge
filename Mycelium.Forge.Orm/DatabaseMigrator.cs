// ------------------------------------------------------------------------------------------------
// <copyright file="DatabaseMigrator.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Orm
{
    using Npgsql;

    /// <summary>
    /// Implements database migration and schema reset operations using DbUp.
    /// </summary>
    public class DatabaseMigrator : IDatabaseMigrator
    {
        /// <summary>
        /// Applies database migrations.
        /// </summary>
        /// <param name="connectionString">The PostgreSQL connection string.</param>
        /// <returns>
        /// <see langword="true" /> if migrations succeeded; otherwise <see langword="false" />.
        /// </returns>
        public bool Migrate(string connectionString)
        {
            return Migrator.Run(connectionString);
        }

        /// <summary>
        /// Drops the existing schema and reapplies database migrations.
        /// </summary>
        /// <param name="connectionString">The PostgreSQL connection string.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task resolving to <see langword="true" /> if schema was reset and migrations succeeded; otherwise
        /// <see langword="false" />.
        /// </returns>
        public async Task<bool> ResetDatabaseAsync(string connectionString, CancellationToken cancellationToken = default)
        {
            await using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await using var dropCommand = new NpgsqlCommand("DROP SCHEMA IF EXISTS \"Forge\" CASCADE; DROP TABLE IF EXISTS \"schemaversions\" CASCADE;", connection);
                await dropCommand.ExecuteNonQueryAsync(cancellationToken);
            }

            return this.Migrate(connectionString);
        }
    }
}
