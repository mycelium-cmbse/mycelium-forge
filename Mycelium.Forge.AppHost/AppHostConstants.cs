// ------------------------------------------------------------------------------------------------
// <copyright file="AppHostConstants.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.AppHost
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Provides shared resource, database, and service identifiers used across the Aspire AppHost.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class AppHostConstants
    {
        /// <summary>
        /// The service identifier for the Forge web application and database.
        /// </summary>
        public const string Forge = "forge";

        /// <summary>
        /// The service identifier for the Keycloak identity provider and database.
        /// </summary>
        public const string Keycloak = "keycloak";

        /// <summary>
        /// The service identifier for the primary PostgreSQL database server.
        /// </summary>
        public const string Postgres = "postgres";

        /// <summary>
        /// The service identifier for the Keycloak PostgreSQL database server.
        /// </summary>
        public const string PostgresAuthentication = "postgres-authentication";

        /// <summary>
        /// The service identifier for the Garage object storage service.
        /// </summary>
        public const string Garage = "garage";

        /// <summary>
        /// The service identifier for the database migrator one-shot project.
        /// </summary>
        public const string Migrator = "migrator";

        /// <summary>
        /// The service identifier for the database seeder one-shot project.
        /// </summary>
        public const string Seeder = "seeder";

        /// <summary>
        /// The default Keycloak administrator username used in local development.
        /// </summary>
        public const string DefaultAdminUsername = "admin";

        /// <summary>
        /// The browser-facing URL of Keycloak in local development.
        /// </summary>
        public const string DevelopmentKeycloakPublicUrl = "http://localhost:8081";
    }
}
