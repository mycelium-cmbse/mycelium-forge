// ------------------------------------------------------------------------------------------------
// <copyright file="IArtifactStore.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Storage.Stores
{
    using ErrorOr;

    /// <summary>
    /// Defines operations for resolving and storing package artifacts by content hash.
    /// </summary>
    public interface IArtifactStore
    {
        /// <summary>
        /// Asynchronously retrieves an artifact stream by its content hash.
        /// </summary>
        /// <param name="contentHash">The content hash of the artifact.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A result containing the artifact stream if found, or an error.</returns>
        Task<ErrorOr<Stream>> GetArtifactAsync(string contentHash, CancellationToken token = default);

        /// <summary>
        /// Asynchronously stores an artifact stream under its computed content hash, skipping upload if already present.
        /// </summary>
        /// <remarks>
        /// Callers must provide a seekable stream. Single-part upload via PutObject is used since standard S3 supports payloads up
        /// to 5 GB (see https://docs.aws.amazon.com/AmazonS3/latest/userguide/upload-objects.html), safely covering the 250 MB
        /// limit without multipart complexity.
        /// </remarks>
        /// <param name="contentStream">The stream containing artifact bytes.</param>
        /// <param name="contentType">The optional MIME type of the artifact.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A result containing the stored content hash, or an error if storage fails.</returns>
        Task<ErrorOr<string>> StoreArtifactAsync(Stream contentStream, string contentType = null, CancellationToken token = default);

        /// <summary>
        /// Asynchronously checks whether an artifact exists in storage by its content hash.
        /// </summary>
        /// <param name="contentHash">The content hash of the artifact.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A result indicating whether the artifact exists.</returns>
        Task<ErrorOr<bool>> ExistsAsync(string contentHash, CancellationToken token = default);

        /// <summary>
        /// Asynchronously deletes an artifact from storage by its content hash.
        /// </summary>
        /// <param name="contentHash">The content hash of the artifact to delete.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A result indicating success or failure of the deletion.</returns>
        Task<ErrorOr<Deleted>> DeleteArtifactAsync(string contentHash, CancellationToken token = default);

        /// <summary>
        /// Asynchronously lists stored artifact content hashes, supporting pagination for cleanup jobs.
        /// </summary>
        /// <param name="continuationToken">The continuation token for pagination.</param>
        /// <param name="maxKeys">The maximum number of keys to return.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A result containing a tuple with the list of content hashes and an optional next continuation token.</returns>
        Task<ErrorOr<(IReadOnlyList<string> Hashes, string NextContinuationToken)>> ListArtifactHashesAsync(string continuationToken = null, int maxKeys = 1000, CancellationToken token = default);
    }
}
