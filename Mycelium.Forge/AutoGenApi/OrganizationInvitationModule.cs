// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationInvitationModule.cs" company="Starion Group S.A.">
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
    /// Routes the OrganizationInvitation endpoints of the Forge HTTP API.
    /// </summary>
    public sealed class OrganizationInvitationModule : ICarterModule
    {
        /// <summary>
        /// Registers the OrganizationInvitation endpoints on the supplied route builder.
        /// </summary>
        /// <param name="app">
        /// The endpoint route builder the routes are registered on.
        /// </param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("/api/v1");

            api.MapGet("/account/{accountIdentifier:guid}/ownedOrganizationInvitation", (Guid accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListAccountIdOwnedOrganizationInvitation");

            api.MapPost("/account/{accountIdentifier:guid}/ownedOrganizationInvitation", (Guid accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateAccountIdOrganizationInvitation");

            api.MapGet("/account/{accountIdentifier:guid}/ownedOrganizationInvitation/{identifier:guid}", (Guid accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdOrganizationInvitationById");

            api.MapPut("/account/{accountIdentifier:guid}/ownedOrganizationInvitation/{identifier:guid}", (Guid accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountIdOrganizationInvitationById");

            api.MapPatch("/account/{accountIdentifier:guid}/ownedOrganizationInvitation/{identifier:guid}", (Guid accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountIdOrganizationInvitationById");

            api.MapDelete("/account/{accountIdentifier:guid}/ownedOrganizationInvitation/{identifier:guid}", (Guid accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountIdOrganizationInvitationById");

            api.MapGet("/account/{accountIdentifier:guid}/ownedOrganizationInvitation/{identifier:ShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdOrganizationInvitationByShortGuid");

            api.MapPut("/account/{accountIdentifier:guid}/ownedOrganizationInvitation/{identifier:ShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountIdOrganizationInvitationByShortGuid");

            api.MapPatch("/account/{accountIdentifier:guid}/ownedOrganizationInvitation/{identifier:ShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountIdOrganizationInvitationByShortGuid");

            api.MapDelete("/account/{accountIdentifier:guid}/ownedOrganizationInvitation/{identifier:ShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountIdOrganizationInvitationByShortGuid");

            api.MapGet("/account/{accountIdentifier:guid}/ownedOrganizationInvitation/{identifier:EnumerableOfShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdOrganizationInvitationByShortGuids");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedOrganizationInvitation", (string accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListAccountShortGuidOwnedOrganizationInvitation");

            api.MapPost("/account/{accountIdentifier:ShortGuid}/ownedOrganizationInvitation", (string accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateAccountShortGuidOrganizationInvitation");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedOrganizationInvitation/{identifier:guid}", (string accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidOrganizationInvitationById");

            api.MapPut("/account/{accountIdentifier:ShortGuid}/ownedOrganizationInvitation/{identifier:guid}", (string accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountShortGuidOrganizationInvitationById");

            api.MapPatch("/account/{accountIdentifier:ShortGuid}/ownedOrganizationInvitation/{identifier:guid}", (string accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountShortGuidOrganizationInvitationById");

            api.MapDelete("/account/{accountIdentifier:ShortGuid}/ownedOrganizationInvitation/{identifier:guid}", (string accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountShortGuidOrganizationInvitationById");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedOrganizationInvitation/{identifier:ShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidOrganizationInvitationByShortGuid");

            api.MapPut("/account/{accountIdentifier:ShortGuid}/ownedOrganizationInvitation/{identifier:ShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountShortGuidOrganizationInvitationByShortGuid");

            api.MapPatch("/account/{accountIdentifier:ShortGuid}/ownedOrganizationInvitation/{identifier:ShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountShortGuidOrganizationInvitationByShortGuid");

            api.MapDelete("/account/{accountIdentifier:ShortGuid}/ownedOrganizationInvitation/{identifier:ShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountShortGuidOrganizationInvitationByShortGuid");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedOrganizationInvitation/{identifier:EnumerableOfShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidOrganizationInvitationByShortGuids");
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
