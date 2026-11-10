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
    using ErrorOr;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Enums;

    /// <summary>
    /// Represents an evaluation check and its execution outcome.
    /// </summary>
    public class ValidationCheckModel
    {
        /// <summary>
        /// The metadata key used to store a specific remediation recommendation in an <see cref="Error" />.
        /// </summary>
        public const string RecommendationMetadataKey = "recommendation";

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
            this.Recommendation = ResolveRecommendation(title, null, recommendation);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationCheckModel" /> class from an <see cref="Error" />.
        /// </summary>
        /// <param name="error">The validation error.</param>
        /// <param name="recommendation">The optional suggested recommendation action to resolve a failed check.</param>
        public ValidationCheckModel(Error error, string recommendation = "")
        {
            this.Title = error.Code;
            this.Status = ValidationStatus.Fail;
            this.Detail = error.Description;
            this.Recommendation = ResolveRecommendation(error.Code, error.Metadata, recommendation);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationCheckModel" /> class from an <see cref="ErrorOr{Success}" />
        /// result.
        /// </summary>
        /// <param name="title">The title or description label of the check.</param>
        /// <param name="result">The validation evaluation result.</param>
        /// <param name="successDetail">The detailed execution summary when validation passes.</param>
        /// <param name="recommendation">The optional suggested recommendation action to resolve a failed check.</param>
        public ValidationCheckModel(string title, ErrorOr<Success> result, string successDetail = "", string recommendation = "")
        {
            this.Title = title;
            this.Status = result.IsError ? ValidationStatus.Fail : ValidationStatus.Pass;
            this.Detail = result.IsError ? result.FirstError.Description : successDetail;
            this.Recommendation = result.IsError ? ResolveRecommendation(title, result.FirstError.Metadata, recommendation) : string.Empty;
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
        /// Resolves the default remediation recommendation message prioritizing explicit recommendation, error metadata, then
        /// title mapping.
        /// </summary>
        /// <param name="title">The validation check title.</param>
        /// <param name="metadata">The optional error metadata dictionary.</param>
        /// <param name="recommendation">The optional explicit recommendation message.</param>
        /// <returns>A recommendation message string.</returns>
        private static string ResolveRecommendation(string title, Dictionary<string, object> metadata = null, string recommendation = "")
        {
            if (!string.IsNullOrWhiteSpace(recommendation))
            {
                return recommendation;
            }

            if (metadata != null && metadata.TryGetValue(RecommendationMetadataKey, out var metaRec) && metaRec is string recString)
            {
                return recString;
            }

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
