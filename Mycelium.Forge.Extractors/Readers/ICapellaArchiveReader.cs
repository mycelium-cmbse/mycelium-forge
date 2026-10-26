// ------------------------------------------------------------------------------------------------
// <copyright file="ICapellaArchiveReader.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Readers
{
    using Mycelium.Forge.Extractors.Models.Capella;

    /// <summary>
    /// Reads and deserializes a Capella project archive package from a stream.
    /// </summary>
    public interface ICapellaArchiveReader
    {
        /// <summary>
        /// Reads a Capella archive package from the specified stream.
        /// </summary>
        /// <param name="stream">The archive content stream to read.</param>
        /// <returns>The deserialized <see cref="CapellaArchive" /> containing the project descriptor.</returns>
        CapellaArchive Read(Stream stream);
    }
}
