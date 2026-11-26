// ------------------------------------------------------------------------------------------------
// <copyright file="PropertyExtension.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Extensions
{
    using uml4net.Classification;
    using uml4net.CommonStructure;
    using uml4net.Extensions;
    using uml4net.Profiling;
    using uml4net.SimpleClassifiers;
    using uml4net.Values;

    /// <summary>
    /// Extension class for the <see cref="IProperty" /> interface for SQL schema generation.
    /// </summary>
    public static class PropertyExtension
    {
        /// <summary>
        /// A mapping of the known UML / SysML value types to PostgreSQL types.
        /// </summary>
        public static readonly IReadOnlyDictionary<string, string> SqlTypeMapping = new Dictionary<string, string>
        {
            { "Boolean", "boolean" },
            { "Integer", "integer" },
            { "Real", "double precision" },
            { "UnlimitedNatural", "integer" },
            { "String", "text" },
            { "DateTime", "timestamp" },
            { "Date", "date" },
            { "UUID", "uuid" },
            { "Uuid", "uuid" },
            { "URI", "text" },
            { "SemVer", "text" },
            { "Markdown", "text" }
        };

        /// <param name="property">The <see cref="IProperty" /> to assert.</param>
        extension(IProperty property)
        {
            /// <summary>
            /// Asserts that the <see cref="IProperty" /> is an enum type with a default value provided.
            /// </summary>
            /// <returns>True if the <see cref="IProperty" /> has a default value for an enum.</returns>
            public bool QueryIsEnumPropertyWithDefaultValue()
            {
                ArgumentNullException.ThrowIfNull(property);

                if (!property.QueryIsEnum())
                {
                    return false;
                }

                var defaultValue = property.QueryDefaultValueAsString();

                if (defaultValue == "null")
                {
                    return false;
                }

                var valueSpecification = property.DefaultValue.FirstOrDefault();

                if (valueSpecification is IInstanceValue instanceValue)
                {
                    return instanceValue.Instance is IEnumerationLiteral;
                }

                if (valueSpecification is ILiteralString)
                {
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Gets the name of the property based on UML properties.
            /// </summary>
            /// <returns>
            /// The <see cref="IProperty.Name" /> with the first letter lower-cased in case of derived property, upper-cased
            /// otherwise.
            /// </returns>
            public string QueryPropertyNameBasedOnUmlProperties()
            {
                ArgumentNullException.ThrowIfNull(property);

                return property.IsDerived || property.IsDerivedUnion ? property.Name.LowerCaseFirstLetter() : property.Name.CapitalizeFirstLetter();
            }

            /// <summary>
            /// Calculates whether the property needs an attribute on the owning class's SQL table.
            /// </summary>
            /// <returns>True if the SQL table needs an attribute, false otherwise.</returns>
            public bool QueryOwnedAttributeNeedsSqlAttribute()
            {
                ArgumentNullException.ThrowIfNull(property);

            if (property.Type == null || property.QueryIsManyToMany() || property.QueryIsDataType() || property.QueryIsEnumerable())
            {
                return false;
            }

                if (property.Opposite == null || property.Opposite.QueryIsEnumerable() || property.Opposite.Class == null)
                {
                    return true;
                }

                if (property.Lower == 1 && property.QueryUpperValue() == 1)
                {
                    return true;
                }

                return property.Opposite.Lower != 1;
            }

            /// <summary>
            /// Calculates whether a property is a member of a many-to-many relationship or requires a junction table.
            /// </summary>
            /// <returns>True if the property uses a many-to-many junction table, false otherwise.</returns>
            public bool QueryIsManyToMany()
            {
                ArgumentNullException.ThrowIfNull(property);

                if (property.QueryIsMemberOfManyToMany())
                {
                    return true;
                }

                return property.QueryIsEnumerable() && property.Type != null && !property.QueryIsDataType() && !property.IsComposite && property.SubsettedProperty.Count > 0;
            }

            /// <summary>
            /// Calculates whether the opposite property needs an attribute on the SQL table.
            /// </summary>
            /// <returns>True if the SQL table needs an attribute, false otherwise.</returns>
            public bool QueryOppositeAttributeNeedsSqlAttribute()
            {
                ArgumentNullException.ThrowIfNull(property);

                if (property.Type == null || property.QueryIsManyToMany() || property.QueryIsDataType() || property.QueryIsEnumerable() || property.Opposite == null)
                {
                    return false;
                }

                if (property.Opposite.QueryIsEnumerable() || property.Opposite.IsComposite)
                {
                    return true;
                }

                if (property.Opposite.Lower == 1 && property.Opposite.QueryUpperValue() == 1)
                {
                    return false;
                }

                return property.Lower == 1 && property.QueryUpperValue() == 1;
            }

            /// <summary>
            /// Queries the SQL table name for a many-to-many junction table.
            /// </summary>
            /// <returns>A string representation of the junction table name.</returns>
            public string QueryManyToManyTableName()
            {
                ArgumentNullException.ThrowIfNull(property);

                if (!property.QueryIsManyToMany())
                {
                    throw new ArgumentException($"{property.Name} is not a many-to-many property", nameof(property));
                }

                var ownerName = (property.Owner as INamedElement)?.Name ?? property.Namespace?.Name ?? string.Empty;
                var typeName = property.Type?.Name ?? string.Empty;

                return $"{ownerName.ToSnakeCase()}_{property.Name.ToSnakeCase()}__{typeName.ToSnakeCase()}";
            }

            /// <summary>
            /// Queries the SQL table's target property type name for a many-to-many junction table.
            /// </summary>
            /// <returns>The target property type name.</returns>
            public string QueryManyToManyTargetPropertyTypeName()
            {
                ArgumentNullException.ThrowIfNull(property);

                if (!property.QueryIsManyToMany())
                {
                    throw new ArgumentException($"{property.Name} is not a many-to-many property", nameof(property));
                }

                return property.Type?.Name.CapitalizeFirstLetter() ?? string.Empty;
            }

            /// <summary>
            /// Queries the SQL table's target property column name for a many-to-many junction table.
            /// </summary>
            /// <returns>The target property column name.</returns>
            public string QueryManyToManyTargetPropertyName()
            {
                ArgumentNullException.ThrowIfNull(property);

                if (!property.QueryIsManyToMany())
                {
                    throw new ArgumentException($"{property.Name} is not a many-to-many property", nameof(property));
                }

                return $"target{property.QueryManyToManyTargetPropertyTypeName()}".ToSnakeCase();
            }

            /// <summary>
            /// Queries the SQL table's source property type name for a many-to-many junction table.
            /// </summary>
            /// <returns>The source property type name.</returns>
            public string QueryManyToManySourcePropertyTypeName()
            {
                ArgumentNullException.ThrowIfNull(property);

                if (!property.QueryIsManyToMany())
                {
                    throw new ArgumentException($"{property.Name} is not a many-to-many property", nameof(property));
                }

                var ownerName = (property.Owner as INamedElement)?.Name ?? property.Namespace?.Name ?? string.Empty;
                return ownerName.CapitalizeFirstLetter();
            }

            /// <summary>
            /// Queries the SQL table's source property column name for a many-to-many junction table.
            /// </summary>
            /// <returns>The source property column name.</returns>
            public string QueryManyToManySourcePropertyName()
            {
                ArgumentNullException.ThrowIfNull(property);

                if (!property.QueryIsManyToMany())
                {
                    throw new ArgumentException($"{property.Name} is not a many-to-many property", nameof(property));
                }

                return $"source{property.QueryManyToManySourcePropertyTypeName()}".ToSnakeCase();
            }

            /// <summary>
            /// Queries the SQL type name of the <see cref="IProperty" />.
            /// </summary>
            /// <returns>The PostgreSQL data type.</returns>
            public string QuerySqlTypeName()
            {
                ArgumentNullException.ThrowIfNull(property);

                if (property.Type == null)
                {
                    return string.Empty;
                }

                if (property.QueryIsDataType())
                {
                    return property.QueryIsEnum()
                        ? "text"
                        : SqlTypeMapping.GetValueOrDefault(property.Type.Name, "text");
                }

                return property.QueryIsEnumerable() ? "[uuid]" : "uuid";
            }

            /// <summary>
            /// Gets the SQL attribute (column) name for this property.
            /// </summary>
            /// <returns>The SQL attribute name in snake_case.</returns>
            public string QuerySqlAttributeName()
            {
                ArgumentNullException.ThrowIfNull(property);

                return string.IsNullOrWhiteSpace(property.Name)
                    ? string.Empty
                    : property.Name.ToSnakeCase();
            }

            /// <summary>
            /// Checks whether the property is a built-in Thing attribute (id or classKind).
            /// </summary>
            /// <returns>True if the property is id or classKind, false otherwise.</returns>
            public bool IsThingAttribute()
            {
                ArgumentNullException.ThrowIfNull(property);

                return string.Equals(property.Name, "id", StringComparison.OrdinalIgnoreCase)
                       || string.Equals(property.Name, "classKind", StringComparison.OrdinalIgnoreCase);
            }

            /// <summary>
            /// Returns a JSONB select data type suffix that can be used in a SQL select query.
            /// </summary>
            /// <returns>The data type suffix string.</returns>
            public string QueryJsonbSelectDataTypeSuffix()
            {
                ArgumentNullException.ThrowIfNull(property);

                var typeName = property.QuerySqlTypeName();

                if (typeName is "text" or "timestamp" or "" || (property.QueryIsEnumerable() && !property.IsComposite))
                {
                    return string.Empty;
                }

                return $"::{typeName}";
            }

            /// <summary>
            /// Returns a string representation of a type conversion expression for reading a property from an NpgsqlDataReader.
            /// </summary>
            /// <returns>The conversion expression string.</returns>
            public string QueryReadConversion()
            {
                ArgumentNullException.ThrowIfNull(property);

                var propName = property.QuerySqlAttributeName();
                var typeName = property.QuerySqlTypeName();

                if (typeName == "timestamp")
                {
                    return property.QueryIsNullable()
                        ? $"reader[\"{propName}\"] is DBNull ? null : DateTime.Parse((string)reader[\"{propName}\"], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)"
                        : $"DateTime.Parse((string)reader[\"{propName}\"], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)";
                }

                if (typeName == "date")
                {
                    return property.QueryIsNullable()
                        ? $"reader[\"{propName}\"] is DBNull ? null : DateOnly.Parse((string)reader[\"{propName}\"], CultureInfo.InvariantCulture)"
                        : $"DateOnly.Parse((string)reader[\"{propName}\"], CultureInfo.InvariantCulture)";
                }

                if (property.Type is IEnumeration enumerationType)
                {
                    return property.QueryIsEnumerable()
                        ? $"JsonSerializer.Deserialize<List<{enumerationType.Name}>>((string)reader[\"{propName}\"])"
                        : $"{enumerationType.Name}Provider.Parse((string)reader[\"{propName}\"])";
                }

                if (property.QueryIsEnumerable() && !property.IsComposite)
                {
                    return $"JsonSerializer.Deserialize<List<{property.QueryCSharpTypeName()}>>((string)reader[\"{propName}\"])";
                }

                var csharpType = property.QueryCSharpTypeName();
                return $"({csharpType})reader[\"{propName}\"]";
            }

            /// <summary>
            /// Queries whether the <see cref="IProperty" /> represents a primary key based on UML metadata
            /// where both <see cref="IProperty.IsID" /> and <see cref="IMultiplicityElement.IsUnique" /> are true.
            /// </summary>
            /// <returns>True if the property is a primary key, false otherwise.</returns>
            public bool QueryIsPrimaryKey()
            {
                ArgumentNullException.ThrowIfNull(property);

                return property.IsID && property.IsUnique;
            }

            /// <summary>
            /// Queries whether the <see cref="IProperty" /> is indexable based on the Mycelium profile <c>&lt;&lt;index&gt;&gt;</c> stereotype.
            /// </summary>
            /// <returns>True if the property has the <c>Mycelium:index</c> stereotype, false otherwise.</returns>
            public bool QueryIsIndexable()
            {
                ArgumentNullException.ThrowIfNull(property);

                // Model 0.6 replaced the UML isID=true / isUnique=false convention with the <<index>> stereotype from the Mycelium profile.
                return property.QueryStereoTypeApplication("index") != null;
            }

            /*
             * TODO (Mycelium profile <<managed>> & onAction follow-up):
             *
             * Forge model 0.6 introduces the <<managed>> stereotype (ProfileName: "Mycelium", StereoTypeName: "managed")
             * and an accompanying Enterprise Architect tagged value `onAction` to mark system/server-managed properties
             * that clients must not freely supply or overwrite.
             *
             * 1. How uml4net exposes them at generation time:
             *    - <<managed>> is exposed directly on IProperty via `property.QueryStereoTypeApplications()`
             *      (uml4net.Profiling.StereoTypeApplicationExtensions), where ProfileName == "Mycelium" and
             *      StereoTypeName == "managed". Note: this works for both OwnedAttribute properties AND reverse
             *      composite `owner` association ends (e.g. APIKey.owner, OrganizationInvitation.owner,
             *      PackageInvitation.owner).
             *    - `onAction` is NOT currently populated on `StereoTypeApplication.TaggedValues` by uml4net 9.1.0;
             *      instead it lives in the EA XMI extension (`xmiReaderResult.XmiRoot.Extensions`) on:
             *        * `Element.Attributes[].Tags` (for class-owned attributes), matched by `ea_guid` / XMI id
             *        * `Connector.SourceEnd.Tags` (for association ends like `owner`)
             *      Values observed in 0.6.0:
             *        * `onAction="OnCreate"` (11 properties):
             *            Thing.createdAt, APIKey.secretHash, APIKey.owner, Invitation.experiesAt,
             *            OrganizationInvitation.owner, PackageInvitation.owner, PackageVersion.checkSum,
             *            PackageVersion.fileExtension, PackageVersion.publicationDate, PackageVersion.sizeInBytes,
             *            Scope.origin
             *        * `onAction="OnCreate,OnUpdate"` (1 property):
             *            Thing.modifiedAt
             *        * No `onAction` tag (7 <<managed>> properties mutated by dedicated workflows/endpoints):
             *            Account.avatarBlobReference, APIKey.lastUsedAt, APIKey.revokedAt,
             *            Invitation.status, Organization.LogoBlobReference, PackageVersion.downloadCount,
             *            ProfileType.LogoBlobReference
             *
             * 2. Potential codegen / runtime touchpoints to evaluate:
             *    - DtoValidatorHelper: currently hardcodes skipping `CreatedAt` and `ModifiedAt` (ShouldSkipProperty).
             *      We could skip validation for `<<managed>>` properties populated server-side (especially `OnCreate`
             *      when creating DTOs before persistence, or conditionally based on `onAction`), while checking
             *      existing rules like `SecretHash`, `CheckSum`, `DownloadCount`, etc.
             *    - DTOs (`Property.WriteForDTOClass` / `Property.WriteForDTOInterface`): decide whether `<<managed>>`
             *      properties should remain `{ get; set; }` (needed when services/mappers/seeders construct or
             *      clone DTOs, e.g. `AcceptInvitationAsync` mutating `Status` or DB-read mapping) or use restricted
             *      setters / separate server-mutation helpers.
             *    - PermissionService / ServiceBase (`BeforeCreate` / `BeforeUpdate`): enforce immutability or reject
             *      client-supplied changes on `<<managed>>` properties (e.g. preventing callers from modifying
             *      `OnCreate`-only properties during `UpdateAsync`, or restoring server-managed fields from `existing`).
             */
        }
    }
}
