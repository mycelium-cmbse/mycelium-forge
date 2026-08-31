// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeModule.cs" company="Starion Group S.A.">
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
    /// Routes the Forge endpoints of the Forge HTTP API.
    /// </summary>
    public sealed class ForgeModule : ICarterModule
    {
        /// <summary>
        /// Registers the Forge endpoints on the supplied route builder.
        /// </summary>
        /// <param name="app">
        /// The endpoint route builder the routes are registered on.
        /// </param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("/api/v1");

            api.MapGet("/forge", () => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetForge");

            api.MapPatch("/forge", () => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateForge");
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
