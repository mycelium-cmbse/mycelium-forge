// ------------------------------------------------------------------------------------------------
// <copyright file="PackageRelationshipModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents a related package or project in the dependency or dependent graph.
    /// </summary>
    /// <param name="Name">The full package or project identifier name.</param>
    /// <param name="Href">The relative URL to the package details page or project.</param>
    /// <param name="Summary">The description of the relationship requirement or imported version.</param>
    /// <param name="IsProject">A value indicating whether the target is a project rather than a package.</param>
    /// <param name="IsVerified">A value indicating whether the target publisher is verified.</param>
    public record PackageRelationshipModel(
        string Name,
        string Href,
        string Summary,
        bool IsProject = false,
        bool IsVerified = false);
}
