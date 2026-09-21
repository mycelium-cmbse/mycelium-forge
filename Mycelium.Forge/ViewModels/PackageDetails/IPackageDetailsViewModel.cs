// ------------------------------------------------------------------------------------------------
// <copyright file="IPackageDetailsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.PackageDetails
{
    using Mycelium.Forge.Common;

    /// <summary>
    /// Defines the view model contract for the Mycelium Forge package details page.
    /// </summary>
    public interface IPackageDetailsViewModel
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
        /// Gets or sets the package type DTO.
        /// </summary>
        IPackageType PackageType { get; set; }

        /// <summary>
        /// Gets or sets the collection of released package version DTOs.
        /// </summary>
        IReadOnlyList<IPackageVersion> Versions { get; set; }

        /// <summary>
        /// Gets or sets the collection of maintainer account DTOs.
        /// </summary>
        IReadOnlyList<IAccount> Maintainers { get; set; }

        /// <summary>
        /// Gets or sets the selected or latest package version DTO.
        /// </summary>
        IPackageVersion SelectedVersion { get; set; }

        /// <summary>
        /// Gets or sets the collection of package metadata DTOs corresponding to package versions.
        /// </summary>
        IReadOnlyList<IPackageMetaData> MetaDatas { get; set; }

        /// <summary>
        /// Gets the package metadata DTO for the currently selected package version.
        /// </summary>
        IPackageMetaData CurrentMetaData { get; }

        /// <summary>
        /// Gets the collection of model elements contained within the package release.
        /// </summary>
        IReadOnlyList<(string Name, string Kind, string Category, string AttributeSummary)> Elements { get; }

        /// <summary>
        /// Gets the collection of direct package dependencies resolved from the usage projection.
        /// </summary>
        IReadOnlyList<(string Name, string Summary, bool IsVerified)> Dependencies { get; }

        /// <summary>
        /// Gets the collection of packages and projects depending on this package.
        /// </summary>
        IReadOnlyList<(string Name, string Summary, bool IsProject, bool IsVerified)> Dependents { get; }

        /// <summary>
        /// Gets the collection of quality evaluation checks.
        /// </summary>
        IReadOnlyList<(string Title, string Detail, bool Passed)> QualityChecks { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user is an administrator of the package.
        /// </summary>
        bool IsUserAdmin { get; set; }

        /// <summary>
        /// Initializes the package view model state for the specified package name and organization asynchronously.
        /// </summary>
        /// <param name="packageName">The name of the package.</param>
        /// <param name="organization">The organization of the package.</param>
        /// <param name="tab">The optional content tab identifier.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous initialization.</returns>
        Task InitializeViewModel(string packageName, string organization, string tab = null);
    }
}
