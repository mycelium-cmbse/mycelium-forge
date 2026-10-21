// ------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.Extensions
{
    /// <summary>
    /// Provides extension methods for string manipulation.
    /// </summary>
    public static class StringExtensions
    {
        /// <param name="value">The string value to sanitize.</param>
        extension(string value)
        {
            /// <summary>
            /// Sanitizes the string for logging by removing carriage return and newline characters to prevent log injection.
            /// </summary>
            /// <returns>A log-safe string value, or an empty string if the input is null or empty.</returns>
            public string SanitizeForLog()
            {
                return string.IsNullOrEmpty(value)
                    ? string.Empty
                    : value.Replace("\r", string.Empty).Replace("\n", string.Empty);
            }

            /// <summary>
            /// Converts the first character of the string to lowercase using invariant culture.
            /// </summary>
            /// <returns>The string with its first character converted to lowercase, or an empty string if the input is null or empty.</returns>
            public string ToLowerCaseFirst()
            {
                if (string.IsNullOrEmpty(value))
                {
                    return string.Empty;
                }

                if (value.Length == 1)
                {
                    return char.ToLowerInvariant(value[0]).ToString();
                }

                return char.ToLowerInvariant(value[0]) + value[1..];
            }
        }
    }
}
