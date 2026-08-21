// ------------------------------------------------------------------------------------------------
// <copyright file="PublishPackageMetadataModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.Publish
{
    using Mycelium.Forge.Common;

    /// <summary>
    /// Represents the form metadata properties of a package being published.
    /// </summary>
    public class PublishPackageMetadataModel
    {
        /// <summary>
        /// Gets or sets the archive file name of the package artefact.
        /// </summary>
        public string ArtefactFileName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the format name detected from the archive.
        /// </summary>
        public string ArtefactFormat { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the publisher namespace scope for the package.
        /// </summary>
        public string Scope { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the scope publisher is verified.
        /// </summary>
        public bool IsScopeVerified { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier name of the package within the scope.
        /// </summary>
        public string PackageName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the semantic version string of the package release.
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the summary description text for the package.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the SPDX license identifier of the package.
        /// </summary>
        public string License { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the visibility level of the package.
        /// </summary>
        public VisibilityKind Visibility { get; set; } = VisibilityKind.PUBLIC;

        /// <summary>
        /// Gets or sets the target metamodel specification and edition.
        /// </summary>
        public string Metamodel { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the comma-separated keywords and topic tags.
        /// </summary>
        public string Tags { get; set; } = string.Empty;
    }
}
