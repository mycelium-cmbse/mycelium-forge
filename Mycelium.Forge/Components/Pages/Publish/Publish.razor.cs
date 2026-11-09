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

    using Mycelium.Forge.Components.Pages.Publish.Dialogs;
    using Mycelium.Forge.Enums;
    using Mycelium.Forge.Extractors.Enums;
    using Mycelium.Forge.Models.Publish;
    using Mycelium.Forge.Services;
    using Mycelium.Forge.ViewModels.Publish;

    /// <summary>
    /// Represents the package publishing wizard page of the Mycelium Forge registry.
    /// </summary>
    public partial class Publish : DisposableComponent
    {
        /// <summary>
        /// The currently staged artefact UI model.
        /// </summary>
        private PublishArtefactModel stagedArtefact;

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
        /// Gets or sets the optional scope segment when publishing a new version of an existing package.
        /// </summary>
        [Parameter]
        public string Scope { get; set; }

        /// <summary>
        /// Gets or sets the optional package short name segment when publishing a new version of an existing package.
        /// </summary>
        [Parameter]
        public string Package { get; set; }

        /// <summary>
        /// Gets the HTML ID for the file input element.
        /// </summary>
        public static string UploadInputId => "publish-package-file-input";

        /// <summary>
        /// Gets or sets the zero-based active wizard step index.
        /// </summary>
        public int ActiveStep { get; set; }

        /// <summary>
        /// Gets a value indicating whether an artefact has been staged in the current wizard session.
        /// </summary>
        private bool HasStagedArtefact => this.stagedArtefact != null;

        /// <summary>
        /// Gets a value indicating whether the user is unauthorized to release a new version of the specified existing package.
        /// </summary>
        public bool IsUnauthorizedExistingPackage => this.ViewModel.IsExistingPackage &&
                                                     (this.ViewModel.Package == null || !this.ViewModel.CanUserReleaseExistingPackageVersion);

        /// <summary>
        /// Handles returning to the preceding wizard step.
        /// </summary>
        public void OnBack()
        {
            // If the user is on the validation step and the staged artefact has an invalid manifest, return to the upload step instead of the metadata step.
            if (this.ActiveStep == 2 && this.stagedArtefact is { IsManifestValid: false })
            {
                this.ActiveStep = 0;
                return;
            }

            if (this.ActiveStep > 0)
            {
                this.ActiveStep--;
            }
        }

        /// <summary>
        /// Handles advancing to the next wizard step.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnNext()
        {
            if (this.ActiveStep == 1)
            {
                await this.ViewModel.Validate();
            }

            this.ActiveStep++;
        }

        /// <summary>
        /// Handles completing the upload step and advancing to metadata or validation.
        /// </summary>
        /// <param name="artefact">The staged artefact UI model.</param>
        public void OnUploadStepContinued(PublishArtefactModel artefact)
        {
            this.stagedArtefact = artefact;

            // If the uploaded artefact has an invalid manifest, skip the metadata step and go directly to validation.
            this.ActiveStep = !artefact.IsManifestValid ? 2 : 1;
        }

        /// <summary>
        /// Handles replacing the currently loaded package archive artefact.
        /// </summary>
        public void OnReplaceArtefact()
        {
            this.ViewModel.DiscardStagedFile();
            this.stagedArtefact = null;
            this.uploadStepKey = Guid.NewGuid();
            this.ActiveStep = 0;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        /// unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.ViewModel.DiscardStagedFile();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Executes the publish submission workflow.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task PublishPackage()
        {
            var result = await this.ViewModel.Publish();

            if (result.IsError)
            {
                this.NotificationService.AddNotification(result.FirstError.Description, "Error", NotificationType.Error);
                return;
            }

            var parameters = new Dictionary<string, object>
            {
                { nameof(PublishedToForgeDialog.Scope), this.ViewModel.SelectedScope.ShortName },
                { nameof(PublishedToForgeDialog.PackageShortname), this.ViewModel.Package.ShortName },
                { nameof(PublishedToForgeDialog.Version), this.ViewModel.PackageVersion.Version }
            };

            var options = new DialogOpenOptions
            {
                ShowClose = false,
                Size = DialogSize.Small,
                PreventClose = true
            };

            await this.DialogService.OpenAsync<PublishedToForgeDialog>(parameters, options);
        }

        /// <summary>
        /// Handles navigating directly to a specific wizard step.
        /// </summary>
        /// <param name="step">The zero-based wizard step index.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnNavigateToStep(int step)
        {
            if (step == 2)
            {
                await this.ViewModel.Validate();
            }

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
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (!string.IsNullOrWhiteSpace(this.Scope) && !string.IsNullOrWhiteSpace(this.Package))
            {
                await this.ViewModel.InitializeViewModel(this.Scope, this.Package);
                return;
            }

            await this.ViewModel.InitializeViewModel();
        }
    }
}
