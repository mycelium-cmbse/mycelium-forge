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
        /// <param name="detail">The detailed execution summary or diagnostic message.</param>
        /// <param name="status">The evaluation status outcome value.</param>
        public ValidationCheckModel(
            string title,
            string detail = "",
            ValidationStatus status = ValidationStatus.Pass)
        {
            this.Title = title;
            this.Detail = detail;
            this.Status = status;
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
        /// Gets a value indicating whether the validation check passed successfully.
        /// </summary>
        public bool IsPassed => this.Status == ValidationStatus.Pass;
    }
}
