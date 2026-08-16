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
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    
    using Mycelium.Forge.Common;

    /// <summary>
    /// The purpose of the <see cref="Serializer"/> is to write an <see cref="IThing"/> and <see cref="IEnumerable{IThing}"/>
    /// as JSON to a <see cref="Stream"/>
    /// </summary>
    public class Serializer : ISerializer
    {
        /// <summary>
        /// Serialize an <see cref="IEnumerable{IThing}"/> as JSON to a target <see cref="Stream"/>
        /// </summary>
        /// <param name="things">
        /// The <see cref="IEnumerable{IElIThingement}"/> that shall be serialized
        /// </param>
        /// <param name="stream">
        /// The target <see cref="Stream"/>
        /// </param>
        /// <param name="jsonWriterOptions">
        /// The <see cref="JsonWriterOptions"/> to use
        /// </param>
        public void Serialize(IEnumerable<IThing> things, Stream stream, JsonWriterOptions jsonWriterOptions)
        {
            using var writer = new Utf8JsonWriter(stream, jsonWriterOptions);

            writer.WriteStartArray();

            foreach (var thing in things)
            {
                var serializationAction = SerializationProvider.Provide(thing.GetType());
                serializationAction(thing, writer);
                writer.Flush();
            }

            writer.WriteEndArray();

            writer.Flush();
        }

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
        public void Serialize(IThing thing, Stream stream, JsonWriterOptions jsonWriterOptions)
        {
            using var writer = new Utf8JsonWriter(stream, jsonWriterOptions);

            var serializationAction = SerializationProvider.Provide(thing.GetType());
            serializationAction(thing, writer);
            writer.Flush();
            
        }

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
        public async Task SerializeAsync(IEnumerable<IThing> things, Stream stream, JsonWriterOptions jsonWriterOptions, CancellationToken cancellationToken)
        {
            await using var writer = new Utf8JsonWriter(stream, jsonWriterOptions);

            writer.WriteStartArray();

            foreach (var element in things)
            {
                var serializationAction = SerializationProvider.Provide(element.GetType());
                serializationAction(element, writer);
                await writer.FlushAsync(cancellationToken);
            }

            writer.WriteEndArray();

            await writer.FlushAsync(cancellationToken);
        }

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
        public async Task SerializeAsync(IThing thing, Stream stream, JsonWriterOptions jsonWriterOptions, CancellationToken cancellationToken)
        {
            await using var writer = new Utf8JsonWriter(stream, jsonWriterOptions);

            var serializationAction = SerializationProvider.Provide(thing.GetType());
            serializationAction(thing, writer);
            await writer.FlushAsync(cancellationToken);
        }
    }
}
