// ------------------------------------------------------------------------------------------------
// <copyright file="SerializerTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Serializer.Json.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Covers the four <see cref="Serializer"/> overloads not already exercised by the round-trip
    /// tests in <see cref="JsonSerializeAndDeserializeTestFixture"/> - the single-<see cref="IThing"/>,
    /// synchronous overload is covered there; this fixture covers the collection and the async
    /// variants.
    /// </summary>
    [TestFixture]
    public class SerializerTestFixture
    {
        private Serializer serializer;

        [SetUp]
        public void SetUp()
        {
            this.serializer = new Serializer();
        }

        private static Package CreateThing()
        {
            return new Package
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                Name = "a-package",
                ShortName = "a-package",
                PackageType = Guid.NewGuid(),
                Visibility = VisibilityKind.PUBLIC,
            };
        }

        [Test]
        public void Verify_that_a_collection_of_things_serializes_to_a_Json_array()
        {
            var things = new List<IThing> { CreateThing(), CreateThing() };

            using var stream = new MemoryStream();
            this.serializer.Serialize(things, stream, default(JsonWriterOptions));

            stream.Position = 0;
            var json = new StreamReader(stream).ReadToEnd();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(json.TrimStart(), Does.StartWith("["));
                Assert.That(json.TrimEnd(), Does.EndWith("]"));
            }
        }

        [Test]
        public async Task Verify_that_SerializeAsync_for_a_single_thing_works()
        {
            var thing = CreateThing();

            using var stream = new MemoryStream();

            await this.serializer.SerializeAsync(thing, stream, default(JsonWriterOptions), CancellationToken.None);

            Assert.That(stream.Length, Is.GreaterThan(0));
        }

        [Test]
        public async Task Verify_that_SerializeAsync_for_a_collection_of_things_works()
        {
            var things = new List<IThing> { CreateThing(), CreateThing() };

            using var stream = new MemoryStream();

            await this.serializer.SerializeAsync(things, stream, default(JsonWriterOptions), CancellationToken.None);

            stream.Position = 0;
            var json = new StreamReader(stream).ReadToEnd();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(json.TrimStart(), Does.StartWith("["));
                Assert.That(json.TrimEnd(), Does.EndWith("]"));
            }
        }

        [Test]
        public void Verify_that_a_collection_serialized_via_SerializeAsync_round_trips_via_DeSerialize()
        {
            var things = new List<IThing> { CreateThing(), CreateThing() };
            var deSerializer = new DeSerializer();

            using var stream = new MemoryStream();
            this.serializer.Serialize(things.Cast<IThing>(), stream, default(JsonWriterOptions));

            stream.Position = 0;
            var result = deSerializer.DeSerialize(stream).ToList();

            Assert.That(result, Has.Count.EqualTo(2));
        }
    }
}
