// ------------------------------------------------------------------------------------------------
// <copyright file="PackageMetaDataSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="PackageMetaDataSerializer"/> is to provide serialization capabilities
    /// capabilities for the <see cref="IPackageMetaData"/> interface
    /// </summary>
    internal static class PackageMetaDataSerializer
    {
        /// <summary>
        /// Serializes an instance of <see cref="IPackageMetaData"/> using an <see cref="Utf8JsonWriter"/>
        /// </summary>
        /// <param name="obj">
        /// The <see cref="IPackageMetaData"/> to serialize
        /// </param>
        /// <param name="writer">
        /// The target <see cref="Utf8JsonWriter"/>
        /// </param>
        internal static void Serialize(object obj, Utf8JsonWriter writer)
        {
            if (obj is not IPackageMetaData iPackageMetaData)
            {
                throw new ArgumentException("The object shall be an IPackageMetaData", nameof(obj));
            }

            writer.WriteStartObject();

            writer.WritePropertyName("@type"u8);
            writer.WriteStringValue("PackageMetaData"u8);

            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iPackageMetaData.Id);

            writer.WritePropertyName("createdAt"u8);
            writer.WriteStringValue(iPackageMetaData.CreatedAt);
            writer.WritePropertyName("modifiedAt"u8);
            writer.WriteStringValue(iPackageMetaData.ModifiedAt);
            writer.WritePropertyName("owner"u8);
            writer.WriteStartObject();
            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iPackageMetaData.Owner);
            writer.WriteEndObject();

            writer.WriteEndObject();
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
