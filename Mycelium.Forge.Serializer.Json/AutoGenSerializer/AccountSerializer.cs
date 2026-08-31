// ------------------------------------------------------------------------------------------------
// <copyright file="AccountSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="AccountSerializer"/> is to provide serialization capabilities
    /// capabilities for the <see cref="IAccount"/> interface
    /// </summary>
    internal static class AccountSerializer
    {
        /// <summary>
        /// Serializes an instance of <see cref="IAccount"/> using an <see cref="Utf8JsonWriter"/>
        /// </summary>
        /// <param name="obj">
        /// The <see cref="IAccount"/> to serialize
        /// </param>
        /// <param name="writer">
        /// The target <see cref="Utf8JsonWriter"/>
        /// </param>
        internal static void Serialize(object obj, Utf8JsonWriter writer)
        {
            if (obj is not IAccount iAccount)
            {
                throw new ArgumentException("The object shall be an IAccount", nameof(obj));
            }

            writer.WriteStartObject();

            writer.WritePropertyName("@type"u8);
            writer.WriteStringValue("Account"u8);

            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iAccount.Id);

            writer.WriteStartArray("address"u8);

            foreach (var item in iAccount.Address)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("@type"u8);
                writer.WriteStringValue("Address"u8);
                writer.WritePropertyName("@id"u8);
                writer.WriteStringValue(item);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WriteStartArray("apiKey"u8);

            foreach (var item in iAccount.ApiKey)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("@type"u8);
                writer.WriteStringValue("APIKey"u8);
                writer.WritePropertyName("@id"u8);
                writer.WriteStringValue(item);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WritePropertyName("avatarBlobReference"u8);
            writer.WriteStringValue(iAccount.AvatarBlobReference);
            writer.WritePropertyName("billingEmail"u8);
            writer.WriteStringValue(iAccount.BillingEmail);
            writer.WritePropertyName("createdAt"u8);
            writer.WriteStringValue(iAccount.CreatedAt);
            writer.WritePropertyName("defaultPackageVisibility"u8);
            writer.WriteStringValue(VisibilityKindProvider.ToUtf8LowerBytes(iAccount.DefaultPackageVisibility));
            writer.WritePropertyName("email"u8);
            writer.WriteStringValue(iAccount.Email);
            writer.WritePropertyName("modifiedAt"u8);
            writer.WriteStringValue(iAccount.ModifiedAt);
            writer.WritePropertyName("name"u8);
            writer.WriteStringValue(iAccount.Name);
            writer.WritePropertyName("origin"u8);
            writer.WriteStringValue(iAccount.Origin);
            writer.WriteStartArray("ownedOrganizationInvitation"u8);

            foreach (var item in iAccount.OwnedOrganizationInvitation)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("@type"u8);
                writer.WriteStringValue("OrganizationInvitation"u8);
                writer.WritePropertyName("@id"u8);
                writer.WriteStringValue(item);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WriteStartArray("ownedPackage"u8);

            foreach (var item in iAccount.OwnedPackage)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("@type"u8);
                writer.WriteStringValue("Package"u8);
                writer.WritePropertyName("@id"u8);
                writer.WriteStringValue(item);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WritePropertyName("ownedPackageInvitation"u8);
            writer.WriteStartObject();
            writer.WritePropertyName("@type"u8);
            writer.WriteStringValue("PackageInvitation"u8);
            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iAccount.OwnedPackageInvitation);
            writer.WriteEndObject();
            writer.WritePropertyName("primaryAddress"u8);
            writer.WriteStartObject();
            writer.WritePropertyName("@type"u8);
            writer.WriteStringValue("Address"u8);
            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iAccount.PrimaryAddress);
            writer.WriteEndObject();
            writer.WriteStartArray("profileLink"u8);

            foreach (var item in iAccount.ProfileLink)
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
            writer.WriteStringValue(iAccount.ShortName);
            writer.WritePropertyName("status"u8);
            writer.WriteStringValue(ScopeStatusKindProvider.ToUtf8LowerBytes(iAccount.Status));
            writer.WritePropertyName("website"u8);
            writer.WriteStringValue(iAccount.Website);

            writer.WriteEndObject();
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
