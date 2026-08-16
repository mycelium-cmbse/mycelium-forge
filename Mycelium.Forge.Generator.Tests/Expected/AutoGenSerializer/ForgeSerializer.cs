// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="ForgeSerializer"/> is to provide serialization capabilities
    /// capabilities for the <see cref="IForge"/> interface
    /// </summary>
    internal static class ForgeSerializer
    {
        /// <summary>
        /// Serializes an instance of <see cref="IForge"/> using an <see cref="Utf8JsonWriter"/>
        /// </summary>
        /// <param name="obj">
        /// The <see cref="IForge"/> to serialize
        /// </param>
        /// <param name="writer">
        /// The target <see cref="Utf8JsonWriter"/>
        /// </param>
        internal static void Serialize(object obj, Utf8JsonWriter writer)
        {
            if (obj is not IForge iForge)
            {
                throw new ArgumentException("The object shall be an IForge", nameof(obj));
            }

            writer.WriteStartObject();

            writer.WritePropertyName("@type"u8);
            writer.WriteStringValue("Forge"u8);

            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iForge.Id);

            writer.WriteStartArray("account"u8);

            foreach (var item in iForge.Account)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("@id"u8);
                writer.WriteStringValue(item);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WriteStartArray("administrator"u8);

            foreach (var item in iForge.Administrator)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("@id"u8);
                writer.WriteStringValue(item);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WriteStartArray("country"u8);

            foreach (var item in iForge.Country)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("@id"u8);
                writer.WriteStringValue(item);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WritePropertyName("createdAt"u8);
            writer.WriteStringValue(iForge.CreatedAt);
            writer.WritePropertyName("description"u8);
            writer.WriteStringValue(iForge.Description);
            writer.WritePropertyName("modifiedAt"u8);
            writer.WriteStringValue(iForge.ModifiedAt);
            writer.WritePropertyName("name"u8);
            writer.WriteStringValue(iForge.Name);
            writer.WriteStartArray("organization"u8);

            foreach (var item in iForge.Organization)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("@id"u8);
                writer.WriteStringValue(item);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WriteStartArray("packageType"u8);

            foreach (var item in iForge.PackageType)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("@id"u8);
                writer.WriteStringValue(item);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WriteStartArray("profileType"u8);

            foreach (var item in iForge.ProfileType)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("@id"u8);
                writer.WriteStringValue(item);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WritePropertyName("shortName"u8);
            writer.WriteStringValue(iForge.ShortName);

            writer.WriteEndObject();
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
