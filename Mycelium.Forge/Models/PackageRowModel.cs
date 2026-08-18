// ------------------------------------------------------------------------------------------------
// <copyright file="PackageRowModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents a package search result row displayed in the packages discovery view.
    /// </summary>
    /// <param name="Name">The package name.</param>
    /// <param name="Href">The relative URL to the package details page.</param>
    /// <param name="Description">The package summary description.</param>
    /// <param name="Format">The format name (e.g., SysML v2, CDP4-COMET, Capella).</param>
    /// <param name="Publisher">The publisher namespace or username.</param>
    /// <param name="Version">The package release version.</param>
    /// <param name="Tags">The tags associated with the package.</param>
    /// <param name="UpdatedAgo">The relative elapsed time since the last update.</param>
    /// <param name="ImportCount">The number of imports or downloads.</param>
    /// <param name="IsVerified">A value indicating whether the publisher is verified.</param>
    public record PackageRowModel(
        string Name,
        string Href,
        string Description,
        string Format,
        string Publisher,
        string Version,
        string Tags,
        string UpdatedAgo,
        string ImportCount,
        bool IsVerified = false);
}
