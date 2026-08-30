// ------------------------------------------------------------------------------------------------
// <copyright file="PackageInvitationModule.cs" company="Starion Group S.A.">
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
    /// Routes the PackageInvitation endpoints of the Forge HTTP API.
    /// </summary>
    public sealed class PackageInvitationModule : ICarterModule
    {
        /// <summary>
        /// Registers the PackageInvitation endpoints on the supplied route builder.
        /// </summary>
        /// <param name="app">
        /// The endpoint route builder the routes are registered on.
        /// </param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("/api/v1");

            api.MapGet("/account/{accountIdentifier:guid}/ownedPackageInvitation", (Guid accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdPackageInvitation");

            api.MapPut("/account/{accountIdentifier:guid}/ownedPackageInvitation", (Guid accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountIdPackageInvitation");

            api.MapPatch("/account/{accountIdentifier:guid}/ownedPackageInvitation", (Guid accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountIdPackageInvitation");

            api.MapDelete("/account/{accountIdentifier:guid}/ownedPackageInvitation", (Guid accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountIdPackageInvitation");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedPackageInvitation", (string accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidPackageInvitation");

            api.MapPut("/account/{accountIdentifier:ShortGuid}/ownedPackageInvitation", (string accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountShortGuidPackageInvitation");

            api.MapPatch("/account/{accountIdentifier:ShortGuid}/ownedPackageInvitation", (string accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountShortGuidPackageInvitation");

            api.MapDelete("/account/{accountIdentifier:ShortGuid}/ownedPackageInvitation", (string accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountShortGuidPackageInvitation");
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
