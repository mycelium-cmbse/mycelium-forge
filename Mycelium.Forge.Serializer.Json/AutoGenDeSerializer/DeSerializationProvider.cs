// ------------------------------------------------------------------------------------------------
// <copyright file="DeSerializationProvider.cs" company="Starion Group S.A.">
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
    using System.Collections.Generic;

    using Microsoft.Extensions.Logging;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Delegate provider for the appropriate deserialization method to deserialize a <see cref="System.Type"/>
    /// </summary>
    internal static class DeSerializationProvider
    {
        /// <summary>
        /// a dictionary that provides delegates for deserialization
        /// </summary>
        private static readonly Dictionary<string, DeSerializeDelegate> DeSerializerActionMap = new Dictionary<string, DeSerializeDelegate>
        {
            { "Account", AccountDeSerializer.DeSerialize },
            { "Address", AddressDeSerializer.DeSerialize },
            { "APIKey", APIKeyDeSerializer.DeSerialize },
            { "Country", CountryDeSerializer.DeSerialize },
            { "Forge", ForgeDeSerializer.DeSerialize },
            { "Organization", OrganizationDeSerializer.DeSerialize },
            { "OrganizationInvitation", OrganizationInvitationDeSerializer.DeSerialize },
            { "Package", PackageDeSerializer.DeSerialize },
            { "PackageInvitation", PackageInvitationDeSerializer.DeSerialize },
            { "PackageMetaData", PackageMetaDataDeSerializer.DeSerialize },
            { "PackageType", PackageTypeDeSerializer.DeSerialize },
            { "PackageVersion", PackageVersionDeSerializer.DeSerialize },
            { "ProfileLink", ProfileLinkDeSerializer.DeSerialize },
            { "ProfileType", ProfileTypeDeSerializer.DeSerialize },
        };

        /// <summary>
        /// Provides the <see cref="DeSerializeDelegate"/> for the
        /// <see cref="System.Type"/> that is to be deserialized
        /// </summary>
        /// <param name="typeName">
        /// The name of the subject <see cref="System.Type"/> that is to be serialized
        /// </param>
        /// <returns>
        /// A <see cref="DeSerializeDelegate"/>
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown when the <see cref="System.Type"/> is not supported.
        /// </exception>
        internal static DeSerializeDelegate Provide(string typeName)
        {
            if (!DeSerializerActionMap.TryGetValue(typeName, out var func))
            {
                throw new NotSupportedException($"The {typeName} is not supported by the DeSerializationProvider.");
            }

            return func;
        }

        /// <summary>
        /// Asserts whether the <paramref name="typeName"/> is supported by the provider
        /// </summary>
        /// <param name="typeName">
        /// The name of the subject <see cref="System.Type"/> for which support is asserted
        /// </param>
        /// <returns></returns>
        internal static bool IsTypeSupported(string typeName)
        {
            return DeSerializerActionMap.ContainsKey(typeName);
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
