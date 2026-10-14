// ------------------------------------------------------------------------------------------------
// <copyright file="IFileUploadService.cs" company="Starion Group S.A.">
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

    /// <summary>
    /// Defines operations for uploading and persisting package artifact files.
    /// </summary>
    public interface IFileUploadService
    {
        /// <summary>
        /// Uploads and stores a browser file directly using in-memory streaming over the active component circuit.
        /// </summary>
        /// <param name="file">The browser file selected for upload.</param>
        /// <param name="contentType">The MIME content type of the file payload.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}" /> containing the artifact SHA256 identifier or a failure error.</returns>
        Task<ErrorOr<string>> UploadFile(IBrowserFile file, string contentType = "application/octet-stream", CancellationToken cancellationToken = default);

        /// <summary>
        /// Streams a large file directly over HTTP fetch to the staging endpoint and persists the staged artifact to object
        /// storage.
        /// </summary>
        /// <param name="inputId">The HTML ID of the file input element.</param>
        /// <param name="contentType">The MIME content type of the file payload.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}" /> containing the artifact SHA256 identifier or a failure error.</returns>
        Task<ErrorOr<string>> UploadFileOverHttps(string inputId, string contentType = "application/octet-stream", CancellationToken cancellationToken = default);
    }
}
