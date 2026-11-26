// ------------------------------------------------------------------------------------------------
// <copyright file="PackageVersionDeSerializer.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Serializer.Json
{
    using System;
    using System.Text.Json;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;

    using Mycelium.Forge.Common;

    /// <summary>
    /// The purpose of the <see cref="PackageVersionDeSerializer"/> is to provide deserialization capabilities
    /// for the <see cref="IPackageVersion"/> interface
    /// </summary>
    internal static class PackageVersionDeSerializer
    {
        /// <summary>
        /// Deserializes an instance of <see cref="IPackageVersion"/> from the provided <see cref="Utf8JsonReader"/>
        /// </summary>
        /// <param name="reader">
        /// The <see cref="Utf8JsonReader"/>, positioned at the <see cref="IPackageVersion"/> json object's <see cref="JsonTokenType.StartObject"/> token
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="IPackageVersion"/>
        /// </returns>
        internal static IPackageVersion DeSerialize(ref Utf8JsonReader reader, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("PackageVersionDeSerializer");

            var dtoInstance = new Mycelium.Forge.Common.PackageVersion();

            var typeSeen = false;
            var applicableLicensesSeen = false;
            var checkSumSeen = false;
            var createdAtSeen = false;
            var downloadCountSeen = false;
            var fileExtensionSeen = false;
            var listedSeen = false;
            var modifiedAtSeen = false;
            var ownerSeen = false;
            var publicationDateSeen = false;
            var readmeSeen = false;
            var releaseNotesSeen = false;
            var sizeInBytesSeen = false;
            var versionSeen = false;

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    continue;
                }

                if (reader.ValueTextEquals("@type"u8))
                {
                    reader.Read();
                    typeSeen = true;
                    var typeValue = reader.GetString();

                    if (typeValue != "PackageVersion")
                    {
                        throw new InvalidOperationException($"The PackageVersionDeSerializer can only be used to deserialize objects of type IPackageVersion, a {typeValue} was provided");
                    }

                    continue;
                }

                if (reader.ValueTextEquals("@id"u8))
                {
                    reader.Read();

                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        throw new JsonException("The @id property is not present, the PackageVersion cannot be deserialized");
                    }

                    dtoInstance.Id = Utf8JsonReaderHelper.ReadGuid(ref reader);
                    continue;
                }

                if (reader.ValueTextEquals("applicableLicenses"u8))
                {
                    applicableLicensesSeen = true;
                    reader.Read();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var applicableLicensesRefId))
                        {
                            dtoInstance.ApplicableLicenses.Add(applicableLicensesRefId);
                        }
                    }

                    continue;
                }
                if (reader.ValueTextEquals("checkSum"u8))
                {
                    checkSumSeen = true;
                    reader.Read();

                    var checkSumScalarValue = reader.GetString();

                    if (checkSumScalarValue != null)
                    {
                        dtoInstance.CheckSum = checkSumScalarValue;
                    }

                    continue;
                }
                if (reader.ValueTextEquals("createdAt"u8))
                {
                    createdAtSeen = true;
                    reader.Read();

                    dtoInstance.CreatedAt = reader.GetDateTime();

                    continue;
                }
                if (reader.ValueTextEquals("downloadCount"u8))
                {
                    downloadCountSeen = true;
                    reader.Read();

                    dtoInstance.DownloadCount = reader.GetInt32();

                    continue;
                }
                if (reader.ValueTextEquals("fileExtension"u8))
                {
                    fileExtensionSeen = true;
                    reader.Read();

                    var fileExtensionScalarValue = reader.GetString();

                    if (fileExtensionScalarValue != null)
                    {
                        dtoInstance.FileExtension = fileExtensionScalarValue;
                    }

                    continue;
                }
                if (reader.ValueTextEquals("listed"u8))
                {
                    listedSeen = true;
                    reader.Read();

                    if (reader.TokenType != JsonTokenType.Null)
                    {
                        dtoInstance.Listed = reader.GetBoolean();
                    }

                    continue;
                }
                if (reader.ValueTextEquals("modifiedAt"u8))
                {
                    modifiedAtSeen = true;
                    reader.Read();

                    dtoInstance.ModifiedAt = reader.GetDateTime();

                    continue;
                }
                if (reader.ValueTextEquals("owner"u8))
                {
                    ownerSeen = true;
                    reader.Read();

                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        dtoInstance.Owner = Guid.Empty;
                        logger.LogDebug($"the PackageVersion.Owner property was not found in the Json. The value is set to Guid.Empty");
                    }
                    else
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var ownerRefId))
                        {
                            dtoInstance.Owner = ownerRefId;
                        }
                    }

                    continue;
                }
                if (reader.ValueTextEquals("publicationDate"u8))
                {
                    publicationDateSeen = true;
                    reader.Read();

                    dtoInstance.PublicationDate = reader.GetDateTime();

                    continue;
                }
                if (reader.ValueTextEquals("readme"u8))
                {
                    readmeSeen = true;
                    reader.Read();

                    dtoInstance.Readme = reader.GetString();

                    continue;
                }
                if (reader.ValueTextEquals("releaseNotes"u8))
                {
                    releaseNotesSeen = true;
                    reader.Read();

                    dtoInstance.ReleaseNotes = reader.GetString();

                    continue;
                }
                if (reader.ValueTextEquals("sizeInBytes"u8))
                {
                    sizeInBytesSeen = true;
                    reader.Read();

                    dtoInstance.SizeInBytes = reader.GetInt32();

                    continue;
                }
                if (reader.ValueTextEquals("version"u8))
                {
                    versionSeen = true;
                    reader.Read();

                    var versionScalarValue = reader.GetString();

                    if (versionScalarValue != null)
                    {
                        dtoInstance.Version = versionScalarValue;
                    }

                    continue;
                }

                reader.Skip();
            }

            if (!typeSeen)
            {
                throw new InvalidOperationException("The @type property is not available, the PackageVersionDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (!applicableLicensesSeen)
            {
                logger.LogDebug("the applicableLicenses Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (!checkSumSeen)
            {
                logger.LogDebug("the checkSum Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (!createdAtSeen)
            {
                logger.LogDebug("the createdAt Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (!downloadCountSeen)
            {
                logger.LogDebug("the downloadCount Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (!fileExtensionSeen)
            {
                logger.LogDebug("the fileExtension Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (!listedSeen)
            {
                logger.LogDebug("the listed Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (!modifiedAtSeen)
            {
                logger.LogDebug("the modifiedAt Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (!ownerSeen)
            {
                logger.LogDebug("the owner Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (!publicationDateSeen)
            {
                logger.LogDebug("the publicationDate Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (!readmeSeen)
            {
                logger.LogDebug("the readme Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (!releaseNotesSeen)
            {
                logger.LogDebug("the releaseNotes Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (!sizeInBytesSeen)
            {
                logger.LogDebug("the sizeInBytes Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (!versionSeen)
            {
                logger.LogDebug("the version Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }

            return dtoInstance;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
