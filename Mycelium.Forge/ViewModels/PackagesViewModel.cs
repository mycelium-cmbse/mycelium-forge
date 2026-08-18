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
    using System.Collections.Generic;

    using Mycelium.Forge.Models;

    /// <summary>
    /// Provides view model state and operations for the Mycelium Forge package discovery and filtering page.
    /// </summary>
    public class PackagesViewModel : IPackagesViewModel
    {
        /// <summary>
        /// Gets or sets the collection of format facet filter options.
        /// </summary>
        public IReadOnlyList<FacetItemModel> Formats { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of kind facet filter options.
        /// </summary>
        public IReadOnlyList<FacetItemModel> Kinds { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of scope and publisher facet filter options.
        /// </summary>
        public IReadOnlyList<FacetItemModel> Scopes { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of category facet filter options.
        /// </summary>
        public IReadOnlyList<FacetItemModel> Categories { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of tag facet filter options.
        /// </summary>
        public IReadOnlyList<FacetItemModel> Tags { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of metamodel facet filter options.
        /// </summary>
        public IReadOnlyList<FacetItemModel> Metamodels { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of license facet filter options.
        /// </summary>
        public IReadOnlyList<FacetItemModel> Licenses { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of displayed package search result items.
        /// </summary>
        public IReadOnlyList<PackageRowModel> PackageResults { get; set; } = [];

        /// <summary>
        /// Initializes the view model state and executes the initial package search.
        /// </summary>
        /// <param name="query">The search query parameter from URL route.</param>
        /// <param name="sort">The sort order parameter from URL route.</param>
        /// <param name="format">The format filter parameter from URL route.</param>
        /// <param name="category">The category filter parameter from URL route.</param>
        public void InitializeViewModel(string query, string sort, string format, string category)
        {
            this.Search();
        }

        /// <summary>
        /// Executes a search operation and refreshes facet options and package results.
        /// </summary>
        public void Search()
        {
            this.InitializeFacets();
            this.InitializePackages();
        }

        /// <summary>
        /// Populates the initial facet filter lists.
        /// </summary>
        private void InitializeFacets()
        {
            this.Formats =
            [
                new FacetItemModel("SysML v2 (kpar)", 5),
                new FacetItemModel("CDP4-COMET (10-25)", 1),
                new FacetItemModel("Capella", 0)
            ];

            this.Kinds =
            [
                new FacetItemModel("Library", 0),
                new FacetItemModel("Model", 6)
            ];

            this.Scopes =
            [
                new FacetItemModel("@esa", 4),
                new FacetItemModel("@starion", 2),
                new FacetItemModel("@mycelium", 0)
            ];

            this.Categories =
            [
                new FacetItemModel("mission-model", 6, true),
                new FacetItemModel("standard-library", 0),
                new FacetItemModel("quantities-units", 0),
                new FacetItemModel("view-definitions", 0)
            ];

            this.Tags =
            [
                new FacetItemModel("aocs", 1),
                new FacetItemModel("power", 1),
                new FacetItemModel("thermal", 1),
                new FacetItemModel("mechanical", 1),
                new FacetItemModel("comms", 1)
            ];

            this.Metamodels =
            [
                new FacetItemModel("SysML v2 (2025-02)", 5),
                new FacetItemModel("ECSS-E-TM-10-25", 1)
            ];

            this.Licenses =
            [
                new FacetItemModel("Apache-2.0", 5),
                new FacetItemModel("MIT", 1)
            ];
        }

        /// <summary>
        /// Populates the package search result items.
        /// </summary>
        private void InitializePackages()
        {
            this.PackageResults =
            [
                new PackageRowModel(
                    "ecss-e-st-10-04c",
                    "/packages/esa/ecss-e-st-10-04c",
                    "Space environment standard reference model for space systems engineering.",
                    "SysML v2",
                    "@esa",
                    "v1.4.0",
                    "aocs · thermal · space · environment · ecss",
                    "3 days ago",
                    "420"),
                new PackageRowModel(
                    "ecss-e-st-70-46c",
                    "/packages/esa/ecss-e-st-70-46c",
                    "Electrical and power system architectures following ECSS engineering standards.",
                    "SysML v2",
                    "@esa",
                    "v2.1.0",
                    "power · electrical · subsystems · ecss",
                    "2 weeks ago",
                    "315"),
                new PackageRowModel(
                    "ecss-e-st-50-14c",
                    "/packages/esa/ecss-e-st-50-14c",
                    "Attitude and orbit control system spacecraft dynamics reference package.",
                    "SysML v2",
                    "@esa",
                    "v0.9.2",
                    "aocs · dynamics · navigation · ecss",
                    "1 month ago",
                    "280"),
                new PackageRowModel(
                    "ecss-e-st-32-10c",
                    "/packages/starion/ecss-e-st-32-10c",
                    "RF telecommunication link budget and space communication interfaces.",
                    "SysML v2",
                    "@starion",
                    "v0.3.0",
                    "comms · rf · telemetry · ecss",
                    "2 months ago",
                    "190"),
                new PackageRowModel(
                    "ecss-e-st-31-01c",
                    "/packages/starion/ecss-e-st-31-01c",
                    "Structural and mechanical engineering domain metamodels and loads analysis.",
                    "SysML v2",
                    "@starion",
                    "v1.0.0",
                    "mechanical · structures · loads · ecss",
                    "3 months ago",
                    "165"),
                new PackageRowModel(
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
        }
    }
}
