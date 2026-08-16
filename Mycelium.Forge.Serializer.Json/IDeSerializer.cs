// ------------------------------------------------------------------------------------------------
// <copyright file="IDeSerializer.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Serializer.Json
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using Mycelium.Forge.Common;

    /// <summary>
    /// The purpose of the <see cref="IDeSerializer"/> is to deserialize a JSON <see cref="Stream"/> to
    /// an <see cref="IThing"/> and <see cref="IEnumerable{IThing}"/>
    /// </summary>
    public interface IDeSerializer
    {
        /// <summary>
        /// Deserializes the JSON stream to an <see cref="IEnumerable{IThing}"/>
        /// </summary>
        /// <param name="stream">
        /// the JSON input stream
        /// </param>
        /// <returns>
        /// an <see cref="IEnumerable{IThing}"/>
        /// </returns>
        IEnumerable<IThing> DeSerialize(Stream stream);

        /// <summary>
        /// Asynchronously deserializes the JSON stream to an <see cref="IEnumerable{IThing}"/>
        /// </summary>
        /// <param name="stream">
        /// the JSON input stream
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to cancel the operation
        /// </param>
        /// <returns>
        /// an <see cref="IEnumerable{IThing}"/>
        /// </returns>
        Task<IEnumerable<IThing>> DeSerializeAsync(Stream stream, CancellationToken cancellationToken);
    }
}
