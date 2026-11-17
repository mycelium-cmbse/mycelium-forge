// ------------------------------------------------------------------------------------------------
// <copyright file="ProfileLinkSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="ProfileLinkSerializer"/> is to provide serialization capabilities
    /// capabilities for the <see cref="IProfileLink"/> interface
    /// </summary>
    internal static class ProfileLinkSerializer
    {
        /// <summary>
        /// Serializes an instance of <see cref="IProfileLink"/> using an <see cref="Utf8JsonWriter"/>
        /// </summary>
        /// <param name="obj">
        /// The <see cref="IProfileLink"/> to serialize
        /// </param>
        /// <param name="writer">
        /// The target <see cref="Utf8JsonWriter"/>
        /// </param>
        internal static void Serialize(object obj, Utf8JsonWriter writer)
        {
            if (obj is not IProfileLink iProfileLink)
            {
                throw new ArgumentException("The object shall be an IProfileLink", nameof(obj));
            }

            writer.WriteStartObject();

            writer.WritePropertyName("@type"u8);
            writer.WriteStringValue("ProfileLink"u8);

            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iProfileLink.Id);

            writer.WritePropertyName("createdAt"u8);
            writer.WriteStringValue(iProfileLink.CreatedAt);
            writer.WritePropertyName("modifiedAt"u8);
            writer.WriteStringValue(iProfileLink.ModifiedAt);
            writer.WritePropertyName("owner"u8);
            writer.WriteStartObject();
            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iProfileLink.Owner);
            writer.WriteEndObject();
            writer.WritePropertyName("profileType"u8);
            writer.WriteStartObject();
            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iProfileLink.ProfileType);
            writer.WriteEndObject();
            writer.WritePropertyName("uri"u8);
            writer.WriteStringValue(iProfileLink.Uri);

            writer.WriteEndObject();
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
