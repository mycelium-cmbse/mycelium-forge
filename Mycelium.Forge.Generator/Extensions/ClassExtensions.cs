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

            return @class.QueryDerivesFrom(ModelConstants.ThingName);
        }

        /// <summary>
        /// Checks if the class's name equals "Thing".
        /// </summary>
        /// <param name="class">The <see cref="IClass" /> to check.</param>
        /// <returns>A value indicating whether the class is the Thing (root) class.</returns>
        public static bool IsThingClass(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            return @class.Name == ModelConstants.ThingName;
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
        /// Returns all the properties that are owned single reference properties on this class.
        /// </summary>
        /// <param name="class">The class to query single reference properties for.</param>
        /// <returns>A collection of owned single reference properties.</returns>
        public static IEnumerable<IProperty> QueryOwnedSingleReferenceProperties(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            return @class.OwnedAttribute
                .Where(x => !x.IsComposite && x.QueryOwnedAttributeNeedsSqlAttribute())
                .Distinct()
                .OrderBy(x => x.Name)
                .ToArray();
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

            var ownedSingleReferenceProperties = @class.QueryOwnedSingleReferenceProperties();
            var oppositeSingleReferenceProperties = @class.QueryOppositeCompositeProperties();

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
        /// Queries all the <see cref="IProperty" /> instances that are owned by the current
        /// <paramref name="class" /> or by a super-class that do not derive from a specific
        /// named <see cref="IClass" /> that is typically at the root of the inheritance tree.
        /// </summary>
        /// <param name="class">The <see cref="IClass" /> for which the properties are queried.</param>
        /// <param name="derivesFrom">The name of the root <see cref="IClass" />.</param>
        /// <returns>A list of <see cref="IProperty" />.</returns>
        public static IReadOnlyList<IProperty> QueryPropertiesThatAreOwnedAndUsableAndInheritedFromDirectNonDerivesFromClasses(this IClass @class, string derivesFrom)
        {
            ArgumentNullException.ThrowIfNull(@class);
            ArgumentException.ThrowIfNullOrWhiteSpace(derivesFrom);

            var properties = @class.SuperClass
                .Where(superClass => !superClass.QueryDerivesFrom(derivesFrom))
                .SelectMany(superClass => superClass.OwnedAttribute)
                .Concat(@class.OwnedAttribute)
                .OrderBy(x => x.Name)
                .ToList();

            return properties;
        }

        /// <summary>
        /// This returns the opposite composite properties for this class.
        /// </summary>
        /// <param name="class">The class to start with.</param>
        /// <returns>The collection of opposite composite properties.</returns>
        /// <remarks>
        /// Resolves reverse composite aggregation relationships across all packages so the child class knows its container owner.
        /// </remarks>
        public static IEnumerable<IProperty> QueryOppositeCompositeProperties(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            var references = @class.QueryAllOppositeReferencesToMe().ToList();

            var results = references
                .Where(x => x.IsComposite)
                .Select(x => x.Opposite)
                .Distinct()
                .ToArray();

            if (results.Contains(null))
            {
                throw new InvalidOperationException("Unexpected null value as opposite property");
            }

            return results;
        }

        /// <summary>
        /// Gets the properties for a DTO interface.
        /// </summary>
        /// <param name="class">The <see cref="IClass" /> to look for properties on or associations with.</param>
        /// <returns>A collection of properties.</returns>
        /// <remarks>
        /// Combines owned attributes with reverse composite owner properties so DTO interfaces declare container ownership.
        /// </remarks>
        public static IEnumerable<IProperty> QueryDtoInterfaceProperties(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            var owned = @class.OwnedAttribute;
            var opposite = @class.QueryOppositeCompositeProperties();

            return owned.Union(opposite).Distinct().OrderBy(property => property.Name);
        }

        /// <summary>
        /// Gets the properties for a DTO class, including all properties from superclasses.
        /// </summary>
        /// <param name="class">The <see cref="IClass" /> to look for properties on or associations with.</param>
        /// <returns>A collection of properties.</returns>
        /// <remarks>
        /// Combines full inheritance hierarchy properties with reverse composite owner properties across superclasses for concrete
        /// DTO classes.
        /// </remarks>
        public static IEnumerable<IProperty> QueryDtoClassProperties(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            var all = @class.QueryAllProperties().ToList();

            List<IProperty> opposite = [];

            foreach (var superClass in @class.QueryAllGeneralClassifiers().OfType<IClass>())
            {
                opposite.AddRange(superClass.QueryOppositeCompositeProperties());
            }

            opposite.AddRange(@class.QueryOppositeCompositeProperties());

            return all.Union(opposite).Distinct().OrderBy(property => property.Name);
        }

        /// <summary>
        /// Queries all general classifiers (superclasses) that derive from the "Thing" root class in reverse hierarchy order.
        /// </summary>
        /// <param name="class">The <see cref="IClass" /> for which the superclasses are queried.</param>
        /// <returns>A list of <see cref="IClass" /> superclasses deriving from Thing in reverse order.</returns>
        public static IReadOnlyList<IClass> QuerySuperClassesDerivingFromThing(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            return @class.QueryAllGeneralClassifiers()
                .OfType<IClass>()
                .Where(x => x.QueryDerivesFrom(ModelConstants.ThingName))
                .Reverse()
                .ToList();
        }

        /// <summary>
        /// Queries the class and all its superclasses that derive from "Thing" or is the "Thing" class in reverse hierarchy order.
        /// </summary>
        /// <param name="class">The <see cref="IClass" /> for which the hierarchy classes are queried.</param>
        /// <returns>A list of <see cref="IClass" /> hierarchy classes in reverse order.</returns>
        public static IReadOnlyList<IClass> QueryThingHierarchyClasses(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            return @class.QueryAllGeneralClassifiers()
                .OfType<IClass>()
                .Where(x => x.QueryDerivesFrom(ModelConstants.ThingName) || x.IsThingClass())
                .Reverse()
                .ToList();
        }

        /// <summary>
        /// Queries all many-to-many properties for a class.
        /// </summary>
        /// <param name="class">The <see cref="IClass" /> to query many-to-many properties for.</param>
        /// <param name="derivesFrom">The root class name to exclude.</param>
        /// <returns>A list of many-to-many <see cref="IProperty" /> instances.</returns>
        public static IReadOnlyList<IProperty> QueryManyToManyProperties(this IClass @class, string derivesFrom = ModelConstants.ThingName)
        {
            ArgumentNullException.ThrowIfNull(@class);
            ArgumentException.ThrowIfNullOrWhiteSpace(derivesFrom);

            return @class
                .QueryPropertiesThatAreOwnedAndUsableAndInheritedFromDirectNonDerivesFromClasses(derivesFrom)
                .Where(x => x.QueryIsMemberOfManyToMany())
                .ToList();
        }
    }
}
