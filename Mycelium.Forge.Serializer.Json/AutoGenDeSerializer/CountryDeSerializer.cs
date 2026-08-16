// ------------------------------------------------------------------------------------------------
// <copyright file="CountryDeSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="CountryDeSerializer"/> is to provide deserialization capabilities
    /// for the <see cref="ICountry"/> interface
    /// </summary>
    internal static class CountryDeSerializer
    {
        /// <summary>
        /// Deserializes an instance of <see cref="ICountry"/> from the provided <see cref="JsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="ICountry"/> json object
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="ICountry"/>
        /// </returns>
        internal static ICountry DeSerialize(JsonElement jsonElement, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("CountryDeSerializer");

            if (!jsonElement.TryGetProperty("@type"u8, out var @type))
            {
                throw new InvalidOperationException("The @type property is not available, the CountryDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (@type.GetString() != "Country")
            {
                throw new InvalidOperationException($"The CountryDeSerializer can only be used to deserialize objects of type ICountry, a {@type.GetString()} was provided");
            }

            var dtoInstance = new Mycelium.Forge.Common.Country();

            if (jsonElement.TryGetProperty("@id"u8, out var idProperty))
            {
                var propertyValue = idProperty.GetString();

                if (propertyValue == null)
                {
                    throw new JsonException("The @id property is not present, the Country cannot be deserialized");
                }
                else
                {
                    dtoInstance.Id = Guid.Parse(propertyValue);
                }
            }

            if (jsonElement.TryGetProperty("alpha2Code"u8, out var alpha2CodeProperty))
            {
                var propertyValue = alpha2CodeProperty.GetString();

                if (propertyValue != null)
                {
                    dtoInstance.Alpha2Code = propertyValue;
                }
            }
            else
            {
                logger.LogDebug("the alpha2Code Json property was not found in the Country: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("alpha3Code"u8, out var Alpha3CodeProperty))
            {
                var propertyValue = Alpha3CodeProperty.GetString();

                if (propertyValue != null)
                {
                    dtoInstance.Alpha3Code = propertyValue;
                }
            }
            else
            {
                logger.LogDebug("the alpha3Code Json property was not found in the Country: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("createdAt"u8, out var createdAtProperty))
            {
                dtoInstance.CreatedAt = createdAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the createdAt Json property was not found in the Country: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("modifiedAt"u8, out var modifiedAtProperty))
            {
                dtoInstance.ModifiedAt = modifiedAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the modifiedAt Json property was not found in the Country: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("name"u8, out var NameProperty))
            {
                var propertyValue = NameProperty.GetString();

                if (propertyValue != null)
                {
                    dtoInstance.Name = propertyValue;
                }
            }
            else
            {
                logger.LogDebug("the name Json property was not found in the Country: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("numericCode"u8, out var NumericCodeProperty))
            {
                var propertyValue = NumericCodeProperty.GetString();

                if (propertyValue != null)
                {
                    dtoInstance.NumericCode = propertyValue;
                }
            }
            else
            {
                logger.LogDebug("the numericCode Json property was not found in the Country: {Id}", dtoInstance.Id);
            }

            return dtoInstance;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
