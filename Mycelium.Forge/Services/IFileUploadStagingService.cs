// ------------------------------------------------------------------------------------------------
// <copyright file="IFileUploadStagingService.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    /// <summary>
    /// Defines operations for temporarily staging large package upload streams on local disk prior to publishing.
    /// </summary>
    public interface IFileUploadStagingService : IDisposable
    {
        /// <summary>
        /// Asynchronously streams an incoming package upload body directly to a temporary staging file on disk.
        /// </summary>
        /// <param name="inputStream">The incoming HTTP body stream.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A unique upload token identifying the staged file.</returns>
        Task<string> StageAsync(Stream inputStream, CancellationToken token = default);

        /// <summary>
        /// Opens a seekable read stream for the specified staged upload token.
        /// </summary>
        /// <param name="uploadToken">The unique upload token identifying the staged file.</param>
        /// <returns>A readable <see cref="Stream" /> representing the staged package file.</returns>
        Stream OpenStagedFile(string uploadToken);

        /// <summary>
        /// Deletes the temporary staged file associated with the specified upload token.
        /// </summary>
        /// <param name="uploadToken">The unique upload token identifying the staged file to delete.</param>
        void Cleanup(string uploadToken);
    }
}
