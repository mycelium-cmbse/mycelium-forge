// ------------------------------------------------------------------------------------------------
// <copyright file="IPackageSettingsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.PackageSettings
{
    using ErrorOr;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Defines the view model contract for the Mycelium Forge package settings page.
    /// </summary>
    public interface IPackageSettingsViewModel
    {
        /// <summary>
        /// Gets or sets the underlying package DTO.
        /// </summary>
        IPackage Package { get; set; }

        /// <summary>
        /// Gets or sets the owning scope DTO.
        /// </summary>
        IScope Owner { get; set; }

        /// <summary>
        /// Gets or sets the collection of maintainer accounts for the package.
        /// </summary>
        IReadOnlyList<IAccount> Maintainers { get; set; }

        /// <summary>
        /// Gets or sets the collection of owner accounts for the package.
        /// </summary>
        IReadOnlyList<IAccount> Owners { get; set; }

        /// <summary>
        /// Gets or sets the collection of released package versions.
        /// </summary>
        IReadOnlyList<IPackageVersion> Versions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user is allowed to manage the package.
        /// </summary>
        bool CanManagePackage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user is allowed to delete the package.
        /// </summary>
        bool CanDeletePackage { get; set; }

        /// <summary>
        /// Initializes the package settings view model state for the specified package name and scope asynchronously.
        /// </summary>
        /// <param name="packageName">The name of the package.</param>
        /// <param name="scope">The owning scope or publisher identifier.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous initialization.</returns>
        Task InitializeViewModel(string packageName, string scope);

        /// <summary>
        /// Updates the package visibility setting asynchronously.
        /// </summary>
        /// <param name="visibility">The target visibility kind.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task SetVisibility(VisibilityKind visibility, CancellationToken token = default);

        /// <summary>
        /// Unlists the specified package version asynchronously.
        /// </summary>
        /// <param name="version">The package version to unlist.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task UnlistVersion(IPackageVersion version, CancellationToken token = default);

        /// <summary>
        /// Relists the specified unlisted package version asynchronously.
        /// </summary>
        /// <param name="version">The package version to relist.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task RelistVersion(IPackageVersion version, CancellationToken token = default);

        /// <summary>
        /// Marks the specified package version as deprecated asynchronously.
        /// </summary>
        /// <param name="version">The package version to deprecate.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task DeprecateVersion(IPackageVersion version, CancellationToken token = default);

        /// <summary>
        /// Transfers package ownership to the specified recipient scope asynchronously.
        /// </summary>
        /// <param name="targetScope">The target scope short name.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A task indicating the success or failure of the operation.</returns>
        Task<ErrorOr<Success>> TransferOwnership(string targetScope, CancellationToken token = default);

        /// <summary>
        /// Deletes the package asynchronously.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A task indicating the success or failure of the operation.</returns>
        Task<ErrorOr<Success>> DeletePackage(CancellationToken token = default);
    }
}
