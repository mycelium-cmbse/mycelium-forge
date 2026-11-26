// ------------------------------------------------------------------------------------------------
// <copyright file="LicenseSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="LicenseSerializer"/> is to provide serialization capabilities
    /// capabilities for the <see cref="ILicense"/> interface
    /// </summary>
    internal static class LicenseSerializer
    {
        /// <summary>
        /// Serializes an instance of <see cref="ILicense"/> using an <see cref="Utf8JsonWriter"/>
        /// </summary>
        /// <param name="obj">
        /// The <see cref="ILicense"/> to serialize
        /// </param>
        /// <param name="writer">
        /// The target <see cref="Utf8JsonWriter"/>
        /// </param>
        internal static void Serialize(object obj, Utf8JsonWriter writer)
        {
            if (obj is not ILicense iLicense)
            {
                throw new ArgumentException("The object shall be an ILicense", nameof(obj));
            }

            writer.WriteStartObject();

            writer.WritePropertyName("@type"u8);
            writer.WriteStringValue("License"u8);

            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iLicense.Id);

            writer.WritePropertyName("createdAt"u8);
            writer.WriteStringValue(iLicense.CreatedAt);
            writer.WritePropertyName("licenseText"u8);
            writer.WriteStringValue(iLicense.LicenseText);
            writer.WritePropertyName("modifiedAt"u8);
            writer.WriteStringValue(iLicense.ModifiedAt);
            writer.WritePropertyName("name"u8);
            writer.WriteStringValue(iLicense.Name);
            writer.WritePropertyName("spdxIdentifier"u8);
            writer.WriteStringValue(iLicense.SpdxIdentifier);

            writer.WriteEndObject();
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
