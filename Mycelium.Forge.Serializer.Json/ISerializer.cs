// ------------------------------------------------------------------------------------------------
// <copyright file="ISerializer.cs" company="Starion Group S.A.">
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
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    using Mycelium.Forge.Common;

    /// <summary>
    /// The purpose of the <see cref="ISerializer"/> is to write an <see cref="IThing"/> and <see cref="IEnumerable{IThing}"/>
    /// as JSON to a <see cref="Stream"/>
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Serialize an <see cref="IEnumerable{IThing}"/> as JSON to a target <see cref="Stream"/>
        /// </summary>
        /// <param name="things">
        /// The <see cref="IEnumerable{IThing}"/> that shall be serialized
        /// </param>
        /// <param name="stream">
        /// The target <see cref="Stream"/>
        /// </param>
        /// <param name="jsonWriterOptions">
        /// The <see cref="JsonWriterOptions"/> to use
        /// </param>
        void Serialize(IEnumerable<IThing> things, Stream stream, JsonWriterOptions jsonWriterOptions);

        /// <summary>
        /// Serialize an <see cref="IThing"/> as JSON to a target <see cref="Stream"/>
        /// </summary>
        /// <param name="thing">
        /// The <see cref="IThing"/> that shall be serialized
        /// </param>
        /// <param name="stream">
        /// The target <see cref="Stream"/>
        /// </param>
        /// <param name="jsonWriterOptions">
        /// The <see cref="JsonWriterOptions"/> to use
        /// </param>
        void Serialize(IThing thing, Stream stream, JsonWriterOptions jsonWriterOptions);

        /// <summary>
        /// Asynchronously serialize an <see cref="IEnumerable{IThing}"/> as JSON to a target <see cref="Stream"/>
        /// </summary>
        /// <param name="things">
        /// The <see cref="IEnumerable{IThing}"/> that shall be serialized
        /// </param>
        /// <param name="stream">
        /// The target <see cref="Stream"/>
        /// </param>
        /// <param name="jsonWriterOptions">
        /// The <see cref="JsonWriterOptions"/> to use
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to cancel the operation
        /// </param>
        Task SerializeAsync(IEnumerable<IThing> things, Stream stream, JsonWriterOptions jsonWriterOptions, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously serialize an <see cref="IThing"/> as JSON to a target <see cref="Stream"/>
        /// </summary>
        /// <param name="thing">
        /// The <see cref="IThing"/> that shall be serialized
        /// </param>
        /// <param name="stream">
        /// The target <see cref="Stream"/>
        /// </param>
        /// <param name="jsonWriterOptions">
        /// The <see cref="JsonWriterOptions"/> to use
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to cancel the operation
        /// </param>
        Task SerializeAsync(IThing thing, Stream stream, JsonWriterOptions jsonWriterOptions, CancellationToken cancellationToken);
    }
}
