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
    using Mycelium.Forge.Data;
    using Mycelium.Forge.Extensions;

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
        /// <param name="tags">The tags string.</param>
        /// <param name="downloadCount">The number of downloads.</param>
        /// <param name="dependentsCount">The number of dependents.</param>
        public PackageModel(
            IPackage package,
            string publisher = "",
            string version = "",
            string format = PackageFormatConstants.SysMlV2,
            string tags = "",
            int downloadCount = 0,
            int dependentsCount = 0)
        {
            this.Package = package;
            this.Publisher = publisher;
            this.Version = version;
            this.Format = format;
            this.Tags = tags;
            this.DownloadCount = downloadCount;
            this.DependentsCount = dependentsCount;
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
        /// Gets the full scoped package identifier.
        /// </summary>
        public string FullName => !string.IsNullOrEmpty(this.Publisher) && !string.IsNullOrEmpty(this.Name)
            ? $"{this.Publisher}/{this.Name}"
            : this.Name ?? string.Empty;

        /// <summary>
        /// Gets or sets the package description.
        /// </summary>
        public string Description => this.Package?.Description ?? string.Empty;

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
        /// Gets or sets the number of downloads.
        /// </summary>
        public int DownloadCount { get; set; }

        /// <summary>
        /// Gets or sets the number of dependents.
        /// </summary>
        public int DependentsCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the publisher is verified.
        /// </summary>
        public bool IsVerified { get; set; }

        /// <summary>
        /// Gets or sets the visibility of the package from the underlying package DTO.
        /// </summary>
        public VisibilityKind Visibility => this.Package?.Visibility ?? VisibilityKind.PUBLIC;

        /// <summary>
        /// Gets the relative elapsed time since the last publish.
        /// </summary>
        public string LastPublished => this.Package != null && this.Package.CreatedAt != default
            ? this.Package.CreatedAt.ToTimeAgo()
            : string.Empty;

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

        /// <summary>
        /// Creates a <see cref="PackageModel" /> by resolving publisher, version, and format information from seed data.
        /// </summary>
        /// <param name="package">The underlying package DTO.</param>
        /// <returns>A new <see cref="PackageModel" />.</returns>
        public static PackageModel FromPackage(IPackage package)
        {
            var publisher = SeedData.Organizations.FirstOrDefault(o => o.Id == package.Owner);
            var publisherScope = publisher != null ? $"@{publisher.ShortName}" : "@starion";
            var version = SeedData.PackageVersions.FirstOrDefault(v => v.Owner == package.Id);
            var versionString = version?.Version ?? "v1.0.0";
            var packageType = SeedData.PackageTypes.FirstOrDefault(t => t.Id == package.PackageType);
            var format = packageType?.Name ?? PackageFormatConstants.SysMlV2;
            var downloadCount = version?.DownloadCount ?? 0;

            var tags = package.ShortName switch
            {
                "sysmlv2-isq-quantities" => "standard-library · units · quantities · isq",
                "sysmlv2-kernel-library" => "standard-library · kerml · kernel",
                "ecss-e-st-10-04c" => "standard-library · space-environment · ecss",
                "ecss-mm-pwr" => "mission-model · power · ecss",
                "smallsat-platform-model" => "mission-model · smallsat · platform",
                "ecss-e-st-32-10c" => "comms · rf · telemetry · ecss",
                "cdp4-comet-core" => "concurrent-design · cdp4 · ecss-10-25",
                "capella-system-template" => "arcadia · capella · operational-analysis",
                "ecss-e-st-31-01c" => "mechanical · structures · loads · ecss",
                _ => "standard-library"
            };

            var dependentsCount = package.ShortName switch
            {
                "sysmlv2-isq-quantities" => 12,
                "sysmlv2-kernel-library" => 18,
                "ecss-e-st-10-04c" => 5,
                "ecss-mm-pwr" => 2,
                "smallsat-platform-model" => 1,
                "ecss-e-st-32-10c" => 3,
                "cdp4-comet-core" => 4,
                "capella-system-template" => 2,
                "ecss-e-st-31-01c" => 3,
                _ => 0
            };

            return new PackageModel(
                package,
                publisherScope,
                versionString,
                format,
                tags,
                downloadCount,
                dependentsCount)
            {
                IsVerified = true
            };
        }
    }
}
