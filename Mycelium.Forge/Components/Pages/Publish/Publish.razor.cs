// ------------------------------------------------------------------------------------------------
// <copyright file="Publish.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.Publish
{
    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.Publish.Dialogs;
    using Mycelium.Forge.Enums;
    using Mycelium.Forge.Services;
    using Mycelium.Forge.ViewModels.Publish;

    /// <summary>
    /// Represents the package publishing wizard page of the Mycelium Forge registry.
    /// </summary>
    public partial class Publish : ComponentBase
    {
        /// <summary>
        /// Gets or sets the uploaded package archive file for validation and publishing.
        /// </summary>
        private IBrowserFile uploadedPackage;

        /// <summary>
        /// Gets or sets the view model for the package publishing page.
        /// </summary>
        [Inject]
        public IPublishViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the notification service.
        /// </summary>
        [Inject]
        public INotificationService NotificationService { get; set; }

        /// <summary>
        /// Gets or sets the navigation manager instance.
        /// </summary>
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the dialog service used to display modal dialogs.
        /// </summary>
        [Inject]
        public DialogService DialogService { get; set; }

        /// <summary>
        /// Gets the HTML ID for the file input element.
        /// </summary>
        public static string UploadInputId => "publish-package-file-input";

        /// <summary>
        /// Gets or sets the zero-based active wizard step index.
        /// </summary>
        public int ActiveStep { get; set; }

        /// <summary>
        /// Gets the CSS classes for the validation status outcome badge.
        /// </summary>
        /// <param name="status">The validation check status outcome.</param>
        /// <returns>The computed CSS class string for the status text.</returns>
        public static string GetValidationStatusBadgeClass(ValidationStatus status)
        {
            return status switch
            {
                ValidationStatus.Pass => "text-xs leading-2xs font-medium text-success-vivid shrink-0",
                ValidationStatus.Warning => "text-xs leading-2xs font-medium text-warning-vivid shrink-0",
                ValidationStatus.Missing => "text-xs leading-2xs font-medium text-warning-vivid shrink-0",
                ValidationStatus.Fail => "text-xs leading-2xs font-medium text-destructive shrink-0",
                _ => "text-xs leading-2xs font-medium text-muted-foreground shrink-0"
            };
        }

        /// <summary>
        /// Gets the formatted summary title describing the number of validation warnings.
        /// </summary>
        /// <returns>A formatted string indicating the warning count.</returns>
        public string GetWarningsTitle()
        {
            var count = this.ViewModel.ValidationChecks.Count(x => x.Status != ValidationStatus.Pass);
            return count == 1 ? "1 warning" : $"{count} warnings";
        }

        /// <summary>
        /// Gets the action button label text for publishing the package release.
        /// </summary>
        /// <returns>The button display text with version.</returns>
        public string GetPublishButtonText()
        {
            return $"Validate & publish v{this.ViewModel.Metadata.Version}";
        }

        /// <summary>
        /// Handles replacing the currently loaded package archive artefact.
        /// </summary>
        public void OnReplaceArtefact()
        {
            this.uploadedPackage = null;
            this.ViewModel.InitializeViewModel();
            this.ActiveStep = 0;
        }

        /// <summary>
        /// Handles changes to the package scope selection.
        /// </summary>
        /// <param name="scope">The selected scope value.</param>
        public void OnScopeChanged(string scope)
        {
            this.ViewModel.Metadata.Scope = scope ?? string.Empty;
        }

        /// <summary>
        /// Handles changes to the package name input value.
        /// </summary>
        /// <param name="packageName">The new package name value.</param>
        public void OnPackageNameChanged(string packageName)
        {
            this.ViewModel.Metadata.PackageName = packageName ?? string.Empty;
        }

        /// <summary>
        /// Handles changes to the package version input value.
        /// </summary>
        /// <param name="version">The new package version value.</param>
        public void OnVersionChanged(string version)
        {
            this.ViewModel.Metadata.Version = version ?? string.Empty;
        }

        /// <summary>
        /// Handles changes to the package description input value.
        /// </summary>
        /// <param name="description">The new description value.</param>
        public void OnDescriptionChanged(string description)
        {
            this.ViewModel.Metadata.Description = description ?? string.Empty;
        }

        /// <summary>
        /// Handles changes to the package license selection.
        /// </summary>
        /// <param name="license">The selected license identifier.</param>
        public void OnLicenseChanged(string license)
        {
            this.ViewModel.Metadata.License = license ?? string.Empty;
        }

        /// <summary>
        /// Handles changes to the package visibility selection.
        /// </summary>
        /// <param name="visibility">The selected visibility setting.</param>
        public void OnVisibilityChanged(VisibilityKind visibility)
        {
            this.ViewModel.Metadata.Visibility = visibility;
        }

        /// <summary>
        /// Handles changes to the package metamodel selection.
        /// </summary>
        /// <param name="metamodel">The selected metamodel specification.</param>
        public void OnMetamodelChanged(string metamodel)
        {
            this.ViewModel.Metadata.Metamodel = metamodel ?? string.Empty;
        }

        /// <summary>
        /// Handles changes to the package tags input value.
        /// </summary>
        /// <param name="tags">The new tags value.</param>
        public void OnTagsChanged(string tags)
        {
            this.ViewModel.Metadata.Tags = tags ?? string.Empty;
        }

        /// <summary>
        /// Executes the validation and publish submission workflow.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task PublishPackage()
        {
            // Issue #180: Implement package publishing workflow.

            var result = this.ViewModel.Publish();

            if (result.IsError)
            {
                this.NotificationService.AddNotification(result.FirstError.Description, "Error", NotificationType.Error);
                return;
            }

            var scope = this.ViewModel.Metadata.Scope;
            var packageName = this.ViewModel.Metadata.PackageName;
            var version = this.ViewModel.Metadata.Version;

            var parameters = new Dictionary<string, object>
            {
                { nameof(PublishedToForgeDialog.Scope), scope },
                { nameof(PublishedToForgeDialog.PackageShortname), packageName },
                { nameof(PublishedToForgeDialog.Version), version }
            };

            var options = new DialogOpenOptions
            {
                ShowClose = false,
                Size = DialogSize.Small
            };

            await this.DialogService.OpenAsync<PublishedToForgeDialog>(parameters, options);
        }

        /// <summary>
        /// Handles completing the upload step and advancing to package metadata.
        /// </summary>
        /// <param name="file">The uploaded file.</param>
        public void OnUploadStepContinued(IBrowserFile file)
        {
            this.uploadedPackage = file;
            this.ViewModel.Metadata.ArtefactFileName = this.uploadedPackage.Name;
            this.ActiveStep = 1;
        }

        /// <summary>
        /// Computes the CSS class string for displaying or hiding a step container.
        /// </summary>
        /// <param name="stepIndex">The zero-based index of the step.</param>
        /// <param name="activeClasses">The CSS classes applied when the step is active.</param>
        /// <returns>The CSS class string.</returns>
        public string GetStepClass(int stepIndex, string activeClasses = "w-full")
        {
            return this.ActiveStep == stepIndex ? activeClasses : "hidden";
        }

        /// <summary>
        /// Initializes the component lifecycle and populates the view model state.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.ViewModel.InitializeViewModel();
        }

        /// <summary>
        /// Determines whether the specified step can be opened based on the current state.
        /// </summary>
        /// <param name="stepNumber">The step number.</param>
        /// <returns><c>true</c> if the step can be opened; otherwise, <c>false</c>.</returns>
        private bool CanOpenStep(int stepNumber)
        {
            // Step 0 (Upload) can always be opened; Step 1 (Metadata) can only be opened if a package has been uploaded. Other steps are still to be implemented
            return stepNumber switch
            {
                0 => true,
                1 => this.CanOpenStep(0) && this.uploadedPackage != null,
                _ => false
            };
        }
    }
}
