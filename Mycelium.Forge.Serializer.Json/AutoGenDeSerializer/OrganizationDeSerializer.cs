// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationDeSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="OrganizationDeSerializer"/> is to provide deserialization capabilities
    /// for the <see cref="IOrganization"/> interface
    /// </summary>
    internal static class OrganizationDeSerializer
    {
        /// <summary>
        /// Deserializes an instance of <see cref="IOrganization"/> from the provided <see cref="Utf8JsonReader"/>
        /// </summary>
        /// <param name="reader">
        /// The <see cref="Utf8JsonReader"/>, positioned at the <see cref="IOrganization"/> json object's <see cref="JsonTokenType.StartObject"/> token
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="IOrganization"/>
        /// </returns>
        internal static IOrganization DeSerialize(ref Utf8JsonReader reader, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("OrganizationDeSerializer");

            var dtoInstance = new Mycelium.Forge.Common.Organization();

            var typeSeen = false;
            var addressSeen = false;
            var administratorSeen = false;
            var billingEmailSeen = false;
            var createdAtSeen = false;
            var defaultPackageVisibilitySeen = false;
            var emailSeen = false;
            var logoBlobReferenceSeen = false;
            var memberSeen = false;
            var modifiedAtSeen = false;
            var nameSeen = false;
            var originSeen = false;
            var ownedPackageSeen = false;
            var ownerSeen = false;
            var primaryAddressSeen = false;
            var profileLinkSeen = false;
            var shortNameSeen = false;
            var statusSeen = false;
            var websiteSeen = false;

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

                    if (typeValue != "Organization")
                    {
                        throw new InvalidOperationException($"The OrganizationDeSerializer can only be used to deserialize objects of type IOrganization, a {typeValue} was provided");
                    }

                    continue;
                }

                if (reader.ValueTextEquals("@id"u8))
                {
                    reader.Read();

                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        throw new JsonException("The @id property is not present, the Organization cannot be deserialized");
                    }

                    dtoInstance.Id = Utf8JsonReaderHelper.ReadGuid(ref reader);
                    continue;
                }

                if (reader.ValueTextEquals("address"u8))
                {
                    addressSeen = true;
                    reader.Read();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var addressRefId))
                        {
                            dtoInstance.Address.Add(addressRefId);
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
                if (reader.ValueTextEquals("billingEmail"u8))
                {
                    billingEmailSeen = true;
                    reader.Read();

                    dtoInstance.BillingEmail = reader.GetString();

                    continue;
                }
                if (reader.ValueTextEquals("createdAt"u8))
                {
                    createdAtSeen = true;
                    reader.Read();

                    dtoInstance.CreatedAt = reader.GetDateTime();

                    continue;
                }
                if (reader.ValueTextEquals("defaultPackageVisibility"u8))
                {
                    defaultPackageVisibilitySeen = true;
                    reader.Read();

                    dtoInstance.DefaultPackageVisibility = VisibilityKindProvider.Parse(reader.GetString());

                    continue;
                }
                if (reader.ValueTextEquals("email"u8))
                {
                    emailSeen = true;
                    reader.Read();

                    var emailScalarValue = reader.GetString();

                    if (emailScalarValue != null)
                    {
                        dtoInstance.Email = emailScalarValue;
                    }

                    continue;
                }
                if (reader.ValueTextEquals("logoBlobReference"u8))
                {
                    logoBlobReferenceSeen = true;
                    reader.Read();

                    dtoInstance.LogoBlobReference = reader.GetString();

                    continue;
                }
                if (reader.ValueTextEquals("member"u8))
                {
                    memberSeen = true;
                    reader.Read();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var memberRefId))
                        {
                            dtoInstance.Member.Add(memberRefId);
                        }
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
                if (reader.ValueTextEquals("origin"u8))
                {
                    originSeen = true;
                    reader.Read();

                    var originScalarValue = reader.GetString();

                    if (originScalarValue != null)
                    {
                        dtoInstance.Origin = originScalarValue;
                    }

                    continue;
                }
                if (reader.ValueTextEquals("ownedPackage"u8))
                {
                    ownedPackageSeen = true;
                    reader.Read();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var ownedPackageRefId))
                        {
                            dtoInstance.OwnedPackage.Add(ownedPackageRefId);
                        }
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
                        logger.LogDebug($"the Organization.Owner property was not found in the Json. The value is set to Guid.Empty");
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
                if (reader.ValueTextEquals("primaryAddress"u8))
                {
                    primaryAddressSeen = true;
                    reader.Read();

                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        dtoInstance.PrimaryAddress = Guid.Empty;
                        logger.LogDebug($"the Organization.PrimaryAddress property was not found in the Json. The value is set to Guid.Empty");
                    }
                    else
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var primaryAddressRefId))
                        {
                            dtoInstance.PrimaryAddress = primaryAddressRefId;
                        }
                    }

                    continue;
                }
                if (reader.ValueTextEquals("profileLink"u8))
                {
                    profileLinkSeen = true;
                    reader.Read();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var profileLinkRefId))
                        {
                            dtoInstance.ProfileLink.Add(profileLinkRefId);
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
                if (reader.ValueTextEquals("status"u8))
                {
                    statusSeen = true;
                    reader.Read();

                    dtoInstance.Status = ScopeStatusKindProvider.Parse(reader.GetString());

                    continue;
                }
                if (reader.ValueTextEquals("website"u8))
                {
                    websiteSeen = true;
                    reader.Read();

                    dtoInstance.Website = reader.GetString();

                    continue;
                }

                reader.Skip();
            }

            if (!typeSeen)
            {
                throw new InvalidOperationException("The @type property is not available, the OrganizationDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (!addressSeen)
            {
                logger.LogDebug("the address Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (!administratorSeen)
            {
                logger.LogDebug("the administrator Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (!billingEmailSeen)
            {
                logger.LogDebug("the billingEmail Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (!createdAtSeen)
            {
                logger.LogDebug("the createdAt Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (!defaultPackageVisibilitySeen)
            {
                logger.LogDebug("the defaultPackageVisibility Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (!emailSeen)
            {
                logger.LogDebug("the email Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (!logoBlobReferenceSeen)
            {
                logger.LogDebug("the logoBlobReference Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (!memberSeen)
            {
                logger.LogDebug("the member Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (!modifiedAtSeen)
            {
                logger.LogDebug("the modifiedAt Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (!nameSeen)
            {
                logger.LogDebug("the name Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (!originSeen)
            {
                logger.LogDebug("the origin Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (!ownedPackageSeen)
            {
                logger.LogDebug("the ownedPackage Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (!ownerSeen)
            {
                logger.LogDebug("the owner Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (!primaryAddressSeen)
            {
                logger.LogDebug("the primaryAddress Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (!profileLinkSeen)
            {
                logger.LogDebug("the profileLink Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (!shortNameSeen)
            {
                logger.LogDebug("the shortName Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (!statusSeen)
            {
                logger.LogDebug("the status Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (!websiteSeen)
            {
                logger.LogDebug("the website Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }

            return dtoInstance;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
