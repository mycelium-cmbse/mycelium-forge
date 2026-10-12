// ------------------------------------------------------------------------------------------------
// <copyright file="ArtifactStoreOptions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Storage.Models
{
    /// <summary>
    /// Configuration options for connecting to S3-compatible content-addressed object storage.
    /// </summary>
    public class ArtifactStoreOptions
    {
        /// <summary>
        /// The configuration section name used in application settings.
        /// </summary>
        public const string SectionName = "ArtifactStore";

        /// <summary>
        /// Gets or sets the service endpoint URL of the object storage cluster.
        /// </summary>
        public string ServiceUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the region identifier for S3 authentication.
        /// </summary>
        public string Region { get; set; } = "garage";

        /// <summary>
        /// Gets or sets the bucket name used to store artifact blobs.
        /// </summary>
        public string BucketName { get; set; } = "forge-artefacts";

        /// <summary>
        /// Gets or sets the access key credential.
        /// </summary>
        public string AccessKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the secret key credential.
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether path-style addressing is used instead of virtual-hosted style.
        /// </summary>
        public bool ForcePathStyle { get; set; } = true;
    }
}
