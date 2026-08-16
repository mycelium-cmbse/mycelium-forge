// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationInvitationSerializer.cs" company="Starion Group S.A.">
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

    using Mycelium.Forge.Common;

    /// <summary>
    /// The purpose of the <see cref="OrganizationInvitationSerializer"/> is to provide serialization capabilities
    /// capabilities for the <see cref="IOrganizationInvitation"/> interface
    /// </summary>
    internal static class OrganizationInvitationSerializer
    {
        /// <summary>
        /// Serializes an instance of <see cref="IOrganizationInvitation"/> using an <see cref="Utf8JsonWriter"/>
        /// </summary>
        /// <param name="obj">
        /// The <see cref="IOrganizationInvitation"/> to serialize
        /// </param>
        /// <param name="writer">
        /// The target <see cref="Utf8JsonWriter"/>
        /// </param>
        internal static void Serialize(object obj, Utf8JsonWriter writer)
        {
            if (obj is not IOrganizationInvitation iOrganizationInvitation)
            {
                throw new ArgumentException("The object shall be an IOrganizationInvitation", nameof(obj));
            }

            writer.WriteStartObject();

            writer.WritePropertyName("@type"u8);
            writer.WriteStringValue("OrganizationInvitation"u8);

            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iOrganizationInvitation.Id);

            writer.WritePropertyName("createdAt"u8);
            writer.WriteStringValue(iOrganizationInvitation.CreatedAt);
            writer.WritePropertyName("experisAt"u8);
            writer.WriteStringValue(iOrganizationInvitation.ExperisAt);
            writer.WritePropertyName("modifiedAt"u8);
            writer.WriteStringValue(iOrganizationInvitation.ModifiedAt);
            writer.WritePropertyName("organization"u8);
            writer.WriteStartObject();
            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iOrganizationInvitation.Organization);
            writer.WriteEndObject();
            writer.WritePropertyName("organizationInvitationKind"u8);
            writer.WriteStringValue(OrganizationInvitationKindProvider.ToUtf8LowerBytes(iOrganizationInvitation.OrganizationInvitationKind));
            writer.WritePropertyName("status"u8);
            writer.WriteStringValue(InvitationStatusKindProvider.ToUtf8LowerBytes(iOrganizationInvitation.Status));
            writer.WritePropertyName("target"u8);
            writer.WriteStartObject();
            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iOrganizationInvitation.Target);
            writer.WriteEndObject();

            writer.WriteEndObject();
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
