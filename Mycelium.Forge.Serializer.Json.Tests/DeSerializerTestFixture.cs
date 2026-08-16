// ------------------------------------------------------------------------------------------------
// <copyright file="DeSerializerTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Serializer.Json.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Covers the <see cref="DeSerializer"/> paths not already exercised by the round-trip tests in
    /// <see cref="JsonSerializeAndDeserializeTestFixture"/> - the single-object,
    /// <see cref="DeSerializer.DeSerialize(Stream)"/> happy path is covered there; this fixture covers
    /// the array root, the async overload, and the error paths.
    /// </summary>
    [TestFixture]
    public class DeSerializerTestFixture
    {
        private Serializer serializer;
        private DeSerializer deSerializer;

        [SetUp]
        public void SetUp()
        {
            this.serializer = new Serializer();
            this.deSerializer = new DeSerializer();
        }

        private static MemoryStream ToStream(string json)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(json));
        }

        [Test]
        public void Verify_that_a_Json_array_of_things_deserializes()
        {
            var things = new IThing[]
            {
                new Package { Id = Guid.NewGuid(), Name = "a", ShortName = "a", PackageType = Guid.NewGuid(), Visibility = VisibilityKind.PUBLIC },
                new Package { Id = Guid.NewGuid(), Name = "b", ShortName = "b", PackageType = Guid.NewGuid(), Visibility = VisibilityKind.PUBLIC },
            };

            using var stream = new MemoryStream();
            this.serializer.Serialize(things, stream, default(JsonWriterOptions));

            stream.Position = 0;
            var result = this.deSerializer.DeSerialize(stream).ToList();

            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result, Has.All.InstanceOf<IPackage>());
        }

        [Test]
        public async Task Verify_that_DeSerializeAsync_deserializes_a_single_object()
        {
            var thing = new Package { Id = Guid.NewGuid(), Name = "a", ShortName = "a", PackageType = Guid.NewGuid(), Visibility = VisibilityKind.PUBLIC };

            using var stream = new MemoryStream();
            this.serializer.Serialize(thing, stream, default(JsonWriterOptions));
            stream.Position = 0;

            var result = await this.deSerializer.DeSerializeAsync(stream, CancellationToken.None);

            Assert.That(result.Single().Id, Is.EqualTo(thing.Id));
        }

        [Test]
        public async Task Verify_that_DeSerializeAsync_deserializes_a_Json_array()
        {
            var things = new IThing[]
            {
                new Package { Id = Guid.NewGuid(), Name = "a", ShortName = "a", PackageType = Guid.NewGuid(), Visibility = VisibilityKind.PUBLIC },
                new Package { Id = Guid.NewGuid(), Name = "b", ShortName = "b", PackageType = Guid.NewGuid(), Visibility = VisibilityKind.PUBLIC },
            };

            using var stream = new MemoryStream();
            this.serializer.Serialize(things, stream, default(JsonWriterOptions));
            stream.Position = 0;

            var result = await this.deSerializer.DeSerializeAsync(stream, CancellationToken.None);

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Verify_that_deserializing_an_object_without_a_type_property_throws()
        {
            using var stream = ToStream("""{ "@id": "8f6a2b1e-8a2f-4a3b-9b0e-1a2b3c4d5e6f" }""");

            Assert.That(() => this.deSerializer.DeSerialize(stream), Throws.TypeOf<SerializationException>());
        }

        [Test]
        public void Verify_that_deserializing_a_root_that_is_neither_an_object_nor_an_array_throws()
        {
            using var stream = ToStream("\"just a string\"");

            Assert.That(() => this.deSerializer.DeSerialize(stream), Throws.TypeOf<SerializationException>());
        }

        [Test]
        public void Verify_that_DeSerializeAsync_with_a_root_that_is_neither_an_object_nor_an_array_throws()
        {
            using var stream = ToStream("42");

            Assert.That(async () => await this.deSerializer.DeSerializeAsync(stream, CancellationToken.None), Throws.TypeOf<SerializationException>());
        }

        [Test]
        public void Verify_that_an_array_containing_a_non_object_element_throws()
        {
            using var stream = ToStream("""[ "not-an-object" ]""");

            Assert.That(() => this.deSerializer.DeSerialize(stream), Throws.TypeOf<ArgumentException>());
        }

        /// <summary>
        /// The private <c>DeserializeArray</c> guards against being called with a non-array
        /// <see cref="JsonElement"/>, but every current call site already checks
        /// <c>ValueKind == JsonValueKind.Array</c> before calling it, so that guard is unreachable
        /// through the public API. Verified directly via reflection instead of removing a legitimate
        /// defensive check on a private method that a future caller might not guarantee the same way.
        /// </summary>
        [Test]
        public void Verify_that_DeserializeArray_rejects_a_non_array_JsonElement()
        {
            var method = typeof(DeSerializer).GetMethod("DeserializeArray", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            using var document = JsonDocument.Parse("{}");
            var nonArrayElement = document.RootElement;

            var exception = Assert.Throws<System.Reflection.TargetInvocationException>(
                () => method!.Invoke(this.deSerializer, [nonArrayElement]));

            Assert.That(exception!.InnerException, Is.TypeOf<ArgumentException>());
        }

        /// <summary>
        /// <see cref="DeSerializer.DeSerialize(Stream)"/>/<see cref="DeSerializer.DeSerializeAsync"/>
        /// only log their completion message when <c>ILogger.IsEnabled(LogLevel.Information)</c> is
        /// true - with the default (parameterless) <see cref="DeSerializer"/> constructor, the logger
        /// is <see cref="Microsoft.Extensions.Logging.Abstractions.NullLogger"/>, which is always
        /// disabled, so every other test in this fixture only exercises the disabled branch. This test
        /// supplies a minimal always-enabled <see cref="ILogger"/> (no mocking framework is referenced
        /// by this project) to reach the enabled one too.
        /// </summary>
        [Test]
        public async Task Verify_that_DeSerialize_and_DeSerializeAsync_still_work_with_an_always_enabled_logger()
        {
            var loggingDeSerializer = new DeSerializer(new AlwaysEnabledLoggerFactory());

            var thing = new Package { Id = Guid.NewGuid(), Name = "a", ShortName = "a", PackageType = Guid.NewGuid(), Visibility = VisibilityKind.PUBLIC };

            using var syncStream = new MemoryStream();
            this.serializer.Serialize(thing, syncStream, default(JsonWriterOptions));
            syncStream.Position = 0;

            var syncResult = loggingDeSerializer.DeSerialize(syncStream);

            using var asyncStream = new MemoryStream();
            this.serializer.Serialize(thing, asyncStream, default(JsonWriterOptions));
            asyncStream.Position = 0;

            var asyncResult = await loggingDeSerializer.DeSerializeAsync(asyncStream, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(syncResult.Single().Id, Is.EqualTo(thing.Id));
                Assert.That(asyncResult.Single().Id, Is.EqualTo(thing.Id));
            }
        }

        private sealed class AlwaysEnabledLoggerFactory : ILoggerFactory
        {
            public void AddProvider(ILoggerProvider provider)
            {
            }

            public ILogger CreateLogger(string categoryName)
            {
                return AlwaysEnabledLogger.Instance;
            }

            public void Dispose()
            {
            }
        }

        private sealed class AlwaysEnabledLogger : ILogger
        {
            public static readonly AlwaysEnabledLogger Instance = new();

            public IDisposable BeginScope<TState>(TState state) where TState : notnull
            {
                return NullScope.Instance;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
            }

            private sealed class NullScope : IDisposable
            {
                public static readonly NullScope Instance = new();

                public void Dispose()
                {
                }
            }
        }
    }
}
