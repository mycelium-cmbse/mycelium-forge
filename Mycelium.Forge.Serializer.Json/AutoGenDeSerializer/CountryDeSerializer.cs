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
        /// Deserializes an instance of <see cref="ICountry"/> from the provided <see cref="Utf8JsonReader"/>
        /// </summary>
        /// <param name="reader">
        /// The <see cref="Utf8JsonReader"/>, positioned at the <see cref="ICountry"/> json object's <see cref="JsonTokenType.StartObject"/> token
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="ICountry"/>
        /// </returns>
        internal static ICountry DeSerialize(ref Utf8JsonReader reader, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("CountryDeSerializer");

            var dtoInstance = new Mycelium.Forge.Common.Country();

            var typeSeen = false;
            var alpha2CodeSeen = false;
            var alpha3CodeSeen = false;
            var createdAtSeen = false;
            var modifiedAtSeen = false;
            var nameSeen = false;
            var numericCodeSeen = false;
            var ownerSeen = false;

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

                    if (typeValue != "Country")
                    {
                        throw new InvalidOperationException($"The CountryDeSerializer can only be used to deserialize objects of type ICountry, a {typeValue} was provided");
                    }

                    continue;
                }

                if (reader.ValueTextEquals("@id"u8))
                {
                    reader.Read();

                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        throw new JsonException("The @id property is not present, the Country cannot be deserialized");
                    }

                    dtoInstance.Id = Utf8JsonReaderHelper.ReadGuid(ref reader);
                    continue;
                }

                if (reader.ValueTextEquals("alpha2Code"u8))
                {
                    alpha2CodeSeen = true;
                    reader.Read();

                    var alpha2CodeScalarValue = reader.GetString();

                    if (alpha2CodeScalarValue != null)
                    {
                        dtoInstance.Alpha2Code = alpha2CodeScalarValue;
                    }

                    continue;
                }
                if (reader.ValueTextEquals("alpha3Code"u8))
                {
                    alpha3CodeSeen = true;
                    reader.Read();

                    var alpha3CodeScalarValue = reader.GetString();

                    if (alpha3CodeScalarValue != null)
                    {
                        dtoInstance.Alpha3Code = alpha3CodeScalarValue;
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
                if (reader.ValueTextEquals("numericCode"u8))
                {
                    numericCodeSeen = true;
                    reader.Read();

                    var numericCodeScalarValue = reader.GetString();

                    if (numericCodeScalarValue != null)
                    {
                        dtoInstance.NumericCode = numericCodeScalarValue;
                    }

                    continue;
                }
                if (reader.ValueTextEquals("owner"u8))
                {
                    ownerSeen = true;
                    reader.Read();

                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        dtoInstance.Owner = Guid.Empty;
                        logger.LogDebug($"the Country.Owner property was not found in the Json. The value is set to Guid.Empty");
                    }
                    else
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var ownerRefId))
                        {
                            dtoInstance.Owner = ownerRefId;
                        }
                    }

                    continue;
                }

                reader.Skip();
            }

            if (!typeSeen)
            {
                throw new InvalidOperationException("The @type property is not available, the CountryDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (!alpha2CodeSeen)
            {
                logger.LogDebug("the alpha2Code Json property was not found in the Country: {Id}", dtoInstance.Id);
            }
            if (!alpha3CodeSeen)
            {
                logger.LogDebug("the alpha3Code Json property was not found in the Country: {Id}", dtoInstance.Id);
            }
            if (!createdAtSeen)
            {
                logger.LogDebug("the createdAt Json property was not found in the Country: {Id}", dtoInstance.Id);
            }
            if (!modifiedAtSeen)
            {
                logger.LogDebug("the modifiedAt Json property was not found in the Country: {Id}", dtoInstance.Id);
            }
            if (!nameSeen)
            {
                logger.LogDebug("the name Json property was not found in the Country: {Id}", dtoInstance.Id);
            }
            if (!numericCodeSeen)
            {
                logger.LogDebug("the numericCode Json property was not found in the Country: {Id}", dtoInstance.Id);
            }
            if (!ownerSeen)
            {
                logger.LogDebug("the owner Json property was not found in the Country: {Id}", dtoInstance.Id);
            }

            return dtoInstance;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
