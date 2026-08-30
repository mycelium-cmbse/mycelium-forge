// ------------------------------------------------------------------------------------------------
// <copyright file="PackageModule.cs" company="Starion Group S.A.">
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
    /// Routes the Package endpoints of the Forge HTTP API.
    /// </summary>
    public sealed class PackageModule : ICarterModule
    {
        /// <summary>
        /// Registers the Package endpoints on the supplied route builder.
        /// </summary>
        /// <param name="app">
        /// The endpoint route builder the routes are registered on.
        /// </param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("/api/v1");

            api.MapGet("/account/{accountIdentifier:guid}/ownedPackage", (Guid accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListAccountIdOwnedPackage");

            api.MapPost("/account/{accountIdentifier:guid}/ownedPackage", (Guid accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateAccountIdPackage");

            api.MapGet("/account/{accountIdentifier:guid}/ownedPackage/{identifier:guid}", (Guid accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdPackageById");

            api.MapPut("/account/{accountIdentifier:guid}/ownedPackage/{identifier:guid}", (Guid accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountIdPackageById");

            api.MapPatch("/account/{accountIdentifier:guid}/ownedPackage/{identifier:guid}", (Guid accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountIdPackageById");

            api.MapDelete("/account/{accountIdentifier:guid}/ownedPackage/{identifier:guid}", (Guid accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountIdPackageById");

            api.MapGet("/account/{accountIdentifier:guid}/ownedPackage/{identifier:ShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdPackageByShortGuid");

            api.MapPut("/account/{accountIdentifier:guid}/ownedPackage/{identifier:ShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountIdPackageByShortGuid");

            api.MapPatch("/account/{accountIdentifier:guid}/ownedPackage/{identifier:ShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountIdPackageByShortGuid");

            api.MapDelete("/account/{accountIdentifier:guid}/ownedPackage/{identifier:ShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountIdPackageByShortGuid");

            api.MapGet("/account/{accountIdentifier:guid}/ownedPackage/{identifier:EnumerableOfShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdPackageByShortGuids");

            api.MapGet("/account/{accountIdentifier:guid}/ownedPackage/{shortName}", (Guid accountIdentifier, string shortName) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdPackageByShortName");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedPackage", (string accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListAccountShortGuidOwnedPackage");

            api.MapPost("/account/{accountIdentifier:ShortGuid}/ownedPackage", (string accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateAccountShortGuidPackage");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedPackage/{identifier:guid}", (string accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidPackageById");

            api.MapPut("/account/{accountIdentifier:ShortGuid}/ownedPackage/{identifier:guid}", (string accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountShortGuidPackageById");

            api.MapPatch("/account/{accountIdentifier:ShortGuid}/ownedPackage/{identifier:guid}", (string accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountShortGuidPackageById");

            api.MapDelete("/account/{accountIdentifier:ShortGuid}/ownedPackage/{identifier:guid}", (string accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountShortGuidPackageById");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedPackage/{identifier:ShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidPackageByShortGuid");

            api.MapPut("/account/{accountIdentifier:ShortGuid}/ownedPackage/{identifier:ShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountShortGuidPackageByShortGuid");

            api.MapPatch("/account/{accountIdentifier:ShortGuid}/ownedPackage/{identifier:ShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountShortGuidPackageByShortGuid");

            api.MapDelete("/account/{accountIdentifier:ShortGuid}/ownedPackage/{identifier:ShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountShortGuidPackageByShortGuid");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedPackage/{identifier:EnumerableOfShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidPackageByShortGuids");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedPackage/{shortName}", (string accountIdentifier, string shortName) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidPackageByShortName");

            api.MapGet("/organization/{organizationIdentifier:guid}/ownedPackage", (Guid organizationIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListOrganizationIdOwnedPackage");

            api.MapPost("/organization/{organizationIdentifier:guid}/ownedPackage", (Guid organizationIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateOrganizationIdPackage");

            api.MapGet("/organization/{organizationIdentifier:guid}/ownedPackage/{identifier:guid}", (Guid organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationIdPackageById");

            api.MapPut("/organization/{organizationIdentifier:guid}/ownedPackage/{identifier:guid}", (Guid organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationIdPackageById");

            api.MapPatch("/organization/{organizationIdentifier:guid}/ownedPackage/{identifier:guid}", (Guid organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationIdPackageById");

            api.MapDelete("/organization/{organizationIdentifier:guid}/ownedPackage/{identifier:guid}", (Guid organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationIdPackageById");

            api.MapGet("/organization/{organizationIdentifier:guid}/ownedPackage/{identifier:ShortGuid}", (Guid organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationIdPackageByShortGuid");

            api.MapPut("/organization/{organizationIdentifier:guid}/ownedPackage/{identifier:ShortGuid}", (Guid organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationIdPackageByShortGuid");

            api.MapPatch("/organization/{organizationIdentifier:guid}/ownedPackage/{identifier:ShortGuid}", (Guid organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationIdPackageByShortGuid");

            api.MapDelete("/organization/{organizationIdentifier:guid}/ownedPackage/{identifier:ShortGuid}", (Guid organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationIdPackageByShortGuid");

            api.MapGet("/organization/{organizationIdentifier:guid}/ownedPackage/{identifier:EnumerableOfShortGuid}", (Guid organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationIdPackageByShortGuids");

            api.MapGet("/organization/{organizationIdentifier:guid}/ownedPackage/{shortName}", (Guid organizationIdentifier, string shortName) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationIdPackageByShortName");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/ownedPackage", (string organizationIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListOrganizationShortGuidOwnedPackage");

            api.MapPost("/organization/{organizationIdentifier:ShortGuid}/ownedPackage", (string organizationIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateOrganizationShortGuidPackage");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{identifier:guid}", (string organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationShortGuidPackageById");

            api.MapPut("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{identifier:guid}", (string organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationShortGuidPackageById");

            api.MapPatch("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{identifier:guid}", (string organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationShortGuidPackageById");

            api.MapDelete("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{identifier:guid}", (string organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationShortGuidPackageById");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{identifier:ShortGuid}", (string organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationShortGuidPackageByShortGuid");

            api.MapPut("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{identifier:ShortGuid}", (string organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationShortGuidPackageByShortGuid");

            api.MapPatch("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{identifier:ShortGuid}", (string organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationShortGuidPackageByShortGuid");

            api.MapDelete("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{identifier:ShortGuid}", (string organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationShortGuidPackageByShortGuid");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{identifier:EnumerableOfShortGuid}", (string organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationShortGuidPackageByShortGuids");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{shortName}", (string organizationIdentifier, string shortName) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationShortGuidPackageByShortName");
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
