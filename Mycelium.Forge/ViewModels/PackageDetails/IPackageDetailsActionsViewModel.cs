// ------------------------------------------------------------------------------------------------
// <copyright file="IPackageDetailsActionsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.PackageDetails
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Models.DialogResults;

    /// <summary>
    /// Defines the view model contract for interactive package details actions and install commands.
    /// </summary>
    public interface IPackageDetailsActionsViewModel
    {
        /// <summary>
        /// Gets or sets the underlying package DTO.
        /// </summary>
        IPackage Package { get; set; }

        /// <summary>
        /// Gets or sets the owning organization DTO.
        /// </summary>
        IOrganization Organization { get; set; }

        /// <summary>
        /// Gets or sets the selected package version DTO.
        /// </summary>
        IPackageVersion SelectedVersion { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of install commands.
        /// </summary>
        IReadOnlyDictionary<string, string> InstallCommands { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user is an administrator of the package.
        /// </summary>
        bool IsUserAdmin { get; set; }

        /// <summary>
        /// Initializes the package actions view model state with provided package details.
        /// </summary>
        /// <param name="package">The package instance.</param>
        /// <param name="organization">The owning organization instance.</param>
        /// <param name="selectedVersion">The currently selected package version.</param>
        /// <param name="isUserAdmin">A value indicating whether the current user is an administrator.</param>
        void Initialize(IPackage package, IOrganization organization, IPackageVersion selectedVersion, bool isUserAdmin);

        /// <summary>
        /// Initiates a migration of the package in Bloom to the specified target project.
        /// </summary>
        /// <param name="result">The migration parameters including destination project and version constraint.</param>
        void MigrateInBloom(MigrateInBloomResult result);
    }
}
