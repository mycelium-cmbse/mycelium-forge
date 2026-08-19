// ------------------------------------------------------------------------------------------------
// <copyright file="PackageElementModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents a model element definition contained within a package release.
    /// </summary>
    /// <param name="Name">The element identifier name.</param>
    /// <param name="Kind">The element definition kind tag (e.g., «part def», «port def», «attribute def»).</param>
    /// <param name="Category">
    /// The category kind grouping for filtering (e.g., Parts, Attributes, Units, Scales, Types,
    /// Templates).
    /// </param>
    /// <param name="AttributeSummary">The summary description of the element attributes or typing.</param>
    public record PackageElementModel(
        string Name,
        string Kind,
        string Category,
        string AttributeSummary);
}
