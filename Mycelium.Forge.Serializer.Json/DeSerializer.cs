// ------------------------------------------------------------------------------------------------
// <copyright file="DeSerializer.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Serializer.Json
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    
    using Mycelium.Forge.Common;
    
    /// <summary>
    /// The purpose of the <see cref="DeSerializer"/> is to deserialize a JSON <see cref="Stream"/> to
    /// an <see cref="IThing"/> and <see cref="IEnumerable{IThing}"/>
    /// </summary>
    public class DeSerializer : IDeSerializer
    {
        /// <summary>
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<DeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        public DeSerializer(ILoggerFactory loggerFactory = null)
        {
            this.loggerFactory = loggerFactory;

            this.logger = this.loggerFactory == null ? NullLogger<DeSerializer>.Instance : this.loggerFactory.CreateLogger<DeSerializer>();
        }

        /// <summary>
        /// Deserializes the JSON stream to an <see cref="IEnumerable{IThing}"/>
        /// </summary>
        /// <param name="stream">
        /// the JSON input stream
        /// </param>
        /// <returns>
        /// an <see cref="IEnumerable{IThing}"/>
        /// </returns>
        public IEnumerable<IThing> DeSerialize(Stream stream)
        {
            var sw = Stopwatch.StartNew();

            var result = new List<IThing>();

            using (var document = JsonDocument.Parse(stream))
            {
                var root = document.RootElement;

                switch (root.ValueKind)
                {
                    case JsonValueKind.Object:
                        result.Add(this.DeserializeObject(root));
                        break;
                    case JsonValueKind.Array:
                        result.AddRange(this.DeserializeArray(root));
                        break;
                    default:
                        throw new SerializationException();
                }
            }

            this.logger.LogInformation("stream deserialized in {ElapsedTime} [ms]", sw.ElapsedMilliseconds);

            return result;
        }

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
        public async Task<IEnumerable<IThing>> DeSerializeAsync(Stream stream, CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();

            var result = new List<IThing>();

            var jsonDocumentOptions = default(JsonDocumentOptions);

            using (var document = await JsonDocument.ParseAsync(stream, jsonDocumentOptions, cancellationToken))
            {
                var root = document.RootElement;

                switch (root.ValueKind)
                {
                    case JsonValueKind.Object:
                        result.Add(this.DeserializeObject(root));
                        break;
                    case JsonValueKind.Array:
                        result.AddRange(this.DeserializeArray(root));
                        break;
                    default:
                        throw new SerializationException();
                }
            }

            this.logger.LogInformation("stream deserialized asynchronously in {ElapsedMilliseconds} [ms]", sw.ElapsedMilliseconds);

            return result;
        }

        /// <summary>
        /// Deserializes an <see cref="JsonElement"/> of type <see cref="JsonValueKind.Object"/> to an <see cref="IThing"/> object
        /// </summary>
        /// <param name="jsonObject">
        /// the subject <see cref="JsonElement"/>
        /// </param>
        /// <returns>
        /// an instance of <see cref="IThing"/>
        /// </returns>
        private IThing DeserializeObject(JsonElement jsonObject)
        {
            if (jsonObject.ValueKind != JsonValueKind.Object)
            {
                throw new ArgumentException($"The {nameof(jsonObject)} must be of type JsonValueKind.Object", nameof(jsonObject));
            }

            if (jsonObject.TryGetProperty("@type", out var typeElement))
            {
                var typeName = typeElement.GetString();

                var func = DeSerializationProvider.Provide(typeName);
                return func(jsonObject, this.loggerFactory);
            }

            throw new SerializationException("The @type Json property is not available, the DeSerializer cannot be used to deserialize this JsonElement");
        }

        /// <summary>
        /// Deserializes an <see cref="JsonElement"/> of type <see cref="JsonValueKind.Array"/> to an <see cref="IEnumerable{IThing}"/> object
        /// </summary>
        /// <param name="jsonArray">
        /// the subject <see cref="JsonElement"/>
        /// </param>
        /// <returns>
        /// an <see cref="IEnumerable{IThing}"/>
        /// </returns>
        private IEnumerable<IThing> DeserializeArray(JsonElement jsonArray)
        {
            if (jsonArray.ValueKind != JsonValueKind.Array)
            {
                throw new ArgumentException($"The {nameof(jsonArray)} must be of type JsonValueKind.Array", nameof(jsonArray));
            }

            var result = new List<IThing>();

            foreach (var jsonElement in jsonArray.EnumerateArray())
            {
                var dataItem = this.DeserializeObject(jsonElement);
                result.Add(dataItem);
            }

            return result;
        }
    }
}
