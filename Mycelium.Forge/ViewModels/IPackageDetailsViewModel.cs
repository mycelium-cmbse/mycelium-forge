// ------------------------------------------------------------------------------------------------
// <copyright file="IPackageDetailsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels
{
    using FluentResults;

    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.Models.Package;

    /// <summary>
    /// Defines the view model contract for the Mycelium Forge package details page.
    /// </summary>
    public interface IPackageDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the package details and metadata.
        /// </summary>
        PackageDetailsModel Package { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user is an administrator of the package.
        /// </summary>
        bool IsUserAdmin { get; set; }

        /// <summary>
        /// Initializes the package view model state for the specified package name and organization.
        /// </summary>
        /// <param name="packageName">The name of the package.</param>
        /// <param name="organization">The organization of the package.</param>
        void InitializeViewModel(string packageName, string organization);

        /// <summary>
        /// Initiates a migration of the package in Bloom to the specified target project.
        /// </summary>
        /// <param name="result">The migration parameters including destination project and version constraint.</param>
        /// <returns>A <see cref="Result" /> indicating the success or failure of the migration initiation.</returns>
        Result MigrateInBloom(MigrateInBloomResult result);
    }
}
