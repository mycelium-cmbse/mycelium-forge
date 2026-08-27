// ------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Tests.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="string" /> type in tests.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Normalizes all line endings in the specified text to CRLF (<c>\r\n</c>).
        /// </summary>
        /// <param name="text">The input text to normalize.</param>
        /// <returns>The text with normalized line endings.</returns>
        public static string NormalizeLineEndings(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            var normalized = text.Replace("\r\n", "\n").Replace("\r", "\n");
            return normalized.Replace("\n", "\r\n");
        }
    }
}
