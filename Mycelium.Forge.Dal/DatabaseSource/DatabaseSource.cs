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
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Mycelium.Forge.Orm;

    using Npgsql;

    /// <summary>
    /// Implements <see cref="IDatabaseSource"/> to open new PostgreSQL database connections using <see cref="DatabaseConfig"/>.
    /// </summary>
    public class DatabaseSource : IDatabaseSource
    {
        /// <summary>
        /// The database configuration options.
        /// </summary>
        private readonly DatabaseConfig databaseConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseSource"/> class.
        /// </summary>
        /// <param name="databaseConfig">The <see cref="DatabaseConfig"/> used to build connection strings.</param>
        public DatabaseSource(DatabaseConfig databaseConfig)
        {
            ArgumentNullException.ThrowIfNull(databaseConfig);

            this.databaseConfig = databaseConfig;
        }

        /// <summary>
        /// Asynchronously opens a new database connection.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A new open <see cref="NpgsqlConnection"/>.</returns>
        public async Task<NpgsqlConnection> OpenNewConnectionAsync(CancellationToken cancellationToken = default)
        {
            var connection = new NpgsqlConnection(this.databaseConfig.BuildConnectionString());
            await connection.OpenAsync(cancellationToken);
            return connection;
        }
    }
}
