// ------------------------------------------------------------------------------------------------
// <copyright file="HomeViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.Home
{
    using Mycelium.Forge.Data;
    using Mycelium.Forge.Models.Package;

    /// <summary>
    /// Provides view model state and operations for the Mycelium Forge home landing page.
    /// </summary>
    public class HomeViewModel : IHomeViewModel
    {
        /// <summary>
        /// Gets or sets the total published package count displayed in the hero section.
        /// </summary>
        public string PackageCount { get; set; } = "42";

        /// <summary>
        /// Gets or sets the total package version count displayed in the hero section.
        /// </summary>
        public string VersionCount { get; set; } = "128";

        /// <summary>
        /// Gets or sets the total registered publisher count displayed in the hero section.
        /// </summary>
        public string PublisherCount { get; set; } = "6";

        /// <summary>
        /// Gets or sets the total package import count displayed in the hero section.
        /// </summary>
        public string ImportCount { get; set; } = "2,582";

        /// <summary>
        /// Gets or sets the standard library package models.
        /// </summary>
        public List<PackageModel> StandardLibraries { get; set; } = [];

        /// <summary>
        /// Gets or sets the recently updated package models.
        /// </summary>
        public List<PackageModel> RecentlyUpdated { get; set; } = [];

        /// <summary>
        /// Gets or sets the most used package models.
        /// </summary>
        public List<PackageModel> MostUsed { get; set; } = [];

        /// <summary>
        /// Gets or sets the package models from other MBSE tools.
        /// </summary>
        public List<PackageModel> ModelsFromOtherMbseTools { get; set; } = [];

        /// <summary>
        /// Initializes the view model state and populates the package catalog collections.
        /// </summary>
        public void InitializeViewModel()
        {
            this.StandardLibraries = [.. SeedData.StandardLibraryPackages];
            this.RecentlyUpdated = [.. SeedData.RecentlyUpdatedPackages];
            this.MostUsed = [.. SeedData.MostUsedPackages];
            this.ModelsFromOtherMbseTools = [.. SeedData.ModelsFromOtherMbseTools];
        }
    }
}
