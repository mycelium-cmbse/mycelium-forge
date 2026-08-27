// ------------------------------------------------------------------------------------------------
// <copyright file="Utf8JsonReaderHelper.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Serializer.Json
{
    using System;
    using System.Text.Json;

    /// <summary>
    /// Low-allocation helpers for reading values directly off a <see cref="Utf8JsonReader"/>, used by
    /// the generated deserializers instead of materializing a <see cref="JsonDocument"/>.
    /// </summary>
    internal static class Utf8JsonReaderHelper
    {
        /// <summary>
        /// Reads a <see cref="Guid"/> off the <paramref name="reader"/>'s current string value token
        /// </summary>
        /// <param name="reader">
        /// The <see cref="Utf8JsonReader"/>, positioned at the string token that contains the <see cref="Guid"/>
        /// </param>
        /// <returns>
        /// the parsed <see cref="Guid"/>
        /// </returns>
        internal static Guid ReadGuid(ref Utf8JsonReader reader)
        {
            if (reader.TryGetGuid(out var value))
            {
                return value;
            }

            return Guid.Parse(reader.GetString());
        }

        /// <summary>
        /// Reads the <c>@id</c> property of a JSON reference object (<c>{ "@id": "..." }</c>) without
        /// ever materializing the object as a <see cref="JsonDocument"/>
        /// </summary>
        /// <param name="reader">
        /// The <see cref="Utf8JsonReader"/>, positioned at the reference object's <see cref="JsonTokenType.StartObject"/> token
        /// </param>
        /// <param name="id">
        /// The parsed <see cref="Guid"/> of the <c>@id</c> property, or <see cref="Guid.Empty"/> when not found
        /// </param>
        /// <returns>
        /// true when the <c>@id</c> property was found, false otherwise
        /// </returns>
        internal static bool TryReadReferenceId(ref Utf8JsonReader reader, out Guid id)
        {
            id = Guid.Empty;
            var found = false;

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType == JsonTokenType.PropertyName && reader.ValueTextEquals("@id"u8))
                {
                    reader.Read();
                    id = ReadGuid(ref reader);
                    found = true;
                    continue;
                }

                reader.Skip();
            }

            return found;
        }

        /// <summary>
        /// Reads the <c>@type</c> property of a JSON object without advancing <paramref name="reader"/>,
        /// so the caller can pick the appropriate <see cref="DeSerializeDelegate"/> before handing the
        /// still-unread reader to it
        /// </summary>
        /// <param name="reader">
        /// A copy of the <see cref="Utf8JsonReader"/>, positioned at the object's <see cref="JsonTokenType.StartObject"/> token.
        /// Passed by value on purpose: <see cref="Utf8JsonReader"/> is a struct, so advancing this copy
        /// does not move the caller's reader.
        /// </param>
        /// <returns>
        /// the value of the <c>@type</c> property, or null when not found
        /// </returns>
        internal static string PeekTypeName(Utf8JsonReader reader)
        {
            var depth = reader.CurrentDepth;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName && reader.ValueTextEquals("@type"u8))
                {
                    reader.Read();
                    return reader.GetString();
                }

                if (reader.TokenType == JsonTokenType.EndObject && reader.CurrentDepth == depth)
                {
                    break;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    reader.Skip();
                }
            }

            return null;
        }
    }
}
