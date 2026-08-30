// ------------------------------------------------------------------------------------------------
// <copyright file="ProfileLinkModule.cs" company="Starion Group S.A.">
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
    /// Routes the ProfileLink endpoints of the Forge HTTP API.
    /// </summary>
    public sealed class ProfileLinkModule : ICarterModule
    {
        /// <summary>
        /// Registers the ProfileLink endpoints on the supplied route builder.
        /// </summary>
        /// <param name="app">
        /// The endpoint route builder the routes are registered on.
        /// </param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("/api/v1");

            api.MapGet("/account/{accountIdentifier:guid}/profileLink", (Guid accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListAccountIdProfileLink");

            api.MapPost("/account/{accountIdentifier:guid}/profileLink", (Guid accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateAccountIdProfileLink");

            api.MapGet("/account/{accountIdentifier:guid}/profileLink/{identifier:guid}", (Guid accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdProfileLinkById");

            api.MapPut("/account/{accountIdentifier:guid}/profileLink/{identifier:guid}", (Guid accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountIdProfileLinkById");

            api.MapPatch("/account/{accountIdentifier:guid}/profileLink/{identifier:guid}", (Guid accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountIdProfileLinkById");

            api.MapDelete("/account/{accountIdentifier:guid}/profileLink/{identifier:guid}", (Guid accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountIdProfileLinkById");

            api.MapGet("/account/{accountIdentifier:guid}/profileLink/{identifier:ShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdProfileLinkByShortGuid");

            api.MapPut("/account/{accountIdentifier:guid}/profileLink/{identifier:ShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountIdProfileLinkByShortGuid");

            api.MapPatch("/account/{accountIdentifier:guid}/profileLink/{identifier:ShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountIdProfileLinkByShortGuid");

            api.MapDelete("/account/{accountIdentifier:guid}/profileLink/{identifier:ShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountIdProfileLinkByShortGuid");

            api.MapGet("/account/{accountIdentifier:guid}/profileLink/{identifier:EnumerableOfShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdProfileLinkByShortGuids");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/profileLink", (string accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListAccountShortGuidProfileLink");

            api.MapPost("/account/{accountIdentifier:ShortGuid}/profileLink", (string accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateAccountShortGuidProfileLink");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/profileLink/{identifier:guid}", (string accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidProfileLinkById");

            api.MapPut("/account/{accountIdentifier:ShortGuid}/profileLink/{identifier:guid}", (string accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountShortGuidProfileLinkById");

            api.MapPatch("/account/{accountIdentifier:ShortGuid}/profileLink/{identifier:guid}", (string accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountShortGuidProfileLinkById");

            api.MapDelete("/account/{accountIdentifier:ShortGuid}/profileLink/{identifier:guid}", (string accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountShortGuidProfileLinkById");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/profileLink/{identifier:ShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidProfileLinkByShortGuid");

            api.MapPut("/account/{accountIdentifier:ShortGuid}/profileLink/{identifier:ShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountShortGuidProfileLinkByShortGuid");

            api.MapPatch("/account/{accountIdentifier:ShortGuid}/profileLink/{identifier:ShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountShortGuidProfileLinkByShortGuid");

            api.MapDelete("/account/{accountIdentifier:ShortGuid}/profileLink/{identifier:ShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountShortGuidProfileLinkByShortGuid");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/profileLink/{identifier:EnumerableOfShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidProfileLinkByShortGuids");

            api.MapGet("/organization/{organizationIdentifier:guid}/profileLink", (Guid organizationIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListOrganizationIdProfileLink");

            api.MapPost("/organization/{organizationIdentifier:guid}/profileLink", (Guid organizationIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateOrganizationIdProfileLink");

            api.MapGet("/organization/{organizationIdentifier:guid}/profileLink/{identifier:guid}", (Guid organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationIdProfileLinkById");

            api.MapPut("/organization/{organizationIdentifier:guid}/profileLink/{identifier:guid}", (Guid organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationIdProfileLinkById");

            api.MapPatch("/organization/{organizationIdentifier:guid}/profileLink/{identifier:guid}", (Guid organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationIdProfileLinkById");

            api.MapDelete("/organization/{organizationIdentifier:guid}/profileLink/{identifier:guid}", (Guid organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationIdProfileLinkById");

            api.MapGet("/organization/{organizationIdentifier:guid}/profileLink/{identifier:ShortGuid}", (Guid organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationIdProfileLinkByShortGuid");

            api.MapPut("/organization/{organizationIdentifier:guid}/profileLink/{identifier:ShortGuid}", (Guid organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationIdProfileLinkByShortGuid");

            api.MapPatch("/organization/{organizationIdentifier:guid}/profileLink/{identifier:ShortGuid}", (Guid organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationIdProfileLinkByShortGuid");

            api.MapDelete("/organization/{organizationIdentifier:guid}/profileLink/{identifier:ShortGuid}", (Guid organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationIdProfileLinkByShortGuid");

            api.MapGet("/organization/{organizationIdentifier:guid}/profileLink/{identifier:EnumerableOfShortGuid}", (Guid organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationIdProfileLinkByShortGuids");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/profileLink", (string organizationIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListOrganizationShortGuidProfileLink");

            api.MapPost("/organization/{organizationIdentifier:ShortGuid}/profileLink", (string organizationIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateOrganizationShortGuidProfileLink");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/profileLink/{identifier:guid}", (string organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationShortGuidProfileLinkById");

            api.MapPut("/organization/{organizationIdentifier:ShortGuid}/profileLink/{identifier:guid}", (string organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationShortGuidProfileLinkById");

            api.MapPatch("/organization/{organizationIdentifier:ShortGuid}/profileLink/{identifier:guid}", (string organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationShortGuidProfileLinkById");

            api.MapDelete("/organization/{organizationIdentifier:ShortGuid}/profileLink/{identifier:guid}", (string organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationShortGuidProfileLinkById");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/profileLink/{identifier:ShortGuid}", (string organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationShortGuidProfileLinkByShortGuid");

            api.MapPut("/organization/{organizationIdentifier:ShortGuid}/profileLink/{identifier:ShortGuid}", (string organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationShortGuidProfileLinkByShortGuid");

            api.MapPatch("/organization/{organizationIdentifier:ShortGuid}/profileLink/{identifier:ShortGuid}", (string organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationShortGuidProfileLinkByShortGuid");

            api.MapDelete("/organization/{organizationIdentifier:ShortGuid}/profileLink/{identifier:ShortGuid}", (string organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationShortGuidProfileLinkByShortGuid");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/profileLink/{identifier:EnumerableOfShortGuid}", (string organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationShortGuidProfileLinkByShortGuids");
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
