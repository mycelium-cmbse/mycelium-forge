// ------------------------------------------------------------------------------------------------
// <copyright file="DockerComposeEnvironmentResourceBuilderExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.AppHost.Extensions
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Aspire.Hosting;
    using Aspire.Hosting.ApplicationModel;
    using Aspire.Hosting.Docker;
    using Aspire.Hosting.Docker.Resources;

    /// <summary>
    /// Provides extension methods for <see cref="IResourceBuilder{DockerComposeEnvironmentResource}" /> to configure the
    /// published Docker Compose and <c>.env</c> files.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DockerComposeEnvironmentResourceBuilderExtensions
    {
        /// <summary>
        /// The container image expression for Forge and the migrator, requiring an explicit release <c>TAG</c> in <c>.env</c>.
        /// </summary>
        private const string ForgeImageExpression = "${IMAGE:-ghcr.io/mycelium-cmbse/mycelium-forge}:${TAG:?set TAG in .env}";

        /// <summary>
        /// The environment variables that must be provided in <c>.env</c> when running the published Docker Compose file.
        /// </summary>
        private static readonly HashSet<string> RequiredPublishVariables =
        [
            "POSTGRES_PASSWORD",
            "FORGE_RUNTIME_PASSWORD",
            "KEYCLOAK_DB_PASSWORD",
            "KEYCLOAK_ADMIN_PASSWORD",
            "KEYCLOAK_PUBLIC_URL",
            "FORGE_PUBLIC_URL",
            "FORGE_CLIENT_SECRET",
            "KEYCLOAK_SPI_API_KEY",
            "ARTIFACT_STORE_ACCESS_KEY",
            "ARTIFACT_STORE_SECRET_KEY",
            "GARAGE_RPC_SECRET",
            "GARAGE_ADMIN_TOKEN",
            "GARAGE_METRICS_TOKEN"
        ];

        /// <summary>
        /// The default values for non-secret configuration variables in the published compose file.
        /// </summary>
        private static readonly Dictionary<string, string> DefaultEnvValues = new()
        {
            { "DATABASE_USERNAME", "forge" },
            { "AUTHENTICATION_DATABASE_USERNAME", "keycloak" },
            { "FORGE_PORT", "8080" }
        };

        /// <param name="environment">The <see cref="IResourceBuilder{DockerComposeEnvironmentResource}" /> to configure.</param>
        extension(IResourceBuilder<DockerComposeEnvironmentResource> environment)
        {
            /// <summary>
            /// Configures the published Docker Compose and <c>.env</c> files with fail-fast required variable expressions, portable
            /// relative bind-mount paths, and production defaults.
            /// </summary>
            /// <returns>The configured <see cref="IResourceBuilder{DockerComposeEnvironmentResource}" /> instance.</returns>
            public IResourceBuilder<DockerComposeEnvironmentResource> WithProductionComposeConfiguration()
            {
                return environment
                    .WithDashboard(false)
                    .ConfigureComposeFile(ConfigureComposeFile)
                    .ConfigureEnvFile(ConfigureEnvFile);
            }
        }

        /// <summary>
        /// Rewrites required environment variable placeholders in the compose file to fail fast if unset in <c>.env</c> and
        /// normalizes bind-mount paths and image tags.
        /// </summary>
        /// <param name="composeFile">The Docker Compose file model.</param>
        private static void ConfigureComposeFile(ComposeFile composeFile)
        {
            composeFile.Name = "forge";

            foreach (var service in composeFile.Services.Values)
            {
                foreach (var key in service.Environment.Keys.ToList())
                {
                    var value = service.Environment[key];

                    if (string.IsNullOrEmpty(value))
                    {
                        continue;
                    }

                    var updated = Regex.Replace(value, @"\$\{(?<name>[A-Z0-9_]+)\}", ReplacePlaceholder);
                    service.Environment[key] = updated;
                }

                foreach (var volume in service.Volumes)
                {
                    if (volume.Source == "${POSTGRES_BINDMOUNT_0}")
                    {
                        volume.Source = "./docker/postgres/00-create-roles.sh";
                    }
                    else if (volume.Source == "${GARAGE_BINDMOUNT_0}")
                    {
                        volume.Source = "./docker/garage.prod.toml";
                    }
                }
            }

            if (composeFile.Services.TryGetValue("migrator", out var migratorService))
            {
                migratorService.Image = ForgeImageExpression;
            }

            if (composeFile.Services.TryGetValue("forge", out var forgeService))
            {
                forgeService.Image = ForgeImageExpression;
            }
        }

        /// <summary>
        /// Populates default non-secret values, adds <c>IMAGE</c> and <c>TAG</c>, and removes unused variables in the generated
        /// <c>.env</c> file.
        /// </summary>
        /// <param name="envVars">The captured environment variables for the <c>.env</c> file.</param>
        private static void ConfigureEnvFile(IDictionary<string, CapturedEnvironmentVariable> envVars)
        {
            foreach (var pair in DefaultEnvValues)
            {
                if (envVars.TryGetValue(pair.Key, out var variable))
                {
                    variable.DefaultValue = pair.Value;
                }
            }

            envVars["IMAGE"] = new CapturedEnvironmentVariable
            {
                Name = "IMAGE",
                Description = "Image to deploy. Defaults to the GHCR image published on every GitHub release.",
                DefaultValue = "ghcr.io/mycelium-cmbse/mycelium-forge"
            };

            envVars["TAG"] = new CapturedEnvironmentVariable
            {
                Name = "TAG",
                Description = "The release tag to deploy, e.g. v0.3.0. Avoid latest in production so a deploy is reproducible."
            };

            envVars.Remove("FORGE_IMAGE");
            envVars.Remove("MIGRATOR_IMAGE");
            envVars.Remove("KEYCLOAK_IMAGE");
            envVars.Remove("POSTGRES_BINDMOUNT_0");
            envVars.Remove("GARAGE_BINDMOUNT_0");
        }

        /// <summary>
        /// Replaces a matched <c>${VAR}</c> placeholder with <c>${VAR:?set VAR in .env}</c> when required, or
        /// <c>${VAR:-default}</c> when a default value exists in <see cref="DefaultEnvValues" />.
        /// </summary>
        /// <param name="match">The regular expression match.</param>
        /// <returns>The replacement placeholder string.</returns>
        private static string ReplacePlaceholder(Match match)
        {
            var name = match.Groups["name"].Value;

            if (RequiredPublishVariables.Contains(name))
            {
                return $"${{{name}:?set {name} in .env}}";
            }

            if (DefaultEnvValues.TryGetValue(name, out var defaultValue))
            {
                return $"${{{name}:-{defaultValue}}}";
            }

            return match.Value;
        }
    }
}
