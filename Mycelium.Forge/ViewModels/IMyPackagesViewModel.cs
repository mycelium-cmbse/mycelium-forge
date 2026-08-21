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
    using Mycelium.Forge.Models.Package;

    /// <summary>
    /// Defines the view model contract for the My Packages page.
    /// </summary>
    public interface IMyPackagesViewModel
    {
        /// <summary>
        /// Gets or sets the collection of packages owned or maintained by the current user.
        /// </summary>
        List<PackageModel> Packages { get; set; }

        /// <summary>
        /// Initializes the view model state and populates the packages collection.
        /// </summary>
        void InitializeViewModel();
    }
}
