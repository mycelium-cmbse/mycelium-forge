// ------------------------------------------------------------------------------------------------
// <copyright file="ProfileTypeModule.cs" company="Starion Group S.A.">
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
    /// Routes the ProfileType endpoints of the Forge HTTP API.
    /// </summary>
    public sealed class ProfileTypeModule : ICarterModule
    {
        /// <summary>
        /// Registers the ProfileType endpoints on the supplied route builder.
        /// </summary>
        /// <param name="app">
        /// The endpoint route builder the routes are registered on.
        /// </param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("/api/v1");

            api.MapGet("/profileType", () => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListProfileType");

            api.MapPost("/profileType", () => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateProfileType");

            api.MapGet("/profileType/{identifier:guid}", (Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetProfileTypeById");

            api.MapPut("/profileType/{identifier:guid}", (Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetProfileTypeById");

            api.MapPatch("/profileType/{identifier:guid}", (Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateProfileTypeById");

            api.MapDelete("/profileType/{identifier:guid}", (Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteProfileTypeById");

            api.MapGet("/profileType/{identifier:ShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetProfileTypeByShortGuid");

            api.MapPut("/profileType/{identifier:ShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetProfileTypeByShortGuid");

            api.MapPatch("/profileType/{identifier:ShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateProfileTypeByShortGuid");

            api.MapDelete("/profileType/{identifier:ShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteProfileTypeByShortGuid");

            api.MapGet("/profileType/{identifier:EnumerableOfShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetProfileTypeByShortGuids");
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
