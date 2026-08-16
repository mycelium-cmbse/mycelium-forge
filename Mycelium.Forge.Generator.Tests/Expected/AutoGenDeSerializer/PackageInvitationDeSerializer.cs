// ------------------------------------------------------------------------------------------------
// <copyright file="PackageInvitationDeSerializer.cs" company="Starion Group S.A.">
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
    /// The purpose of the <see cref="PackageInvitationDeSerializer"/> is to provide deserialization capabilities
    /// for the <see cref="IPackageInvitation"/> interface
    /// </summary>
    internal static class PackageInvitationDeSerializer
    {
        /// <summary>
        /// Deserializes an instance of <see cref="IPackageInvitation"/> from the provided <see cref="JsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="IPackageInvitation"/> json object
        /// </param>
        /// <param name="loggerFactory">
        /// The <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        /// <returns>
        /// an instance of <see cref="IPackageInvitation"/>
        /// </returns>
        internal static IPackageInvitation DeSerialize(JsonElement jsonElement, ILoggerFactory loggerFactory = null)
        {
            var logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger("PackageInvitationDeSerializer");

            if (!jsonElement.TryGetProperty("@type"u8, out var @type))
            {
                throw new InvalidOperationException("The @type property is not available, the PackageInvitationDeSerializer cannot be used to deserialize this JsonElement");
            }

            if (@type.GetString() != "PackageInvitation")
            {
                throw new InvalidOperationException($"The PackageInvitationDeSerializer can only be used to deserialize objects of type IPackageInvitation, a {@type.GetString()} was provided");
            }

            var dtoInstance = new Mycelium.Forge.Common.PackageInvitation();

            if (jsonElement.TryGetProperty("@id"u8, out var idProperty))
            {
                var propertyValue = idProperty.GetString();

                if (propertyValue == null)
                {
                    throw new JsonException("The @id property is not present, the PackageInvitation cannot be deserialized");
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
                logger.LogDebug("the createdAt Json property was not found in the PackageInvitation: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("experisAt"u8, out var experisAtProperty))
            {
                dtoInstance.ExperisAt = experisAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the experisAt Json property was not found in the PackageInvitation: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("modifiedAt"u8, out var modifiedAtProperty))
            {
                dtoInstance.ModifiedAt = modifiedAtProperty.GetDateTime();
            }
            else
            {
                logger.LogDebug("the modifiedAt Json property was not found in the PackageInvitation: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("package"u8, out var packageProperty))
            {
                if (packageProperty.ValueKind == JsonValueKind.Null)
                {
                    dtoInstance.Package = Guid.Empty;
                    logger.LogDebug($"the PackageInvitation.Package property was not found in the Json. The value is set to Guid.Empty");
                }
                else
                {
                    if (packageProperty.TryGetProperty("@id"u8, out var packageExternalIdProperty))
                    {
                        var propertyValue = packageExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.Package = Guid.Parse(propertyValue);
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the package Json property was not found in the PackageInvitation: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("packageInvitationKind"u8, out var packageInvitationKindProperty))
            {
                dtoInstance.PackageInvitationKind = PackageInvitationKindProvider.Parse(packageInvitationKindProperty.GetString());
            }
            else
            {
                logger.LogDebug("the packageInvitationKind Json property was not found in the PackageInvitation: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("status"u8, out var statusProperty))
            {
                dtoInstance.Status = InvitationStatusKindProvider.Parse(statusProperty.GetString());
            }
            else
            {
                logger.LogDebug("the status Json property was not found in the PackageInvitation: {Id}", dtoInstance.Id);
            }
            if (jsonElement.TryGetProperty("target"u8, out var targetProperty))
            {
                if (targetProperty.ValueKind == JsonValueKind.Null)
                {
                    dtoInstance.Target = Guid.Empty;
                    logger.LogDebug($"the PackageInvitation.Target property was not found in the Json. The value is set to Guid.Empty");
                }
                else
                {
                    if (targetProperty.TryGetProperty("@id"u8, out var targetExternalIdProperty))
                    {
                        var propertyValue = targetExternalIdProperty.GetString();

                        if (propertyValue != null)
                        {
                            dtoInstance.Target = Guid.Parse(propertyValue);
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("the target Json property was not found in the PackageInvitation: {Id}", dtoInstance.Id);
            }

            return dtoInstance;
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
