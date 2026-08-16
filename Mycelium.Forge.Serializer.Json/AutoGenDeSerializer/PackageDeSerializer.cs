// ------------------------------------------------------------------------------------------------
// <copyright file="PackageDeSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="PackageDeSerializer"/> is to provide deserialization capabilities
    /// for the <see cref="IPackage"/> interface
    /// </summary>
    internal static class PackageDeSerializer
    {
        /// <summary>
        /// Deserializes an instance of <see cref="IPackage"/> from the provided <see cref="JsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="IPackage"/> json object
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="IPackage"/>
        /// </returns>
        internal static IPackage DeSerialize(JsonElement jsonElement, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("PackageDeSerializer");

            if (!jsonElement.TryGetProperty("@type"u8, out var @type))
            {
                throw new InvalidOperationException("The @type property is not available, the PackageDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (@type.GetString() != "Package")
            {
                throw new InvalidOperationException($"The PackageDeSerializer can only be used to deserialize objects of type IPackage, a {@type.GetString()} was provided");
            }

            var dtoInstance = new Mycelium.Forge.Common.Package();

            if (jsonElement.TryGetProperty("@id"u8, out var idProperty))
            {
                var propertyValue = idProperty.GetString();

                if (propertyValue == null)
                {
                    throw new JsonException("The @id property is not present, the Package cannot be deserialized");
                }
                else
                {
                    dtoInstance.Id = Guid.Parse(propertyValue);
                }
            }

            if (jsonElement.TryGetProperty("createdAt"u8, out var createdAtProperty))
            {
                dtoInstance.CreatedAt = createdAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the createdAt Json property was not found in the Package: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("listed"u8, out var listedProperty))
            {
                if (listedProperty.ValueKind != JsonValueKind.Null)
                {
                    dtoInstance.Listed = listedProperty.GetBoolean();
                }
            }
            else
            {
                logger.LogDebug("the listed Json property was not found in the Package: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("modifiedAt"u8, out var modifiedAtProperty))
            {
                dtoInstance.ModifiedAt = modifiedAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the modifiedAt Json property was not found in the Package: {Id}", dtoInstance.Id);
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
                logger.LogDebug("the name Json property was not found in the Package: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("packageMaintainer"u8, out var packageMaintainerProperty))
            {
                foreach (var arrayItem in packageMaintainerProperty.EnumerateArray())
                {
                    if (arrayItem.TryGetProperty("@id"u8, out var packageMaintainerExternalIdProperty))
                    {
                        var propertyValue = packageMaintainerExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.PackageMaintainer.Add(Guid.Parse(propertyValue));
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the packageMaintainer Json property was not found in the Package: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("packageOwner"u8, out var packageOwnerProperty))
            {
                foreach (var arrayItem in packageOwnerProperty.EnumerateArray())
                {
                    if (arrayItem.TryGetProperty("@id"u8, out var packageOwnerExternalIdProperty))
                    {
                        var propertyValue = packageOwnerExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.PackageOwner.Add(Guid.Parse(propertyValue));
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the packageOwner Json property was not found in the Package: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("packageType"u8, out var packageTypeProperty))
            {
                if (packageTypeProperty.ValueKind == JsonValueKind.Null)
                {
                    dtoInstance.PackageType = Guid.Empty;
                    logger.LogDebug($"the Package.PackageType property was not found in the Json. The value is set to Guid.Empty");
                }
                else
                {
                    if (packageTypeProperty.TryGetProperty("@id"u8, out var packageTypeExternalIdProperty))
                    {
                        var propertyValue = packageTypeExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.PackageType = Guid.Parse(propertyValue);
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the packageType Json property was not found in the Package: {Id}", dtoInstance.Id);
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
                logger.LogDebug("the shortName Json property was not found in the Package: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("version"u8, out var versionProperty))
            {
                foreach (var arrayItem in versionProperty.EnumerateArray())
                {
                    if (arrayItem.TryGetProperty("@id"u8, out var versionExternalIdProperty))
                    {
                        var propertyValue = versionExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.Version.Add(Guid.Parse(propertyValue));
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the version Json property was not found in the Package: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("visibility"u8, out var visibilityProperty))
            {
                dtoInstance.Visibility = VisibilityKindProvider.Parse(visibilityProperty.GetString());
            }
            else
            {
                logger.LogDebug("the visibility Json property was not found in the Package: {Id}", dtoInstance.Id);
            }

            return dtoInstance;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
