// ------------------------------------------------------------------------------------------------
// <copyright file="ContainerResourceBuilderExtensions.cs" company="Starion Group S.A.">
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
    using Aspire.Hosting.Docker.Resources.ServiceNodes;

    /// <summary>
    /// Provides extension methods for <see cref="IResourceBuilder{ContainerResource}" /> to configure container resources.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ContainerResourceBuilderExtensions
    {
        /// <summary>
        /// The Docker Compose project label used to group containers in Docker Desktop.
        /// </summary>
        private const string DockerComposeProjectLabel = "com.docker.compose.project=forge-aspire";

        /// <param name="container">The <see cref="IResourceBuilder{ContainerResource}" /> to configure.</param>
        extension<T>(IResourceBuilder<T> container) where T : ContainerResource
        {
            /// <summary>
            /// Configures the container resource with a persistent lifetime and the <c>forge-aspire</c> Docker Desktop project group
            /// label.
            /// </summary>
            /// <param name="serviceName">The service name assigned to the <c>com.docker.compose.service</c> label.</param>
            /// <returns>The configured <see cref="IResourceBuilder{T}" /> instance.</returns>
            public IResourceBuilder<T> WithForgeContainerDefaults(string serviceName)
            {
                return container
                    .WithContainerRuntimeArgs("--label", DockerComposeProjectLabel, "--label", $"com.docker.compose.service={serviceName}")
                    .WithLifetime(ContainerLifetime.Persistent);
            }
        }

        /// <param name="container">The <see cref="IResourceBuilder{ContainerResource}" /> to configure.</param>
        extension(IResourceBuilder<ContainerResource> container)
        {
            /// <summary>
            /// Configures the Garage object-storage service for Docker Compose publishing with health checks, a restart policy, and
            /// production tokens.
            /// </summary>
            /// <param name="builder">The distributed application builder.</param>
            /// <returns>The configured <see cref="IResourceBuilder{ContainerResource}" /> instance.</returns>
            public IResourceBuilder<ContainerResource> WithGaragePublishConfiguration(IDistributedApplicationBuilder builder)
            {
                if (builder.ExecutionContext.IsPublishMode)
                {
                    var garageRpcSecret = builder.AddParameter("garage-rpc-secret", true);
                    var garageAdminToken = builder.AddParameter("garage-admin-token", true);
                    var garageMetricsToken = builder.AddParameter("garage-metrics-token", true);

                    container.WithEnvironment("GARAGE_RPC_SECRET", garageRpcSecret)
                        .WithEnvironment("GARAGE_ADMIN_TOKEN", garageAdminToken)
                        .WithEnvironment("GARAGE_METRICS_TOKEN", garageMetricsToken);
                }

                return container.PublishAsDockerComposeService((_, service) =>
                {
                    service.Restart = "unless-stopped";

                    service.Healthcheck = new Healthcheck
                    {
                        Test = ["CMD", "/garage", "status"],
                        Interval = "5s",
                        Timeout = "5s",
                        Retries = 10,
                        StartPeriod = "0s"
                    };
                });
            }

            /// <summary>
            /// Configures the Keycloak service for run mode or Docker Compose publishing mode.
            /// </summary>
            /// <param name="builder">The distributed application builder.</param>
            /// <param name="keycloakPublicUrl">The local development public URL of Keycloak.</param>
            /// <param name="forgePublicUrl">The local development public URL of Forge.</param>
            /// <param name="forgeUrlFromContainers">The URL containers use to reach Forge on the host in run mode.</param>
            /// <param name="keycloakPublicUrlParameter">
            /// When in publish mode, receives the parameter representing the public Keycloak
            /// URL.
            /// </param>
            /// <returns>The configured <see cref="IResourceBuilder{ContainerResource}" /> instance.</returns>
            public IResourceBuilder<ContainerResource> WithKeycloakEnvironmentAndPublishConfiguration(IDistributedApplicationBuilder builder, string keycloakPublicUrl, string forgePublicUrl, string forgeUrlFromContainers, out IResourceBuilder<ParameterResource> keycloakPublicUrlParameter)
            {
                if (builder.ExecutionContext.IsPublishMode)
                {
                    keycloakPublicUrlParameter = builder.AddParameter("keycloak-public-url");
                    var forgePublicUrlParameter = builder.AddParameter("forge-public-url");

                    container.WithArgs("start", "--import-realm")
                        .WithEnvironment("KC_HOSTNAME", keycloakPublicUrlParameter)
                        .WithEnvironment("KC_HTTP_ENABLED", "true")
                        .WithEnvironment("KC_PROXY_HEADERS", "xforwarded")
                        .WithEnvironment("FORGE_BASE_URL", forgePublicUrlParameter)
                        .WithEnvironment("FORGE_DEV_REDIRECT_URI", $"{forgePublicUrlParameter}/*")
                        .WithEnvironment("FORGE_DEV_WEB_ORIGIN", forgePublicUrlParameter)
                        .WithEnvironment("FORGE_KEYCLOAK_SSL_REQUIRED", "external")
                        .WithEnvironment("FORGE_SPI_BASE_URL", "http://forge:8080");
                }
                else
                {
                    keycloakPublicUrlParameter = null;

                    container.WithArgs("start-dev", "--import-realm")
                        .WithEnvironment("KC_HOSTNAME", keycloakPublicUrl)
                        .WithEnvironment("FORGE_BASE_URL", forgePublicUrl)
                        .WithEnvironment("FORGE_KEYCLOAK_SSL_REQUIRED", "none")
                        .WithEnvironment("FORGE_SPI_BASE_URL", forgeUrlFromContainers)
                        .WithBindMount("../docker/keycloak/themes/forge", "/opt/keycloak/themes/forge", true)
                        .WithContainerRuntimeArgs("--add-host", "host.docker.internal:host-gateway");
                }

                return container.PublishAsDockerComposeService((_, service) =>
                {
                    service.Build = new Build
                    {
                        Context = "docker/keycloak",
                        Dockerfile = "Dockerfile"
                    };

                    service.Image = null;
                    service.Restart = "unless-stopped";
                    service.Ports = ["127.0.0.1:${KEYCLOAK_PORT:-8081}:8080"];
                    service.DependsOn["postgres-authentication"].Condition = "service_healthy";

                    service.Healthcheck = new Healthcheck
                    {
                        Test = ["CMD-SHELL", "exec 3<>/dev/tcp/127.0.0.1/9000 && echo -e 'GET /health/ready HTTP/1.1\\r\\nHost: localhost\\r\\nConnection: close\\r\\n\\r\\n' >&3 && grep -q '\"status\": \"UP\"' <&3"],
                        Interval = "10s",
                        Timeout = "5s",
                        Retries = 15,
                        StartPeriod = "0s"
                    };
                });
            }
        }
    }
}
