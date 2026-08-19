// ------------------------------------------------------------------------------------------------
// <copyright file="IPackageViewModel.cs" company="Starion Group S.A.">
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
    /// Defines the view model contract for the Mycelium Forge package details page.
    /// </summary>
    public interface IPackageViewModel
    {
        /// <summary>
        /// Gets or sets the package details and metadata.
        /// </summary>
        PackageDetailsModel Package { get; set; }

        /// <summary>
        /// Initializes the package view model state for the specified package identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the package.</param>
        void InitializeViewModel(Guid id);
    }
}
