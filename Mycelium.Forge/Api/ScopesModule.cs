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
            app.MapGet("/api/v1/scopes/shortnames/{shortName}", ScopeShortNameExistsAsync)
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
        /// A 404 when no API key is configured or the scope does not exist, a 401 when the API key is missing or invalid,
        /// a 200 when the scope exists, or a 500 when it could not be read.
        /// </returns>
        public static async Task<IResult> ScopeShortNameExistsAsync(string shortName, HttpRequest request, IOptions<KeycloakSpiOptions> options, IScopeService scopeService, CancellationToken cancellationToken)
        {
            var apiKey = options.Value.ApiKey;

            if (string.IsNullOrEmpty(apiKey))
            {
                return Results.NotFound();
            }

            if (!IsApiKeyValid(request.Headers[KeycloakSpiOptions.ApiKeyHeaderName].ToString(), apiKey))
            {
                return Results.Unauthorized();
            }

            var systemContext = new UserContext
            {
                AccountId = Guid.Empty,
                Username = "system",
                CurrentRoles = [RoleKind.InstallationAdministrator]
            };

            var result = await scopeService.ReadByShortNameAsync(systemContext, cancellationToken, [shortName]);

            if (result.IsError)
            {
                return result.FirstError.Type == ErrorType.NotFound
                    ? Results.NotFound()
                    : Results.StatusCode(StatusCodes.Status500InternalServerError);
            }

            return result.Value.IsEmpty ? Results.NotFound() : Results.Ok();
        }

        /// <summary>
        /// Compares the presented API key to the configured one in constant time.
        /// </summary>
        /// <param name="presentedApiKey">The API key presented by the caller.</param>
        /// <param name="configuredApiKey">The configured API key.</param>
        /// <returns><see langword="true" /> when both keys are equal; otherwise <see langword="false" />.</returns>
        private static bool IsApiKeyValid(string presentedApiKey, string configuredApiKey)
        {
            var presented = SHA256.HashData(Encoding.UTF8.GetBytes(presentedApiKey));
            var configured = SHA256.HashData(Encoding.UTF8.GetBytes(configuredApiKey));

            return CryptographicOperations.FixedTimeEquals(presented, configured);
        }
    }
}
