// ------------------------------------------------------------------------------------------------
// <copyright file="ProjectResourceBuilderExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.AppHost.Extensions
{
    using System.Diagnostics.CodeAnalysis;

    using Aspire.Hosting;
    using Aspire.Hosting.ApplicationModel;
    using Aspire.Hosting.Docker.Resources.ComposeNodes;

    /// <summary>
    /// Provides extension methods for <see cref="IResourceBuilder{ProjectResource}" /> to configure Forge project resources.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ProjectResourceBuilderExtensions
    {
        /// <param name="project">The <see cref="IResourceBuilder{ProjectResource}" /> to configure.</param>
        extension(IResourceBuilder<ProjectResource> project)
        {
            /// <summary>
            /// Adds the <c>DatabaseConnection</c> settings Forge uses to reach the given PostgreSQL server.
            /// </summary>
            /// <param name="postgres">The PostgreSQL server the resource connects to.</param>
            /// <param name="username">The database username parameter.</param>
            /// <param name="password">The database password parameter.</param>
            /// <returns>The configured <see cref="IResourceBuilder{ProjectResource}" /> instance.</returns>
            public IResourceBuilder<ProjectResource> WithDatabaseEnvironment(IResourceBuilder<PostgresServerResource> postgres, IResourceBuilder<ParameterResource> username, IResourceBuilder<ParameterResource> password)
            {
                return project
                    .WithEnvironment("DatabaseConnection__Host", postgres.GetEndpoint("tcp").Property(EndpointProperty.Host))
                    .WithEnvironment("DatabaseConnection__Port", postgres.GetEndpoint("tcp").Property(EndpointProperty.Port))
                    .WithEnvironment("DatabaseConnection__Database", "forge")
                    .WithEnvironment("DatabaseConnection__Username", username)
                    .WithEnvironment("DatabaseConnection__Password", password);
            }

            /// <summary>
            /// Configures the migrator service for Docker Compose publishing.
            /// </summary>
            /// <param name="builder">The distributed application builder.</param>
            /// <returns>The configured <see cref="IResourceBuilder{ProjectResource}" /> instance.</returns>
            public IResourceBuilder<ProjectResource> WithMigratorPublishConfiguration(IDistributedApplicationBuilder builder)
            {
                if (builder.ExecutionContext.IsPublishMode)
                {
                    project.WithEnvironment("ASPNETCORE_ENVIRONMENT", "Production");
                }

                return project.PublishAsDockerComposeService((_, service) => { service.DependsOn["postgres"].Condition = "service_healthy"; });
            }

            /// <summary>
            /// Configures the Forge application service for run mode or Docker Compose publishing mode.
            /// </summary>
            /// <param name="builder">The distributed application builder.</param>
            /// <param name="keycloakPublicUrl">The local development public URL of Keycloak.</param>
            /// <param name="keycloakPublicUrlParameter">The publish-mode parameter representing the public Keycloak URL.</param>
            /// <param name="keycloakAdminPassword">The Keycloak administrator password parameter.</param>
            /// <param name="migrator">The migrator project resource builder.</param>
            /// <param name="seeder">The seeder project resource builder.</param>
            /// <returns>The configured <see cref="IResourceBuilder{ProjectResource}" /> instance.</returns>
            public IResourceBuilder<ProjectResource> WithForgeEnvironmentAndPublishConfiguration(IDistributedApplicationBuilder builder, string keycloakPublicUrl, IResourceBuilder<ParameterResource> keycloakPublicUrlParameter, IResourceBuilder<ParameterResource> keycloakAdminPassword, IResourceBuilder<ProjectResource> migrator, IResourceBuilder<ProjectResource> seeder)
            {
                if (builder.ExecutionContext.IsPublishMode)
                {
                    project.WithEnvironment("ASPNETCORE_ENVIRONMENT", "Production")
                        .WithEnvironment("Authentication__Authority", $"{keycloakPublicUrlParameter}/realms/forge")
                        .WithEnvironment("Authentication__MetadataAddress", "http://keycloak:8080/realms/forge/.well-known/openid-configuration")
                        .WaitForCompletion(migrator);
                }
                else
                {
                    project.WithEnvironment("Authentication__Authority", $"{keycloakPublicUrl}/realms/forge")
                        .WithEnvironment("Keycloak__BaseUrl", keycloakPublicUrl)
                        .WithEnvironment("Keycloak__Realm", "forge")
                        .WithEnvironment("Keycloak__AdminUsername", "admin")
                        .WithEnvironment("Keycloak__AdminPassword", keycloakAdminPassword)
                        .WaitForCompletion(seeder);
                }

                return project.PublishAsDockerComposeService((_, service) =>
                {
                    service.Restart = "unless-stopped";
                    service.Ports = ["127.0.0.1:${APP_PORT:-8080}:8080"];

                    service.DependsOn["migrator"] = new ServiceDependency
                    {
                        Condition = "service_completed_successfully"
                    };

                    service.DependsOn["garage"].Condition = "service_healthy";
                    service.DependsOn["keycloak"].Condition = "service_healthy";
                });
            }
        }
    }
}
