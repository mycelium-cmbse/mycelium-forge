// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeDeSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="ForgeDeSerializer"/> is to provide deserialization capabilities
    /// for the <see cref="IForge"/> interface
    /// </summary>
    internal static class ForgeDeSerializer
    {
        /// <summary>
        /// Deserializes an instance of <see cref="IForge"/> from the provided <see cref="Utf8JsonReader"/>
        /// </summary>
        /// <param name="reader">
        /// The <see cref="Utf8JsonReader"/>, positioned at the <see cref="IForge"/> json object's <see cref="JsonTokenType.StartObject"/> token
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="IForge"/>
        /// </returns>
        internal static IForge DeSerialize(ref Utf8JsonReader reader, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("ForgeDeSerializer");

            var dtoInstance = new Mycelium.Forge.Common.Forge();

            var typeSeen = false;
            var accountSeen = false;
            var administratorSeen = false;
            var countrySeen = false;
            var createdAtSeen = false;
            var descriptionSeen = false;
            var modifiedAtSeen = false;
            var nameSeen = false;
            var organizationSeen = false;
            var packageTypeSeen = false;
            var profileTypeSeen = false;
            var shortNameSeen = false;

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

                    if (typeValue != "Forge")
                    {
                        throw new InvalidOperationException($"The ForgeDeSerializer can only be used to deserialize objects of type IForge, a {typeValue} was provided");
                    }

                    continue;
                }

                if (reader.ValueTextEquals("@id"u8))
                {
                    reader.Read();

                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        throw new JsonException("The @id property is not present, the Forge cannot be deserialized");
                    }

                    dtoInstance.Id = Utf8JsonReaderHelper.ReadGuid(ref reader);
                    continue;
                }

                if (reader.ValueTextEquals("account"u8))
                {
                    accountSeen = true;
                    reader.Read();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var accountRefId))
                        {
                            dtoInstance.Account.Add(accountRefId);
                        }
                    }

                    continue;
                }
                if (reader.ValueTextEquals("administrator"u8))
                {
                    administratorSeen = true;
                    reader.Read();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var administratorRefId))
                        {
                            dtoInstance.Administrator.Add(administratorRefId);
                        }
                    }

                    continue;
                }
                if (reader.ValueTextEquals("country"u8))
                {
                    countrySeen = true;
                    reader.Read();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var countryRefId))
                        {
                            dtoInstance.Country.Add(countryRefId);
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
                if (reader.ValueTextEquals("description"u8))
                {
                    descriptionSeen = true;
                    reader.Read();

                    var descriptionScalarValue = reader.GetString();

                    if (descriptionScalarValue != null)
                    {
                        dtoInstance.Description = descriptionScalarValue;
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
                if (reader.ValueTextEquals("organization"u8))
                {
                    organizationSeen = true;
                    reader.Read();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var organizationRefId))
                        {
                            dtoInstance.Organization.Add(organizationRefId);
                        }
                    }

                    continue;
                }
                if (reader.ValueTextEquals("packageType"u8))
                {
                    packageTypeSeen = true;
                    reader.Read();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var packageTypeRefId))
                        {
                            dtoInstance.PackageType.Add(packageTypeRefId);
                        }
                    }

                    continue;
                }
                if (reader.ValueTextEquals("profileType"u8))
                {
                    profileTypeSeen = true;
                    reader.Read();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var profileTypeRefId))
                        {
                            dtoInstance.ProfileType.Add(profileTypeRefId);
                        }
                    }

                    continue;
                }
                if (reader.ValueTextEquals("shortName"u8))
                {
                    shortNameSeen = true;
                    reader.Read();

                    var shortNameScalarValue = reader.GetString();

                    if (shortNameScalarValue != null)
                    {
                        dtoInstance.ShortName = shortNameScalarValue;
                    }

                    continue;
                }

                reader.Skip();
            }

            if (!typeSeen)
            {
                throw new InvalidOperationException("The @type property is not available, the ForgeDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (!accountSeen)
            {
                logger.LogDebug("the account Json property was not found in the Forge: {Id}", dtoInstance.Id);
            }
            if (!administratorSeen)
            {
                logger.LogDebug("the administrator Json property was not found in the Forge: {Id}", dtoInstance.Id);
            }
            if (!countrySeen)
            {
                logger.LogDebug("the country Json property was not found in the Forge: {Id}", dtoInstance.Id);
            }
            if (!createdAtSeen)
            {
                logger.LogDebug("the createdAt Json property was not found in the Forge: {Id}", dtoInstance.Id);
            }
            if (!descriptionSeen)
            {
                logger.LogDebug("the description Json property was not found in the Forge: {Id}", dtoInstance.Id);
            }
            if (!modifiedAtSeen)
            {
                logger.LogDebug("the modifiedAt Json property was not found in the Forge: {Id}", dtoInstance.Id);
            }
            if (!nameSeen)
            {
                logger.LogDebug("the name Json property was not found in the Forge: {Id}", dtoInstance.Id);
            }
            if (!organizationSeen)
            {
                logger.LogDebug("the organization Json property was not found in the Forge: {Id}", dtoInstance.Id);
            }
            if (!packageTypeSeen)
            {
                logger.LogDebug("the packageType Json property was not found in the Forge: {Id}", dtoInstance.Id);
            }
            if (!profileTypeSeen)
            {
                logger.LogDebug("the profileType Json property was not found in the Forge: {Id}", dtoInstance.Id);
            }
            if (!shortNameSeen)
            {
                logger.LogDebug("the shortName Json property was not found in the Forge: {Id}", dtoInstance.Id);
            }

            return dtoInstance;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
