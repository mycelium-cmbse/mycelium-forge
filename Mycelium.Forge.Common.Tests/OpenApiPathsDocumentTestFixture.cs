// ------------------------------------------------------------------------------------------------
// <copyright file="OpenApiPathsDocumentTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json.Nodes;

    /// <summary>
    /// Verifies that the hand-authored, shared <c>OpenApi/shared.json</c> is internally consistent
    /// with the generated <c>AutoGenOpenApi/paths.json</c> and <c>AutoGenOpenApi/schemas.json</c>,
    /// since the three are only ever combined at bundle time - a broken <c>$ref</c> anywhere would
    /// otherwise go unnoticed until someone opened the bundled <c>openapi.json</c> in a viewer.
    /// </summary>
    [TestFixture]
    public class OpenApiPathsDocumentTestFixture
    {
        private JsonObject pathsDocument;
        private JsonObject mergedComponents;

        [SetUp]
        public void SetUp()
        {
            var repositoryRoot = FindRepositoryRoot();

            var sharedJson = File.ReadAllText(Path.Combine(repositoryRoot, "Mycelium.Forge.Common", "OpenApi", "shared.json"));
            var pathsJson = File.ReadAllText(Path.Combine(repositoryRoot, "Mycelium.Forge.Common", "AutoGenOpenApi", "paths.json"));
            var schemasJson = File.ReadAllText(Path.Combine(repositoryRoot, "Mycelium.Forge.Common", "AutoGenOpenApi", "schemas.json"));

            var sharedDocument = JsonNode.Parse(sharedJson)!.AsObject();
            this.pathsDocument = JsonNode.Parse(pathsJson)!.AsObject();
            var schemasDocument = JsonNode.Parse(schemasJson)!.AsObject();

            this.mergedComponents = sharedDocument["components"]!.DeepClone().AsObject();
            var generatedSchemas = schemasDocument["components"]!["schemas"]!.AsObject();

            var handAuthoredSchemas = this.mergedComponents["schemas"]!.AsObject();

            foreach (var (name, schema) in generatedSchemas.ToList())
            {
                handAuthoredSchemas.TryAdd(name, schema?.DeepClone());
            }
        }

        private static string FindRepositoryRoot()
        {
            var directory = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);

            while (directory != null && directory.GetFiles("Mycelium.Forge.sln").Length == 0)
            {
                directory = directory.Parent;
            }

            if (directory == null)
            {
                throw new InvalidOperationException("Could not locate the repository root (Mycelium.Forge.sln) above the test directory.");
            }

            return directory.FullName;
        }

        [Test]
        public void Verify_that_every_dollar_ref_in_the_paths_document_resolves()
        {
            var refs = CollectRefs(this.pathsDocument).Distinct().OrderBy(x => x).ToList();

            Assert.That(refs, Is.Not.Empty);

            using (Assert.EnterMultipleScope())
            {
                foreach (var reference in refs)
                {
                    Assert.That(TryResolve(reference, out _), Is.True, $"Unresolved $ref: {reference}");
                }
            }
        }

        [Test]
        public void Verify_that_every_operation_has_a_unique_operation_id()
        {
            var operationIds = new List<string>();

            var paths = this.pathsDocument["paths"]!.AsObject();

            foreach (var (_, pathItem) in paths)
            {
                foreach (var (_, operation) in pathItem!.AsObject())
                {
                    operationIds.Add(operation!["operationId"]!.GetValue<string>());
                }
            }

            Assert.That(operationIds, Is.Unique);
        }

        /// <summary>
        /// Resolves a <c>#/a/b/c</c> style JSON Pointer against <see cref="mergedComponents"/> -
        /// every <c>$ref</c> in this codebase's hand-authored document points into
        /// <c>#/components/...</c>, so resolution only ever needs to walk the merged component tree,
        /// not the full document.
        /// </summary>
        private bool TryResolve(string reference, out JsonNode? node)
        {
            node = null;

            const string prefix = "#/components/";

            if (!reference.StartsWith(prefix, StringComparison.Ordinal))
            {
                return false;
            }

            var segments = reference[prefix.Length..].Split('/');

            JsonNode? current = this.mergedComponents;

            foreach (var segment in segments)
            {
                if (current is not JsonObject obj || !obj.TryGetPropertyValue(segment, out current))
                {
                    return false;
                }
            }

            node = current;

            return node != null;
        }

        private static IEnumerable<string> CollectRefs(JsonNode? node)
        {
            switch (node)
            {
                case JsonObject obj:
                    foreach (var (key, value) in obj)
                    {
                        if (key == "$ref" && value is JsonValue refValue && refValue.TryGetValue<string>(out var refString))
                        {
                            yield return refString;
                        }
                        else
                        {
                            foreach (var nested in CollectRefs(value))
                            {
                                yield return nested;
                            }
                        }
                    }

                    break;

                case JsonArray array:
                    foreach (var item in array)
                    {
                        foreach (var nested in CollectRefs(item))
                        {
                            yield return nested;
                        }
                    }

                    break;
            }
        }
    }
}
