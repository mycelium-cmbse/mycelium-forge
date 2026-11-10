// ------------------------------------------------------------------------------------------------
// <copyright file="PublishValidationStep.razor.cs" company="Starion Group S.A.">
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
    using Mycelium.Forge.ViewModels.Publish;

    /// <summary>
    /// Represents the third step (pre-publish validation) component of the package publishing wizard.
    /// </summary>
    public partial class PublishValidationStep : ComponentBase
    {
        /// <summary>
        /// The singular warning text literal used for quantity formatting.
        /// </summary>
        private const string WarningLiteral = "warning";

        /// <summary>
        /// The guidance information card configuration for the validation step.
        /// </summary>
        private static readonly InformationCardModel StepInformationCard = new()
        {
            Title = "HOW VALIDATION WORKS",
            Items =
            [
                "Blocking checks stop the release. Warnings never do.",
                "Nothing is published at this step. You confirm on the last one.",
                "The same checks run when you publish with forge publish."
            ],
            HelpText = "Checks follow SemVer 2.0.0 and the kpar specification.",
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
        /// Gets or sets the callback invoked when returning to the package metadata step.
        /// </summary>
        [Parameter]
        public EventCallback OnBack { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when proceeding to the review and publish step.
        /// </summary>
        [Parameter]
        public EventCallback OnContinue { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when replacing the uploaded artefact.
        /// </summary>
        [Parameter]
        public EventCallback OnReplaceArtefact { get; set; }

        /// <summary>
        /// Gets a value indicating whether any validation checks failed with a blocking error.
        /// </summary>
        public bool HasFailures => this.ViewModel.ValidationChecks.Any(x => x.Status == ValidationStatus.Fail);

        /// <summary>
        /// Gets the count of non-blocking warning check outcomes.
        /// </summary>
        public int WarningCount => this.ViewModel.ValidationChecks.Count(x => x.Status is ValidationStatus.Warning or ValidationStatus.Missing);

        /// <summary>
        /// Resolves the CSS class string for a validation check row based on its status.
        /// </summary>
        /// <param name="status">The validation check status outcome.</param>
        /// <returns>The CSS class string for the row container.</returns>
        public static string GetCheckRowClass(ValidationStatus status)
        {
            return status == ValidationStatus.Fail
                ? "w-full flex items-start gap-3 p-3 my-1 rounded-md bg-destructive/10 border-t border-border"
                : "w-full flex items-start gap-3 py-3.5 border-t border-border";
        }

        /// <summary>
        /// Handles re-running pre-publish validation checks.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task HandleReRun()
        {
            await this.ViewModel.Validate();
        }

        /// <summary>
        /// Handles advancing to the review step after verifying that all blocking validation checks have passed.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task HandleContinue()
        {
            if (this.HasFailures)
            {
                return;
            }

            await this.OnContinue.InvokeAsync();
        }

        /// <summary>
        /// Gets the back button display text depending on whether the manifest is valid.
        /// </summary>
        /// <returns>The button text.</returns>
        public string GetBackText()
        {
            return this.ViewModel.StagedArtefact is { IsManifestValid: false } ? "Back to upload" : "Back to metadata";
        }

        /// <summary>
        /// Resolves the explanatory footnote text displayed in the validation step footer.
        /// </summary>
        /// <returns>A string containing the footnote description.</returns>
        public string GetFootnoteText()
        {
            if (this.HasFailures)
            {
                return "Resolve all blocking errors before proceeding.";
            }

            if (this.WarningCount > 0)
            {
                return $"{this.GetWarningsTitle()} do not block publishing.";
            }

            return "All checks passed. Ready to proceed.";
        }

        /// <summary>
        /// Gets the formatted summary title describing the number of validation warnings.
        /// </summary>
        /// <returns>A formatted string indicating the warning count.</returns>
        public string GetWarningsTitle()
        {
            return WarningLiteral.ToQuantity(this.WarningCount);
        }

        /// <summary>
        /// Gets the summary count text for passed, warning, and failed checks.
        /// </summary>
        /// <returns>A formatted string of evaluation outcome totals.</returns>
        public string GetValidationCountsText()
        {
            var passedCount = this.ViewModel.ValidationChecks.Count(x => x.Status == ValidationStatus.Pass);
            var failCount = this.ViewModel.ValidationChecks.Count(x => x.Status == ValidationStatus.Fail);

            return failCount > 0
                ? $"{"error".ToQuantity(failCount)} \u2022 {WarningLiteral.ToQuantity(this.WarningCount)}"
                : $"{passedCount} passed \u2022 {WarningLiteral.ToQuantity(this.WarningCount)}";
        }

        /// <summary>
        /// Gets the descriptive subtitle text for the validation overview card.
        /// </summary>
        /// <returns>The explanatory validation summary message.</returns>
        public string GetValidationSubText()
        {
            if (this.HasFailures)
            {
                return "Package cannot be published until all blocking errors are resolved.";
            }

            return this.WarningCount == 0
                ? "All pre-publish checks completed successfully."
                : $"{WarningLiteral.ToQuantity(this.WarningCount)} - not block publishing, but review them below.";
        }
    }
}
