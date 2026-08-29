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

            return @class.OwnedAttribute
                .Where(x => x.Type != null && x.QueryIsDataType())
                .Where(x => !x.IsID)
                .Where(x => !x.IsDerived && !x.IsDerivedUnion)
                .Where(x => !x.QueryIsEnumerable());
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

            return @class.QueryAllGeneralClassifiers()
                .OfType<IClass>()
                .Where(ancestor => !ancestor.IsThingClass())
                .SelectMany(ancestor => ancestor.QuerySqlIndexableOwnAttributes())
                .DistinctBy(property => property.Name)
                .OrderBy(property => property.Name);
        }
    }
}
