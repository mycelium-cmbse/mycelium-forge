// ------------------------------------------------------------------------------------------------
// <copyright file="CountryModule.cs" company="Starion Group S.A.">
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
    /// Routes the Country endpoints of the Forge HTTP API.
    /// </summary>
    public sealed class CountryModule : ICarterModule
    {
        /// <summary>
        /// Registers the Country endpoints on the supplied route builder.
        /// </summary>
        /// <param name="app">
        /// The endpoint route builder the routes are registered on.
        /// </param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("/api/v1");

            api.MapGet("/country", () => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListCountry");

            api.MapPost("/country", () => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateCountry");

            api.MapGet("/country/{identifier:guid}", (Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetCountryById");

            api.MapPut("/country/{identifier:guid}", (Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetCountryById");

            api.MapPatch("/country/{identifier:guid}", (Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateCountryById");

            api.MapDelete("/country/{identifier:guid}", (Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteCountryById");

            api.MapGet("/country/{identifier:ShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetCountryByShortGuid");

            api.MapPut("/country/{identifier:ShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetCountryByShortGuid");

            api.MapPatch("/country/{identifier:ShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateCountryByShortGuid");

            api.MapDelete("/country/{identifier:ShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteCountryByShortGuid");

            api.MapGet("/country/{identifier:EnumerableOfShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetCountryByShortGuids");
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
