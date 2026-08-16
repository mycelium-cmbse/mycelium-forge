// ------------------------------------------------------------------------------------------------
// <copyright file="InvitationStatusKindProvider.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="InvitationStatusKindProvider"/> is to provide conversion methods
    /// to the <see cref="InvitationStatusKind"/> enum
    /// </summary>
    public static class InvitationStatusKindProvider
    {
        /// <summary>
        /// Parses the <see cref="ReadOnlySpan{Byte}"/> to a <see cref="InvitationStatusKind"/>
        /// </summary>
        /// <param name="value">
        /// The <see cref="ReadOnlySpan{Char}"/> that is to be parsed
        /// </param>
        /// <returns>
        /// A <see cref="InvitationStatusKind"/> enumeration literal
        /// </returns>
        /// <exception cref="ArgumentException">
        /// thrown when the content of the <see cref="ReadOnlySpan{Char}"/> cannot be
        /// parsed into a valid <see cref="InvitationStatusKind"/> enumeration literal
        /// </exception>
        /// <remarks>
        /// This method is suited for  string parsing
        /// There are zero allocations, no boxing, Fast short-circuit evaluation
        /// JIT friendly
        /// </remarks>
        public static InvitationStatusKind Parse(ReadOnlySpan<char> value)
        {
            if (value.Length == 7 && value.Equals("PENDING".AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                return InvitationStatusKind.PENDING;
            }

            if (value.Length == 8 && value.Equals("ACCEPTED".AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                return InvitationStatusKind.ACCEPTED;
            }

            if (value.Length == 8 && value.Equals("REJECTED".AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                return InvitationStatusKind.REJECTED;
            }

            if (value.Length == 7 && value.Equals("REVOKED".AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                return InvitationStatusKind.REVOKED;
            }

            throw new ArgumentException($"'{new string(value)}' is not a valid InvitationStatusKind", nameof(value));
        }

        /// <summary>
        /// Tries to parse the <see cref="ReadOnlySpan{Char}"/> to a <see cref="InvitationStatusKind"/>
        /// </summary>
        /// <param name="value">
        /// The <see cref="ReadOnlySpan{Char}"/> that is to be parsed
        /// </param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="InvitationStatusKind"/> value equivalent
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
        public static bool TryParse(ReadOnlySpan<char> value, out InvitationStatusKind result)
        {
            if (value.Length == 7 && value.Equals("PENDING".AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                result = InvitationStatusKind.PENDING;
                return true;
            }

            if (value.Length == 8 && value.Equals("ACCEPTED".AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                result = InvitationStatusKind.ACCEPTED;
                return true;
            }

            if (value.Length == 8 && value.Equals("REJECTED".AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                result = InvitationStatusKind.REJECTED;
                return true;
            }

            if (value.Length == 7 && value.Equals("REVOKED".AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                result = InvitationStatusKind.REVOKED;
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// Parses the <see cref="ReadOnlySpan{Byte}"/> to a <see cref="InvitationStatusKind"/>
        /// </summary>
        /// <param name="value">
        /// The <see cref="ReadOnlySpan{Byte}"/> that is to be parsed
        /// </param>
        /// <returns>
        /// A <see cref="InvitationStatusKind"/> enumeration literal
        /// </returns>
        /// <exception cref="ArgumentException">
        /// thrown when the content of the <see cref="ReadOnlySpan{Byte}"/> cannot be
        /// parsed into a valid <see cref="InvitationStatusKind"/> enumeration literal
        /// </exception>
        /// <remarks>
        /// This method is suited for streaming parsing
        /// There are zero allocations, no boxing, Fast short-circuit evaluation
        /// JIT friendly
        /// </remarks>
        public static InvitationStatusKind Parse(ReadOnlySpan<byte> value)
        {
            if (value.Length == 7 && value.SequenceEqual("PENDING"u8))
            {
                return InvitationStatusKind.PENDING;
            }

            if (value.Length == 8 && value.SequenceEqual("ACCEPTED"u8))
            {
                return InvitationStatusKind.ACCEPTED;
            }

            if (value.Length == 8 && value.SequenceEqual("REJECTED"u8))
            {
                return InvitationStatusKind.REJECTED;
            }

            if (value.Length == 7 && value.SequenceEqual("REVOKED"u8))
            {
                return InvitationStatusKind.REVOKED;
            }

            throw new ArgumentException($"'{System.Text.Encoding.UTF8.GetString(value)}' is not a valid InvitationStatusKind", nameof(value));
        }

        /// <summary>
        /// Parses a UTF-8 encoded <see cref="ReadOnlySequence{Byte}"/> into a
        /// <see cref="InvitationStatusKind"/> enumeration literal.
        /// </summary>
        /// <param name="value">
        /// A UTF-8 encoded sequence representing a <see cref="InvitationStatusKind"/> literal.
        /// Valid values are
        /// <list type="bullet">
        /// <item><c>PENDING (7 bytes) </c></item>
        /// <item><c>ACCEPTED (8 bytes) </c></item>
        /// <item><c>REJECTED (8 bytes) </c></item>
        /// <item><c>REVOKED (7 bytes) </c></item>
        /// </list>
        /// </param>
        /// <returns>
        /// The corresponding <see cref="InvitationStatusKind"/> enumeration value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the supplied sequence does not represent a valid
        /// <see cref="InvitationStatusKind"/> literal.
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
        /// stack-allocated buffer (maximum 8 bytes).
        /// </para>
        /// <para>
        /// No allocations, no boxing, and JIT-friendly control flow.
        /// </para>
        /// </remarks>
        public static InvitationStatusKind Parse(in ReadOnlySequence<byte> value)
        {
            if (value.IsSingleSegment)
            {
                return Parse(value.FirstSpan);
            }

            if (value.Length > 8)
            {
                throw new ArgumentException("Invalid InvitationStatusKind length", nameof(value));
            }

            Span<byte> tmp = stackalloc byte[8];
            value.CopyTo(tmp);
            return Parse(tmp[..(int)value.Length]);
        }

        /// <summary>
        /// Converts a <see cref="InvitationStatusKind"/> value to its
        /// lowercase UTF-8 encoded byte representation.
        /// </summary>
        /// <param name="value">
        /// The <see cref="InvitationStatusKind"/> value to convert.
        /// </param>
        /// <returns>
        /// A <see cref="ReadOnlySpan{Byte}"/> containing the lowercase UTF-8
        /// encoding of the specified <see cref="InvitationStatusKind"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="value"/> is not a defined
        /// <see cref="InvitationStatusKind"/> enumeration value.
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
        /// <item><c>PENDING</c></item>
        /// <item><c>ACCEPTED</c></item>
        /// <item><c>REJECTED</c></item>
        /// <item><c>REVOKED</c></item>
        /// </list>
        /// </para>
        /// <para>
        /// No allocations, no boxing, branch-predictable switch,
        /// and JIT-friendly control flow.
        /// </para>
        /// </remarks>
        public static ReadOnlySpan<byte> ToUtf8LowerBytes(InvitationStatusKind value)
        {
            return value switch
            {
                InvitationStatusKind.PENDING => "PENDING"u8,
                InvitationStatusKind.ACCEPTED => "ACCEPTED"u8,
                InvitationStatusKind.REJECTED => "REJECTED"u8,
                InvitationStatusKind.REVOKED => "REVOKED"u8,
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
