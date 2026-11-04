// ------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.AppHost
{
    using System.Diagnostics.CodeAnalysis;

    using Aspire.Hosting;
    using Aspire.Hosting.ApplicationModel;

    using Mycelium.Forge.AppHost.Extensions;

    using Projects;

    /// <summary>
    /// Defines and runs the local Forge environment (PostgreSQL, object storage, identity provider,
    /// one-shot migrator and seeder, and the Forge web application) and supports publishing the
    /// production Docker Compose manifest.
    /// </summary>
    /// <remarks>
    /// In run mode, every credential defaults to a fixed, checked-in, dev-only value. In publish mode,
    /// secrets are emitted as unpopulated parameters and the seeder is excluded.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        /// <summary>
        /// The endpoint name for the Garage administrator API and health check.
        /// </summary>
        private const string GarageAdminEndpointName = "admin";

        /// <summary>
        /// Builds the Aspire resource model and runs or publishes the application.
        /// </summary>
        /// <param name="args">The command-line arguments forwarded to the Aspire application builder.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="args" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="DistributedApplicationException">
        /// The Aspire operation or resource model configuration is invalid.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The host cannot be constructed from its configuration and registered services.
        /// </exception>
        /// <exception cref="System.AggregateException">
        /// The asynchronous application run fails and its exception is propagated by the synchronous host runner.
        /// </exception>
        public static void Main(string[] args)
        {
            var builder = DistributedApplication.CreateBuilder(args);

            builder.AddDockerComposeEnvironment("compose")
                .WithProductionComposeConfiguration();

            var databaseUsername = builder.AddParameter("database-username", AppHostConstants.ForgeDatabase);
            var databasePassword = builder.AddSecretParameter("postgres-password", "forge-dev-password");
            var runtimePassword = builder.AddSecretParameter("forge-runtime-password", "forge-runtime-dev-password");
            var authenticationDatabaseUsername = builder.AddParameter("authentication-database-username", AppHostConstants.KeycloakDatabase);
            var authenticationDatabasePassword = builder.AddSecretParameter("keycloak-db-password", "keycloak-dev-password");
            var keycloakAdminPassword = builder.AddSecretParameter("keycloak-admin-password", "admin");
            var clientSecret = builder.AddSecretParameter("forge-client-secret", "forge-dev-client-secret");
            var spiApiKey = builder.AddSecretParameter("keycloak-spi-api-key", "forge-dev-keycloak-spi-key");
            var storageAccessKey = builder.AddSecretParameter("artifact-store-access-key", "GKdevlocal000000000");
            var storageSecretKey = builder.AddSecretParameter("artifact-store-secret-key", "dev0000000000000000000000000000000000000000000000000000000000");
            var garageConfigPath = builder.ExecutionContext.IsPublishMode ? "../docker/garage.prod.toml" : "../docker/garage.toml";

            var postgres = builder.AddPostgres(AppHostConstants.Postgres, databaseUsername, databasePassword, 5432)
                .WithImageTag("18")
                .WithEnvironment("POSTGRES_DB", AppHostConstants.ForgeDatabase)
                .WithEnvironment("FORGE_RUNTIME_PASSWORD", runtimePassword)
                .WithDataVolume("forge-postgres-data")
                .WithBindMount("../docker/postgres/00-create-roles.sh", "/docker-entrypoint-initdb.d/00-create-roles.sh", true)
                .WithForgeContainerDefaults(AppHostConstants.Postgres)
                .WithPostgresPublishConfiguration(AppHostConstants.ForgeDatabase, AppHostConstants.ForgeDatabase);

            var authenticationPostgres = builder.AddPostgres(AppHostConstants.PostgresAuthentication, authenticationDatabaseUsername, authenticationDatabasePassword, 5433)
                .WithImageTag("18")
                .WithEnvironment("POSTGRES_DB", AppHostConstants.KeycloakDatabase)
                .WithDataVolume("forge-keycloak-postgres-data")
                .WithForgeContainerDefaults(AppHostConstants.PostgresAuthentication)
                .WithPostgresPublishConfiguration(AppHostConstants.KeycloakDatabase, AppHostConstants.KeycloakDatabase);

            var garage = builder.AddContainer(AppHostConstants.Garage, "dxflrs/garage", "v2.3.0")
                .WithArgs("/garage", "server", "--single-node", "--default-bucket")
                .WithEnvironment("GARAGE_DEFAULT_ACCESS_KEY", storageAccessKey)
                .WithEnvironment("GARAGE_DEFAULT_SECRET_KEY", storageSecretKey)
                .WithEnvironment("GARAGE_DEFAULT_BUCKET", "forge-artefacts")
                .WithBindMount(garageConfigPath, "/etc/garage.toml", true)
                .WithVolume("forge-garage-meta", "/var/lib/garage/meta")
                .WithVolume("forge-garage-data", "/var/lib/garage/data")
                .WithHttpEndpoint(3900, 3900, "s3")
                .WithHttpEndpoint(3903, 3903, GarageAdminEndpointName)
                .WithHttpHealthCheck("/health", endpointName: GarageAdminEndpointName)
                .WithForgeContainerDefaults(AppHostConstants.Garage)
                .WithGaragePublishConfiguration(builder);

            var keycloak = builder.AddDockerfile(AppHostConstants.Keycloak, "../docker/keycloak")
                .WithEnvironment("KC_BOOTSTRAP_ADMIN_USERNAME", AppHostConstants.DefaultAdminUsername)
                .WithEnvironment("KC_BOOTSTRAP_ADMIN_PASSWORD", keycloakAdminPassword)
                .WithEnvironment("KC_DB", "postgres")
                .WithEnvironment("KC_DB_URL_HOST", authenticationPostgres.GetEndpoint("tcp").Property(EndpointProperty.Host))
                .WithEnvironment("KC_DB_URL_PORT", authenticationPostgres.GetEndpoint("tcp").Property(EndpointProperty.Port))
                .WithEnvironment("KC_DB_URL_DATABASE", AppHostConstants.KeycloakDatabase)
                .WithEnvironment("KC_DB_USERNAME", authenticationDatabaseUsername)
                .WithEnvironment("KC_DB_PASSWORD", authenticationDatabasePassword)
                .WithEnvironment("KC_HOSTNAME_BACKCHANNEL_DYNAMIC", "true")
                .WithEnvironment("KC_HEALTH_ENABLED", "true")
                .WithEnvironment("FORGE_CLIENT_SECRET", clientSecret)
                .WithEnvironment("FORGE_KEYCLOAK_VERIFY_EMAIL", "false")
                .WithEnvironment("FORGE_SPI_API_KEY", spiApiKey)
                .WithHttpEndpoint(8081, 8080, "http")
                .WithHttpEndpoint(null, 9000, "management")
                .WithHttpHealthCheck("/health/ready", endpointName: "management")
                .WithForgeContainerDefaults(AppHostConstants.Keycloak)
                .WaitFor(authenticationPostgres)
                .WithKeycloakEnvironmentAndPublishConfiguration(builder, out var keycloakPublicUrlParameter);

            var migrator = builder.AddProject<Mycelium_Forge>(AppHostConstants.Migrator, options => options.ExcludeLaunchProfile = true)
                .WithArgs("migrate")
                .WithDatabaseEnvironment(postgres, databaseUsername, databasePassword)
                .WaitFor(postgres)
                .WithMigratorPublishConfiguration(builder);

            var seeder = builder.AddProject<Mycelium_Forge>(AppHostConstants.Seeder, options => options.ExcludeLaunchProfile = true)
                .WithArgs("seed")
                .WithDatabaseEnvironment(postgres, databaseUsername, databasePassword)
                .WithEnvironment("Keycloak__BaseUrl", AppHostConstants.DevelopmentKeycloakPublicUrl)
                .WithEnvironment("Keycloak__Realm", "forge")
                .WithEnvironment("Keycloak__AdminUsername", AppHostConstants.DefaultAdminUsername)
                .WithEnvironment("Keycloak__AdminPassword", keycloakAdminPassword)
                .WaitForCompletion(migrator)
                .WaitFor(keycloak)
                .ExcludeFromManifest();

            builder.AddProject<Mycelium_Forge>(AppHostConstants.Forge, "http")
                .WithDatabaseEnvironment(postgres, databaseUsername, databasePassword)
                .WithEnvironment("ArtifactStore__ServiceUrl", garage.GetEndpoint("s3"))
                .WithEnvironment("ArtifactStore__Region", "garage")
                .WithEnvironment("ArtifactStore__BucketName", "forge-artefacts")
                .WithEnvironment("ArtifactStore__AccessKey", storageAccessKey)
                .WithEnvironment("ArtifactStore__SecretKey", storageSecretKey)
                .WithEnvironment("ArtifactStore__ForcePathStyle", "true")
                .WithEnvironment("Authentication__ClientId", "forge-web")
                .WithEnvironment("Authentication__ClientSecret", clientSecret)
                .WithEnvironment("Authentication__RequireHttpsMetadata", "false")
                .WithEnvironment("KeycloakSpi__ApiKey", spiApiKey)
                .WaitFor(garage)
                .WaitFor(keycloak)
                .WithForgeEnvironmentAndPublishConfiguration(builder, keycloakPublicUrlParameter, keycloakAdminPassword, migrator, seeder);

            builder.Build().Run();
        }
    }
}
