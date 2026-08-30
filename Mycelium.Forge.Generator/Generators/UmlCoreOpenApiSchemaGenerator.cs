// ------------------------------------------------------------------------------------------------
// <copyright file="UmlCoreOpenApiSchemaGenerator.cs" company="Starion Group S.A.">
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
    /// Generates OpenAPI 3.1 (JSON Schema 2020-12) component schemas from the same Enterprise
    /// Architect model that already drives <see cref="UmlCoreDtoGenerator"/> (DD-07) and
    /// <see cref="UmlCoreJsonDtoSerializerGenerator"/> (DD-05), so a model change updates the
    /// OpenAPI spec's types the same way it already updates the DTOs.
    /// </summary>
    /// <remarks>
    /// One schema file per <see cref="IClass"/> in the model (abstract classes included, since a
    /// concrete class's schema composes its ancestors' schemas via <c>allOf</c>), plus
    /// <see cref="ThingReferenceFileName"/> (the single, generic reference-stub schema every
    /// relationship property points at) and <see cref="ConcreteThingUnionFileName"/> (the
    /// <c>oneOf</c>/<c>discriminator</c> union over every concrete class, for describing "what can
    /// appear in a response array"). Every generated file is a standalone JSON Schema document that
    /// cross-references its siblings with plain relative <c>$ref</c>s - resolving those into
    /// <c>#/components/schemas/...</c> refs inside one bundled <c>openapi.json</c> is a later,
    /// separate step, not this generator's concern.
    /// </remarks>
    public class UmlCoreOpenApiSchemaGenerator : Generator
    {
        /// <summary>
        /// The file name of the generic reference-stub schema (<c>{"@id": ..., "@type": ...}</c>)
        /// every reference property points at, regardless of what it targets.
        /// </summary>
        public const string ThingReferenceFileName = "ThingReference.schema.json";

        /// <summary>
        /// The file name of the <c>oneOf</c>/<c>discriminator</c> union over every concrete class in
        /// the model.
        /// </summary>
        public const string ConcreteThingUnionFileName = "ConcreteThing.schema.json";

        /// <summary>
        /// The UML/SysML primitive DataType names mapped to their JSON Schema representation.
        /// Mirrors <see cref="Extensions.PropertyExtension.SqlTypeMapping"/>'s role for the SQL
        /// schema generator, but targeting JSON Schema types/formats instead of PostgreSQL types.
        /// </summary>
        private static readonly IReadOnlyDictionary<string, JsonObject> ScalarTypeMapping = new Dictionary<string, JsonObject>
        {
            { "Boolean", new JsonObject { ["type"] = "boolean" } },
            { "Integer", new JsonObject { ["type"] = "integer" } },
            { "Real", new JsonObject { ["type"] = "number" } },
            { "UnlimitedNatural", new JsonObject { ["type"] = "integer" } },
            { "String", new JsonObject { ["type"] = "string" } },
            { "DateTime", new JsonObject { ["type"] = "string", ["format"] = "date-time" } },
            { "Date", new JsonObject { ["type"] = "string", ["format"] = "date" } },
            { "UUID", new JsonObject { ["type"] = "string", ["format"] = "uuid" } },
            { "Uuid", new JsonObject { ["type"] = "string", ["format"] = "uuid" } },
            { "URI", new JsonObject { ["type"] = "string", ["format"] = "uri" } },
            { "SemVer", new JsonObject { ["type"] = "string" } }
        };

        private static readonly JsonSerializerOptions WriteOptions = new()
        {
            WriteIndented = true
        };

        /// <summary>
        /// Generates every class schema, <see cref="ThingReferenceFileName"/> and
        /// <see cref="ConcreteThingUnionFileName"/>, and writes each to <paramref name="outputDirectory"/>.
        /// </summary>
        /// <param name="xmiReaderResult">
        /// the <see cref="XmiReaderResult"/> that contains the UML model to generate from
        /// </param>
        /// <param name="outputDirectory">
        /// The target <see cref="DirectoryInfo"/>
        /// </param>
        public async Task GenerateAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            var classes = QueryAllClasses(xmiReaderResult);

            foreach (var @class in classes)
            {
                var schema = this.BuildClassSchema(@class);

                await WriteAsync(Serialize(schema), outputDirectory, ClassSchemaFileName(@class));
            }

            await WriteAsync(Serialize(BuildThingReferenceSchema()), outputDirectory, ThingReferenceFileName);
            await WriteAsync(Serialize(this.BuildConcreteThingUnionSchema(classes)), outputDirectory, ConcreteThingUnionFileName);
        }

        /// <summary>
        /// Generates the schema for a single, named <see cref="IClass"/>, without necessarily writing
        /// it to disk; the rendered text is returned so it can be diffed against a committed golden
        /// file by <c>ExpectedOutputTestFixture</c>.
        /// </summary>
        public Task<string> GenerateClassSchemaAsync(XmiReaderResult xmiReaderResult, string className)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentException.ThrowIfNullOrWhiteSpace(className);

            var @class = QueryAllClasses(xmiReaderResult).Single(x => x.Name == className);

            return Task.FromResult(Serialize(this.BuildClassSchema(@class)));
        }

        /// <summary>
        /// Generates the <see cref="ConcreteThingUnionFileName"/> document, without necessarily
        /// writing it to disk.
        /// </summary>
        public Task<string> GenerateConcreteThingUnionSchemaAsync(XmiReaderResult xmiReaderResult)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);

            return Task.FromResult(Serialize(this.BuildConcreteThingUnionSchema(QueryAllClasses(xmiReaderResult))));
        }

        /// <summary>
        /// The file name a given <see cref="IClass"/>'s schema is written to / referenced by.
        /// </summary>
        public static string ClassSchemaFileName(IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            return $"{@class.Name.CapitalizeFirstLetter()}.schema.json";
        }

        /// <summary>
        /// Walks every top level <see cref="uml4net.Packages.IPackage"/> of the
        /// <paramref name="xmiReaderResult"/> and every package contained (directly or indirectly)
        /// within it, and collects the <see cref="IClass"/>es declared there.
        /// </summary>
        /// <remarks>
        /// Deliberately not shared with <see cref="UmlHandleBarsGenerator.QueryAllClasses"/>: that
        /// method is <c>protected</c> to Handlebars-based generators, and this generator has no
        /// Handlebars dependency (JSON Schema documents are built directly as
        /// <see cref="JsonObject"/> trees - templating nested JSON as strings risks malformed output,
        /// e.g. trailing commas, that a Handlebars template can't catch at compile time).
        /// </remarks>
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

            return classes.OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// Builds the schema for the reference-stub every relationship property points at:
        /// <c>{"@id": {type: string, format: uuid}, "@type": {type: string}}</c>, both required.
        /// </summary>
        private static JsonObject BuildThingReferenceSchema()
        {
            return new JsonObject
            {
                ["type"] = "object",
                ["properties"] = new JsonObject
                {
                    ["@id"] = new JsonObject { ["type"] = "string", ["format"] = "uuid" },
                    ["@type"] = new JsonObject { ["type"] = "string" }
                },
                ["required"] = new JsonArray("@id", "@type")
            };
        }

        /// <summary>
        /// Builds the <c>oneOf</c>/<c>discriminator</c> union listing every concrete class's schema,
        /// disambiguated by <c>@type</c> - "what can appear in a response array".
        /// </summary>
        private JsonObject BuildConcreteThingUnionSchema(IReadOnlyList<IClass> classes)
        {
            var concreteClasses = classes.Where(x => !x.IsAbstract).OrderBy(x => x.Name).ToList();

            var oneOf = new JsonArray();
            var mapping = new JsonObject();

            foreach (var @class in concreteClasses)
            {
                var relativeRef = $"./{ClassSchemaFileName(@class)}";

                oneOf.Add(new JsonObject { ["$ref"] = relativeRef });
                mapping[@class.Name] = relativeRef;
            }

            return new JsonObject
            {
                ["oneOf"] = oneOf,
                ["discriminator"] = new JsonObject
                {
                    ["propertyName"] = "@type",
                    ["mapping"] = mapping
                }
            };
        }

        /// <summary>
        /// Builds one class's schema: the root <c>Thing</c> class gets the universal envelope
        /// (<c>@id</c>, <c>@type</c>, <c>createdAt</c>, <c>modifiedAt</c>); every other class
        /// composes its immediate parent via <c>allOf</c> plus an object fragment for the properties
        /// it declares that its parent doesn't already carry.
        /// </summary>
        private JsonObject BuildClassSchema(IClass @class)
        {
            if (@class.IsThingClass())
            {
                return BuildThingSchema();
            }

            var parent = ImmediateParent(@class);

            var ownFragment = this.BuildOwnPropertiesFragment(@class, parent);

            if (parent == null)
            {
                return ownFragment;
            }

            return new JsonObject
            {
                ["allOf"] = new JsonArray(
                    new JsonObject { ["$ref"] = $"./{ClassSchemaFileName(parent)}" },
                    ownFragment)
            };
        }

        /// <summary>
        /// Builds the <c>Thing</c> root schema itself: the universal envelope every other class's
        /// schema ultimately composes via its <c>allOf</c> chain.
        /// </summary>
        private static JsonObject BuildThingSchema()
        {
            return new JsonObject
            {
                ["type"] = "object",
                ["properties"] = new JsonObject
                {
                    ["@id"] = new JsonObject { ["type"] = "string", ["format"] = "uuid" },
                    ["@type"] = new JsonObject { ["type"] = "string" },
                    ["createdAt"] = new JsonObject { ["type"] = "string", ["format"] = "date-time" },
                    ["modifiedAt"] = new JsonObject { ["type"] = "string", ["format"] = "date-time" }
                },
                ["required"] = new JsonArray("@id", "@type", "createdAt", "modifiedAt")
            };
        }

        /// <summary>
        /// The immediate generalization target of <paramref name="class"/>, or <see langword="null"/>
        /// for the root <c>Thing</c> class. Some classes carry a redundant direct
        /// <see cref="IClass.Generalization"/> edge straight to an indirect ancestor alongside the
        /// real, more specific one (e.g. <c>Package</c> has direct edges to both <c>Namespace</c> and
        /// <c>Thing</c>, even though <c>Namespace</c> itself derives from <c>Thing</c>) - the most
        /// specific candidate is the one no other candidate derives from.
        /// </summary>
        private static IClass? ImmediateParent(IClass @class)
        {
            var generals = @class.Generalization.Select(x => x.General).OfType<IClass>().ToList();

            return generals.SingleOrDefault(candidate =>
                !generals.Any(other => other != candidate && other.QueryDerivesFrom(candidate.Name)));
        }

        /// <summary>
        /// Builds the <c>{"type": "object", "properties": {...}, "required": [...]}</c> fragment for
        /// the properties <paramref name="class"/> declares that <paramref name="parent"/> (its
        /// immediate ancestor, already covered by the <c>allOf</c> chain) does not already carry -
        /// computed by diffing <c>QueryAllProperties</c> between the two, by property identity,
        /// rather than assuming <see cref="IClass.OwnedAttribute"/> alone (a property navigable only
        /// from its association's opposite end - e.g. <c>Account.OwnedPackage</c>, declared from
        /// <c>Package.PackageOwner</c>'s side - is not in <see cref="IClass.OwnedAttribute"/>, but
        /// still needs to appear at the level where it first becomes visible).
        /// </summary>
        private JsonObject BuildOwnPropertiesFragment(IClass @class, IClass? parent)
        {
            var ownProperties = FilteredProperties(@class);

            if (parent != null)
            {
                var parentProperties = new HashSet<IProperty>(FilteredProperties(parent));

                ownProperties = ownProperties.Where(p => !parentProperties.Contains(p)).ToList();
            }

            var properties = new JsonObject();
            var required = new JsonArray();

            foreach (var property in ownProperties.OrderBy(x => x.Name))
            {
                var propertyName = property.Name.LowerCaseFirstLetter();

                properties[propertyName] = this.BuildPropertySchema(property);
                required.Add(propertyName);
            }

            return new JsonObject
            {
                ["type"] = "object",
                ["properties"] = properties,
                ["required"] = required
            };
        }

        /// <summary>
        /// The properties of <paramref name="class"/> - own and inherited - that end up on the wire:
        /// excludes derived properties (never serialized), <c>Thing.Id</c> (surfaced as <c>@id</c>
        /// instead), and a base property that <paramref name="class"/> itself redefines (the
        /// redefinition is what appears, not the property it redefines).
        /// </summary>
        private static List<IProperty> FilteredProperties(IClass @class)
        {
            return @class.QueryAllProperties()
                .Where(p => !p.IsDerived)
                .Where(p => !p.Name.Equals("id", StringComparison.OrdinalIgnoreCase))
                .Where(p => !p.TryQueryRedefinedByProperty(@class, out _))
                .ToList();
        }

        /// <summary>
        /// Builds the JSON Schema fragment for a single own property: a <c>$ref</c> to
        /// <see cref="ThingReferenceFileName"/> for a relationship, the mapped primitive type for a
        /// scalar, wrapped in <c>{"type": "array", "items": ...}</c> when multi-valued, and wrapped
        /// in <c>anyOf: [..., {"type": "null"}]</c> when the property's lower multiplicity bound is
        /// zero - the generated JSON serializer always writes every own property's key (a value or
        /// <c>null</c>, never omitting it), so nullability here is about the value, never about the
        /// key being absent.
        /// </summary>
        private JsonObject BuildPropertySchema(IProperty property)
        {
            var itemSchema = property.QueryIsReferenceType()
                ? new JsonObject { ["$ref"] = $"./{ThingReferenceFileName}" }
                : this.BuildScalarSchema(property);

            var schema = property.QueryIsEnumerable()
                ? new JsonObject { ["type"] = "array", ["items"] = itemSchema }
                : itemSchema;

            if (!property.QueryIsEnumerable() && property.Lower == 0)
            {
                schema = new JsonObject
                {
                    ["anyOf"] = new JsonArray(schema, new JsonObject { ["type"] = "null" })
                };
            }

            return schema;
        }

        /// <summary>
        /// Builds the JSON Schema fragment for a scalar (non-reference) property: an enum property's
        /// literal names as a <c>string</c> enum, a byte-collection item as <c>integer</c> (the
        /// serializer writes each byte as a JSON number, not base64), or the mapped primitive type.
        /// </summary>
        private JsonObject BuildScalarSchema(IProperty property)
        {
            if (property.QueryIsEnum() && property.Type is IEnumeration enumeration)
            {
                var enumValues = new JsonArray();

                foreach (var literal in enumeration.OwnedLiteral)
                {
                    enumValues.Add(literal.Name.CapitalizeFirstLetter());
                }

                return new JsonObject { ["type"] = "string", ["enum"] = enumValues };
            }

            var typeName = property.Type?.Name ?? string.Empty;

            if (typeName == "byte")
            {
                return new JsonObject { ["type"] = "integer" };
            }

            return ScalarTypeMapping.TryGetValue(typeName, out var mapped)
                ? mapped.DeepClone().AsObject()
                : new JsonObject { ["type"] = "string" };
        }

        /// <summary>
        /// Serializes a schema document deterministically, so that re-generating from an unchanged
        /// model produces byte-for-byte identical output - the JSON Schema equivalent of
        /// <see cref="Generator.CodeCleanup"/>'s role for C# output. <see cref="Generator.CodeCleanup"/>
        /// itself is a Roslyn C# formatter and would corrupt JSON, so this generator bypasses it
        /// entirely rather than overriding it.
        /// </summary>
        private static string Serialize(JsonObject schema)
        {
            return schema.ToJsonString(WriteOptions);
        }
    }
}
