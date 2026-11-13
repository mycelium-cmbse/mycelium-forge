// ------------------------------------------------------------------------------------------------
// <copyright file="ProfileTypeSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="ProfileTypeSerializer"/> is to provide serialization capabilities
    /// capabilities for the <see cref="IProfileType"/> interface
    /// </summary>
    internal static class ProfileTypeSerializer
    {
        /// <summary>
        /// Serializes an instance of <see cref="IProfileType"/> using an <see cref="Utf8JsonWriter"/>
        /// </summary>
        /// <param name="obj">
        /// The <see cref="IProfileType"/> to serialize
        /// </param>
        /// <param name="writer">
        /// The target <see cref="Utf8JsonWriter"/>
        /// </param>
        internal static void Serialize(object obj, Utf8JsonWriter writer)
        {
            if (obj is not IProfileType iProfileType)
            {
                throw new ArgumentException("The object shall be an IProfileType", nameof(obj));
            }

            writer.WriteStartObject();

            writer.WritePropertyName("@type"u8);
            writer.WriteStringValue("ProfileType"u8);

            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iProfileType.Id);

            writer.WritePropertyName("createdAt"u8);
            writer.WriteStringValue(iProfileType.CreatedAt);
            writer.WritePropertyName("logoBlobReference"u8);
            writer.WriteStringValue(iProfileType.LogoBlobReference);
            writer.WritePropertyName("modifiedAt"u8);
            writer.WriteStringValue(iProfileType.ModifiedAt);
            writer.WritePropertyName("name"u8);
            writer.WriteStringValue(iProfileType.Name);
            writer.WritePropertyName("owner"u8);
            writer.WriteStartObject();
            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iProfileType.Owner);
            writer.WriteEndObject();

            writer.WriteEndObject();
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
