// ------------------------------------------------------------------------------------------------
// <copyright file="IPackagesViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.Packages
{
    using Mycelium.Forge.Enums;
    using Mycelium.Forge.Models.Package;

    /// <summary>
    /// Defines the view model contract for the Mycelium Forge package discovery and filtering page.
    /// </summary>
    public interface IPackagesViewModel
    {
        /// <summary>
        /// Gets or sets the collection of facet filter options.
        /// </summary>
        List<OptionModel> Facets { get; set; }

        /// <summary>
        /// Gets or sets the collection of displayed package search result items.
        /// </summary>
        List<PackageModel> PackageResults { get; set; }

        /// <summary>
        /// Initializes the view model state and executes the initial package search.
        /// </summary>
        /// <param name="query">The search query parameter from URL route.</param>
        /// <param name="sort">The sort order parameter from URL route.</param>
        /// <param name="includePrereleases">A value indicating whether prerelease packages should be included.</param>
        void InitializeViewModel(string query, PackageSortOption sort, bool includePrereleases);

        /// <summary>
        /// Executes a search operation with query, sort option, and prerelease inclusion parameters.
        /// </summary>
        /// <param name="query">The search query text filter.</param>
        /// <param name="sort">The sort option filter.</param>
        /// <param name="includePrereleases">A value indicating whether prerelease packages should be included.</param>
        void Search(string query = "", PackageSortOption sort = PackageSortOption.Relevance, bool includePrereleases = false);
    }
}
