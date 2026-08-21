// ------------------------------------------------------------------------------------------------
// <copyright file="IPackageSettingsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels
{
    using Mycelium.Forge.Models.Package;

    /// <summary>
    /// Defines the view model contract for the Mycelium Forge package settings page.
    /// </summary>
    public interface IPackageSettingsViewModel
    {
        /// <summary>
        /// Gets or sets the package model.
        /// </summary>
        PackageModel Package { get; set; }

        /// <summary>
        /// Initializes the package settings view model state for the specified package name and organization.
        /// </summary>
        /// <param name="packageName">The name of the package.</param>
        /// <param name="organization">The organization identifier.</param>
        void InitializeViewModel(string packageName, string organization);

        /// <summary>
        /// Saves the exposed package model state.
        /// </summary>
        void SavePackage();
    }
}
