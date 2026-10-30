// ------------------------------------------------------------------------------------------------
// <copyright file="ZipArchiveReader.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Readers
{
    using System.IO.Compression;

    using Mycelium.Forge.Extractors.Enums;
    using Mycelium.Forge.Extractors.Models;

    /// <summary>
    /// Base class for readers that inspect and extract entries from a zip-based artifact archive.
    /// </summary>
    public abstract class ZipArchiveReader : IArchiveReader
    {
        /// <summary>
        /// Determines whether the reader handles the specified artifact format.
        /// </summary>
        /// <param name="kind">The artifact format.</param>
        /// <returns>True if the reader handles the format, otherwise false.</returns>
        public abstract bool CanHandle(ArtifactKind kind);

        /// <summary>
        /// Reads an artifact archive package from the specified stream.
        /// </summary>
        /// <param name="stream">The archive content stream to read.</param>
        /// <returns>The deserialized <see cref="IArchive" />.</returns>
        public abstract IArchive Read(Stream stream);

        /// <summary>
        /// Gets the single archive entry with the specified file name.
        /// </summary>
        /// <param name="archive">The zip archive.</param>
        /// <param name="entryName">The expected entry file name.</param>
        /// <returns>The unique <see cref="ZipArchiveEntry" />.</returns>
        /// <exception cref="InvalidDataException">
        /// Thrown when the entry does not exist or when multiple entries share the same file name.
        /// </exception>
        protected static ZipArchiveEntry GetUniqueEntry(ZipArchive archive, string entryName)
        {
            var matchingEntries = archive.Entries.Where(entry => string.Equals(entry.Name, entryName, StringComparison.OrdinalIgnoreCase)).ToList();

            return EnsureSingleEntry(matchingEntries, entryName);
        }

        /// <summary>
        /// Gets the single archive entry whose file extension matches any of the specified extensions.
        /// </summary>
        /// <param name="archive">The zip archive.</param>
        /// <param name="extensions">The collection of allowed file extensions.</param>
        /// <returns>The unique <see cref="ZipArchiveEntry" />.</returns>
        /// <exception cref="InvalidDataException">
        /// Thrown when no entry has a matching extension or when multiple entries share matching extensions.
        /// </exception>
        protected static ZipArchiveEntry GetUniqueEntry(ZipArchive archive, params string[] extensions)
        {
            var matchingEntries = archive.Entries.Where(entry => extensions.Contains(Path.GetExtension(entry.FullName), StringComparer.OrdinalIgnoreCase)).ToList();

            return EnsureSingleEntry(matchingEntries, string.Join(" or ", extensions));
        }

        /// <summary>
        /// Ensures that exactly one archive entry matched the search criteria.
        /// </summary>
        /// <param name="matchingEntries">The list of matched entries.</param>
        /// <param name="description">The description of the expected file or extension pattern.</param>
        /// <returns>The single matched <see cref="ZipArchiveEntry" />.</returns>
        /// <exception cref="InvalidDataException">
        /// Thrown when no entries match or when multiple entries match.
        /// </exception>
        private static ZipArchiveEntry EnsureSingleEntry(List<ZipArchiveEntry> matchingEntries, string description)
        {
            return matchingEntries.Count switch
            {
                0 => throw new InvalidDataException($"The archive does not contain a {description} file."),
                > 1 => throw new InvalidDataException($"The archive contains multiple {description} files."),
                _ => matchingEntries[0]
            };
        }
    }
}
