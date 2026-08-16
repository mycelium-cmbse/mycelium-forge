// ------------------------------------------------------------------------------------------------
// <copyright file="SerializationProvider.cs" company="Starion Group S.A.">
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
    using System.Text.Json;

    /// <summary>
    /// Delegate provider for the appropriate serialization method to serialize a <see cref="Type" />
    /// </summary>
    internal static class SerializationProvider
    {
        /// <summary>
        /// Caches the delegate <c>Action&lt;object, Utf8JsonWriter&gt;</c> for the
        /// <see cref="System.Type"/> that is to be serialized
        /// </summary>
        private static readonly Dictionary<System.Type, Action<object, Utf8JsonWriter>> SerializerActionMap = new Dictionary<System.Type, Action<object, Utf8JsonWriter>>
        {
            { typeof(Mycelium.Forge.Common.Account), AccountSerializer.Serialize },
            { typeof(Mycelium.Forge.Common.Address), AddressSerializer.Serialize },
            { typeof(Mycelium.Forge.Common.APIKey), APIKeySerializer.Serialize },
            { typeof(Mycelium.Forge.Common.Country), CountrySerializer.Serialize },
            { typeof(Mycelium.Forge.Common.Forge), ForgeSerializer.Serialize },
            { typeof(Mycelium.Forge.Common.Organization), OrganizationSerializer.Serialize },
            { typeof(Mycelium.Forge.Common.OrganizationInvitation), OrganizationInvitationSerializer.Serialize },
            { typeof(Mycelium.Forge.Common.Package), PackageSerializer.Serialize },
            { typeof(Mycelium.Forge.Common.PackageInvitation), PackageInvitationSerializer.Serialize },
            { typeof(Mycelium.Forge.Common.PackageMetaData), PackageMetaDataSerializer.Serialize },
            { typeof(Mycelium.Forge.Common.PackageType), PackageTypeSerializer.Serialize },
            { typeof(Mycelium.Forge.Common.PackageVersion), PackageVersionSerializer.Serialize },
            { typeof(Mycelium.Forge.Common.ProfileLink), ProfileLinkSerializer.Serialize },
            { typeof(Mycelium.Forge.Common.ProfileType), ProfileTypeSerializer.Serialize },
        };

        /// <summary>
        /// Provides the delegate <c>Action&lt;object, Utf8JsonWriter&gt;</c> for the
        /// <see cref="System.Type"/> that is to be serialized
        /// </summary>
        /// <param name="type">
        /// The subject <see cref="System.Type"/> that is to be serialized
        /// </param>
        /// <returns>
        /// A Delegate of <c>Action&lt;object, Utf8JsonWriter&gt;</c>
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown when the <see cref="System.Type"/> is not supported.
        /// </exception>
        internal static Action<object, Utf8JsonWriter> Provide(System.Type type)
        {
            return !SerializerActionMap.TryGetValue(type, out var action) ? throw new NotSupportedException($"The {type.Name} is not supported by the SerializationProvider.") : action;
        }

        /// <summary>
        /// Asserts whether the <paramref name="type"/> is supported by the provider
        /// </summary>
        /// <param name="type">
        /// The <see cref="System.Type"/> for which support is asserted
        /// </param>
        /// <returns></returns>
        internal static bool IsTypeSupported(System.Type type)
        {
            return SerializerActionMap.ContainsKey(type);
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
