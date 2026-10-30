// ------------------------------------------------------------------------------------------------
// <copyright file="PageRoutes.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    using Mycelium.Forge.Extensions;

    /// <summary>
    /// Defines application route path constants used for navigation and link generation.
    /// </summary>
    public static class PageRoutes
    {
        /// <summary>
        /// The root landing page route path.
        /// </summary>
        public const string Home = "/";

        /// <summary>
        /// The package search and discovery page route path.
        /// </summary>
        public const string Packages = "/packages";

        /// <summary>
        /// The package publishing page route path.
        /// </summary>
        public const string Publish = "/publish";

        /// <summary>
        /// The package details page route path.
        /// </summary>
        public const string Package = "/packages/{scope}/{packageShortname}";

        /// <summary>
        /// The package details tab page route path.
        /// </summary>
        public const string PackageTab = "/packages/{scope}/{packageShortname}/{tab}";

        /// <summary>
        /// The organization and publisher profile page route path.
        /// </summary>
        public const string Organization = "/organizations/{shortName}";

        /// <summary>
        /// The sign-in endpoint route path, which redirects to the OpenID Connect identity provider.
        /// </summary>
        public const string Login = "/login";

        /// <summary>
        /// The sign-out endpoint route path, which also ends the identity provider session.
        /// </summary>
        public const string Logout = "/api/auth/logout";

        /// <summary>
        /// The sign-up endpoint route path, which redirects to the identity provider registration form.
        /// </summary>
        public const string SignUp = "/signup";

        /// <summary>
        /// The generic error handling page route path.
        /// </summary>
        public const string Error = "/error";

        /// <summary>
        /// The page not found route path.
        /// </summary>
        public const string NotFound = "/not-found";

        /// <summary>
        /// The user's personal package management page route path.
        /// </summary>
        public const string MyPackages = "/my-packages";

        /// <summary>
        /// The API key management page route path.
        /// </summary>
        public const string ApiKeys = "/api-keys";

        /// <summary>
        /// The package settings page route path.
        /// </summary>
        public const string PackageSettings = "/packages/{scope}/{packageShortname}/settings";

        /// <summary>
        /// The user account settings page route path.
        /// </summary>
        public const string AccountSettings = "/settings/account";

        /// <summary>
        /// The organization settings page route path.
        /// </summary>
        public const string OrganizationSettings = "/organizations/{shortName}/settings";

        /// <summary>
        /// The installation accounts administration page route path.
        /// </summary>
        public const string Accounts = "/admin/accounts";

        /// <summary>
        /// Generates the relative URL for the package details page, optionally with a specified tab.
        /// </summary>
        /// <param name="scope">The owning scope or publisher name.</param>
        /// <param name="packageShortname">The package short name.</param>
        /// <param name="tab">The optional content tab identifier.</param>
        /// <returns>The formatted package route path.</returns>
        public static string GetPackageRoute(string scope, string packageShortname, string tab = null)
        {
            var cleanScope = scope.CleanScope();

            if (string.IsNullOrWhiteSpace(tab))
            {
                return $"/packages/{cleanScope}/{packageShortname}";
            }

            return $"/packages/{cleanScope}/{packageShortname}/{tab.ToLowerInvariant()}";
        }

        /// <summary>
        /// Computes the navigation route URL from a package identifier name (e.g., @scope/packageShortname).
        /// </summary>
        /// <param name="name">The package identifier coordinate string.</param>
        /// <returns>The resolved package route URL, or a hash fallback if not matched.</returns>
        public static string GetHrefFromPackageFullName(string name)
        {
            var parts = (name ?? string.Empty).TrimStart('@').Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 2)
            {
                return GetPackageRoute(parts[0], parts[1]);
            }

            return $"#{name}";
        }

        /// <summary>
        /// Generates the relative URL for the organization profile page.
        /// </summary>
        /// <param name="shortName">The organization identifier or scope.</param>
        /// <returns>The formatted organization route path.</returns>
        public static string GetOrganizationRoute(string shortName)
        {
            var cleanShortName = shortName.CleanScope();
            return $"/organizations/{cleanShortName}";
        }

        /// <summary>
        /// Generates the relative URL for the package settings page.
        /// </summary>
        /// <param name="scope">The owning scope or publisher name.</param>
        /// <param name="packageShortname">The package short name.</param>
        /// <returns>The formatted package settings route path.</returns>
        public static string GetPackageSettingsRoute(string scope, string packageShortname)
        {
            var cleanScope = scope.CleanScope();
            return $"/packages/{cleanScope}/{packageShortname}/settings";
        }

        /// <summary>
        /// Generates the relative URL for the organization settings page.
        /// </summary>
        /// <param name="shortName">The organization identifier or scope.</param>
        /// <returns>The formatted organization settings route path.</returns>
        public static string GetOrganizationSettingsRoute(string shortName)
        {
            var cleanShortName = shortName.CleanScope();
            return $"/organizations/{cleanShortName}/settings";
        }

        /// <summary>
        /// Generates the relative URL for downloading a package version artifact.
        /// </summary>
        /// <param name="scope">The owning scope or publisher name.</param>
        /// <param name="packageShortname">The package short name.</param>
        /// <param name="version">The package version string.</param>
        /// <returns>The formatted package download route path.</returns>
        public static string GetPackageDownloadRoute(string scope, string packageShortname, string version)
        {
            var cleanScope = scope.CleanScope();
            return $"/api/packages/{cleanScope}/{packageShortname}/{version}/download";
        }

        /// <summary>
        /// Contains route constants for documentation pages.
        /// </summary>
        public static class Documentation
        {
            /// <summary>
            /// The documentation overview page route path.
            /// </summary>
            public const string Overview = "/docs";

            /// <summary>
            /// The documentation packages and kpar format page route path.
            /// </summary>
            public const string PackagesAndKparFormat = "/docs/packages-and-the-kpar-format";
        }
    }
}
