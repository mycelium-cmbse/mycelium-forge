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
        private static readonly Dictionary<string, Func<JsonElement, ILoggerFactory, IThing>> DeSerializerActionMap = new Dictionary<string, Func<JsonElement, ILoggerFactory, IThing>>
        {
            { "Account", Account.DeSerialize },
        };

        /// <summary>
        /// Provides the delegate <see cref="Func{JsonElement, ILoggerFactory, IThing}"/> for the
        /// <see cref="System.Type"/> that is to be deserialized
        /// </summary>
        /// <param name="typeName">
        /// The name of the subject <see cref="System.Type"/> that is to be serialized
        /// </param>
        /// <returns>
        /// A Delegate of <see cref="Func{JsonElement, ILoggerFactory, IThing}"/>
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown when the <see cref="System.Type"/> is not supported.
        /// </exception>
        internal static Func<JsonElement, ILoggerFactory, IThing> Provide(string typeName)
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
