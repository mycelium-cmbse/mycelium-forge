// ------------------------------------------------------------------------------------------------
// <copyright file="ReferenceTypeNameHelper.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.HandleBarHelpers
{
    using System;
    using System.Linq;

    using HandlebarsDotNet;

    using Mycelium.Forge.Generator.Extensions;

    using uml4net.Classification;

    /// <summary>
    /// A Handlebars helper that writes the name of the class an <see cref="IProperty" /> reference
    /// points to. Unlike uml4net.HandleBars's own <c>Property.WriteTypeName</c> - which only resolves
    /// a name for concrete reference types, and is otherwise used exclusively for enum properties in
    /// this codebase's templates - this always resolves to the property's declared target class name,
    /// concrete or abstract, so every reference stub in the generated JSON serializers gets a uniform
    /// <c>@type</c>.
    /// </summary>
    public static class ReferenceTypeNameHelper
    {
        /// <summary>
        /// Registers the <see cref="ReferenceTypeNameHelper" />
        /// </summary>
        /// <param name="handlebars">
        /// The <see cref="IHandlebars" /> context with which the helper needs to be registered
        /// </param>
        public static void RegisterReferenceTypeNameHelper(this IHandlebars handlebars)
        {
            handlebars.RegisterHelper("Property.WriteReferenceTypeName", (writer, _, arguments) =>
            {
                if (arguments.Length != 1)
                {
                    throw new HandlebarsException("{{Property.WriteReferenceTypeName}} helper must have exactly one argument");
                }

                if (arguments.Single() is not IProperty property)
                {
                    throw new HandlebarsException("{{Property.WriteReferenceTypeName}} argument must be an IProperty");
                }

                writer.WriteSafeString(property.QueryReferenceTypeName());
            });
        }
    }
}
