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
            var createdAtSeen = false;
            var downloadCountSeen = false;
            var listedSeen = false;
            var metaDataSeen = false;
            var modifiedAtSeen = false;
            var publicationDateSeen = false;
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
                if (reader.ValueTextEquals("metaData"u8))
                {
                    metaDataSeen = true;
                    reader.Read();

                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        dtoInstance.MetaData = Guid.Empty;
                        logger.LogDebug($"the PackageVersion.MetaData property was not found in the Json. The value is set to Guid.Empty");
                    }
                    else
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var metaDataRefId))
                        {
                            dtoInstance.MetaData = metaDataRefId;
                        }
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
                if (reader.ValueTextEquals("publicationDate"u8))
                {
                    publicationDateSeen = true;
                    reader.Read();

                    dtoInstance.PublicationDate = reader.GetDateTime();

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

            if (!createdAtSeen)
            {
                logger.LogDebug("the createdAt Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (!downloadCountSeen)
            {
                logger.LogDebug("the downloadCount Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (!listedSeen)
            {
                logger.LogDebug("the listed Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (!metaDataSeen)
            {
                logger.LogDebug("the metaData Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (!modifiedAtSeen)
            {
                logger.LogDebug("the modifiedAt Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (!publicationDateSeen)
            {
                logger.LogDebug("the publicationDate Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
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
