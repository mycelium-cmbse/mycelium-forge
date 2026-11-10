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
    using ErrorOr;

    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Enums;
    using Mycelium.Forge.Models.Publish;
    using Mycelium.Forge.Services;
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
        /// Backing field indicating whether an upload operation is currently in progress.
        /// </summary>
        private bool isUploading;

        /// <summary>
        /// The maximum allowed upload file size in megabytes.
        /// </summary>
        private long maxAllowedFileSizeInMb;

        /// <summary>
        /// The currently selected browser file.
        /// </summary>
        private IBrowserFile selectedFile;

        /// <summary>
        /// The resolved or selected format of the uploaded archive.
        /// </summary>
        private SupportedPackageFormat selectedFormat;

        /// <summary>
        /// Gets or sets the application configuration.
        /// </summary>
        [Inject]
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the JavaScript interop service.
        /// </summary>
        [Inject]
        public IJsInterop JsInterop { get; set; }

        /// <summary>
        /// Gets or sets the notification service.
        /// </summary>
        [Inject]
        public INotificationService NotificationService { get; set; }

        /// <summary>
        /// Gets or sets the view model for the package publishing page.
        /// </summary>
        [Parameter]
        [EditorRequired]
        public IPublishViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when the user proceeds to the next step.
        /// </summary>
        [Parameter]
        public EventCallback OnContinue { get; set; }

        /// <summary>
        /// Gets or sets the HTML ID applied to the dropzone file input element.
        /// </summary>
        [Parameter]
        public string InputId { get; set; } = "publish-package-file-input";

        /// <summary>
        /// Gets the validation manager handling field validation states for the upload form.
        /// </summary>
        public ValidationManager ValidationManager { get; } = new();

        /// <summary>
        /// Gets the comma-separated list of accepted file extensions.
        /// </summary>
        private string AcceptedExtensions => this.ViewModel.ExpectedFormat != null
            ? this.ViewModel.ExpectedFormat.Extension
            : string.Join(",", SupportedPackageFormat.All.Select(x => x.Extension).Distinct());

        /// <summary>
        /// Gets the list of accepted package formats.
        /// </summary>
        private IReadOnlyList<SupportedPackageFormat> AcceptedFormats => this.ViewModel.ExpectedFormat != null
            ? [this.ViewModel.ExpectedFormat]
            : SupportedPackageFormat.All;

        /// <summary>
        /// Gets a value indicating whether a file is currently selected.
        /// </summary>
        private bool HasSelectedFile => this.selectedFile != null;

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
        /// Determines whether the selected file has multiple matching model types requiring explicit user selection.
        /// </summary>
        /// <returns><c>true</c> if multiple model types share the file extension; otherwise, <c>false</c>.</returns>
        private bool RequiresModelTypeSelection()
        {
            return this.ViewModel.ExpectedFormat == null && this.GetMatchingFormats().Count > 1;
        }

        /// <summary>
        /// Gets the list of supported formats that match the extension of the selected file.
        /// </summary>
        /// <returns>A collection of matching <see cref="SupportedPackageFormat" /> instances.</returns>
        private IReadOnlyList<SupportedPackageFormat> GetMatchingFormats()
        {
            if (this.selectedFile == null)
            {
                return [];
            }

            var extension = Path.GetExtension(this.selectedFile.Name);
            return [.. SupportedPackageFormat.All.Where(x => x.Extension.Equals(extension, StringComparison.OrdinalIgnoreCase))];
        }

        /// <summary>
        /// Gets the summary description text for the selected file model type outcome.
        /// </summary>
        /// <returns>The summary description text.</returns>
        private string GetSelectedModelTypeSummary()
        {
            if (this.selectedFormat == null)
            {
                return this.GetMatchingFormats().Count > 0
                    ? "Model type selection required"
                    : "Unsupported format";
            }

            return this.RequiresModelTypeSelection()
                ? $"Selected {this.selectedFormat.Name}"
                : $"Detected {this.selectedFormat.Name}";
        }

        /// <summary>
        /// Gets the detected or selected model type name of the currently selected file.
        /// </summary>
        /// <returns>The human-readable model type name or file extension.</returns>
        private string GetSelectedModelType()
        {
            return this.selectedFormat?.Name ?? Path.GetExtension(this.selectedFile.Name);
        }

        /// <summary>
        /// Handles the selected files change event from the dropzone upload component.
        /// </summary>
        /// <param name="files">The list of selected files.</param>
        private void HandleFilesSelected(IReadOnlyList<IBrowserFile> files)
        {
            this.selectedFile = files is { Count: > 0 } ? files[0] : null;
            var matches = this.GetMatchingFormats();
            this.selectedFormat = this.ViewModel.ExpectedFormat ?? (matches.Count == 1 ? matches[0] : null);
            this.ValidationManager.ClearError(nameof(this.selectedFile));
            this.ValidationManager.ClearError(nameof(this.selectedFormat));
        }

        /// <summary>
        /// Handles the format change event from the model type selector toggle group.
        /// </summary>
        /// <param name="format">The selected format.</param>
        private void HandleFormatChanged(SupportedPackageFormat format)
        {
            this.selectedFormat = format;
            this.ValidationManager.ClearError(nameof(this.selectedFormat));
        }

        /// <summary>
        /// Validates the currently selected file and format.
        /// </summary>
        /// <returns>A <see cref="Result.Success" /> if valid; otherwise, a validation <see cref="Error" /> with description.</returns>
        private ErrorOr<Success> ValidateSelectedFile()
        {
            if (!this.HasSelectedFile)
            {
                return Error.Validation(description: "Please select an archive file to upload.");
            }

            if (this.ViewModel.ExpectedFormat != null && this.selectedFormat != this.ViewModel.ExpectedFormat)
            {
                return Error.Validation(description: $"Only {this.ViewModel.ExpectedFormat.Name} ({this.ViewModel.ExpectedFormat.Extension}) archives can be uploaded for this package.");
            }

            if (this.ViewModel.ExpectedFormat == null && this.GetMatchingFormats().Count == 0)
            {
                return Error.Validation(description: "The selected archive format is not supported.");
            }

            return Result.Success;
        }

        /// <summary>
        /// Handles the continue to metadata button click by staging the file over HTTP and extracting its manifest.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task HandleContinue()
        {
            if (this.isUploading)
            {
                return;
            }

            var fileValidation = this.ValidateSelectedFile();

            var isValid = this.ValidationManager
                .Check(nameof(this.selectedFile), !fileValidation.IsError, fileValidation.FirstError.Description)
                .Check(nameof(this.selectedFormat), this.selectedFormat != null, "Please select the model type for this archive.")
                .IsValid;

            if (!isValid)
            {
                return;
            }

            this.isUploading = true;

            try
            {
                var uploadToken = await this.JsInterop.UploadLargeFile(this.InputId, ApiRoutes.UploadFileForStage);

                var artefact = new PublishArtefactModel
                {
                    FileName = this.selectedFile.Name,
                    Format = this.selectedFormat!.Name,
                    SizeInBytes = this.selectedFile.Size,
                    Kind = this.selectedFormat.Kind,
                    StagingToken = uploadToken
                };

                await this.ViewModel.ExtractManifest(artefact);
                await this.OnContinue.InvokeAsync();
            }
            catch (Exception exception)
            {
                this.NotificationService.AddNotification(exception.Message, "Upload failed", NotificationType.Error);
            }
            finally
            {
                this.isUploading = false;
            }
        }
    }
}
