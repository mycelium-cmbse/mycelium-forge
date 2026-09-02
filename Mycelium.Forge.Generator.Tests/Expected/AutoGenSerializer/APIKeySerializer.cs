// ------------------------------------------------------------------------------------------------
// <copyright file="APIKeySerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="APIKeySerializer"/> is to provide serialization capabilities
    /// capabilities for the <see cref="IAPIKey"/> interface
    /// </summary>
    internal static class APIKeySerializer
    {
        /// <summary>
        /// Serializes an instance of <see cref="IAPIKey"/> using an <see cref="Utf8JsonWriter"/>
        /// </summary>
        /// <param name="obj">
        /// The <see cref="IAPIKey"/> to serialize
        /// </param>
        /// <param name="writer">
        /// The target <see cref="Utf8JsonWriter"/>
        /// </param>
        internal static void Serialize(object obj, Utf8JsonWriter writer)
        {
            if (obj is not IAPIKey iAPIKey)
            {
                throw new ArgumentException("The object shall be an IAPIKey", nameof(obj));
            }

            writer.WriteStartObject();

            writer.WritePropertyName("@type"u8);
            writer.WriteStringValue("APIKey"u8);

            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iAPIKey.Id);

            writer.WritePropertyName("createdAt"u8);
            writer.WriteStringValue(iAPIKey.CreatedAt);
            writer.WritePropertyName("expiresAt"u8);
            writer.WriteStringValue(iAPIKey.ExpiresAt);
            writer.WritePropertyName("lastUsedAt"u8);
            writer.WriteStringValue(iAPIKey.LastUsedAt);
            writer.WritePropertyName("modifiedAt"u8);
            writer.WriteStringValue(iAPIKey.ModifiedAt);
            writer.WritePropertyName("name"u8);
            writer.WriteStringValue(iAPIKey.Name);
            writer.WritePropertyName("owner"u8);
            writer.WriteStartObject();
            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iAPIKey.Owner);
            writer.WriteEndObject();
            writer.WritePropertyName("permissions"u8);
            writer.WriteStartObject();
            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iAPIKey.Permissions);
            writer.WriteEndObject();
            writer.WritePropertyName("revokedAt"u8);
            writer.WriteStringValue(iAPIKey.RevokedAt);
            writer.WriteStartArray("secretHash"u8);

            foreach (var item in iAPIKey.SecretHash)
            {
                writer.WriteNumberValue(item);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
