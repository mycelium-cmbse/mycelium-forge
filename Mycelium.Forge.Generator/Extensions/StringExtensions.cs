// ------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Extensions
{
    using System.Text;

    /// <summary>
    /// Extension methods for the <see cref="string" /> type.
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

        /// <summary>
        /// Converts the specified text to snake_case (lower-case words separated by underscores).
        /// </summary>
        /// <param name="text">The input text to convert.</param>
        /// <returns>The snake_case text, or an empty string if the input is null or empty.</returns>
        public static string ToSnakeCase(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            var builder = new StringBuilder(text.Length + 4);

            for (var index = 0; index < text.Length; index++)
            {
                var current = text[index];

                if (char.IsUpper(current) && index > 0)
                {
                    var previous = text[index - 1];
                    var startsNewWord = char.IsLower(previous) || char.IsDigit(previous) || (char.IsUpper(previous) && index + 1 < text.Length && char.IsLower(text[index + 1]));

                    if (startsNewWord)
                    {
                        builder.Append('_');
                    }
                }

                builder.Append(char.ToLowerInvariant(current));
            }

            return builder.ToString();
        }
    }
}
