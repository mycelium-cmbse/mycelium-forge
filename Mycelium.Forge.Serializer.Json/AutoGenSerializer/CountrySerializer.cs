// ------------------------------------------------------------------------------------------------
// <copyright file="CountrySerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="CountrySerializer"/> is to provide serialization capabilities
    /// capabilities for the <see cref="ICountry"/> interface
    /// </summary>
    internal static class CountrySerializer
    {
        /// <summary>
        /// Serializes an instance of <see cref="ICountry"/> using an <see cref="Utf8JsonWriter"/>
        /// </summary>
        /// <param name="obj">
        /// The <see cref="ICountry"/> to serialize
        /// </param>
        /// <param name="writer">
        /// The target <see cref="Utf8JsonWriter"/>
        /// </param>
        internal static void Serialize(object obj, Utf8JsonWriter writer)
        {
            if (obj is not ICountry iCountry)
            {
                throw new ArgumentException("The object shall be an ICountry", nameof(obj));
            }

            writer.WriteStartObject();

            writer.WritePropertyName("@type"u8);
            writer.WriteStringValue("Country"u8);

            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iCountry.Id);

            writer.WritePropertyName("alpha2Code"u8);
            writer.WriteStringValue(iCountry.Alpha2Code);
            writer.WritePropertyName("alpha3Code"u8);
            writer.WriteStringValue(iCountry.Alpha3Code);
            writer.WritePropertyName("createdAt"u8);
            writer.WriteStringValue(iCountry.CreatedAt);
            writer.WritePropertyName("modifiedAt"u8);
            writer.WriteStringValue(iCountry.ModifiedAt);
            writer.WritePropertyName("name"u8);
            writer.WriteStringValue(iCountry.Name);
            writer.WritePropertyName("numericCode"u8);
            writer.WriteStringValue(iCountry.NumericCode);
            writer.WritePropertyName("owner"u8);
            writer.WriteStartObject();
            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iCountry.Owner);
            writer.WriteEndObject();

            writer.WriteEndObject();
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
