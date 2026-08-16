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
        /// Deserializes an instance of <see cref="IPackageVersion"/> from the provided <see cref="JsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="IPackageVersion"/> json object
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="IPackageVersion"/>
        /// </returns>
        internal static IPackageVersion DeSerialize(JsonElement jsonElement, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("PackageVersionDeSerializer");

            if (!jsonElement.TryGetProperty("@type"u8, out var @type))
            {
                throw new InvalidOperationException("The @type property is not available, the PackageVersionDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (@type.GetString() != "PackageVersion")
            {
                throw new InvalidOperationException($"The PackageVersionDeSerializer can only be used to deserialize objects of type IPackageVersion, a {@type.GetString()} was provided");
            }

            var dtoInstance = new Mycelium.Forge.Common.PackageVersion();

            if (jsonElement.TryGetProperty("@id"u8, out var idProperty))
            {
                var propertyValue = idProperty.GetString();

                if (propertyValue == null)
                {
                    throw new JsonException("The @id property is not present, the PackageVersion cannot be deserialized");
                }
                else
                {
                    dtoInstance.Id = Guid.Parse(propertyValue);
                }
            }

            if (jsonElement.TryGetProperty("createdAt"u8, out var createdAtProperty))
            {
                dtoInstance.CreatedAt = createdAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the createdAt Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("downloadCount"u8, out var downloadCountProperty))
            {
                dtoInstance.DownloadCount = downloadCountProperty.GetInt32();
            }
            else
            {
                logger.LogDebug("the downloadCount Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("listed"u8, out var listedProperty))
            {
                if (listedProperty.ValueKind != JsonValueKind.Null)
                {
                    dtoInstance.Listed = listedProperty.GetBoolean();
                }
            }
            else
            {
                logger.LogDebug("the listed Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("metaData"u8, out var metaDataProperty))
            {
                if (metaDataProperty.ValueKind == JsonValueKind.Null)
                {
                    dtoInstance.MetaData = Guid.Empty;
                    logger.LogDebug($"the PackageVersion.MetaData property was not found in the Json. The value is set to Guid.Empty");
                }
                else
                {
                    if (metaDataProperty.TryGetProperty("@id"u8, out var metaDataExternalIdProperty))
                    {
                        var propertyValue = metaDataExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.MetaData = Guid.Parse(propertyValue);
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the metaData Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("modifiedAt"u8, out var modifiedAtProperty))
            {
                dtoInstance.ModifiedAt = modifiedAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the modifiedAt Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("publicationDate"u8, out var publicationDateProperty))
            {
                dtoInstance.PublicationDate = publicationDateProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the publicationDate Json property was not found in the PackageVersion: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("version"u8, out var versionProperty))
            {
                var propertyValue = versionProperty.GetString();

                if (propertyValue != null)
                {
                    dtoInstance.Version = propertyValue;
                }
            }
            else
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
