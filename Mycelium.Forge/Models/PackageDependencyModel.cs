// ------------------------------------------------------------------------------------------------
// <copyright file="PackageDependencyModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents a package dependency requirement resolved in the package dependency graph.
    /// </summary>
    /// <param name="Name">The full package name of the dependency.</param>
    /// <param name="Href">The relative URL to the dependency package page.</param>
    /// <param name="Requirement">The version range requirement and license summary.</param>
    /// <param name="IsVerified">A value indicating whether the dependency publisher is verified.</param>
    public record PackageDependencyModel(
        string Name,
        string Href,
        string Requirement,
        bool IsVerified = false);
}
