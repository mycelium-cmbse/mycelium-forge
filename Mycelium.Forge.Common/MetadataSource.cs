// ------------------------------------------------------------------------------------------------
// <copyright file="MetadataSource.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    /// <summary>
    /// Specifies the provenance origin of package metadata.
    /// </summary>
    public enum MetadataSource
    {
        /// <summary>
        /// The metadata is declared directly by the package artifact manifest.
        /// </summary>
        DeclaredByArtefact,

        /// <summary>
        /// The metadata is asserted by the publisher rather than extracted from the artifact.
        /// </summary>
        AssertedByPublisher
    }
}
