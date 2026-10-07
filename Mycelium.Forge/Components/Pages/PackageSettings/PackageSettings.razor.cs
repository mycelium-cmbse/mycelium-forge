// ------------------------------------------------------------------------------------------------
// <copyright file="PackageSettings.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.PackageSettings
{
    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.PackageSettings.Dialogs;
    using Mycelium.Forge.Models.Common;
    using Mycelium.Forge.ViewModels.PackageSettings;

    /// <summary>
    /// Represents the package settings and governance management view of the Mycelium Forge registry.
    /// </summary>
    public partial class PackageSettings : ComponentBase
    {
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
        /// Gets or sets the view model for the package settings page.
        /// </summary>
        [Inject]
        public IPackageSettingsViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the dialog service used to display modal dialogs.
        /// </summary>
        [Inject]
        public DialogService DialogService { get; set; }

        /// <summary>
        /// Gets or sets the navigation manager instance.
        /// </summary>
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Handles component parameter updates and initializes the view model with the route parameters asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            await this.ViewModel.InitializeViewModel(this.PackageName, this.Scope);
        }

        /// <summary>
        /// Gets the breadcrumb navigation items for the package settings page.
        /// </summary>
        /// <returns>A collection of <see cref="BreadcrumbItem" /> entries representing the trail.</returns>
        private IEnumerable<BreadcrumbItem> GetBreadcrumbItems()
        {
            var publisher = this.ViewModel.Owner.ShortName;
            var packageName = this.ViewModel.Package.Name;

            return
            [
                new BreadcrumbItem
                {
                    Name = "Search",
                    Link = PageRoutes.Packages
                },
                new BreadcrumbItem
                {
                    Name = $"@{publisher}",
                    Link = PageRoutes.GetOrganizationRoute(publisher)
                },
                new BreadcrumbItem
                {
                    Name = packageName,
                    Link = PageRoutes.GetPackageRoute(publisher, packageName)
                },
                new BreadcrumbItem
                {
                    Name = "Settings"
                }
            ];
        }

        /// <summary>
        /// Handles the action to add a new maintainer to the package.
        /// </summary>
        private static void OnAddMaintainer()
        {
            // TODO: Implement add maintainer logic.
        }

        /// <summary>
        /// Handles opening the options menu for the specified maintainer.
        /// </summary>
        /// <param name="maintainer">The target maintainer account.</param>
        private static void OnMaintainerMenu(IAccount maintainer)
        {
            // TODO: Implement maintainer options menu logic.
        }

        /// <summary>
        /// Selects the specified visibility option for the package and saves the update asynchronously.
        /// </summary>
        /// <param name="visibility">The visibility kind to set.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnSelectVisibility(VisibilityKind visibility)
        {
            await this.ViewModel.SetVisibility(visibility);
        }

        /// <summary>
        /// Prompts confirmation to unlist the specified package version.
        /// </summary>
        /// <param name="version">The package version to unlist.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnUnlistVersion(IPackageVersion version)
        {
            var onResult = new EventCallbackFactory().Create(this, async () => { await this.ViewModel.UnlistVersion(version); });

            var parameters = new Dictionary<string, object>
            {
                { nameof(ConfirmPackageActionDialog.PackageName), this.ViewModel.Package.Name },
                { nameof(ConfirmPackageActionDialog.Title), $"Unlist version {version.Version}" },
                { nameof(ConfirmPackageActionDialog.Description), $"Unlisting version {version.Version} hides it from search and discovery while keeping existing installations working." },
                { nameof(ConfirmPackageActionDialog.ActionButtonText), "Unlist version" },
                { nameof(ConfirmPackageActionDialog.OnResult), onResult }
            };

            var options = new DialogOpenOptions
            {
                Title = $"Unlist version {version.Version}"
            };

            await this.DialogService.OpenAsync<ConfirmPackageActionDialog>(parameters, options);
        }

        /// <summary>
        /// Relists the specified unlisted package version asynchronously.
        /// </summary>
        /// <param name="version">The package version to relist.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnRelistVersion(IPackageVersion version)
        {
            await this.ViewModel.RelistVersion(version);
        }

        /// <summary>
        /// Deprecates the specified package version asynchronously.
        /// </summary>
        /// <param name="version">The package version to deprecate.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnDeprecateVersion(IPackageVersion version)
        {
            await this.ViewModel.DeprecateVersion(version);
        }

        /// <summary>
        /// Opens the confirmation dialog to transfer package ownership.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnTransferOwnership()
        {
            var onResult = new EventCallbackFactory().Create(this, async (string targetScope) =>
            {
                var result = await this.ViewModel.TransferOwnership(targetScope);

                if (!result.IsError)
                {
                    this.NavigationManager.NavigateTo(PageRoutes.GetPackageRoute(targetScope, this.ViewModel.Package.ShortName), true);
                }
            });

            var parameters = new Dictionary<string, object>
            {
                { nameof(TransferPackageOwnershipDialog.PackageName), this.ViewModel.Package.Name },
                { nameof(TransferPackageOwnershipDialog.CurrentOwner), this.ViewModel.Owner.ShortName },
                { nameof(TransferPackageOwnershipDialog.OnResult), onResult }
            };

            var options = new DialogOpenOptions
            {
                Title = "Transfer package ownership"
            };

            await this.DialogService.OpenAsync<TransferPackageOwnershipDialog>(parameters, options);
        }

        /// <summary>
        /// Opens the confirmation dialog to permanently delete the package.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnDeletePackage()
        {
            var onResult = new EventCallbackFactory().Create(this, async () =>
            {
                var result = await this.ViewModel.DeletePackage();

                if (!result.IsError)
                {
                    this.NavigationManager.NavigateTo(PageRoutes.Packages, replace: true);
                }
            });

            var parameters = new Dictionary<string, object>
            {
                { nameof(ConfirmPackageActionDialog.PackageName), this.ViewModel.Package.Name },
                { nameof(ConfirmPackageActionDialog.Title), "Delete package" },
                { nameof(ConfirmPackageActionDialog.Description), "This will permanently delete the package and all its versions. Existing consumers will break." },
                { nameof(ConfirmPackageActionDialog.ActionButtonText), "Delete package" },
                { nameof(ConfirmPackageActionDialog.OnResult), onResult }
            };

            var options = new DialogOpenOptions
            {
                Title = "Delete package"
            };

            await this.DialogService.OpenAsync<ConfirmPackageActionDialog>(parameters, options);
        }

        /// <summary>
        /// Determines whether the specified version is the latest published release.
        /// </summary>
        /// <param name="version">The package version to evaluate.</param>
        /// <returns><c>true</c> if the version is the newest release; otherwise, <c>false</c>.</returns>
        private bool IsLatestVersion(IPackageVersion version)
        {
            return this.ViewModel.Versions.Count > 0 && this.ViewModel.Versions[0].Id == version.Id;
        }
    }
}
