// ------------------------------------------------------------------------------------------------
// <copyright file="IPackagesViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels
{
    using System.Collections.Generic;

    using Mycelium.Forge.Models;

    /// <summary>
    /// Defines the view model contract for the Mycelium Forge package discovery and filtering page.
    /// </summary>
    public interface IPackagesViewModel
    {
        /// <summary>
        /// Gets or sets the collection of format facet filter options.
        /// </summary>
        IReadOnlyList<FacetItemModel> Formats { get; set; }

        /// <summary>
        /// Gets or sets the collection of kind facet filter options.
        /// </summary>
        IReadOnlyList<FacetItemModel> Kinds { get; set; }

        /// <summary>
        /// Gets or sets the collection of scope and publisher facet filter options.
        /// </summary>
        IReadOnlyList<FacetItemModel> Scopes { get; set; }

        /// <summary>
        /// Gets or sets the collection of category facet filter options.
        /// </summary>
        IReadOnlyList<FacetItemModel> Categories { get; set; }

        /// <summary>
        /// Gets or sets the collection of tag facet filter options.
        /// </summary>
        IReadOnlyList<FacetItemModel> Tags { get; set; }

        /// <summary>
        /// Gets or sets the collection of metamodel facet filter options.
        /// </summary>
        IReadOnlyList<FacetItemModel> Metamodels { get; set; }

        /// <summary>
        /// Gets or sets the collection of license facet filter options.
        /// </summary>
        IReadOnlyList<FacetItemModel> Licenses { get; set; }

        /// <summary>
        /// Gets or sets the collection of displayed package search result items.
        /// </summary>
        IReadOnlyList<PackageRowModel> PackageResults { get; set; }

        /// <summary>
        /// Initializes the view model state and executes the initial package search.
        /// </summary>
        /// <param name="query">The search query parameter from URL route.</param>
        /// <param name="sort">The sort order parameter from URL route.</param>
        /// <param name="format">The format filter parameter from URL route.</param>
        /// <param name="category">The category filter parameter from URL route.</param>
        void InitializeViewModel(string query, string sort, string format, string category);

        /// <summary>
        /// Executes a search operation and refreshes facet options and package results.
        /// </summary>
        void Search();
    }
}
