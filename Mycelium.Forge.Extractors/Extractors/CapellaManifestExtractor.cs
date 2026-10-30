// ------------------------------------------------------------------------------------------------
// <copyright file="CapellaManifestExtractor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Extractors
{
    using System.Xml;

    using Auriga.Libraries;

    using ErrorOr;

    using Mycelium.Forge.Extractors.Enums;
    using Mycelium.Forge.Extractors.Errors;
    using Mycelium.Forge.Extractors.Models;
    using Mycelium.Forge.Extractors.Models.Capella;
    using Mycelium.Forge.Extractors.Readers;

    /// <summary>
    /// Validates a Capella project archive using <see cref="ICapellaArchiveReader" /> and projects its
    /// descriptors onto a <see cref="CapellaManifest" />.
    /// </summary>
    public class CapellaManifestExtractor : IArtifactManifestExtractor
    {
        /// <summary>
        /// The reader of the Capella archive package.
        /// </summary>
        private readonly ICapellaArchiveReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="CapellaManifestExtractor" /> class.
        /// </summary>
        /// <param name="reader">The reader of the Capella archive package.</param>
        public CapellaManifestExtractor(ICapellaArchiveReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Determines whether the extractor handles the specified artifact format.
        /// </summary>
        /// <param name="kind">The artifact format.</param>
        /// <returns>True if the extractor handles the format, otherwise false.</returns>
        public bool CanHandle(ArtifactKind kind)
        {
            return kind == ArtifactKind.Capella;
        }

        /// <summary>
        /// Validates a Capella project archive and extracts its manifest.
        /// </summary>
        /// <param name="artifact">The artifact content stream.</param>
        /// <returns>A result containing the <see cref="CapellaManifest" />, or one error for each failed validation check.</returns>
        public ErrorOr<IArtifactManifest> Extract(Stream artifact)
        {
            CapellaArchive archive;

            try
            {
                archive = (CapellaArchive)this.reader.Read(artifact);
            }
            catch (InvalidDataException exception)
            {
                return Error.Validation(CapellaErrorCodes.InvalidArchive, exception.Message);
            }
            catch (XmlException exception)
            {
                return Error.Validation(CapellaErrorCodes.InvalidDescriptor, exception.Message);
            }
            catch (Exception exception)
            {
                return Error.Unexpected(CapellaErrorCodes.UnexpectedError, exception.Message);
            }

            var errors = Validate(archive);

            if (errors.Count > 0)
            {
                return errors;
            }

            return CreateManifest(archive);
        }

        /// <summary>
        /// Validates the Capella archive descriptors against registry invariants.
        /// </summary>
        /// <param name="archive">The archive to validate.</param>
        /// <returns>The list of validation errors, or empty if validation succeeds.</returns>
        private static List<Error> Validate(CapellaArchive archive)
        {
            List<Error> errors = [];

            if (string.IsNullOrWhiteSpace(archive.Project.Name))
            {
                errors.Add(Error.Validation(CapellaErrorCodes.MissingName, "The Capella project root element must have a non-empty name."));
            }

            return errors;
        }

        /// <summary>
        /// Maps the validated <see cref="CapellaArchive" /> to a <see cref="CapellaManifest" />.
        /// </summary>
        /// <param name="archive">The validated archive.</param>
        /// <returns>The populated manifest.</returns>
        private static CapellaManifest CreateManifest(CapellaArchive archive)
        {
            var modelVersion = archive.Project.OwnedExtensions
                .OfType<IModelInformation>()
                .Select(modelInformation => modelInformation.Version)
                .FirstOrDefault();

            return new CapellaManifest
            {
                Name = archive.Project.Name,
                Summary = archive.Project.Summary,
                Description = archive.Project.Description,
                MajorVersionNumber = modelVersion?.MajorVersionNumber,
                MinorVersionNumber = modelVersion?.MinorVersionNumber,
                LastModifiedFileStamp = modelVersion?.LastModifiedFileStamp
            };
        }
    }
}
