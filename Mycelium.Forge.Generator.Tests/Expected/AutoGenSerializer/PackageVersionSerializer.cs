// ------------------------------------------------------------------------------------------------
// <copyright file="PackageVersionSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="PackageVersionSerializer"/> is to provide serialization capabilities
    /// capabilities for the <see cref="IPackageVersion"/> interface
    /// </summary>
    internal static class PackageVersionSerializer
    {
        /// <summary>
        /// Serializes an instance of <see cref="IPackageVersion"/> using an <see cref="Utf8JsonWriter"/>
        /// </summary>
        /// <param name="obj">
        /// The <see cref="IPackageVersion"/> to serialize
        /// </param>
        /// <param name="writer">
        /// The target <see cref="Utf8JsonWriter"/>
        /// </param>
        internal static void Serialize(object obj, Utf8JsonWriter writer)
        {
            if (obj is not IPackageVersion iPackageVersion)
            {
                throw new ArgumentException("The object shall be an IPackageVersion", nameof(obj));
            }

            writer.WriteStartObject();

            writer.WritePropertyName("@type"u8);
            writer.WriteStringValue("PackageVersion"u8);

            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iPackageVersion.Id);

            writer.WritePropertyName("createdAt"u8);
            writer.WriteStringValue(iPackageVersion.CreatedAt);
            writer.WritePropertyName("downloadCount"u8);
            writer.WriteNumberValue(iPackageVersion.DownloadCount);
            writer.WritePropertyName("listed"u8);
            writer.WriteBooleanValue(iPackageVersion.Listed);
            writer.WritePropertyName("metaData"u8);
            writer.WriteStartObject();
            writer.WritePropertyName("@type"u8);
            writer.WriteStringValue("PackageMetaData"u8);
            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iPackageVersion.MetaData);
            writer.WriteEndObject();
            writer.WritePropertyName("modifiedAt"u8);
            writer.WriteStringValue(iPackageVersion.ModifiedAt);
            writer.WritePropertyName("publicationDate"u8);
            writer.WriteStringValue(iPackageVersion.PublicationDate);
            writer.WritePropertyName("version"u8);
            writer.WriteStringValue(iPackageVersion.Version);

            writer.WriteEndObject();
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
