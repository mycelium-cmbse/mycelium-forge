// ------------------------------------------------------------------------------------------------
// <copyright file="PackageTypeModule.cs" company="Starion Group S.A.">
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
    /// Routes the PackageType endpoints of the Forge HTTP API.
    /// </summary>
    public sealed class PackageTypeModule : ICarterModule
    {
        /// <summary>
        /// Registers the PackageType endpoints on the supplied route builder.
        /// </summary>
        /// <param name="app">
        /// The endpoint route builder the routes are registered on.
        /// </param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("/api/v1");

            api.MapGet("/packageType", () => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListPackageType");

            api.MapPost("/packageType", () => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreatePackageType");

            api.MapGet("/packageType/{identifier:guid}", (Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetPackageTypeById");

            api.MapPut("/packageType/{identifier:guid}", (Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetPackageTypeById");

            api.MapPatch("/packageType/{identifier:guid}", (Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdatePackageTypeById");

            api.MapDelete("/packageType/{identifier:guid}", (Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeletePackageTypeById");

            api.MapGet("/packageType/{identifier:ShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetPackageTypeByShortGuid");

            api.MapPut("/packageType/{identifier:ShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetPackageTypeByShortGuid");

            api.MapPatch("/packageType/{identifier:ShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdatePackageTypeByShortGuid");

            api.MapDelete("/packageType/{identifier:ShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeletePackageTypeByShortGuid");

            api.MapGet("/packageType/{identifier:EnumerableOfShortGuid}", (string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetPackageTypeByShortGuids");
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
