// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationDeSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="OrganizationDeSerializer"/> is to provide deserialization capabilities
    /// for the <see cref="IOrganization"/> interface
    /// </summary>
    internal static class OrganizationDeSerializer
    {
        /// <summary>
        /// Deserializes an instance of <see cref="IOrganization"/> from the provided <see cref="JsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="IOrganization"/> json object
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="IOrganization"/>
        /// </returns>
        internal static IOrganization DeSerialize(JsonElement jsonElement, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("OrganizationDeSerializer");

            if (!jsonElement.TryGetProperty("@type"u8, out var @type))
            {
                throw new InvalidOperationException("The @type property is not available, the OrganizationDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (@type.GetString() != "Organization")
            {
                throw new InvalidOperationException($"The OrganizationDeSerializer can only be used to deserialize objects of type IOrganization, a {@type.GetString()} was provided");
            }

            var dtoInstance = new Mycelium.Forge.Common.Organization();

            if (jsonElement.TryGetProperty("@id"u8, out var idProperty))
            {
                var propertyValue = idProperty.GetString();

                if (propertyValue == null)
                {
                    throw new JsonException("The @id property is not present, the Organization cannot be deserialized");
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
                logger.LogDebug("the address Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("administrator"u8, out var administratorProperty))
            {
                foreach (var arrayItem in administratorProperty.EnumerateArray())
                {
                    if (arrayItem.TryGetProperty("@id"u8, out var administratorExternalIdProperty))
                    {
                        var propertyValue = administratorExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.Administrator.Add(Guid.Parse(propertyValue));
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the administrator Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("billingEmail"u8, out var billingEmailProperty))
            {
                dtoInstance.BillingEmail = billingEmailProperty.GetString();
            }
            else
            {
                logger.LogDebug("the billingEmail Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("createdAt"u8, out var createdAtProperty))
            {
                dtoInstance.CreatedAt = createdAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the createdAt Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("defaultPackageVisibility"u8, out var defaultPackageVisibilityProperty))
            {
                dtoInstance.DefaultPackageVisibility = VisibilityKindProvider.Parse(defaultPackageVisibilityProperty.GetString());
            }
            else
            {
                logger.LogDebug("the defaultPackageVisibility Json property was not found in the Organization: {Id}", dtoInstance.Id);
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
                logger.LogDebug("the email Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("logoBlobReference"u8, out var LogoBlobReferenceProperty))
            {
                dtoInstance.LogoBlobReference = LogoBlobReferenceProperty.GetString();
            }
            else
            {
                logger.LogDebug("the logoBlobReference Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("member"u8, out var memberProperty))
            {
                foreach (var arrayItem in memberProperty.EnumerateArray())
                {
                    if (arrayItem.TryGetProperty("@id"u8, out var memberExternalIdProperty))
                    {
                        var propertyValue = memberExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.Member.Add(Guid.Parse(propertyValue));
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the member Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("modifiedAt"u8, out var modifiedAtProperty))
            {
                dtoInstance.ModifiedAt = modifiedAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the modifiedAt Json property was not found in the Organization: {Id}", dtoInstance.Id);
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
                logger.LogDebug("the name Json property was not found in the Organization: {Id}", dtoInstance.Id);
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
                logger.LogDebug("the origin Json property was not found in the Organization: {Id}", dtoInstance.Id);
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
                logger.LogDebug("the ownedPackage Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("primaryAddress"u8, out var primaryAddressProperty))
            {
                if (primaryAddressProperty.ValueKind == JsonValueKind.Null)
                {
                    dtoInstance.PrimaryAddress = Guid.Empty;
                    logger.LogDebug($"the Organization.PrimaryAddress property was not found in the Json. The value is set to Guid.Empty");
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
                logger.LogDebug("the primaryAddress Json property was not found in the Organization: {Id}", dtoInstance.Id);
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
                logger.LogDebug("the profileLink Json property was not found in the Organization: {Id}", dtoInstance.Id);
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
                logger.LogDebug("the shortName Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("status"u8, out var statusProperty))
            {
                dtoInstance.Status = ScopeStatusKindProvider.Parse(statusProperty.GetString());
            }
            else
            {
                logger.LogDebug("the status Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("website"u8, out var websiteProperty))
            {
                dtoInstance.Website = websiteProperty.GetString();
            }
            else
            {
                logger.LogDebug("the website Json property was not found in the Organization: {Id}", dtoInstance.Id);
            }

            return dtoInstance;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
