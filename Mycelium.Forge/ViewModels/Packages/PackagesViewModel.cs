// ------------------------------------------------------------------------------------------------
// <copyright file="PackagesViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.Packages
{
    using Mycelium.Forge.Data;
    using Mycelium.Forge.Enums;
    using Mycelium.Forge.Models.Package;

    /// <summary>
    /// Provides view model state and operations for the Mycelium Forge package discovery and catalog page.
    /// </summary>
    public class PackagesViewModel : IPackagesViewModel
    {
        /// <summary>
        /// The library package tag descriptor constant.
        /// </summary>
        private const string LibraryKind = "library";

        /// <summary>
        /// The model package tag descriptor constant.
        /// </summary>
        private const string ModelKind = "model";

        /// <summary>
        /// The facet group identifier for package kind.
        /// </summary>
        private const string KindFacet = "Kind";

        /// <summary>
        /// The facet group identifier for package category.
        /// </summary>
        private const string CategoryFacet = "Category";

        /// <summary>
        /// The facet group identifier for metamodel definitions.
        /// </summary>
        private const string MetamodelFacet = "Metamodel";

        /// <summary>
        /// The master collection of seed package catalog search results.
        /// </summary>
        private readonly List<PackageModel> allPackages = [];

        /// <summary>
        /// Gets or sets the collection of facet filter options.
        /// </summary>
        public List<OptionModel> Facets { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of displayed package search result items.
        /// </summary>
        public List<PackageModel> PackageResults { get; set; } = [];

        /// <summary>
        /// Initializes the view model state and executes the initial package search.
        /// </summary>
        /// <param name="query">The search query parameter from URL route.</param>
        /// <param name="sort">The sort order parameter from URL route.</param>
        /// <param name="includePrereleases">A value indicating whether prerelease packages should be included.</param>
        public void InitializeViewModel(string query, PackageSortOption sort, bool includePrereleases)
        {
            this.allPackages.Clear();
            this.allPackages.AddRange(SeedData.CatalogPackages);

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
            var filtered = this.allPackages.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var trimmed = query.Trim();

                filtered = filtered.Where(package =>
                    package.Name.Contains(trimmed, StringComparison.OrdinalIgnoreCase) ||
                    package.Description.Contains(trimmed, StringComparison.OrdinalIgnoreCase) ||
                    package.Tags.Contains(trimmed, StringComparison.OrdinalIgnoreCase));
            }

            filtered = this.ApplyFacetFilters(filtered);

            filtered = sort switch
            {
                PackageSortOption.Downloads => filtered.OrderByDescending(package => ParseImportCount(package.ImportCount)),
                PackageSortOption.Alphabetical => filtered.OrderBy(package => package.Name),
                PackageSortOption.RecentlyUpdated => filtered.OrderBy(package => package.LastPublished),
                _ => filtered
            };

            this.PackageResults = [.. filtered];
        }

        /// <summary>
        /// Filters the package collection based on currently checked facet options.
        /// </summary>
        /// <param name="source">The source collection of package row models.</param>
        /// <returns>The filtered collection of package row models.</returns>
        private IEnumerable<PackageModel> ApplyFacetFilters(IEnumerable<PackageModel> source)
        {
            var result = source;
            var checkedGroups = this.Facets.Where(facet => facet.IsChecked).GroupBy(facet => facet.Property).ToList();

            foreach (var group in checkedGroups)
            {
                var labels = group.Select(facet => facet.Label).ToList();

                if (labels.Count == 0)
                {
                    continue;
                }

                result = group.Key switch
                {
                    nameof(PackageModel.Format) => result.Where(package => labels.Exists(label => package.Format.Contains(label.Replace(" (kpar)", string.Empty).Replace(" (10-25)", string.Empty), StringComparison.OrdinalIgnoreCase))),
                    KindFacet => result.Where(package => labels.Exists(label => package.Tags.Contains(label, StringComparison.OrdinalIgnoreCase))),
                    nameof(PackageModel.Publisher) => result.Where(package => labels.Exists(label => string.Equals(package.Publisher, label, StringComparison.OrdinalIgnoreCase))),
                    CategoryFacet => result.Where(package => labels.Exists(label => package.Tags.Contains(label, StringComparison.OrdinalIgnoreCase))),
                    nameof(PackageModel.Tags) => result.Where(package => labels.Exists(label => package.Tags.Contains(label, StringComparison.OrdinalIgnoreCase))),
                    MetamodelFacet => result.Where(package => labels.Exists(label => (label.Contains("SysML", StringComparison.OrdinalIgnoreCase) && package.Format.Contains("SysML", StringComparison.OrdinalIgnoreCase)) || (label.Contains("ECSS", StringComparison.OrdinalIgnoreCase) && (package.Format.Contains("CDP4", StringComparison.OrdinalIgnoreCase) || package.Tags.Contains("ecss-10-25", StringComparison.OrdinalIgnoreCase))))),
                    nameof(PackageModel.License) => result.Where(package => labels.Exists(label => string.Equals(package.License, label, StringComparison.OrdinalIgnoreCase))),
                    _ => result
                };
            }

            return result;
        }

        /// <summary>
        /// Populates the initial facet filter lists dynamically computed by grouping seed packages.
        /// </summary>
        private void InitializeFacets()
        {
            var formatOptions = this.allPackages
                .GroupBy(package => package.Format)
                .OrderByDescending(group => group.Count())
                .Select(group => new OptionModel(nameof(PackageModel.Format), group.Key, group.Count()));

            var kindOptions = this.allPackages
                .SelectMany(package => new[]
                {
                    package.Tags.Contains(LibraryKind, StringComparison.OrdinalIgnoreCase) ? "Library" : null,
                    package.Tags.Contains(ModelKind, StringComparison.OrdinalIgnoreCase) ? "Model" : null
                })
                .Where(kind => !string.IsNullOrEmpty(kind))
                .GroupBy(kind => kind)
                .OrderByDescending(group => group.Count())
                .Select(group => new OptionModel(KindFacet, group.Key, group.Count()));

            var publisherOptions = this.allPackages
                .GroupBy(package => package.Publisher)
                .OrderByDescending(group => group.Count())
                .Select(group => new OptionModel(nameof(PackageModel.Publisher), group.Key, group.Count()));

            var categoryOptions = this.allPackages
                .SelectMany(package => package.Tags.Split(['·', ','], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                .Select(tag => tag.Trim())
                .Where(tag => tag.Contains(ModelKind) || tag.Contains(LibraryKind) || tag.Contains("units") || tag.Contains("engineering"))
                .GroupBy(tag => tag)
                .OrderByDescending(group => group.Count())
                .Select(group => new OptionModel(CategoryFacet, group.Key, group.Count(), string.Equals(group.Key, "mission-model", StringComparison.OrdinalIgnoreCase)));

            var tagOptions = this.allPackages
                .SelectMany(package => package.Tags.Split(['·', ','], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                .Select(tag => tag.Trim())
                .Where(tag => !tag.Contains(LibraryKind) && !tag.Contains("mission-model") && !tag.Contains("units") && !tag.Contains("engineering") && !tag.Contains("sysml"))
                .GroupBy(tag => tag)
                .OrderByDescending(group => group.Count())
                .Select(group => new OptionModel(nameof(PackageModel.Tags), group.Key, group.Count()));

            var metamodelOptions = this.allPackages
                .Select(package => package.Format.Contains("SysML", StringComparison.OrdinalIgnoreCase) ? "SysML v2 (2025-02)" : "ECSS-E-TM-10-25")
                .GroupBy(metamodel => metamodel)
                .OrderByDescending(group => group.Count())
                .Select(group => new OptionModel(MetamodelFacet, group.Key, group.Count()));

            var licenseOptions = this.allPackages
                .Where(package => !string.IsNullOrWhiteSpace(package.License))
                .GroupBy(package => package.License)
                .OrderByDescending(group => group.Count())
                .Select(group => new OptionModel(nameof(PackageModel.License), group.Key, group.Count()));

            this.Facets =
            [
                .. formatOptions,
                .. kindOptions,
                .. publisherOptions,
                .. categoryOptions,
                .. tagOptions,
                .. metamodelOptions,
                .. licenseOptions
            ];
        }

        /// <summary>
        /// Parses a human-readable import count string into a numeric value for sorting.
        /// </summary>
        /// <param name="importCount">The formatted import count string.</param>
        /// <returns>The parsed numeric count.</returns>
        private static int ParseImportCount(string importCount)
        {
            if (string.IsNullOrWhiteSpace(importCount))
            {
                return 0;
            }

            var clean = importCount.Trim().ToLowerInvariant();

            if (clean.EndsWith('k') && double.TryParse(clean[..^1], out var kValue))
            {
                return (int)(kValue * 1000);
            }

            if (int.TryParse(clean, out var intValue))
            {
                return intValue;
            }

            return 0;
        }
    }
}
