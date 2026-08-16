// ------------------------------------------------------------------------------------------------
// <copyright file="EnumerationHelper.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.HandleBarHelpers
{
    using System.Linq;

    using HandlebarsDotNet;

    using uml4net.SimpleClassifiers;

    /// <summary>
    /// A block helper to support the generation of <see cref="Enumeration" /> and <see cref="EnumerationLiteral" />
    /// </summary>
    public static class EnumerationHelper
    {
        /// <summary>
        /// Registers the <see cref="EnumerationHelper" />
        /// </summary>
        /// <param name="handlebars">
        /// The <see cref="IHandlebars" /> context with which the helper needs to be registered
        /// </param>
        public static void RegisterEnumerationHelper(this IHandlebars handlebars)
        {
            handlebars.RegisterHelper("Enumeration.WriteLengthLongestLiteral", (writer, context, arguments) =>
            {
                if (arguments.Length != 1)
                {
                    throw new HandlebarsException("{{#Enumeration.WriteLengthLongestLiteral}} helper must have exactly one argument");
                }

                if (arguments.Single() is not Enumeration enumeration)
                {
                    throw new HandlebarsException("{{#Enumeration.WriteLengthLongestLiteral}} argument must be an Enumeration");
                }

                var maxLenght = enumeration.OwnedLiteral
                    .Select(enumerationLiteral => enumerationLiteral.Name)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .Select(name => name.Length)
                    .DefaultIfEmpty(0)
                    .Max();

                writer.WriteSafeString(maxLenght);
            });
        }
    }
}
