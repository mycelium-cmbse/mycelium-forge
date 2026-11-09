// ------------------------------------------------------------------------------------------------
// <copyright file="PublishArtefactModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.Publish
{
    using Mycelium.Forge.Extractors.Enums;

    /// <summary>
    /// Represents UI display properties of a staged package artefact across the publish wizard steps.
    /// </summary>
    public class PublishArtefactModel
    {
        /// <summary>
        /// Gets or sets the file name of the staged artefact.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the token identifying the staged upload artefact.
        /// </summary>
        public string StagingToken { get; set; }

        /// <summary>
        /// Gets or sets the human-readable format name of the staged artefact.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the size in bytes of the staged artefact.
        /// </summary>
        public long SizeInBytes { get; set; }

        /// <summary>
        /// Gets or sets the kind of the staged artefact.
        /// </summary>
        public ArtifactKind Kind { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether manifest extraction succeeded without errors.
        /// </summary>
        public bool IsManifestValid { get; set; }
    }
}
