// ------------------------------------------------------------------------------------------------
// <copyright file="FacetGroupModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a group of related facet filter options.
    /// </summary>
    /// <param name="Title">The category or attribute title of the facet group.</param>
    /// <param name="Items">The collection of facet options within this group.</param>
    public record FacetGroupModel(
        string Title,
        IReadOnlyList<FacetItemModel> Items);
}
