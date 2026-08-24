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
        /// The package browsing and discovery page route path.
        /// </summary>
        public const string Packages = "/packages";

        /// <summary>
        /// The package publishing page route path.
        /// </summary>
        public const string Publish = "/publish";

        /// <summary>
        /// The package details page route path.
        /// </summary>
        public const string Package = "/packages/{organization}/{packageName}";

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
        public const string PackageSettings = "/packages/{organization}/{packageName}/settings";

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
        /// Generates the relative URL for the package details page.
        /// </summary>
        /// <param name="organization">The organization scope or publisher name.</param>
        /// <param name="packageName">The package name.</param>
        /// <returns>The formatted package route path.</returns>
        public static string GetPackageRoute(string organization, string packageName)
        {
            var cleanOrg = (organization ?? string.Empty).TrimStart('@');
            return $"/packages/{cleanOrg}/{packageName}";
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
        /// <param name="organization">The organization scope or publisher name.</param>
        /// <param name="packageName">The package name.</param>
        /// <returns>The formatted package settings route path.</returns>
        public static string GetPackageSettingsRoute(string organization, string packageName)
        {
            var cleanOrg = (organization ?? string.Empty).TrimStart('@');
            return $"/packages/{cleanOrg}/{packageName}/settings";
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
