// ------------------------------------------------------------------------------------------------
// <copyright file="LongExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extensions
{
    using System.Globalization;

    /// <summary>
    /// Provides extension methods for 64-bit integer values.
    /// </summary>
    public static class LongExtensions
    {
        /// <param name="bytes">The byte length to format.</param>
        extension(long bytes)
        {
            /// <summary>
            /// Formats binary byte length into a human-readable file size string (e.g., B, KB, MB, GB).
            /// </summary>
            /// <returns>A formatted string indicating the file size.</returns>
            public string FormatFileSize()
            {
                return bytes switch
                {
                    >= 1024 * 1024 * 1024 => string.Format(CultureInfo.InvariantCulture, "{0:0.#} GB", bytes / (1024.0 * 1024.0 * 1024.0)),
                    >= 1024 * 1024 => string.Format(CultureInfo.InvariantCulture, "{0:0.#} MB", bytes / (1024.0 * 1024.0)),
                    >= 1024 => string.Format(CultureInfo.InvariantCulture, "{0:0.#} KB", bytes / 1024.0),
                    _ => $"{bytes} B"
                };
            }
        }
    }
}
