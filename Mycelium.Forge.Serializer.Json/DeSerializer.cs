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
    using System.Buffers;
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
    /// <remarks>
    /// The stream is read once into an <see cref="ArrayPool{Byte}"/>-rented buffer and walked with a
    /// single forward-only <see cref="Utf8JsonReader"/> pass. Unlike <see cref="JsonDocument"/>, this
    /// never materializes a DOM node per JSON value, which is the dominant allocation/CPU cost of
    /// deserializing large payloads.
    /// </remarks>
    public class DeSerializer : IDeSerializer
    {
        /// <summary>
        /// The initial size, in bytes, of the buffer rented from <see cref="ArrayPool{Byte}"/> to read a JSON stream into
        /// </summary>
        private const int InitialBufferSize = 81920;

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

            var rentedBuffer = ReadToPooledBuffer(stream, out var length);

            try
            {
                this.DeserializeBuffer(rentedBuffer, length, result);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(rentedBuffer);
            }

            if (this.logger.IsEnabled(LogLevel.Information))
            {
                this.logger.LogInformation("stream deserialized in {ElapsedTime} [ms]", sw.ElapsedMilliseconds);
            }

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

            var (rentedBuffer, length) = await ReadToPooledBufferAsync(stream, cancellationToken);

            try
            {
                this.DeserializeBuffer(rentedBuffer, length, result);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(rentedBuffer);
            }

            if (this.logger.IsEnabled(LogLevel.Information))
            {
                this.logger.LogInformation("stream deserialized asynchronously in {ElapsedMilliseconds} [ms]", sw.ElapsedMilliseconds);
            }

            return result;
        }

        /// <summary>
        /// Reads the entirety of the <paramref name="stream"/> into a rented buffer, growing it as needed
        /// </summary>
        /// <param name="stream">
        /// the JSON input stream
        /// </param>
        /// <param name="length">
        /// the number of bytes read into the returned buffer
        /// </param>
        /// <returns>
        /// a buffer rented from <see cref="ArrayPool{Byte}"/> that the caller must return
        /// </returns>
        private static byte[] ReadToPooledBuffer(Stream stream, out int length)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(InitialBufferSize);
            var totalRead = 0;

            int bytesRead;
            while ((bytesRead = stream.Read(buffer, totalRead, buffer.Length - totalRead)) > 0)
            {
                totalRead += bytesRead;

                if (totalRead == buffer.Length)
                {
                    buffer = Grow(buffer, totalRead);
                }
            }

            length = totalRead;
            return buffer;
        }

        /// <summary>
        /// Asynchronously reads the entirety of the <paramref name="stream"/> into a rented buffer, growing it as needed
        /// </summary>
        /// <param name="stream">
        /// the JSON input stream
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to cancel the operation
        /// </param>
        /// <returns>
        /// a buffer rented from <see cref="ArrayPool{Byte}"/> that the caller must return, and the number of bytes read into it
        /// </returns>
        private static async Task<(byte[] Buffer, int Length)> ReadToPooledBufferAsync(Stream stream, CancellationToken cancellationToken)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(InitialBufferSize);
            var totalRead = 0;

            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer.AsMemory(totalRead, buffer.Length - totalRead), cancellationToken)) > 0)
            {
                totalRead += bytesRead;

                if (totalRead == buffer.Length)
                {
                    buffer = Grow(buffer, totalRead);
                }
            }

            return (buffer, totalRead);
        }

        /// <summary>
        /// Rents a larger buffer, copies the previously read bytes into it, and returns the old buffer to the pool
        /// </summary>
        private static byte[] Grow(byte[] buffer, int bytesToPreserve)
        {
            var newBuffer = ArrayPool<byte>.Shared.Rent(buffer.Length * 2);
            Buffer.BlockCopy(buffer, 0, newBuffer, 0, bytesToPreserve);
            ArrayPool<byte>.Shared.Return(buffer);
            return newBuffer;
        }

        /// <summary>
        /// Drives a single <see cref="Utf8JsonReader"/> pass over the buffered JSON payload
        /// </summary>
        private void DeserializeBuffer(byte[] buffer, int length, List<IThing> result)
        {
            var span = new ReadOnlySpan<byte>(buffer, 0, length);

            if (span.StartsWith(Utf8Bom))
            {
                span = span[Utf8Bom.Length..];
            }

            var reader = new Utf8JsonReader(span);

            if (!reader.Read())
            {
                return;
            }

            switch (reader.TokenType)
            {
                case JsonTokenType.StartObject:
                    result.Add(this.DeserializeObject(ref reader));
                    break;
                case JsonTokenType.StartArray:
                    this.DeserializeArray(ref reader, result);
                    break;
                default:
                    throw new SerializationException();
            }
        }

        /// <summary>
        /// The UTF-8 byte-order-mark, which <see cref="Utf8JsonReader"/> does not skip on its own
        /// </summary>
        private static ReadOnlySpan<byte> Utf8Bom => [0xEF, 0xBB, 0xBF];

        /// <summary>
        /// Deserializes a single JSON object off the <paramref name="reader"/> to an <see cref="IThing"/> object
        /// </summary>
        /// <param name="reader">
        /// The <see cref="Utf8JsonReader"/>, positioned at the object's <see cref="JsonTokenType.StartObject"/> token
        /// </param>
        /// <returns>
        /// an instance of <see cref="IThing"/>
        /// </returns>
        private IThing DeserializeObject(ref Utf8JsonReader reader)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new ArgumentException($"The {nameof(reader)} must be positioned at a JSON object", nameof(reader));
            }

            var typeName = Utf8JsonReaderHelper.PeekTypeName(reader);

            if (typeName == null)
            {
                throw new SerializationException("The @type Json property is not available, the DeSerializer cannot be used to deserialize this JsonElement");
            }

            var deSerialize = DeSerializationProvider.Provide(typeName);
            return deSerialize(ref reader, this.loggerFactory);
        }

        /// <summary>
        /// Deserializes a JSON array off the <paramref name="reader"/> to an <see cref="IEnumerable{IThing}"/> object
        /// </summary>
        /// <param name="reader">
        /// The <see cref="Utf8JsonReader"/>, positioned at the array's <see cref="JsonTokenType.StartArray"/> token
        /// </param>
        /// <param name="result">
        /// the <see cref="List{IThing}"/> to which the deserialized items are added
        /// </param>
        private void DeserializeArray(ref Utf8JsonReader reader, List<IThing> result)
        {
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                result.Add(this.DeserializeObject(ref reader));
            }
        }
    }
}
