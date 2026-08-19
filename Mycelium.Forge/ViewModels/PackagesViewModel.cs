// ------------------------------------------------------------------------------------------------
// <copyright file="PackagesViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels
{
    using Mycelium.Forge.Models;

    /// <summary>
    /// Provides view model state and operations for the Mycelium Forge package discovery and filtering page.
    /// </summary>
    public class PackagesViewModel : IPackagesViewModel
    {
        /// <summary>
        /// The master collection of package search result items.
        /// </summary>
        private readonly IReadOnlyList<PackageRowModel> allPackages =
        [
            new(
                "ecss-e-st-10-04c",
                "/packages/esa/ecss-e-st-10-04c",
                "Space environment standard reference model for space systems engineering.",
                "SysML v2",
                "@esa",
                "v1.4.0",
                "aocs · thermal · space · environment · ecss",
                "3 days ago",
                "420"),
            new(
                "ecss-e-st-70-46c",
                "/packages/esa/ecss-e-st-70-46c",
                "Electrical and power system architectures following ECSS engineering standards.",
                "SysML v2",
                "@esa",
                "v2.1.0",
                "power · electrical · subsystems · ecss",
                "2 weeks ago",
                "315"),
            new(
                "ecss-e-st-50-14c",
                "/packages/esa/ecss-e-st-50-14c",
                "Attitude and orbit control system spacecraft dynamics reference package.",
                "SysML v2",
                "@esa",
                "v0.9.2",
                "aocs · dynamics · navigation · ecss",
                "1 month ago",
                "280"),
            new(
                "ecss-e-st-32-10c",
                "/packages/starion/ecss-e-st-32-10c",
                "RF telecommunication link budget and space communication interfaces.",
                "SysML v2",
                "@starion",
                "v0.3.0",
                "comms · rf · telemetry · ecss",
                "2 months ago",
                "190"),
            new(
                "ecss-e-st-31-01c",
                "/packages/starion/ecss-e-st-31-01c",
                "Structural and mechanical engineering domain metamodels and loads analysis.",
                "SysML v2",
                "@starion",
                "v1.0.0",
                "mechanical · structures · loads · ecss",
                "3 months ago",
                "165"),
            new(
                "ECSS-E-TM-10-25-Schema",
                "/packages/esa/ECSS-E-TM-10-25-Schema",
                "Space engineering model-based data exchange engineering ontology.",
                "CDP4-COMET",
                "@esa",
                "v1.2.0",
                "ecss · cdp4 · space · ontology · exchange",
                "4 months ago",
                "940",
                true)
        ];

        /// <summary>
        /// Gets or sets the collection of facet filter options.
        /// </summary>
        public IReadOnlyList<FacetItemModel> Facets { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of displayed package search result items.
        /// </summary>
        public IReadOnlyList<PackageRowModel> PackageResults { get; set; } = [];

        /// <summary>
        /// Initializes the view model state and executes the initial package search.
        /// </summary>
        /// <param name="query">The search query parameter from URL route.</param>
        /// <param name="sort">The sort order parameter from URL route.</param>
        /// <param name="includePrereleases">A value indicating whether prerelease packages should be included.</param>
        public void InitializeViewModel(string query, PackageSortOption sort, bool includePrereleases)
        {
            this.InitializeFacets();
            this.Search(query, sort, includePrereleases);
        }

        /// <summary>
        /// Executes a search operation with query, sort option, and prerelease inclusion parameters.
        /// </summary>
        /// <param name="query">The search query text filter.</param>
        /// <param name="sort">The sort option filter.</param>
        /// <param name="includePrereleases">A value indicating whether prerelease packages should be included.</param>
        public void Search(string query = "", PackageSortOption sort = PackageSortOption.Relevance, bool includePrereleases = false)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                this.PackageResults = this.allPackages;
                return;
            }

            var trimmed = query.Trim();

            this.PackageResults =
            [
                .. this.allPackages.Where(p =>
                    p.Name.Contains(trimmed, StringComparison.OrdinalIgnoreCase))
            ];
        }

        /// <summary>
        /// Populates the initial facet filter lists.
        /// </summary>
        private void InitializeFacets()
        {
            this.Facets =
            [
                new FacetItemModel("FORMAT", "SysML v2 (kpar)", 5),
                new FacetItemModel("FORMAT", "CDP4-COMET (10-25)", 1),
                new FacetItemModel("FORMAT", "Capella", 0),
                new FacetItemModel("KIND", "Library", 0),
                new FacetItemModel("KIND", "Model", 6),
                new FacetItemModel("SCOPE / PUBLISHER", "@esa", 4),
                new FacetItemModel("SCOPE / PUBLISHER", "@starion", 2),
                new FacetItemModel("SCOPE / PUBLISHER", "@mycelium", 0),
                new FacetItemModel("CATEGORY", "mission-model", 5, true),
                new FacetItemModel("CATEGORY", "standard-library", 1),
                new FacetItemModel("CATEGORY", "quantities-units", 1),
                new FacetItemModel("CATEGORY", "view-definitions", 0),
                new FacetItemModel("TAGS", "aocs", 1),
                new FacetItemModel("TAGS", "power", 1),
                new FacetItemModel("TAGS", "thermal", 1),
                new FacetItemModel("TAGS", "mechanical", 1),
                new FacetItemModel("TAGS", "comms", 1),
                new FacetItemModel("METAMODEL", "SysML v2 (2025-02)", 6),
                new FacetItemModel("METAMODEL", "ECSS-E-TM-10-25", 1),
                new FacetItemModel("LICENSE", "Apache-2.0", 5),
                new FacetItemModel("LICENSE", "MIT", 1)
            ];
        }
    }
}
