// ------------------------------------------------------------------------------------------------
// <copyright file="CapellaArchiveReader.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Readers
{
    using System.IO.Compression;
    using System.Xml.Linq;

    using Auriga.Capellamodeller;
    using Auriga.Libraries;

    using Mycelium.Forge.Extractors.Models.Capella;

    /// <summary>
    /// Reads and deserializes a Capella project archive package from a stream.
    /// </summary>
    /// <remarks>
    /// Once the <c>Auriga.Xmi</c> package is published on NuGet, this reader should be updated to use Auriga's native
    /// XMI capabilities and tools to parse the semantic model directly.
    /// </remarks>
    public class CapellaArchiveReader : ICapellaArchiveReader
    {
        /// <summary>
        /// The file extensions that identify a Capella semantic model file.
        /// </summary>
        private static readonly string[] ModelExtensions = [".capella", ".melodymodeller"];

        /// <summary>
        /// Reads a Capella archive package from the specified stream.
        /// </summary>
        /// <param name="stream">The archive content stream to read.</param>
        /// <returns>The deserialized <see cref="CapellaArchive" /> containing the project descriptor.</returns>
        public CapellaArchive Read(Stream stream)
        {
            using var archive = new ZipArchive(stream, ZipArchiveMode.Read, true);

            var modelEntry = GetUniqueModelEntry(archive);

            using var entryStream = modelEntry.Open();
            var document = XDocument.Load(entryStream);

            var root = document.Root;

            if (root == null)
            {
                throw new InvalidDataException("The Capella semantic model XML root cannot be null.");
            }

            var isLibrary = root.Name.LocalName.Equals("Library", StringComparison.OrdinalIgnoreCase);
            var isProject = root.Name.LocalName.Equals("Project", StringComparison.OrdinalIgnoreCase);

            if (!isProject && !isLibrary)
            {
                throw new InvalidDataException($"The Capella semantic model root element '{root.Name}' is not a Project or Library.");
            }

            IProject project = isLibrary ? new Library() : new Project();
            project.Id = (string)root.Attribute("id");
            project.Name = (string)root.Attribute("name");
            project.Summary = (string)root.Attribute("summary");
            project.Description = (string)root.Attribute("description");

            var modelInformationElement = root.Elements().FirstOrDefault(IsModelInformationElement);

            if (modelInformationElement == null)
            {
                return new CapellaArchive(project);
            }

            var modelInformation = new ModelInformation
            {
                Id = (string)modelInformationElement.Attribute("id")
            };

            var versionElement = modelInformationElement.Elements().FirstOrDefault(IsModelVersionElement);

            if (versionElement != null)
            {
                var modelVersion = new ModelVersion
                {
                    Id = (string)versionElement.Attribute("id"),
                    MajorVersionNumber = (int)versionElement.Attribute("majorVersionNumber"),
                    MinorVersionNumber = (int)versionElement.Attribute("minorVersionNumber"),
                    LastModifiedFileStamp = (long?)versionElement.Attribute("lastModifiedFileStamp")
                };

                modelInformation.Version = modelVersion;
            }

            project.OwnedExtensions.Add(modelInformation);

            return new CapellaArchive(project);
        }

        /// <summary>
        /// Determines whether the specified XML element represents a ModelInformation extension.
        /// </summary>
        /// <param name="element">The XML element.</param>
        /// <returns>True if the element represents ModelInformation, otherwise false.</returns>
        private static bool IsModelInformationElement(XElement element)
        {
            var isOwnedExtension = element.Name.LocalName.Equals("ownedExtensions", StringComparison.OrdinalIgnoreCase);
            var xsiType = (string)element.Attribute(XName.Get("type", "http://www.w3.org/2001/XMLSchema-instance")) ?? string.Empty;

            return isOwnedExtension && xsiType.EndsWith("ModelInformation", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether the specified XML element represents a ModelVersion descriptor.
        /// </summary>
        /// <param name="element">The XML element.</param>
        /// <returns>True if the element represents ModelVersion, otherwise false.</returns>
        private static bool IsModelVersionElement(XElement element)
        {
            var isVersionName = element.Name.LocalName.Equals("version", StringComparison.OrdinalIgnoreCase);
            var xsiType = (string)element.Attribute(XName.Get("type", "http://www.w3.org/2001/XMLSchema-instance")) ?? string.Empty;

            return isVersionName || xsiType.EndsWith("ModelVersion", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the single root semantic model file entry (.capella or .melodymodeller) from the archive.
        /// </summary>
        /// <param name="archive">The zip archive.</param>
        /// <returns>The unique <see cref="ZipArchiveEntry" />.</returns>
        private static ZipArchiveEntry GetUniqueModelEntry(ZipArchive archive)
        {
            var matchingEntries = archive.Entries
                .Where(entry => ModelExtensions.Contains(Path.GetExtension(entry.FullName), StringComparer.OrdinalIgnoreCase))
                .ToList();

            return matchingEntries.Count switch
            {
                0 => throw new InvalidDataException("The archive does not contain a .capella or .melodymodeller file."),
                1 => matchingEntries[0],
                _ => throw new InvalidDataException("The archive contains multiple semantic model files.")
            };
        }
    }
}
