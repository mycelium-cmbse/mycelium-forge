// ------------------------------------------------------------------------------------------------
// <copyright file="APIKeyModule.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Api
{
    using Carter;

    /// <summary>
    /// Routes the APIKey endpoints of the Forge HTTP API.
    /// </summary>
    public sealed class APIKeyModule : ICarterModule
    {
        /// <summary>
        /// Registers the APIKey endpoints on the supplied route builder.
        /// </summary>
        /// <param name="app">
        /// The endpoint route builder the routes are registered on.
        /// </param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("/api/v1");

            api.MapGet("/account/{accountIdentifier:guid}/apiKey", (Guid accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListAccountIdApiKey");

            api.MapPost("/account/{accountIdentifier:guid}/apiKey", (Guid accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateAccountIdAPIKey");

            api.MapGet("/account/{accountIdentifier:guid}/apiKey/{identifier:guid}", (Guid accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdAPIKeyById");

            api.MapPut("/account/{accountIdentifier:guid}/apiKey/{identifier:guid}", (Guid accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountIdAPIKeyById");

            api.MapPatch("/account/{accountIdentifier:guid}/apiKey/{identifier:guid}", (Guid accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountIdAPIKeyById");

            api.MapDelete("/account/{accountIdentifier:guid}/apiKey/{identifier:guid}", (Guid accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountIdAPIKeyById");

            api.MapGet("/account/{accountIdentifier:guid}/apiKey/{identifier:ShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdAPIKeyByShortGuid");

            api.MapPut("/account/{accountIdentifier:guid}/apiKey/{identifier:ShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountIdAPIKeyByShortGuid");

            api.MapPatch("/account/{accountIdentifier:guid}/apiKey/{identifier:ShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountIdAPIKeyByShortGuid");

            api.MapDelete("/account/{accountIdentifier:guid}/apiKey/{identifier:ShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountIdAPIKeyByShortGuid");

            api.MapGet("/account/{accountIdentifier:guid}/apiKey/{identifier:EnumerableOfShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdAPIKeyByShortGuids");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/apiKey", (string accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListAccountShortGuidApiKey");

            api.MapPost("/account/{accountIdentifier:ShortGuid}/apiKey", (string accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateAccountShortGuidAPIKey");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/apiKey/{identifier:guid}", (string accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidAPIKeyById");

            api.MapPut("/account/{accountIdentifier:ShortGuid}/apiKey/{identifier:guid}", (string accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountShortGuidAPIKeyById");

            api.MapPatch("/account/{accountIdentifier:ShortGuid}/apiKey/{identifier:guid}", (string accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountShortGuidAPIKeyById");

            api.MapDelete("/account/{accountIdentifier:ShortGuid}/apiKey/{identifier:guid}", (string accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountShortGuidAPIKeyById");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/apiKey/{identifier:ShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidAPIKeyByShortGuid");

            api.MapPut("/account/{accountIdentifier:ShortGuid}/apiKey/{identifier:ShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountShortGuidAPIKeyByShortGuid");

            api.MapPatch("/account/{accountIdentifier:ShortGuid}/apiKey/{identifier:ShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountShortGuidAPIKeyByShortGuid");

            api.MapDelete("/account/{accountIdentifier:ShortGuid}/apiKey/{identifier:ShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountShortGuidAPIKeyByShortGuid");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/apiKey/{identifier:EnumerableOfShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidAPIKeyByShortGuids");
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
