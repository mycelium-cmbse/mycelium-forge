// ------------------------------------------------------------------------------------------------
// <copyright file="IArtifactManifestExtractor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Extractors
{
    using ErrorOr;

    using Mycelium.Forge.Extractors.Enums;
    using Mycelium.Forge.Extractors.Models;

    /// <summary>
    /// Defines the validation and metadata extraction of one artifact format.
    /// </summary>
    public interface IArtifactManifestExtractor
    {
        /// <summary>
        /// Determines whether the extractor handles the specified artifact format.
        /// </summary>
        /// <param name="kind">The artifact format.</param>
        /// <returns>True if the extractor handles the format, otherwise false.</returns>
        bool CanHandle(ArtifactKind kind);

        /// <summary>
        /// Validates an artifact and extracts its manifest.
        /// </summary>
        /// <param name="artifact">The artifact content.</param>
        /// <returns>A result containing the manifest, or one error for each failed validation check.</returns>
        ErrorOr<IArtifactManifest> Extract(Stream artifact);
    }
}
