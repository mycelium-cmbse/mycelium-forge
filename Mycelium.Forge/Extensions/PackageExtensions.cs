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
        /// Computes the fully qualified package name in the format @organization/packageName.
        /// </summary>
        /// <param name="package">The package instance.</param>
        /// <param name="organization">The owning organization instance.</param>
        /// <returns>The fully qualified package identifier string.</returns>
        public static string GetFullName(this IPackage package, IOrganization organization)
        {
            if (package == null || organization == null)
            {
                return string.Empty;
            }

            return package.GetFullName(organization.ShortName);
        }

        /// <summary>
        /// Computes the fully qualified package name in the format @organization/packageName.
        /// </summary>
        /// <param name="package">The package instance.</param>
        /// <param name="organizationShortName">The organization short name or publisher slug.</param>
        /// <returns>The fully qualified package identifier string.</returns>
        public static string GetFullName(this IPackage package, string organizationShortName)
        {
            if (package == null || string.IsNullOrWhiteSpace(organizationShortName))
            {
                return string.Empty;
            }

            var cleanOrg = organizationShortName.TrimStart('@');
            return $"@{cleanOrg}/{package.Name}";
        }
    }
}
