// ------------------------------------------------------------------------------------------------
// <copyright file="MyPackages.razor.cs" company="Starion Group S.A.">
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
    using Mycelium.Forge.Models;
    using Mycelium.Forge.ViewModels;

    /// <summary>
    /// Represents the My Packages page, which lists all packages owned or maintained
    /// by the current user across their account and organizations.
    /// </summary>
    public partial class MyPackages : ComponentBase
    {
        /// <summary>
        /// Gets or sets the identifier of the currently selected publisher filter option.
        /// Defaults to <c>"all"</c>.
        /// </summary>
        public string SelectedPublisher { get; set; } = "all";

        /// <summary>
        /// Gets or sets the view model for the My Packages page.
        /// </summary>
        [Inject]
        public IMyPackagesViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets the filtered collection of package models based on the selected publisher.
        /// </summary>
        /// <returns>The collection of matching package models.</returns>
        public IReadOnlyList<PackageModel> FilteredPackages()
        {
            if (string.Equals(this.SelectedPublisher, "all", StringComparison.OrdinalIgnoreCase))
            {
                return this.ViewModel.Packages;
            }

            return
            [
                .. this.ViewModel.Packages
                    .Where(package => string.Equals(package.Publisher, this.SelectedPublisher, StringComparison.OrdinalIgnoreCase))
            ];
        }

        /// <summary>
        /// Computes the list of publisher filter options with item counts.
        /// </summary>
        /// <returns>The collection of publisher filter options.</returns>
        public IReadOnlyList<OptionModel> GetPublisherFilterOptions()
        {
            var options = new List<OptionModel>
            {
                new("all", $"All ({this.ViewModel.Packages.Count})")
            };

            var publisherGroups = this.ViewModel.Packages
                .GroupBy(package => package.Publisher)
                .OrderBy(group => group.Key);

            foreach (var group in publisherGroups)
            {
                options.Add(new OptionModel(group.Key, $"{group.Key} ({group.Count()})"));
            }

            return options;
        }

        /// <summary>
        /// Gets the CSS class string for a publisher toggle chip based on whether it is currently selected.
        /// </summary>
        /// <param name="key">The publisher filter identifier.</param>
        /// <returns>The computed CSS class string for the chip button.</returns>
        public string GetPublisherChipClass(string key)
        {
            const string baseClass = "h-6 px-2.5 py-1 rounded-md bg-secondary text-xs leading-none transition-colors cursor-pointer border-0 outline-none inline-flex items-center text-left data-[state=on]:text-primary data-[state=on]:font-semibold data-[state=on]:bg-secondary data-[state=off]:text-secondary-foreground data-[state=off]:font-medium data-[state=off]:bg-secondary hover:text-primary";

            return string.Equals(this.SelectedPublisher, key, StringComparison.OrdinalIgnoreCase)
                ? $"{baseClass} text-primary font-semibold"
                : $"{baseClass} text-secondary-foreground font-medium";
        }

        /// <summary>
        /// Gets the CSS class string for the visibility cell, applying bold foreground for private packages.
        /// </summary>
        /// <param name="entry">The package model item.</param>
        /// <returns>The computed CSS class string for the visibility table cell.</returns>
        public string GetVisibilityCellClass(PackageModel entry)
        {
            var isPrivate = entry.Visibility == VisibilityKind.PRIVATE;
            var modifier = isPrivate ? "font-medium text-foreground" : "font-normal text-secondary-foreground";
            return $"w-[130px] shrink-0 p-0 {modifier}";
        }

        /// <summary>
        /// Gets the CSS class string for the role cell, applying primary color for owners.
        /// </summary>
        /// <param name="entry">The package model item.</param>
        /// <returns>The computed CSS class string for the role table cell.</returns>
        public string GetRoleCellClass(PackageModel entry)
        {
            var isOwner = string.Equals(entry.Role, "Owner", StringComparison.OrdinalIgnoreCase);
            var colorClass = isOwner ? "text-primary" : "text-muted-foreground";
            return $"w-[150px] shrink-0 text-xs leading-2xs font-medium p-0 {colorClass}";
        }

        /// <summary>
        /// Initializes the component lifecycle and populates the view model state.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.ViewModel.InitializeViewModel();
        }
    }
}
