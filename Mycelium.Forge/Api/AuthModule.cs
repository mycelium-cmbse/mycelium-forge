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
    using System.Security.Claims;

    using Carter;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Data;
    using Mycelium.Forge.Services;

    /// <summary>
    /// Routes the local cookie authentication endpoints for user login and logout.
    /// </summary>
    /// <remarks>
    /// This module is a temporary placeholder used solely to allow mock authentication data during development.
    /// It MUST be removed once OpenID Connect (OIDC) is fully configured and MUST NEVER be used in real production.
    /// </remarks>
    public sealed class AuthModule : ICarterModule
    {
        /// <summary>
        /// Registers the authentication endpoints on the supplied route builder.
        /// </summary>
        /// <remarks>
        /// This method registers temporary local mock login/logout endpoints that will be replaced when OIDC is configured.
        /// </remarks>
        /// <param name="app">The endpoint route builder the routes are registered on.</param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var auth = app.MapGroup("/api/auth");

            auth.MapGet("/login", async (HttpContext httpContext, IUserService userService, CancellationToken cancellationToken) =>
            {
                var regisAccount = SeedData.RegisAccount;
                await userService.SetCurrentUser(regisAccount.Id, cancellationToken);

                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, regisAccount.Id.ToString()),
                    new(ClaimTypes.Name, regisAccount.ShortName),
                    new(ClaimTypes.Email, regisAccount.Email),
                    new(ClaimTypes.Role, nameof(RoleKind.InstallationAdministrator)),
                    new(ClaimTypes.Role, nameof(RoleKind.OrganizationAdministrator)),
                    new(ClaimTypes.Role, nameof(RoleKind.Account))
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                var principal = new ClaimsPrincipal(identity);

                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return Results.LocalRedirect(PageRoutes.Home);
            }).WithName("AuthLogin");

            auth.MapGet("/logout", async (HttpContext httpContext, IUserService userService, CancellationToken cancellationToken) =>
            {
                await userService.SetCurrentUser(null, cancellationToken);
                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return Results.LocalRedirect(PageRoutes.Home);
            }).WithName("AuthLogout");
        }
    }
}
