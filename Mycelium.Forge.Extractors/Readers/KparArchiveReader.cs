// ------------------------------------------------------------------------------------------------
// <copyright file="KparArchiveReader.cs" company="Starion Group S.A.">
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
    using Mycelium.Forge.Extractors.Models.Kpar;

    using SysML2.NET.Kpar;

    /// <summary>
    /// Reads and deserializes a SysML v2 or KerML project interchange package (<c>.kpar</c>) from a stream using
    /// <see cref="IReader" />.
    /// </summary>
    public class KparArchiveReader : IKparArchiveReader
    {
        /// <summary>
        /// The options making the <see cref="IReader" /> enforce the root descriptors, the existence of every indexed model file
        /// and the declared checksums, collecting every checksum mismatch.
        /// </summary>
        private static readonly ReadOptions ReadOptions = new()
        {
            ValidateRequiredDescriptors = true,
            ValidateIndexPaths = true,
            ValidateChecksums = true,
            ChecksumFailureBehavior = ChecksumFailureBehavior.Collect
        };

        /// <summary>
        /// The reader of the kpar archive format.
        /// </summary>
        private readonly IReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="KparArchiveReader" /> class.
        /// </summary>
        /// <param name="reader">The reader of the kpar archive format.</param>
        public KparArchiveReader(IReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Determines whether the reader handles the specified artifact format.
        /// </summary>
        /// <param name="kind">The artifact format.</param>
        /// <returns>True if the reader handles the format, otherwise false.</returns>
        public bool CanHandle(ArtifactKind kind)
        {
            return kind == ArtifactKind.Kpar;
        }

        /// <summary>
        /// Reads an artifact archive package from the specified stream.
        /// </summary>
        /// <param name="stream">The archive content stream to read.</param>
        /// <returns>The deserialized <see cref="IArchive" />.</returns>
        public IArchive Read(Stream stream)
        {
            var archive = this.reader.Read(stream, ReadOptions);
            return new KparArchive(archive);
        }
    }
}
