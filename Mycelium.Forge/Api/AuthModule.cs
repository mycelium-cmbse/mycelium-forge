// ------------------------------------------------------------------------------------------------
// <copyright file="AuthModule.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Api
{
    using System.Diagnostics.CodeAnalysis;

    using Carter;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Protocols.OpenIdConnect;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Routes the endpoints that start and end the OpenID Connect session. The identity provider callbacks
    /// (<c>/signin-oidc</c>, <c>/signout-callback-oidc</c>) are served by the authentication middleware itself.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class AuthModule : ICarterModule
    {
        /// <summary>
        /// The value of the OpenID Connect <c>prompt</c> parameter that opens the identity provider registration form.
        /// </summary>
        private const string CreatePrompt = "create";

        /// <summary>
        /// Registers the sign-in, sign-up and sign-out endpoints on the supplied route builder.
        /// </summary>
        /// <param name="app">The endpoint route builder the routes are registered on.</param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(PageRoutes.Login, ([FromQuery(Name = UrlParameterNames.ReturnUrl)] string returnUrl) => Results.Challenge(CreateProperties(returnUrl), [OpenIdConnectDefaults.AuthenticationScheme])).WithName("AuthLogin");

            app.MapGet(PageRoutes.SignUp, ([FromQuery(Name = UrlParameterNames.ReturnUrl)] string returnUrl) =>
            {
                var properties = CreateProperties(returnUrl);
                properties.SetParameter(OpenIdConnectParameterNames.Prompt, CreatePrompt);

                return Results.Challenge(properties, [OpenIdConnectDefaults.AuthenticationScheme]);
            }).WithName("AuthSignUp");

            app.MapGet(PageRoutes.Logout, () => Results.SignOut(CreateProperties(PageRoutes.Home), [CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme])).WithName("AuthLogout");
        }

        /// <summary>
        /// Creates the authentication properties redirecting back to a validated local URL.
        /// </summary>
        /// <param name="returnUrl">The requested return URL, which falls back to the home page unless it is local.</param>
        /// <returns>The <see cref="AuthenticationProperties" /> to pass to the authentication handlers.</returns>
        private static AuthenticationProperties CreateProperties(string returnUrl)
        {
            return new AuthenticationProperties { RedirectUri = returnUrl.GetSafeReturnUrl() };
        }
    }
}
