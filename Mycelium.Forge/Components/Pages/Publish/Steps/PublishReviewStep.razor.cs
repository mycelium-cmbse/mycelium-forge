// ------------------------------------------------------------------------------------------------
// <copyright file="PublishReviewStep.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.Publish.Steps
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Enums;
    using Mycelium.Forge.Extensions;
    using Mycelium.Forge.Models.Publish;
    using Mycelium.Forge.Models.Validation;
    using Mycelium.Forge.Services;
    using Mycelium.Forge.ViewModels.Publish;

    /// <summary>
    /// Represents the fourth step (review and publish) component of the package publishing wizard.
    /// </summary>
    public partial class PublishReviewStep : ComponentBase
    {
        /// <summary>
        /// The guidance information card configuration for the review step.
        /// </summary>
        private static readonly InformationCardModel StepInformationCard = new()
        {
            Title = "WHEN YOU PUBLISH",
            Items =
            [
                "A published version becomes immutable. A correction ships as a new version.",
                "Visibility follows the setting chosen for the package.",
                "You can unlist it later. Only the Installation Administrator can erase it."
            ],
            HelpText = "The publish is recorded in the package audit log.",
            LinkText = "Read the publishing guide \u2192",
            LinkHref = "/docs/publishing"
        };

        /// <summary>
        /// Gets the validation manager handling field validation states for the review form.
        /// </summary>
        public ValidationManager ValidationManager { get; } = new();

        /// <summary>
        /// Gets or sets the view model for the package publishing page.
        /// </summary>
        [Parameter]
        [EditorRequired]
        public IPublishViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when navigating back to the validation step.
        /// </summary>
        [Parameter]
        public EventCallback OnBack { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when initiating package publication.
        /// </summary>
        [Parameter]
        public EventCallback OnPublish { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when navigating directly to a wizard step by index.
        /// </summary>
        [Parameter]
        public EventCallback<int> OnNavigateToStep { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when replacing the uploaded artefact.
        /// </summary>
        [Parameter]
        public EventCallback OnReplaceArtefact { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has confirmed the registry compliance checkbox.
        /// </summary>
        public bool IsAcknowledged { get; set; }

        /// <summary>
        /// Handles changes to the registry compliance acknowledgment checkbox.
        /// </summary>
        /// <param name="isAcknowledged">The new checkbox state.</param>
        public void OnAcknowledgmentChanged(bool isAcknowledged)
        {
            this.IsAcknowledged = isAcknowledged;
            this.ValidationManager.ClearError(nameof(this.IsAcknowledged));
        }

        /// <summary>
        /// Handles initiating package publication after validating acknowledgment of registry terms.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task HandlePublish()
        {
            if (this.ViewModel.IsPublishing)
            {
                return;
            }

            var isValid = this.ValidationManager
                .Check(nameof(this.IsAcknowledged), this.IsAcknowledged, "Please confirm that the package complies with the registry terms.")
                .IsValid;

            if (!isValid)
            {
                return;
            }

            await this.OnPublish.InvokeAsync();
        }

        /// <summary>
        /// Gets the formatted `@scope/packageName` coordinate string for display.
        /// </summary>
        /// <returns>The formatted package coordinate string.</returns>
        public string GetScopePackageCoordinate()
        {
            return $"{this.ViewModel.SelectedScope.GetDisplayShortName()}/{this.ViewModel.Package.ShortName}";
        }

        /// <summary>
        /// Gets the formatted license text or "None" when unspecified.
        /// </summary>
        /// <returns>The license display string.</returns>
        public string GetLicenseText()
        {
            return string.IsNullOrWhiteSpace(this.ViewModel.Package.License) ? "None" : this.ViewModel.Package.License;
        }

        /// <summary>
        /// Gets the comma-separated tags display text or "None" when empty.
        /// </summary>
        /// <returns>The tags display string.</returns>
        public string GetTagsText()
        {
            return this.ViewModel.Package.Tags is { Count: > 0 } ? string.Join(", ", this.ViewModel.Package.Tags) : "None";
        }

        /// <summary>
        /// Gets the comma-separated dependencies display text or "None" when empty.
        /// </summary>
        /// <returns>The dependencies display string.</returns>
        public string GetDependenciesText()
        {
            if (this.ViewModel.PackageMetaData.Dependencies is not { Count: > 0 })
            {
                return "None";
            }

            return string.Join(", ", this.ViewModel.PackageMetaData.Dependencies.Select(d => string.IsNullOrWhiteSpace(d.Summary) ? d.Name : $"{d.Name} {d.Summary}"));
        }

        /// <summary>
        /// Gets the human-readable summary of the validation status outcome.
        /// </summary>
        /// <returns>A string describing the validation status outcome.</returns>
        public string GetValidationStatusText()
        {
            List<ValidationCheckModel> failures = [.. this.ViewModel.ValidationChecks.Where(x => x.Status == ValidationStatus.Fail)];

            if (failures.Count > 0)
            {
                var failureNames = string.Join(", ", failures.Select(f => f.Title));
                return $"Failed with {"error".ToQuantity(failures.Count)} ({failureNames})";
            }

            List<ValidationCheckModel> warnings = [.. this.ViewModel.ValidationChecks.Where(x => x.Status is ValidationStatus.Warning or ValidationStatus.Missing)];

            if (warnings.Count > 0)
            {
                var warningNames = string.Join(", ", warnings.Select(w => w.Title));
                return $"Passed with {"warning".ToQuantity(warnings.Count)} ({warningNames})";
            }

            return "Passed all validation checks";
        }

        /// <summary>
        /// Handles replacing the artefact by invoking the replace callback.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task HandleReplaceArtefact()
        {
            await this.OnReplaceArtefact.InvokeAsync();
        }

        /// <summary>
        /// Handles editing metadata by navigating to the package metadata step.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task HandleEditMetadata()
        {
            await this.OnNavigateToStep.InvokeAsync(1);
        }

        /// <summary>
        /// Handles viewing validation details by navigating to the validation step.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task HandleViewValidationDetails()
        {
            await this.OnNavigateToStep.InvokeAsync(2);
        }
    }
}
