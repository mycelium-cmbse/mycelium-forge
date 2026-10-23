// ------------------------------------------------------------------------------------------------
// <copyright file="EcssHeaderTypeInfoResolver.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Resolvers
{
    using System.Text.Json;
    using System.Text.Json.Serialization.Metadata;

    using CDP4JsonFileDal.Json;

    /// <summary>
    /// Configures JSON type information modifiers for ECSS exchange header data types during deserialization.
    /// </summary>
    public sealed class EcssHeaderTypeInfoResolver : DefaultJsonTypeInfoResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EcssHeaderTypeInfoResolver" /> class.
        /// </summary>
        /// <remarks>
        /// <see cref="ExchangeDateTime" /> does not define a parameterless constructor and lacks a
        /// <see cref="System.Text.Json.Serialization.JsonConstructorAttribute" />. Without an explicit object creation
        /// delegate assigned to <see cref="JsonTypeInfo.CreateObject" />, <see cref="JsonSerializer" /> fails to deserialize
        /// properties of type <see cref="ExchangeDateTime" />. This resolver configures the default instantiation.
        /// </remarks>
        public EcssHeaderTypeInfoResolver()
        {
            this.Modifiers.Add(static typeInfo =>
            {
                if (typeInfo.Type == typeof(ExchangeDateTime))
                {
                    typeInfo.CreateObject = static () => new ExchangeDateTime(default);
                }
            });
        }
    }
}
