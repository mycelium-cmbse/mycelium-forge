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
        public const string Package = "/packages/{id}";

        /// <summary>
        /// The scoped package details page route path.
        /// </summary>
        public const string ScopedPackage = "/packages/{scope}/{id}";

        /// <summary>
        /// The organization and publisher profile page route path.
        /// </summary>
        public const string Organization = "/organizations/{id}";

        /// <summary>
        /// The documentation page route path.
        /// </summary>
        public const string Docs = "/docs";

        /// <summary>
        /// The user authentication and sign-in page route path.
        /// </summary>
        public const string Login = "/login";

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
    }
}
