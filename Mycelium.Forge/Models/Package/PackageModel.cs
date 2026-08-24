// ------------------------------------------------------------------------------------------------
// <copyright file="PackageModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.Package
{
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
        /// <param name="package">The underlying package DTO.</param>
        /// <param name="publisher">The publisher namespace or author handle.</param>
        /// <param name="version">The package release version.</param>
        /// <param name="format">The format name (e.g., SysML v2, CDP4-COMET, Capella).</param>
        /// <param name="description">The package description.</param>
        /// <param name="tags">The tags string.</param>
        /// <param name="importCount">The number of imports.</param>
        /// <param name="isVerified">Whether the publisher is verified.</param>
        /// <param name="lastPublished">The relative elapsed time since the last publish.</param>
        /// <param name="role">The user's role for this package.</param>
        /// <param name="license">The SPDX license identifier of the package.</param>
        /// <param name="href">The relative URL to the package page.</param>
        /// <param name="maintainers">The collection of maintainers for the package.</param>
        /// <param name="versions">The collection of release versions for the package.</param>
        public PackageModel(
            IPackage package,
            string publisher = "",
            string version = "",
            string format = "SysML v2",
            string description = "",
            string tags = "",
            string importCount = "",
            bool isVerified = false,
            string lastPublished = "",
            PackageInvitationKind role = PackageInvitationKind.OWNER,
            string license = "Apache-2.0",
            string href = "",
            IReadOnlyList<PackageMaintainerModel> maintainers = null,
            IReadOnlyList<PackageVersionModel> versions = null)
        {
            this.Package = package;
            this.Publisher = publisher;
            this.Version = version;
            this.Format = format;
            this.Description = description;
            this.Tags = tags;
            this.ImportCount = importCount;
            this.IsVerified = isVerified;
            this.LastPublished = lastPublished;
            this.Role = role;
            this.License = license;

            this.Href = !string.IsNullOrEmpty(href)
                ? href
                : PageRoutes.GetPackageRoute(string.IsNullOrEmpty(publisher) ? "starion" : publisher, package?.ShortName ?? string.Empty);

            this.Maintainers = maintainers ?? [];
            this.Versions = versions ?? [];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageModel" /> class with specified string-based properties.
        /// </summary>
        /// <param name="name">The package name.</param>
        /// <param name="description">The package description.</param>
        /// <param name="format">The format name (e.g., SysML v2, CDP4-COMET, Capella).</param>
        /// <param name="publisher">The publisher namespace or author handle.</param>
        /// <param name="version">The package release version.</param>
        /// <param name="tags">The tags string.</param>
        /// <param name="lastPublished">The relative elapsed time since the last publish.</param>
        /// <param name="importCount">The number of imports.</param>
        /// <param name="isVerified">Whether the publisher is verified.</param>
        /// <param name="role">The user's role for this package.</param>
        /// <param name="license">The SPDX license identifier of the package.</param>
        public PackageModel(
            string name,
            string description = "",
            string format = "SysML v2",
            string publisher = "",
            string version = "",
            string tags = "",
            string lastPublished = "",
            string importCount = "",
            bool isVerified = false,
            PackageInvitationKind role = PackageInvitationKind.OWNER,
            string license = "Apache-2.0")
            : this(new Package { Name = name, ShortName = name.ToLowerInvariant() }, publisher, version, format, description, tags, importCount, isVerified, lastPublished, role, license)
        {
        }

        /// <summary>
        /// Gets or sets the underlying package DTO.
        /// </summary>
        public IPackage Package { get; set; }

        /// <summary>
        /// Gets the package name.
        /// </summary>
        public string Name => this.Package?.Name ?? string.Empty;

        /// <summary>
        /// Gets the package short name handle.
        /// </summary>
        public string ShortName => this.Package?.ShortName ?? string.Empty;

        /// <summary>
        /// Gets the full scoped package identifier.
        /// </summary>
        public string FullName => !string.IsNullOrEmpty(this.Publisher) && !string.IsNullOrEmpty(this.Name)
            ? $"{this.Publisher}/{this.Name}"
            : this.Name ?? string.Empty;

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
        /// Gets or sets the visibility of the package from the underlying package DTO.
        /// </summary>
        public VisibilityKind Visibility => this.Package?.Visibility ?? VisibilityKind.PUBLIC;

        /// <summary>
        /// Gets or sets the relative elapsed time since the last publish.
        /// </summary>
        public string LastPublished { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's role for this package.
        /// </summary>
        public PackageInvitationKind Role { get; set; } = PackageInvitationKind.OWNER;

        /// <summary>
        /// Gets or sets the SPDX license identifier of the package.
        /// </summary>
        public string License { get; set; } = "Apache-2.0";

        /// <summary>
        /// Gets or sets the collection of maintainers for the package.
        /// </summary>
        public IReadOnlyList<PackageMaintainerModel> Maintainers { get; set; } = [];

        /// <summary>
        /// Gets the collection of release versions for the package.
        /// </summary>
        public IReadOnlyList<PackageVersionModel> Versions { get; set; } = [];

        /// <summary>
        /// Gets the default caret-prefixed version constraint expression based on the release version.
        /// </summary>
        /// <returns>The formatted default version constraint string.</returns>
        public string GetDefaultVersionConstraint()
        {
            if (string.IsNullOrWhiteSpace(this.Version))
            {
                return "^1.0.0";
            }

            var cleanVersion = this.Version.TrimStart('v', 'V');
            return $"^{cleanVersion}";
        }
    }
}
