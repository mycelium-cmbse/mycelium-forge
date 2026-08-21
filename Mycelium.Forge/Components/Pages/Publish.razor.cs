// ------------------------------------------------------------------------------------------------
// <copyright file="Publish.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Common;
    using Mycelium.Forge.Models;
    using Mycelium.Forge.ViewModels;

    /// <summary>
    /// Represents the package publishing wizard page of the Mycelium Forge registry.
    /// </summary>
    public partial class Publish : ComponentBase
    {
        /// <summary>
        /// Gets or sets the view model for the package publishing page.
        /// </summary>
        [Inject]
        public IPublishViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the toast notification service.
        /// </summary>
        [Inject]
        public ToastService ToastService { get; set; }

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
        /// Gets the CSS classes for the step bubble circle indicator.
        /// </summary>
        /// <param name="step">The wizard step model.</param>
        /// <returns>The computed CSS class string for the step bubble.</returns>
        public string GetStepBubbleClass(PublishStepModel step)
        {
            const string baseClass = "w-5.5 h-5.5 rounded-full flex items-center justify-center shrink-0";

            return step.IsCurrent
                ? $"{baseClass} bg-primary"
                : $"{baseClass} bg-secondary";
        }

        /// <summary>
        /// Gets the CSS classes for the step number label within the bubble.
        /// </summary>
        /// <param name="step">The wizard step model.</param>
        /// <returns>The computed CSS class string for the step number.</returns>
        public string GetStepNumberClass(PublishStepModel step)
        {
            return step.IsCurrent
                ? "text-2xs leading-3xs font-semibold text-white"
                : "text-2xs leading-3xs font-semibold text-muted-foreground";
        }

        /// <summary>
        /// Gets the CSS classes for the step title text.
        /// </summary>
        /// <param name="step">The wizard step model.</param>
        /// <returns>The computed CSS class string for the step title.</returns>
        public string GetStepTitleClass(PublishStepModel step)
        {
            return step.IsCurrent
                ? "text-sm leading-xs font-semibold text-foreground"
                : "text-sm leading-xs font-medium text-muted-foreground";
        }

        /// <summary>
        /// Gets the CSS classes for the validation status outcome badge.
        /// </summary>
        /// <param name="status">The validation check status outcome.</param>
        /// <returns>The computed CSS class string for the status text.</returns>
        public string GetValidationStatusBadgeClass(ValidationStatus status)
        {
            return status switch
            {
                ValidationStatus.Pass => "text-xs leading-2xs font-medium text-validated shrink-0",
                ValidationStatus.Warning => "text-xs leading-2xs font-medium text-warning-foreground shrink-0",
                ValidationStatus.Missing => "text-xs leading-2xs font-medium text-warning-foreground shrink-0",
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
            this.ViewModel.InitializeViewModel();
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
            var result = this.ViewModel.Publish();

            if (result.IsSuccess)
            {
                var scope = this.ViewModel.Metadata.Scope;
                var packageName = this.ViewModel.Metadata.PackageName;
                var version = this.ViewModel.Metadata.Version;

                var parameters = new Dictionary<string, object>
                {
                    { nameof(PublishedToForgeDialog.Scope), scope },
                    { nameof(PublishedToForgeDialog.PackageName), packageName },
                    { nameof(PublishedToForgeDialog.Version), version }
                };

                var options = new DialogOpenOptions
                {
                    ShowClose = false,
                    Size = DialogSize.Small
                };

                await this.DialogService.OpenAsync<PublishedToForgeDialog>(parameters, options);
            }
            else
            {
                var errorMessage = result.Reasons.Count > 0 ? result.Reasons[0].Message : "Failed to publish package.";
                this.ToastService.Error(errorMessage, "Error");
            }
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
