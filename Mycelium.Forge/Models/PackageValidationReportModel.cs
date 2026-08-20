// ------------------------------------------------------------------------------------------------
// <copyright file="PackageValidationReportModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents the comprehensive automated release validation report for a package.
    /// </summary>
    public class PackageValidationReportModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageValidationReportModel" /> class.
        /// </summary>
        public PackageValidationReportModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageValidationReportModel" /> class with specified properties.
        /// </summary>
        /// <param name="title">The validation summary header title.</param>
        /// <param name="description">The detailed validation description message.</param>
        /// <param name="score">The validation score ratio text (e.g., 5 / 5).</param>
        /// <param name="isPassed">A value indicating whether the overall validation passed.</param>
        /// <param name="checks">The list of detailed validation checks performed.</param>
        public PackageValidationReportModel(
            string title,
            string description,
            string score,
            bool isPassed,
            IReadOnlyList<ValidationCheckModel> checks)
        {
            this.Title = title;
            this.Description = description;
            this.Score = score;
            this.IsPassed = isPassed;
            this.Checks = checks ?? [];
        }

        /// <summary>
        /// Gets or sets the validation summary header title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the detailed validation description message.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the validation score ratio text (e.g., 5 / 5).
        /// </summary>
        public string Score { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the overall validation passed.
        /// </summary>
        public bool IsPassed { get; set; }

        /// <summary>
        /// Gets or sets the list of detailed validation checks performed.
        /// </summary>
        public IReadOnlyList<ValidationCheckModel> Checks { get; set; } = [];
    }
}
