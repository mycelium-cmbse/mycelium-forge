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
        /// Deserializes an instance of <see cref="IProfileType"/> from the provided <see cref="Utf8JsonReader"/>
        /// </summary>
        /// <param name="reader">
        /// The <see cref="Utf8JsonReader"/>, positioned at the <see cref="IProfileType"/> json object's <see cref="JsonTokenType.StartObject"/> token
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="IProfileType"/>
        /// </returns>
        internal static IProfileType DeSerialize(ref Utf8JsonReader reader, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("ProfileTypeDeSerializer");

            var dtoInstance = new Mycelium.Forge.Common.ProfileType();

            var typeSeen = false;
            var createdAtSeen = false;
            var LogoBlobReferenceSeen = false;
            var modifiedAtSeen = false;
            var nameSeen = false;

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

                    if (typeValue != "ProfileType")
                    {
                        throw new InvalidOperationException($"The ProfileTypeDeSerializer can only be used to deserialize objects of type IProfileType, a {typeValue} was provided");
                    }

                    continue;
                }

                if (reader.ValueTextEquals("@id"u8))
                {
                    reader.Read();

                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        throw new JsonException("The @id property is not present, the ProfileType cannot be deserialized");
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
                if (reader.ValueTextEquals("logoBlobReference"u8))
                {
                    LogoBlobReferenceSeen = true;
                    reader.Read();

                    var LogoBlobReferenceScalarValue = reader.GetString();

                    if (LogoBlobReferenceScalarValue != null)
                    {
                        dtoInstance.LogoBlobReference = LogoBlobReferenceScalarValue;
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

                reader.Skip();
            }

            if (!typeSeen)
            {
                throw new InvalidOperationException("The @type property is not available, the ProfileTypeDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (!createdAtSeen)
            {
                logger.LogDebug("the createdAt Json property was not found in the ProfileType: {Id}", dtoInstance.Id);
            }
            if (!LogoBlobReferenceSeen)
            {
                logger.LogDebug("the logoBlobReference Json property was not found in the ProfileType: {Id}", dtoInstance.Id);
            }
            if (!modifiedAtSeen)
            {
                logger.LogDebug("the modifiedAt Json property was not found in the ProfileType: {Id}", dtoInstance.Id);
            }
            if (!nameSeen)
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
