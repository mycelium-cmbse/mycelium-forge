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
            { "SemVer", "text" }
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

                if (property.Type == null || property.QueryIsMemberOfManyToMany() || property.QueryIsDataType() || property.QueryIsEnumerable())
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
            /// Calculates whether the opposite property needs an attribute on the SQL table.
            /// </summary>
            /// <returns>True if the SQL table needs an attribute, false otherwise.</returns>
            public bool QueryOppositeAttributeNeedsSqlAttribute()
            {
                ArgumentNullException.ThrowIfNull(property);

                if (property.Type == null || property.QueryIsMemberOfManyToMany() || property.QueryIsDataType() || property.QueryIsEnumerable() || property.Opposite == null)
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

                if (!property.QueryIsMemberOfManyToMany())
                {
                    throw new ArgumentException($"{property.Name} is not a many-to-many property", nameof(property));
                }

                var ownerName = (property.Owner as INamedElement)?.Name ?? property.Namespace?.Name ?? string.Empty;
                var typeName = property.Type?.Name ?? string.Empty;

                return $"{ownerName.CapitalizeFirstLetter()}_{property.Name.LowerCaseFirstLetter()}__{typeName.CapitalizeFirstLetter()}";
            }

            /// <summary>
            /// Queries the SQL table's target property type name for a many-to-many junction table.
            /// </summary>
            /// <returns>The target property type name.</returns>
            public string QueryManyToManyTargetPropertyTypeName()
            {
                ArgumentNullException.ThrowIfNull(property);

                if (!property.QueryIsMemberOfManyToMany())
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

                if (!property.QueryIsMemberOfManyToMany())
                {
                    throw new ArgumentException($"{property.Name} is not a many-to-many property", nameof(property));
                }

                return $"target{property.QueryManyToManyTargetPropertyTypeName()}";
            }

            /// <summary>
            /// Queries the SQL table's source property type name for a many-to-many junction table.
            /// </summary>
            /// <returns>The source property type name.</returns>
            public string QueryManyToManySourcePropertyTypeName()
            {
                ArgumentNullException.ThrowIfNull(property);

                if (!property.QueryIsMemberOfManyToMany())
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

                if (!property.QueryIsMemberOfManyToMany())
                {
                    throw new ArgumentException($"{property.Name} is not a many-to-many property", nameof(property));
                }

                return $"source{property.QueryManyToManySourcePropertyTypeName()}";
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
            /// <returns>The SQL attribute name in lower-camel-case.</returns>
            public string QuerySqlAttributeName()
            {
                ArgumentNullException.ThrowIfNull(property);

                return string.IsNullOrWhiteSpace(property.Name)
                    ? string.Empty
                    : property.Name.LowerCaseFirstLetter();
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

                var propName = property.Name.LowerCaseFirstLetter();
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
            /// Queries whether the <see cref="IProperty" /> is indexable based on UML metadata
            /// where <see cref="IProperty.IsID" /> is true and <see cref="IMultiplicityElement.IsUnique" /> is false.
            /// </summary>
            /// <returns>True if the property is indexable, false otherwise.</returns>
            public bool QueryIsIndexable()
            {
                ArgumentNullException.ThrowIfNull(property);

                return property.IsID && !property.IsUnique;
            }
        }
    }
}
