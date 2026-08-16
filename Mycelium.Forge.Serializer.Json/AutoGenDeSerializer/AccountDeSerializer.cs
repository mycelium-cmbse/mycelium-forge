// ------------------------------------------------------------------------------------------------
// <copyright file="AccountDeSerializer.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Serializer.Json
{
    using System;
    using System.Text.Json;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;

    using Mycelium.Forge.Common;

    /// <summary>
    /// The purpose of the <see cref="AccountDeSerializer"/> is to provide deserialization capabilities
    /// for the <see cref="IAccount"/> interface
    /// </summary>
    internal static class AccountDeSerializer
    {
        /// <summary>
        /// Deserializes an instance of <see cref="IAccount"/> from the provided <see cref="JsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="IAccount"/> json object
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="IAccount"/>
        /// </returns>
        internal static IAccount DeSerialize(JsonElement jsonElement, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("AccountDeSerializer");

            if (!jsonElement.TryGetProperty("@type"u8, out var @type))
            {
                throw new InvalidOperationException("The @type property is not available, the AccountDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (@type.GetString() != "Account")
            {
                throw new InvalidOperationException($"The AccountDeSerializer can only be used to deserialize objects of type IAccount, a {@type.GetString()} was provided");
            }

            var dtoInstance = new Mycelium.Forge.Common.Account();

            if (jsonElement.TryGetProperty("@id"u8, out var idProperty))
            {
                var propertyValue = idProperty.GetString();

                if (propertyValue == null)
                {
                    throw new JsonException("The @id property is not present, the Account cannot be deserialized");
                }
                else
                {
                    dtoInstance.Id = Guid.Parse(propertyValue);
                }
            }

            if (jsonElement.TryGetProperty("address"u8, out var addressProperty))
            {
                foreach (var arrayItem in addressProperty.EnumerateArray())
                {
                    if (arrayItem.TryGetProperty("@id"u8, out var addressExternalIdProperty))
                    {
                        var propertyValue = addressExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.Address.Add(Guid.Parse(propertyValue));
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the address Json property was not found in the Account: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("apiKey"u8, out var apiKeyProperty))
            {
                foreach (var arrayItem in apiKeyProperty.EnumerateArray())
                {
                    if (arrayItem.TryGetProperty("@id"u8, out var apiKeyExternalIdProperty))
                    {
                        var propertyValue = apiKeyExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.ApiKey.Add(Guid.Parse(propertyValue));
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the apiKey Json property was not found in the Account: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("avatarBlobReference"u8, out var avatarBlobReferenceProperty))
            {
                dtoInstance.AvatarBlobReference = avatarBlobReferenceProperty.GetString();
            }
            else
            {
                logger.LogDebug("the avatarBlobReference Json property was not found in the Account: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("billingEmail"u8, out var billingEmailProperty))
            {
                dtoInstance.BillingEmail = billingEmailProperty.GetString();
            }
            else
            {
                logger.LogDebug("the billingEmail Json property was not found in the Account: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("createdAt"u8, out var createdAtProperty))
            {
                dtoInstance.CreatedAt = createdAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the createdAt Json property was not found in the Account: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("defaultPackageVisibility"u8, out var defaultPackageVisibilityProperty))
            {
                dtoInstance.DefaultPackageVisibility = VisibilityKindProvider.Parse(defaultPackageVisibilityProperty.GetString());
            }
            else
            {
                logger.LogDebug("the defaultPackageVisibility Json property was not found in the Account: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("email"u8, out var emailProperty))
            {
                var propertyValue = emailProperty.GetString();

                if (propertyValue != null)
                {
                    dtoInstance.Email = propertyValue;
                }
            }
            else
            {
                logger.LogDebug("the email Json property was not found in the Account: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("modifiedAt"u8, out var modifiedAtProperty))
            {
                dtoInstance.ModifiedAt = modifiedAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the modifiedAt Json property was not found in the Account: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("name"u8, out var nameProperty))
            {
                var propertyValue = nameProperty.GetString();

                if (propertyValue != null)
                {
                    dtoInstance.Name = propertyValue;
                }
            }
            else
            {
                logger.LogDebug("the name Json property was not found in the Account: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("origin"u8, out var originProperty))
            {
                var propertyValue = originProperty.GetString();

                if (propertyValue != null)
                {
                    dtoInstance.Origin = propertyValue;
                }
            }
            else
            {
                logger.LogDebug("the origin Json property was not found in the Account: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("ownedOrganizationInvitation"u8, out var ownedOrganizationInvitationProperty))
            {
                foreach (var arrayItem in ownedOrganizationInvitationProperty.EnumerateArray())
                {
                    if (arrayItem.TryGetProperty("@id"u8, out var ownedOrganizationInvitationExternalIdProperty))
                    {
                        var propertyValue = ownedOrganizationInvitationExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.OwnedOrganizationInvitation.Add(Guid.Parse(propertyValue));
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the ownedOrganizationInvitation Json property was not found in the Account: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("ownedPackage"u8, out var ownedPackageProperty))
            {
                foreach (var arrayItem in ownedPackageProperty.EnumerateArray())
                {
                    if (arrayItem.TryGetProperty("@id"u8, out var ownedPackageExternalIdProperty))
                    {
                        var propertyValue = ownedPackageExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.OwnedPackage.Add(Guid.Parse(propertyValue));
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the ownedPackage Json property was not found in the Account: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("ownedPackageInvitation"u8, out var ownedPackageInvitationProperty))
            {
                if (ownedPackageInvitationProperty.ValueKind == JsonValueKind.Null)
                {
                    dtoInstance.OwnedPackageInvitation = Guid.Empty;
                    logger.LogDebug($"the Account.OwnedPackageInvitation property was not found in the Json. The value is set to Guid.Empty");
                }
                else
                {
                    if (ownedPackageInvitationProperty.TryGetProperty("@id"u8, out var ownedPackageInvitationExternalIdProperty))
                    {
                        var propertyValue = ownedPackageInvitationExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.OwnedPackageInvitation = Guid.Parse(propertyValue);
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the ownedPackageInvitation Json property was not found in the Account: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("primaryAddress"u8, out var primaryAddressProperty))
            {
                if (primaryAddressProperty.ValueKind == JsonValueKind.Null)
                {
                    dtoInstance.PrimaryAddress = Guid.Empty;
                    logger.LogDebug($"the Account.PrimaryAddress property was not found in the Json. The value is set to Guid.Empty");
                }
                else
                {
                    if (primaryAddressProperty.TryGetProperty("@id"u8, out var primaryAddressExternalIdProperty))
                    {
                        var propertyValue = primaryAddressExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.PrimaryAddress = Guid.Parse(propertyValue);
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the primaryAddress Json property was not found in the Account: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("profileLink"u8, out var profileLinkProperty))
            {
                foreach (var arrayItem in profileLinkProperty.EnumerateArray())
                {
                    if (arrayItem.TryGetProperty("@id"u8, out var profileLinkExternalIdProperty))
                    {
                        var propertyValue = profileLinkExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.ProfileLink.Add(Guid.Parse(propertyValue));
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the profileLink Json property was not found in the Account: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("shortName"u8, out var shortNameProperty))
            {
                var propertyValue = shortNameProperty.GetString();

                if (propertyValue != null)
                {
                    dtoInstance.ShortName = propertyValue;
                }
            }
            else
            {
                logger.LogDebug("the shortName Json property was not found in the Account: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("status"u8, out var statusProperty))
            {
                dtoInstance.Status = ScopeStatusKindProvider.Parse(statusProperty.GetString());
            }
            else
            {
                logger.LogDebug("the status Json property was not found in the Account: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("website"u8, out var websiteProperty))
            {
                dtoInstance.Website = websiteProperty.GetString();
            }
            else
            {
                logger.LogDebug("the website Json property was not found in the Account: {Id}", dtoInstance.Id);
            }

            return dtoInstance;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
