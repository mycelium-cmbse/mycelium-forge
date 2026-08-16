// ------------------------------------------------------------------------------------------------
// <copyright file="AddressDeSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="AddressDeSerializer"/> is to provide deserialization capabilities
    /// for the <see cref="IAddress"/> interface
    /// </summary>
    internal static class AddressDeSerializer
    {
        /// <summary>
        /// Deserializes an instance of <see cref="IAddress"/> from the provided <see cref="JsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="IAddress"/> json object
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="IAddress"/>
        /// </returns>
        internal static IAddress DeSerialize(JsonElement jsonElement, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("AddressDeSerializer");

            if (!jsonElement.TryGetProperty("@type"u8, out var @type))
            {
                throw new InvalidOperationException("The @type property is not available, the AddressDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (@type.GetString() != "Address")
            {
                throw new InvalidOperationException($"The AddressDeSerializer can only be used to deserialize objects of type IAddress, a {@type.GetString()} was provided");
            }

            var dtoInstance = new Mycelium.Forge.Common.Address();

            if (jsonElement.TryGetProperty("@id"u8, out var idProperty))
            {
                var propertyValue = idProperty.GetString();

                if (propertyValue == null)
                {
                    throw new JsonException("The @id property is not present, the Address cannot be deserialized");
                }
                else
                {
                    dtoInstance.Id = Guid.Parse(propertyValue);
                }
            }

            if (jsonElement.TryGetProperty("addressLine1"u8, out var addressLine1Property))
            {
                var propertyValue = addressLine1Property.GetString();

                if (propertyValue != null)
                {
                    dtoInstance.AddressLine1 = propertyValue;
                }
            }
            else
            {
                logger.LogDebug("the addressLine1 Json property was not found in the Address: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("addressLine2"u8, out var addressLine2Property))
            {
                dtoInstance.AddressLine2 = addressLine2Property.GetString();
            }
            else
            {
                logger.LogDebug("the addressLine2 Json property was not found in the Address: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("country"u8, out var countryProperty))
            {
                if (countryProperty.ValueKind == JsonValueKind.Null)
                {
                    dtoInstance.Country = Guid.Empty;
                    logger.LogDebug($"the Address.Country property was not found in the Json. The value is set to Guid.Empty");
                }
                else
                {
                    if (countryProperty.TryGetProperty("@id"u8, out var countryExternalIdProperty))
                    {
                        var propertyValue = countryExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.Country = Guid.Parse(propertyValue);
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the country Json property was not found in the Address: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("createdAt"u8, out var createdAtProperty))
            {
                dtoInstance.CreatedAt = createdAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the createdAt Json property was not found in the Address: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("locality"u8, out var localityProperty))
            {
                var propertyValue = localityProperty.GetString();

                if (propertyValue != null)
                {
                    dtoInstance.Locality = propertyValue;
                }
            }
            else
            {
                logger.LogDebug("the locality Json property was not found in the Address: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("modifiedAt"u8, out var modifiedAtProperty))
            {
                dtoInstance.ModifiedAt = modifiedAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the modifiedAt Json property was not found in the Address: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("postalCode"u8, out var postalCodeProperty))
            {
                dtoInstance.PostalCode = postalCodeProperty.GetString();
            }
            else
            {
                logger.LogDebug("the postalCode Json property was not found in the Address: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("region"u8, out var regionProperty))
            {
                dtoInstance.Region = regionProperty.GetString();
            }
            else
            {
                logger.LogDebug("the region Json property was not found in the Address: {Id}", dtoInstance.Id);
            }

            return dtoInstance;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
