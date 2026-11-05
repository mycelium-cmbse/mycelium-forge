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
        /// A unique key to force re-mounting and clearing of the upload step component upon artefact replacement.
        /// </summary>
        private Guid uploadStepKey = Guid.NewGuid();

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
        /// Handles returning to the preceding wizard step.
        /// </summary>
        public void OnBack()
        {
            if (this.ActiveStep > 0)
            {
                this.ActiveStep--;
            }
        }

        /// <summary>
        /// Handles advancing to the next wizard step.
        /// </summary>
        public void OnNext()
        {
            this.ActiveStep++;
        }

        /// <summary>
        /// Handles replacing the currently loaded package archive artefact.
        /// </summary>
        /// <remarks>
        /// Reassigning <see cref="uploadStepKey" /> to a new <see cref="Guid" /> forces Blazor to re-render
        /// and reconstruct the upload step component, resetting the native HTML file input DOM element so that
        /// previously selected files are discarded.
        /// </remarks>
        public void OnReplaceArtefact()
        {
            this.uploadedPackage = null;
            this.uploadStepKey = Guid.NewGuid();
            this.ViewModel.InitializeViewModel();
            this.ActiveStep = 0;
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
            this.OnNext();
        }

        /// <summary>
        /// Handles navigating directly to a specific wizard step.
        /// </summary>
        /// <param name="step">The zero-based wizard step index.</param>
        public void OnNavigateToStep(int step)
        {
            this.ActiveStep = step;
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
    }
}