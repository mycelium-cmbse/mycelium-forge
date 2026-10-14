// ------------------------------------------------------------------------------------------------
// <copyright file="UploadModule.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Api
{
    using Carter;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Services;

    /// <summary>
    /// Configures the package artifact binary upload endpoint of the Forge HTTP API.
    /// </summary>
    public sealed class UploadModule : ICarterModule
    {
        /// <summary>
        /// Registers the upload endpoints on the supplied route builder.
        /// </summary>
        /// <param name="app">The endpoint route builder the routes are registered on.</param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(ApiRoutes.PackageUpload, async (HttpRequest request, IFileUploadStagingService stagingService, CancellationToken token) =>
                {
                    var uploadToken = await stagingService.StageAsync(request.Body, token);
                    return Results.Ok(new { uploadToken });
                })
                .WithName("UploadPackage")
                .RequireAuthorization();
        }
    }
}
