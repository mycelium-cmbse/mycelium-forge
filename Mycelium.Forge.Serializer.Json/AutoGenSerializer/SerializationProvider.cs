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

    using Mycelium.Forge.Common;

    /// <summary>
    /// Delegate provider for the appropriate serialization method to serialize a <see cref="Type" />
    /// </summary>
    internal static class SerializationProvider
    {
        /// <summary>
        /// Caches the delegate <c>Action&lt;object, Utf8JsonWriter&gt;</c> for the
        /// <see cref="System.Type" /> that is to be serialized
        /// </summary>
        private static readonly Dictionary<Type, Action<object, Utf8JsonWriter>> SerializerActionMap = new()
        {
            { typeof(Account), AccountSerializer.Serialize },
            { typeof(Address), AddressSerializer.Serialize },
            { typeof(APIKey), APIKeySerializer.Serialize },
            { typeof(Country), CountrySerializer.Serialize },
            { typeof(Forge), ForgeSerializer.Serialize },
            { typeof(Organization), OrganizationSerializer.Serialize },
            { typeof(OrganizationInvitation), OrganizationInvitationSerializer.Serialize },
            { typeof(Package), PackageSerializer.Serialize },
            { typeof(PackageInvitation), PackageInvitationSerializer.Serialize },
            { typeof(PackageMetaData), PackageMetaDataSerializer.Serialize },
            { typeof(PackageType), PackageTypeSerializer.Serialize },
            { typeof(PackageVersion), PackageVersionSerializer.Serialize },
            { typeof(ProfileLink), ProfileLinkSerializer.Serialize },
            { typeof(ProfileType), ProfileTypeSerializer.Serialize }
        };

        /// <summary>
        /// Caches the delegate <c>Action&lt;object, Utf8JsonWriter&gt;</c> for value-property-only
        /// serialization of the <see cref="System.Type" />
        /// </summary>
        private static readonly Dictionary<Type, Action<object, Utf8JsonWriter>> ValuePropertiesSerializerActionMap = new()
        {
            { typeof(Account), AccountSerializer.SerializeValueProperties },
            { typeof(Address), AddressSerializer.SerializeValueProperties },
            { typeof(APIKey), APIKeySerializer.SerializeValueProperties },
            { typeof(Country), CountrySerializer.SerializeValueProperties },
            { typeof(Forge), ForgeSerializer.SerializeValueProperties },
            { typeof(Organization), OrganizationSerializer.SerializeValueProperties },
            { typeof(OrganizationInvitation), OrganizationInvitationSerializer.SerializeValueProperties },
            { typeof(Package), PackageSerializer.SerializeValueProperties },
            { typeof(PackageInvitation), PackageInvitationSerializer.SerializeValueProperties },
            { typeof(PackageMetaData), PackageMetaDataSerializer.SerializeValueProperties },
            { typeof(PackageType), PackageTypeSerializer.SerializeValueProperties },
            { typeof(PackageVersion), PackageVersionSerializer.SerializeValueProperties },
            { typeof(ProfileLink), ProfileLinkSerializer.SerializeValueProperties },
            { typeof(ProfileType), ProfileTypeSerializer.SerializeValueProperties }
        };

        /// <summary>
        /// Provides the delegate <c>Action&lt;object, Utf8JsonWriter&gt;</c> for the
        /// <see cref="System.Type" /> that is to be serialized
        /// </summary>
        /// <param name="type">
        /// The subject <see cref="System.Type" /> that is to be serialized
        /// </param>
        /// <returns>
        /// A Delegate of <c>Action&lt;object, Utf8JsonWriter&gt;</c>
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown when the <see cref="System.Type" /> is not supported.
        /// </exception>
        internal static Action<object, Utf8JsonWriter> Provide(Type type)
        {
            return !SerializerActionMap.TryGetValue(type, out var action) ? throw new NotSupportedException($"The {type.Name} is not supported by the SerializationProvider.") : action;
        }

        /// <summary>
        /// Provides the delegate <c>Action&lt;object, Utf8JsonWriter&gt;</c> for value-property-only
        /// serialization of the <see cref="System.Type" />
        /// </summary>
        /// <param name="type">
        /// The subject <see cref="System.Type" /> that is to be serialized
        /// </param>
        /// <returns>
        /// A Delegate of <c>Action&lt;object, Utf8JsonWriter&gt;</c>
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown when the <see cref="System.Type" /> is not supported.
        /// </exception>
        internal static Action<object, Utf8JsonWriter> ProvideValuePropertiesSerializer(Type type)
        {
            return !ValuePropertiesSerializerActionMap.TryGetValue(type, out var action) ? throw new NotSupportedException($"The {type.Name} is not supported by the SerializationProvider.") : action;
        }

        /// <summary>
        /// Asserts whether the <paramref name="type" /> is supported by the provider
        /// </summary>
        /// <param name="type">
        /// The <see cref="System.Type" /> for which support is asserted
        /// </param>
        /// <returns></returns>
        internal static bool IsTypeSupported(Type type)
        {
            return SerializerActionMap.ContainsKey(type);
        }
    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------