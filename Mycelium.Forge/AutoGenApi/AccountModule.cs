// ------------------------------------------------------------------------------------------------
// <copyright file="AccountModule.cs" company="Starion Group S.A.">
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
    /// Routes the Account endpoints of the Forge HTTP API.
    /// </summary>
    public sealed class AccountModule : ICarterModule
    {
        /// <summary>
        /// Registers the Account endpoints on the supplied route builder.
        /// </summary>
        /// <param name="app">
        /// The endpoint route builder the routes are registered on.
        /// </param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("/api/v1");

            api.MapGet("/account", () => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListAccount");

            api.MapPost("/account", () => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateAccount");

            api.MapGet("/account/{identifier:guid}", (Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountById");

            api.MapPut("/account/{identifier:guid}", (Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountById");

            api.MapPatch("/account/{identifier:guid}", (Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountById");

            api.MapDelete("/account/{identifier:guid}", (Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountById");

            api.MapGet("/account/{identifier:ShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountByShortGuid");

            api.MapPut("/account/{identifier:ShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountByShortGuid");

            api.MapPatch("/account/{identifier:ShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountByShortGuid");

            api.MapDelete("/account/{identifier:ShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountByShortGuid");

            api.MapGet("/account/{identifier:EnumerableOfShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountByShortGuids");

            api.MapGet("/account/{shortName}", (string shortName) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountByShortName");
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
