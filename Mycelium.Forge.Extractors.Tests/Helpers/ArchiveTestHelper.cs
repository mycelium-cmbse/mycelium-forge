// ------------------------------------------------------------------------------------------------
// <copyright file="ArchiveTestHelper.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Tests.Helpers
{
    using System.IO.Compression;

    using Mycelium.Forge.Extractors.Tests.Models;

    /// <summary>
    /// Provides utility methods for creating test archive streams.
    /// </summary>
    internal static class ArchiveTestHelper
    {
        /// <summary>
        /// Creates an in-memory zip archive stream from the specified entries.
        /// </summary>
        /// <param name="entries">The entries to write into the zip archive.</param>
        /// <returns>A seekable <see cref="MemoryStream" /> containing the zip archive.</returns>
        internal static MemoryStream CreateZipStream(params ZipEntryModel[] entries)
        {
            var stream = new MemoryStream();

            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true))
            {
                foreach (var entry in entries)
                {
                    var zipEntry = archive.CreateEntry(entry.Name);
                    using var writer = new StreamWriter(zipEntry.Open());
                    writer.Write(entry.Content);
                }
            }

            stream.Position = 0;
            return stream;
        }
    }
}
