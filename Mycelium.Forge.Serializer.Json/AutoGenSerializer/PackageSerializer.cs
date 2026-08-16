// ------------------------------------------------------------------------------------------------
// <copyright file="PackageSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="PackageSerializer"/> is to provide serialization capabilities
    /// capabilities for the <see cref="IPackage"/> interface
    /// </summary>
    internal static class PackageSerializer
    {
        /// <summary>
        /// Serializes an instance of <see cref="IPackage"/> using an <see cref="Utf8JsonWriter"/>
        /// </summary>
        /// <param name="obj">
        /// The <see cref="IPackage"/> to serialize
        /// </param>
        /// <param name="writer">
        /// The target <see cref="Utf8JsonWriter"/>
        /// </param>
        internal static void Serialize(object obj, Utf8JsonWriter writer)
        {
            if (obj is not IPackage iPackage)
            {
                throw new ArgumentException("The object shall be an IPackage", nameof(obj));
            }

            writer.WriteStartObject();

            writer.WritePropertyName("@type"u8);
            writer.WriteStringValue("Package"u8);

            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iPackage.Id);

            writer.WritePropertyName("createdAt"u8);
            writer.WriteStringValue(iPackage.CreatedAt);
            writer.WritePropertyName("listed"u8);
            writer.WriteBooleanValue(iPackage.Listed);
            writer.WritePropertyName("modifiedAt"u8);
            writer.WriteStringValue(iPackage.ModifiedAt);
            writer.WritePropertyName("name"u8);
            writer.WriteStringValue(iPackage.Name);
            writer.WriteStartArray("packageMaintainer"u8);

            foreach (var item in iPackage.PackageMaintainer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("@id"u8);
                writer.WriteStringValue(item);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WriteStartArray("packageOwner"u8);

            foreach (var item in iPackage.PackageOwner)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("@id"u8);
                writer.WriteStringValue(item);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WritePropertyName("packageType"u8);
            writer.WriteStartObject();
            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iPackage.PackageType);
            writer.WriteEndObject();
            writer.WritePropertyName("shortName"u8);
            writer.WriteStringValue(iPackage.ShortName);
            writer.WriteStartArray("version"u8);

            foreach (var item in iPackage.Version)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("@id"u8);
                writer.WriteStringValue(item);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WritePropertyName("visibility"u8);
            writer.WriteStringValue(VisibilityKindProvider.ToUtf8LowerBytes(iPackage.Visibility));

            writer.WriteEndObject();
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
