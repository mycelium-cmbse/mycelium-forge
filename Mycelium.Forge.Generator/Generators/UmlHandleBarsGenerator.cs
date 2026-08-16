// ------------------------------------------------------------------------------------------------
// <copyright file="UmlHandleBarsGenerator.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Generators
{
    using uml4net.Extensions;
    using uml4net.SimpleClassifiers;
    using uml4net.StructuredClassifiers;
    using uml4net.xmi.Readers;

    /// <summary>
    /// Abstract super class from which all uml4net based <see cref="HandlebarsDotNet"/> generators
    /// need to derive
    /// </summary>
    /// <remarks>
    /// This class is deliberately model-agnostic: it has no dependency on any specific Mycelium
    /// model package so it can be reused unmodified by future generation pipelines (e.g. F-05, F-10).
    /// </remarks>
    public abstract class UmlHandleBarsGenerator : HandleBarsGenerator
    {
        /// <summary>
        /// Generates code specific to the concrete implementation
        /// </summary>
        /// <param name="xmiReaderResult">
        /// the <see cref="XmiReaderResult"/> that contains the UML model to generate from
        /// </param>
        /// <param name="outputDirectory">
        /// The target <see cref="DirectoryInfo"/>
        /// </param>
        /// <returns>
        /// an awaitable <see cref="Task"/>
        /// </returns>
        public abstract Task GenerateAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory);

        /// <summary>
        /// Gets an optional subfolder location path to locate templates
        /// </summary>
        /// <returns>An optional subfolder name</returns>
        protected override string GetOptionalSubfolderTemplateLocation()
        {
            return "Uml";
        }

        /// <summary>
        /// Walks every top level <see cref="uml4net.Packages.IPackage"/> of the <paramref name="xmiReaderResult"/>
        /// and every <see cref="uml4net.Packages.IPackage"/> contained (directly or indirectly) within it, and
        /// collects the <see cref="IClass"/>es that are declared there.
        /// </summary>
        /// <param name="xmiReaderResult">
        /// the <see cref="XmiReaderResult"/> that contains the UML model to query
        /// </param>
        /// <returns>
        /// the <see cref="IClass"/>es found in the model, ordered by name
        /// </returns>
        protected static IReadOnlyList<IClass> QueryAllClasses(XmiReaderResult xmiReaderResult)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);

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
        /// Walks every top level <see cref="uml4net.Packages.IPackage"/> of the <paramref name="xmiReaderResult"/>
        /// and every <see cref="uml4net.Packages.IPackage"/> contained (directly or indirectly) within it, and
        /// collects the <see cref="IEnumeration"/>s that are declared there.
        /// </summary>
        /// <param name="xmiReaderResult">
        /// the <see cref="XmiReaderResult"/> that contains the UML model to query
        /// </param>
        /// <returns>
        /// the <see cref="IEnumeration"/>s found in the model, ordered by name
        /// </returns>
        protected static IReadOnlyList<IEnumeration> QueryAllEnumerations(XmiReaderResult xmiReaderResult)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);

            var enumerations = new List<IEnumeration>();

            foreach (var package in xmiReaderResult.Packages)
            {
                foreach (var containedPackage in package.QueryPackages())
                {
                    enumerations.AddRange(containedPackage.PackagedElement.OfType<IEnumeration>());
                }
            }

            return enumerations.OrderBy(x => x.Name).ToList();
        }
    }
}
