// ------------------------------------------------------------------------------------------------
// <copyright file="ShortGuid.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Converts between a <see cref="Guid"/> and its "ShortGuid" representation: the same 16 bytes,
    /// base64-encoded and made URL-safe (<c>/</c> -&gt; <c>_</c>, <c>+</c> -&gt; <c>-</c>, the trailing
    /// <c>==</c> padding dropped) - 22 characters instead of the canonical form's 36, decoding back to
    /// the exact same <see cref="Guid"/>.
    /// </summary>
    public static class ShortGuid
    {
        /// <summary>
        /// Encodes <paramref name="guid"/> as a ShortGuid.
        /// </summary>
        public static string ToShortGuid(this Guid guid)
        {
            return Convert.ToBase64String(guid.ToByteArray())
                .Replace('/', '_')
                .Replace('+', '-')[..22];
        }

        /// <summary>
        /// Decodes a ShortGuid back into the <see cref="Guid"/> it was encoded from.
        /// </summary>
        /// <exception cref="FormatException">
        /// <paramref name="value"/> is not a valid ShortGuid.
        /// </exception>
        public static Guid FromShortGuid(this string value)
        {
            ArgumentNullException.ThrowIfNull(value);

            if (value.Length != 22)
            {
                throw new FormatException($"'{value}' is not a valid ShortGuid: expected 22 characters, got {value.Length}.");
            }

            var base64 = value.Replace('_', '/').Replace('-', '+') + "==";

            return new Guid(Convert.FromBase64String(base64));
        }

        /// <summary>
        /// Encodes <paramref name="guids"/> as a <c>[shortGuid,shortGuid,...]</c> bracketed,
        /// comma-separated list, for a single route segment addressing more than one resource at once.
        /// </summary>
        public static string ToShortGuidArray(this IEnumerable<Guid> guids)
        {
            ArgumentNullException.ThrowIfNull(guids);

            return $"[{string.Join(',', guids.Select(ToShortGuid))}]";
        }

        /// <summary>
        /// Decodes a <c>[shortGuid,shortGuid,...]</c> bracketed, comma-separated list back into the
        /// <see cref="Guid"/>s it was encoded from.
        /// </summary>
        /// <exception cref="FormatException">
        /// <paramref name="value"/> is not a valid bracketed ShortGuid list.
        /// </exception>
        public static IReadOnlyList<Guid> FromShortGuidArray(this string value)
        {
            ArgumentNullException.ThrowIfNull(value);

            if (!value.StartsWith('[') || !value.EndsWith(']'))
            {
                throw new FormatException($"'{value}' is not a valid ShortGuid array: expected '[' ... ']'.");
            }

            var inner = value[1..^1];

            if (inner.Length == 0)
            {
                return [];
            }

            return inner.Split(',').Select(FromShortGuid).ToList();
        }
    }
}
