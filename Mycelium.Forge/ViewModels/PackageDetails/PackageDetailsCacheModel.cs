// ------------------------------------------------------------------------------------------------
// <copyright file="PackageDetailsCacheModel.cs" company="Starion Group S.A.">
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
    /// Represents the cached package details bundle used across package tab navigations.
    /// </summary>
    public class PackageDetailsCacheModel
    {
        /// <summary>
        /// Gets or sets the underlying package DTO.
        /// </summary>
        public IPackage Package { get; set; }

        /// <summary>
        /// Gets or sets the owning organization DTO.
        /// </summary>
        public IOrganization Organization { get; set; }

        /// <summary>
        /// Gets or sets the package type DTO.
        /// </summary>
        public IPackageType PackageType { get; set; }

        /// <summary>
        /// Gets or sets the collection of released package version DTOs.
        /// </summary>
        public IReadOnlyList<IPackageVersion> Versions { get; set; } = [];

        /// <summary>
        /// Gets or sets the selected or latest package version DTO.
        /// </summary>
        public IPackageVersion SelectedVersion { get; set; }

        /// <summary>
        /// Gets or sets the collection of maintainer account DTOs.
        /// </summary>
        public IReadOnlyList<IAccount> Maintainers { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of package metadata DTOs corresponding to package versions.
        /// </summary>
        public IReadOnlyList<IPackageMetaData> MetaDatas { get; set; } = [];
    }
}
