// ------------------------------------------------------------------------------------------------
// <copyright file="PackageVersionModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents an immutable release version entry for a package in the version history.
    /// </summary>
    /// <param name="Version">The semver version string.</param>
    /// <param name="Badge">The optional status badge text (e.g., Latest, pre).</param>
    /// <param name="PublishedAgo">The elapsed time string since publication.</param>
    /// <param name="DependentCount">The number of dependent packages or projects using this version.</param>
    /// <param name="IsValidated">A value indicating whether the release passed validation checks.</param>
    /// <param name="Size">The formatted package archive size.</param>
    /// <param name="IsUnlisted">A value indicating whether the package version is unlisted.</param>
    /// <param name="DownloadUrl">The direct download URL for the package version artifact.</param>
    public record PackageVersionModel(
        string Version,
        string Badge,
        string PublishedAgo,
        int DependentCount,
        bool IsValidated,
        string Size,
        bool IsUnlisted = false,
        string DownloadUrl = "#");
}
