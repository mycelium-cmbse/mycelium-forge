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
    /// <param name="Title">The validation summary header title.</param>
    /// <param name="Description">The detailed validation description message.</param>
    /// <param name="Score">The validation score ratio text (e.g., 5 / 5).</param>
    /// <param name="IsPassed">A value indicating whether the overall validation passed.</param>
    /// <param name="Checks">The list of detailed validation checks performed.</param>
    public record PackageValidationReportModel(
        string Title,
        string Description,
        string Score,
        bool IsPassed,
        IReadOnlyList<ValidationCheckModel> Checks);
}
