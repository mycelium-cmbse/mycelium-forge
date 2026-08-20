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
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Models;
    using Mycelium.Forge.ViewModels;

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
        /// Gets or sets the package identifier supplied from the URL route.
        /// </summary>
        [Parameter]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the view model for the package settings page.
        /// </summary>
        [Inject]
        public IPackageSettingsViewModel ViewModel { get; set; }

        /// <summary>
        /// Handles the action to add a new maintainer to the package.
        /// </summary>
        public void OnAddMaintainer()
        {
        }

        /// <summary>
        /// Handles opening the options menu for the specified maintainer.
        /// </summary>
        /// <param name="maintainer">The target maintainer model.</param>
        public void OnMaintainerMenu(PackageMaintainerModel maintainer)
        {
        }

        /// <summary>
        /// Selects the specified visibility option for the package and saves the model.
        /// </summary>
        /// <param name="visibility">The visibility kind to set.</param>
        public void OnSelectVisibility(VisibilityKind visibility)
        {
            if (this.ViewModel.Package == null)
            {
                return;
            }

            this.ViewModel.Package.Visibility = visibility;
            this.ViewModel.SavePackage();
        }

        /// <summary>
        /// Unlists the specified package version and saves the model.
        /// </summary>
        /// <param name="version">The package version model to unlist.</param>
        public void OnUnlistVersion(PackageVersionModel version)
        {
            if (version == null)
            {
                return;
            }

            version.IsUnlisted = true;
            this.ViewModel.SavePackage();
        }

        /// <summary>
        /// Relists the specified unlisted package version and saves the model.
        /// </summary>
        /// <param name="version">The package version model to relist.</param>
        public void OnRelistVersion(PackageVersionModel version)
        {
            if (version == null)
            {
                return;
            }

            version.IsUnlisted = false;
            this.ViewModel.SavePackage();
        }

        /// <summary>
        /// Deprecates the specified package version and saves the model.
        /// </summary>
        /// <param name="version">The package version model to deprecate.</param>
        public void OnDeprecateVersion(PackageVersionModel version)
        {
            if (version == null)
            {
                return;
            }

            version.IsDeprecated = true;
            this.ViewModel.SavePackage();
        }

        /// <summary>
        /// Handles the action to transfer ownership of the package.
        /// </summary>
        public void OnTransferOwnership()
        {
        }

        /// <summary>
        /// Handles component parameter updates and initializes the view model with the route parameters.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            this.ViewModel.InitializeViewModel(this.Id, this.Scope);
        }
    }
}
