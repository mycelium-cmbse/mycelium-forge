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
    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.PackageDetails.Dialogs;
    using Mycelium.Forge.Extensions;
    using Mycelium.Forge.Models.Common;
    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.ViewModels.PackageDetails;

    /// <summary>
    /// Represents the package details and release overview page for Mycelium Forge packages.
    /// </summary>
    public partial class PackageDetails : ComponentBase
    {
        /// <summary>
        /// Gets or sets the dialog service used to display modal dialogs.
        /// </summary>
        [Inject]
        public DialogService DialogService { get; set; }

        /// <summary>
        /// Gets or sets the organization segment supplied from the URL route.
        /// </summary>
        [Parameter]
        public string Organization { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the package name supplied from the URL route.
        /// </summary>
        [Parameter]
        public string PackageName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the view model for the package details page.
        /// </summary>
        [Inject]
        public IPackageDetailsViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the currently selected install method tab.
        /// </summary>
        public string SelectedInstallTab { get; set; } = InstallCommandConstants.ForgeCli;

        /// <summary>
        /// Gets the available installation method tabs.
        /// </summary>
        public IReadOnlyList<string> InstallTabs { get; } =
        [
            InstallCommandConstants.ForgeCli,
            InstallCommandConstants.SysMlV2Import,
            InstallCommandConstants.Manifest,
            InstallCommandConstants.Purl
        ];

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
        /// Gets the install command string for the currently selected install tab.
        /// </summary>
        /// <returns>The resolved install command string, or an empty string if not available.</returns>
        public string GetCurrentInstallCommand()
        {
            return this.ViewModel.InstallCommands.TryGetValue(this.SelectedInstallTab, out var command)
                ? command
                : string.Empty;
        }

        /// <summary>
        /// Selects a main content navigation tab.
        /// </summary>
        /// <param name="tab">The name of the content tab.</param>
        public void SelectContentTab(string tab)
        {
            this.SelectedContentTab = tab;
        }

        /// <summary>
        /// Selects an installation method tab.
        /// </summary>
        /// <param name="tab">The name of the installation method tab.</param>
        public void SelectInstallTab(string tab)
        {
            this.SelectedInstallTab = tab;
        }

        /// <summary>
        /// Gets the CSS classes for an installation method tab trigger.
        /// </summary>
        /// <param name="tab">The installation method tab name.</param>
        /// <returns>The computed CSS class string.</returns>
        public string GetInstallTabClass(string tab)
        {
            const string baseClass = "cursor-pointer transition-colors";

            return string.Equals(this.SelectedInstallTab, tab, StringComparison.OrdinalIgnoreCase)
                ? $"{baseClass} font-semibold text-primary"
                : $"{baseClass} font-medium text-muted-foreground hover:text-foreground";
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
        /// Computes the fully qualified package name in the format @organization/packageName.
        /// </summary>
        /// <returns>The fully qualified package identifier string.</returns>
        public string GetPackageFullName()
        {
            return $"@{this.ViewModel.Organization.ShortName}/{this.ViewModel.Package.Name}";
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
            if (this.ViewModel.SelectedVersion != null)
            {
                return this.ViewModel.SelectedVersion.PublicationDate.ToTimeAgo();
            }

            return this.ViewModel.Package.CreatedAt.ToTimeAgo();
        }

        /// <summary>
        /// Computes the display format string for the package.
        /// </summary>
        /// <returns>The package format name.</returns>
        public string GetFormat()
        {
            return this.ViewModel.PackageType?.Name ?? "SysML v2";
        }

        /// <summary>
        /// Computes the lifecycle release status label for the package.
        /// </summary>
        /// <returns>A string indicating the release status label.</returns>
        public string GetReleaseStatus()
        {
            if (this.ViewModel.Package.IsDeprecated || (this.ViewModel.SelectedVersion?.IsDeprecated ?? false))
            {
                return "Deprecated";
            }

            return "Latest stable";
        }

        /// <summary>
        /// Computes the publication provenance summary string for the package.
        /// </summary>
        /// <returns>A formatted provenance string.</returns>
        public string GetProvenance()
        {
            var publisher = this.ViewModel.Organization.ShortName;
            var license = this.ViewModel.Package.License;
            var downloads = this.ViewModel.Package.downloadCount;

            return $"Published recently by @{publisher} · {license} · {downloads} downloads";
        }

        /// <summary>
        /// Computes the metamodel specification string for the package.
        /// </summary>
        /// <returns>The metamodel specification string.</returns>
        public string GetMetamodel()
        {
            return this.ViewModel.PackageType.Name;
        }

        /// <summary>
        /// Computes the display text for the repository URL.
        /// </summary>
        /// <returns>The repository URL string.</returns>
        public string GetRepositoryDisplayName()
        {
            return this.ViewModel.Package.RepositoryUrl;
        }

        /// <summary>
        /// Computes the canonical Package URL (purl) identifier.
        /// </summary>
        /// <returns>The resolved package URL string.</returns>
        public string GetPackageUrl()
        {
            return InstallCommandHelper.GeneratePurl(this.ViewModel.Organization.ShortName, this.ViewModel.Package.ShortName, this.ViewModel.SelectedVersion.GetVersion());
        }

        /// <summary>
        /// Opens the Add to Project dialog.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OpenAddToProjectDialog()
        {
            var onResult = new EventCallbackFactory().Create(this, (AddToProjectResult result) => this.HandleAddDependency(result));

            var parameters = new Dictionary<string, object>
            {
                { nameof(AddToProjectDialog.Package), this.ViewModel.Package },
                { nameof(AddToProjectDialog.PackageVersion), this.ViewModel.SelectedVersion },
                { nameof(AddToProjectDialog.OnResult), onResult }
            };

            var options = new DialogOpenOptions
            {
                Title = "Add to project",
                Description = $"{this.GetPackageFullName()} · {this.ViewModel.SelectedVersion.GetVersion()}"
            };

            await this.DialogService.OpenAsync<AddToProjectDialog>(parameters, options);
        }

        /// <summary>
        /// Handles the event when a package dependency is added to a project.
        /// </summary>
        /// <param name="result">The result containing the target project name and version constraint.</param>
        public void HandleAddDependency(AddToProjectResult result)
        {
            // Implementation pending future project integration support.
        }

        /// <summary>
        /// Opens the Migrate in Bloom dialog.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OpenMigrateInBloomDialog()
        {
            var onResult = new EventCallbackFactory().Create(this, (MigrateInBloomResult result) => this.HandleMigrateInBloom(result));

            var parameters = new Dictionary<string, object>
            {
                { nameof(MigrateInBloomDialog.Package), this.ViewModel.Package },
                { nameof(MigrateInBloomDialog.PackageVersion), this.ViewModel.SelectedVersion },
                { nameof(MigrateInBloomDialog.OnResult), onResult }
            };

            var options = new DialogOpenOptions
            {
                Title = "Migrate in Bloom",
                Description = $"{this.GetPackageFullName()} · {this.ViewModel.SelectedVersion.GetVersion()}"
            };

            await this.DialogService.OpenAsync<MigrateInBloomDialog>(parameters, options);
        }

        /// <summary>
        /// Handles the event when a package migration in Bloom is initiated.
        /// </summary>
        /// <param name="result">The result containing the target project name and version constraint.</param>
        public void HandleMigrateInBloom(MigrateInBloomResult result)
        {
            this.ViewModel.MigrateInBloom(result);
        }

        /// <summary>
        /// Handles asynchronous initialization of the component and view model state.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await this.ViewModel.InitializeViewModel(this.PackageName, this.Organization);
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
                    Name = $"@{this.ViewModel.Organization.ShortName}",
                    Link = PageRoutes.GetOrganizationRoute(this.ViewModel.Organization.ShortName)
                },
                new BreadcrumbItem
                {
                    Name = this.ViewModel.Package.Name
                }
            ];
        }
    }
}
