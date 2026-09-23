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
    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Models.Package;
    using Mycelium.Forge.ViewModels.MyPackages;

    using ReactiveUI;

    /// <summary>
    /// Represents the My Packages page, which lists all packages owned or maintained
    /// by the current user across their account and organizations.
    /// </summary>
    public partial class MyPackages : DisposableComponent
    {
        /// <summary>
        /// The identifier representing all publisher filter options.
        /// </summary>
        public const string AllPublishers = "all";

        /// <summary>
        /// Gets or sets the identifier of the currently selected publisher filter option.
        /// Defaults to <see cref="AllPublishers" />.
        /// </summary>
        public string SelectedPublisher { get; set; } = AllPublishers;

        /// <summary>
        /// Gets or sets the view model for the My Packages page.
        /// </summary>
        [Inject]
        public IMyPackagesViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets the filtered collection of package DTOs based on the selected publisher.
        /// </summary>
        /// <returns>The collection of matching package DTOs.</returns>
        public IReadOnlyList<IPackage> FilteredPackages()
        {
            if (string.IsNullOrWhiteSpace(this.SelectedPublisher) || string.Equals(this.SelectedPublisher, AllPublishers, StringComparison.OrdinalIgnoreCase))
            {
                return this.ViewModel.Packages;
            }

            return
            [
                .. this.ViewModel.Packages
                    .Where(package => string.Equals(this.ViewModel.GetPublisher(package), this.SelectedPublisher, StringComparison.OrdinalIgnoreCase))
            ];
        }

        /// <summary>
        /// Computes the list of publisher filter options with item counts.
        /// </summary>
        /// <returns>The collection of publisher filter options.</returns>
        public IReadOnlyList<OptionModel> GetPublisherFilterOptions()
        {
            List<OptionModel> options =
            [
                new(AllPublishers, $"All ({this.ViewModel.Packages.Count})")
            ];

            var publisherGroups = this.ViewModel.Packages
                .GroupBy(package => this.ViewModel.GetPublisher(package))
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

            var isSelected = string.IsNullOrWhiteSpace(this.SelectedPublisher)
                ? string.Equals(key, AllPublishers, StringComparison.OrdinalIgnoreCase)
                : string.Equals(this.SelectedPublisher, key, StringComparison.OrdinalIgnoreCase);

            return isSelected
                ? $"{baseClass} text-primary font-semibold"
                : $"{baseClass} text-secondary-foreground font-medium";
        }

        /// <summary>
        /// Gets the badge variant for the package visibility column.
        /// Private packages use the default (primary-tinted) variant; others use secondary.
        /// </summary>
        /// <param name="package">The package DTO item.</param>
        /// <returns>The badge variant for the given visibility.</returns>
        public static BadgeVariant GetVisibilityBadgeVariant(IPackage package)
        {
            return package.Visibility == VisibilityKind.PRIVATE
                ? BadgeVariant.Default
                : BadgeVariant.Secondary;
        }

        /// <summary>
        /// Performs component initialization and subscribes to view model property changes.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.Disposables.Add(this.ViewModel.WhenAnyValue(x => x.IsLoading).Subscribe(_ => this.InvokeAsync(this.StateHasChanged)));
        }

        /// <summary>
        /// Initializes the component lifecycle and populates the view model state asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await this.ViewModel.InitializeViewModel();
        }
    }
}
