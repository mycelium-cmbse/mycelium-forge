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
        /// Deserializes an instance of <see cref="IOrganizationInvitation"/> from the provided <see cref="JsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="IOrganizationInvitation"/> json object
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="IOrganizationInvitation"/>
        /// </returns>
        internal static IOrganizationInvitation DeSerialize(JsonElement jsonElement, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("OrganizationInvitationDeSerializer");

            if (!jsonElement.TryGetProperty("@type"u8, out var @type))
            {
                throw new InvalidOperationException("The @type property is not available, the OrganizationInvitationDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (@type.GetString() != "OrganizationInvitation")
            {
                throw new InvalidOperationException($"The OrganizationInvitationDeSerializer can only be used to deserialize objects of type IOrganizationInvitation, a {@type.GetString()} was provided");
            }

            var dtoInstance = new Mycelium.Forge.Common.OrganizationInvitation();

            if (jsonElement.TryGetProperty("@id"u8, out var idProperty))
            {
                var propertyValue = idProperty.GetString();

                if (propertyValue == null)
                {
                    throw new JsonException("The @id property is not present, the OrganizationInvitation cannot be deserialized");
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
                logger.LogDebug("the createdAt Json property was not found in the OrganizationInvitation: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("experisAt"u8, out var experisAtProperty))
            {
                dtoInstance.ExperisAt = experisAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the experisAt Json property was not found in the OrganizationInvitation: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("modifiedAt"u8, out var modifiedAtProperty))
            {
                dtoInstance.ModifiedAt = modifiedAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the modifiedAt Json property was not found in the OrganizationInvitation: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("organization"u8, out var organizationProperty))
            {
                if (organizationProperty.ValueKind == JsonValueKind.Null)
                {
                    dtoInstance.Organization = Guid.Empty;
                    logger.LogDebug($"the OrganizationInvitation.Organization property was not found in the Json. The value is set to Guid.Empty");
                }
                else
                {
                    if (organizationProperty.TryGetProperty("@id"u8, out var organizationExternalIdProperty))
                    {
                        var propertyValue = organizationExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.Organization = Guid.Parse(propertyValue);
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the organization Json property was not found in the OrganizationInvitation: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("organizationInvitationKind"u8, out var organizationInvitationKindProperty))
            {
                dtoInstance.OrganizationInvitationKind = OrganizationInvitationKindProvider.Parse(organizationInvitationKindProperty.GetString());
            }
            else
            {
                logger.LogDebug("the organizationInvitationKind Json property was not found in the OrganizationInvitation: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("status"u8, out var statusProperty))
            {
                dtoInstance.Status = InvitationStatusKindProvider.Parse(statusProperty.GetString());
            }
            else
            {
                logger.LogDebug("the status Json property was not found in the OrganizationInvitation: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("target"u8, out var targetProperty))
            {
                if (targetProperty.ValueKind == JsonValueKind.Null)
                {
                    dtoInstance.Target = Guid.Empty;
                    logger.LogDebug($"the OrganizationInvitation.Target property was not found in the Json. The value is set to Guid.Empty");
                }
                else
                {
                    if (targetProperty.TryGetProperty("@id"u8, out var targetExternalIdProperty))
                    {
                        var propertyValue = targetExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.Target = Guid.Parse(propertyValue);
                        }
                    }
                }
            }
            else
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
