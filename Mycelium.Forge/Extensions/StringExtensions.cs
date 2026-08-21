// ------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extensions
{
    /// <summary>
    /// Provides extension methods for string manipulation.
    /// </summary>
    public static class StringExtensions
    {
        /// <param name="value">The string value to convert.</param>
        extension(string value)
        {
            /// <summary>
            /// Converts the first character of the string to uppercase and the remaining characters to lowercase.
            /// </summary>
            /// <returns>The converted string with uppercase characters, or an empty string if the input is null or empty.</returns>
            public string ToUpperCaseFirst()
            {
                if (string.IsNullOrEmpty(value))
                {
                    return string.Empty;
                }

                if (value.Length == 1)
                {
                    return char.ToUpperInvariant(value[0]).ToString();
                }

                return char.ToUpperInvariant(value[0]) + value[1..].ToLowerInvariant();
            }

            /// <summary>
            /// Computes the uppercase initials from a string of words separated by whitespace.
            /// </summary>
            /// <returns>The uppercase initials extracted from the first character of each word, or an empty string if the input is null or whitespace.</returns>
            public string ToInitials()
            {
                return string.IsNullOrWhiteSpace(value) 
                    ? string.Empty 
                    : string.Concat(value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(word => word[0])).ToUpperInvariant();
            }
        }
    }
}
