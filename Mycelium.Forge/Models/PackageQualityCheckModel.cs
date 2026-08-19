// ------------------------------------------------------------------------------------------------
// <copyright file="PackageQualityCheckModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents a quality evaluation check item for a package release.
    /// </summary>
    /// <param name="Label">The description label of the quality check.</param>
    /// <param name="IsPassed">A value indicating whether the quality check passed successfully.</param>
    public record PackageQualityCheckModel(
        string Label,
        bool IsPassed = true);
}
