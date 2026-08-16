// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeDeSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="ForgeDeSerializer"/> is to provide deserialization capabilities
    /// for the <see cref="IForge"/> interface
    /// </summary>
    internal static class ForgeDeSerializer
    {
        /// <summary>
        /// Deserializes an instance of <see cref="IForge"/> from the provided <see cref="JsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="IForge"/> json object
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="IForge"/>
        /// </returns>
        internal static IForge DeSerialize(JsonElement jsonElement, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("ForgeDeSerializer");

            if (!jsonElement.TryGetProperty("@type"u8, out var @type))
            {
                throw new InvalidOperationException("The @type property is not available, the ForgeDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (@type.GetString() != "Forge")
            {
                throw new InvalidOperationException($"The ForgeDeSerializer can only be used to deserialize objects of type IForge, a {@type.GetString()} was provided");
            }

            var dtoInstance = new Mycelium.Forge.Common.Forge();

            if (jsonElement.TryGetProperty("@id"u8, out var idProperty))
            {
                var propertyValue = idProperty.GetString();

                if (propertyValue == null)
                {
                    throw new JsonException("The @id property is not present, the Forge cannot be deserialized");
                }
                else
                {
                    dtoInstance.Id = Guid.Parse(propertyValue);
                }
            }

            if (jsonElement.TryGetProperty("account"u8, out var accountProperty))
            {
                foreach (var arrayItem in accountProperty.EnumerateArray())
                {
                    if (arrayItem.TryGetProperty("@id"u8, out var accountExternalIdProperty))
                    {
                        var propertyValue = accountExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.Account.Add(Guid.Parse(propertyValue));
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the account Json property was not found in the Forge: {Id}", dtoInstance.Id);
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
                logger.LogDebug("the administrator Json property was not found in the Forge: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("country"u8, out var countryProperty))
            {
                foreach (var arrayItem in countryProperty.EnumerateArray())
                {
                    if (arrayItem.TryGetProperty("@id"u8, out var countryExternalIdProperty))
                    {
                        var propertyValue = countryExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.Country.Add(Guid.Parse(propertyValue));
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the country Json property was not found in the Forge: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("createdAt"u8, out var createdAtProperty))
            {
                dtoInstance.CreatedAt = createdAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the createdAt Json property was not found in the Forge: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("description"u8, out var descriptionProperty))
            {
                var propertyValue = descriptionProperty.GetString();

                if (propertyValue != null)
                {
                    dtoInstance.Description = propertyValue;
                }
            }
            else
            {
                logger.LogDebug("the description Json property was not found in the Forge: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("modifiedAt"u8, out var modifiedAtProperty))
            {
                dtoInstance.ModifiedAt = modifiedAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the modifiedAt Json property was not found in the Forge: {Id}", dtoInstance.Id);
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
                logger.LogDebug("the name Json property was not found in the Forge: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("organization"u8, out var organizationProperty))
            {
                foreach (var arrayItem in organizationProperty.EnumerateArray())
                {
                    if (arrayItem.TryGetProperty("@id"u8, out var organizationExternalIdProperty))
                    {
                        var propertyValue = organizationExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.Organization.Add(Guid.Parse(propertyValue));
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the organization Json property was not found in the Forge: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("packageType"u8, out var packageTypeProperty))
            {
                foreach (var arrayItem in packageTypeProperty.EnumerateArray())
                {
                    if (arrayItem.TryGetProperty("@id"u8, out var packageTypeExternalIdProperty))
                    {
                        var propertyValue = packageTypeExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.PackageType.Add(Guid.Parse(propertyValue));
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the packageType Json property was not found in the Forge: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("profileType"u8, out var profileTypeProperty))
            {
                foreach (var arrayItem in profileTypeProperty.EnumerateArray())
                {
                    if (arrayItem.TryGetProperty("@id"u8, out var profileTypeExternalIdProperty))
                    {
                        var propertyValue = profileTypeExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.ProfileType.Add(Guid.Parse(propertyValue));
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the profileType Json property was not found in the Forge: {Id}", dtoInstance.Id);
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
                logger.LogDebug("the shortName Json property was not found in the Forge: {Id}", dtoInstance.Id);
            }

            return dtoInstance;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
