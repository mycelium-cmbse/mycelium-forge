// ------------------------------------------------------------------------------------------------
// <copyright file="PublishUploadStep.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.Publish
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Represents the first step (upload) component of the package publishing wizard.
    /// </summary>
    public partial class PublishUploadStep : ComponentBase
    {
        /// <summary>
        /// The dictionary mapping supported file extensions to human-readable format names.
        /// </summary>
        /// <remarks>Used just as a mock until we do a real parsing of the model</remarks>
        private readonly Dictionary<string, string> acceptedFormats = new()
        {
            { ".kpar", "SysML v2 (kpar)" },
            { ".zip", "ECSS-E-TM-10-25" },
            { ".tar.gz", "Capella" }
        };

        /// <summary>
        /// The maximum allowed upload file size in bytes.
        /// </summary>
        private long maxAllowedFileSize;

        /// <summary>
        /// The currently selected browser file.
        /// </summary>
        private IBrowserFile selectedFile;

        /// <summary>
        /// Gets or sets the application configuration.
        /// </summary>
        [Inject]
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets the comma-separated list of accepted file extensions.
        /// </summary>
        private string AcceptedExtensions => string.Join(",", this.acceptedFormats.Keys);

        /// <summary>
        /// Gets a value indicating whether a file is currently selected.
        /// </summary>
        private bool HasSelectedFile => this.selectedFile != null;

        /// <summary>
        /// Gets or sets the callback invoked when the user proceeds to the next step.
        /// </summary>
        [Parameter]
        public EventCallback<IBrowserFile> OnContinue { get; set; }

        /// <summary>
        /// Gets or sets the HTML ID applied to the dropzone file input element.
        /// </summary>
        [Parameter]
        public string InputId { get; set; } = "publish-package-file-input";

        /// <summary>
        /// Initializes the component lifecycle and resolves configured upload limits.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            var configuredSizeInMb = this.Configuration.GetValue<long?>(ConfigurationConstants.MaximumUploadFileSizeInMb);
            this.maxAllowedFileSize = (configuredSizeInMb ?? 250L) * 1024 * 1024;
        }

        /// <summary>
        /// Gets the detected format name of the selected file.
        /// </summary>
        /// <returns>The human-readable format name or file extension.</returns>
        private string GetSelectedFileFormat()
        {
            var fileExtension = Path.GetExtension(this.selectedFile.Name);
            var extension = !string.IsNullOrEmpty(fileExtension) ? fileExtension : "unknown";

            // If format is a recognized model extension, return the corresponding format name; otherwise, return the file extension.
            return this.acceptedFormats.TryGetValue(extension, out var format)
                ? format
                : extension;
        }

        /// <summary>
        /// Handles the selected files change event from the dropzone upload component.
        /// </summary>
        /// <param name="files">The list of selected files.</param>
        private void HandleFilesSelected(IReadOnlyList<IBrowserFile> files)
        {
            if (files is { Count: > 0 })
            {
                this.selectedFile = files[0];
            }
        }

        /// <summary>
        /// Handles the continue to metadata button click.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task HandleContinue()
        {
            await this.OnContinue.InvokeAsync(this.selectedFile);
        }
    }
}
