// ------------------------------------------------------------------------------------------------
// <copyright file="PackageModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    using System.Collections.Generic;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Represents a package item displayed in the catalog sections, package lists, and package settings.
    /// </summary>
    public class PackageModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageModel" /> class.
        /// </summary>
        public PackageModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageModel" /> class with specified properties.
        /// </summary>
        /// <param name="name">The package name.</param>
        /// <param name="href">The relative URL to the package page.</param>
        /// <param name="description">The package description.</param>
        /// <param name="format">The format name (e.g., SysML v2, CDP4-COMET, Capella).</param>
        /// <param name="publisher">The publisher namespace or author.</param>
        /// <param name="version">The package release version.</param>
        /// <param name="tags">The tags string.</param>
        /// <param name="importCount">The number of imports.</param>
        /// <param name="isVerified">Whether the publisher is verified.</param>
        /// <param name="visibility">The visibility of the package.</param>
        /// <param name="lastPublished">The relative elapsed time since the last publish.</param>
        /// <param name="role">The user's role for this package (e.g., Owner, Maintainer).</param>
        /// <param name="maintainers">The collection of maintainers for the package.</param>
        /// <param name="versions">The collection of release versions for the package.</param>
        public PackageModel(
            string name,
            string href = "",
            string description = "",
            string format = "",
            string publisher = "",
            string version = "",
            string tags = "",
            string importCount = "",
            bool isVerified = false,
            VisibilityKind visibility = VisibilityKind.PUBLIC,
            string lastPublished = "",
            string role = "Owner",
            IReadOnlyList<PackageMaintainerModel> maintainers = null,
            IReadOnlyList<PackageVersionModel> versions = null)
        {
            this.Name = name;
            this.Href = href;
            this.Description = description;
            this.Format = format;
            this.Publisher = publisher;
            this.Version = version;
            this.Tags = tags;
            this.ImportCount = importCount;
            this.IsVerified = isVerified;
            this.Visibility = visibility;
            this.LastPublished = lastPublished;
            this.Role = role;
            this.Maintainers = maintainers ?? [];
            this.Versions = versions ?? [];
        }

        /// <summary>
        /// Gets or sets the package name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the relative URL to the package page.
        /// </summary>
        public string Href { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the package description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the format name.
        /// </summary>
        public string Format { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the publisher namespace or author.
        /// </summary>
        public string Publisher { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the package release version.
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the tags string.
        /// </summary>
        public string Tags { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the number of imports.
        /// </summary>
        public string ImportCount { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the publisher is verified.
        /// </summary>
        public bool IsVerified { get; set; }

        /// <summary>
        /// Gets or sets the visibility of the package.
        /// </summary>
        public VisibilityKind Visibility { get; set; } = VisibilityKind.PUBLIC;

        /// <summary>
        /// Gets or sets the relative elapsed time since the last publish.
        /// </summary>
        public string LastPublished { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's role for this package (e.g., Owner, Maintainer).
        /// </summary>
        public string Role { get; set; } = "Owner";

        /// <summary>
        /// Gets or sets the collection of maintainers for the package.
        /// </summary>
        public IReadOnlyList<PackageMaintainerModel> Maintainers { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of release versions for the package.
        /// </summary>
        public IReadOnlyList<PackageVersionModel> Versions { get; set; } = [];
    }
}
