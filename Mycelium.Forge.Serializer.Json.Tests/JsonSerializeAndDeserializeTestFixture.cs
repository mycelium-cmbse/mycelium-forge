// ------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializeAndDeserializeTestFixture.cs" company="Starion Group S.A.">
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

    using Mycelium.Forge.Common;

    /// <summary>
    /// Serializes a populated DTO to JSON and deserializes it back, asserting the round-trip is
    /// lossless. Per DD-05's own reasoning, this is the single most important test in this project:
    /// "a defect in a template is not a single-type bug but a systematic one affecting every
    /// generated serialiser."
    /// </summary>
    [TestFixture]
    public class JsonSerializeAndDeserializeTestFixture
    {
        private Serializer serializer;
        private DeSerializer deSerializer;

        [SetUp]
        public void SetUp()
        {
            this.serializer = new Serializer();
            this.deSerializer = new DeSerializer();
        }

        /// <summary>
        /// Covers scalar Guid/string/bool/DateTime, an enum property (routed through the F-100
        /// <c>VisibilityKindProvider</c>), and enumerable Guid reference properties.
        /// </summary>
        [Test]
        public void Verify_that_a_fully_populated_Package_round_trips()
        {
            var original = new Package
            {
                Id = Guid.NewGuid(),
                CreatedAt = new DateTime(2026, 1, 15, 10, 30, 0, DateTimeKind.Utc),
                ModifiedAt = new DateTime(2026, 2, 20, 8, 0, 0, DateTimeKind.Utc),
                Listed = true,
                Name = "mycelium-forge",
                ShortName = "mycelium-forge",
                PackageType = Guid.NewGuid(),
                Visibility = VisibilityKind.PUBLIC,
                PackageMaintainer = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
                PackageOwner = new List<Guid> { Guid.NewGuid() },
                Version = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() },
            };

            using var stream = new MemoryStream();
            this.serializer.Serialize(original, stream, default(JsonWriterOptions));

            stream.Position = 0;
            var result = this.deSerializer.DeSerialize(stream);

            var roundTripped = (IPackage)result.Single();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(roundTripped.Id, Is.EqualTo(original.Id));
                Assert.That(roundTripped.CreatedAt, Is.EqualTo(original.CreatedAt));
                Assert.That(roundTripped.ModifiedAt, Is.EqualTo(original.ModifiedAt));
                Assert.That(roundTripped.Listed, Is.EqualTo(original.Listed));
                Assert.That(roundTripped.Name, Is.EqualTo(original.Name));
                Assert.That(roundTripped.ShortName, Is.EqualTo(original.ShortName));
                Assert.That(roundTripped.PackageType, Is.EqualTo(original.PackageType));
                Assert.That(roundTripped.Visibility, Is.EqualTo(original.Visibility));
                Assert.That(roundTripped.PackageMaintainer, Is.EquivalentTo(original.PackageMaintainer));
                Assert.That(roundTripped.PackageOwner, Is.EquivalentTo(original.PackageOwner));
                Assert.That(roundTripped.Version, Is.EquivalentTo(original.Version));
            }
        }

        /// <summary>
        /// Covers nullable string properties in both states: present (<c>AddressLine2</c>) and
        /// absent/null (<c>PostalCode</c>, <c>Region</c>).
        /// </summary>
        [Test]
        public void Verify_that_an_Address_with_null_and_populated_optional_properties_round_trips()
        {
            var original = new Address
            {
                Id = Guid.NewGuid(),
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ModifiedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Country = Guid.NewGuid(),
                AddressLine1 = "1 Infinite Loop",
                AddressLine2 = "Suite 100",
                Locality = "Cupertino",
                PostalCode = null,
                Region = null,
            };

            using var stream = new MemoryStream();
            this.serializer.Serialize(original, stream, default(JsonWriterOptions));

            stream.Position = 0;
            var result = this.deSerializer.DeSerialize(stream);

            var roundTripped = (IAddress)result.Single();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(roundTripped.Id, Is.EqualTo(original.Id));
                Assert.That(roundTripped.Country, Is.EqualTo(original.Country));
                Assert.That(roundTripped.AddressLine1, Is.EqualTo(original.AddressLine1));
                Assert.That(roundTripped.AddressLine2, Is.EqualTo(original.AddressLine2));
                Assert.That(roundTripped.Locality, Is.EqualTo(original.Locality));
                Assert.That(roundTripped.PostalCode, Is.Null);
                Assert.That(roundTripped.Region, Is.Null);
            }
        }
    }
}
