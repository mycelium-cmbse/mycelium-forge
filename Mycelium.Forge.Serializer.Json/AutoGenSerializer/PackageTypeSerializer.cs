// ------------------------------------------------------------------------------------------------
// <copyright file="PackageTypeSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="PackageTypeSerializer"/> is to provide serialization capabilities
    /// capabilities for the <see cref="IPackageType"/> interface
    /// </summary>
    internal static class PackageTypeSerializer
    {
        /// <summary>
        /// Serializes an instance of <see cref="IPackageType"/> using an <see cref="Utf8JsonWriter"/>
        /// </summary>
        /// <param name="obj">
        /// The <see cref="IPackageType"/> to serialize
        /// </param>
        /// <param name="writer">
        /// The target <see cref="Utf8JsonWriter"/>
        /// </param>
        internal static void Serialize(object obj, Utf8JsonWriter writer)
        {
            if (obj is not IPackageType iPackageType)
            {
                throw new ArgumentException("The object shall be an IPackageType", nameof(obj));
            }

            writer.WriteStartObject();

            writer.WritePropertyName("@type"u8);
            writer.WriteStringValue("PackageType"u8);

            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iPackageType.Id);

            writer.WritePropertyName("createdAt"u8);
            writer.WriteStringValue(iPackageType.CreatedAt);
            writer.WritePropertyName("description"u8);
            writer.WriteStringValue(iPackageType.Description);
            writer.WritePropertyName("modifiedAt"u8);
            writer.WriteStringValue(iPackageType.ModifiedAt);
            writer.WritePropertyName("name"u8);
            writer.WriteStringValue(iPackageType.Name);

            writer.WriteEndObject();
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
