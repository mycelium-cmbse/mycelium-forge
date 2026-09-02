// ------------------------------------------------------------------------------------------------
// <copyright file="DatabaseConfig.cs" company="Starion Group S.A.">
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
    /// Configuration options for connecting to the PostgreSQL database backing store.
    /// </summary>
    public class DatabaseConfig
    {
        /// <summary>
        /// Gets or sets the database host name or IP address.
        /// </summary>
        public string Host { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        public string Database { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database user name.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database user password.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Builds a valid PostgreSQL connection string based on the configured properties.
        /// </summary>
        /// <returns>A formatted PostgreSQL connection string.</returns>
        public string BuildConnectionString()
        {
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = this.Host,
                Port = this.Port,
                Database = this.Database,
                Username = this.Username,
                Password = this.Password
            };

            return builder.ConnectionString;
        }
    }
}
