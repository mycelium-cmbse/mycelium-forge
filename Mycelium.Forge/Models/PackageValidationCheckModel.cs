// ------------------------------------------------------------------------------------------------
// <copyright file="PackageValidationCheckModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents an individual automated validation check and its execution outcome.
    /// </summary>
    /// <param name="Title">The title of the validation check.</param>
    /// <param name="Detail">The detailed execution summary or diagnostic message.</param>
    /// <param name="StatusText">The status badge text (e.g., Pass, Fail).</param>
    /// <param name="IsPassed">A value indicating whether the validation check passed.</param>
    public record PackageValidationCheckModel(
        string Title,
        string Detail,
        string StatusText,
        bool IsPassed = true);
}
