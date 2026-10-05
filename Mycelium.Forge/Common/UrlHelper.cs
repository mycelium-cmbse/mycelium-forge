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
    using Microsoft.AspNetCore.Components;

    /// <summary>
    /// Provides helper methods for URL operations and security validations.
    /// </summary>
    public static class UrlHelper
    {
        /// <param name="url">The URL string to evaluate.</param>
        extension(string url)
        {
            /// <summary>
            /// Determines whether the specified URL is a local relative URL to prevent open redirect vulnerabilities.
            /// </summary>
            /// <returns><c>true</c> if the URL is local and relative; otherwise, <c>false</c>.</returns>
            public bool IsLocalUrl()
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
            /// Sanitizes and returns a safe local return URL, falling back to a default route if the specified URL is invalid or
            /// targets login.
            /// </summary>
            /// <param name="fallbackUrl">
            /// The fallback URL to return if the candidate is invalid. Defaults to
            /// <see cref="PageRoutes.Home" />.
            /// </param>
            /// <returns>A safe local relative route string.</returns>
            public string GetSafeReturnUrl(string fallbackUrl = PageRoutes.Home)
            {
                if (string.IsNullOrWhiteSpace(url) ||
                    !url.IsLocalUrl() ||
                    string.Equals(url.TrimStart('/'), PageRoutes.Login.TrimStart('/'), StringComparison.OrdinalIgnoreCase))
                {
                    return fallbackUrl;
                }

                return url;
            }

            /// <summary>
            /// Computes the target sign-in URL, appending the sanitized return URL query parameter if available.
            /// </summary>
            /// <param name="navigationManager">The optional navigation manager used to determine the current relative path.</param>
            /// <returns>The computed login URL string.</returns>
            public string GetSignInUrl(NavigationManager navigationManager = null)
            {
                var candidate = !string.IsNullOrWhiteSpace(url)
                    ? url
                    : navigationManager?.ToBaseRelativePath(navigationManager.Uri);

                var safeReturnUrl = candidate.GetSafeReturnUrl(null);

                if (string.IsNullOrWhiteSpace(safeReturnUrl))
                {
                    return PageRoutes.Login;
                }

                return $"{PageRoutes.Login}?{UrlParameterNames.ReturnUrl}={Uri.EscapeDataString(safeReturnUrl)}";
            }

            /// <summary>
            /// Computes the authentication cookie login endpoint URL with the sanitized return URL query parameter.
            /// </summary>
            /// <param name="fallbackUrl">The fallback URL if candidate is invalid. Defaults to <see cref="PageRoutes.Home" />.</param>
            /// <returns>The formatted authentication endpoint URL string.</returns>
            public string GetAuthLoginUrl(string fallbackUrl = PageRoutes.Home)
            {
                var safeRoute = url.GetSafeReturnUrl(fallbackUrl);
                return $"{PageRoutes.AuthLogin}?{UrlParameterNames.ReturnUrl}={Uri.EscapeDataString(safeRoute)}";
            }
        }
    }
}
