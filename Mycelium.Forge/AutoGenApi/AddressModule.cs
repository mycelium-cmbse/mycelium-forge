// ------------------------------------------------------------------------------------------------
// <copyright file="AddressModule.cs" company="Starion Group S.A.">
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
    /// Routes the Address endpoints of the Forge HTTP API.
    /// </summary>
    public sealed class AddressModule : ICarterModule
    {
        /// <summary>
        /// Registers the Address endpoints on the supplied route builder.
        /// </summary>
        /// <param name="app">
        /// The endpoint route builder the routes are registered on.
        /// </param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("/api/v1");

            api.MapGet("/account/{accountIdentifier:guid}/address", (Guid accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListAccountIdAddress");

            api.MapPost("/account/{accountIdentifier:guid}/address", (Guid accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateAccountIdAddress");

            api.MapGet("/account/{accountIdentifier:guid}/address/{identifier:guid}", (Guid accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdAddressById");

            api.MapPut("/account/{accountIdentifier:guid}/address/{identifier:guid}", (Guid accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountIdAddressById");

            api.MapPatch("/account/{accountIdentifier:guid}/address/{identifier:guid}", (Guid accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountIdAddressById");

            api.MapDelete("/account/{accountIdentifier:guid}/address/{identifier:guid}", (Guid accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountIdAddressById");

            api.MapGet("/account/{accountIdentifier:guid}/address/{identifier:ShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdAddressByShortGuid");

            api.MapPut("/account/{accountIdentifier:guid}/address/{identifier:ShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountIdAddressByShortGuid");

            api.MapPatch("/account/{accountIdentifier:guid}/address/{identifier:ShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountIdAddressByShortGuid");

            api.MapDelete("/account/{accountIdentifier:guid}/address/{identifier:ShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountIdAddressByShortGuid");

            api.MapGet("/account/{accountIdentifier:guid}/address/{identifier:EnumerableOfShortGuid}", (Guid accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountIdAddressByShortGuids");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/address", (string accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListAccountShortGuidAddress");

            api.MapPost("/account/{accountIdentifier:ShortGuid}/address", (string accountIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateAccountShortGuidAddress");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/address/{identifier:guid}", (string accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidAddressById");

            api.MapPut("/account/{accountIdentifier:ShortGuid}/address/{identifier:guid}", (string accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountShortGuidAddressById");

            api.MapPatch("/account/{accountIdentifier:ShortGuid}/address/{identifier:guid}", (string accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountShortGuidAddressById");

            api.MapDelete("/account/{accountIdentifier:ShortGuid}/address/{identifier:guid}", (string accountIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountShortGuidAddressById");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/address/{identifier:ShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidAddressByShortGuid");

            api.MapPut("/account/{accountIdentifier:ShortGuid}/address/{identifier:ShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetAccountShortGuidAddressByShortGuid");

            api.MapPatch("/account/{accountIdentifier:ShortGuid}/address/{identifier:ShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateAccountShortGuidAddressByShortGuid");

            api.MapDelete("/account/{accountIdentifier:ShortGuid}/address/{identifier:ShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteAccountShortGuidAddressByShortGuid");

            api.MapGet("/account/{accountIdentifier:ShortGuid}/address/{identifier:EnumerableOfShortGuid}", (string accountIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetAccountShortGuidAddressByShortGuids");

            api.MapGet("/organization/{organizationIdentifier:guid}/address", (Guid organizationIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListOrganizationIdAddress");

            api.MapPost("/organization/{organizationIdentifier:guid}/address", (Guid organizationIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateOrganizationIdAddress");

            api.MapGet("/organization/{organizationIdentifier:guid}/address/{identifier:guid}", (Guid organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationIdAddressById");

            api.MapPut("/organization/{organizationIdentifier:guid}/address/{identifier:guid}", (Guid organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationIdAddressById");

            api.MapPatch("/organization/{organizationIdentifier:guid}/address/{identifier:guid}", (Guid organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationIdAddressById");

            api.MapDelete("/organization/{organizationIdentifier:guid}/address/{identifier:guid}", (Guid organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationIdAddressById");

            api.MapGet("/organization/{organizationIdentifier:guid}/address/{identifier:ShortGuid}", (Guid organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationIdAddressByShortGuid");

            api.MapPut("/organization/{organizationIdentifier:guid}/address/{identifier:ShortGuid}", (Guid organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationIdAddressByShortGuid");

            api.MapPatch("/organization/{organizationIdentifier:guid}/address/{identifier:ShortGuid}", (Guid organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationIdAddressByShortGuid");

            api.MapDelete("/organization/{organizationIdentifier:guid}/address/{identifier:ShortGuid}", (Guid organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationIdAddressByShortGuid");

            api.MapGet("/organization/{organizationIdentifier:guid}/address/{identifier:EnumerableOfShortGuid}", (Guid organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationIdAddressByShortGuids");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/address", (string organizationIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("ListOrganizationShortGuidAddress");

            api.MapPost("/organization/{organizationIdentifier:ShortGuid}/address", (string organizationIdentifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("CreateOrganizationShortGuidAddress");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/address/{identifier:guid}", (string organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationShortGuidAddressById");

            api.MapPut("/organization/{organizationIdentifier:ShortGuid}/address/{identifier:guid}", (string organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationShortGuidAddressById");

            api.MapPatch("/organization/{organizationIdentifier:ShortGuid}/address/{identifier:guid}", (string organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationShortGuidAddressById");

            api.MapDelete("/organization/{organizationIdentifier:ShortGuid}/address/{identifier:guid}", (string organizationIdentifier, Guid identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationShortGuidAddressById");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/address/{identifier:ShortGuid}", (string organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationShortGuidAddressByShortGuid");

            api.MapPut("/organization/{organizationIdentifier:ShortGuid}/address/{identifier:ShortGuid}", (string organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("SetOrganizationShortGuidAddressByShortGuid");

            api.MapPatch("/organization/{organizationIdentifier:ShortGuid}/address/{identifier:ShortGuid}", (string organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("UpdateOrganizationShortGuidAddressByShortGuid");

            api.MapDelete("/organization/{organizationIdentifier:ShortGuid}/address/{identifier:ShortGuid}", (string organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("DeleteOrganizationShortGuidAddressByShortGuid");

            api.MapGet("/organization/{organizationIdentifier:ShortGuid}/address/{identifier:EnumerableOfShortGuid}", (string organizationIdentifier, string identifier) => Results.StatusCode(StatusCodes.Status501NotImplemented))
                .WithName("GetOrganizationShortGuidAddressByShortGuids");
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
