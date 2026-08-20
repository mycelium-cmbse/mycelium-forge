// ------------------------------------------------------------------------------------------------
// <copyright file="ValidationCheckModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents an evaluation check and its execution outcome.
    /// </summary>
    /// <param name="Title">The title or description label of the check.</param>
    /// <param name="Detail">The detailed execution summary or diagnostic message.</param>
    /// <param name="Status">The evaluation status outcome value.</param>
    public record ValidationCheckModel(
        string Title,
        string Detail = "",
        ValidationStatus Status = ValidationStatus.Pass)
    {
        /// <summary>
        /// Gets a value indicating whether the validation check passed successfully.
        /// </summary>
        public bool IsPassed => this.Status == ValidationStatus.Pass;
    }
}
