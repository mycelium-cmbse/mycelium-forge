// ------------------------------------------------------------------------------------------------
// <copyright file="APIKeyDeSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="APIKeyDeSerializer"/> is to provide deserialization capabilities
    /// for the <see cref="IAPIKey"/> interface
    /// </summary>
    internal static class APIKeyDeSerializer
    {
        /// <summary>
        /// Deserializes an instance of <see cref="IAPIKey"/> from the provided <see cref="Utf8JsonReader"/>
        /// </summary>
        /// <param name="reader">
        /// The <see cref="Utf8JsonReader"/>, positioned at the <see cref="IAPIKey"/> json object's <see cref="JsonTokenType.StartObject"/> token
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="IAPIKey"/>
        /// </returns>
        internal static IAPIKey DeSerialize(ref Utf8JsonReader reader, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("APIKeyDeSerializer");

            var dtoInstance = new Mycelium.Forge.Common.APIKey();

            var typeSeen = false;
            var createdAtSeen = false;
            var expiresAtSeen = false;
            var lastUsedAtSeen = false;
            var modifiedAtSeen = false;
            var nameSeen = false;
            var permissionsSeen = false;
            var revokedAtSeen = false;
            var secretHashSeen = false;

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

                    if (typeValue != "APIKey")
                    {
                        throw new InvalidOperationException($"The APIKeyDeSerializer can only be used to deserialize objects of type IAPIKey, a {typeValue} was provided");
                    }

                    continue;
                }

                if (reader.ValueTextEquals("@id"u8))
                {
                    reader.Read();

                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        throw new JsonException("The @id property is not present, the APIKey cannot be deserialized");
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
                if (reader.ValueTextEquals("expiresAt"u8))
                {
                    expiresAtSeen = true;
                    reader.Read();

                    dtoInstance.ExpiresAt = reader.GetDateTime();

                    continue;
                }
                if (reader.ValueTextEquals("lastUsedAt"u8))
                {
                    lastUsedAtSeen = true;
                    reader.Read();

                    dtoInstance.LastUsedAt = reader.GetDateTime();

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
                if (reader.ValueTextEquals("permissions"u8))
                {
                    permissionsSeen = true;
                    reader.Read();

                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        dtoInstance.Permissions = Guid.Empty;
                        logger.LogDebug($"the APIKey.Permissions property was not found in the Json. The value is set to Guid.Empty");
                    }
                    else
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var permissionsRefId))
                        {
                            dtoInstance.Permissions = permissionsRefId;
                        }
                    }

                    continue;
                }
                if (reader.ValueTextEquals("revokedAt"u8))
                {
                    revokedAtSeen = true;
                    reader.Read();

                    dtoInstance.RevokedAt = reader.GetDateTime();

                    continue;
                }
                if (reader.ValueTextEquals("secretHash"u8))
                {
                    secretHashSeen = true;
                    reader.Read();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        dtoInstance.SecretHash.Add(reader.GetByte());
                    }

                    continue;
                }

                reader.Skip();
            }

            if (!typeSeen)
            {
                throw new InvalidOperationException("The @type property is not available, the APIKeyDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (!createdAtSeen)
            {
                logger.LogDebug("the createdAt Json property was not found in the APIKey: {Id}", dtoInstance.Id);
            }
            if (!expiresAtSeen)
            {
                logger.LogDebug("the expiresAt Json property was not found in the APIKey: {Id}", dtoInstance.Id);
            }
            if (!lastUsedAtSeen)
            {
                logger.LogDebug("the lastUsedAt Json property was not found in the APIKey: {Id}", dtoInstance.Id);
            }
            if (!modifiedAtSeen)
            {
                logger.LogDebug("the modifiedAt Json property was not found in the APIKey: {Id}", dtoInstance.Id);
            }
            if (!nameSeen)
            {
                logger.LogDebug("the name Json property was not found in the APIKey: {Id}", dtoInstance.Id);
            }
            if (!permissionsSeen)
            {
                logger.LogDebug("the permissions Json property was not found in the APIKey: {Id}", dtoInstance.Id);
            }
            if (!revokedAtSeen)
            {
                logger.LogDebug("the revokedAt Json property was not found in the APIKey: {Id}", dtoInstance.Id);
            }
            if (!secretHashSeen)
            {
                logger.LogDebug("the secretHash Json property was not found in the APIKey: {Id}", dtoInstance.Id);
            }

            return dtoInstance;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
