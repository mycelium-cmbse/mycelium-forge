// ------------------------------------------------------------------------------------------------
// <copyright file="EcssC3ArchiveReader.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Readers
{
    using System.IO.Compression;
    using System.Text.Json;

    using CDP4JsonFileDal;
    using CDP4JsonFileDal.Json;

    using Mycelium.Forge.Extractors.Models.EcssC3;
    using Mycelium.Forge.Extractors.Resolvers;

    /// <summary>
    /// Reads and deserializes an ECSS-E-TM-10-25 Annex C.3 exchange archive package from a stream.
    /// </summary>
    /// <remarks>
    /// Locates and deserializes <c>Header.json</c> using <see cref="JsonSerializer" /> with
    /// <see cref="EcssHeaderTypeInfoResolver" />, and <c>SiteDirectory.json</c> using <see cref="JsonFileDal.Serializer" />.
    /// It populates an <see cref="EcssC3Archive" /> holding the extracted descriptors for manifest extraction.
    /// </remarks>
    public class EcssC3ArchiveReader : IEcssC3ArchiveReader
    {
        /// <summary>
        /// The relative entry name of the header descriptor within an archive.
        /// </summary>
        private const string HeaderEntryName = "Header.json";

        /// <summary>
        /// The relative entry name of the site directory descriptor within an archive.
        /// </summary>
        private const string SiteDirectoryEntryName = "SiteDirectory.json";

        /// <summary>
        /// The JSON serializer options for Header.json deserialization.
        /// </summary>
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            TypeInfoResolver = new EcssHeaderTypeInfoResolver()
        };

        /// <summary>
        /// The JSON file DAL instance used to deserialize archive descriptors.
        /// </summary>
        private readonly JsonFileDal jsonFileDal;

        /// <summary>
        /// Initializes a new instance of the <see cref="EcssC3ArchiveReader" /> class.
        /// </summary>
        /// <param name="jsonFileDal">The <see cref="JsonFileDal" /> instance.</param>
        public EcssC3ArchiveReader(JsonFileDal jsonFileDal)
        {
            this.jsonFileDal = jsonFileDal;
        }

        /// <summary>
        /// Reads an ECSS-E-TM-10-25 Annex C.3 exchange file archive from the specified stream.
        /// </summary>
        /// <param name="stream">The archive content stream to read.</param>
        /// <returns>The deserialized <see cref="EcssC3Archive" /> containing the header and site directory descriptors.</returns>
        public EcssC3Archive Read(Stream stream)
        {
            using var archive = new ZipArchive(stream, ZipArchiveMode.Read, true);

            var headerEntry = archive.Entries.FirstOrDefault(entry => entry.FullName.EndsWith(HeaderEntryName, StringComparison.OrdinalIgnoreCase))
                              ?? throw new InvalidDataException("The archive does not contain a Header.json file.");

            var siteEntry = archive.Entries.FirstOrDefault(entry => entry.FullName.EndsWith(SiteDirectoryEntryName, StringComparison.OrdinalIgnoreCase))
                            ?? throw new InvalidDataException("The archive does not contain a SiteDirectory.json file.");

            using var headerStream = headerEntry.Open();
            var header = JsonSerializer.Deserialize<ExchangeFileHeader>(headerStream, SerializerOptions) ?? throw new JsonException("The Header.json content cannot be null.");

            using var siteStream = siteEntry.Open();
            var siteDirectoryThings = this.jsonFileDal.Serializer.Deserialize(siteStream)?.ToList() ?? [];

            return new EcssC3Archive(header, siteDirectoryThings);
        }
    }
}
