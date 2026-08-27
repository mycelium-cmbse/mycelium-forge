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

        /// <summary>
        /// Asserts that the <see cref="IProperty" /> is an enum type with a default value provided.
        /// </summary>
        /// <param name="property">The <see cref="IProperty" /> to assert.</param>
        /// <returns>True if the <see cref="IProperty" /> has a default value for an enum.</returns>
        public static bool QueryIsEnumPropertyWithDefaultValue(this IProperty property)
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
        /// <param name="property">The <see cref="IProperty" />.</param>
        /// <returns>
        /// The <see cref="IProperty.Name" /> with the first letter lower-cased in case of derived property, upper-cased
        /// otherwise.
        /// </returns>
        public static string QueryPropertyNameBasedOnUmlProperties(this IProperty property)
        {
            ArgumentNullException.ThrowIfNull(property);

            return property.IsDerived || property.IsDerivedUnion ? property.Name.LowerCaseFirstLetter() : property.Name.CapitalizeFirstLetter();
        }

        /// <summary>
        /// Calculates whether the property needs an attribute on the owning class's SQL table.
        /// </summary>
        /// <param name="property">The subject <see cref="IProperty" />.</param>
        /// <returns>True if the SQL table needs an attribute, false otherwise.</returns>
        public static bool QueryOwnedAttributeNeedsSqlAttribute(this IProperty property)
        {
            ArgumentNullException.ThrowIfNull(property);

            if (property.Type == null || property.QueryIsMemberOfManyToMany() || property.QueryIsDataType())
            {
                return false;
            }

            if (property.QueryIsEnumerable())
            {
                return false;
            }

            if (property.Opposite?.QueryIsEnumerable() ?? true)
            {
                return true;
            }

            if (property.Lower == 1 && property.QueryUpperValue() == 1)
            {
                return true;
            }

            return property.Opposite.Lower != 1 || property.QueryUpperValue() != 1;
        }

        /// <summary>
        /// Calculates whether the opposite property needs an attribute on the SQL table.
        /// </summary>
        /// <param name="property">The subject <see cref="IProperty" />.</param>
        /// <returns>True if the SQL table needs an attribute, false otherwise.</returns>
        public static bool QueryOppositeAttributeNeedsSqlAttribute(this IProperty property)
        {
            ArgumentNullException.ThrowIfNull(property);

            if (property.Type == null || property.QueryIsMemberOfManyToMany() || property.QueryIsDataType())
            {
                return false;
            }

            if (property.QueryIsEnumerable())
            {
                return false;
            }

            if (property.Opposite == null)
            {
                return false;
            }

            if (property.Opposite.QueryIsEnumerable())
            {
                return true;
            }

            if (property.Opposite.IsComposite)
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
        /// <param name="property">The subject <see cref="IProperty" />.</param>
        /// <returns>A string representation of the junction table name.</returns>
        public static string QueryManyToManyTableName(this IProperty property)
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
        /// <param name="property">The subject <see cref="IProperty" />.</param>
        /// <returns>The target property type name.</returns>
        public static string QueryManyToManyTargetPropertyTypeName(this IProperty property)
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
        /// <param name="property">The subject <see cref="IProperty" />.</param>
        /// <returns>The target property column name.</returns>
        public static string QueryManyToManyTargetPropertyName(this IProperty property)
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
        /// <param name="property">The subject <see cref="IProperty" />.</param>
        /// <returns>The source property type name.</returns>
        public static string QueryManyToManySourcePropertyTypeName(this IProperty property)
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
        /// <param name="property">The subject <see cref="IProperty" />.</param>
        /// <returns>The source property column name.</returns>
        public static string QueryManyToManySourcePropertyName(this IProperty property)
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
        /// <param name="property">The subject <see cref="IProperty" />.</param>
        /// <returns>The PostgreSQL data type.</returns>
        public static string QuerySqlTypeName(this IProperty property)
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
        /// <param name="property">The <see cref="IProperty" />.</param>
        /// <returns>The SQL attribute name in lower-camel-case.</returns>
        public static string QuerySqlAttributeName(this IProperty property)
        {
            ArgumentNullException.ThrowIfNull(property);

            return string.IsNullOrWhiteSpace(property.Name)
                ? string.Empty
                : property.Name.LowerCaseFirstLetter();
        }
    }
}
