// ------------------------------------------------------------------------------------------------
// <copyright file="PackageRowModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents a package search result row displayed in the packages discovery view.
    /// </summary>
    public class PackageRowModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageRowModel" /> class.
        /// </summary>
        public PackageRowModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageRowModel" /> class with specified properties.
        /// </summary>
        /// <param name="name">The package name.</param>
        /// <param name="href">The relative URL to the package details page.</param>
        /// <param name="description">The package summary description.</param>
        /// <param name="format">The format name (e.g., SysML v2, CDP4-COMET, Capella).</param>
        /// <param name="publisher">The publisher namespace or username.</param>
        /// <param name="version">The package release version.</param>
        /// <param name="tags">The tags associated with the package.</param>
        /// <param name="updatedAgo">The relative elapsed time since the last update.</param>
        /// <param name="importCount">The number of imports or downloads.</param>
        /// <param name="isVerified">A value indicating whether the publisher is verified.</param>
        public PackageRowModel(
            string name,
            string href,
            string description,
            string format,
            string publisher,
            string version,
            string tags,
            string updatedAgo,
            string importCount,
            bool isVerified = false)
        {
            this.Name = name;
            this.Href = href;
            this.Description = description;
            this.Format = format;
            this.Publisher = publisher;
            this.Version = version;
            this.Tags = tags;
            this.UpdatedAgo = updatedAgo;
            this.ImportCount = importCount;
            this.IsVerified = isVerified;
        }

        /// <summary>
        /// Gets or sets the package name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the relative URL to the package details page.
        /// </summary>
        public string Href { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the package summary description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the format name (e.g., SysML v2, CDP4-COMET, Capella).
        /// </summary>
        public string Format { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the publisher namespace or username.
        /// </summary>
        public string Publisher { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the package release version.
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the tags associated with the package.
        /// </summary>
        public string Tags { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the relative elapsed time since the last update.
        /// </summary>
        public string UpdatedAgo { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the number of imports or downloads.
        /// </summary>
        public string ImportCount { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the publisher is verified.
        /// </summary>
        public bool IsVerified { get; set; }
    }
}
