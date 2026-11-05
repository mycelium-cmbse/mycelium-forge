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
    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;

    using Mycelium.Forge.Enums;
    using Mycelium.Forge.Models.Publish;
    using Mycelium.Forge.Models.Validation;
    using Mycelium.Forge.ViewModels.Publish;

    /// <summary>
    /// Represents the third step (pre-publish validation) component of the package publishing wizard.
    /// </summary>
    public partial class PublishValidationStep : ComponentBase
    {
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
        /// Gets or sets the uploaded package archive file for metadata and sizing.
        /// </summary>
        [Parameter]
        [EditorRequired]
        public IBrowserFile UploadedFile { get; set; }

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
        public int WarningCount => this.ViewModel.ValidationChecks.Count(x => x.Status != ValidationStatus.Pass && x.Status != ValidationStatus.Fail);

        /// <summary>
        /// Resolves the badge visual variant for a validation status outcome.
        /// </summary>
        /// <param name="status">The validation check status outcome.</param>
        /// <returns>The corresponding <see cref="BadgeVariant" />.</returns>
        public static BadgeVariant GetValidationBadgeVariant(ValidationStatus status)
        {
            return status switch
            {
                ValidationStatus.Pass => BadgeVariant.SoftSuccess,
                ValidationStatus.Fail => BadgeVariant.SoftDestructive,
                ValidationStatus.Warning => BadgeVariant.SoftWarning,
                ValidationStatus.Missing => BadgeVariant.Secondary,
                _ => BadgeVariant.Outline
            };
        }

        /// <summary>
        /// Handles re-running pre-publish validation checks.
        /// </summary>
        public void HandleReRun()
        {
            // TODO: Replace with live package archive validation once backend validation service is implemented.
            this.ViewModel.InitializeViewModel();
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
            var count = this.ViewModel.ValidationChecks.Count(x => x.Status != ValidationStatus.Pass && x.Status != ValidationStatus.Fail);
            return count == 1 ? "1 warning" : $"{count} warnings";
        }

        /// <summary>
        /// Gets the summary count text for passed, warning, and failed checks.
        /// </summary>
        /// <returns>A formatted string of evaluation outcome totals.</returns>
        public string GetValidationCountsText()
        {
            var passedCount = this.ViewModel.ValidationChecks.Count(x => x.Status == ValidationStatus.Pass);
            var warningCount = this.ViewModel.ValidationChecks.Count(x => x.Status == ValidationStatus.Warning || x.Status == ValidationStatus.Missing);
            var failCount = this.ViewModel.ValidationChecks.Count(x => x.Status == ValidationStatus.Fail);

            if (failCount > 0)
            {
                return $"{failCount} {(failCount == 1 ? "error" : "errors")} \u2022 {warningCount} {(warningCount == 1 ? "warning" : "warnings")}";
            }

            return $"{passedCount} passed \u2022 {warningCount} {(warningCount == 1 ? "warning" : "warnings")}";
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

            var count = this.ViewModel.ValidationChecks.Count(x => x.Status != ValidationStatus.Pass);

            return count == 0
                ? "All pre-publish checks completed successfully."
                : $"{count} {(count == 1 ? "warning does" : "warnings do")} not block publishing, but review them below.";
        }

        /// <summary>
        /// Resolves the explanatory description for a validation rule check item.
        /// </summary>
        /// <param name="check">The validation check evaluation model.</param>
        /// <returns>The detailed description for the check.</returns>
        public string GetCheckDescription(ValidationCheckModel check)
        {
            if (!string.IsNullOrWhiteSpace(check.Detail))
            {
                return check.Detail;
            }

            return check.Title switch
            {
                "Schema validation" => "Archive structure and manifests conform to specification.",
                "Namespace uniqueness" => "Package identifier is available in the selected scope.",
                "Required metadata" => "All mandatory manifest fields are present and valid.",
                "Dependency resolution" => "Declared dependencies are resolvable from registry.",
                "License file" => "No LICENSE file found in archive root. Consider adding one.",
                "README.md" => "No README.md documentation file found in archive root.",
                _ => check.Title
            };
        }

        /// <summary>
        /// Resolves the corrective recommendation message for a failed validation check.
        /// </summary>
        /// <param name="check">The validation check evaluation model.</param>
        /// <returns>The suggested remediation action or string empty.</returns>
        public string GetCheckFixRecommendation(ValidationCheckModel check)
        {
            return check.Title switch
            {
                "Version" or "Package version" => "Must be greater than the latest published version.",
                "Namespace uniqueness" => "Choose a unique package name in the selected namespace scope.",
                "Schema validation" => "Verify archive manifest syntax and conform to specification.",
                _ => "Resolve the issue in the archive manifest and re-run validation."
            };
        }
    }
}