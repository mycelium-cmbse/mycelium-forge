// ------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extensions
{
    using Microsoft.Extensions.Options;

    using Mycelium.Forge.Orm;

    using Npgsql;

    /// <summary>
    /// Provides extension methods for <see cref="IConfiguration" /> and <see cref="IServiceCollection" /> to resolve and
    /// register database configuration.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// The configuration section name for database connection options.
        /// </summary>
        private const string DatabaseConnectionSectionName = "DatabaseConnection";

        /// <summary>
        /// The connection string name for the default database connection.
        /// </summary>
        private const string DefaultConnectionStringName = "Default";

        /// <summary>
        /// Retrieves the <see cref="DatabaseConfig" /> from configuration, checking both "DatabaseConfig" and "DatabaseConnection"
        /// sections,
        /// or falling back to parsing the "Default" connection string.
        /// </summary>
        /// <param name="configuration">The configuration to retrieve settings from.</param>
        /// <returns>The resolved <see cref="DatabaseConfig" /> instance.</returns>
        public static DatabaseConfig GetDatabaseConfig(this IConfiguration configuration)
        {
            var databaseConfig = configuration.GetSection(nameof(DatabaseConfig)).Get<DatabaseConfig>()
                                 ?? configuration.GetSection(DatabaseConnectionSectionName).Get<DatabaseConfig>();

            if (databaseConfig != null && !string.IsNullOrWhiteSpace(databaseConfig.Host))
            {
                return databaseConfig;
            }

            var defaultConnectionString = configuration.GetConnectionString(DefaultConnectionStringName);

            if (string.IsNullOrWhiteSpace(defaultConnectionString))
            {
                return databaseConfig ?? new DatabaseConfig();
            }

            var builder = new NpgsqlConnectionStringBuilder(defaultConnectionString);

            return new DatabaseConfig
            {
                Host = builder.Host ?? string.Empty,
                Port = builder.Port,
                Database = builder.Database ?? string.Empty,
                Username = builder.Username ?? string.Empty,
                Password = builder.Password ?? string.Empty
            };
        }

        /// <summary>
        /// Retrieves the database connection string from configuration by building from <see cref="DatabaseConfig" />.
        /// </summary>
        /// <param name="configuration">The configuration to retrieve settings from.</param>
        /// <returns>The resolved database connection string.</returns>
        public static string GetDatabaseConnectionString(this IConfiguration configuration)
        {
            var databaseConfig = configuration.GetDatabaseConfig();

            return !string.IsNullOrWhiteSpace(databaseConfig.Host)
                ? databaseConfig.BuildConnectionString()
                : string.Empty;
        }

        /// <summary>
        /// Registers <see cref="DatabaseConfig" /> and related dependencies in the service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to configure.</param>
        /// <param name="configuration">The configuration instance.</param>
        /// <returns>The configured <see cref="IServiceCollection" /> instance.</returns>
        public static IServiceCollection RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseConfig = configuration.GetDatabaseConfig();

            services.AddSingleton(databaseConfig);
            services.AddSingleton(Options.Create(databaseConfig));

            return services;
        }
    }
}
