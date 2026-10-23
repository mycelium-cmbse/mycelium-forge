// ------------------------------------------------------------------------------------------------
// <copyright file="IEcssC3ArchiveReader.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Readers
{
    using Mycelium.Forge.Extractors.Models.EcssC3;

    /// <summary>
    /// Reads and deserializes an ECSS-E-TM-10-25 Annex C.3 exchange archive package from a stream.
    /// </summary>
    /// <remarks>
    /// This reader inspects the archive stream to locate and deserialize the root exchange descriptors,
    /// namely <c>Header.json</c> and <c>SiteDirectory.json</c>. It returns an <see cref="EcssC3Archive" />
    /// container suitable for manifest extraction and publish-time archive validation without initializing
    /// a full DAL session.
    /// </remarks>
    public interface IEcssC3ArchiveReader
    {
        /// <summary>
        /// Reads an ECSS-E-TM-10-25 Annex C.3 exchange file archive from the specified stream.
        /// </summary>
        /// <param name="stream">The archive content stream to read.</param>
        /// <returns>The deserialized <see cref="EcssC3Archive" /> containing the header and site directory descriptors.</returns>
        EcssC3Archive Read(Stream stream);
    }
}
