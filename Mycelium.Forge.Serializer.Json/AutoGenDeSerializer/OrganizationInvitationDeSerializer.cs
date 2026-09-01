// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationInvitationDeSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="OrganizationInvitationDeSerializer"/> is to provide deserialization capabilities
    /// for the <see cref="IOrganizationInvitation"/> interface
    /// </summary>
    internal static class OrganizationInvitationDeSerializer
    {
        /// <summary>
        /// Deserializes an instance of <see cref="IOrganizationInvitation"/> from the provided <see cref="Utf8JsonReader"/>
        /// </summary>
        /// <param name="reader">
        /// The <see cref="Utf8JsonReader"/>, positioned at the <see cref="IOrganizationInvitation"/> json object's <see cref="JsonTokenType.StartObject"/> token
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="IOrganizationInvitation"/>
        /// </returns>
        internal static IOrganizationInvitation DeSerialize(ref Utf8JsonReader reader, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("OrganizationInvitationDeSerializer");

            var dtoInstance = new Mycelium.Forge.Common.OrganizationInvitation();

            var typeSeen = false;
            var createdAtSeen = false;
            var experisAtSeen = false;
            var modifiedAtSeen = false;
            var organizationSeen = false;
            var organizationInvitationKindSeen = false;
            var ownerSeen = false;
            var statusSeen = false;
            var targetSeen = false;

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

                    if (typeValue != "OrganizationInvitation")
                    {
                        throw new InvalidOperationException($"The OrganizationInvitationDeSerializer can only be used to deserialize objects of type IOrganizationInvitation, a {typeValue} was provided");
                    }

                    continue;
                }

                if (reader.ValueTextEquals("@id"u8))
                {
                    reader.Read();

                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        throw new JsonException("The @id property is not present, the OrganizationInvitation cannot be deserialized");
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
                if (reader.ValueTextEquals("experisAt"u8))
                {
                    experisAtSeen = true;
                    reader.Read();

                    dtoInstance.ExperisAt = reader.GetDateTime();

                    continue;
                }
                if (reader.ValueTextEquals("modifiedAt"u8))
                {
                    modifiedAtSeen = true;
                    reader.Read();

                    dtoInstance.ModifiedAt = reader.GetDateTime();

                    continue;
                }
                if (reader.ValueTextEquals("organization"u8))
                {
                    organizationSeen = true;
                    reader.Read();

                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        dtoInstance.Organization = Guid.Empty;
                        logger.LogDebug($"the OrganizationInvitation.Organization property was not found in the Json. The value is set to Guid.Empty");
                    }
                    else
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var organizationRefId))
                        {
                            dtoInstance.Organization = organizationRefId;
                        }
                    }

                    continue;
                }
                if (reader.ValueTextEquals("organizationInvitationKind"u8))
                {
                    organizationInvitationKindSeen = true;
                    reader.Read();

                    dtoInstance.OrganizationInvitationKind = OrganizationInvitationKindProvider.Parse(reader.GetString());

                    continue;
                }
                if (reader.ValueTextEquals("owner"u8))
                {
                    ownerSeen = true;
                    reader.Read();

                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        dtoInstance.Owner = Guid.Empty;
                        logger.LogDebug($"the OrganizationInvitation.Owner property was not found in the Json. The value is set to Guid.Empty");
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
                if (reader.ValueTextEquals("status"u8))
                {
                    statusSeen = true;
                    reader.Read();

                    dtoInstance.Status = InvitationStatusKindProvider.Parse(reader.GetString());

                    continue;
                }
                if (reader.ValueTextEquals("target"u8))
                {
                    targetSeen = true;
                    reader.Read();

                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        dtoInstance.Target = Guid.Empty;
                        logger.LogDebug($"the OrganizationInvitation.Target property was not found in the Json. The value is set to Guid.Empty");
                    }
                    else
                    {
                        if (Utf8JsonReaderHelper.TryReadReferenceId(ref reader, out var targetRefId))
                        {
                            dtoInstance.Target = targetRefId;
                        }
                    }

                    continue;
                }

                reader.Skip();
            }

            if (!typeSeen)
            {
                throw new InvalidOperationException("The @type property is not available, the OrganizationInvitationDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (!createdAtSeen)
            {
                logger.LogDebug("the createdAt Json property was not found in the OrganizationInvitation: {Id}", dtoInstance.Id);
            }
            if (!experisAtSeen)
            {
                logger.LogDebug("the experisAt Json property was not found in the OrganizationInvitation: {Id}", dtoInstance.Id);
            }
            if (!modifiedAtSeen)
            {
                logger.LogDebug("the modifiedAt Json property was not found in the OrganizationInvitation: {Id}", dtoInstance.Id);
            }
            if (!organizationSeen)
            {
                logger.LogDebug("the organization Json property was not found in the OrganizationInvitation: {Id}", dtoInstance.Id);
            }
            if (!organizationInvitationKindSeen)
            {
                logger.LogDebug("the organizationInvitationKind Json property was not found in the OrganizationInvitation: {Id}", dtoInstance.Id);
            }
            if (!ownerSeen)
            {
                logger.LogDebug("the owner Json property was not found in the OrganizationInvitation: {Id}", dtoInstance.Id);
            }
            if (!statusSeen)
            {
                logger.LogDebug("the status Json property was not found in the OrganizationInvitation: {Id}", dtoInstance.Id);
            }
            if (!targetSeen)
            {
                logger.LogDebug("the target Json property was not found in the OrganizationInvitation: {Id}", dtoInstance.Id);
            }

            return dtoInstance;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
