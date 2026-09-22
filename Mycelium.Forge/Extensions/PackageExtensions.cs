// ------------------------------------------------------------------------------------------------
// <copyright file="PackageExtensions.cs" company="Starion Group S.A.">
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
    /// Provides extension methods for <see cref="IPackage" /> instances.
    /// </summary>
    public static class PackageExtensions
    {
        /// <summary>
        /// Computes the fully qualified package name in the format @scope/packageName.
        /// </summary>
        /// <param name="package">The package instance.</param>
        /// <param name="owner">The owning scope instance.</param>
        /// <returns>The fully qualified package identifier string.</returns>
        public static string GetFullName(this IPackage package, IScope owner)
        {
            if (package == null || owner == null)
            {
                return string.Empty;
            }

            return package.GetFullName(owner.ShortName);
        }

        /// <summary>
        /// Computes the fully qualified package name in the format @scope/packageName.
        /// </summary>
        /// <param name="package">The package instance.</param>
        /// <param name="scopeShortName">The scope short name or publisher slug.</param>
        /// <returns>The fully qualified package identifier string.</returns>
        public static string GetFullName(this IPackage package, string scopeShortName)
        {
            if (package == null || string.IsNullOrWhiteSpace(scopeShortName))
            {
                return string.Empty;
            }

            var cleanScope = scopeShortName.TrimStart('@');
            return $"@{cleanScope}/{package.Name}";
        }
    }
}
