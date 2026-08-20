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
    public class PackageDetailsModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageDetailsModel" /> class.
        /// </summary>
        public PackageDetailsModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageDetailsModel" /> class with the specified package model and details.
        /// </summary>
        /// <param name="package">The underlying package model.</param>
        /// <param name="releaseStatus">The release stability status.</param>
        /// <param name="provenance">The provenance summary text.</param>
        /// <param name="qualityScore">The overall quality score text.</param>
        /// <param name="metamodel">The target metamodel and version.</param>
        /// <param name="repositoryUrl">The source repository URL.</param>
        /// <param name="repositoryDisplayName">The shortened display text for the repository URL.</param>
        /// <param name="packageUrl">The standard package URL identifier.</param>
        /// <param name="packageUrlDisplayName">The shortened display text for the package URL.</param>
        /// <param name="readmeDescription">The overview section description.</param>
        /// <param name="readmeContents">The contents specification description.</param>
        /// <param name="codeUsageImport">The import statement code example.</param>
        /// <param name="codeUsageBody">The body code example.</param>
        /// <param name="qualityChecks">The collection of quality evaluation checks.</param>
        /// <param name="tags">The collection of classification tags.</param>
        /// <param name="installCommands">The dictionary mapping install method tabs to command strings.</param>
        /// <param name="elements">The collection of model element definitions contained within the package.</param>
        /// <param name="dependencies">The collection of dependencies required by the package.</param>
        /// <param name="dependents">The collection of packages and projects depending on this package.</param>
        /// <param name="validationReport">The automated release validation report for this package.</param>
        public PackageDetailsModel(
            PackageModel package,
            string releaseStatus = "Latest stable",
            string provenance = "",
            string qualityScore = "5/5 checks",
            string metamodel = "SysML v2 (2025-02)",
            string repositoryUrl = "",
            string repositoryDisplayName = "",
            string packageUrl = "",
            string packageUrlDisplayName = "",
            string readmeDescription = "",
            string readmeContents = "",
            string codeUsageImport = "",
            string codeUsageBody = "",
            IReadOnlyList<ValidationCheckModel> qualityChecks = null,
            IReadOnlyList<string> tags = null,
            IReadOnlyDictionary<string, string> installCommands = null,
            IReadOnlyList<PackageElementModel> elements = null,
            IReadOnlyList<PackageRelationshipModel> dependencies = null,
            IReadOnlyList<PackageRelationshipModel> dependents = null,
            PackageValidationReportModel validationReport = null)
        {
            this.Package = package;
            this.ReleaseStatus = releaseStatus;
            this.Provenance = provenance;
            this.QualityScore = qualityScore;
            this.Metamodel = metamodel;
            this.RepositoryUrl = repositoryUrl;
            this.RepositoryDisplayName = repositoryDisplayName;
            this.PackageUrl = packageUrl;
            this.PackageUrlDisplayName = packageUrlDisplayName;
            this.ReadmeDescription = readmeDescription;
            this.ReadmeContents = readmeContents;
            this.CodeUsageImport = codeUsageImport;
            this.CodeUsageBody = codeUsageBody;
            this.QualityChecks = qualityChecks ?? [];
            this.Tags = tags ?? [];
            this.InstallCommands = installCommands ?? new Dictionary<string, string>();
            this.Elements = elements ?? [];
            this.Dependencies = dependencies ?? [];
            this.Dependents = dependents ?? [];
            this.ValidationReport = validationReport;
        }

        /// <summary>
        /// Gets or sets the underlying package model.
        /// </summary>
        public PackageModel Package { get; set; }

        /// <summary>
        /// Gets or sets the release stability status.
        /// </summary>
        public string ReleaseStatus { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the provenance summary text.
        /// </summary>
        public string Provenance { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the overall quality score text.
        /// </summary>
        public string QualityScore { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the target metamodel and version.
        /// </summary>
        public string Metamodel { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the source repository URL.
        /// </summary>
        public string RepositoryUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the shortened display text for the repository URL.
        /// </summary>
        public string RepositoryDisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the standard package URL identifier.
        /// </summary>
        public string PackageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the shortened display text for the package URL.
        /// </summary>
        public string PackageUrlDisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the overview section description.
        /// </summary>
        public string ReadmeDescription { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the contents specification description.
        /// </summary>
        public string ReadmeContents { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the import statement code example.
        /// </summary>
        public string CodeUsageImport { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the body code example.
        /// </summary>
        public string CodeUsageBody { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of quality evaluation checks.
        /// </summary>
        public IReadOnlyList<ValidationCheckModel> QualityChecks { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of classification tags.
        /// </summary>
        public IReadOnlyList<string> Tags { get; set; } = [];

        /// <summary>
        /// Gets or sets the dictionary mapping install method tabs to command strings.
        /// </summary>
        public IReadOnlyDictionary<string, string> InstallCommands { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the collection of model element definitions contained within the package.
        /// </summary>
        public IReadOnlyList<PackageElementModel> Elements { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of dependencies required by the package.
        /// </summary>
        public IReadOnlyList<PackageRelationshipModel> Dependencies { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of packages and projects depending on this package.
        /// </summary>
        public IReadOnlyList<PackageRelationshipModel> Dependents { get; set; } = [];

        /// <summary>
        /// Gets or sets the automated release validation report for this package.
        /// </summary>
        public PackageValidationReportModel ValidationReport { get; set; }
    }
}
