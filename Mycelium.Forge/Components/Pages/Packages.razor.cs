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
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Models;
    using Mycelium.Forge.ViewModels;

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
        /// Gets or sets the currently selected sort option display name.
        /// </summary>
        public string SelectedSortOption { get; set; } = "Relevance";

        /// <summary>
        /// Gets or sets a value indicating whether the sort options dropdown is visible.
        /// </summary>
        public bool IsSortDropdownOpen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether prerelease packages should be included in results.
        /// </summary>
        public bool IncludePrereleases { get; set; }

        /// <summary>
        /// Gets the list of available sort option labels.
        /// </summary>
        public IReadOnlyList<string> SortOptions { get; } =
        [
            "Relevance",
            "Downloads",
            "Recently updated",
            "Alphabetical"
        ];

        /// <summary>
        /// Gets or sets the view model for the packages discovery page.
        /// </summary>
        [Inject]
        public IPackagesViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets the human-readable search results summary heading text.
        /// </summary>
        /// <returns>
        /// A formatted string indicating the number of matching package results.
        /// </returns>
        public string GetResultsCountText()
        {
            var count = this.ViewModel.PackageResults.Count;

            return !string.IsNullOrWhiteSpace(this.SearchQuery) 
                ? $"{count} packages matching “{this.SearchQuery}”" 
                : $"{count} packages";
        }

        /// <summary>
        /// Handles search input value changes from the search input.
        /// </summary>
        /// <param name="value">The new search query value.</param>
        public void OnSearchValueChanged(string value)
        {
            this.SearchQuery = value ?? string.Empty;
        }

        /// <summary>
        /// Clears the active search query text and updates results.
        /// </summary>
        public void ClearSearch()
        {
            this.SearchQuery = string.Empty;
            this.ViewModel.Search();
        }

        /// <summary>
        /// Toggles the open/closed state of the sort dropdown menu.
        /// </summary>
        public void ToggleSortDropdown()
        {
            this.IsSortDropdownOpen = !this.IsSortDropdownOpen;
        }

        /// <summary>
        /// Selects the specified sort option and closes the dropdown menu.
        /// </summary>
        /// <param name="sortOption">The selected sort option label.</param>
        public void SelectSortOption(string sortOption)
        {
            this.SelectedSortOption = sortOption;
            this.IsSortDropdownOpen = false;
        }

        /// <summary>
        /// Handles changes to the include prereleases checkbox state.
        /// </summary>
        /// <param name="isChecked">A value indicating whether prerelease packages should be included.</param>
        public void OnIncludePrereleasesChanged(bool isChecked)
        {
            this.IncludePrereleases = isChecked;
        }

        /// <summary>
        /// Toggles the selection state of the specified facet item.
        /// </summary>
        /// <param name="item">The facet option item to toggle.</param>
        public void ToggleFacet(FacetItemModel item)
        {
            item.IsChecked = !item.IsChecked;
        }

        /// <summary>
        /// Applies the current facet filters and search query to update search results.
        /// </summary>
        public void ApplyFilters()
        {
            this.ViewModel.Search();
        }

        /// <summary>
        /// Resets all facet filters to their default state and clears search query.
        /// </summary>
        public void ResetFilters()
        {
            this.SearchQuery = string.Empty;
            this.ViewModel.Search();
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
                var matchedSort = this.SortOptions.FirstOrDefault(x => x.ToLowerInvariant() == this.Sort.ToLowerInvariant());

                if (!string.IsNullOrWhiteSpace(matchedSort))
                {
                    this.SelectedSortOption = matchedSort;
                }
            }

            this.ViewModel.InitializeViewModel(this.SearchQuery, this.Sort, this.Format, this.Category);
        }
    }
}
