// ------------------------------------------------------------------------------------------------
// <copyright file="PackageVersionsTab.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.PackageDetails.Tabs
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extensions;

    /// <summary>
    /// Represents the release versions history tab component for package details.
    /// </summary>
    public partial class PackageVersionsTab : ComponentBase
    {
        /// <summary>
        /// Gets or sets the collection of released version DTOs for the package.
        /// </summary>
        [Parameter]
        public IReadOnlyList<IPackageVersion> Versions { get; set; } = [];

        /// <summary>
        /// Gets or sets the owning organization DTO.
        /// </summary>
        [Parameter]
        public IOrganization Organization { get; set; }

        /// <summary>
        /// Gets or sets the package DTO.
        /// </summary>
        [Parameter]
        public IPackage Package { get; set; }

        /// <summary>
        /// Gets or sets the collection of package metadata DTOs corresponding to package versions.
        /// </summary>
        [Parameter]
        public IReadOnlyList<IPackageMetaData> MetaDatas { get; set; } = [];

        /// <summary>
        /// Determines whether the specified package version is the latest release.
        /// </summary>
        /// <param name="version">The package version DTO.</param>
        /// <returns><see langword="true" /> if the version is the latest release; otherwise, <see langword="false" />.</returns>
        public bool IsLatestVersion(IPackageVersion version)
        {
            return this.Versions.Count > 0 && version == this.Versions[0];
        }

        /// <summary>
        /// Determines whether the specified package version has passed automated validation checks.
        /// </summary>
        /// <param name="version">The package version DTO.</param>
        /// <returns><see langword="true" /> if the version has passed all quality checks; otherwise, <see langword="false" />.</returns>
        public bool IsValidated(IPackageVersion version)
        {
            var metaData = this.MetaDatas.FirstOrDefault(m => m.Owner == version.Id || m.Id == version.MetaData);
            return metaData != null && metaData.QualityChecks.Count > 0 && metaData.QualityChecks.All(check => check.Passed);
        }

        /// <summary>
        /// Generates the direct download URL for a package version artifact.
        /// </summary>
        /// <param name="version">The package version DTO.</param>
        /// <returns>The formatted download route URL string.</returns>
        public string GetDownloadUrl(IPackageVersion version)
        {
            return PageRoutes.GetPackageDownloadRoute(this.Organization.ShortName, this.Package.ShortName, version.GetVersion());
        }

        /// <summary>
        /// Gets the number of dependent packages or projects using the specified package version.
        /// </summary>
        /// <param name="version">The package version DTO.</param>
        /// <returns>The count of dependents; currently returns 0 because version-specific dependent tracking is not yet supported.</returns>
        public int GetDependentCount(IPackageVersion version)
        {
            return 0;
        }
    }
}
