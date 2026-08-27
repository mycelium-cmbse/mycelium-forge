// ------------------------------------------------------------------------------------------------
// <copyright file="PackageDeSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="PackageDeSerializer"/> is to provide deserialization capabilities
    /// for the <see cref="IPackage"/> interface
    /// </summary>
    internal static class PackageDeSerializer
    {
        /// <summary>
        /// Deserializes an instance of <see cref="IPackage"/> from the provided <see cref="Utf8JsonReader"/>
        /// </summary>
        /// <param name="reader">
        /// The <see cref="Utf8JsonReader"/>, positioned at the <see cref="IPackage"/> json object's <see cref="JsonTokenType.StartObject"/> token
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="IPackage"/>
        /// </returns>
        internal static IPackage DeSerialize(ref Utf8JsonReader reader, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("PackageDeSerializer");

            var dtoInstance = new Mycelium.Forge.Common.Package();

            var typeSeen = false;
            var createdAtSeen = false;
            var listedSeen = false;
            var modifiedAtSeen = false;
            var nameSeen = false;
            var packageMaintainerSeen = false;
            var packageOwnerSeen = false;
            var packageTypeSeen = false;
            var shortNameSeen = false;
            var versionSeen = false;
            var visibilitySeen = false;

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

                    if (typeValue != "Package")
                    {
                        throw new InvalidOperationException($"The PackageDeSerializer can only be used to deserialize objects of type IPackage, a {typeValue} was provided");
                    }

                    continue;
                }

                if (reader.ValueTextEquals("@id"u8))
                {
                    reader.Read();

                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        throw new JsonException("The @id property is not present, the Package cannot be deserialized");
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
                if (reader.ValueTextEquals("name"u8))
                {
                    nameSeen = true;
                    reader.Read();

                    var nameScalarValue = reader.GetString();

                    if (nameScalarValue != null)
                    {
                        dtoInstance.Name = nameScalarValue;
                    }

                    continue;
                }
                if (reader.ValueTextEquals("packageMaintainer"u8))
                {
                    packageMaintainerSeen = true;
                    reader.Read();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var packageMaintainerRefId))
                        {
                            dtoInstance.PackageMaintainer.Add(packageMaintainerRefId);
                        }
                    }

                    continue;
                }
                if (reader.ValueTextEquals("packageOwner"u8))
                {
                    packageOwnerSeen = true;
                    reader.Read();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var packageOwnerRefId))
                        {
                            dtoInstance.PackageOwner.Add(packageOwnerRefId);
                        }
                    }

                    continue;
                }
                if (reader.ValueTextEquals("packageType"u8))
                {
                    packageTypeSeen = true;
                    reader.Read();

                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        dtoInstance.PackageType = Guid.Empty;
                        logger.LogDebug($"the Package.PackageType property was not found in the Json. The value is set to Guid.Empty");
                    }
                    else
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var packageTypeRefId))
                        {
                            dtoInstance.PackageType = packageTypeRefId;
                        }
                    }

                    continue;
                }
                if (reader.ValueTextEquals("shortName"u8))
                {
                    shortNameSeen = true;
                    reader.Read();

                    var shortNameScalarValue = reader.GetString();

                    if (shortNameScalarValue != null)
                    {
                        dtoInstance.ShortName = shortNameScalarValue;
                    }

                    continue;
                }
                if (reader.ValueTextEquals("version"u8))
                {
                    versionSeen = true;
                    reader.Read();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var versionRefId))
                        {
                            dtoInstance.Version.Add(versionRefId);
                        }
                    }

                    continue;
                }
                if (reader.ValueTextEquals("visibility"u8))
                {
                    visibilitySeen = true;
                    reader.Read();

                    dtoInstance.Visibility = VisibilityKindProvider.Parse(reader.GetString());

                    continue;
                }

                reader.Skip();
            }

            if (!typeSeen)
            {
                throw new InvalidOperationException("The @type property is not available, the PackageDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (!createdAtSeen)
            {
                logger.LogDebug("the createdAt Json property was not found in the Package: {Id}", dtoInstance.Id);
            }
            if (!listedSeen)
            {
                logger.LogDebug("the listed Json property was not found in the Package: {Id}", dtoInstance.Id);
            }
            if (!modifiedAtSeen)
            {
                logger.LogDebug("the modifiedAt Json property was not found in the Package: {Id}", dtoInstance.Id);
            }
            if (!nameSeen)
            {
                logger.LogDebug("the name Json property was not found in the Package: {Id}", dtoInstance.Id);
            }
            if (!packageMaintainerSeen)
            {
                logger.LogDebug("the packageMaintainer Json property was not found in the Package: {Id}", dtoInstance.Id);
            }
            if (!packageOwnerSeen)
            {
                logger.LogDebug("the packageOwner Json property was not found in the Package: {Id}", dtoInstance.Id);
            }
            if (!packageTypeSeen)
            {
                logger.LogDebug("the packageType Json property was not found in the Package: {Id}", dtoInstance.Id);
            }
            if (!shortNameSeen)
            {
                logger.LogDebug("the shortName Json property was not found in the Package: {Id}", dtoInstance.Id);
            }
            if (!versionSeen)
            {
                logger.LogDebug("the version Json property was not found in the Package: {Id}", dtoInstance.Id);
            }
            if (!visibilitySeen)
            {
                logger.LogDebug("the visibility Json property was not found in the Package: {Id}", dtoInstance.Id);
            }

            return dtoInstance;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
