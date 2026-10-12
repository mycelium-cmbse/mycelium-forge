// ------------------------------------------------------------------------------------------------
// <copyright file="StorageServiceCollectionExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Storage.Extensions
{
    using Amazon.Runtime;
    using Amazon.S3;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    using Mycelium.Forge.Storage.Models;
    using Mycelium.Forge.Storage.Stores;

    /// <summary>
    /// Provides extension methods for registering artifact storage dependencies in the service collection.
    /// </summary>
    public static class StorageServiceCollectionExtensions
    {
        /// <summary>
        /// Registers artifact store options, the Amazon S3 client, and the <see cref="IArtifactStore" /> implementation.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to configure.</param>
        /// <param name="configuration">The application configuration providing storage settings.</param>
        /// <returns>The configured <see cref="IServiceCollection" /> instance.</returns>
        public static IServiceCollection RegisterArtifactStore(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configuration);

            services.Configure<ArtifactStoreOptions>(configuration.GetSection(ArtifactStoreOptions.SectionName));

            services.AddSingleton<IAmazonS3>(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<ArtifactStoreOptions>>().Value;

                var s3Config = new AmazonS3Config
                {
                    ServiceURL = options.ServiceUrl,
                    ForcePathStyle = options.ForcePathStyle,
                    UseHttp = options.ServiceUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                };

                if (!string.IsNullOrWhiteSpace(options.Region))
                {
                    s3Config.AuthenticationRegion = options.Region;
                }

                var credentials = new BasicAWSCredentials(options.AccessKey, options.SecretKey);
                return new AmazonS3Client(credentials, s3Config);
            });

            services.AddScoped<IArtifactStore, ArtifactStore>();

            return services;
        }
    }
}
