// ------------------------------------------------------------------------------------------------
// <copyright file="IHomeViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.Home
{
    using Mycelium.Forge.Models.Package;

    /// <summary>
    /// Defines the view model contract for the Mycelium Forge home landing page.
    /// </summary>
    public interface IHomeViewModel
    {
        /// <summary>
        /// Gets or sets the total published package count displayed in the hero section.
        /// </summary>
        string PackageCount { get; set; }

        /// <summary>
        /// Gets or sets the total package version count displayed in the hero section.
        /// </summary>
        string VersionCount { get; set; }

        /// <summary>
        /// Gets or sets the total registered publisher count displayed in the hero section.
        /// </summary>
        string PublisherCount { get; set; }

        /// <summary>
        /// Gets or sets the total package import count displayed in the hero section.
        /// </summary>
        string ImportCount { get; set; }

        /// <summary>
        /// Gets or sets the standard library package models.
        /// </summary>
        List<PackageModel> StandardLibraries { get; set; }

        /// <summary>
        /// Gets or sets the recently updated package models.
        /// </summary>
        List<PackageModel> RecentlyUpdated { get; set; }

        /// <summary>
        /// Gets or sets the most used package models.
        /// </summary>
        List<PackageModel> MostUsed { get; set; }

        /// <summary>
        /// Gets or sets the package models from other MBSE tools.
        /// </summary>
        List<PackageModel> ModelsFromOtherMbseTools { get; set; }

        /// <summary>
        /// Initializes the view model state and populates the package catalog collections.
        /// </summary>
        void InitializeViewModel();
    }
}
