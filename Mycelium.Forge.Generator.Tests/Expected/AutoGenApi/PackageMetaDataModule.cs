// ------------------------------------------------------------------------------------------------
// <copyright file="PackageMetaDataModule.cs" company="Starion Group S.A.">
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
    /// Routes the PackageMetaData endpoints of the Forge HTTP API.
    /// </summary>
    public sealed class PackageMetaDataModule : ICarterModule
    {
        /// <summary>
        /// Registers the PackageMetaData endpoints on the supplied route builder.
        /// </summary>
        /// <param name="app">
        /// The endpoint route builder the routes are registered on.
        /// </param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("/api/v1");

            api.MapGet("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:guid}/metaData", (Guid accountIdentifier, Guid packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdPackageIdPackageVersionIdPackageMetaData");

            api.MapPut("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:guid}/metaData", (Guid accountIdentifier, Guid packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountIdPackageIdPackageVersionIdPackageMetaData");

            api.MapPatch("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:guid}/metaData", (Guid accountIdentifier, Guid packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountIdPackageIdPackageVersionIdPackageMetaData");

            api.MapDelete("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:guid}/metaData", (Guid accountIdentifier, Guid packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountIdPackageIdPackageVersionIdPackageMetaData");

            api.MapGet("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (Guid accountIdentifier, Guid packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdPackageIdPackageVersionShortGuidPackageMetaData");

            api.MapPut("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (Guid accountIdentifier, Guid packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountIdPackageIdPackageVersionShortGuidPackageMetaData");

            api.MapPatch("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (Guid accountIdentifier, Guid packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountIdPackageIdPackageVersionShortGuidPackageMetaData");

            api.MapDelete("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (Guid accountIdentifier, Guid packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountIdPackageIdPackageVersionShortGuidPackageMetaData");

            api.MapGet("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:guid}/metaData", (Guid accountIdentifier, string packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdPackageShortGuidPackageVersionIdPackageMetaData");

            api.MapPut("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:guid}/metaData", (Guid accountIdentifier, string packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountIdPackageShortGuidPackageVersionIdPackageMetaData");

            api.MapPatch("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:guid}/metaData", (Guid accountIdentifier, string packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountIdPackageShortGuidPackageVersionIdPackageMetaData");

            api.MapDelete("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:guid}/metaData", (Guid accountIdentifier, string packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountIdPackageShortGuidPackageVersionIdPackageMetaData");

            api.MapGet("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (Guid accountIdentifier, string packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdPackageShortGuidPackageVersionShortGuidPackageMetaData");

            api.MapPut("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (Guid accountIdentifier, string packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountIdPackageShortGuidPackageVersionShortGuidPackageMetaData");

            api.MapPatch("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (Guid accountIdentifier, string packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountIdPackageShortGuidPackageVersionShortGuidPackageMetaData");

            api.MapDelete("/account/{accountIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (Guid accountIdentifier, string packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountIdPackageShortGuidPackageVersionShortGuidPackageMetaData");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:guid}/metaData", (string accountIdentifier, Guid packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidPackageIdPackageVersionIdPackageMetaData");

            api.MapPut("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:guid}/metaData", (string accountIdentifier, Guid packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountShortGuidPackageIdPackageVersionIdPackageMetaData");

            api.MapPatch("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:guid}/metaData", (string accountIdentifier, Guid packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountShortGuidPackageIdPackageVersionIdPackageMetaData");

            api.MapDelete("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:guid}/metaData", (string accountIdentifier, Guid packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountShortGuidPackageIdPackageVersionIdPackageMetaData");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (string accountIdentifier, Guid packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidPackageIdPackageVersionShortGuidPackageMetaData");

            api.MapPut("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (string accountIdentifier, Guid packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountShortGuidPackageIdPackageVersionShortGuidPackageMetaData");

            api.MapPatch("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (string accountIdentifier, Guid packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountShortGuidPackageIdPackageVersionShortGuidPackageMetaData");

            api.MapDelete("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (string accountIdentifier, Guid packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountShortGuidPackageIdPackageVersionShortGuidPackageMetaData");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:guid}/metaData", (string accountIdentifier, string packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidPackageShortGuidPackageVersionIdPackageMetaData");

            api.MapPut("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:guid}/metaData", (string accountIdentifier, string packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountShortGuidPackageShortGuidPackageVersionIdPackageMetaData");

            api.MapPatch("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:guid}/metaData", (string accountIdentifier, string packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountShortGuidPackageShortGuidPackageVersionIdPackageMetaData");

            api.MapDelete("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:guid}/metaData", (string accountIdentifier, string packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountShortGuidPackageShortGuidPackageVersionIdPackageMetaData");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (string accountIdentifier, string packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidPackageShortGuidPackageVersionShortGuidPackageMetaData");

            api.MapPut("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (string accountIdentifier, string packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountShortGuidPackageShortGuidPackageVersionShortGuidPackageMetaData");

            api.MapPatch("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (string accountIdentifier, string packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountShortGuidPackageShortGuidPackageVersionShortGuidPackageMetaData");

            api.MapDelete("/account/{accountIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (string accountIdentifier, string packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountShortGuidPackageShortGuidPackageVersionShortGuidPackageMetaData");

            api.MapGet("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:guid}/metaData", (Guid organizationIdentifier, Guid packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationIdPackageIdPackageVersionIdPackageMetaData");

            api.MapPut("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:guid}/metaData", (Guid organizationIdentifier, Guid packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationIdPackageIdPackageVersionIdPackageMetaData");

            api.MapPatch("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:guid}/metaData", (Guid organizationIdentifier, Guid packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationIdPackageIdPackageVersionIdPackageMetaData");

            api.MapDelete("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:guid}/metaData", (Guid organizationIdentifier, Guid packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationIdPackageIdPackageVersionIdPackageMetaData");

            api.MapGet("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (Guid organizationIdentifier, Guid packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationIdPackageIdPackageVersionShortGuidPackageMetaData");

            api.MapPut("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (Guid organizationIdentifier, Guid packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationIdPackageIdPackageVersionShortGuidPackageMetaData");

            api.MapPatch("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (Guid organizationIdentifier, Guid packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationIdPackageIdPackageVersionShortGuidPackageMetaData");

            api.MapDelete("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (Guid organizationIdentifier, Guid packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationIdPackageIdPackageVersionShortGuidPackageMetaData");

            api.MapGet("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:guid}/metaData", (Guid organizationIdentifier, string packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationIdPackageShortGuidPackageVersionIdPackageMetaData");

            api.MapPut("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:guid}/metaData", (Guid organizationIdentifier, string packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationIdPackageShortGuidPackageVersionIdPackageMetaData");

            api.MapPatch("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:guid}/metaData", (Guid organizationIdentifier, string packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationIdPackageShortGuidPackageVersionIdPackageMetaData");

            api.MapDelete("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:guid}/metaData", (Guid organizationIdentifier, string packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationIdPackageShortGuidPackageVersionIdPackageMetaData");

            api.MapGet("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (Guid organizationIdentifier, string packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationIdPackageShortGuidPackageVersionShortGuidPackageMetaData");

            api.MapPut("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (Guid organizationIdentifier, string packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationIdPackageShortGuidPackageVersionShortGuidPackageMetaData");

            api.MapPatch("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (Guid organizationIdentifier, string packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationIdPackageShortGuidPackageVersionShortGuidPackageMetaData");

            api.MapDelete("/organization/{organizationIdentifier:guid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (Guid organizationIdentifier, string packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationIdPackageShortGuidPackageVersionShortGuidPackageMetaData");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:guid}/metaData", (string organizationIdentifier, Guid packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationShortGuidPackageIdPackageVersionIdPackageMetaData");

            api.MapPut("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:guid}/metaData", (string organizationIdentifier, Guid packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationShortGuidPackageIdPackageVersionIdPackageMetaData");

            api.MapPatch("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:guid}/metaData", (string organizationIdentifier, Guid packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationShortGuidPackageIdPackageVersionIdPackageMetaData");

            api.MapDelete("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:guid}/metaData", (string organizationIdentifier, Guid packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationShortGuidPackageIdPackageVersionIdPackageMetaData");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (string organizationIdentifier, Guid packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationShortGuidPackageIdPackageVersionShortGuidPackageMetaData");

            api.MapPut("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (string organizationIdentifier, Guid packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationShortGuidPackageIdPackageVersionShortGuidPackageMetaData");

            api.MapPatch("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (string organizationIdentifier, Guid packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationShortGuidPackageIdPackageVersionShortGuidPackageMetaData");

            api.MapDelete("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:guid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (string organizationIdentifier, Guid packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationShortGuidPackageIdPackageVersionShortGuidPackageMetaData");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:guid}/metaData", (string organizationIdentifier, string packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationShortGuidPackageShortGuidPackageVersionIdPackageMetaData");

            api.MapPut("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:guid}/metaData", (string organizationIdentifier, string packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationShortGuidPackageShortGuidPackageVersionIdPackageMetaData");

            api.MapPatch("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:guid}/metaData", (string organizationIdentifier, string packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationShortGuidPackageShortGuidPackageVersionIdPackageMetaData");

            api.MapDelete("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:guid}/metaData", (string organizationIdentifier, string packageIdentifier, Guid packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationShortGuidPackageShortGuidPackageVersionIdPackageMetaData");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (string organizationIdentifier, string packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationShortGuidPackageShortGuidPackageVersionShortGuidPackageMetaData");

            api.MapPut("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (string organizationIdentifier, string packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationShortGuidPackageShortGuidPackageVersionShortGuidPackageMetaData");

            api.MapPatch("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (string organizationIdentifier, string packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationShortGuidPackageShortGuidPackageVersionShortGuidPackageMetaData");

            api.MapDelete("/organization/{organizationIdentifier:ShortGuid}/ownedPackage/{packageIdentifier:ShortGuid}/version/{packageVersionIdentifier:ShortGuid}/metaData", (string organizationIdentifier, string packageIdentifier, string packageVersionIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationShortGuidPackageShortGuidPackageVersionShortGuidPackageMetaData");
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
