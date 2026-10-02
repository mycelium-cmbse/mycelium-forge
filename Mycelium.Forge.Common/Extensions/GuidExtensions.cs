// ------------------------------------------------------------------------------------------------
// <copyright file="GuidExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.Extensions
{
    using System;

    /// <summary>
    /// Provides extension methods for <see cref="Guid" /> manipulation and sanitization.
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        /// The number of unmasked trailing characters preserved when redacting an identifier for logging.
        /// </summary>
        private const int PreservedCharacterCount = 8;

        /// <param name="value">The <see cref="Guid" /> value to sanitize.</param>
        extension(Guid value)
        {
            /// <summary>
            /// Sanitizes the <see cref="Guid" /> for logging by masking all but the last 8 characters to prevent sensitive identifier
            /// exposure.
            /// </summary>
        /// <param name="value">The <see cref="Guid" /> value to sanitize.</param>
            /// <returns>A redacted, log-safe string representation of the identifier.</returns>
            public string SanitizeForLog()
            {
                var valueText = value.ToString("N");
                return $"***{valueText[^PreservedCharacterCount..]}";
            }
        }
    }
}
