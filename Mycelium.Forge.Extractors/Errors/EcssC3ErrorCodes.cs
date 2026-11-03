// ------------------------------------------------------------------------------------------------
// <copyright file="EcssC3ErrorCodes.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Errors
{
    /// <summary>
    /// Provides the error code constants returned when ECSS-E-TM-10-25 Annex C.3 archive validation fails.
    /// </summary>
    public static class EcssC3ErrorCodes
    {
        /// <summary>
        /// Indicates that the artifact stream is not a valid ZIP archive or does not contain required files.
        /// </summary>
        public const string InvalidArchive = "EcssC3.InvalidArchive";

        /// <summary>
        /// Indicates that a descriptor file (Header.json or SiteDirectory.json) cannot be parsed or deserialized.
        /// </summary>
        public const string InvalidDescriptor = "EcssC3.InvalidDescriptor";

        /// <summary>
        /// Indicates that Header.json is missing the mediaType property or it is empty.
        /// </summary>
        public const string MissingMediaType = "EcssC3.MissingMediaType";

        /// <summary>
        /// Indicates that Header.json is missing the dataModelVersion property or it is empty.
        /// </summary>
        public const string MissingDataModelVersion = "EcssC3.MissingDataModelVersion";

        /// <summary>
        /// Indicates that Header.json is missing the exchangeFileFormatVersion property or it is empty.
        /// </summary>
        public const string MissingExchangeFileFormatVersion = "EcssC3.MissingExchangeFileFormatVersion";

        /// <summary>
        /// Indicates that Header.json is missing the createdOn timestamp or it cannot be parsed.
        /// </summary>
        public const string MissingCreatedOn = "EcssC3.MissingCreatedOn";

        /// <summary>
        /// Indicates that Header.json is missing the creatorPerson descriptor.
        /// </summary>
        public const string MissingCreatorPerson = "EcssC3.MissingCreatorPerson";

        /// <summary>
        /// Indicates that SiteDirectory.json contains no SiteDirectory entity.
        /// </summary>
        public const string InvalidSiteDirectory = "EcssC3.InvalidSiteDirectory";

        /// <summary>
        /// Indicates that an unexpected error occurred while processing the archive.
        /// </summary>
        public const string UnexpectedError = "EcssC3.UnexpectedError";
    }
}
