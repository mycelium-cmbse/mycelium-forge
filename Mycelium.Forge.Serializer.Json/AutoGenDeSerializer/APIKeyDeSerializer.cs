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
        /// Deserializes an instance of <see cref="IAPIKey"/> from the provided <see cref="JsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="IAPIKey"/> json object
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="IAPIKey"/>
        /// </returns>
        internal static IAPIKey DeSerialize(JsonElement jsonElement, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("APIKeyDeSerializer");

            if (!jsonElement.TryGetProperty("@type"u8, out var @type))
            {
                throw new InvalidOperationException("The @type property is not available, the APIKeyDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (@type.GetString() != "APIKey")
            {
                throw new InvalidOperationException($"The APIKeyDeSerializer can only be used to deserialize objects of type IAPIKey, a {@type.GetString()} was provided");
            }

            var dtoInstance = new Mycelium.Forge.Common.APIKey();

            if (jsonElement.TryGetProperty("@id"u8, out var idProperty))
            {
                var propertyValue = idProperty.GetString();

                if (propertyValue == null)
                {
                    throw new JsonException("The @id property is not present, the APIKey cannot be deserialized");
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
                logger.LogDebug("the createdAt Json property was not found in the APIKey: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("expiresAt"u8, out var expiresAtProperty))
            {
                dtoInstance.ExpiresAt = expiresAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the expiresAt Json property was not found in the APIKey: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("lastUsedAt"u8, out var lastUsedAtProperty))
            {
                dtoInstance.LastUsedAt = lastUsedAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the lastUsedAt Json property was not found in the APIKey: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("modifiedAt"u8, out var modifiedAtProperty))
            {
                dtoInstance.ModifiedAt = modifiedAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the modifiedAt Json property was not found in the APIKey: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("name"u8, out var nameProperty))
            {
                var propertyValue = nameProperty.GetString();

                if (propertyValue != null)
                {
                    dtoInstance.Name = propertyValue;
                }
            }
            else
            {
                logger.LogDebug("the name Json property was not found in the APIKey: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("permissions"u8, out var permissionsProperty))
            {
                if (permissionsProperty.ValueKind == JsonValueKind.Null)
                {
                    dtoInstance.Permissions = Guid.Empty;
                    logger.LogDebug($"the APIKey.Permissions property was not found in the Json. The value is set to Guid.Empty");
                }
                else
                {
                    if (permissionsProperty.TryGetProperty("@id"u8, out var permissionsExternalIdProperty))
                    {
                        var propertyValue = permissionsExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.Permissions = Guid.Parse(propertyValue);
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the permissions Json property was not found in the APIKey: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("revokedAt"u8, out var revokedAtProperty))
            {
                dtoInstance.RevokedAt = revokedAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the revokedAt Json property was not found in the APIKey: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("secretHash"u8, out var secretHashProperty))
            {
                foreach (var arrayItem in secretHashProperty.EnumerateArray())
                {
                    throw new NotImplementedException("Enumerable Numeric - APIKey.secretHash is not yet supported");
                }
            }
            else
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
