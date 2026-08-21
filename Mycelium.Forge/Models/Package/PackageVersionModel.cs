// ------------------------------------------------------------------------------------------------
// <copyright file="PackageVersionModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.Package
{
    /// <summary>
    /// Represents an immutable release version entry for a package in the version history.
    /// </summary>
    public class PackageVersionModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageVersionModel" /> class.
        /// </summary>
        public PackageVersionModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageVersionModel" /> class with specified properties.
        /// </summary>
        /// <param name="version">The semver version string.</param>
        /// <param name="publishedAgo">The elapsed time string since publication.</param>
        /// <param name="dependentCount">The number of dependent packages or projects using this version.</param>
        /// <param name="isValidated">A value indicating whether the release passed validation checks.</param>
        /// <param name="size">The formatted package archive size.</param>
        /// <param name="isLatest">A value indicating whether the version is the latest release.</param>
        /// <param name="isUnlisted">A value indicating whether the package version is unlisted.</param>
        /// <param name="isDeprecated">A value indicating whether the package version is deprecated.</param>
        /// <param name="downloadUrl">The direct download URL for the package version artifact.</param>
        public PackageVersionModel(
            string version,
            string publishedAgo = "",
            int dependentCount = 0,
            bool isValidated = false,
            string size = "",
            bool isLatest = false,
            bool isUnlisted = false,
            bool isDeprecated = false,
            string downloadUrl = "#")
        {
            this.Version = version;
            this.PublishedAgo = publishedAgo;
            this.DependentCount = dependentCount;
            this.IsValidated = isValidated;
            this.Size = size;
            this.IsLatest = isLatest;
            this.IsUnlisted = isUnlisted;
            this.IsDeprecated = isDeprecated;
            this.DownloadUrl = downloadUrl;
        }

        /// <summary>
        /// Gets or sets the semver version string.
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the elapsed time string since publication.
        /// </summary>
        public string PublishedAgo { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the number of dependent packages or projects using this version.
        /// </summary>
        public int DependentCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the release passed validation checks.
        /// </summary>
        public bool IsValidated { get; set; }

        /// <summary>
        /// Gets or sets the formatted package archive size.
        /// </summary>
        public string Size { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the version is the latest release.
        /// </summary>
        public bool IsLatest { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the package version is unlisted.
        /// </summary>
        public bool IsUnlisted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the package version is deprecated.
        /// </summary>
        public bool IsDeprecated { get; set; }

        /// <summary>
        /// Gets or sets the direct download URL for the package version artifact.
        /// </summary>
        public string DownloadUrl { get; set; } = "#";
    }
}
