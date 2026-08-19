// ------------------------------------------------------------------------------------------------
// <copyright file="PackageDependentModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents a dependent package or project that imports or specializes this package.
    /// </summary>
    /// <param name="Name">The display name of the dependent package or project.</param>
    /// <param name="Href">The relative URL to the dependent package page or project.</param>
    /// <param name="RelationshipSummary">The description of the relationship and imported version.</param>
    /// <param name="IsProject">A value indicating whether the dependent is a project rather than a package.</param>
    /// <param name="IsVerified">A value indicating whether the dependent publisher is verified.</param>
    public record PackageDependentModel(
        string Name,
        string Href,
        string RelationshipSummary,
        bool IsProject = false,
        bool IsVerified = false);
}
