// ------------------------------------------------------------------------------------------------
// <copyright file="ContractRoundTripTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Text.Json.Nodes;

    using Mycelium.Forge.Serializer.Json;

    /// <summary>
    /// The contract-test harness for the generated serialisers (F-06): per DD-05, a defect in a
    /// generator template is systematic rather than confined to one type, so the fixture in
    /// <c>TestData/all-dto-types-and-enum-variations.json</c> carries one instance of every concrete
    /// generated DTO class, with enough repeated instances to cover every member of every generated
    /// enum at least once. Deserializing it and serializing the result again must reproduce the same
    /// JSON, structurally.
    /// </summary>
    [TestFixture]
    public class ContractRoundTripTestFixture
    {
        private string fixtureJson;

        [SetUp]
        public void SetUp()
        {
            var fixturePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "all-dto-types-and-enum-variations.json");

            this.fixtureJson = File.ReadAllText(fixturePath);
        }

        [Test]
        public void Verify_that_the_fixture_covers_every_concrete_generated_DTO_class()
        {
            var expectedTypes = new[]
            {
                "APIKey", "Account", "Address", "Country", "Forge", "Organization", "OrganizationInvitation",
                "Package", "PackageInvitation", "PackageMetaData", "PackageType", "PackageVersion", "ProfileLink", "ProfileType",
            };

            var actualTypes = JsonNode.Parse(this.fixtureJson)!.AsArray()
                .Select(node => node!["@type"]!.GetValue<string>())
                .Distinct()
                .ToArray();

            Assert.That(actualTypes, Is.SupersetOf(expectedTypes));
        }

        [Test]
        public void Verify_that_deserializing_and_reserializing_the_fixture_reproduces_the_same_JSON()
        {
            var deSerializer = new DeSerializer();
            var serializer = new Serializer();

            using var inputStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(this.fixtureJson));
            var things = deSerializer.DeSerialize(inputStream).ToList();

            using var outputStream = new MemoryStream();
            serializer.Serialize(things, outputStream, default(JsonWriterOptions));

            var roundTrippedJson = System.Text.Encoding.UTF8.GetString(outputStream.ToArray());

            var original = JsonNode.Parse(this.fixtureJson);
            var roundTripped = JsonNode.Parse(roundTrippedJson);

            Assert.That(JsonNode.DeepEquals(original, roundTripped), Is.True,
                () => $"Round-tripped JSON differs from the fixture.\n--- original ---\n{original}\n--- round-tripped ---\n{roundTripped}");
        }

        [Test]
        public void Verify_that_every_member_of_every_generated_enum_appears_in_the_fixture()
        {
            var array = JsonNode.Parse(this.fixtureJson)!.AsArray();

            void AssertEnumCovered<TEnum>(params string[] propertyNames)
                where TEnum : struct, System.Enum
            {
                var actualValues = propertyNames
                    .SelectMany(propertyName => array
                        .Where(node => node![propertyName] != null)
                        .Select(node => node![propertyName]!.GetValue<string>()))
                    .ToHashSet();

                var expectedValues = System.Enum.GetNames<TEnum>();

                Assert.That(actualValues, Is.SupersetOf(expectedValues), $"not every {typeof(TEnum).Name} member is covered");
            }

            using (Assert.EnterMultipleScope())
            {
                AssertEnumCovered<VisibilityKind>("visibility", "defaultPackageVisibility");
                AssertEnumCovered<ScopeStatusKind>("status");
                AssertEnumCovered<InvitationStatusKind>("status");
                AssertEnumCovered<OrganizationInvitationKind>("organizationInvitationKind");
                AssertEnumCovered<PackageInvitationKind>("packageInvitationKind");
            }
        }
    }
}
