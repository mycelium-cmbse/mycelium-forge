// ------------------------------------------------------------------------------------------------
// <copyright file="PackageContentsTab.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.PackageDetails.Tabs
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Models.Package;

    /// <summary>
    /// Represents the model elements and declarations tab component for package details.
    /// </summary>
    public partial class PackageContentsTab : ComponentBase
    {
        /// <summary>
        /// Gets or sets the collection of model elements contained in the package.
        /// </summary>
        [Parameter]
        public IReadOnlyList<PackageElementModel> Elements { get; set; } = [];

        /// <summary>
        /// Gets or sets the currently selected element kind filter tab.
        /// </summary>
        public string SelectedKindTab { get; set; } = "Parts";

        /// <summary>
        /// Gets the available element kind category tabs.
        /// </summary>
        public IReadOnlyList<string> KindTabs { get; } =
        [
            "Parts",
            "Attributes",
            "Units",
            "Scales",
            "Types",
            "Templates"
        ];

        /// <summary>
        /// Selects an element kind category filter tab.
        /// </summary>
        /// <param name="kindTab">The name of the kind category tab.</param>
        public void SelectKindTab(string kindTab)
        {
            this.SelectedKindTab = kindTab;
        }

        /// <summary>
        /// Gets the CSS classes for an element kind filter button.
        /// </summary>
        /// <param name="kindTab">The kind category tab name.</param>
        /// <returns>The computed CSS class string.</returns>
        public string GetKindButtonClass(string kindTab)
        {
            return this.SelectedKindTab == kindTab
                ? "rounded-full bg-primary/10 border-primary text-primary font-semibold hover:bg-primary/15"
                : "rounded-full";
        }

        /// <summary>
        /// Gets the collection of model elements filtered by the selected element kind category.
        /// </summary>
        /// <returns>A list of model elements matching the selected kind category or all elements if none match.</returns>
        public IReadOnlyList<PackageElementModel> GetFilteredElements()
        {
            if (this.Elements == null || this.Elements.Count == 0)
            {
                return [];
            }

            var filtered = this.Elements
                .Where(element => string.Equals(element.Category, this.SelectedKindTab, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return filtered.Count > 0
                ? filtered
                : this.Elements;
        }
    }
}
