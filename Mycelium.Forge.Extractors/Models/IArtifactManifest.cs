// ------------------------------------------------------------------------------------------------
// <copyright file="IArtifactManifest.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Models
{
    /// <summary>
    /// Represents the metadata extracted from an artifact of any supported format.
    /// </summary>
    public interface IArtifactManifest
    {
        /// <summary>
        /// Gets the format of the artifact the manifest was extracted from.
        /// </summary>
        ArtifactKind Kind { get; }
    }
}
