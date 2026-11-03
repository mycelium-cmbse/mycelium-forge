// ------------------------------------------------------------------------------------------------
// <copyright file="EcssC3ManifestExtractor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Extractors
{
    using System.Text.Json;

    using CDP4Common.CommonData;

    using CDP4JsonSerializer;

    using ErrorOr;

    using Mycelium.Forge.Extractors.Enums;
    using Mycelium.Forge.Extractors.Errors;
    using Mycelium.Forge.Extractors.Models;
    using Mycelium.Forge.Extractors.Models.EcssC3;
    using Mycelium.Forge.Extractors.Readers;

    using SiteDirectory = CDP4Common.DTO.SiteDirectory;

    /// <summary>
    /// Validates an ECSS-E-TM-10-25 Annex C.3 exchange file using <see cref="IEcssC3ArchiveReader" /> and projects its
    /// descriptors onto an <see cref="EcssC3Manifest" />.
    /// </summary>
    /// <remarks>
    /// Validating <c>SiteDirectory.json</c> is essential during archive extraction because ECSS-E-TM-10-25 Annex C.3 mandates
    /// both <c>Header.json</c> and <c>SiteDirectory.json</c> as structural requirements of a conforming exchange file.
    /// Deserializing <c>SiteDirectory.json</c> via <see cref="CDP4JsonFileDal.JsonFileDal.Serializer" /> exercises the
    /// CDP4 DTO deserializer on model content, ensuring an archive contains a valid <see cref="SiteDirectory" /> without
    /// exposing unnecessary model objects on the manifest.
    /// </remarks>
    public class EcssC3ManifestExtractor : IArtifactManifestExtractor
    {
        /// <summary>
        /// The reader of the ECSS-E-TM-10-25 Annex C.3 exchange archive package.
        /// </summary>
        private readonly IEcssC3ArchiveReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="EcssC3ManifestExtractor" /> class.
        /// </summary>
        /// <param name="reader">The reader of the ECSS-E-TM-10-25 Annex C.3 exchange archive package.</param>
        public EcssC3ManifestExtractor(IEcssC3ArchiveReader reader)
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
            return kind == ArtifactKind.EcssC3;
        }

        /// <summary>
        /// Validates an ECSS-E-TM-10-25 Annex C.3 exchange file and extracts its manifest.
        /// </summary>
        /// <param name="artifact">The artifact content stream.</param>
        /// <returns>A result containing the <see cref="EcssC3Manifest" />, or one error for each failed validation check.</returns>
        public ErrorOr<IArtifactManifest> Extract(Stream artifact)
        {
            EcssC3Archive archive;

            try
            {
                archive = (EcssC3Archive)this.reader.Read(artifact);
            }
            catch (InvalidDataException exception)
            {
                return Error.Validation(EcssC3ErrorCodes.InvalidArchive, exception.Message);
            }
            catch (Exception exception) when (exception is JsonException or DeSerializationException)
            {
                return Error.Validation(EcssC3ErrorCodes.InvalidDescriptor, exception.Message);
            }
            catch (Exception exception)
            {
                return Error.Unexpected(EcssC3ErrorCodes.UnexpectedError, exception.Message);
            }

            var errors = Validate(archive);

            if (errors.Count > 0)
            {
                return errors;
            }

            return CreateManifest(archive);
        }

        /// <summary>
        /// Runs the validation checks for the archive.
        /// </summary>
        /// <param name="archive">The archive read from the stream.</param>
        /// <returns>One error for each failed check.</returns>
        private static List<Error> Validate(EcssC3Archive archive)
        {
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(archive.Header.MediaType))
            {
                errors.Add(Error.Validation(EcssC3ErrorCodes.MissingMediaType, "The Header.json has no mediaType."));
            }

            if (string.IsNullOrWhiteSpace(archive.Header.DataModelVersion))
            {
                errors.Add(Error.Validation(EcssC3ErrorCodes.MissingDataModelVersion, "The Header.json has no dataModelVersion."));
            }

            if (string.IsNullOrWhiteSpace(archive.Header.ExchangeFileFormatVersion))
            {
                errors.Add(Error.Validation(EcssC3ErrorCodes.MissingExchangeFileFormatVersion, "The Header.json has no exchangeFileFormatVersion."));
            }

            if (archive.Header.CreatedOn == null || archive.Header.CreatedOn.Utc == default)
            {
                errors.Add(Error.Validation(EcssC3ErrorCodes.MissingCreatedOn, "The Header.json has no createdOn timestamp."));
            }

            if (archive.Header.CreatorPerson == null)
            {
                errors.Add(Error.Validation(EcssC3ErrorCodes.MissingCreatorPerson, "The Header.json has no creatorPerson."));
            }

            if (archive.SiteDirectoryThings.Count == 0 || !archive.SiteDirectoryThings.Any(thing => thing is SiteDirectory || thing.ClassKind == ClassKind.SiteDirectory))
            {
                errors.Add(Error.Validation(EcssC3ErrorCodes.InvalidSiteDirectory, "The SiteDirectory.json does not contain a SiteDirectory."));
            }

            return errors;
        }

        /// <summary>
        /// Projects the validated header into an <see cref="EcssC3Manifest" /> instance.
        /// </summary>
        /// <param name="archive">The validated archive.</param>
        /// <returns>The constructed <see cref="EcssC3Manifest" />.</returns>
        private static EcssC3Manifest CreateManifest(EcssC3Archive archive)
        {
            return new EcssC3Manifest
            {
                MediaType = archive.Header.MediaType,
                DataModelVersion = archive.Header.DataModelVersion,
                ExchangeFileFormatVersion = archive.Header.ExchangeFileFormatVersion,
                CreatorOrganization = archive.Header.CreatorOrganization?.Name,
                CreatorPerson = $"{archive.Header.CreatorPerson.GivenName} {archive.Header.CreatorPerson.Surname}",
                Copyright = archive.Header.Copyright,
                Remark = archive.Header.Remark,
                Extensions = archive.Header.Extensions,
                CreatedOn = archive.Header.CreatedOn.Utc,
                LastModifiedOn = archive.Header.LastModifiedOn?.Utc
            };
        }
    }
}
