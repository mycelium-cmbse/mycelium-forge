// ------------------------------------------------------------------------------------------------
// <copyright file="ClassExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Extensions
{
    using uml4net.Classification;
    using uml4net.Extensions;
    using uml4net.StructuredClassifiers;

    /// <summary>
    /// Defines extension methods to the <see cref="IClass" /> interface for SQL schema generation.
    /// </summary>
    public static class ClassExtensions
    {
        /// <summary>
        /// Queries whether the specified <see cref="IClass" /> derives from
        /// another <see cref="IClass" /> with the specified name.
        /// </summary>
        /// <param name="class">The subject <see cref="IClass" /> for which the query is executed.</param>
        /// <param name="derivesFrom">The name of the <see cref="IClass" /> for which the query is executed.</param>
        /// <returns>
        /// true if the <see cref="IClass" /> derives (direct or indirect) from <paramref name="derivesFrom" />,
        /// false if not.
        /// </returns>
        public static bool QueryDerivesFrom(this IClass @class, string derivesFrom)
        {
            ArgumentNullException.ThrowIfNull(@class);
            ArgumentException.ThrowIfNullOrWhiteSpace(derivesFrom);

            return @class.QueryAllGeneralClassifiers()
                .OfType<IClass>()
                .Where(superClass => superClass != @class)
                .Any(superClass => superClass.Name == derivesFrom);
        }

        /// <summary>
        /// Checks all super classes in the class hierarchy up to the top if the class name equals "Thing".
        /// </summary>
        /// <param name="class">The <see cref="IClass" /> to check.</param>
        /// <returns>A value indicating whether the Thing (root) class is present in the hierarchy of super classes.</returns>
        public static bool HasThingClass(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            return @class.QueryAllGeneralClassifiers()
                .OfType<IClass>()
                .Where(superClass => superClass != @class)
                .Any(superClass => superClass.IsThingClass());
        }

        /// <summary>
        /// Checks if the class's name equals "Thing".
        /// </summary>
        /// <param name="class">The <see cref="IClass" /> to check.</param>
        /// <returns>A value indicating whether the class is the Thing (root) class.</returns>
        public static bool IsThingClass(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            return @class.Name == "Thing";
        }

        /// <summary>
        /// Gets the SQL table name for this class.
        /// </summary>
        /// <param name="class">The <see cref="IClass" />.</param>
        /// <returns>The SQL table name.</returns>
        public static string QuerySqlTableName(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            return @class.Name.CapitalizeFirstLetter();
        }

        /// <summary>
        /// Returns all the properties that need a single foreign key reference column in a table based on ownership and/or
        /// cardinality.
        /// </summary>
        /// <param name="class">The class to query single reference properties for.</param>
        /// <returns>A collection of single reference properties.</returns>
        public static IEnumerable<IProperty> QuerySqlSingleReferenceProperties(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            var ownedSingleReferenceProperties = @class.OwnedAttribute
                .Where(x => x.Type != null && !x.QueryIsDataType())
                .Where(x => !x.IsComposite)
                .Where(x => x.QueryOwnedAttributeNeedsSqlAttribute());

            var oppositeSingleReferenceProperties = @class.QueryAllOppositeReferencesToMe()
                .Where(x => x.Opposite != null)
                .Select(x => x.Opposite!)
                .Where(x => x.QueryOppositeAttributeNeedsSqlAttribute());

            var results = ownedSingleReferenceProperties
                .Union(oppositeSingleReferenceProperties)
                .Distinct()
                .OrderBy(x => x.Name)
                .ToArray();

            return results;
        }

        /// <summary>
        /// Queries all the references from all packages to find those associated at any member end with the class.
        /// </summary>
        /// <param name="class">The <see cref="IClass" /> used as a filter.</param>
        /// <returns>All matching <see cref="IProperty" /> references.</returns>
        public static IEnumerable<IProperty> QueryAllOppositeReferencesToMe(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            var rootPackage = @class.QueryRootPackage();

            if (rootPackage == null)
            {
                return [];
            }

            var packages = rootPackage.QueryAllNestedAndImportedPackages();

            return packages.SelectMany(p => p.PackagedElement.OfType<IClass>())
                .SelectMany(x => x.OwnedAttribute)
                .Where(x => x.Type == @class)
                .Distinct();
        }

        /// <summary>
        /// Returns all the properties that are owned and represent a ManyToMany relationship.
        /// </summary>
        /// <param name="class">The class to look for properties on or associations with.</param>
        /// <returns>A list of many-to-many properties.</returns>
        public static IEnumerable<IProperty> QueryOwnedManyToManyProperties(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            var results = @class.OwnedAttribute
                .Where(x => x.QueryIsMemberOfManyToMany())
                .OrderBy(x => x.Name)
                .Distinct()
                .ToList();

            return results;
        }

        /// <summary>
        /// Returns the single-valued, non-derived, non-identifying, primitive-typed attributes this class owns
        /// directly - i.e. the scalar attributes that end up as keys in this class's row's <c>Thing.data</c> JSONB
        /// document and can have an expression index built against them.
        /// </summary>
        /// <param name="class">The class to query owned indexable scalar attributes for.</param>
        /// <returns>A collection of indexable scalar attributes owned directly by this class.</returns>
        public static IEnumerable<IProperty> QuerySqlIndexableOwnAttributes(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            return @class.QuerySqlIndexableOwnAttributesCore(enumerable: false);
        }

        /// <summary>
        /// Returns the single-valued, non-derived, non-identifying, primitive-typed attributes this class owns
        /// or inherits from any non-<c>Thing</c> ancestor - the full set of scalar attributes present in this
        /// class's row's <c>Thing.data</c> JSONB document, excluding the universal <c>Thing</c> attributes
        /// (<c>createdAt</c>/<c>modifiedAt</c>), which get their own shared indexes instead of one per class.
        /// </summary>
        /// <param name="class">The class to query indexable scalar attributes for.</param>
        /// <returns>A collection of indexable scalar attributes, own and inherited, ordered by name.</returns>
        public static IEnumerable<IProperty> QuerySqlIndexableAttributes(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            return @class.QuerySqlIndexableAttributesCore(enumerable: false);
        }

        /// <summary>
        /// Returns the multi-valued (<c>0..*</c>/<c>1..*</c>), non-derived, primitive-typed attributes this class
        /// owns directly - i.e. the attributes that serialize as a JSON array under a key in this class's row's
        /// <c>Thing.data</c> JSONB document, and need a GIN containment index rather than a B-tree expression one.
        /// </summary>
        /// <param name="class">The class to query owned indexable multi-valued attributes for.</param>
        /// <returns>A collection of indexable multi-valued attributes owned directly by this class.</returns>
        public static IEnumerable<IProperty> QuerySqlIndexableOwnMultiValuedAttributes(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            return @class.QuerySqlIndexableOwnAttributesCore(enumerable: true);
        }

        /// <summary>
        /// Returns the multi-valued (<c>0..*</c>/<c>1..*</c>), non-derived, primitive-typed attributes this class
        /// owns or inherits from any non-<c>Thing</c> ancestor.
        /// </summary>
        /// <param name="class">The class to query indexable multi-valued attributes for.</param>
        /// <returns>A collection of indexable multi-valued attributes, own and inherited, ordered by name.</returns>
        public static IEnumerable<IProperty> QuerySqlIndexableMultiValuedAttributes(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            return @class.QuerySqlIndexableAttributesCore(enumerable: true);
        }

        /// <summary>
        /// Shared filter behind <see cref="QuerySqlIndexableOwnAttributes" /> and
        /// <see cref="QuerySqlIndexableOwnMultiValuedAttributes" />: primitive-typed, non-identifying, non-derived
        /// attributes owned directly by this class, split by cardinality.
        /// </summary>
        /// <param name="class">The class to query owned indexable attributes for.</param>
        /// <param name="enumerable">Whether to return the multi-valued attributes rather than the single-valued ones.</param>
        /// <returns>A collection of indexable attributes owned directly by this class.</returns>
        private static IEnumerable<IProperty> QuerySqlIndexableOwnAttributesCore(this IClass @class, bool enumerable)
        {
            return @class.OwnedAttribute
                .Where(x => x.Type != null && x.QueryIsDataType())
                .Where(x => !x.IsID)
                .Where(x => !x.IsDerived && !x.IsDerivedUnion)
                .Where(x => x.QueryIsEnumerable() == enumerable);
        }

        /// <summary>
        /// Shared traversal behind <see cref="QuerySqlIndexableAttributes" /> and
        /// <see cref="QuerySqlIndexableMultiValuedAttributes" />: own-or-inherited indexable attributes from any
        /// non-<c>Thing</c> ancestor, split by cardinality.
        /// </summary>
        /// <param name="class">The class to query indexable attributes for.</param>
        /// <param name="enumerable">Whether to return the multi-valued attributes rather than the single-valued ones.</param>
        /// <returns>A collection of indexable attributes, own and inherited, ordered by name.</returns>
        private static IEnumerable<IProperty> QuerySqlIndexableAttributesCore(this IClass @class, bool enumerable)
        {
            return @class.QueryAllGeneralClassifiers()
                .OfType<IClass>()
                .Where(ancestor => !ancestor.IsThingClass())
                .SelectMany(ancestor => ancestor.QuerySqlIndexableOwnAttributesCore(enumerable))
                .DistinctBy(property => property.Name)
                .OrderBy(property => property.Name);
        }
    }
}
