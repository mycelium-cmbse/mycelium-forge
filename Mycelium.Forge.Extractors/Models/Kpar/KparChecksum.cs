// ------------------------------------------------------------------------------------------------
// <copyright file="KparChecksum.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Models.Kpar
{
    /// <summary>
    /// Represents one <c>checksum</c> entry of a <c>.meta.json</c>.
    /// </summary>
    public class KparChecksum
    {
        /// <summary>
        /// Gets or sets the name of the algorithm that produced the checksum.
        /// </summary>
        public string Algorithm { get; set; }

        /// <summary>
        /// Gets or sets the checksum value.
        /// </summary>
        public string Value { get; set; }
    }
}
