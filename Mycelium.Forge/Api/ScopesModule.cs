// ------------------------------------------------------------------------------------------------
// <copyright file="ScopesModule.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Api
{
    using System.Security.Cryptography;
    using System.Text;

    using Carter;

    using ErrorOr;

    using Microsoft.Extensions.Options;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.Services;

    /// <summary>
    /// Routes the scope-oriented endpoints of the Forge HTTP API.
    /// </summary>
    public sealed class ScopesModule : ICarterModule
    {
        /// <summary>
        /// Registers the scope endpoints on the supplied route builder.
        /// </summary>
        /// <param name="app">The endpoint route builder the routes are registered on.</param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            // Used by the Keycloak username validator to keep usernames from matching the short name of an existing scope.
            app.MapGet(ApiRoutes.ScopeShortNameExists, ScopeShortNameExistsAsync)
                .WithName("ScopeShortNameExists");
        }

        /// <summary>
        /// Determines whether an <see cref="IScope" /> with the specified short name exists.
        /// </summary>
        /// <param name="shortName">The short name to look up.</param>
        /// <param name="request">The current <see cref="HttpRequest" />.</param>
        /// <param name="options">The injected <see cref="KeycloakSpiOptions" />.</param>
        /// <param name="scopeService">The injected <see cref="IScopeService" />.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A 401 when the secret is missing, invalid, or unconfigured; a 404 when the scope does not exist; a 200 when the
        /// scope exists; or a 500 when it could not be read.
        /// </returns>
        public static async Task<IResult> ScopeShortNameExistsAsync(string shortName, HttpRequest request, IOptions<KeycloakSpiOptions> options, IScopeService scopeService, CancellationToken cancellationToken)
        {
            var secret = options.Value.Secret;

            if (string.IsNullOrEmpty(secret) || !IsSecretValid(request.Headers[KeycloakSpiOptions.SecretHeaderName].ToString(), secret))
            {
                return Results.Unauthorized();
            }

            var systemContext = new UserContext
            {
                AccountId = Guid.Empty,
                Username = "system",
                CurrentRoles = [RoleKind.InstallationAdministrator]
            };

            var result = await scopeService.ReadByShortNameAsync(systemContext, cancellationToken, [shortName.ToLowerInvariant()]);

            if (result.IsError)
            {
                return result.FirstError.Type == ErrorType.NotFound
                    ? Results.NotFound()
                    : Results.StatusCode(StatusCodes.Status500InternalServerError);
            }

            return result.Value.IsEmpty ? Results.NotFound() : Results.Ok();
        }

        /// <summary>
        /// Compares the presented secret to the configured one in constant time.
        /// </summary>
        /// <param name="presentedSecret">The secret presented by the caller.</param>
        /// <param name="configuredSecret">The configured secret.</param>
        /// <returns><see langword="true" /> when both secrets are equal; otherwise <see langword="false" />.</returns>
        private static bool IsSecretValid(string presentedSecret, string configuredSecret)
        {
            var presented = SHA256.HashData(Encoding.UTF8.GetBytes(presentedSecret));
            var configured = SHA256.HashData(Encoding.UTF8.GetBytes(configuredSecret));

            return CryptographicOperations.FixedTimeEquals(presented, configured);
        }
    }
}
