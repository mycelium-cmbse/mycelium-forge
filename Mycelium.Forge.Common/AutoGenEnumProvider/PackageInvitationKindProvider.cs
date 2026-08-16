// ------------------------------------------------------------------------------------------------
// <copyright file="PackageInvitationKindProvider.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    using System;
    using System.Buffers;

    /// <summary>
    /// The purpose of the <see cref="PackageInvitationKindProvider"/> is to provide conversion methods
    /// to the <see cref="PackageInvitationKind"/> enum
    /// </summary>
    public static class PackageInvitationKindProvider
    {
        /// <summary>
        /// Parses the <see cref="ReadOnlySpan{Byte}"/> to a <see cref="PackageInvitationKind"/>
        /// </summary>
        /// <param name="value">
        /// The <see cref="ReadOnlySpan{Char}"/> that is to be parsed
        /// </param>
        /// <returns>
        /// A <see cref="PackageInvitationKind"/> enumeration literal
        /// </returns>
        /// <exception cref="ArgumentException">
        /// thrown when the content of the <see cref="ReadOnlySpan{Char}"/> cannot be
        /// parsed into a valid <see cref="PackageInvitationKind"/> enumeration literal
        /// </exception>
        /// <remarks>
        /// This method is suited for  string parsing
        /// There are zero allocations, no boxing, Fast short-circuit evaluation
        /// JIT friendly
        /// </remarks>
        public static PackageInvitationKind Parse(ReadOnlySpan<char> value)
        {
            if (value.Length == 5 && value.Equals("OWNER".AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                return PackageInvitationKind.OWNER;
            }

            if (value.Length == 10 && value.Equals("MAINTAINER".AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                return PackageInvitationKind.MAINTAINER;
            }

            throw new ArgumentException($"'{new string(value)}' is not a valid PackageInvitationKind", nameof(value));
        }

        /// <summary>
        /// Tries to parse the <see cref="ReadOnlySpan{Char}"/> to a <see cref="PackageInvitationKind"/>
        /// </summary>
        /// <param name="value">
        /// The <see cref="ReadOnlySpan{Char}"/> that is to be parsed
        /// </param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="PackageInvitationKind"/> value equivalent
        /// to the span, if the conversion succeeded, or <c>default</c> if the conversion failed.
        /// </param>
        /// <returns>
        /// <c>true</c> if <paramref name="value"/> was converted successfully; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method is suited for  string parsing
        /// There are zero allocations, no boxing, Fast short-circuit evaluation
        /// JIT friendly
        /// </remarks>
        public static bool TryParse(ReadOnlySpan<char> value, out PackageInvitationKind result)
        {
            if (value.Length == 5 && value.Equals("OWNER".AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                result = PackageInvitationKind.OWNER;
                return true;
            }

            if (value.Length == 10 && value.Equals("MAINTAINER".AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                result = PackageInvitationKind.MAINTAINER;
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// Parses the <see cref="ReadOnlySpan{Byte}"/> to a <see cref="PackageInvitationKind"/>
        /// </summary>
        /// <param name="value">
        /// The <see cref="ReadOnlySpan{Byte}"/> that is to be parsed
        /// </param>
        /// <returns>
        /// A <see cref="PackageInvitationKind"/> enumeration literal
        /// </returns>
        /// <exception cref="ArgumentException">
        /// thrown when the content of the <see cref="ReadOnlySpan{Byte}"/> cannot be
        /// parsed into a valid <see cref="PackageInvitationKind"/> enumeration literal
        /// </exception>
        /// <remarks>
        /// This method is suited for streaming parsing
        /// There are zero allocations, no boxing, Fast short-circuit evaluation
        /// JIT friendly
        /// </remarks>
        public static PackageInvitationKind Parse(ReadOnlySpan<byte> value)
        {
            if (value.Length == 5 && value.SequenceEqual("OWNER"u8))
            {
                return PackageInvitationKind.OWNER;
            }

            if (value.Length == 10 && value.SequenceEqual("MAINTAINER"u8))
            {
                return PackageInvitationKind.MAINTAINER;
            }

            throw new ArgumentException($"'{System.Text.Encoding.UTF8.GetString(value)}' is not a valid PackageInvitationKind", nameof(value));
        }

        /// <summary>
        /// Parses a UTF-8 encoded <see cref="ReadOnlySequence{Byte}"/> into a
        /// <see cref="PackageInvitationKind"/> enumeration literal.
        /// </summary>
        /// <param name="value">
        /// A UTF-8 encoded sequence representing a <see cref="PackageInvitationKind"/> literal.
        /// Valid values are
        /// <list type="bullet">
        /// <item><c>OWNER (5 bytes) </c></item>
        /// <item><c>MAINTAINER (10 bytes) </c></item>
        /// </list>
        /// </param>
        /// <returns>
        /// The corresponding <see cref="PackageInvitationKind"/> enumeration value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the supplied sequence does not represent a valid
        /// <see cref="PackageInvitationKind"/> literal.
        /// </exception>
        /// <remarks>
        /// <para>
        /// This method is optimized for streaming scenarios.
        /// It avoids heap allocations and performs fast short-circuit evaluation.
        /// </para>
        /// <para>
        /// If the sequence consists of a single contiguous segment, parsing is
        /// delegated directly to the <see cref="Parse(ReadOnlySpan{Byte})"/> overload.
        /// For multi-segment sequences, the data is copied into a small
        /// stack-allocated buffer (maximum 10 bytes).
        /// </para>
        /// <para>
        /// No allocations, no boxing, and JIT-friendly control flow.
        /// </para>
        /// </remarks>
        public static PackageInvitationKind Parse(in ReadOnlySequence<byte> value)
        {
            if (value.IsSingleSegment)
            {
                return Parse(value.FirstSpan);
            }

            if (value.Length > 10)
            {
                throw new ArgumentException("Invalid PackageInvitationKind length", nameof(value));
            }

            Span<byte> tmp = stackalloc byte[10];
            value.CopyTo(tmp);
            return Parse(tmp[..(int)value.Length]);
        }

        /// <summary>
        /// Converts a <see cref="PackageInvitationKind"/> value to its
        /// lowercase UTF-8 encoded byte representation.
        /// </summary>
        /// <param name="value">
        /// The <see cref="PackageInvitationKind"/> value to convert.
        /// </param>
        /// <returns>
        /// A <see cref="ReadOnlySpan{Byte}"/> containing the lowercase UTF-8
        /// encoding of the specified <see cref="PackageInvitationKind"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="value"/> is not a defined
        /// <see cref="PackageInvitationKind"/> enumeration value.
        /// </exception>
        /// <remarks>
        /// <para>
        /// This method is optimized for serialization scenarios (e.g. MessagePack).
        /// It returns a span backed by a static UTF-8 literal and performs
        /// no heap allocations.
        /// </para>
        /// <para>
        /// The returned span is backed by static UTF-8 data and remains valid for
        /// the lifetime of the current process.The span must be consumed
        /// immediately and must not be stored beyond the calling scope.
        /// </para>
        /// <para>
        /// Valid encodings are:
        /// <list type="bullet">
        /// <item><c>OWNER</c></item>
        /// <item><c>MAINTAINER</c></item>
        /// </list>
        /// </para>
        /// <para>
        /// No allocations, no boxing, branch-predictable switch,
        /// and JIT-friendly control flow.
        /// </para>
        /// </remarks>
        public static ReadOnlySpan<byte> ToUtf8LowerBytes(PackageInvitationKind value)
        {
            return value switch
            {
                PackageInvitationKind.OWNER => "OWNER"u8,
                PackageInvitationKind.MAINTAINER => "MAINTAINER"u8,
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
