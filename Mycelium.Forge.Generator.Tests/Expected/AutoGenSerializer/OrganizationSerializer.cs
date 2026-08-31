// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="OrganizationSerializer"/> is to provide serialization capabilities
    /// capabilities for the <see cref="IOrganization"/> interface
    /// </summary>
    internal static class OrganizationSerializer
    {
        /// <summary>
        /// Serializes an instance of <see cref="IOrganization"/> using an <see cref="Utf8JsonWriter"/>
        /// </summary>
        /// <param name="obj">
        /// The <see cref="IOrganization"/> to serialize
        /// </param>
        /// <param name="writer">
        /// The target <see cref="Utf8JsonWriter"/>
        /// </param>
        internal static void Serialize(object obj, Utf8JsonWriter writer)
        {
            if (obj is not IOrganization iOrganization)
            {
                throw new ArgumentException("The object shall be an IOrganization", nameof(obj));
            }

            writer.WriteStartObject();

            writer.WritePropertyName("@type"u8);
            writer.WriteStringValue("Organization"u8);

            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iOrganization.Id);

            writer.WriteStartArray("address"u8);

            foreach (var item in iOrganization.Address)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("@type"u8);
                writer.WriteStringValue("Address"u8);
                writer.WritePropertyName("@id"u8);
                writer.WriteStringValue(item);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WriteStartArray("administrator"u8);

            foreach (var item in iOrganization.Administrator)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("@type"u8);
                writer.WriteStringValue("Account"u8);
                writer.WritePropertyName("@id"u8);
                writer.WriteStringValue(item);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WritePropertyName("billingEmail"u8);
            writer.WriteStringValue(iOrganization.BillingEmail);
            writer.WritePropertyName("createdAt"u8);
            writer.WriteStringValue(iOrganization.CreatedAt);
            writer.WritePropertyName("defaultPackageVisibility"u8);
            writer.WriteStringValue(VisibilityKindProvider.ToUtf8LowerBytes(iOrganization.DefaultPackageVisibility));
            writer.WritePropertyName("email"u8);
            writer.WriteStringValue(iOrganization.Email);
            writer.WritePropertyName("logoBlobReference"u8);
            writer.WriteStringValue(iOrganization.LogoBlobReference);
            writer.WriteStartArray("member"u8);

            foreach (var item in iOrganization.Member)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("@type"u8);
                writer.WriteStringValue("Account"u8);
                writer.WritePropertyName("@id"u8);
                writer.WriteStringValue(item);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WritePropertyName("modifiedAt"u8);
            writer.WriteStringValue(iOrganization.ModifiedAt);
            writer.WritePropertyName("name"u8);
            writer.WriteStringValue(iOrganization.Name);
            writer.WritePropertyName("origin"u8);
            writer.WriteStringValue(iOrganization.Origin);
            writer.WriteStartArray("ownedPackage"u8);

            foreach (var item in iOrganization.OwnedPackage)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("@type"u8);
                writer.WriteStringValue("Package"u8);
                writer.WritePropertyName("@id"u8);
                writer.WriteStringValue(item);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WritePropertyName("primaryAddress"u8);
            writer.WriteStartObject();
            writer.WritePropertyName("@type"u8);
            writer.WriteStringValue("Address"u8);
            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iOrganization.PrimaryAddress);
            writer.WriteEndObject();
            writer.WriteStartArray("profileLink"u8);

            foreach (var item in iOrganization.ProfileLink)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("@type"u8);
                writer.WriteStringValue("ProfileLink"u8);
                writer.WritePropertyName("@id"u8);
                writer.WriteStringValue(item);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WritePropertyName("shortName"u8);
            writer.WriteStringValue(iOrganization.ShortName);
            writer.WritePropertyName("status"u8);
            writer.WriteStringValue(ScopeStatusKindProvider.ToUtf8LowerBytes(iOrganization.Status));
            writer.WritePropertyName("website"u8);
            writer.WriteStringValue(iOrganization.Website);

            writer.WriteEndObject();
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
