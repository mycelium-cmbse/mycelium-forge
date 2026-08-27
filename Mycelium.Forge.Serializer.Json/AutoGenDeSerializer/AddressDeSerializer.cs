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
        /// Deserializes an instance of <see cref="IAddress"/> from the provided <see cref="Utf8JsonReader"/>
        /// </summary>
        /// <param name="reader">
        /// The <see cref="Utf8JsonReader"/>, positioned at the <see cref="IAddress"/> json object's <see cref="JsonTokenType.StartObject"/> token
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="IAddress"/>
        /// </returns>
        internal static IAddress DeSerialize(ref Utf8JsonReader reader, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("AddressDeSerializer");

            var dtoInstance = new Mycelium.Forge.Common.Address();

            var typeSeen = false;
            var addressLine1Seen = false;
            var addressLine2Seen = false;
            var countrySeen = false;
            var createdAtSeen = false;
            var localitySeen = false;
            var modifiedAtSeen = false;
            var postalCodeSeen = false;
            var regionSeen = false;

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

                    if (typeValue != "Address")
                    {
                        throw new InvalidOperationException($"The AddressDeSerializer can only be used to deserialize objects of type IAddress, a {typeValue} was provided");
                    }

                    continue;
                }

                if (reader.ValueTextEquals("@id"u8))
                {
                    reader.Read();

                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        throw new JsonException("The @id property is not present, the Address cannot be deserialized");
                    }

                    dtoInstance.Id = Utf8JsonReaderHelper.ReadGuid(ref reader);
                    continue;
                }

                if (reader.ValueTextEquals("addressLine1"u8))
                {
                    addressLine1Seen = true;
                    reader.Read();

                    var addressLine1ScalarValue = reader.GetString();

                    if (addressLine1ScalarValue != null)
                    {
                        dtoInstance.AddressLine1 = addressLine1ScalarValue;
                    }

                    continue;
                }
                if (reader.ValueTextEquals("addressLine2"u8))
                {
                    addressLine2Seen = true;
                    reader.Read();

                    dtoInstance.AddressLine2 = reader.GetString();

                    continue;
                }
                if (reader.ValueTextEquals("country"u8))
                {
                    countrySeen = true;
                    reader.Read();

                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        dtoInstance.Country = Guid.Empty;
                        logger.LogDebug($"the Address.Country property was not found in the Json. The value is set to Guid.Empty");
                    }
                    else
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var countryRefId))
                        {
                            dtoInstance.Country = countryRefId;
                        }
                    }

                    continue;
                }
                if (reader.ValueTextEquals("createdAt"u8))
                {
                    createdAtSeen = true;
                    reader.Read();

                    dtoInstance.CreatedAt = reader.GetDateTime();

                    continue;
                }
                if (reader.ValueTextEquals("locality"u8))
                {
                    localitySeen = true;
                    reader.Read();

                    var localityScalarValue = reader.GetString();

                    if (localityScalarValue != null)
                    {
                        dtoInstance.Locality = localityScalarValue;
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
                if (reader.ValueTextEquals("postalCode"u8))
                {
                    postalCodeSeen = true;
                    reader.Read();

                    dtoInstance.PostalCode = reader.GetString();

                    continue;
                }
                if (reader.ValueTextEquals("region"u8))
                {
                    regionSeen = true;
                    reader.Read();

                    dtoInstance.Region = reader.GetString();

                    continue;
                }

                reader.Skip();
            }

            if (!typeSeen)
            {
                throw new InvalidOperationException("The @type property is not available, the AddressDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (!addressLine1Seen)
            {
                logger.LogDebug("the addressLine1 Json property was not found in the Address: {Id}", dtoInstance.Id);
            }
            if (!addressLine2Seen)
            {
                logger.LogDebug("the addressLine2 Json property was not found in the Address: {Id}", dtoInstance.Id);
            }
            if (!countrySeen)
            {
                logger.LogDebug("the country Json property was not found in the Address: {Id}", dtoInstance.Id);
            }
            if (!createdAtSeen)
            {
                logger.LogDebug("the createdAt Json property was not found in the Address: {Id}", dtoInstance.Id);
            }
            if (!localitySeen)
            {
                logger.LogDebug("the locality Json property was not found in the Address: {Id}", dtoInstance.Id);
            }
            if (!modifiedAtSeen)
            {
                logger.LogDebug("the modifiedAt Json property was not found in the Address: {Id}", dtoInstance.Id);
            }
            if (!postalCodeSeen)
            {
                logger.LogDebug("the postalCode Json property was not found in the Address: {Id}", dtoInstance.Id);
            }
            if (!regionSeen)
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
