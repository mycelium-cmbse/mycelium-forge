// ------------------------------------------------------------------------------------------------
// <copyright file="MyPackagesViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels
{
    using Mycelium.Forge.Data;
    using Mycelium.Forge.Models;

    /// <summary>
    /// Provides view model state and operations for the My Packages page, listing packages
    /// owned or maintained by the current user across their account and organizations.
    /// </summary>
    public class MyPackagesViewModel : IMyPackagesViewModel
    {
        /// <summary>
        /// Gets or sets the collection of packages owned or maintained by the current user.
        /// </summary>
        public List<PackageModel> Packages { get; set; } = [];

        /// <summary>
        /// Initializes the view model state and populates the packages collection.
        /// </summary>
        public void InitializeViewModel()
        {
            this.Packages = [.. SeedData.MyPackages];
        }
    }
}
