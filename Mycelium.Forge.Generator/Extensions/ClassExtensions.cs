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
    using System.Collections.Immutable;

    using uml4net.Classification;
    using uml4net.Extensions;
    using uml4net.Packages;
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

            foreach (var superClass in @class.SuperClass)
            {
                if (superClass.Name == derivesFrom)
                {
                    return true;
                }

                if (superClass.QueryDerivesFrom(derivesFrom))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks all super classes in the class hierarchy up to the top if the class name equals "Thing".
        /// </summary>
        /// <param name="class">The <see cref="IClass" /> to check.</param>
        /// <returns>A value indicating whether the Thing (root) class is present in the hierarchy of super classes.</returns>
        public static bool HasThingClass(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            foreach (var superClass in @class.SuperClass)
            {
                if (superClass.IsThingClass())
                {
                    return true;
                }

                if (superClass.HasThingClass())
                {
                    return true;
                }
            }

            return false;
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
        /// Returns all the properties that are composite properties indicating that this
        /// class is owned by another class.
        /// </summary>
        /// <param name="class">The class to look for associations with.</param>
        /// <returns>A collection of the opposite composite properties found if any.</returns>
        public static IEnumerable<IProperty> QueryOppositeCompositeProperties(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            var references = @class.QueryAllOppositeReferencesToMe().ToList();

            var results = references
                .Where(x => x.IsComposite)
                .Select(x => x.Opposite)
                .Where(x => x != null)
                .Select(x => x!)
                .Distinct()
                .ToArray();

            return results;
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
                .Where(x => x.QueryIsReferenceProperty())
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
        /// Gets this class and all its superclasses.
        /// </summary>
        /// <param name="class">The <see cref="IClass" />.</param>
        /// <returns>The class and a distinct collection of any superclasses it may have.</returns>
        public static IEnumerable<IClass> QueryAllSuperClasses(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            return GetAllSuperClasses(@class).Distinct();
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

            List<IPackage> packages = [];

            foreach (var package in rootPackage.QueryPackages())
            {
                packages.Add(package);

                foreach (var packageImport in package.PackageImport)
                {
                    if (packageImport.ImportedPackage != null)
                    {
                        packages.Add(packageImport.ImportedPackage);
                    }
                }
            }

            packages = packages.Distinct().ToList();

            return packages.SelectMany(p => p.PackagedElement.OfType<IClass>())
                .SelectMany(x => x.OwnedAttribute)
                .Where(x => x.QueryIsReferenceProperty())
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
                .Where(x => x.QueryIsManyToMany())
                .OrderBy(x => x.Name)
                .Distinct()
                .ToList();

            return results;
        }

        /// <summary>
        /// Returns all non-abstract ancestor classes of the specified <see cref="IClass" />,
        /// ordered from immediate superclass up to the topmost ancestor.
        /// </summary>
        /// <param name="class">The <see cref="IClass" /> whose concrete ancestors to query.</param>
        /// <returns>An immutable list of non-abstract ancestor classes.</returns>
        public static IImmutableList<IClass> QueryConcreteAncestors(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            var result = @class
                .QueryAllGeneralClassifiers()
                .OfType<IClass>()
                .Except([@class])
                .Where(c => !c.IsAbstract)
                .ToImmutableList();

            return result;
        }

        /// <summary>
        /// Checks if a class has a non-abstract class in its inheritance hierarchy.
        /// </summary>
        /// <param name="class">The <see cref="IClass" /> to check.</param>
        /// <returns>True if a concrete base class exists in the hierarchy, false otherwise.</returns>
        public static bool HasConcreteBase(this IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            foreach (var superClass in @class.SuperClass)
            {
                if (!superClass.IsAbstract)
                {
                    return true;
                }

                if (superClass.HasConcreteBase())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Recursively retrieves the class and all its superclasses.
        /// </summary>
        /// <param name="class">The <see cref="IClass" /> to traverse.</param>
        /// <returns>An enumeration of classes in the inheritance hierarchy.</returns>
        private static IEnumerable<IClass> GetAllSuperClasses(IClass @class)
        {
            yield return @class;

            foreach (var superClass in @class.SuperClass)
            {
                foreach (var c in GetAllSuperClasses(superClass))
                {
                    yield return c;
                }
            }
        }
    }
}
