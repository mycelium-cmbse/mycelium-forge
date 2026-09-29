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
        public const string Package = "/packages/{scope}/{packageName}";

        /// <summary>
        /// The package details tab page route path.
        /// </summary>
        public const string PackageTab = "/packages/{scope}/{packageName}/{tab}";

        /// <summary>
        /// The organization and publisher profile page route path.
        /// </summary>
        public const string Organization = "/organizations/{id}";

        /// <summary>
        /// The user authentication and sign-in page route path.
        /// </summary>
        public const string Login = "/login";

        /// <summary>
        /// The user registration and sign-up page route path.
        /// </summary>
        public const string SignUp = "/signup";

        /// <summary>
        /// The user email verification page route path.
        /// </summary>
        public const string VerifyEmail = "/verify-email";

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
        public const string PackageSettings = "/packages/{scope}/{packageName}/settings";

        /// <summary>
        /// The user account settings page route path.
        /// </summary>
        public const string AccountSettings = "/settings/account";

        /// <summary>
        /// The organization settings page route path.
        /// </summary>
        public const string OrganizationSettings = "/organizations/{id}/settings";

        /// <summary>
        /// The installation accounts administration page route path.
        /// </summary>
        public const string Accounts = "/admin/accounts";

        /// <summary>
        /// Generates the relative URL for the package details page, optionally with a specified tab.
        /// </summary>
        /// <param name="scope">The owning scope or publisher name.</param>
        /// <param name="packageName">The package name.</param>
        /// <param name="tab">The optional content tab identifier.</param>
        /// <returns>The formatted package route path.</returns>
        public static string GetPackageRoute(string scope, string packageName, string tab = null)
        {
            var cleanScope = (scope ?? string.Empty).TrimStart('@');

            if (string.IsNullOrWhiteSpace(tab))
            {
                return $"/packages/{cleanScope}/{packageName}";
            }

            return $"/packages/{cleanScope}/{packageName}/{tab.ToLowerInvariant()}";
        }

        /// <summary>
        /// Computes the navigation route URL from a package identifier name (e.g., @scope/packageName).
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
        /// <param name="id">The organization identifier or scope.</param>
        /// <returns>The formatted organization route path.</returns>
        public static string GetOrganizationRoute(string id)
        {
            var cleanId = (id ?? string.Empty).TrimStart('@');
            return $"/organizations/{cleanId}";
        }

        /// <summary>
        /// Generates the relative URL for the package settings page.
        /// </summary>
        /// <param name="scope">The owning scope or publisher name.</param>
        /// <param name="packageName">The package name.</param>
        /// <returns>The formatted package settings route path.</returns>
        public static string GetPackageSettingsRoute(string scope, string packageName)
        {
            var cleanScope = (scope ?? string.Empty).TrimStart('@');
            return $"/packages/{cleanScope}/{packageName}/settings";
        }

        /// <summary>
        /// Generates the relative URL for the organization settings page.
        /// </summary>
        /// <param name="id">The organization identifier or scope.</param>
        /// <returns>The formatted organization settings route path.</returns>
        public static string GetOrganizationSettingsRoute(string id)
        {
            var cleanId = (id ?? string.Empty).TrimStart('@');
            return $"/organizations/{cleanId}/settings";
        }

        /// <summary>
        /// Generates the relative URL for downloading a package version artifact.
        /// </summary>
        /// <param name="scope">The owning scope or publisher name.</param>
        /// <param name="packageName">The package name.</param>
        /// <param name="version">The package version string.</param>
        /// <returns>The formatted package download route path.</returns>
        public static string GetPackageDownloadRoute(string scope, string packageName, string version)
        {
            var cleanScope = (scope ?? string.Empty).TrimStart('@');
            return $"/api/packages/{cleanScope}/{packageName}/{version}/download";
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
