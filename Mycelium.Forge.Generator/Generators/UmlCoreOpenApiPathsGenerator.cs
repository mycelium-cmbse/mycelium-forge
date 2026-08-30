// ------------------------------------------------------------------------------------------------
// <copyright file="UmlCoreOpenApiPathsGenerator.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Generators
{
    using System.Text.Json;
    using System.Text.Json.Nodes;

    using Mycelium.Forge.Generator.Extensions;

    using uml4net.Classification;
    using uml4net.Extensions;
    using uml4net.SimpleClassifiers;
    using uml4net.StructuredClassifiers;
    using uml4net.xmi.Readers;

    /// <summary>
    /// Generates the OpenAPI <c>paths</c> document from the model's own containment structure, not
    /// REST convention - this HTTP API is not meant for humans navigating URLs in a browser.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The URL itself mirrors the containment tree, and every path segment is the model's own
    /// composite-owning property name, verbatim - never a class name, never invented by pluralizing
    /// one. <c>Forge.organization</c> (an array-valued property, despite its singular name) routes as
    /// <c>/organization</c>, not <c>/organizations</c>; <c>Account.apiKey</c> routes as
    /// <c>/account/{accountIdentifier}/apiKey</c>. This generator never re-derives a collection name
    /// from a class name - it always looks up the actual composite property that owns the class and
    /// uses that property's own <see cref="uml4net.SimpleClassifiers.IProperty.Name"/>.
    /// </para>
    /// <para>
    /// A class with no composite owner other than <c>Forge</c> (the one singleton root) - reached via
    /// <c>Forge.account</c>, <c>Forge.organization</c>, <c>Forge.country</c>, <c>Forge.packageType</c>,
    /// <c>Forge.profileType</c> - gets a flat, top-level collection. Every other composite child nests
    /// under its owner's own item path, however deep the chain - <c>Package.version</c> (owning
    /// <c>PackageVersion</c>) under <c>Scope.ownedPackage</c> (owning <c>Package</c>) under
    /// <c>Forge.account</c>/<c>Forge.organization</c> becomes
    /// <c>/account/{accountIdentifier}/ownedPackage/{packageIdentifier}/version</c> - so there is no
    /// ownerless <c>POST /version</c>: a class that's never independently created doesn't get one.
    /// When the owner is abstract (<c>Scope</c>, whose only concrete subtypes are <c>Account</c> and
    /// <c>Organization</c>), the nested route is registered once per concrete subtype - <c>Address</c>
    /// is reachable at both <c>/account/{accountIdentifier}/address</c> and
    /// <c>/organization/{organizationIdentifier}/address</c>, never at a bare <c>/address</c>.
    /// </para>
    /// <para>
    /// The owning property's own multiplicity decides the route shape, not just its name. An
    /// upper-bound-1 composite property - <c>PackageVersion.metaData</c>,
    /// <c>Account.ownedPackageInvitation</c> - is a singleton, not a collection: no <c>List</c>, no
    /// <c>POST</c>, no <c>{identifier}</c>/<c>{shortName}</c> sub-path, since there is always exactly
    /// one and the parent context alone addresses it - <c>GET</c>/<c>PUT</c>/<c>PATCH</c>/<c>DELETE</c>
    /// operate directly at <c>.../metaData</c>, the same shape <c>/forge</c> itself already has. An
    /// unbounded (<c>*</c>) composite property gets the full collection shape:
    /// <c>GET</c>/<c>POST /{prefix}/{collection}</c>,
    /// <c>GET</c>/<c>PUT</c>/<c>PATCH</c>/<c>DELETE /{prefix}/{collection}/{identifier}</c>. A class's
    /// deeper composite children remain additionally reachable via <c>include-contained</c> walked
    /// from this same <c>GET</c>, and a plain association's reference stubs via
    /// <c>include-referenced</c> - containment still governs that reachability, just not, on its own,
    /// whether a class has routes of its own.
    /// </para>
    /// <para>
    /// <c>{identifier}</c> - the leaf, being-operated-on segment of a collection route - accepts the
    /// resource's own <c>Thing.Id</c>, a <c>ShortGuid</c> encoding of it, or (for a <c>GET</c>) a
    /// bracketed list of ShortGuids for a batch lookup in one request. A class that (via
    /// <c>Namespace</c>) owns a property literally named <c>shortName</c> - today, <c>Account</c>,
    /// <c>Organization</c>, <c>Package</c> - gets a second, separate, <c>GET</c>-only
    /// <c>{shortName}</c> item path. <c>PUT</c>, <c>PATCH</c> and <c>DELETE</c> only ever go through
    /// <c>{identifier}</c> - <c>{shortName}</c> is a read-only alias, never a write target. An ancestor
    /// segment earlier in the path (e.g. <c>{accountIdentifier}</c>) only ever scopes the route to that
    /// ancestor - it accepts the same GUID/ShortGuid forms as <c>{identifier}</c>, but never a batch
    /// list and never a <c>shortName</c> alias. A singleton ancestor segment (none exist in the model
    /// today, but the generator handles one correctly if it ever does) contributes no identifier at
    /// all - just its own property-named path segment, since there's only ever one.
    /// </para>
    /// <para>
    /// Every one of those forms - guid, ShortGuid, batch, shortName, at the leaf, and guid/ShortGuid at
    /// every non-singleton ancestor segment - is indistinguishable as an OpenAPI path template (OpenAPI
    /// has no constraint syntax), so they all collapse into the one path/parameter this generator
    /// emits; <see cref="UmlCoreCarterModuleGenerator"/> is what actually disambiguates them,
    /// registering one literal, constrained route per accepted combination of forms under this same
    /// OpenAPI path - which, for a deeply-nested class, is the cartesian product of every non-singleton
    /// ancestor's own two forms.
    /// </para>
    /// <para>
    /// This generator produces only the <c>paths</c> object. <c>info</c>/<c>servers</c>/<c>tags</c> and
    /// the shared, HTTP-layer <c>components</c> (<c>ProblemDetails</c>, <c>ThingArray</c>,
    /// <c>include-contained</c>/<c>include-referenced</c>, the standard error responses) are hand-
    /// authored in <c>Mycelium.Forge.Common/OpenApi/shared.json</c>, since none of that is derivable
    /// from the model - <see cref="Mycelium.Forge.OpenApiBundler"/> merges the two, plus the generated
    /// component schemas, into one <c>openapi.json</c>.
    /// </para>
    /// </remarks>
    public class UmlCoreOpenApiPathsGenerator : Generator
    {
        /// <summary>
        /// The file name the generated <c>paths</c> document is written to.
        /// </summary>
        public const string PathsFileName = "paths.json";

        private static readonly JsonSerializerOptions WriteOptions = new()
        {
            WriteIndented = true
        };

        /// <summary>
        /// Generates the <c>{"paths": {...}}</c> document and writes it to <paramref name="outputDirectory"/>.
        /// </summary>
        public async Task GenerateAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            var document = BuildPathsDocument(xmiReaderResult);

            await WriteAsync(document.ToJsonString(WriteOptions), outputDirectory, PathsFileName);
        }

        /// <summary>
        /// Builds the <c>{"paths": {...}}</c> document, without necessarily writing it to disk; the
        /// rendered text is returned so it can be diffed against a committed golden file by
        /// <c>ExpectedOutputTestFixture</c>.
        /// </summary>
        public Task<string> GeneratePathsDocumentAsync(XmiReaderResult xmiReaderResult)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);

            return Task.FromResult(BuildPathsDocument(xmiReaderResult).ToJsonString(WriteOptions));
        }

        /// <summary>
        /// The route-generation model for a single class's own segment - the exact name of the
        /// composite property that owns it, whether that property is a singleton or a collection, and
        /// whether it has a <c>shortName</c> alias - independent of where that segment sits in a
        /// containment chain. Shared with <see cref="UmlCoreCarterModuleGenerator"/> so the two
        /// generators can never key a class differently from one another.
        /// </summary>
        /// <param name="Class">The class this route addresses.</param>
        /// <param name="CollectionName">The owning composite property's own name, verbatim - never
        /// derived from <see cref="Class"/>'s own name, never pluralized.</param>
        /// <param name="HasShortName">Whether this class (directly or via a generalization ancestor)
        /// owns a property literally named <c>shortName</c>, and so gets a second item path keyed by
        /// it, alongside <c>{identifier}</c>. Never true when <see cref="IsSingleton"/> is true - a
        /// singleton has no item sub-path at all.</param>
        /// <param name="IsSingleton">Whether the owning composite property's upper multiplicity bound
        /// is 1 - always exactly one, so this class gets no <c>List</c>/<c>POST</c>/
        /// <c>{identifier}</c>/<c>{shortName}</c>, only <c>GET</c>/<c>PUT</c>/<c>PATCH</c>/
        /// <c>DELETE</c> directly at its own property-named path.</param>
        public sealed record ClassRoute(IClass Class, string CollectionName, bool HasShortName, bool IsSingleton);

        /// <summary>
        /// Every concrete class in <paramref name="xmiReaderResult"/> that gets its own routes -
        /// every concrete class, <c>Forge</c> excepted (it is the one singleton, handled separately) -
        /// in the same order the generator itself walks them. Exposed for tests that need to assert on
        /// the set itself rather than the rendered JSON.
        /// </summary>
        public static IReadOnlyList<IClass> QueryRoutableClasses(XmiReaderResult xmiReaderResult)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);

            return QueryAllConcreteClasses(xmiReaderResult).Where(c => c.Name != "Forge").ToList();
        }

        /// <summary>
        /// Every containment chain <paramref name="class"/> is reachable through, root-first, each
        /// chain ending in <paramref name="class"/> itself. A class with no composite owner other than
        /// <c>Forge</c> has exactly one, single-element chain (itself, top-level). A class whose
        /// composite owner is abstract - <c>Scope</c>, whose only concrete subtypes are
        /// <c>Account</c>/<c>Organization</c> - branches into one chain per concrete subtype, each
        /// chain then extended the same way through that subtype's own ancestry, however deep.
        /// </summary>
        public static IReadOnlyList<IReadOnlyList<IClass>> QueryOwnerChains(XmiReaderResult xmiReaderResult, IClass @class)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(@class);

            return BuildOwnerChains(@class, QueryAllClasses(xmiReaderResult));
        }

        /// <summary>
        /// The path segment naming an ancestor's own identifier within a nested route - e.g.
        /// <c>accountIdentifier</c> for an <c>Account</c> ancestor - distinct from the leaf's own
        /// <c>identifier</c> segment so a multi-level path never repeats a parameter name (ASP.NET Core
        /// requires every route parameter name in a template to be unique). Named after the ancestor's
        /// own class, not the property that owns it - it identifies "which Account", not "which
        /// property got us here".
        /// </summary>
        public static string AncestorParameterName(IClass ownerClass)
        {
            ArgumentNullException.ThrowIfNull(ownerClass);

            return $"{ToLowerCamelCase(ownerClass.Name)}Identifier";
        }

        /// <summary>
        /// The composite property that owns <paramref name="class"/>, and whether that property is a
        /// singleton (upper bound 1) or a collection - the single source of truth
        /// <see cref="BuildClassRoute"/> and the ancestor-segment builders both read from, so a class's
        /// route shape can never disagree with itself between the leaf and ancestor positions.
        /// </summary>
        private sealed record CompositeOwner(IClass DeclaringClass, IProperty Property);

        private static JsonObject BuildPathsDocument(XmiReaderResult xmiReaderResult)
        {
            var paths = new JsonObject();

            BuildForgeSingletonPaths(paths);

            foreach (var @class in QueryRoutableClasses(xmiReaderResult).OrderBy(c => c.Name))
            {
                var chains = QueryOwnerChains(xmiReaderResult, @class)
                    .OrderBy(chain => string.Join('/', chain.Select(c => c.Name)));

                foreach (var chain in chains)
                {
                    BuildCollectionPaths(paths, chain, xmiReaderResult);
                }
            }

            return new JsonObject { ["paths"] = paths };
        }

        private static IReadOnlyList<IReadOnlyList<IClass>> BuildOwnerChains(IClass @class, IReadOnlyList<IClass> allClasses)
        {
            var owner = FindCompositeOwner(@class, allClasses);

            if (owner == null || owner.DeclaringClass.Name == "Forge")
            {
                return [[@class]];
            }

            var chains = new List<IReadOnlyList<IClass>>();

            foreach (var concreteOwner in QueryConcreteSubclasses(owner.DeclaringClass, allClasses))
            {
                foreach (var ownerChain in BuildOwnerChains(concreteOwner, allClasses))
                {
                    chains.Add([.. ownerChain, @class]);
                }
            }

            return chains;
        }

        /// <summary>
        /// The property that composite-owns <paramref name="target"/>, and the class that
        /// <em>declares</em> it - which may itself be abstract (e.g. <c>Scope</c> declares
        /// <c>ownedPackage</c>/<c>address</c>/<c>profileLink</c>, not <c>Account</c> or
        /// <c>Organization</c>). Searches <see cref="IClass.OwnedAttribute"/> (own-declared properties
        /// only, not inherited) across every class, abstract included, so the declaring class is found
        /// even when it's an abstract superclass rather than one of its concrete subtypes.
        /// </summary>
        private static CompositeOwner? FindCompositeOwner(IClass target, IReadOnlyList<IClass> allClasses)
        {
            foreach (var candidate in allClasses)
            {
                var property = candidate.OwnedAttribute.FirstOrDefault(p => p.IsComposite && p.Type == target);

                if (property != null)
                {
                    return new CompositeOwner(candidate, property);
                }
            }

            return null;
        }

        /// <summary>
        /// <paramref name="class"/> itself if it's concrete, otherwise every concrete class that
        /// (directly or transitively) specializes it - <c>Scope</c> -&gt; <c>Account</c>,
        /// <c>Organization</c>.
        /// </summary>
        private static IReadOnlyList<IClass> QueryConcreteSubclasses(IClass @class, IReadOnlyList<IClass> allClasses)
        {
            if (!@class.IsAbstract)
            {
                return [@class];
            }

            return allClasses
                .Where(c => c.Generalization.Select(g => g.General).OfType<IClass>().Contains(@class))
                .SelectMany(c => QueryConcreteSubclasses(c, allClasses))
                .Distinct()
                .ToList();
        }

        private static IReadOnlyList<IClass> QueryAllClasses(XmiReaderResult xmiReaderResult)
        {
            var classes = new List<IClass>();

            foreach (var package in xmiReaderResult.Packages)
            {
                foreach (var containedPackage in package.QueryPackages())
                {
                    classes.AddRange(containedPackage.PackagedElement.OfType<IClass>());
                }
            }

            return classes.Distinct().OrderBy(c => c.Name).ToList();
        }

        private static IReadOnlyList<IClass> QueryAllConcreteClasses(XmiReaderResult xmiReaderResult)
        {
            return QueryAllClasses(xmiReaderResult).Where(c => !c.IsAbstract).ToList();
        }

        private static void BuildForgeSingletonPaths(JsonObject paths)
        {
            paths["/forge"] = new JsonObject
            {
                ["get"] = new JsonObject
                {
                    ["tags"] = new JsonArray("Forge"),
                    ["operationId"] = "GetForge",
                    ["summary"] = "The Forge singleton itself.",
                    ["parameters"] = new JsonArray(
                        new JsonObject { ["$ref"] = "#/components/parameters/IncludeContained" },
                        new JsonObject { ["$ref"] = "#/components/parameters/IncludeReferenced" }),
                    ["responses"] = new JsonObject
                    {
                        ["200"] = new JsonObject { ["$ref"] = "#/components/responses/ThingArray" }
                    }
                },
                ["patch"] = new JsonObject
                {
                    ["tags"] = new JsonArray("Forge"),
                    ["operationId"] = "UpdateForge",
                    ["summary"] = "Partially update the Forge singleton's own properties.",
                    ["requestBody"] = new JsonObject { ["$ref"] = "#/components/requestBodies/PartialThing" },
                    ["responses"] = new JsonObject
                    {
                        ["200"] = new JsonObject { ["$ref"] = "#/components/responses/ThingArray" },
                        ["400"] = new JsonObject { ["$ref"] = "#/components/responses/BadRequest" }
                    }
                }
            };
        }

        /// <summary>
        /// One ancestor segment, fully resolved: its own property-named path segment, and - unless
        /// it's a singleton, which needs none - the <c>{xIdentifier}</c> parameter that selects which
        /// one.
        /// </summary>
        private sealed record AncestorSegment(IClass Class, string CollectionName, bool IsSingleton, string? ParameterName);

        private static IReadOnlyList<AncestorSegment> BuildAncestorSegments(IReadOnlyList<IClass> ancestors, XmiReaderResult xmiReaderResult)
        {
            return ancestors.Select(a =>
            {
                var route = BuildClassRoute(a, xmiReaderResult);
                return new AncestorSegment(a, route.CollectionName, route.IsSingleton, route.IsSingleton ? null : AncestorParameterName(a));
            }).ToList();
        }

        private static void BuildCollectionPaths(JsonObject paths, IReadOnlyList<IClass> chain, XmiReaderResult xmiReaderResult)
        {
            var leaf = chain[^1];
            var ancestorClasses = chain.Take(chain.Count - 1).ToList();
            var ancestors = BuildAncestorSegments(ancestorClasses, xmiReaderResult);
            var route = BuildClassRoute(leaf, xmiReaderResult);
            var tag = leaf.Name;
            var pathPrefix = string.Concat(ancestors.Select(a => a.IsSingleton ? $"/{a.CollectionName}" : $"/{a.CollectionName}/{{{a.ParameterName}}}"));
            var operationIdPrefix = string.Concat(ancestorClasses.Select(a => a.Name.CapitalizeFirstLetter()));
            var itemPath = $"{pathPrefix}/{route.CollectionName}";
            var scopeNote = ancestorClasses.Count > 0 ? $" Scoped to a single {ancestorClasses[^1].Name}." : string.Empty;
            var compositeChildNames = QueryCompositeChildNames(leaf);

            var createDescription = compositeChildNames.Count > 0
                ? $"The request body is a flat array of the {leaf.Name} and any composite children being created with it in the same call (e.g. its {string.Join('/', compositeChildNames)}), mirroring the response wire format."
                : $"The request body is a flat array containing the {leaf.Name} being created, mirroring the response wire format.";

            var getSummary = compositeChildNames.Count > 0
                ? $"A single {leaf.Name}. Its composite children ({string.Join(", ", compositeChildNames)}) are also reachable directly, in their own right - include-contained reaches them here too, as sibling nodes in the response array."
                : $"A single {leaf.Name}.";

            var putSummary = compositeChildNames.Count > 0
                ? $"Create or replace {Article(leaf.Name)} {leaf.Name}, atomically with any composite children in the same call (e.g. its {string.Join('/', compositeChildNames)})."
                : $"Create or replace {Article(leaf.Name)} {leaf.Name}.";

            var patchSummary = compositeChildNames.Count > 0
                ? $"Partially update {Article(leaf.Name)} {leaf.Name}, or add/update one of its composite children ({string.Join(", ", compositeChildNames)}) as an additional array element, cross-referenced by @id. Not yet decided: how to express removing a composite child this way (JSON Merge Patch has no per-element delete for a nested collection)."
                : $"Partially update {Article(leaf.Name)} {leaf.Name}'s own properties.";

            if (route.IsSingleton)
            {
                BuildSingletonPath(paths, leaf, tag, itemPath, operationIdPrefix, ancestors, getSummary, putSummary, patchSummary);
                return;
            }

            var listParameters = new JsonArray();
            foreach (var p in BuildAncestorParameterObjects(ancestors))
            {
                listParameters.Add(p);
            }

            listParameters.Add(new JsonObject { ["$ref"] = "#/components/parameters/IncludeContained" });
            listParameters.Add(new JsonObject { ["$ref"] = "#/components/parameters/IncludeReferenced" });

            var getOperation = new JsonObject
            {
                ["tags"] = new JsonArray(tag),
                ["operationId"] = $"List{operationIdPrefix}{route.CollectionName.CapitalizeFirstLetter()}",
                ["summary"] = $"The {leaf.Name} collection.{scopeNote}",
                ["parameters"] = listParameters,
                ["responses"] = new JsonObject
                {
                    ["200"] = new JsonObject { ["$ref"] = "#/components/responses/ThingArray" }
                }
            };

            var postOperation = new JsonObject
            {
                ["tags"] = new JsonArray(tag),
                ["operationId"] = $"Create{operationIdPrefix}{leaf.Name.CapitalizeFirstLetter()}",
                ["summary"] = $"Add {Article(leaf.Name)} {leaf.Name}.{scopeNote}",
                ["description"] = createDescription,
                ["requestBody"] = new JsonObject { ["$ref"] = "#/components/requestBodies/ThingArray" },
                ["responses"] = new JsonObject
                {
                    ["201"] = new JsonObject { ["$ref"] = "#/components/responses/ThingArray" },
                    ["400"] = new JsonObject { ["$ref"] = "#/components/responses/BadRequest" },
                    ["409"] = new JsonObject { ["$ref"] = "#/components/responses/Conflict" }
                }
            };

            if (ancestors.Count > 0)
            {
                var postParameters = new JsonArray();
                foreach (var p in BuildAncestorParameterObjects(ancestors))
                {
                    postParameters.Add(p);
                }

                postOperation["parameters"] = postParameters;
            }

            paths[itemPath] = new JsonObject { ["get"] = getOperation, ["post"] = postOperation };

            BuildItemPaths(paths, leaf, tag, $"{itemPath}/{{identifier}}", "#/components/parameters/Identifier", operationIdPrefix, string.Empty, ancestors, getSummary, putSummary, patchSummary, includeWrites: true);

            if (route.HasShortName)
            {
                BuildItemPaths(paths, leaf, tag, $"{itemPath}/{{shortName}}", "#/components/parameters/ShortName", operationIdPrefix, "ByShortName", ancestors, getSummary, putSummary, patchSummary, includeWrites: false);
            }
        }

        /// <summary>
        /// A singleton composite child - upper multiplicity 1 - gets no <c>List</c>/<c>POST</c>, no
        /// <c>{identifier}</c>/<c>{shortName}</c> sub-path: there's always exactly one, and the parent
        /// context already addresses it. <c>GET</c>/<c>PUT</c>/<c>PATCH</c>/<c>DELETE</c> operate
        /// directly at <paramref name="path"/> - the same shape <c>/forge</c> itself has, plus writes
        /// (unlike Forge, this is a child that's actually created/replaced/removed through its owner).
        /// </summary>
        private static void BuildSingletonPath(JsonObject paths, IClass @class, string tag, string path, string operationIdPrefix, IReadOnlyList<AncestorSegment> ancestors, string getSummary, string putSummary, string patchSummary)
        {
            JsonArray Parameters(bool includeIncludeParams)
            {
                var array = new JsonArray();

                foreach (var p in BuildAncestorParameterObjects(ancestors))
                {
                    array.Add(p);
                }

                if (includeIncludeParams)
                {
                    array.Add(new JsonObject { ["$ref"] = "#/components/parameters/IncludeContained" });
                    array.Add(new JsonObject { ["$ref"] = "#/components/parameters/IncludeReferenced" });
                }

                return array;
            }

            paths[path] = new JsonObject
            {
                ["get"] = new JsonObject
                {
                    ["tags"] = new JsonArray(tag),
                    ["operationId"] = $"Get{operationIdPrefix}{@class.Name.CapitalizeFirstLetter()}",
                    ["summary"] = getSummary,
                    ["parameters"] = Parameters(includeIncludeParams: true),
                    ["responses"] = new JsonObject
                    {
                        ["200"] = new JsonObject { ["$ref"] = "#/components/responses/ThingArray" },
                        ["404"] = new JsonObject { ["$ref"] = "#/components/responses/NotFound" }
                    }
                },
                ["put"] = new JsonObject
                {
                    ["tags"] = new JsonArray(tag),
                    ["operationId"] = $"Set{operationIdPrefix}{@class.Name.CapitalizeFirstLetter()}",
                    ["summary"] = putSummary,
                    ["parameters"] = Parameters(includeIncludeParams: false),
                    ["requestBody"] = new JsonObject { ["$ref"] = "#/components/requestBodies/ThingArray" },
                    ["responses"] = new JsonObject
                    {
                        ["200"] = new JsonObject { ["$ref"] = "#/components/responses/ThingArray" },
                        ["201"] = new JsonObject { ["$ref"] = "#/components/responses/ThingArray" },
                        ["400"] = new JsonObject { ["$ref"] = "#/components/responses/BadRequest" },
                        ["409"] = new JsonObject { ["$ref"] = "#/components/responses/Conflict" }
                    }
                },
                ["patch"] = new JsonObject
                {
                    ["tags"] = new JsonArray(tag),
                    ["operationId"] = $"Update{operationIdPrefix}{@class.Name.CapitalizeFirstLetter()}",
                    ["summary"] = patchSummary,
                    ["parameters"] = Parameters(includeIncludeParams: false),
                    ["requestBody"] = new JsonObject { ["$ref"] = "#/components/requestBodies/PartialThing" },
                    ["responses"] = new JsonObject
                    {
                        ["200"] = new JsonObject { ["$ref"] = "#/components/responses/ThingArray" },
                        ["400"] = new JsonObject { ["$ref"] = "#/components/responses/BadRequest" },
                        ["404"] = new JsonObject { ["$ref"] = "#/components/responses/NotFound" }
                    }
                },
                ["delete"] = new JsonObject
                {
                    ["tags"] = new JsonArray(tag),
                    ["operationId"] = $"Delete{operationIdPrefix}{@class.Name.CapitalizeFirstLetter()}",
                    ["summary"] = $"Delete {Article(@class.Name)} {@class.Name}.",
                    ["parameters"] = Parameters(includeIncludeParams: false),
                    ["responses"] = new JsonObject
                    {
                        ["204"] = new JsonObject { ["description"] = "Deleted." },
                        ["404"] = new JsonObject { ["$ref"] = "#/components/responses/NotFound" },
                        ["409"] = new JsonObject { ["$ref"] = "#/components/responses/Conflict" }
                    }
                }
            };
        }

        /// <summary>
        /// Builds one item path. <paramref name="includeWrites"/> is <see langword="false"/> for the
        /// <c>{shortName}</c> path: writes only ever go through <c>{identifier}</c>, so
        /// <c>shortName</c> is a read-only alias.
        /// </summary>
        private static void BuildItemPaths(JsonObject paths, IClass @class, string tag, string itemPath, string keyParameterRef, string operationIdPrefix, string operationIdSuffix, IReadOnlyList<AncestorSegment> ancestors, string getSummary, string putSummary, string patchSummary, bool includeWrites)
        {
            var getParameters = new JsonArray();
            foreach (var p in BuildAncestorParameterObjects(ancestors))
            {
                getParameters.Add(p);
            }

            getParameters.Add(new JsonObject { ["$ref"] = keyParameterRef });
            getParameters.Add(new JsonObject { ["$ref"] = "#/components/parameters/IncludeContained" });
            getParameters.Add(new JsonObject { ["$ref"] = "#/components/parameters/IncludeReferenced" });

            var operations = new JsonObject
            {
                ["get"] = new JsonObject
                {
                    ["tags"] = new JsonArray(tag),
                    ["operationId"] = $"Get{operationIdPrefix}{@class.Name.CapitalizeFirstLetter()}{operationIdSuffix}",
                    ["summary"] = getSummary,
                    ["parameters"] = getParameters,
                    ["responses"] = new JsonObject
                    {
                        ["200"] = new JsonObject { ["$ref"] = "#/components/responses/ThingArray" },
                        ["404"] = new JsonObject { ["$ref"] = "#/components/responses/NotFound" }
                    }
                }
            };

            if (includeWrites)
            {
                JsonArray WriteParameters()
                {
                    var array = new JsonArray();
                    foreach (var p in BuildAncestorParameterObjects(ancestors))
                    {
                        array.Add(p);
                    }

                    array.Add(new JsonObject { ["$ref"] = keyParameterRef });
                    return array;
                }

                operations["put"] = new JsonObject
                {
                    ["tags"] = new JsonArray(tag),
                    ["operationId"] = $"Set{operationIdPrefix}{@class.Name.CapitalizeFirstLetter()}{operationIdSuffix}",
                    ["summary"] = putSummary,
                    ["parameters"] = WriteParameters(),
                    ["requestBody"] = new JsonObject { ["$ref"] = "#/components/requestBodies/ThingArray" },
                    ["responses"] = new JsonObject
                    {
                        ["200"] = new JsonObject { ["$ref"] = "#/components/responses/ThingArray" },
                        ["201"] = new JsonObject { ["$ref"] = "#/components/responses/ThingArray" },
                        ["400"] = new JsonObject { ["$ref"] = "#/components/responses/BadRequest" },
                        ["409"] = new JsonObject { ["$ref"] = "#/components/responses/Conflict" }
                    }
                };

                operations["patch"] = new JsonObject
                {
                    ["tags"] = new JsonArray(tag),
                    ["operationId"] = $"Update{operationIdPrefix}{@class.Name.CapitalizeFirstLetter()}{operationIdSuffix}",
                    ["summary"] = patchSummary,
                    ["parameters"] = WriteParameters(),
                    ["requestBody"] = new JsonObject { ["$ref"] = "#/components/requestBodies/PartialThing" },
                    ["responses"] = new JsonObject
                    {
                        ["200"] = new JsonObject { ["$ref"] = "#/components/responses/ThingArray" },
                        ["400"] = new JsonObject { ["$ref"] = "#/components/responses/BadRequest" },
                        ["404"] = new JsonObject { ["$ref"] = "#/components/responses/NotFound" }
                    }
                };

                operations["delete"] = new JsonObject
                {
                    ["tags"] = new JsonArray(tag),
                    ["operationId"] = $"Delete{operationIdPrefix}{@class.Name.CapitalizeFirstLetter()}{operationIdSuffix}",
                    ["summary"] = $"Delete {Article(@class.Name)} {@class.Name}.",
                    ["parameters"] = WriteParameters(),
                    ["responses"] = new JsonObject
                    {
                        ["204"] = new JsonObject { ["description"] = "Deleted." },
                        ["404"] = new JsonObject { ["$ref"] = "#/components/responses/NotFound" },
                        ["409"] = new JsonObject { ["$ref"] = "#/components/responses/Conflict" }
                    }
                };
            }

            paths[itemPath] = operations;
        }

        /// <summary>
        /// One inline (not <c>$ref</c>'d - there's no single shared component that would fit every
        /// possible ancestor class name) <c>in: path</c> parameter object per non-singleton ancestor,
        /// root-first - a singleton ancestor contributes no parameter at all, since there's only ever
        /// one. Returns freshly-constructed <see cref="JsonObject"/> instances on every call, since a
        /// <see cref="JsonNode"/> can only ever belong to one parent - callers that need the same
        /// ancestor parameters in more than one operation's <c>parameters</c> array call this again
        /// rather than reusing a previous result.
        /// </summary>
        private static IEnumerable<JsonObject> BuildAncestorParameterObjects(IReadOnlyList<AncestorSegment> ancestors)
        {
            foreach (var ancestor in ancestors.Where(a => !a.IsSingleton))
            {
                yield return new JsonObject
                {
                    ["name"] = ancestor.ParameterName,
                    ["in"] = "path",
                    ["required"] = true,
                    ["schema"] = new JsonObject { ["type"] = "string" },
                    ["description"] = $"The owning {ancestor.Class.Name}'s own identifier - a canonical GUID, or a ShortGuid encoding of it - never a batch list, never its shortName. Scopes this route to the {ancestor.CollectionName} owned by that {ancestor.Class.Name}."
                };
            }
        }

        /// <summary>
        /// The distinct class names <paramref name="class"/> composite-owns, own and inherited (e.g.
        /// <c>Account</c> -&gt; <c>Package</c>, <c>ProfileLink</c>, <c>Address</c>, <c>APIKey</c>,
        /// <c>OrganizationInvitation</c>, <c>PackageInvitation</c>) - empty for a leaf class like
        /// <c>Country</c> that owns no composite children of its own.
        /// </summary>
        public static IReadOnlyList<string> QueryCompositeChildNames(IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            return @class.QueryAllProperties()
                .Where(p => p.IsComposite && p.Type != null)
                .Select(p => p.Type!.Name.CapitalizeFirstLetter())
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }

        /// <summary>
        /// Every routable class (<c>Forge</c> excepted) is composite-owned by exactly one property
        /// somewhere in the model - <see cref="CollectionName"/> is that property's own name,
        /// verbatim, never a class-name-derived guess, and <see cref="IsSingleton"/> reflects its
        /// actual upper multiplicity bound rather than assuming every composite relationship is a
        /// collection. A class is additionally addressed by its own <c>shortName</c> when it (directly
        /// or via a generalization ancestor) owns a property literally named <c>shortName</c> - true
        /// for every <c>Namespace</c> subtype (<c>Account</c>, <c>Organization</c>, <c>Package</c>) -
        /// though never when <see cref="IsSingleton"/> is true, since a singleton has no item sub-path
        /// to alias in the first place.
        /// </summary>
        public static ClassRoute BuildClassRoute(IClass @class, XmiReaderResult xmiReaderResult)
        {
            ArgumentNullException.ThrowIfNull(@class);
            ArgumentNullException.ThrowIfNull(xmiReaderResult);

            var owner = FindCompositeOwner(@class, QueryAllClasses(xmiReaderResult))
                ?? throw new InvalidOperationException($"{@class.Name} has no composite owner - every routable class (Forge excepted) must be composite-owned by exactly one property somewhere in the model.");

            var isSingleton = owner.Property.QueryUpperValue() == 1;

            var hasShortName = !isSingleton && @class.QueryAllProperties()
                .Any(p => p.Name.Equals("shortName", StringComparison.OrdinalIgnoreCase));

            return new ClassRoute(@class, owner.Property.Name, hasShortName, isSingleton);
        }

        /// <summary>
        /// lowerCamelCase, acronym-aware: <c>Package</c> -&gt; <c>package</c>, but <c>APIKey</c> -&gt;
        /// <c>apiKey</c>, not <c>aPIKey</c>. Used only for <see cref="AncestorParameterName"/> - naming
        /// an ancestor's own identifier parameter after its class, which is never itself the model's
        /// own property naming (that's what <see cref="BuildClassRoute"/> reads directly off the
        /// composite property instead).
        /// </summary>
        private static string ToLowerCamelCase(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            var chars = name.ToCharArray();
            var leadingUpperCount = 0;

            while (leadingUpperCount < chars.Length && char.IsUpper(chars[leadingUpperCount]))
            {
                leadingUpperCount++;
            }

            if (leadingUpperCount == 0)
            {
                return name;
            }

            if (leadingUpperCount == chars.Length)
            {
                return name.ToLowerInvariant();
            }

            for (var i = 0; i < leadingUpperCount - 1; i++)
            {
                chars[i] = char.ToLowerInvariant(chars[i]);
            }

            if (leadingUpperCount == 1)
            {
                chars[0] = char.ToLowerInvariant(chars[0]);
            }

            return new string(chars);
        }

        /// <summary>
        /// "a" or "an", chosen by whether <paramref name="name"/> starts with a vowel sound - every
        /// class name in this model starts with a plain letter, so a simple vowel-letter check is
        /// sufficient.
        /// </summary>
        public static string Article(string name)
        {
            return name.Length > 0 && "AEIOUaeiou".Contains(name[0]) ? "an" : "a";
        }
    }
}
