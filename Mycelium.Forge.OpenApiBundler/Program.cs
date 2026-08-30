// ------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Mycelium.Forge.OpenApiBundler.Tests")]

namespace Mycelium.Forge.OpenApiBundler
{
    using System.Text.Json;
    using System.Text.Json.Nodes;

    /// <summary>
    /// Merges three documents into one self-contained <c>openapi.json</c>: the hand-authored, shared
    /// <c>info</c>/<c>servers</c>/<c>components</c>
    /// (<c>Mycelium.Forge.Common/OpenApi/shared.json</c>), the generated <c>paths</c>
    /// (<c>Mycelium.Forge.Common/AutoGenOpenApi/paths.json</c>), and the generated component schemas
    /// (<c>Mycelium.Forge.Common/AutoGenOpenApi/schemas.json</c>). Invoked from
    /// <c>Directory.Build.targets</c> the same way DD-08 invokes the Tailwind CLI: a build-time step
    /// producing a gitignored artefact, not a manually-run, by-hand-reviewed regeneration test like
    /// every other generator in this codebase.
    /// </summary>
    internal static class Program
    {
        private static readonly JsonSerializerOptions WriteOptions = new()
        {
            WriteIndented = true
        };

        private static int Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.Error.WriteLine("Usage: Mycelium.Forge.OpenApiBundler <shared.json> <paths.json> <schemas.json> <output openapi.json>");

                return 1;
            }

            var sharedDocumentPath = args[0];
            var pathsDocumentPath = args[1];
            var schemasDocumentPath = args[2];
            var outputPath = args[3];

            try
            {
                var sharedDocument = JsonNode.Parse(File.ReadAllText(sharedDocumentPath))!.AsObject();
                var pathsDocument = JsonNode.Parse(File.ReadAllText(pathsDocumentPath))!.AsObject();
                var schemasDocument = JsonNode.Parse(File.ReadAllText(schemasDocumentPath))!.AsObject();

                var bundled = Bundle(sharedDocument, pathsDocument, schemasDocument);

                ValidateEveryRefResolves(bundled);

                var outputDirectory = Path.GetDirectoryName(outputPath);

                if (!string.IsNullOrEmpty(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                File.WriteAllText(outputPath, bundled.ToJsonString(WriteOptions));

                Console.WriteLine($"Bundled {outputPath}");

                return 0;
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine($"OpenAPI bundling failed: {exception.Message}");

                return 1;
            }
        }

        /// <summary>
        /// Builds the bundled document: <c>"openapi": "3.1.0"</c>, <paramref name="sharedDocument"/>'s
        /// own <c>info</c>/<c>servers</c>/<c>components</c> verbatim, <paramref name="pathsDocument"/>'s
        /// <c>paths</c>, and every <paramref name="schemasDocument"/> <c>components.schemas</c> entry
        /// merged into the shared document's own <c>components.schemas</c> bucket - a name collision
        /// between the two is a real authoring bug (a hand-authored schema shadowing a generated one,
        /// or vice versa), so it fails loudly rather than silently letting one side win.
        /// </summary>
        internal static JsonObject Bundle(JsonObject sharedDocument, JsonObject pathsDocument, JsonObject schemasDocument)
        {
            var bundled = sharedDocument.DeepClone().AsObject();

            bundled.Remove("openapi");
            bundled.Insert(0, "openapi", "3.1.0");

            bundled["paths"] = pathsDocument["paths"]!.DeepClone();

            var components = bundled["components"]!.AsObject();
            var handAuthoredSchemas = components["schemas"]!.AsObject();
            var generatedSchemas = schemasDocument["components"]!["schemas"]!.AsObject();

            foreach (var (name, schema) in generatedSchemas)
            {
                if (!handAuthoredSchemas.TryAdd(name, schema?.DeepClone()))
                {
                    throw new InvalidOperationException(
                        $"Both the shared document and the generated schemas document define a components.schemas entry named '{name}'.");
                }
            }

            return bundled;
        }

        /// <summary>
        /// Walks the entire bundled document collecting every <c>$ref</c>, and resolves each as a
        /// <c>#/...</c> JSON Pointer against the same document - the bundled file is a self-contained
        /// <c>openapi.json</c>, so nothing in it should point outside itself.
        /// </summary>
        internal static void ValidateEveryRefResolves(JsonObject bundled)
        {
            foreach (var reference in CollectRefs(bundled).Distinct())
            {
                if (!TryResolve(bundled, reference, out _))
                {
                    throw new InvalidOperationException($"Unresolved $ref in the bundled document: {reference}");
                }
            }
        }

        private static bool TryResolve(JsonObject root, string reference, out JsonNode? node)
        {
            node = null;

            if (!reference.StartsWith("#/", StringComparison.Ordinal))
            {
                return false;
            }

            var segments = reference[2..].Split('/');

            JsonNode? current = root;

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
