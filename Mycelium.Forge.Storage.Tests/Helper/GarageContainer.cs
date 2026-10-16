// ------------------------------------------------------------------------------------------------
// <copyright file="GarageContainer.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Storage.Tests.Helper
{
    using Amazon.S3;

    using DotNet.Testcontainers.Builders;
    using DotNet.Testcontainers.Containers;

    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Options;

    using Mycelium.Forge.Storage.Models;
    using Mycelium.Forge.Storage.Stores;

    using BasicAWSCredentials = Amazon.Runtime.BasicAWSCredentials;

    /// <summary>
    /// Encapsulates the lifecycle, configuration, and S3 client setup for a self-contained Garage test container.
    /// </summary>
    public sealed class GarageContainer : IAsyncDisposable
    {
        /// <summary>
        /// The default bucket name used for integration testing.
        /// </summary>
        private const string BucketName = "forge-artefacts";

        /// <summary>
        /// The local development access key used by Garage.
        /// </summary>
        private const string AccessKey = "GKdevlocal000000000";

        /// <summary>
        /// The local development secret key used by Garage.
        /// </summary>
        private const string SecretKey = "dev0000000000000000000000000000000000000000000000000000000000";

        /// <summary>
        /// The region configured for the Garage S3 endpoint.
        /// </summary>
        private const string Region = "garage";

        /// <summary>
        /// Garage server requires an /etc/garage.toml file to boot because it does not support pure environment-variable
        /// configuration for storage paths and RPC cluster settings.
        /// </summary>
        private static readonly byte[] GarageConfigBytes =
        [
            .. """
               metadata_dir = "/var/lib/garage/meta"
               data_dir = "/var/lib/garage/data"
               db_engine = "sqlite"

               replication_factor = 1

               rpc_bind_addr = "[::]:3901"
               rpc_public_addr = "127.0.0.1:3901"
               rpc_secret = "303ab2f89e73c94250ef6518327de64af57061dec1a0eb1ed1307ffd00ca6b50"

               [s3_api]
               s3_region = "garage"
               api_bind_addr = "[::]:3900"
               root_domain = ".s3.garage.localhost"

               [admin]
               api_bind_addr = "[::]:3903"
               admin_token = "dev-admin-token-0000000000000000000000000000"
               metrics_token = "dev-metrics-token-0000000000000000000000000"
               """u8
        ];

        /// <summary>
        /// The underlying Testcontainers Docker container instance.
        /// </summary>
        private readonly IContainer container = new ContainerBuilder("dxflrs/garage:v2.4.1")
            .WithCommand("/garage", "server", "--single-node", "--default-bucket")
            .WithEnvironment("GARAGE_DEFAULT_ACCESS_KEY", AccessKey)
            .WithEnvironment("GARAGE_DEFAULT_SECRET_KEY", SecretKey)
            .WithEnvironment("GARAGE_DEFAULT_BUCKET", BucketName)
            .WithResourceMapping(GarageConfigBytes, "/etc/garage.toml")
            .WithPortBinding(3900, true)
            .Build();

        /// <summary>
        /// The Amazon S3 client connected to the Garage container.
        /// </summary>
        private AmazonS3Client amazonS3Client;

        /// <summary>
        /// Gets the artifact store instance backed by the Garage container.
        /// </summary>
        public ArtifactStore ArtifactStore { get; private set; }

        /// <summary>
        /// Disposes the Amazon S3 client and the underlying Garage container.
        /// </summary>
        /// <returns>A <see cref="ValueTask" /> representing the asynchronous dispose operation.</returns>
        public async ValueTask DisposeAsync()
        {
            this.amazonS3Client?.Dispose();
            await this.container.DisposeAsync();
        }

        /// <summary>
        /// Starts the Garage container and initializes the S3 client and artifact store.
        /// </summary>
        /// <returns>An awaitable <see cref="Task" /> representing the asynchronous start operation.</returns>
        public async Task StartAsync()
        {
            await this.container.StartAsync();

            var mappedPort = this.container.GetMappedPublicPort(3900);
            var serviceUrl = $"http://localhost:{mappedPort}";

            var options = Options.Create(new ArtifactStoreOptions
            {
                ServiceUrl = serviceUrl,
                Region = Region,
                BucketName = BucketName,
                AccessKey = AccessKey,
                SecretKey = SecretKey,
                ForcePathStyle = true
            });

            var s3Config = new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                ForcePathStyle = true,
                UseHttp = true,
                AuthenticationRegion = Region
            };

            var credentials = new BasicAWSCredentials(AccessKey, SecretKey);
            this.amazonS3Client = new AmazonS3Client(credentials, s3Config);

            var logger = NullLogger<ArtifactStore>.Instance;
            this.ArtifactStore = new ArtifactStore(this.amazonS3Client, options, logger);
        }
    }
}
