// ------------------------------------------------------------------------------------------------
// <copyright file="PackageModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents a package card item displayed in the catalog sections and package lists.
    /// </summary>
    /// <param name="Name">The package name.</param>
    /// <param name="Href">The relative URL to the package page.</param>
    /// <param name="Description">The package description.</param>
    /// <param name="Format">The format name (e.g., SysML v2, CDP4-COMET, Capella).</param>
    /// <param name="Publisher">The publisher namespace or author.</param>
    /// <param name="Version">The package release version.</param>
    /// <param name="Tags">The tags string.</param>
    /// <param name="ImportCount">The number of imports.</param>
    /// <param name="IsVerified">Whether the publisher is verified.</param>
    /// <param name="Visibility">The visibility of the package (e.g., Public, Private, Organization).</param>
    /// <param name="LastPublished">The relative elapsed time since the last publish.</param>
    /// <param name="Role">The user's role for this package (e.g., Owner, Maintainer).</param>
    public record PackageModel(
        string Name,
        string Href,
        string Description,
        string Format,
        string Publisher,
        string Version,
        string Tags,
        string ImportCount,
        bool IsVerified = false,
        string Visibility = "Public",
        string LastPublished = "",
        string Role = "Owner");
}
