// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationInvitationKindProvider.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="OrganizationInvitationKindProvider"/> is to provide conversion methods
    /// to the <see cref="OrganizationInvitationKind"/> enum
    /// </summary>
    public static class OrganizationInvitationKindProvider
    {
        /// <summary>
        /// Parses the <see cref="ReadOnlySpan{Byte}"/> to a <see cref="OrganizationInvitationKind"/>
        /// </summary>
        /// <param name="value">
        /// The <see cref="ReadOnlySpan{Char}"/> that is to be parsed
        /// </param>
        /// <returns>
        /// A <see cref="OrganizationInvitationKind"/> enumeration literal
        /// </returns>
        /// <exception cref="ArgumentException">
        /// thrown when the content of the <see cref="ReadOnlySpan{Char}"/> cannot be
        /// parsed into a valid <see cref="OrganizationInvitationKind"/> enumeration literal
        /// </exception>
        /// <remarks>
        /// This method is suited for  string parsing
        /// There are zero allocations, no boxing, Fast short-circuit evaluation
        /// JIT friendly
        /// </remarks>
        public static OrganizationInvitationKind Parse(ReadOnlySpan<char> value)
        {
            if (value.Length == 6 && value.Equals("MEMBER".AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                return OrganizationInvitationKind.MEMBER;
            }

            if (value.Length == 13 && value.Equals("ADMINISTRATOR".AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                return OrganizationInvitationKind.ADMINISTRATOR;
            }

            throw new ArgumentException($"'{new string(value)}' is not a valid OrganizationInvitationKind", nameof(value));
        }

        /// <summary>
        /// Tries to parse the <see cref="ReadOnlySpan{Char}"/> to a <see cref="OrganizationInvitationKind"/>
        /// </summary>
        /// <param name="value">
        /// The <see cref="ReadOnlySpan{Char}"/> that is to be parsed
        /// </param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="OrganizationInvitationKind"/> value equivalent
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
        public static bool TryParse(ReadOnlySpan<char> value, out OrganizationInvitationKind result)
        {
            if (value.Length == 6 && value.Equals("MEMBER".AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                result = OrganizationInvitationKind.MEMBER;
                return true;
            }

            if (value.Length == 13 && value.Equals("ADMINISTRATOR".AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                result = OrganizationInvitationKind.ADMINISTRATOR;
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// Parses the <see cref="ReadOnlySpan{Byte}"/> to a <see cref="OrganizationInvitationKind"/>
        /// </summary>
        /// <param name="value">
        /// The <see cref="ReadOnlySpan{Byte}"/> that is to be parsed
        /// </param>
        /// <returns>
        /// A <see cref="OrganizationInvitationKind"/> enumeration literal
        /// </returns>
        /// <exception cref="ArgumentException">
        /// thrown when the content of the <see cref="ReadOnlySpan{Byte}"/> cannot be
        /// parsed into a valid <see cref="OrganizationInvitationKind"/> enumeration literal
        /// </exception>
        /// <remarks>
        /// This method is suited for streaming parsing
        /// There are zero allocations, no boxing, Fast short-circuit evaluation
        /// JIT friendly
        /// </remarks>
        public static OrganizationInvitationKind Parse(ReadOnlySpan<byte> value)
        {
            if (value.Length == 6 && value.SequenceEqual("MEMBER"u8))
            {
                return OrganizationInvitationKind.MEMBER;
            }

            if (value.Length == 13 && value.SequenceEqual("ADMINISTRATOR"u8))
            {
                return OrganizationInvitationKind.ADMINISTRATOR;
            }

            throw new ArgumentException($"'{System.Text.Encoding.UTF8.GetString(value)}' is not a valid OrganizationInvitationKind", nameof(value));
        }

        /// <summary>
        /// Parses a UTF-8 encoded <see cref="ReadOnlySequence{Byte}"/> into a
        /// <see cref="OrganizationInvitationKind"/> enumeration literal.
        /// </summary>
        /// <param name="value">
        /// A UTF-8 encoded sequence representing a <see cref="OrganizationInvitationKind"/> literal.
        /// Valid values are
        /// <list type="bullet">
        /// <item><c>MEMBER (6 bytes) </c></item>
        /// <item><c>ADMINISTRATOR (13 bytes) </c></item>
        /// </list>
        /// </param>
        /// <returns>
        /// The corresponding <see cref="OrganizationInvitationKind"/> enumeration value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the supplied sequence does not represent a valid
        /// <see cref="OrganizationInvitationKind"/> literal.
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
        /// stack-allocated buffer (maximum 13 bytes).
        /// </para>
        /// <para>
        /// No allocations, no boxing, and JIT-friendly control flow.
        /// </para>
        /// </remarks>
        public static OrganizationInvitationKind Parse(in ReadOnlySequence<byte> value)
        {
            if (value.IsSingleSegment)
            {
                return Parse(value.FirstSpan);
            }

            if (value.Length > 13)
            {
                throw new ArgumentException("Invalid OrganizationInvitationKind length", nameof(value));
            }

            Span<byte> tmp = stackalloc byte[13];
            value.CopyTo(tmp);
            return Parse(tmp[..(int)value.Length]);
        }

        /// <summary>
        /// Converts a <see cref="OrganizationInvitationKind"/> value to its
        /// lowercase UTF-8 encoded byte representation.
        /// </summary>
        /// <param name="value">
        /// The <see cref="OrganizationInvitationKind"/> value to convert.
        /// </param>
        /// <returns>
        /// A <see cref="ReadOnlySpan{Byte}"/> containing the lowercase UTF-8
        /// encoding of the specified <see cref="OrganizationInvitationKind"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="value"/> is not a defined
        /// <see cref="OrganizationInvitationKind"/> enumeration value.
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
        /// <item><c>MEMBER</c></item>
        /// <item><c>ADMINISTRATOR</c></item>
        /// </list>
        /// </para>
        /// <para>
        /// No allocations, no boxing, branch-predictable switch,
        /// and JIT-friendly control flow.
        /// </para>
        /// </remarks>
        public static ReadOnlySpan<byte> ToUtf8LowerBytes(OrganizationInvitationKind value)
        {
            return value switch
            {
                OrganizationInvitationKind.MEMBER => "MEMBER"u8,
                OrganizationInvitationKind.ADMINISTRATOR => "ADMINISTRATOR"u8,
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
