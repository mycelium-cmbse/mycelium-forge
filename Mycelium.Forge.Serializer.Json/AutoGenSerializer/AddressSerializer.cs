// ------------------------------------------------------------------------------------------------
// <copyright file="AddressSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="AddressSerializer"/> is to provide serialization capabilities
    /// capabilities for the <see cref="IAddress"/> interface
    /// </summary>
    internal static class AddressSerializer
    {
        /// <summary>
        /// Serializes an instance of <see cref="IAddress"/> using an <see cref="Utf8JsonWriter"/>
        /// </summary>
        /// <param name="obj">
        /// The <see cref="IAddress"/> to serialize
        /// </param>
        /// <param name="writer">
        /// The target <see cref="Utf8JsonWriter"/>
        /// </param>
        internal static void Serialize(object obj, Utf8JsonWriter writer)
        {
            if (obj is not IAddress iAddress)
            {
                throw new ArgumentException("The object shall be an IAddress", nameof(obj));
            }

            writer.WriteStartObject();

            writer.WritePropertyName("@type"u8);
            writer.WriteStringValue("Address"u8);

            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iAddress.Id);

            writer.WritePropertyName("addressLine1"u8);
            writer.WriteStringValue(iAddress.AddressLine1);
            writer.WritePropertyName("addressLine2"u8);
            writer.WriteStringValue(iAddress.AddressLine2);
            writer.WritePropertyName("country"u8);
            writer.WriteStartObject();
            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iAddress.Country);
            writer.WriteEndObject();
            writer.WritePropertyName("createdAt"u8);
            writer.WriteStringValue(iAddress.CreatedAt);
            writer.WritePropertyName("locality"u8);
            writer.WriteStringValue(iAddress.Locality);
            writer.WritePropertyName("modifiedAt"u8);
            writer.WriteStringValue(iAddress.ModifiedAt);
            writer.WritePropertyName("owner"u8);
            writer.WriteStartObject();
            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iAddress.Owner);
            writer.WriteEndObject();
            writer.WritePropertyName("postalCode"u8);
            writer.WriteStringValue(iAddress.PostalCode);
            writer.WritePropertyName("region"u8);
            writer.WriteStringValue(iAddress.Region);

            writer.WriteEndObject();
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
