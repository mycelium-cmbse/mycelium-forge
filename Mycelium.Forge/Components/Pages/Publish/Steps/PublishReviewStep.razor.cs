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
    using Microsoft.AspNetCore.Components.Forms;

    using Mycelium.Forge.Enums;
    using Mycelium.Forge.Models.Publish;
    using Mycelium.Forge.Models.Validation;
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
                "It is public. Anyone can find and download it.",
                "You can unlist it later. Only the Installation Administrator can erase it."
            ],
            HelpText = "The publish is recorded in the package audit log.",
            LinkText = "Read the publishing guide \u2192",
            LinkHref = "/docs/publishing"
        };

        /// <summary>
        /// Gets or sets the view model for the package publishing page.
        /// </summary>
        [Parameter]
        [EditorRequired]
        public IPublishViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the uploaded package archive file for sizing and metadata display.
        /// </summary>
        [Parameter]
        [EditorRequired]
        public IBrowserFile UploadedFile { get; set; }

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
        /// Gets a value indicating whether the publish action is disabled.
        /// </summary>
        public bool IsPublishDisabled => !this.IsAcknowledged || this.ViewModel.IsPublishing;

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

                return failures.Count == 1
                    ? $"Failed with 1 error ({failureNames})"
                    : $"Failed with {failures.Count} errors ({failureNames})";
            }

            List<ValidationCheckModel> warnings = [.. this.ViewModel.ValidationChecks.Where(x => x.Status != ValidationStatus.Pass && x.Status != ValidationStatus.Fail)];

            if (warnings.Count > 0)
            {
                var warningNames = string.Join(", ", warnings.Select(w => w.Title));

                return warnings.Count == 1
                    ? $"Passed with 1 warning ({warningNames})"
                    : $"Passed with {warnings.Count} warnings ({warningNames})";
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
