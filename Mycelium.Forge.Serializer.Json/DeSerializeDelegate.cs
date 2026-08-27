// ------------------------------------------------------------------------------------------------
// <copyright file="DeSerializeDelegate.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Serializer.Json
{
    using System.Text.Json;

    using Microsoft.Extensions.Logging;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Deserializes an <see cref="IThing"/> from a single forward-only pass over a <see cref="Utf8JsonReader"/>
    /// positioned at the <see cref="JsonTokenType.StartObject"/> token of the object to deserialize.
    /// </summary>
    /// <remarks>
    /// A named delegate is required here because <see cref="Utf8JsonReader"/> is a ref struct and can
    /// only be passed by <c>ref</c>, which <see cref="System.Func{T1,T2,TResult}"/> cannot express.
    /// </remarks>
    /// <param name="reader">
    /// The <see cref="Utf8JsonReader"/>, positioned at the object's <see cref="JsonTokenType.StartObject"/> token
    /// </param>
    /// <param name="loggerFactory">
    /// The <see cref="ILoggerFactory"/> used to setup logging
    /// </param>
    /// <returns>
    /// an instance of <see cref="IThing"/>
    /// </returns>
    internal delegate IThing DeSerializeDelegate(ref Utf8JsonReader reader, ILoggerFactory loggerFactory);
}
