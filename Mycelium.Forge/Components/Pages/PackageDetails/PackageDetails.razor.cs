// ------------------------------------------------------------------------------------------------
// <copyright file="PackageDetails.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.PackageDetails
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extensions;
    using Mycelium.Forge.Models.Common;
    using Mycelium.Forge.ViewModels.PackageDetails;

    /// <summary>
    /// Represents the package details and release overview page for Mycelium Forge packages.
    /// </summary>
    public partial class PackageDetails : ComponentBase
    {
        /// <summary>
        /// Gets or sets the navigation manager used for URI navigation.
        /// </summary>
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the scope segment supplied from the URL route.
        /// </summary>
        [Parameter]
        public string Scope { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the package name supplied from the URL route.
        /// </summary>
        [Parameter]
        public string PackageName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the content tab supplied from the URL route.
        /// </summary>
        [Parameter]
        public string Tab { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the view model for the package details page.
        /// </summary>
        [Inject]
        public IPackageDetailsViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets the available package content tabs.
        /// </summary>
        public IReadOnlyList<string> ContentTabs { get; } =
        [
            PackageTabConstants.Overview,
            PackageTabConstants.Contents,
            PackageTabConstants.Dependencies,
            PackageTabConstants.Dependents,
            PackageTabConstants.Versions,
            PackageTabConstants.Validation
        ];

        /// <summary>
        /// Gets or sets the currently selected content section tab.
        /// </summary>
        public string SelectedContentTab { get; set; } = PackageTabConstants.Overview;

        /// <summary>
        /// Selects a main content navigation tab and navigates to its route.
        /// </summary>
        /// <param name="tab">The name of the content tab.</param>
        public void SelectContentTab(string tab)
        {
            this.SelectedContentTab = tab;
            var targetRoute = PageRoutes.GetPackageRoute(this.Scope, this.PackageName, tab);
            this.NavigationManager.NavigateTo(targetRoute);
        }

        /// <summary>
        /// Gets the CSS classes for a content section tab trigger.
        /// </summary>
        /// <param name="tab">The content section tab name.</param>
        /// <returns>The computed CSS class string.</returns>
        public string GetContentTabClass(string tab)
        {
            const string baseClass = "h-full px-4 rounded-md text-sm leading-xs transition-colors whitespace-nowrap cursor-pointer inline-flex items-center justify-center";

            return string.Equals(this.SelectedContentTab, tab, StringComparison.OrdinalIgnoreCase)
                ? $"{baseClass} bg-primary/10 text-primary font-semibold"
                : $"{baseClass} text-muted-foreground hover:text-foreground hover:bg-muted/50 font-medium";
        }

        /// <summary>
        /// Computes the formatted quality score summary string from the quality checks (e.g., "5/5 checks").
        /// </summary>
        /// <returns>The formatted quality score string.</returns>
        public string GetQualityScore()
        {
            if (this.ViewModel.QualityChecks.Count == 0)
            {
                return string.Empty;
            }

            var passedCount = this.ViewModel.QualityChecks.Count(check => check.Passed);
            return $"{passedCount}/{this.ViewModel.QualityChecks.Count} checks";
        }

        /// <summary>
        /// Computes the relative time ago string for the package publication.
        /// </summary>
        /// <returns>A human-readable relative time string.</returns>
        public string GetPublishedAgo()
        {
            return this.ViewModel.CurrentVersion.PublicationDate.ToTimeAgo();
        }

        /// <summary>
        /// Computes the lifecycle release status label for the package.
        /// </summary>
        /// <returns>A string indicating the release status label.</returns>
        public string GetReleaseStatus()
        {
            if (this.ViewModel.Package.IsDeprecated || this.ViewModel.CurrentVersion.IsDeprecated)
            {
                return "Deprecated";
            }

            return "Latest stable";
        }

        /// <summary>
        /// Computes the canonical Package URL (purl) identifier.
        /// </summary>
        /// <returns>The resolved package URL string.</returns>
        public string GetPackageUrl()
        {
            return InstallCommandHelper.GeneratePurl(this.ViewModel.Owner.ShortName, this.ViewModel.Package.ShortName, this.ViewModel.CurrentVersion.GetVersion());
        }

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// Override this method if you will perform an asynchronous operation and
        /// want the component to refresh when that operation is completed.
        /// </summary>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (!string.IsNullOrWhiteSpace(this.Tab) && this.ContentTabs.Contains(this.Tab.ToLowerInvariant()))
            {
                this.SelectedContentTab = this.Tab.ToLowerInvariant();
            }
            else
            {
                this.SelectedContentTab = PackageTabConstants.Overview;
            }

            await this.ViewModel.InitializeViewModel(this.PackageName, this.Scope, this.SelectedContentTab);
        }

        /// <summary>
        /// Gets the breadcrumb navigation items for the package details page.
        /// </summary>
        /// <returns>A collection of <see cref="BreadcrumbItem" /> entries representing the trail.</returns>
        private IEnumerable<BreadcrumbItem> GetBreadcrumbItems()
        {
            return
            [
                new BreadcrumbItem
                {
                    Name = "Search",
                    Link = PageRoutes.Packages
                },
                new BreadcrumbItem
                {
                    Name = $"@{this.ViewModel.Owner.ShortName}",
                    Link = PageRoutes.GetOrganizationRoute(this.ViewModel.Owner.ShortName)
                },
                new BreadcrumbItem
                {
                    Name = this.ViewModel.Package.Name
                }
            ];
        }
    }
}
