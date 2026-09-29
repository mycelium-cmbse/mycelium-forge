// ------------------------------------------------------------------------------------------------
// <copyright file="PackageVersionExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extensions
{
    using Mycelium.Forge.Common;

    /// <summary>
    /// Provides extension methods for <see cref="IPackageVersion" /> instances.
    /// </summary>
    public static class PackageVersionExtensions
    {
        /// <summary>
        /// Gets the version string from the package version DTO, or returns an empty string if null or empty.
        /// </summary>
        /// <param name="packageVersion">The package version DTO.</param>
        /// <returns>The resolved version string.</returns>
        public static string GetVersion(this IPackageVersion packageVersion)
        {
            return packageVersion?.Version ?? string.Empty;
        }

        /// <summary>
        /// Formats binary byte length into a human-readable file size string (e.g., KB, MB, GB).
        /// </summary>
        /// <param name="bytes">The byte length to format.</param>
        /// <returns>A formatted string indicating the file size.</returns>
        public static string FormatFileSize(long bytes)
        {
            return bytes switch
            {
                >= 1024 * 1024 * 1024 => $"{bytes / (1024.0 * 1024.0 * 1024.0):0.#} GB",
                >= 1024 * 1024 => $"{bytes / (1024.0 * 1024.0):0.#} MB",
                >= 1024 => $"{bytes / 1024.0:0.#} KB",
                _ => $"{bytes} B"
            };
        }

        /// <summary>
        /// Formats the artifact size of the package version into a human-readable file size string.
        /// </summary>
        /// <param name="packageVersion">The package version DTO.</param>
        /// <returns>The human-readable formatted file size string.</returns>
        public static string GetFormattedSize(this IPackageVersion packageVersion)
        {
            return FormatFileSize(packageVersion.SizeInBytes);
        }
    }
}
