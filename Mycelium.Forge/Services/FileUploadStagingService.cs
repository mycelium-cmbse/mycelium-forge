// ------------------------------------------------------------------------------------------------
// <copyright file="FileUploadStagingService.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    using System.Collections.Concurrent;

    /// <summary>
    /// Implements operations for temporarily staging large package upload streams on local disk prior to publishing.
    /// </summary>
    public class FileUploadStagingService : IFileUploadStagingService
    {
        /// <summary>
        /// The buffer size in bytes used for file streaming operations.
        /// </summary>
        private const int BufferSize = 128 * 1024;

        /// <summary>
        /// The logger instance.
        /// </summary>
        private readonly ILogger<FileUploadStagingService> logger;

        /// <summary>
        /// The concurrent dictionary mapping upload tokens to staged temporary file paths.
        /// </summary>
        private readonly ConcurrentDictionary<string, string> stagedFiles = new();

        /// <summary>
        /// The directory path used for temporary staging files.
        /// </summary>
        private readonly string stagingDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileUploadStagingService" /> class.
        /// </summary>
        /// <param name="logger">The logger for diagnostic output.</param>
        public FileUploadStagingService(ILogger<FileUploadStagingService> logger)
        {
            this.logger = logger;
            this.stagingDirectory = Path.Combine(Path.GetTempPath(), "mycelium_forge_staged_uploads");
            this.DeleteStagingDirectory();
            Directory.CreateDirectory(this.stagingDirectory);
        }

        /// <summary>
        /// Asynchronously streams an incoming package upload body directly to a temporary staging file on disk.
        /// </summary>
        /// <param name="inputStream">The incoming HTTP body stream.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A unique upload token identifying the staged file.</returns>
        public async Task<string> StageAsync(Stream inputStream, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(inputStream);

            var uploadToken = Guid.NewGuid().ToString("N");
            var filePath = Path.Combine(this.stagingDirectory, $"{uploadToken}.tmp");

            try
            {
                await using var fileStream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None, BufferSize, FileOptions.Asynchronous);
                await inputStream.CopyToAsync(fileStream, BufferSize, token);
            }
            catch (Exception exception)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                this.logger.LogWarning(exception, "Package upload staging aborted or cancelled for token {UploadToken}", uploadToken);
                throw;
            }

            this.stagedFiles.TryAdd(uploadToken, filePath);
            this.logger.LogDebug("Staged incoming package upload {UploadToken} to {FilePath}", uploadToken, filePath);

            return uploadToken;
        }

        /// <summary>
        /// Opens a seekable read stream for the specified staged upload token.
        /// </summary>
        /// <param name="uploadToken">The unique upload token identifying the staged file.</param>
        /// <returns>A readable <see cref="Stream" /> representing the staged package file.</returns>
        public Stream OpenStagedFile(string uploadToken)
        {
            if (string.IsNullOrWhiteSpace(uploadToken) || !this.stagedFiles.TryGetValue(uploadToken, out var filePath) || !File.Exists(filePath))
            {
                throw new FileNotFoundException($"No staged upload found for token '{uploadToken}'.");
            }

            this.stagedFiles.TryRemove(uploadToken, out _);
            return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, FileOptions.Asynchronous | FileOptions.DeleteOnClose);
        }

        /// <summary>
        /// Deletes the temporary staged file associated with the specified upload token.
        /// </summary>
        /// <param name="uploadToken">The unique upload token identifying the staged file to delete.</param>
        public void Cleanup(string uploadToken)
        {
            if (string.IsNullOrWhiteSpace(uploadToken))
            {
                return;
            }

            if (!this.stagedFiles.TryRemove(uploadToken, out var filePath) || !File.Exists(filePath))
            {
                return;
            }

            try
            {
                File.Delete(filePath);
                this.logger.LogDebug("Cleaned up staged package upload {UploadToken} at {FilePath}", uploadToken, filePath);
            }
            catch (Exception exception)
            {
                this.logger.LogWarning(exception, "Failed to delete temporary staged file at {FilePath}", filePath);
            }
        }

        /// <summary>
        /// Releases all managed and unmanaged resources, deleting any remaining staged temporary files.
        /// </summary>
        public void Dispose()
        {
            foreach (var uploadToken in this.stagedFiles.Keys)
            {
                this.Cleanup(uploadToken);
            }

            this.DeleteStagingDirectory();
        }

        /// <summary>
        /// Deletes the temporary staging directory and all of its contents if it exists.
        /// </summary>
        private void DeleteStagingDirectory()
        {
            if (!Directory.Exists(this.stagingDirectory))
            {
                return;
            }

            try
            {
                Directory.Delete(this.stagingDirectory, true);
            }
            catch (Exception exception)
            {
                this.logger.LogWarning(exception, "Failed to delete existing staging directory {Directory}", this.stagingDirectory);
            }
        }
    }
}
