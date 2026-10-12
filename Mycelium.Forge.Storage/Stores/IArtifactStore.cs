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
        Task<ErrorOr<string>> StoreArtifactAsync(string contentHash, Stream contentStream, long contentLength, string contentType = null, CancellationToken token = default);

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
