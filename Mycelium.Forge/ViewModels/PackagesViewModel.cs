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
    /// Provides view model state and operations for the Mycelium Forge package discovery and catalog page.
    /// </summary>
    public class PackagesViewModel : IPackagesViewModel
    {
        /// <summary>
        /// The master collection of seed package catalog search results.
        /// </summary>
        private readonly List<PackageRowModel> allPackages =
        [
            new(
                "ECSS-MM-PWR",
                "/packages/starion/ECSS-MM-PWR",
                "ECSS mission model: Power subsystem. Part definitions for power bus, battery, solar array, and power conditioning unit, typed by ISQ quantity kinds.",
                "SysML v2",
                "@starion",
                "v1.2.0",
                "mission-model · power · ecss",
                "2 weeks ago",
                "210",
                true),
            new(
                "SysMLv2-ISQ-Quantities",
                "/packages/omg/SysMLv2-ISQ-Quantities",
                "Standard quantities and units definition package for SysML v2 models based on ISO/IEC 80000. Quantities of kind, measurement units, and dimension vectors.",
                "SysML v2",
                "@omg",
                "v2025.2",
                "standard-library · quantities-units · isq · sysml2",
                "1 month ago",
                "1.4k",
                true),
            new(
                "SysMLv2-Kernel-Library",
                "/packages/omg/SysMLv2-Kernel-Library",
                "Fundamental KerML metamodel library containing base types, collections, control functions, and measurement scales used by all SysML v2 packages.",
                "SysML v2",
                "@omg",
                "v2025.2",
                "standard-library · kerml · kernel · sysml2",
                "1 month ago",
                "2.1k",
                true),
            new(
                "ECSS-E-ST-10-04C",
                "/packages/esa/ECSS-E-ST-10-04C",
                "Space environment definitions following ECSS-E-ST-10-04C. Earth atmosphere models, solar radiation, geomagnetic field, and planetary constants.",
                "SysML v2",
                "@esa",
                "v1.0.0",
                "mission-model · space-environment · ecss · esa",
                "2 months ago",
                "860",
                true),
            new(
                "SmallSat-Platform-Model",
                "/packages/starion/SmallSat-Platform-Model",
                "Parametric smallsat platform model including bus geometry, mass properties, power budget, and propulsion subsystem interfaces.",
                "SysML v2",
                "@starion",
                "v0.8.2",
                "mission-model · smallsat · platform · starion",
                "3 weeks ago",
                "145",
                true),
            new(
                "CDP4-COMET-Core",
                "/packages/starion/CDP4-COMET-Core",
                "Core concurrent engineering data definitions and iteration exchange schemas for ECSS-E-TM-10-25 concurrent design platform.",
                "CDP4-COMET (10-25)",
                "@starion",
                "v10.25.1",
                "concurrent-engineering · cdp4 · comet · ecss-10-25",
                "1 month ago",
                "320",
                true)
        ];

        /// <summary>
        /// Gets or sets the collection of facet filter options.
        /// </summary>
        public List<OptionModel> Facets { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of displayed package search result items.
        /// </summary>
        public List<PackageRowModel> PackageResults { get; set; } = [];

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
                this.PackageResults = [.. this.allPackages];
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
                new OptionModel("FORMAT", "SysML v2 (kpar)", 5),
                new OptionModel("FORMAT", "CDP4-COMET (10-25)", 1),
                new OptionModel("FORMAT", "Capella", 0),
                new OptionModel("KIND", "Library", 0),
                new OptionModel("KIND", "Model", 6),
                new OptionModel("SCOPE / PUBLISHER", "@esa", 4),
                new OptionModel("SCOPE / PUBLISHER", "@starion", 2),
                new OptionModel("SCOPE / PUBLISHER", "@mycelium", 0),
                new OptionModel("CATEGORY", "mission-model", 5, true),
                new OptionModel("CATEGORY", "standard-library", 1),
                new OptionModel("CATEGORY", "quantities-units", 1),
                new OptionModel("CATEGORY", "view-definitions", 0),
                new OptionModel("TAGS", "aocs", 1),
                new OptionModel("TAGS", "power", 1),
                new OptionModel("TAGS", "thermal", 1),
                new OptionModel("TAGS", "mechanical", 1),
                new OptionModel("TAGS", "comms", 1),
                new OptionModel("METAMODEL", "SysML v2 (2025-02)", 6),
                new OptionModel("METAMODEL", "ECSS-E-TM-10-25", 1),
                new OptionModel("LICENSE", "Apache-2.0", 5),
                new OptionModel("LICENSE", "MIT", 1)
            ];
        }
    }
}
