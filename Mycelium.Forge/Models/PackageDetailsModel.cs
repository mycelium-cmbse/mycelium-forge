// ------------------------------------------------------------------------------------------------
// <copyright file="PackageDetailsModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents comprehensive metadata and details for a package release displayed on the package page.
    /// </summary>
    /// <param name="Name">The package short name.</param>
    /// <param name="Scope">The publisher namespace scope.</param>
    /// <param name="FullName">The full scoped package identifier.</param>
    /// <param name="Format">The package metamodel or schema format.</param>
    /// <param name="Version">The latest package release version.</param>
    /// <param name="ReleaseStatus">The release stability status.</param>
    /// <param name="Description">The summary description of the package.</param>
    /// <param name="Provenance">The provenance summary text.</param>
    /// <param name="IsVerified">A value indicating whether the publisher is verified.</param>
    /// <param name="ImportCount">The total number of package imports or downloads.</param>
    /// <param name="QualityScore">The overall quality score text.</param>
    /// <param name="PublishedAgo">The elapsed time since publication.</param>
    /// <param name="License">The software license expression.</param>
    /// <param name="Metamodel">The target metamodel and version.</param>
    /// <param name="RepositoryUrl">The source repository URL.</param>
    /// <param name="RepositoryDisplayName">The shortened display text for the repository URL.</param>
    /// <param name="PackageUrl">The standard package URL identifier.</param>
    /// <param name="PackageUrlDisplayName">The shortened display text for the package URL.</param>
    /// <param name="ReadmeDescription">The overview section description.</param>
    /// <param name="ReadmeContents">The contents specification description.</param>
    /// <param name="CodeUsageImport">The import statement code example.</param>
    /// <param name="CodeUsageBody">The body code example.</param>
    /// <param name="QualityChecks">The collection of quality evaluation checks.</param>
    /// <param name="Maintainers">The collection of package maintainers.</param>
    /// <param name="Tags">The collection of classification tags.</param>
    /// <param name="InstallCommands">The dictionary mapping install method tabs to command strings.</param>
    /// <param name="Elements">The collection of model element definitions contained within the package.</param>
    /// <param name="Dependencies">The collection of dependencies required by the package.</param>
    /// <param name="Dependents">The collection of packages and projects depending on this package.</param>
    /// <param name="Versions">The collection of historical releases for this package.</param>
    /// <param name="ValidationReport">The automated release validation report for this package.</param>
    public record PackageDetailsModel(
        string Name,
        string Scope,
        string FullName,
        string Format,
        string Version,
        string ReleaseStatus,
        string Description,
        string Provenance,
        bool IsVerified,
        string ImportCount,
        string QualityScore,
        string PublishedAgo,
        string License,
        string Metamodel,
        string RepositoryUrl,
        string RepositoryDisplayName,
        string PackageUrl,
        string PackageUrlDisplayName,
        string ReadmeDescription,
        string ReadmeContents,
        string CodeUsageImport,
        string CodeUsageBody,
        IReadOnlyList<PackageQualityCheckModel> QualityChecks,
        IReadOnlyList<PackageMaintainerModel> Maintainers,
        IReadOnlyList<string> Tags,
        IReadOnlyDictionary<string, string> InstallCommands,
        IReadOnlyList<PackageElementModel> Elements,
        IReadOnlyList<PackageDependencyModel> Dependencies,
        IReadOnlyList<PackageDependentModel> Dependents,
        IReadOnlyList<PackageVersionModel> Versions,
        PackageValidationReportModel ValidationReport);
}
