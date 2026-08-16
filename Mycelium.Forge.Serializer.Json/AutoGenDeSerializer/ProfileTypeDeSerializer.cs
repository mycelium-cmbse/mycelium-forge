// ------------------------------------------------------------------------------------------------
// <copyright file="ProfileTypeDeSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="ProfileTypeDeSerializer"/> is to provide deserialization capabilities
    /// for the <see cref="IProfileType"/> interface
    /// </summary>
    internal static class ProfileTypeDeSerializer
    {
        /// <summary>
        /// Deserializes an instance of <see cref="IProfileType"/> from the provided <see cref="JsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="IProfileType"/> json object
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="IProfileType"/>
        /// </returns>
        internal static IProfileType DeSerialize(JsonElement jsonElement, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("ProfileTypeDeSerializer");

            if (!jsonElement.TryGetProperty("@type"u8, out var @type))
            {
                throw new InvalidOperationException("The @type property is not available, the ProfileTypeDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (@type.GetString() != "ProfileType")
            {
                throw new InvalidOperationException($"The ProfileTypeDeSerializer can only be used to deserialize objects of type IProfileType, a {@type.GetString()} was provided");
            }

            var dtoInstance = new Mycelium.Forge.Common.ProfileType();

            if (jsonElement.TryGetProperty("@id"u8, out var idProperty))
            {
                var propertyValue = idProperty.GetString();

                if (propertyValue == null)
                {
                    throw new JsonException("The @id property is not present, the ProfileType cannot be deserialized");
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
                logger.LogDebug("the createdAt Json property was not found in the ProfileType: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("logoBlobReference"u8, out var LogoBlobReferenceProperty))
            {
                var propertyValue = LogoBlobReferenceProperty.GetString();

                if (propertyValue != null)
                {
                    dtoInstance.LogoBlobReference = propertyValue;
                }
            }
            else
            {
                logger.LogDebug("the logoBlobReference Json property was not found in the ProfileType: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("modifiedAt"u8, out var modifiedAtProperty))
            {
                dtoInstance.ModifiedAt = modifiedAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the modifiedAt Json property was not found in the ProfileType: {Id}", dtoInstance.Id);
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
                logger.LogDebug("the name Json property was not found in the ProfileType: {Id}", dtoInstance.Id);
            }

            return dtoInstance;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
