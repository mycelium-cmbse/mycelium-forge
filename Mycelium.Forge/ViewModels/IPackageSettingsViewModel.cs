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
    using Mycelium.Forge.Models;

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
        /// Initializes the package settings view model state for the specified package identifier.
        /// </summary>
        /// <param name="id">The unique identifier or name of the package.</param>
        /// <param name="scope">The scope identifier.</param>
        void InitializeViewModel(string id, string scope);

        /// <summary>
        /// Saves the exposed package model state.
        /// </summary>
        void SavePackage();
    }
}
