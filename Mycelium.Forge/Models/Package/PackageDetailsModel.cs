// ------------------------------------------------------------------------------------------------
// <copyright file="PackageDetailsModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.Package
{
    using Mycelium.Forge.Models.Validation;

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
        /// Initializes a new instance of the <see cref="PackageDetailsModel" /> class with the specified package model.
        /// </summary>
        /// <param name="package">The underlying package model.</param>
        public PackageDetailsModel(PackageModel package = null)
        {
            this.Package = package;
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
