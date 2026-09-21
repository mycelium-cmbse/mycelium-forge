// ------------------------------------------------------------------------------------------------
// <copyright file="PackageRowViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.Rows
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extensions;

    /// <summary>
    /// Represents the display data for a single package version row or card in the catalog sections.
    /// </summary>
    public sealed class PackageRowViewModel
    {
        /// <summary>
        /// Constant representing the default starion publisher namespace.
        /// </summary>
        private const string DefaultPublisher = "@starion";

        /// <summary>
        /// Constant representing the default package version string when no versions are available.
        /// </summary>
        private const string DefaultVersion = "1.0.0";

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageRowViewModel" /> class.
        /// </summary>
        private PackageRowViewModel()
        {
        }

        /// <summary>
        /// Gets the package name.
        /// </summary>
        public string Name { get; private init; } = string.Empty;

        /// <summary>
        /// Gets the package short name (slug).
        /// </summary>
        public string ShortName { get; private init; } = string.Empty;

        /// <summary>
        /// Gets the full scoped package identifier in the form '{Publisher}/{Name}', or just the name if either part is empty.
        /// </summary>
        public string FullName => !string.IsNullOrEmpty(this.Publisher) && !string.IsNullOrEmpty(this.Name)
            ? $"{this.Publisher}/{this.Name}"
            : this.Name;

        /// <summary>
        /// Gets the package description.
        /// </summary>
        public string Description { get; private init; } = string.Empty;

        /// <summary>
        /// Gets the publisher namespace or author handle (e.g. '@omg').
        /// </summary>
        public string Publisher { get; private init; } = string.Empty;

        /// <summary>
        /// Gets the package release version string.
        /// </summary>
        public string Version { get; private init; } = string.Empty;

        /// <summary>
        /// Gets the format name (e.g. 'SysML v2', 'Cdp4Comet', 'Capella').
        /// </summary>
        public string Format { get; private init; } = string.Empty;

        /// <summary>
        /// Gets the tags string for the package.
        /// </summary>
        public string Tags { get; private init; } = string.Empty;

        /// <summary>
        /// Gets the number of downloads for this version.
        /// </summary>
        public int DownloadCount { get; private init; }

        /// <summary>
        /// Gets the number of packages that depend on this package.
        /// </summary>
        public int DependentsCount { get; private init; }

        /// <summary>
        /// Gets a value indicating whether the publisher is verified.
        /// </summary>
        public bool IsVerified { get; private init; }

        /// <summary>
        /// Gets the relative elapsed time since the package was created, formatted as a human-readable string.
        /// Gets the relative elapsed time since the latest listed package version was published, formatted as a human-readable
        /// string.
        /// </summary>
        public string LastPublished { get; private init; } = string.Empty;

        /// <summary>
        /// Gets the UTC creation timestamp of the package.
        /// </summary>
        public DateTime CreatedAt { get; private init; }

        /// <summary>
        /// Gets the publication timestamp of the latest listed package version.
        /// </summary>
        public DateTime LatestPublicationDate { get; private init; }

        /// <summary>
        /// Generates <see cref="PackageRowViewModel" /> instances for the specified packages and related things.
        /// </summary>
        /// <param name="packages">The collection of <see cref="IPackage" /> instances.</param>
        /// <param name="things">
        /// The collection of related <see cref="IThing" /> instances containing organizations, package
        /// versions, and package types.
        /// </param>
        /// <returns>A read-only list of <see cref="PackageRowViewModel" /> instances.</returns>
        public static IReadOnlyList<PackageRowViewModel> GenerateRows(IEnumerable<IPackage> packages, IEnumerable<IThing> things)
        {
            var thingsList = things?.ToList() ?? [];
            var organizations = thingsList.OfType<IOrganization>().ToList();
            var packageVersions = thingsList.OfType<IPackageVersion>().ToList();
            var packageTypes = thingsList.OfType<IPackageType>().ToList();

            var rows = new List<PackageRowViewModel>();

            foreach (var package in packages)
            {
                var organization = organizations.FirstOrDefault(o => o.Id == package.Owner);
                var publisher = organization != null ? $"@{organization.ShortName}" : DefaultPublisher;

                var packageType = packageTypes.FirstOrDefault(t => t.Id == package.PackageType);

                var latestVersion = packageVersions
                    .Where(v => v.Owner == package.Id && v.Listed)
                    .OrderByDescending(v => v.PublicationDate)
                    .ThenByDescending(v => v.Version)
                    .FirstOrDefault();

                var latestPublicationDate = latestVersion?.PublicationDate ?? package.CreatedAt;

                rows.Add(new PackageRowViewModel
                {
                    Name = package.Name,
                    ShortName = package.ShortName,
                    Description = package.Description,
                    Publisher = publisher,
                    Version = latestVersion != null ? latestVersion.Version : DefaultVersion,
                    Format = packageType?.Name ?? PackageFormatConstants.SysMlV2,
                    Tags = ResolveTagsForPackage(package),
                    DownloadCount = latestVersion?.DownloadCount ?? 0,
                    DependentsCount = ResolveDependentsCountForPackage(package),
                    IsVerified = true,
                    CreatedAt = package.CreatedAt,
                    LatestPublicationDate = latestPublicationDate,
                    LastPublished = latestPublicationDate.ToTimeAgo()
                });
            }

            return rows;
        }

        /// <summary>
        /// Resolves the display tags for a given package based on its short name.
        /// </summary>
        /// <param name="package">The package DTO.</param>
        /// <returns>A tags string appropriate for the package.</returns>
        public static string ResolveTagsForPackage(IPackage package)
        {
            return package.ShortName switch
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
        }

        /// <summary>
        /// Resolves the number of dependents for a given package based on its short name.
        /// </summary>
        /// <param name="package">The package DTO.</param>
        /// <returns>The dependents count for the package.</returns>
        public static int ResolveDependentsCountForPackage(IPackage package)
        {
            return package.ShortName switch
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
        }
    }
}
