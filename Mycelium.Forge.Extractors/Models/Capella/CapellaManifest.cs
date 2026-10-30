// ------------------------------------------------------------------------------------------------
// <copyright file="CapellaManifest.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Models.Capella
{
    using Mycelium.Forge.Extractors.Enums;

    /// <summary>
    /// Represents the registry metadata projected from the semantic model file of a Capella project archive.
    /// </summary>
    public class CapellaManifest : IArtifactManifest
    {
        /// <summary>
        /// Gets the format of the artifact the manifest was extracted from.
        /// </summary>
        public ArtifactKind Kind => ArtifactKind.Capella;

        /// <summary>
        /// Gets or sets the Capella project or library name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the project summary description.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the project full description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the major version number from the model version if defined.
        /// </summary>
        public int? MajorVersionNumber { get; set; }

        /// <summary>
        /// Gets or sets the minor version number from the model version if defined.
        /// </summary>
        public int? MinorVersionNumber { get; set; }

        /// <summary>
        /// Gets or sets the last modified file timestamp from the model version if defined.
        /// </summary>
        public long? LastModifiedFileStamp { get; set; }
    }
}
