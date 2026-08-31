// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationModule.cs" company="Starion Group S.A.">
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
    /// Routes the Organization endpoints of the Forge HTTP API.
    /// </summary>
    public sealed class OrganizationModule : ICarterModule
    {
        /// <summary>
        /// Registers the Organization endpoints on the supplied route builder.
        /// </summary>
        /// <param name="app">
        /// The endpoint route builder the routes are registered on.
        /// </param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("/api/v1");

            api.MapGet("/organization", () => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListOrganization");

            api.MapPost("/organization", () => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateOrganization");

            api.MapGet("/organization/{identifier:guid}", (Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationById");

            api.MapPut("/organization/{identifier:guid}", (Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationById");

            api.MapPatch("/organization/{identifier:guid}", (Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationById");

            api.MapDelete("/organization/{identifier:guid}", (Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationById");

            api.MapGet("/organization/{identifier:ShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationByShortGuid");

            api.MapPut("/organization/{identifier:ShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationByShortGuid");

            api.MapPatch("/organization/{identifier:ShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationByShortGuid");

            api.MapDelete("/organization/{identifier:ShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationByShortGuid");

            api.MapGet("/organization/{identifier:EnumerableOfShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationByShortGuids");

            api.MapGet("/organization/{shortName}", (string shortName) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationByShortName");
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
