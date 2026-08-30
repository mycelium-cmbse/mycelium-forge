// ------------------------------------------------------------------------------------------------
// <copyright file="PackageVersionModule.cs" company="Starion Group S.A.">
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
    /// Routes the PackageVersion endpoints of the Forge HTTP API.
    /// </summary>
    public sealed class PackageVersionModule : ICarterModule
    {
        /// <summary>
        /// Registers the PackageVersion endpoints on the supplied route builder.
        /// </summary>
        /// <param name="app">
        /// The endpoint route builder the routes are registered on.
        /// </param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("/api/v1");

            api.MapGet("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version", (Guid accountIdentifier, Guid packageIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListAccountIdPackageIdVersion");

            api.MapPost("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version", (Guid accountIdentifier, Guid packageIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateAccountIdPackageIdPackageVersion");

            api.MapGet("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:guid}", (Guid accountIdentifier, Guid packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdPackageIdPackageVersionById");

            api.MapPut("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:guid}", (Guid accountIdentifier, Guid packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountIdPackageIdPackageVersionById");

            api.MapPatch("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:guid}", (Guid accountIdentifier, Guid packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountIdPackageIdPackageVersionById");

            api.MapDelete("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:guid}", (Guid accountIdentifier, Guid packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountIdPackageIdPackageVersionById");

            api.MapGet("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:ShortGuid}", (Guid accountIdentifier, Guid packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdPackageIdPackageVersionByShortGuid");

            api.MapPut("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:ShortGuid}", (Guid accountIdentifier, Guid packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountIdPackageIdPackageVersionByShortGuid");

            api.MapPatch("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:ShortGuid}", (Guid accountIdentifier, Guid packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountIdPackageIdPackageVersionByShortGuid");

            api.MapDelete("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:ShortGuid}", (Guid accountIdentifier, Guid packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountIdPackageIdPackageVersionByShortGuid");

            api.MapGet("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:EnumerableOfShortGuid}", (Guid accountIdentifier, Guid packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdPackageIdPackageVersionByShortGuids");

            api.MapGet("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version", (Guid accountIdentifier, string packageIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListAccountIdPackageShortGuidVersion");

            api.MapPost("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version", (Guid accountIdentifier, string packageIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateAccountIdPackageShortGuidPackageVersion");

            api.MapGet("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:guid}", (Guid accountIdentifier, string packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdPackageShortGuidPackageVersionById");

            api.MapPut("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:guid}", (Guid accountIdentifier, string packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountIdPackageShortGuidPackageVersionById");

            api.MapPatch("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:guid}", (Guid accountIdentifier, string packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountIdPackageShortGuidPackageVersionById");

            api.MapDelete("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:guid}", (Guid accountIdentifier, string packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountIdPackageShortGuidPackageVersionById");

            api.MapGet("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:ShortGuid}", (Guid accountIdentifier, string packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdPackageShortGuidPackageVersionByShortGuid");

            api.MapPut("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:ShortGuid}", (Guid accountIdentifier, string packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountIdPackageShortGuidPackageVersionByShortGuid");

            api.MapPatch("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:ShortGuid}", (Guid accountIdentifier, string packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountIdPackageShortGuidPackageVersionByShortGuid");

            api.MapDelete("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:ShortGuid}", (Guid accountIdentifier, string packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountIdPackageShortGuidPackageVersionByShortGuid");

            api.MapGet("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:EnumerableOfShortGuid}", (Guid accountIdentifier, string packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdPackageShortGuidPackageVersionByShortGuids");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version", (string accountIdentifier, Guid packageIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListAccountShortGuidPackageIdVersion");

            api.MapPost("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version", (string accountIdentifier, Guid packageIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateAccountShortGuidPackageIdPackageVersion");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:guid}", (string accountIdentifier, Guid packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidPackageIdPackageVersionById");

            api.MapPut("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:guid}", (string accountIdentifier, Guid packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountShortGuidPackageIdPackageVersionById");

            api.MapPatch("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:guid}", (string accountIdentifier, Guid packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountShortGuidPackageIdPackageVersionById");

            api.MapDelete("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:guid}", (string accountIdentifier, Guid packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountShortGuidPackageIdPackageVersionById");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:ShortGuid}", (string accountIdentifier, Guid packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidPackageIdPackageVersionByShortGuid");

            api.MapPut("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:ShortGuid}", (string accountIdentifier, Guid packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountShortGuidPackageIdPackageVersionByShortGuid");

            api.MapPatch("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:ShortGuid}", (string accountIdentifier, Guid packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountShortGuidPackageIdPackageVersionByShortGuid");

            api.MapDelete("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:ShortGuid}", (string accountIdentifier, Guid packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountShortGuidPackageIdPackageVersionByShortGuid");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:EnumerableOfShortGuid}", (string accountIdentifier, Guid packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidPackageIdPackageVersionByShortGuids");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version", (string accountIdentifier, string packageIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListAccountShortGuidPackageShortGuidVersion");

            api.MapPost("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version", (string accountIdentifier, string packageIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateAccountShortGuidPackageShortGuidPackageVersion");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:guid}", (string accountIdentifier, string packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidPackageShortGuidPackageVersionById");

            api.MapPut("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:guid}", (string accountIdentifier, string packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountShortGuidPackageShortGuidPackageVersionById");

            api.MapPatch("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:guid}", (string accountIdentifier, string packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountShortGuidPackageShortGuidPackageVersionById");

            api.MapDelete("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:guid}", (string accountIdentifier, string packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountShortGuidPackageShortGuidPackageVersionById");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:ShortGuid}", (string accountIdentifier, string packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidPackageShortGuidPackageVersionByShortGuid");

            api.MapPut("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:ShortGuid}", (string accountIdentifier, string packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountShortGuidPackageShortGuidPackageVersionByShortGuid");

            api.MapPatch("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:ShortGuid}", (string accountIdentifier, string packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountShortGuidPackageShortGuidPackageVersionByShortGuid");

            api.MapDelete("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:ShortGuid}", (string accountIdentifier, string packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountShortGuidPackageShortGuidPackageVersionByShortGuid");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:EnumerableOfShortGuid}", (string accountIdentifier, string packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidPackageShortGuidPackageVersionByShortGuids");

            api.MapGet("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version", (Guid organizationIdentifier, Guid packageIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListOrganizationIdPackageIdVersion");

            api.MapPost("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version", (Guid organizationIdentifier, Guid packageIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateOrganizationIdPackageIdPackageVersion");

            api.MapGet("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:guid}", (Guid organizationIdentifier, Guid packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationIdPackageIdPackageVersionById");

            api.MapPut("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:guid}", (Guid organizationIdentifier, Guid packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationIdPackageIdPackageVersionById");

            api.MapPatch("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:guid}", (Guid organizationIdentifier, Guid packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationIdPackageIdPackageVersionById");

            api.MapDelete("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:guid}", (Guid organizationIdentifier, Guid packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationIdPackageIdPackageVersionById");

            api.MapGet("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:ShortGuid}", (Guid organizationIdentifier, Guid packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationIdPackageIdPackageVersionByShortGuid");

            api.MapPut("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:ShortGuid}", (Guid organizationIdentifier, Guid packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationIdPackageIdPackageVersionByShortGuid");

            api.MapPatch("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:ShortGuid}", (Guid organizationIdentifier, Guid packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationIdPackageIdPackageVersionByShortGuid");

            api.MapDelete("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:ShortGuid}", (Guid organizationIdentifier, Guid packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationIdPackageIdPackageVersionByShortGuid");

            api.MapGet("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:EnumerableOfShortGuid}", (Guid organizationIdentifier, Guid packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationIdPackageIdPackageVersionByShortGuids");

            api.MapGet("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version", (Guid organizationIdentifier, string packageIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListOrganizationIdPackageShortGuidVersion");

            api.MapPost("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version", (Guid organizationIdentifier, string packageIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateOrganizationIdPackageShortGuidPackageVersion");

            api.MapGet("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:guid}", (Guid organizationIdentifier, string packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationIdPackageShortGuidPackageVersionById");

            api.MapPut("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:guid}", (Guid organizationIdentifier, string packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationIdPackageShortGuidPackageVersionById");

            api.MapPatch("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:guid}", (Guid organizationIdentifier, string packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationIdPackageShortGuidPackageVersionById");

            api.MapDelete("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:guid}", (Guid organizationIdentifier, string packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationIdPackageShortGuidPackageVersionById");

            api.MapGet("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:ShortGuid}", (Guid organizationIdentifier, string packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationIdPackageShortGuidPackageVersionByShortGuid");

            api.MapPut("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:ShortGuid}", (Guid organizationIdentifier, string packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationIdPackageShortGuidPackageVersionByShortGuid");

            api.MapPatch("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:ShortGuid}", (Guid organizationIdentifier, string packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationIdPackageShortGuidPackageVersionByShortGuid");

            api.MapDelete("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:ShortGuid}", (Guid organizationIdentifier, string packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationIdPackageShortGuidPackageVersionByShortGuid");

            api.MapGet("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:EnumerableOfShortGuid}", (Guid organizationIdentifier, string packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationIdPackageShortGuidPackageVersionByShortGuids");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version", (string organizationIdentifier, Guid packageIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListOrganizationShortGuidPackageIdVersion");

            api.MapPost("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version", (string organizationIdentifier, Guid packageIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateOrganizationShortGuidPackageIdPackageVersion");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:guid}", (string organizationIdentifier, Guid packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationShortGuidPackageIdPackageVersionById");

            api.MapPut("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:guid}", (string organizationIdentifier, Guid packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationShortGuidPackageIdPackageVersionById");

            api.MapPatch("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:guid}", (string organizationIdentifier, Guid packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationShortGuidPackageIdPackageVersionById");

            api.MapDelete("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:guid}", (string organizationIdentifier, Guid packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationShortGuidPackageIdPackageVersionById");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:ShortGuid}", (string organizationIdentifier, Guid packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationShortGuidPackageIdPackageVersionByShortGuid");

            api.MapPut("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:ShortGuid}", (string organizationIdentifier, Guid packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationShortGuidPackageIdPackageVersionByShortGuid");

            api.MapPatch("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:ShortGuid}", (string organizationIdentifier, Guid packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationShortGuidPackageIdPackageVersionByShortGuid");

            api.MapDelete("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:ShortGuid}", (string organizationIdentifier, Guid packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationShortGuidPackageIdPackageVersionByShortGuid");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{identifier:EnumerableOfShortGuid}", (string organizationIdentifier, Guid packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationShortGuidPackageIdPackageVersionByShortGuids");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version", (string organizationIdentifier, string packageIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListOrganizationShortGuidPackageShortGuidVersion");

            api.MapPost("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version", (string organizationIdentifier, string packageIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateOrganizationShortGuidPackageShortGuidPackageVersion");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:guid}", (string organizationIdentifier, string packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationShortGuidPackageShortGuidPackageVersionById");

            api.MapPut("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:guid}", (string organizationIdentifier, string packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationShortGuidPackageShortGuidPackageVersionById");

            api.MapPatch("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:guid}", (string organizationIdentifier, string packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationShortGuidPackageShortGuidPackageVersionById");

            api.MapDelete("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:guid}", (string organizationIdentifier, string packageIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationShortGuidPackageShortGuidPackageVersionById");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:ShortGuid}", (string organizationIdentifier, string packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationShortGuidPackageShortGuidPackageVersionByShortGuid");

            api.MapPut("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:ShortGuid}", (string organizationIdentifier, string packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationShortGuidPackageShortGuidPackageVersionByShortGuid");

            api.MapPatch("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:ShortGuid}", (string organizationIdentifier, string packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationShortGuidPackageShortGuidPackageVersionByShortGuid");

            api.MapDelete("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:ShortGuid}", (string organizationIdentifier, string packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationShortGuidPackageShortGuidPackageVersionByShortGuid");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{identifier:EnumerableOfShortGuid}", (string organizationIdentifier, string packageIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationShortGuidPackageShortGuidPackageVersionByShortGuids");
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
