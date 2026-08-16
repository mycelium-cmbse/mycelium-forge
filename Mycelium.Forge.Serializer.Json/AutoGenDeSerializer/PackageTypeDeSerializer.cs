// ------------------------------------------------------------------------------------------------
// <copyright file="PackageTypeDeSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="PackageTypeDeSerializer"/> is to provide deserialization capabilities
    /// for the <see cref="IPackageType"/> interface
    /// </summary>
    internal static class PackageTypeDeSerializer
    {
        /// <summary>
        /// Deserializes an instance of <see cref="IPackageType"/> from the provided <see cref="JsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="IPackageType"/> json object
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="IPackageType"/>
        /// </returns>
        internal static IPackageType DeSerialize(JsonElement jsonElement, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("PackageTypeDeSerializer");

            if (!jsonElement.TryGetProperty("@type"u8, out var @type))
            {
                throw new InvalidOperationException("The @type property is not available, the PackageTypeDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (@type.GetString() != "PackageType")
            {
                throw new InvalidOperationException($"The PackageTypeDeSerializer can only be used to deserialize objects of type IPackageType, a {@type.GetString()} was provided");
            }

            var dtoInstance = new Mycelium.Forge.Common.PackageType();

            if (jsonElement.TryGetProperty("@id"u8, out var idProperty))
            {
                var propertyValue = idProperty.GetString();

                if (propertyValue == null)
                {
                    throw new JsonException("The @id property is not present, the PackageType cannot be deserialized");
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
                logger.LogDebug("the createdAt Json property was not found in the PackageType: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("description"u8, out var descriptionProperty))
            {
                var propertyValue = descriptionProperty.GetString();

                if (propertyValue != null)
                {
                    dtoInstance.Description = propertyValue;
                }
            }
            else
            {
                logger.LogDebug("the description Json property was not found in the PackageType: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("modifiedAt"u8, out var modifiedAtProperty))
            {
                dtoInstance.ModifiedAt = modifiedAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the modifiedAt Json property was not found in the PackageType: {Id}", dtoInstance.Id);
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
                logger.LogDebug("the name Json property was not found in the PackageType: {Id}", dtoInstance.Id);
            }

            return dtoInstance;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
