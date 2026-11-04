// ------------------------------------------------------------------------------------------------
// <copyright file="PostgresServerResourceBuilderExtensions.cs" company="Starion Group S.A.">
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
    /// Provides extension methods for <see cref="IResourceBuilder{PostgresServerResource}" /> to configure PostgreSQL
    /// services.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class PostgresServerResourceBuilderExtensions
    {
        /// <param name="postgres">The <see cref="IResourceBuilder{PostgresServerResource}" /> to configure.</param>
        extension(IResourceBuilder<PostgresServerResource> postgres)
        {
            /// <summary>
            /// Configures the PostgreSQL server resource with a health check and restart policy for Docker Compose publishing.
            /// </summary>
            /// <param name="user">The database user name checked by <c>pg_isready</c>.</param>
            /// <param name="database">The database name checked by <c>pg_isready</c>.</param>
            /// <returns>The configured <see cref="IResourceBuilder{PostgresServerResource}" /> instance.</returns>
            public IResourceBuilder<PostgresServerResource> WithPostgresPublishConfiguration(string user, string database)
            {
                return postgres.PublishAsDockerComposeService((_, service) =>
                {
                    service.Restart = "unless-stopped";

                    service.Healthcheck = new Healthcheck
                    {
                        Test = ["CMD-SHELL", $"pg_isready -U {user} -d {database}"],
                        Interval = "5s",
                        Timeout = "5s",
                        Retries = 10,
                        StartPeriod = "0s"
                    };
                });
            }
        }
    }
}
