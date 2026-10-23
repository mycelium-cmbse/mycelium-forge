// ------------------------------------------------------------------------------------------------
// <copyright file="ArtifactKind.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Models
{
    /// <summary>
    /// Enumerates the artifact formats that Forge can publish.
    /// </summary>
    public enum ArtifactKind
    {
        /// <summary>
        /// A SysML v2 or KerML project interchange file (<c>.kpar</c>).
        /// </summary>
        Kpar,

        /// <summary>
        /// An ECSS-E-TM-10-25 Annex C.3 exchange file.
        /// </summary>
        EcssC3,

        /// <summary>
        /// A Capella project archive.
        /// </summary>
        Capella
    }
}
