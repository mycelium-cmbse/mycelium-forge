// ------------------------------------------------------------------------------------------------
// <copyright file="UrlHelper.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    using System;

    using Microsoft.AspNetCore.Components;

    /// <summary>
    /// Provides helper methods for URL operations and security validations.
    /// </summary>
    public static class UrlHelper
    {
        /// <summary>
        /// Determines whether the specified URL is a local relative URL to prevent open redirect vulnerabilities.
        /// </summary>
        /// <param name="url">The URL string to evaluate.</param>
        /// <returns><c>true</c> if the URL is local and relative; otherwise, <c>false</c>.</returns>
        public static bool IsLocalUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            if (url.StartsWith('/') || url.StartsWith('\\'))
            {
                if (url.Length == 1)
                {
                    return url[0] == '/';
                }

                return url[0] == '/' && url[1] != '/' && url[1] != '\\';
            }

            if (Uri.TryCreate(url, UriKind.Relative, out var uri) && !uri.IsAbsoluteUri)
            {
                return !url.Contains(':') && !url.StartsWith("//", StringComparison.Ordinal) && !url.StartsWith("/\\", StringComparison.Ordinal);
            }

            return false;
        }

        /// <summary>
        /// Sanitizes and returns a safe local return URL, falling back to a default route if the specified URL is invalid or targets login.
        /// </summary>
        /// <param name="returnUrl">The candidate return URL to evaluate.</param>
        /// <param name="fallbackUrl">The fallback URL to return if the candidate is invalid. Defaults to <see cref="PageRoutes.Home" />.</param>
        /// <returns>A safe local relative route string.</returns>
        public static string GetSafeReturnUrl(string returnUrl, string fallbackUrl = PageRoutes.Home)
        {
            if (string.IsNullOrWhiteSpace(returnUrl) ||
                !IsLocalUrl(returnUrl) ||
                string.Equals(returnUrl.TrimStart('/'), PageRoutes.Login.TrimStart('/'), StringComparison.OrdinalIgnoreCase))
            {
                return fallbackUrl;
            }

            return returnUrl;
        }

        /// <summary>
        /// Computes the target sign-in URL, appending the sanitized return URL query parameter if available.
        /// </summary>
        /// <param name="returnUrl">The explicit return URL string, if any.</param>
        /// <param name="navigationManager">The optional navigation manager used to determine the current relative path.</param>
        /// <returns>The computed login URL string.</returns>
        public static string GetSignInUrl(string returnUrl = null, NavigationManager navigationManager = null)
        {
            var candidate = !string.IsNullOrWhiteSpace(returnUrl)
                ? returnUrl
                : navigationManager?.ToBaseRelativePath(navigationManager.Uri);

            var safeReturnUrl = GetSafeReturnUrl(candidate, null);

            if (string.IsNullOrWhiteSpace(safeReturnUrl))
            {
                return PageRoutes.Login;
            }

            return $"{PageRoutes.Login}?{UrlParameterNames.ReturnUrl}={Uri.EscapeDataString(safeReturnUrl)}";
        }

        /// <summary>
        /// Computes the authentication cookie login endpoint URL with the sanitized return URL query parameter.
        /// </summary>
        /// <param name="returnUrl">The candidate return URL.</param>
        /// <param name="fallbackUrl">The fallback URL if candidate is invalid. Defaults to <see cref="PageRoutes.Home" />.</param>
        /// <returns>The formatted authentication endpoint URL string.</returns>
        public static string GetAuthLoginUrl(string returnUrl = null, string fallbackUrl = PageRoutes.Home)
        {
            var safeRoute = GetSafeReturnUrl(returnUrl, fallbackUrl);
            return $"{PageRoutes.AuthLogin}?{UrlParameterNames.ReturnUrl}={Uri.EscapeDataString(safeRoute)}";
        }
    }
}
