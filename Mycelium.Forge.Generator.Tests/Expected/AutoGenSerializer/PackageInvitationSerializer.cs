// ------------------------------------------------------------------------------------------------
// <copyright file="PackageInvitationSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="PackageInvitationSerializer"/> is to provide serialization capabilities
    /// capabilities for the <see cref="IPackageInvitation"/> interface
    /// </summary>
    internal static class PackageInvitationSerializer
    {
        /// <summary>
        /// Serializes an instance of <see cref="IPackageInvitation"/> using an <see cref="Utf8JsonWriter"/>
        /// </summary>
        /// <param name="obj">
        /// The <see cref="IPackageInvitation"/> to serialize
        /// </param>
        /// <param name="writer">
        /// The target <see cref="Utf8JsonWriter"/>
        /// </param>
        internal static void Serialize(object obj, Utf8JsonWriter writer)
        {
            if (obj is not IPackageInvitation iPackageInvitation)
            {
                throw new ArgumentException("The object shall be an IPackageInvitation", nameof(obj));
            }

            writer.WriteStartObject();

            writer.WritePropertyName("@type"u8);
            writer.WriteStringValue("PackageInvitation"u8);

            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iPackageInvitation.Id);

            writer.WritePropertyName("createdAt"u8);
            writer.WriteStringValue(iPackageInvitation.CreatedAt);
            writer.WritePropertyName("experisAt"u8);
            writer.WriteStringValue(iPackageInvitation.ExperisAt);
            writer.WritePropertyName("modifiedAt"u8);
            writer.WriteStringValue(iPackageInvitation.ModifiedAt);
            writer.WritePropertyName("package"u8);
            writer.WriteStartObject();
            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iPackageInvitation.Package);
            writer.WriteEndObject();
            writer.WritePropertyName("packageInvitationKind"u8);
            writer.WriteStringValue(PackageInvitationKindProvider.ToUtf8LowerBytes(iPackageInvitation.PackageInvitationKind));
            writer.WritePropertyName("status"u8);
            writer.WriteStringValue(InvitationStatusKindProvider.ToUtf8LowerBytes(iPackageInvitation.Status));
            writer.WritePropertyName("target"u8);
            writer.WriteStartObject();
            writer.WritePropertyName("@id"u8);
            writer.WriteStringValue(iPackageInvitation.Target);
            writer.WriteEndObject();

            writer.WriteEndObject();
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
