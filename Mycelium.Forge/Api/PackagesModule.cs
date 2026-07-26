// ------------------------------------------------------------------------------------------------
// <copyright file="PackagesModule.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Api
{
    using Carter;

    /// <summary>
    /// Routes the package-oriented endpoints of the Forge HTTP API.
    /// </summary>
    /// <remarks>
    /// This module is a placeholder that pins the endpoint shapes established by SSS 5.2.3.1. The
    /// request and response contracts, the persistence model, and the authorisation rules are
    /// settled in docs/design.md before these handlers are implemented.
    /// </remarks>
    public sealed class PackagesModule : ICarterModule
    {
        /// <summary>
        /// Registers the package endpoints on the supplied route builder.
        /// </summary>
        /// <param name="app">
        /// The endpoint route builder the routes are registered on.
        /// </param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var packages = app.MapGroup("/api/v1/packages");

            // SSS-FG-REG-Q7G: free-text search with pagination, sort and filters.
            packages.MapGet("/", () => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SearchPackages");

            // SSS-FG-REG-M8H: manifest, version list, dependency graph and release notes.
            packages.MapGet("/{scope}/{name}", (string scope, string name) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetPackageMetadata");

            // SSS-FG-REG-D6F: kpar content of the latest listed, non-prerelease version.
            packages.MapGet("/{scope}/{name}/kpar", (string scope, string name) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DownloadLatestKpar");

            // SSS-FG-REG-D6F: kpar content of an explicit {package identifier, version} pair.
            packages.MapGet("/{scope}/{name}/{version}/kpar", (string scope, string name, string version) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DownloadKpar");

            // SSS-FG-REG-A5E: atomic publish of a kpar, authenticated by a scoped API key.
            packages.MapPut("/", () => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("PublishKpar");

            // SSS-FG-REG-U4D: unlist a version without deleting its content.
            packages.MapPost("/{scope}/{name}/{version}/unlist", (string scope, string name, string version) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UnlistVersion");
        }
    }
}
