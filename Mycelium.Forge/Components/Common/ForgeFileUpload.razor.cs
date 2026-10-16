// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeFileUpload.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Common
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;

    /// <summary>
    /// Represents a common dropzone file upload component styled according to the Forge design specification.
    /// </summary>
    public partial class ForgeFileUpload : ComponentBase
    {
        /// <summary>
        /// The default maximum allowed file size in megabytes.
        /// </summary>
        public const long DefaultMaxFileSizeInMb = 30;

        /// <summary>
        /// Backing field holding the current validation error message.
        /// </summary>
        private string errorMessage = string.Empty;

        /// <summary>
        /// Backing field indicating whether a drag operation is currently hovering over the dropzone.
        /// </summary>
        private bool isDraggingOver;

        /// <summary>
        /// Backing collection containing the currently selected files.
        /// </summary>
        private IReadOnlyList<IBrowserFile> selectedFiles = [];

        /// <summary>
        /// Gets or sets the HTML ID applied to the underlying input file element.
        /// </summary>
        [Parameter]
        public string InputId { get; set; } = "forge-file-input";

        /// <summary>
        /// Gets a value indicating whether multiple files can be selected simultaneously based on the allowed file count.
        /// </summary>
        private bool Multiple => this.MaxAllowedFiles > 1;

        /// <summary>
        /// Gets or sets the title text displayed in the dropzone.
        /// </summary>
        [Parameter]
        public string Title { get; set; } = "Drag and drop files here";

        /// <summary>
        /// Gets or sets the subtitle text displayed in the dropzone.
        /// </summary>
        [Parameter]
        public string Subtitle { get; set; } = "or browse to choose a file";

        /// <summary>
        /// Gets or sets the label text displayed on the action button.
        /// </summary>
        [Parameter]
        public string ButtonText { get; set; } = "Choose a file";

        /// <summary>
        /// Gets or sets the name of the Lucide icon displayed in the dropzone badge.
        /// </summary>
        [Parameter]
        public string IconName { get; set; } = "upload";

        /// <summary>
        /// Gets or sets the size in pixels of the Lucide icon.
        /// </summary>
        [Parameter]
        public int IconSize { get; set; } = 16;

        /// <summary>
        /// Gets or sets the file extensions or MIME types accepted by the input.
        /// </summary>
        [Parameter]
        public string Accept { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the upload component is disabled.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of files allowed when multiple file selection is enabled.
        /// </summary>
        [Parameter]
        public int MaxAllowedFiles { get; set; } = 1;

        /// <summary>
        /// Gets or sets optional custom CSS classes applied to the outer dropzone container.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional HTML attributes applied to the underlying input file element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> AdditionalAttributes { get; set; } = new();

        /// <summary>
        /// Gets or sets the callback invoked when the file input change event is triggered.
        /// </summary>
        [Parameter]
        public EventCallback<InputFileChangeEventArgs> OnFilesSubmitted { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked with the list of selected browser files.
        /// </summary>
        [Parameter]
        public EventCallback<IReadOnlyList<IBrowserFile>> OnSelectedFilesChanged { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowed file size in megabytes.
        /// </summary>
        [Parameter]
        public long MaxFileSizeInMb { get; set; } = DefaultMaxFileSizeInMb;

        /// <summary>
        /// Gets the maximum allowed file size in bytes, computed from the configured megabyte limit.
        /// </summary>
        private long MaxFileSize => this.MaxFileSizeInMb * 1024 * 1024;

        /// <summary>
        /// Gets the computed CSS class string for the outer dropzone container.
        /// </summary>
        /// <returns>The CSS class string.</returns>
        public string GetContainerClass()
        {
            const string baseClass = "relative w-full max-w-[650px] min-h-[205px] py-8.5 px-4 gap-2.5 bg-subtle border border-dashed rounded-10 flex flex-col justify-center items-center self-stretch transition-colors";

            if (this.Disabled)
            {
                return $"{baseClass} border-input opacity-60 cursor-not-allowed {this.Class}";
            }

            if (this.isDraggingOver)
            {
                return $"{baseClass} border-primary bg-accent/30 {this.Class}";
            }

            if (!string.IsNullOrEmpty(this.errorMessage))
            {
                return $"{baseClass} border-destructive hover:border-destructive {this.Class}";
            }

            return $"{baseClass} border-input hover:border-primary {this.Class}";
        }

        /// <summary>
        /// Handles the file input change event when a file is selected or dropped.
        /// </summary>
        /// <param name="args">The input file change event arguments.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task HandleFileChange(InputFileChangeEventArgs args)
        {
            if (this.Disabled)
            {
                return;
            }

            var rawFiles = this.Multiple ? args.GetMultipleFiles(this.MaxAllowedFiles) : [args.File];
            var validFiles = new List<IBrowserFile>();
            var hasOversizedFile = false;

            foreach (var file in rawFiles)
            {
                if (file.Size > this.MaxFileSize)
                {
                    hasOversizedFile = true;
                    continue;
                }

                validFiles.Add(file);
            }

            this.errorMessage = hasOversizedFile ? $"File exceeds the maximum size of {this.MaxFileSizeInMb} MB." : string.Empty;
            this.selectedFiles = validFiles;

            if (validFiles.Count > 0)
            {
                await this.OnFilesSubmitted.InvokeAsync(args);
            }

            await this.OnSelectedFilesChanged.InvokeAsync(this.selectedFiles);
        }

        /// <summary>
        /// Handles the drag enter event on the dropzone.
        /// </summary>
        private void HandleDragEnter()
        {
            if (this.Disabled)
            {
                return;
            }

            this.isDraggingOver = true;
            this.errorMessage = string.Empty;
        }

        /// <summary>
        /// Handles the drag leave event on the dropzone.
        /// </summary>
        private void HandleDragLeave()
        {
            this.isDraggingOver = false;
        }

        /// <summary>
        /// Handles the drop event on the dropzone.
        /// </summary>
        private void HandleDrop()
        {
            this.isDraggingOver = false;
        }
    }
}
