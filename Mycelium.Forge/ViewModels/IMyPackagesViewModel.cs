// ------------------------------------------------------------------------------------------------
// <copyright file="IMyPackagesViewModel.cs" company="Starion Group S.A.">
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
    /// Defines the view model contract for the My Packages page, which lists packages
    /// owned or maintained by the current user across their account and organizations.
    /// </summary>
    public interface IMyPackagesViewModel
    {
        /// <summary>
        /// Gets or sets the collection of package entries to display in the table.
        /// </summary>
        IReadOnlyList<PackageModel> Packages { get; set; }

        /// <summary>
        /// Initializes the view model state and populates the package collection.
        /// </summary>
        void InitializeViewModel();
    }
}