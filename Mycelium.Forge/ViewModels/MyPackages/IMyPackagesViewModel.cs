// ------------------------------------------------------------------------------------------------
// <copyright file="IMyPackagesViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.MyPackages
{
    using Mycelium.Forge.Common;

    /// <summary>
    /// Defines the view model contract for the My Packages page.
    /// </summary>
    public interface IMyPackagesViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether the view model is currently loading data.
        /// </summary>
        bool IsLoading { get; set; }

        /// <summary>
        /// Gets or sets the collection of packages owned or maintained by the current user.
        /// </summary>
        List<IPackage> Packages { get; set; }

        /// <summary>
        /// Initializes the view model state and populates the packages collection.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task InitializeViewModel();

        /// <summary>
        /// Gets the publisher string formatted with leading '@' for the specified package.
        /// </summary>
        /// <param name="package">The package DTO.</param>
        /// <returns>The publisher handle string.</returns>
        string GetPublisher(IPackage package);

        /// <summary>
        /// Gets the latest listed release version string for the specified package.
        /// </summary>
        /// <param name="package">The package DTO.</param>
        /// <returns>The latest release version.</returns>
        IPackageVersion GetLatestVersion(IPackage package);

        /// <summary>
        /// Gets the role of the current user for the specified package.
        /// </summary>
        /// <param name="package">The package DTO.</param>
        /// <returns>The <see cref="PackageInvitationKind" /> role value.</returns>
        PackageInvitationKind GetRole(IPackage package);
    }
}
