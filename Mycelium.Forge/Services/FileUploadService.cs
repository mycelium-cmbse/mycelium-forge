// ------------------------------------------------------------------------------------------------
// <copyright file="FileUploadService.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    using ErrorOr;

    using Microsoft.AspNetCore.Components.Forms;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Storage.Stores;

    /// <summary>
    /// Implements operations for uploading and persisting package artifact files.
    /// </summary>
    public class FileUploadService : IFileUploadService
    {
        /// <summary>
        /// The buffer size in bytes used for file streaming operations.
        /// </summary>
        private const int BufferSize = 128 * 1024;

        /// <summary>
        /// The artifact store instance for storing packages in object storage.
        /// </summary>
        private readonly IArtifactStore artifactStore;

        /// <summary>
        /// The JavaScript runtime interop service.
        /// </summary>
        private readonly IJsInterop jsInterop;

        /// <summary>
        /// The logger instance.
        /// </summary>
        private readonly ILogger<FileUploadService> logger;

        /// <summary>
        /// The maximum allowed file size in bytes for circuit-streamed uploads.
        /// </summary>
        private readonly long maxFileSize;

        /// <summary>
        /// The package upload staging service for temporary disk storage.
        /// </summary>
        private readonly IFileUploadStagingService uploadStagingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileUploadService" /> class.
        /// </summary>
        /// <param name="jsInterop">The JavaScript runtime interop service.</param>
        /// <param name="uploadStagingService">The package upload staging service.</param>
        /// <param name="artifactStore">The artifact store instance.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="logger">The logger for diagnostic output.</param>
        public FileUploadService(IJsInterop jsInterop, IFileUploadStagingService uploadStagingService, IArtifactStore artifactStore, IConfiguration configuration, ILogger<FileUploadService> logger)
        {
            this.jsInterop = jsInterop;
            this.uploadStagingService = uploadStagingService;
            this.artifactStore = artifactStore;
            this.logger = logger;

            var configuredSizeInMb = configuration.GetValue<long?>(ConfigurationConstants.MaximumUploadFileSizeInMb);
            this.maxFileSize = (configuredSizeInMb ?? 250L) * 1024 * 1024;
        }

        /// <summary>
        /// Uploads and stores a browser file directly using in-memory streaming over the active component circuit.
        /// </summary>
        /// <param name="file">The browser file selected for upload.</param>
        /// <param name="contentType">The MIME content type of the file payload.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}" /> containing the artifact SHA256 identifier or a failure error.</returns>
        public async Task<ErrorOr<string>> UploadFile(IBrowserFile file, string contentType = "application/octet-stream", CancellationToken cancellationToken = default)
        {
            if (file == null)
            {
                return Error.Validation(description: "No package archive file selected for upload.");
            }

            try
            {
                await using var memoryStream = new MemoryStream();

                await using (var browserStream = file.OpenReadStream(this.maxFileSize, cancellationToken))
                {
                    await browserStream.CopyToAsync(memoryStream, BufferSize, cancellationToken);
                }

                memoryStream.Position = 0;
                return await this.artifactStore.StoreArtifactAsync(memoryStream, contentType, cancellationToken);
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Failed to upload file {FileName} over circuit stream", file.Name);
                return Error.Failure(description: $"Failed to process package archive: {exception.Message}");
            }
        }

        /// <summary>
        /// Streams a large file directly over HTTP fetch to the staging endpoint and persists the staged artifact to object
        /// storage.
        /// </summary>
        /// <param name="inputId">The HTML ID of the file input element.</param>
        /// <param name="contentType">The MIME content type of the file payload.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}" /> containing the artifact SHA256 identifier or a failure error.</returns>
        public async Task<ErrorOr<string>> UploadFileOverHttps(string inputId, string contentType = "application/octet-stream", CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(inputId))
            {
                return Error.Validation(description: "The file input identifier is required.");
            }

            string uploadToken = null;

            try
            {
                uploadToken = await this.jsInterop.UploadLargeFile(inputId, ApiRoutes.UploadFileForStage, contentType);
                await using var stagedStream = this.uploadStagingService.OpenStagedFile(uploadToken);
                return await this.artifactStore.StoreArtifactAsync(stagedStream, contentType, cancellationToken);
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "Failed to upload package archive over HTTPS for input {InputId}", inputId);
                return Error.Failure(description: $"Failed to upload package archive: {exception.Message}");
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(uploadToken))
                {
                    this.uploadStagingService.Cleanup(uploadToken);
                }
            }
        }
    }
}
