// ------------------------------------------------------------------------------------------------
// <copyright file="IArchiveReader.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Readers
{
    using Mycelium.Forge.Extractors.Enums;
    using Mycelium.Forge.Extractors.Models;

    /// <summary>
    /// Reads and deserializes an artifact archive package from a stream.
    /// </summary>
    public interface IArchiveReader
    {
        /// <summary>
        /// Determines whether the reader handles the specified artifact format.
        /// </summary>
        /// <param name="kind">The artifact format.</param>
        /// <returns>True if the reader handles the format, otherwise false.</returns>
        bool CanHandle(ArtifactKind kind);

        /// <summary>
        /// Reads an artifact archive package from the specified stream.
        /// </summary>
        /// <param name="stream">The archive content stream to read.</param>
        /// <returns>The deserialized <see cref="IArchive" />.</returns>
        IArchive Read(Stream stream);
    }
}
