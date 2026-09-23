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
        /// <summary>
        /// Sanitizes the string for logging by removing carriage return and newline characters to prevent log injection.
        /// </summary>
        /// <param name="value">The string value to sanitize.</param>
        /// <returns>A log-safe string value, or an empty string if the input is null or empty.</returns>
        public static string SanitizeForLog(this string value)
        {
            return string.IsNullOrEmpty(value)
                ? string.Empty
                : value.Replace("\r", string.Empty).Replace("\n", string.Empty);
        }
    }
}
