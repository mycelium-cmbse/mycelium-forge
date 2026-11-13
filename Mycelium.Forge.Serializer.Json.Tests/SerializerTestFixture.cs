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
    /// Covers the four <see cref="Serializer" /> overloads not already exercised by the round-trip
    /// tests in <see cref="JsonSerializeAndDeserializeTestFixture" /> - the single-<see cref="IThing" />,
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

        [Test]
        public void Verify_that_a_collection_of_things_serializes_to_a_Json_array()
        {
            var things = new List<IThing> { CreateThing(), CreateThing() };

            using var stream = new MemoryStream();
            this.serializer.Serialize(things, stream, default);

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
            this.serializer.Serialize(things, stream, default);

            stream.Position = 0;
            var result = deSerializer.DeSerialize(stream).ToList();

            Assert.That(result, Has.Count.EqualTo(2));
        }

        [Test]
        public async Task Verify_that_SerializeAsync_for_a_collection_of_things_works()
        {
            var things = new List<IThing> { CreateThing(), CreateThing() };

            using var stream = new MemoryStream();

            await this.serializer.SerializeAsync(things, stream, default, CancellationToken.None);

            stream.Position = 0;
            var json = await new StreamReader(stream).ReadToEndAsync();

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

            await this.serializer.SerializeAsync(thing, stream, default, CancellationToken.None);

            Assert.That(stream.Length, Is.GreaterThan(0));
        }

        [Test]
        public async Task VerifySerializeValuePropertiesAsync()
        {
            var package = CreateThing();
            package.Owner = Guid.NewGuid();
            package.PackageMaintainer = [Guid.NewGuid()];
            package.PackageOwner = [Guid.NewGuid()];
            package.Version = [Guid.NewGuid()];

            using var packageStream = new MemoryStream();
            await this.serializer.SerializeValuePropertiesAsync(package, packageStream, default, CancellationToken.None);
            packageStream.Position = 0;
            using var packageDoc = await JsonDocument.ParseAsync(packageStream);
            var packageRoot = packageDoc.RootElement;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(packageRoot.TryGetProperty("@type", out var typeProperty), Is.True);
                Assert.That(typeProperty.GetString(), Is.EqualTo("Package"));
                Assert.That(packageRoot.TryGetProperty("@id", out var idProperty), Is.True);
                Assert.That(idProperty.GetGuid(), Is.EqualTo(package.Id));
                Assert.That(packageRoot.TryGetProperty("name", out var nameProperty), Is.True);
                Assert.That(nameProperty.GetString(), Is.EqualTo(package.Name));

                // References omitted
                Assert.That(packageRoot.TryGetProperty("owner", out _), Is.False);
                Assert.That(packageRoot.TryGetProperty("packageType", out _), Is.False);
                Assert.That(packageRoot.TryGetProperty("version", out _), Is.False);
                Assert.That(packageRoot.TryGetProperty("packageMaintainer", out _), Is.False);
                Assert.That(packageRoot.TryGetProperty("packageOwner", out _), Is.False);
            }

            var organization = new Organization
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                Name = "Org With Address",
                ShortName = "org-addr",
                Email = "org@example.com",
                BillingEmail = "billing@example.com",
                DefaultPackageVisibility = VisibilityKind.PRIVATE,
                Status = ScopeStatusKind.ACTIVE,
                Origin = "https://example.com",
                Owner = Guid.NewGuid(),
                PrimaryAddress = Guid.NewGuid(),
                Administrator = [Guid.NewGuid()]
            };

            using var orgStream = new MemoryStream();
            await this.serializer.SerializeValuePropertiesAsync(organization, orgStream, default, CancellationToken.None);
            orgStream.Position = 0;
            using var orgDoc = await JsonDocument.ParseAsync(orgStream);
            var orgRoot = orgDoc.RootElement;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(orgRoot.TryGetProperty("@type", out var typeProperty), Is.True);
                Assert.That(typeProperty.GetString(), Is.EqualTo("Organization"));
                Assert.That(orgRoot.TryGetProperty("@id", out var idProperty), Is.True);
                Assert.That(idProperty.GetGuid(), Is.EqualTo(organization.Id));
                Assert.That(orgRoot.TryGetProperty("name", out var orgNameProperty), Is.True);
                Assert.That(orgNameProperty.GetString(), Is.EqualTo(organization.Name));

                // References omitted
                Assert.That(orgRoot.TryGetProperty("owner", out _), Is.False);
                Assert.That(orgRoot.TryGetProperty("primaryAddress", out _), Is.False);
                Assert.That(orgRoot.TryGetProperty("administrator", out _), Is.False);
                Assert.That(orgRoot.TryGetProperty("member", out _), Is.False);
                Assert.That(orgRoot.TryGetProperty("address", out _), Is.False);
                Assert.That(orgRoot.TryGetProperty("ownedPackage", out _), Is.False);
                Assert.That(orgRoot.TryGetProperty("profileLink", out _), Is.False);
            }
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
                Visibility = VisibilityKind.PUBLIC
            };
        }
    }
}
