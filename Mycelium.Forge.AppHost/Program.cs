// ------------------------------------------------------------------------------------------------
//  <copyright file="Program.cs" company="Starion Group S.A.">
//
//    Copyright 2026 Starion Group S.A.
//    SPDX-License-Identifier: Apache-2.0
//
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.AppHost
{
    using System.Diagnostics.CodeAnalysis;

    using Aspire.Hosting;
    using Aspire.Hosting.ApplicationModel;

    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Defines and runs the local Forge environment: PostgreSQL, object storage, the identity provider,
    /// the one-shot migrator and seeder, and the Forge web application itself.
    /// </summary>
    /// <remarks>
    /// Every credential below is a fixed, checked-in, dev-only value (exposed as an Aspire parameter so it
    /// can still be overridden through configuration) - never used outside local development.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        /// <summary>
        /// The browser-facing URL of Keycloak.
        /// </summary>
        private const string KeycloakPublicUrl = "http://localhost:8081";

        /// <summary>
        /// The browser-facing URL of Forge, matching the <c>http</c> launch profile of Mycelium.Forge.
        /// </summary>
        private const string ForgePublicUrl = "http://localhost:5000";

        /// <summary>
        /// The URL Keycloak uses, from inside its container, to reach Forge running as a host process.
        /// </summary>
        private const string ForgeUrlFromContainers = "http://host.docker.internal:5000";

        /// <summary>
        /// Builds the local Aspire resource model and runs the application until host shutdown.
        /// </summary>
        /// <param name="args">The command-line arguments forwarded to the Aspire application builder.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="args"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="DistributedApplicationException">
        /// The Aspire operation or resource model configuration is invalid.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The host is not running in the Development environment, or cannot be constructed from its configuration and registered services.
        /// </exception>
        /// <exception cref="System.AggregateException">
        /// The asynchronous application run fails and its exception is propagated by the synchronous host runner.
        /// </exception>
        public static void Main(string[] args)
        {
            var builder = DistributedApplication.CreateBuilder(args);

            if (!builder.Environment.IsDevelopment())
            {
                throw new System.InvalidOperationException("The Mycelium Forge AppHost is a development tool: it uses fixed, checked-in credentials and runs the seeder. Deploy with docker-compose.prod.yml instead.");
            }

            var databaseUsername = builder.AddParameter("database-username", "forge");
            var databasePassword = builder.AddParameter("database-password", "forge-dev-password", secret: true);
            var runtimePassword = builder.AddParameter("database-runtime-password", "forge-runtime-dev-password", secret: true);
            var authenticationDatabaseUsername = builder.AddParameter("authentication-database-username", "keycloak");
            var authenticationDatabasePassword = builder.AddParameter("authentication-database-password", "keycloak-dev-password", secret: true);
            var keycloakAdminPassword = builder.AddParameter("keycloak-admin-password", "admin", secret: true);
            var clientSecret = builder.AddParameter("forge-client-secret", "forge-dev-client-secret", secret: true);
            var spiApiKey = builder.AddParameter("keycloak-spi-api-key", "forge-dev-keycloak-spi-key", secret: true);
            var storageAccessKey = builder.AddParameter("artifact-store-access-key", "GKdevlocal000000000");
            var storageSecretKey = builder.AddParameter("artifact-store-secret-key", "dev0000000000000000000000000000000000000000000000000000000000", secret: true);

            var postgres = builder.AddPostgres("postgres", databaseUsername, databasePassword, 5432)
                .WithImageTag("18")
                .WithEnvironment("POSTGRES_DB", "forge")
                .WithEnvironment("FORGE_RUNTIME_PASSWORD", runtimePassword)
                .WithDataVolume("forge-postgres-data")
                .WithBindMount("../docker/postgres/00-create-roles.sh", "/docker-entrypoint-initdb.d/00-create-roles.sh", true)
                .WithLifetime(ContainerLifetime.Persistent);

            var authenticationPostgres = builder.AddPostgres("postgres-authentication", authenticationDatabaseUsername, authenticationDatabasePassword, 5433)
                .WithImageTag("18")
                .WithEnvironment("POSTGRES_DB", "keycloak")
                .WithDataVolume("forge-keycloak-postgres-data")
                .WithLifetime(ContainerLifetime.Persistent);

            var garage = builder.AddContainer("garage", "dxflrs/garage", "v2.3.0")
                .WithArgs("/garage", "server", "--single-node", "--default-bucket")
                .WithEnvironment("GARAGE_DEFAULT_ACCESS_KEY", storageAccessKey)
                .WithEnvironment("GARAGE_DEFAULT_SECRET_KEY", storageSecretKey)
                .WithEnvironment("GARAGE_DEFAULT_BUCKET", "forge-artefacts")
                .WithBindMount("../docker/garage.toml", "/etc/garage.toml", true)
                .WithVolume("forge-garage-meta", "/var/lib/garage/meta")
                .WithVolume("forge-garage-data", "/var/lib/garage/data")
                .WithHttpEndpoint(3900, 3900, "s3")
                .WithHttpEndpoint(3903, 3903, "admin")
                .WithHttpHealthCheck("/health", endpointName: "admin")
                .WithLifetime(ContainerLifetime.Persistent);

            var keycloak = builder.AddDockerfile("keycloak", "../docker/keycloak")
                .WithArgs("start-dev", "--import-realm")
                .WithEnvironment("KC_BOOTSTRAP_ADMIN_USERNAME", "admin")
                .WithEnvironment("KC_BOOTSTRAP_ADMIN_PASSWORD", keycloakAdminPassword)
                .WithEnvironment("KC_DB", "postgres")
                .WithEnvironment("KC_DB_URL_HOST", authenticationPostgres.GetEndpoint("tcp").Property(EndpointProperty.Host))
                .WithEnvironment("KC_DB_URL_PORT", authenticationPostgres.GetEndpoint("tcp").Property(EndpointProperty.Port))
                .WithEnvironment("KC_DB_URL_DATABASE", "keycloak")
                .WithEnvironment("KC_DB_USERNAME", authenticationDatabaseUsername)
                .WithEnvironment("KC_DB_PASSWORD", authenticationDatabasePassword)
                .WithEnvironment("KC_HOSTNAME", KeycloakPublicUrl)
                .WithEnvironment("KC_HOSTNAME_BACKCHANNEL_DYNAMIC", "true")
                .WithEnvironment("KC_HEALTH_ENABLED", "true")
                .WithEnvironment("FORGE_BASE_URL", ForgePublicUrl)
                .WithEnvironment("FORGE_CLIENT_SECRET", clientSecret)
                .WithEnvironment("FORGE_KEYCLOAK_SSL_REQUIRED", "none")
                .WithEnvironment("FORGE_KEYCLOAK_VERIFY_EMAIL", "false")
                .WithEnvironment("FORGE_SPI_BASE_URL", ForgeUrlFromContainers)
                .WithEnvironment("FORGE_SPI_API_KEY", spiApiKey)
                .WithBindMount("../docker/keycloak/themes/forge", "/opt/keycloak/themes/forge", true)
                .WithContainerRuntimeArgs("--add-host", "host.docker.internal:host-gateway")
                .WithHttpEndpoint(8081, 8080, "http")
                .WithHttpEndpoint(null, 9000, "management")
                .WithHttpHealthCheck("/health/ready", endpointName: "management")
                .WaitFor(authenticationPostgres)
                .WithLifetime(ContainerLifetime.Persistent);

            var migrator = builder.AddProject<Projects.Mycelium_Forge>("migrator", options => options.ExcludeLaunchProfile = true)
                .WithArgs("migrate")
                .WithDatabaseEnvironment(postgres, databaseUsername, databasePassword)
                .WaitFor(postgres);

            var seeder = builder.AddProject<Projects.Mycelium_Forge>("seeder", options => options.ExcludeLaunchProfile = true)
                .WithArgs("seed")
                .WithDatabaseEnvironment(postgres, databaseUsername, databasePassword)
                .WithEnvironment("Keycloak__BaseUrl", KeycloakPublicUrl)
                .WithEnvironment("Keycloak__Realm", "forge")
                .WithEnvironment("Keycloak__AdminUsername", "admin")
                .WithEnvironment("Keycloak__AdminPassword", keycloakAdminPassword)
                .WaitForCompletion(migrator)
                .WaitFor(keycloak);

            builder.AddProject<Projects.Mycelium_Forge>("forge", "http")
                .WithDatabaseEnvironment(postgres, databaseUsername, databasePassword)
                .WithEnvironment("ArtifactStore__ServiceUrl", garage.GetEndpoint("s3"))
                .WithEnvironment("ArtifactStore__Region", "garage")
                .WithEnvironment("ArtifactStore__BucketName", "forge-artefacts")
                .WithEnvironment("ArtifactStore__AccessKey", storageAccessKey)
                .WithEnvironment("ArtifactStore__SecretKey", storageSecretKey)
                .WithEnvironment("ArtifactStore__ForcePathStyle", "true")
                .WithEnvironment("Authentication__Authority", $"{KeycloakPublicUrl}/realms/forge")
                .WithEnvironment("Authentication__ClientId", "forge-web")
                .WithEnvironment("Authentication__ClientSecret", clientSecret)
                .WithEnvironment("Authentication__RequireHttpsMetadata", "false")
                .WithEnvironment("Keycloak__BaseUrl", KeycloakPublicUrl)
                .WithEnvironment("Keycloak__Realm", "forge")
                .WithEnvironment("Keycloak__AdminUsername", "admin")
                .WithEnvironment("Keycloak__AdminPassword", keycloakAdminPassword)
                .WithEnvironment("KeycloakSpi__ApiKey", spiApiKey)
                .WaitForCompletion(seeder)
                .WaitFor(garage)
                .WaitFor(keycloak);

            builder.Build().Run();
        }

        /// <summary>
        /// Adds the <c>DatabaseConnection</c> settings Forge uses to reach the given PostgreSQL server.
        /// </summary>
        /// <typeparam name="TResource">The type of the resource receiving the environment.</typeparam>
        /// <param name="resource">The resource builder to configure.</param>
        /// <param name="postgres">The PostgreSQL server the resource connects to.</param>
        /// <param name="username">The database username parameter.</param>
        /// <param name="password">The database password parameter.</param>
        /// <returns>The configured resource builder.</returns>
        private static IResourceBuilder<TResource> WithDatabaseEnvironment<TResource>(this IResourceBuilder<TResource> resource, IResourceBuilder<PostgresServerResource> postgres, IResourceBuilder<ParameterResource> username, IResourceBuilder<ParameterResource> password) where TResource : IResourceWithEnvironment
        {
            return resource
                .WithEnvironment("DatabaseConnection__Host", postgres.GetEndpoint("tcp").Property(EndpointProperty.Host))
                .WithEnvironment("DatabaseConnection__Port", postgres.GetEndpoint("tcp").Property(EndpointProperty.Port))
                .WithEnvironment("DatabaseConnection__Database", "forge")
                .WithEnvironment("DatabaseConnection__Username", username)
                .WithEnvironment("DatabaseConnection__Password", password);
        }
    }
}
