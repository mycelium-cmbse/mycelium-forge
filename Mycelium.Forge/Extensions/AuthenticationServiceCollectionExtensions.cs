// ------------------------------------------------------------------------------------------------
// <copyright file="AuthenticationServiceCollectionExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extensions
{
    using System.Security.Claims;
    using System.Text.Json;

    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.IdentityModel.JsonWebTokens;
    using Microsoft.IdentityModel.Protocols.OpenIdConnect;
    using Microsoft.IdentityModel.Tokens;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Services;

    /// <summary>
    /// Provides extension methods registering OpenID Connect interactive authentication (F1-01, SSS-FG-AUTH-S1A).
    /// </summary>
    public static class AuthenticationServiceCollectionExtensions
    {
        /// <summary>
        /// The claim type carrying the preferred username issued by the identity provider.
        /// </summary>
        private const string PreferredUsernameClaimType = "preferred_username";

        /// <summary>
        /// The claim type carrying the realm access of the user in the access token.
        /// </summary>
        private const string RealmAccessClaimType = "realm_access";

        /// <summary>
        /// The property of the realm access claim listing the realm roles.
        /// </summary>
        private const string RolesProperty = "roles";

        /// <summary>
        /// Adds the Forge authentication services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to configure.</param>
        extension(IServiceCollection services)
        {
            /// <summary>
            /// Registers cookie sessions backed by OpenID Connect sign-in against the configured identity provider. Switching
            /// provider is a configuration change only (<c>Authentication:Authority</c>, <c>ClientId</c>, <c>ClientSecret</c>).
            /// </summary>
            /// <param name="configuration">The application configuration.</param>
            /// <returns>The configured <see cref="IServiceCollection" /> instance.</returns>
            public IServiceCollection AddForgeAuthentication(IConfiguration configuration)
            {
                var authenticationSection = configuration.GetSection(AuthenticationOptions.SectionName);
                var authenticationOptions = authenticationSection.Get<AuthenticationOptions>() ?? new AuthenticationOptions();
                var securePolicy = authenticationOptions.RequireHttpsMetadata ? CookieSecurePolicy.Always : CookieSecurePolicy.SameAsRequest;

                services.Configure<AuthenticationOptions>(authenticationSection);

                services.AddAuthentication(options =>
                    {
                        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    })
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                    {
                        options.LoginPath = PageRoutes.Login;
                        options.LogoutPath = PageRoutes.Logout;
                        options.ExpireTimeSpan = TimeSpan.FromDays(7);
                        options.SlidingExpiration = true;
                        options.Cookie.SecurePolicy = securePolicy;
                    })
                    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                    {
                        options.MapInboundClaims = false;
                        options.Authority = authenticationOptions.Authority;
                        options.MetadataAddress = authenticationOptions.MetadataAddress;
                        options.ClientId = authenticationOptions.ClientId;
                        options.ClientSecret = authenticationOptions.ClientSecret;
                        options.RequireHttpsMetadata = authenticationOptions.RequireHttpsMetadata;
                        options.ResponseType = OpenIdConnectResponseType.Code;
                        options.ResponseMode = OpenIdConnectResponseMode.Query;
                        options.UsePkce = true;
                        options.SaveTokens = true;
                        options.GetClaimsFromUserInfoEndpoint = true;

                        options.Scope.Clear();
                        options.Scope.Add("openid");
                        options.Scope.Add("profile");
                        options.Scope.Add("email");

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            NameClaimType = PreferredUsernameClaimType,
                            RoleClaimType = ClaimTypes.Role
                        };

                        options.CorrelationCookie.SameSite = SameSiteMode.Lax;
                        options.CorrelationCookie.SecurePolicy = securePolicy;
                        options.NonceCookie.SameSite = SameSiteMode.Lax;
                        options.NonceCookie.SecurePolicy = securePolicy;

                        options.Events.OnTokenValidated = OnTokenValidated;

                        options.Events.OnRemoteFailure = context =>
                        {
                            context.Response.Redirect(PageRoutes.Home);
                            context.HandleResponse();
                            return Task.CompletedTask;
                        };
                    });

                services.AddScoped<IAccountProvisioner, AccountProvisioner>();

                return services;
            }
        }

        /// <summary>
        /// Adds the realm roles of the access token to the signed-in identity and provisions the Forge account of the user
        /// on first sign-in.
        /// </summary>
        /// <remarks>
        /// The ID token does not carry the realm roles, the access token does. Roles unknown to <see cref="RoleKind" /> are
        /// ignored.
        /// </remarks>
        /// <param name="context">The <see cref="TokenValidatedContext" /> of the sign-in.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private static async Task OnTokenValidated(TokenValidatedContext context)
        {
            if (context.Principal?.Identity is not ClaimsIdentity identity)
            {
                context.Fail("The validated token principal has no claims identity.");
                return;
            }

            foreach (var role in ReadRealmRoles(context.TokenEndpointResponse?.AccessToken))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var provisioner = context.HttpContext.RequestServices.GetRequiredService<IAccountProvisioner>();
            var result = await provisioner.ProvisionAsync(context.Principal, context.HttpContext.RequestAborted);

            if (result.IsError)
            {
                context.Fail(string.Join(", ", result.Errors.Select(error => error.Description)));
            }
        }

        /// <summary>
        /// Reads the Forge roles from the <c>realm_access</c> claim of an access token.
        /// </summary>
        /// <param name="accessToken">The access token issued by the identity provider.</param>
        /// <returns>The distinct <see cref="RoleKind" /> values the token grants.</returns>
        private static IEnumerable<RoleKind> ReadRealmRoles(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return [];
            }

            var token = new JsonWebTokenHandler().ReadJsonWebToken(accessToken);

            if (!token.TryGetPayloadValue<JsonElement>(RealmAccessClaimType, out var realmAccess) ||
                !realmAccess.TryGetProperty(RolesProperty, out var roles) ||
                roles.ValueKind != JsonValueKind.Array)
            {
                return [];
            }

            return roles.EnumerateArray()
                .Select(role => Enum.TryParse<RoleKind>(role.GetString(), out var kind) ? kind : (RoleKind?)null)
                .Where(role => role.HasValue)
                .Select(role => role.Value)
                .Distinct();
        }
    }
}
