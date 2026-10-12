// ------------------------------------------------------------------------------------------------
// <copyright file="ArtifactStore.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Storage.Stores
{
    using System.Net;

    using Amazon.S3;
    using Amazon.S3.Model;

    using ErrorOr;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Mycelium.Forge.Storage.Models;

    /// <summary>
    /// Implements content-addressed artifact blob storage against S3-compatible stores.
    /// </summary>
    public class ArtifactStore : IArtifactStore
    {
        /// <summary>
        /// The Amazon S3 client used for communicating with object storage.
        /// </summary>
        private readonly IAmazonS3 amazonS3Client;

        /// <summary>
        /// The logger instance.
        /// </summary>
        private readonly ILogger<ArtifactStore> logger;

        /// <summary>
        /// The options monitor providing access to the current storage configuration.
        /// </summary>
        private readonly IOptions<ArtifactStoreOptions> options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtifactStore" /> class.
        /// </summary>
        /// <param name="amazonS3Client">The configured S3 client.</param>
        /// <param name="options">The storage configuration options.</param>
        /// <param name="logger">The logger for diagnostic output.</param>
        public ArtifactStore(IAmazonS3 amazonS3Client, IOptions<ArtifactStoreOptions> options, ILogger<ArtifactStore> logger)
        {
            ArgumentNullException.ThrowIfNull(amazonS3Client);
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(logger);

            this.amazonS3Client = amazonS3Client;
            this.options = options;
            this.logger = logger;
        }

        /// <summary>
        /// Asynchronously retrieves an artifact stream by its content hash.
        /// </summary>
        /// <param name="contentHash">The content hash of the artifact.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A result containing the artifact stream if found, or an error.</returns>
        public async Task<ErrorOr<Stream>> GetArtifactAsync(string contentHash, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(contentHash))
            {
                return Error.Validation(nameof(contentHash), "Content hash cannot be null or empty.");
            }

            var key = this.NormalizeContentHash(contentHash);
            var bucketName = this.options.Value.BucketName;

            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = key
            };

            try
            {
                var response = await this.amazonS3Client.GetObjectAsync(request, token);
                return response.ResponseStream;
            }
            catch (AmazonS3Exception amazonException) when (amazonException.StatusCode == HttpStatusCode.NotFound)
            {
                return Error.NotFound(description: $"Artifact with content hash '{key}' was not found in bucket '{bucketName}'.");
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Failed to retrieve artifact with hash {ContentHash} from bucket {BucketName}.", key, bucketName);
                return Error.Failure(description: exception.Message);
            }
        }

        /// <summary>
        /// Asynchronously stores an artifact stream under its content hash, skipping upload if already present.
        /// </summary>
        /// <remarks>
        /// For artifacts exceeding single-part upload limits or multi-gigabyte files, this operation can be extended
        /// to leverage S3 multipart upload (InitiateMultipartUpload, UploadPart, CompleteMultipartUpload).
        /// </remarks>
        /// <param name="contentHash">The expected content hash of the artifact.</param>
        /// <param name="contentStream">The stream containing artifact bytes.</param>
        /// <param name="contentLength">The total byte length of the artifact stream.</param>
        /// <param name="contentType">The optional MIME type of the artifact.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A result containing the stored content hash, or an error if storage fails.</returns>
        public async Task<ErrorOr<string>> StoreArtifactAsync(string contentHash, Stream contentStream, long contentLength, string contentType = null, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(contentHash))
            {
                return Error.Validation(nameof(contentHash), "Content hash cannot be null or empty.");
            }

            if (contentStream == null)
            {
                return Error.Validation(nameof(contentStream), "Content stream cannot be null.");
            }

            var key = this.NormalizeContentHash(contentHash);
            var bucketName = this.options.Value.BucketName;

            try
            {
                var existsResult = await this.ExistsAsync(key, token);

                if (existsResult is { IsError: false, Value: true })
                {
                    return key;
                }

                await this.amazonS3Client.EnsureBucketExistsAsync(bucketName);

                var isHttps = this.options.Value.ServiceUrl != null &&
                              this.options.Value.ServiceUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase);

                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = key,
                    InputStream = contentStream,
                    AutoCloseStream = false,
                    ContentType = string.IsNullOrWhiteSpace(contentType) ? "application/octet-stream" : contentType,
                    DisablePayloadSigning = isHttps,
                    UseChunkEncoding = false,
                    ChecksumAlgorithm = ChecksumAlgorithm.SHA256,
                    ChecksumSHA256 = Convert.ToBase64String(Convert.FromHexString(key)),
                    Headers =
                    {
                        ContentLength = contentLength
                    }
                };

                await this.amazonS3Client.PutObjectAsync(putRequest, token);
                return key;
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Failed to store artifact with hash {ContentHash} into bucket {BucketName}.", key, bucketName);
                return Error.Failure(description: exception.Message);
            }
        }

        /// <summary>
        /// Asynchronously checks whether an artifact exists in storage by its content hash.
        /// </summary>
        /// <param name="contentHash">The content hash of the artifact.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A result indicating whether the artifact exists.</returns>
        public async Task<ErrorOr<bool>> ExistsAsync(string contentHash, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(contentHash))
            {
                return Error.Validation(nameof(contentHash), "Content hash cannot be null or empty.");
            }

            var key = this.NormalizeContentHash(contentHash);
            var bucketName = this.options.Value.BucketName;

            var request = new GetObjectMetadataRequest
            {
                BucketName = bucketName,
                Key = key
            };

            try
            {
                await this.amazonS3Client.GetObjectMetadataAsync(request, token);
                return true;
            }
            catch (AmazonS3Exception amazonException) when (amazonException.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Failed to check artifact existence for hash {ContentHash} in bucket {BucketName}.", key, bucketName);
                return Error.Failure(description: exception.Message);
            }
        }

        /// <summary>
        /// Asynchronously deletes an artifact from storage by its content hash.
        /// </summary>
        /// <param name="contentHash">The content hash of the artifact to delete.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A result indicating success or failure of the deletion.</returns>
        public async Task<ErrorOr<Deleted>> DeleteArtifactAsync(string contentHash, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(contentHash))
            {
                return Error.Validation(nameof(contentHash), "Content hash cannot be null or empty.");
            }

            var key = this.NormalizeContentHash(contentHash);
            var bucketName = this.options.Value.BucketName;

            var request = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = key
            };

            try
            {
                await this.amazonS3Client.DeleteObjectAsync(request, token);
                return Result.Deleted;
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Failed to delete artifact with hash {ContentHash} from bucket {BucketName}.", key, bucketName);
                return Error.Failure(description: exception.Message);
            }
        }

        /// <summary>
        /// Asynchronously lists stored artifact content hashes, supporting pagination for cleanup jobs.
        /// </summary>
        /// <param name="continuationToken">The continuation token for pagination.</param>
        /// <param name="maxKeys">The maximum number of keys to return.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A result containing a tuple with the list of content hashes and an optional next continuation token.</returns>
        public async Task<ErrorOr<(IReadOnlyList<string> Hashes, string NextContinuationToken)>> ListArtifactHashesAsync(string continuationToken = null, int maxKeys = 1000, CancellationToken token = default)
        {
            var bucketName = this.options.Value.BucketName;

            var request = new ListObjectsV2Request
            {
                BucketName = bucketName,
                ContinuationToken = continuationToken,
                MaxKeys = maxKeys
            };

            try
            {
                var response = await this.amazonS3Client.ListObjectsV2Async(request, token);
                var hashes = new List<string>();

                foreach (var s3Object in response.S3Objects)
                {
                    hashes.Add(s3Object.Key);
                }

                return (hashes, response.NextContinuationToken);
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Failed to list artifact hashes from bucket {BucketName}.", bucketName);
                return Error.Failure(description: exception.Message);
            }
        }

        /// <summary>
        /// Normalizes a content hash string by trimming and converting to lowercase invariant hexadecimal representation.
        /// </summary>
        /// <param name="contentHash">The raw content hash string.</param>
        /// <returns>The normalized content hash string.</returns>
        private string NormalizeContentHash(string contentHash)
        {
            return contentHash.Trim().ToLowerInvariant();
        }
    }
}
