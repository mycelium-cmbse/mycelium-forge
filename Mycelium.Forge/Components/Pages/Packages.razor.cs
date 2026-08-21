// ------------------------------------------------------------------------------------------------
// <copyright file="Packages.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extensions;
    using Mycelium.Forge.Models.Package;
    using Mycelium.Forge.ViewModels.Packages;

    /// <summary>
    /// Represents the package discovery and facet filtering page of the Mycelium Forge registry.
    /// </summary>
    public partial class Packages : ComponentBase
    {
        /// <summary>
        /// Gets or sets the search query parameter supplied from the URL route.
        /// </summary>
        [SupplyParameterFromQuery(Name = UrlParameterNames.Query)]
        public string Query { get; set; } = "ecss";

        /// <summary>
        /// Gets or sets the sort order query parameter supplied from the URL route.
        /// </summary>
        [SupplyParameterFromQuery(Name = UrlParameterNames.Sort)]
        public string Sort { get; set; } = "relevance";

        /// <summary>
        /// Gets or sets the format filter query parameter supplied from the URL route.
        /// </summary>
        [SupplyParameterFromQuery(Name = UrlParameterNames.Format)]
        public string Format { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the category filter query parameter supplied from the URL route.
        /// </summary>
        [SupplyParameterFromQuery(Name = UrlParameterNames.Category)]
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current search query text in the input box.
        /// </summary>
        public string SearchQuery { get; set; } = "ecss";

        /// <summary>
        /// Gets or sets the currently selected sort option enum value.
        /// </summary>
        public PackageSortOption SelectedSortOption { get; set; } = PackageSortOption.Relevance;

        /// <summary>
        /// Gets or sets a value indicating whether prerelease packages should be included in results.
        /// </summary>
        public bool IncludePrereleases { get; set; }

        /// <summary>
        /// Gets the list of available sort option enum values.
        /// </summary>
        public IReadOnlyList<PackageSortOption> SortOptions { get; } =
        [
            PackageSortOption.Relevance,
            PackageSortOption.Downloads,
            PackageSortOption.RecentlyUpdated,
            PackageSortOption.Alphabetical
        ];

        /// <summary>
        /// Gets or sets the view model for the packages discovery page.
        /// </summary>
        [Inject]
        public IPackagesViewModel ViewModel { get; set; }

        /// <summary>
        /// Handles changes to the sort option selection and updates search results.
        /// </summary>
        /// <param name="sortOption">The selected sort option.</param>
        public void OnSortOptionChanged(PackageSortOption sortOption)
        {
            this.SelectedSortOption = sortOption;
            this.ViewModel.Search(this.SearchQuery, this.SelectedSortOption, this.IncludePrereleases);
        }

        /// <summary>
        /// Gets the display text representation for a sort option enum value.
        /// </summary>
        /// <param name="option">The package sort option.</param>
        /// <returns>The human-readable sort option label.</returns>
        public static string GetSortOptionLabel(PackageSortOption option)
        {
            return option switch
            {
                PackageSortOption.Relevance => "Relevance",
                PackageSortOption.Downloads => "Downloads",
                PackageSortOption.RecentlyUpdated => "Recently updated",
                PackageSortOption.Alphabetical => "Alphabetical",
                _ => nameof(PackageSortOption.Relevance).ToUpperCaseFirst()
            };
        }

        /// <summary>
        /// Gets the CSS classes for a facet option label.
        /// </summary>
        /// <param name="item">The facet item model.</param>
        /// <returns>The computed CSS class string.</returns>
        public static string GetFacetLabelClass(OptionModel item)
        {
            const string baseClass = "text-xs leading-2xs flex-1 truncate transition-colors cursor-pointer";

            return item.IsChecked
                ? $"{baseClass} font-medium text-foreground"
                : $"{baseClass} font-normal text-secondary-text hover:text-foreground";
        }

        /// <summary>
        /// Gets the human-readable search results summary heading text.
        /// </summary>
        /// <returns>
        /// A formatted string indicating the number of matching package results.
        /// </returns>
        public string GetResultsCountText()
        {
            var count = this.ViewModel.PackageResults.Count;

            if (!string.IsNullOrWhiteSpace(this.SearchQuery))
            {
                return count == 1
                    ? $"1 package matches “{this.SearchQuery}”"
                    : $"{count} packages match “{this.SearchQuery}”";
            }

            return count == 1
                ? "1 package"
                : $"{count} packages";
        }

        /// <summary>
        /// Handles search input value changes from the search input.
        /// </summary>
        /// <param name="value">The new search query value.</param>
        public void OnSearchValueChanged(string value)
        {
            this.SearchQuery = value ?? string.Empty;
            this.ViewModel.Search(this.SearchQuery, this.SelectedSortOption, this.IncludePrereleases);
        }

        /// <summary>
        /// Handles changes to the include prereleases checkbox state.
        /// </summary>
        /// <param name="isChecked">A value indicating whether prerelease packages should be included.</param>
        public void OnIncludePrereleasesChanged(bool isChecked)
        {
            this.IncludePrereleases = isChecked;
            this.ViewModel.Search(this.SearchQuery, this.SelectedSortOption, this.IncludePrereleases);
        }

        /// <summary>
        /// Toggles the selection state of the specified facet item.
        /// </summary>
        /// <param name="item">The facet option item to toggle.</param>
        public void ToggleFacet(OptionModel item)
        {
            item.IsChecked = !item.IsChecked;
        }

        /// <summary>
        /// Applies the current facet filters and search query to update search results.
        /// </summary>
        public void ApplyFilters()
        {
            this.ViewModel.Search(this.SearchQuery, this.SelectedSortOption, this.IncludePrereleases);
        }

        /// <summary>
        /// Resets all facet filters to their default state and clears search query.
        /// </summary>
        public void ResetFilters()
        {
            this.SearchQuery = string.Empty;
            this.ResetFacetSelections();
            this.ViewModel.Search(this.SearchQuery, this.SelectedSortOption, this.IncludePrereleases);
        }

        /// <summary>
        /// Clears all active filters and search terms and browses the full catalog.
        /// </summary>
        public void BrowseAllPackages()
        {
            this.SearchQuery = string.Empty;
            this.ResetFacetSelections();
            this.ViewModel.Search(this.SearchQuery, this.SelectedSortOption, this.IncludePrereleases);
        }

        /// <summary>
        /// Initializes the component lifecycle and populates the view model state.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (!string.IsNullOrWhiteSpace(this.Query))
            {
                this.SearchQuery = this.Query;
            }

            if (!string.IsNullOrWhiteSpace(this.Sort))
            {
                if (Enum.TryParse<PackageSortOption>(this.Sort, true, out var parsedSort))
                {
                    this.SelectedSortOption = parsedSort;
                }
            }

            this.ViewModel.InitializeViewModel(this.SearchQuery, this.SelectedSortOption, this.IncludePrereleases);
        }

        /// <summary>
        /// Resets the checked state of all facet filter options across all categories.
        /// </summary>
        private void ResetFacetSelections()
        {
            foreach (var item in this.ViewModel.Facets)
            {
                item.IsChecked = false;
            }
        }
    }
}
