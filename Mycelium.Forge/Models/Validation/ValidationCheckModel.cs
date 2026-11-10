// ------------------------------------------------------------------------------------------------
// <copyright file="ValidationCheckModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.Validation
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Enums;

    /// <summary>
    /// Represents an evaluation check and its execution outcome.
    /// </summary>
    public class ValidationCheckModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationCheckModel" /> class.
        /// </summary>
        public ValidationCheckModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationCheckModel" /> class with specified properties.
        /// </summary>
        /// <param name="title">The title or description label of the check.</param>
        /// <param name="status">The evaluation status outcome value.</param>
        /// <param name="detail">The detailed execution summary or diagnostic message.</param>
        /// <param name="recommendation">The optional suggested recommendation action to resolve a failed check.</param>
        public ValidationCheckModel(string title, ValidationStatus status, string detail = "", string recommendation = "")
        {
            this.Title = title;
            this.Detail = detail;
            this.Status = status;
            this.Recommendation = !string.IsNullOrWhiteSpace(recommendation) ? recommendation : ResolveRecommendation(title);
        }

        /// <summary>
        /// Gets or sets the title or description label of the check.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the detailed execution summary or diagnostic message.
        /// </summary>
        public string Detail { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the evaluation status outcome value.
        /// </summary>
        public ValidationStatus Status { get; set; } = ValidationStatus.Pass;

        /// <summary>
        /// Gets or sets the suggested recommendation action to resolve a failed check.
        /// </summary>
        public string Recommendation { get; set; } = string.Empty;

        /// <summary>
        /// Resolves the default remediation recommendation message for a given check title.
        /// </summary>
        /// <param name="title">The validation check title.</param>
        /// <returns>A recommendation message string.</returns>
        private static string ResolveRecommendation(string title)
        {
            return title switch
            {
                ValidationTypes.PackageVersion => "Must be greater than the latest published version.",
                ValidationTypes.NamespaceUniqueness => "Choose a unique package name in the selected namespace scope.",
                ValidationTypes.SchemaValidation => "Verify archive manifest syntax and conform to specification.",
                ValidationTypes.License => "Select a license identifier for the package.",
                _ => "Review the validation failure and resolve the underlying issue before publishing."
            };
        }
    }
}
