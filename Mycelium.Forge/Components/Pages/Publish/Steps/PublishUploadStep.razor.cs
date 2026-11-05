// ------------------------------------------------------------------------------------------------
// <copyright file="PublishUploadStep.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.Publish.Steps
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Models.Publish;
    using Mycelium.Forge.ViewModels.Publish;

    /// <summary>
    /// Represents the first step (upload) component of the package publishing wizard.
    /// </summary>
    public partial class PublishUploadStep : ComponentBase
    {
        /// <summary>
        /// The guidance information card configuration for the upload step.
        /// </summary>
        private static readonly InformationCardModel StepInformationCard = new()
        {
            Title = "BEFORE YOU UPLOAD",
            Items =
            [
                "A published version is immutable. A correction ships as a new version, never as a replacement.",
                "Everything inside the archive stays public for as long as the version is served. No secrets in the manifest.",
                "Only the Installation Administrator can erase a version. You can unlist one."
            ],
            HelpText = "Publishing from a pipeline? Use forge publish with a key scoped to publish only.",
            LinkText = "Read the publishing guide \u2192",
            LinkHref = "/docs/publishing"
        };

        /// <summary>
        /// The dictionary mapping human-readable model types to supported file extensions.
        /// </summary>
        private readonly Dictionary<string, string> acceptedFormats = new()
        {
            { "SysML v2 (kpar)", ".kpar" },
            { "ECSS-E-TM-10-25", ".zip" },
            { "Capella", ".zip" }
        };

        /// <summary>
        /// The maximum allowed upload file size in megabytes.
        /// </summary>
        private long maxAllowedFileSizeInMb;

        /// <summary>
        /// The currently selected browser file.
        /// </summary>
        private IBrowserFile selectedFile;

        /// <summary>
        /// The user-selected model type when an ambiguous archive extension with multiple models is uploaded.
        /// </summary>
        private string selectedAmbiguousModelType = string.Empty;

        /// <summary>
        /// Backing field indicating whether an upload operation is currently in progress.
        /// </summary>
        private bool isUploading;

        /// <summary>
        /// The maximum allowed upload file size in bytes.
        /// </summary>
        private long MaxAllowedFileSize => this.maxAllowedFileSizeInMb * 1024 * 1024;

        /// <summary>
        /// Gets or sets the application configuration.
        /// </summary>
        [Inject]
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the view model for the package publishing page.
        /// </summary>
        [Parameter]
        public IPublishViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets the comma-separated list of accepted file extensions.
        /// </summary>
        private string AcceptedExtensions => string.Join(",", this.acceptedFormats.Values.Distinct());

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
            this.maxAllowedFileSizeInMb = configuredSizeInMb ?? 250L;
        }

        /// <summary>
        /// Determines whether advancing to the next step is disabled.
        /// </summary>
        /// <returns><c>true</c> if continue is disabled; otherwise, <c>false</c>.</returns>
        private bool IsContinueDisabled()
        {
            return !this.HasSelectedFile ||
                   this.isUploading || 
                   (this.RequiresModelTypeSelection() && string.IsNullOrWhiteSpace(this.selectedAmbiguousModelType));
        }

        /// <summary>
        /// Determines whether the selected file has multiple matching model types requiring explicit user selection.
        /// </summary>
        /// <returns><c>true</c> if multiple model types share the file extension; otherwise, <c>false</c>.</returns>
        private bool RequiresModelTypeSelection()
        {
            return this.GetMatchingModelTypes().Count > 1;
        }

        /// <summary>
        /// Gets the list of supported model types that match the extension of the selected file.
        /// </summary>
        /// <returns>A collection of matching model type names.</returns>
        private IReadOnlyList<string> GetMatchingModelTypes()
        {
            if (this.selectedFile == null)
            {
                return [];
            }

            var extension = Path.GetExtension(this.selectedFile.Name);

            if (string.IsNullOrEmpty(extension))
            {
                return [];
            }

            return [.. this.acceptedFormats.Where(x => x.Value.Equals(extension, StringComparison.OrdinalIgnoreCase)).Select(x => x.Key)];
        }

        /// <summary>
        /// Gets the summary description text for the selected file model type outcome.
        /// </summary>
        /// <returns>The summary description text.</returns>
        private string GetSelectedModelTypeSummary()
        {
            var modelType = this.GetSelectedModelType();

            if (string.IsNullOrEmpty(modelType))
            {
                return "Model type selection required";
            }

            return this.RequiresModelTypeSelection()
                ? $"Selected {modelType}"
                : $"Detected {modelType}";
        }

        /// <summary>
        /// Gets the detected or selected model type of the currently selected file.
        /// </summary>
        /// <returns>The human-readable model type name or file extension.</returns>
        private string GetSelectedModelType()
        {
            if (this.selectedFile == null)
            {
                return string.Empty;
            }

            var matches = this.GetMatchingModelTypes();

            if (matches.Count > 1)
            {
                return this.selectedAmbiguousModelType;
            }

            if (matches.Count == 1)
            {
                return matches[0];
            }

            var extension = Path.GetExtension(this.selectedFile.Name);
            return !string.IsNullOrEmpty(extension) ? extension : "unknown";
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
                this.selectedAmbiguousModelType = string.Empty;
            }
        }

        /// <summary>
        /// Handles the format change event from the model type selector toggle group.
        /// </summary>
        /// <param name="modelType">The selected model type string.</param>
        private void HandleModelSelectionChanged(string modelType)
        {
            this.selectedAmbiguousModelType = modelType ?? string.Empty;
        }

        /// <summary>
        /// Handles the continue to metadata button click.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task HandleContinue()
        {
            this.isUploading = true;

            this.ViewModel.Metadata = new PublishPackageMetadataModel
            {
                ArtefactFileName = this.selectedFile.Name,
                ArtefactFormat = this.GetSelectedModelType()
            };

            await this.OnContinue.InvokeAsync(this.selectedFile);
            this.isUploading = false;
        }
    }
}