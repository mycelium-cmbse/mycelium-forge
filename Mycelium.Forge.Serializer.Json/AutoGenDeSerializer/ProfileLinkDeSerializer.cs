// ------------------------------------------------------------------------------------------------
// <copyright file="ProfileLinkDeSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="ProfileLinkDeSerializer"/> is to provide deserialization capabilities
    /// for the <see cref="IProfileLink"/> interface
    /// </summary>
    internal static class ProfileLinkDeSerializer
    {
        /// <summary>
        /// Deserializes an instance of <see cref="IProfileLink"/> from the provided <see cref="JsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="IProfileLink"/> json object
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="IProfileLink"/>
        /// </returns>
        internal static IProfileLink DeSerialize(JsonElement jsonElement, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("ProfileLinkDeSerializer");

            if (!jsonElement.TryGetProperty("@type"u8, out var @type))
            {
                throw new InvalidOperationException("The @type property is not available, the ProfileLinkDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (@type.GetString() != "ProfileLink")
            {
                throw new InvalidOperationException($"The ProfileLinkDeSerializer can only be used to deserialize objects of type IProfileLink, a {@type.GetString()} was provided");
            }

            var dtoInstance = new Mycelium.Forge.Common.ProfileLink();

            if (jsonElement.TryGetProperty("@id"u8, out var idProperty))
            {
                var propertyValue = idProperty.GetString();

                if (propertyValue == null)
                {
                    throw new JsonException("The @id property is not present, the ProfileLink cannot be deserialized");
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
                logger.LogDebug("the createdAt Json property was not found in the ProfileLink: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("modifiedAt"u8, out var modifiedAtProperty))
            {
                dtoInstance.ModifiedAt = modifiedAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the modifiedAt Json property was not found in the ProfileLink: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("profileType"u8, out var profileTypeProperty))
            {
                if (profileTypeProperty.ValueKind == JsonValueKind.Null)
                {
                    dtoInstance.ProfileType = Guid.Empty;
                    logger.LogDebug($"the ProfileLink.ProfileType property was not found in the Json. The value is set to Guid.Empty");
                }
                else
                {
                    if (profileTypeProperty.TryGetProperty("@id"u8, out var profileTypeExternalIdProperty))
                    {
                        var propertyValue = profileTypeExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.ProfileType = Guid.Parse(propertyValue);
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the profileType Json property was not found in the ProfileLink: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("uri"u8, out var uriProperty))
            {
                var propertyValue = uriProperty.GetString();

                if (propertyValue != null)
                {
                    dtoInstance.Uri = propertyValue;
                }
            }
            else
            {
                logger.LogDebug("the uri Json property was not found in the ProfileLink: {Id}", dtoInstance.Id);
            }

            return dtoInstance;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
