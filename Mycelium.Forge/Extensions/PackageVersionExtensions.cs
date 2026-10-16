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
        /// <param name="packageVersion">The package version DTO.</param>
        extension(IPackageVersion packageVersion)
        {
            /// <summary>
            /// Gets the version string from the package version DTO, or returns an empty string if null or empty.
            /// </summary>
            /// <returns>The resolved version string.</returns>
            public string GetVersion()
            {
                return packageVersion?.Version ?? string.Empty;
            }
        }
    }
}
