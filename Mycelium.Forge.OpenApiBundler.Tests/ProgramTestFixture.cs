// ------------------------------------------------------------------------------------------------
// <copyright file="ProgramTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.OpenApiBundler.Tests
{
    using System.Linq;
    using System.Text.Json.Nodes;

    /// <summary>
    /// Exercises <see cref="Program"/>'s merge and ref-resolution logic directly, against small
    /// hand-built documents rather than the real, large checked-in files - the integration-level
    /// check that the real <c>shared.json</c>, <c>paths.json</c> and <c>schemas.json</c> merge cleanly
    /// already lives in <c>Mycelium.Forge.Common.Tests.OpenApiPathsDocumentTestFixture</c>.
    /// </summary>
    [TestFixture]
    public class ProgramTestFixture
    {
        private static JsonObject MinimalSharedDocument()
        {
            return new JsonObject
            {
                ["info"] = new JsonObject { ["title"] = "Test", ["version"] = "1.0.0" },
                ["components"] = new JsonObject
                {
                    ["schemas"] = new JsonObject
                    {
                        ["ProblemDetails"] = new JsonObject { ["type"] = "object" }
                    }
                }
            };
        }

        private static JsonObject MinimalPathsDocument()
        {
            return new JsonObject
            {
                ["paths"] = new JsonObject
                {
                    ["/things"] = new JsonObject
                    {
                        ["get"] = new JsonObject
                        {
                            ["operationId"] = "ListThings",
                            ["responses"] = new JsonObject
                            {
                                ["200"] = new JsonObject
                                {
                                    ["content"] = new JsonObject
                                    {
                                        ["application/json"] = new JsonObject
                                        {
                                            ["schema"] = new JsonObject { ["$ref"] = "#/components/schemas/Thing" }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        private static JsonObject SchemasDocument(params string[] schemaNames)
        {
            var schemas = new JsonObject();

            foreach (var name in schemaNames)
            {
                schemas[name] = new JsonObject { ["type"] = "object" };
            }

            return new JsonObject
            {
                ["components"] = new JsonObject { ["schemas"] = schemas }
            };
        }

        [Test]
        public void Verify_that_Bundle_sets_the_openapi_version()
        {
            var bundled = Program.Bundle(MinimalSharedDocument(), MinimalPathsDocument(), SchemasDocument("Thing"));

            Assert.That(bundled["openapi"]!.GetValue<string>(), Is.EqualTo("3.1.0"));
        }

        [Test]
        public void Verify_that_Bundle_copies_paths_from_the_paths_document()
        {
            var bundled = Program.Bundle(MinimalSharedDocument(), MinimalPathsDocument(), SchemasDocument("Thing"));

            Assert.That(bundled["paths"]!["/things"]!["get"]!["operationId"]!.GetValue<string>(), Is.EqualTo("ListThings"));
        }

        [Test]
        public void Verify_that_Bundle_merges_generated_schemas_into_components_schemas()
        {
            var bundled = Program.Bundle(MinimalSharedDocument(), MinimalPathsDocument(), SchemasDocument("Thing", "ThingReference"));

            var schemaNames = bundled["components"]!["schemas"]!.AsObject().Select(x => x.Key).OrderBy(x => x).ToArray();

            Assert.That(schemaNames, Is.EqualTo(new[] { "ProblemDetails", "Thing", "ThingReference" }));
        }

        [Test]
        public void Verify_that_Bundle_throws_on_a_components_schemas_name_collision()
        {
            Assert.That(
                () => Program.Bundle(MinimalSharedDocument(), MinimalPathsDocument(), SchemasDocument("ProblemDetails")),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Verify_that_ValidateEveryRefResolves_does_not_throw_when_every_ref_resolves()
        {
            var bundled = Program.Bundle(MinimalSharedDocument(), MinimalPathsDocument(), SchemasDocument("Thing"));

            Assert.That(() => Program.ValidateEveryRefResolves(bundled), Throws.Nothing);
        }

        [Test]
        public void Verify_that_ValidateEveryRefResolves_throws_when_a_ref_is_unresolved()
        {
            var bundled = Program.Bundle(MinimalSharedDocument(), MinimalPathsDocument(), SchemasDocument());

            Assert.That(() => Program.ValidateEveryRefResolves(bundled), Throws.TypeOf<InvalidOperationException>());
        }
    }
}
