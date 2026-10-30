// ------------------------------------------------------------------------------------------------
// <copyright file="EcssC3Archive.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Models.EcssC3
{
    using CDP4Common.DTO;

    using CDP4JsonFileDal.Json;

    /// <summary>
    /// Represents the raw extracted contents of an ECSS-E-TM-10-25 Annex C.3 exchange file.
    /// </summary>
    /// <param name="Header">The deserialized Header descriptor.</param>
    /// <param name="SiteDirectoryThings">The things deserialized from the SiteDirectory.json file.</param>
    public sealed record EcssC3Archive(ExchangeFileHeader Header, IReadOnlyCollection<Thing> SiteDirectoryThings) : IArchive;
}
