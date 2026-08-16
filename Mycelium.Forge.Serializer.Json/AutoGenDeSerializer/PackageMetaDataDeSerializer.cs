// ------------------------------------------------------------------------------------------------
// <copyright file="PackageMetaDataDeSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="PackageMetaDataDeSerializer"/> is to provide deserialization capabilities
    /// for the <see cref="IPackageMetaData"/> interface
    /// </summary>
    internal static class PackageMetaDataDeSerializer
    {
        /// <summary>
        /// Deserializes an instance of <see cref="IPackageMetaData"/> from the provided <see cref="JsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="IPackageMetaData"/> json object
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="IPackageMetaData"/>
        /// </returns>
        internal static IPackageMetaData DeSerialize(JsonElement jsonElement, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("PackageMetaDataDeSerializer");

            if (!jsonElement.TryGetProperty("@type"u8, out var @type))
            {
                throw new InvalidOperationException("The @type property is not available, the PackageMetaDataDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (@type.GetString() != "PackageMetaData")
            {
                throw new InvalidOperationException($"The PackageMetaDataDeSerializer can only be used to deserialize objects of type IPackageMetaData, a {@type.GetString()} was provided");
            }

            var dtoInstance = new Mycelium.Forge.Common.PackageMetaData();

            if (jsonElement.TryGetProperty("@id"u8, out var idProperty))
            {
                var propertyValue = idProperty.GetString();

                if (propertyValue == null)
                {
                    throw new JsonException("The @id property is not present, the PackageMetaData cannot be deserialized");
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
                logger.LogDebug("the createdAt Json property was not found in the PackageMetaData: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("modifiedAt"u8, out var modifiedAtProperty))
            {
                dtoInstance.ModifiedAt = modifiedAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the modifiedAt Json property was not found in the PackageMetaData: {Id}", dtoInstance.Id);
            }

            return dtoInstance;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
