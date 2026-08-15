// ------------------------------------------------------------------------------------------------
// <copyright file="NamedElementExtensions.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Extensions
{
    using System;
    using System.Linq;

    using uml4net.CommonStructure;
    using uml4net.SimpleClassifiers;

    /// <summary>
    /// Extension class for <see cref="INamedElement"/>
    /// </summary>
    public static class NamedElementExtensions
    {
        /// <summary>
        /// Query the CSharp namespace of a <see cref="INamedElement"/>
        /// </summary>
        /// <param name="namedElement">The <see cref="INamedElement"/></param>
        /// <returns>The CSharp compliant namespace</returns>
        public static string QueryNamespace(this INamedElement namedElement)
        {
            var qualifiedNameSpaces = namedElement.QualifiedName.Split("::");
            var namespaces = qualifiedNameSpaces.Skip(1).Take(qualifiedNameSpaces.Length - 2);
            return string.Join('.', namespaces);
        }
    }
}
