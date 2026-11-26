// ------------------------------------------------------------------------------------------------
// <copyright file="LicenseDeSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="LicenseDeSerializer"/> is to provide deserialization capabilities
    /// for the <see cref="ILicense"/> interface
    /// </summary>
    internal static class LicenseDeSerializer
    {
        /// <summary>
        /// Deserializes an instance of <see cref="ILicense"/> from the provided <see cref="Utf8JsonReader"/>
        /// </summary>
        /// <param name="reader">
        /// The <see cref="Utf8JsonReader"/>, positioned at the <see cref="ILicense"/> json object's <see cref="JsonTokenType.StartObject"/> token
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="ILicense"/>
        /// </returns>
        internal static ILicense DeSerialize(ref Utf8JsonReader reader, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("LicenseDeSerializer");

            var dtoInstance = new Mycelium.Forge.Common.License();

            var typeSeen = false;
            var createdAtSeen = false;
            var licenseTextSeen = false;
            var modifiedAtSeen = false;
            var nameSeen = false;
            var spdxIdentifierSeen = false;

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

                    if (typeValue != "License")
                    {
                        throw new InvalidOperationException($"The LicenseDeSerializer can only be used to deserialize objects of type ILicense, a {typeValue} was provided");
                    }

                    continue;
                }

                if (reader.ValueTextEquals("@id"u8))
                {
                    reader.Read();

                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        throw new JsonException("The @id property is not present, the License cannot be deserialized");
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
                if (reader.ValueTextEquals("licenseText"u8))
                {
                    licenseTextSeen = true;
                    reader.Read();

                    var licenseTextScalarValue = reader.GetString();

                    if (licenseTextScalarValue != null)
                    {
                        dtoInstance.LicenseText = licenseTextScalarValue;
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
                if (reader.ValueTextEquals("spdxIdentifier"u8))
                {
                    spdxIdentifierSeen = true;
                    reader.Read();

                    var spdxIdentifierScalarValue = reader.GetString();

                    if (spdxIdentifierScalarValue != null)
                    {
                        dtoInstance.SpdxIdentifier = spdxIdentifierScalarValue;
                    }

                    continue;
                }

                reader.Skip();
            }

            if (!typeSeen)
            {
                throw new InvalidOperationException("The @type property is not available, the LicenseDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (!createdAtSeen)
            {
                logger.LogDebug("the createdAt Json property was not found in the License: {Id}", dtoInstance.Id);
            }
            if (!licenseTextSeen)
            {
                logger.LogDebug("the licenseText Json property was not found in the License: {Id}", dtoInstance.Id);
            }
            if (!modifiedAtSeen)
            {
                logger.LogDebug("the modifiedAt Json property was not found in the License: {Id}", dtoInstance.Id);
            }
            if (!nameSeen)
            {
                logger.LogDebug("the name Json property was not found in the License: {Id}", dtoInstance.Id);
            }
            if (!spdxIdentifierSeen)
            {
                logger.LogDebug("the spdxIdentifier Json property was not found in the License: {Id}", dtoInstance.Id);
            }

            return dtoInstance;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
