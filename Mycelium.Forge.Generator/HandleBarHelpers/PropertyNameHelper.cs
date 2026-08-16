// ------------------------------------------------------------------------------------------------
// <copyright file="PropertyNameHelper.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.HandleBarHelpers
{
    using HandlebarsDotNet;

    using uml4net.Classification;
    using uml4net.Extensions;

    /// <summary>
    /// A Handlebars helper that writes the C# property name a generated DTO exposes for a given
    /// <see cref="IProperty"/> - the same naming rule <see cref="PropertyHelper"/> already applies
    /// when declaring the property (<c>property.Name.CapitalizeFirstLetter()</c>, lower-cased first
    /// letter for a derived/derived-union property), extracted into its own helper because the JSON
    /// (de)serializer templates only need the name, not a full property declaration.
    /// </summary>
    public static class PropertyNameHelper
    {
        /// <summary>
        /// Registers the <see cref="PropertyNameHelper"/>
        /// </summary>
        /// <param name="handlebars">
        /// The <see cref="IHandlebars"/> context with which the helper needs to be registered
        /// </param>
        public static void RegisterPropertyNameHelper(this IHandlebars handlebars)
        {
            handlebars.RegisterHelper("Property.WritePropertyName", (writer, context, arguments) =>
            {
                if (arguments.Length != 1 || arguments[0] is not IProperty property)
                {
                    throw new HandlebarsException("{{#Property.WritePropertyName}} helper must have exactly one IProperty argument");
                }

                var propertyName = property.Name.CapitalizeFirstLetter();

                if (property.IsDerived || property.IsDerivedUnion)
                {
                    propertyName = propertyName.LowerCaseFirstLetter();
                }

                writer.WriteSafeString(propertyName);
            });

            // The JSON (de)serializer templates write/read IThing.Id explicitly as "@id" before
            // iterating the rest of a class's properties, so the ordinary "id" property returned by
            // Class.QueryAllProperties must be skipped in that loop - otherwise the same value is
            // written/read twice, once as "@id" and once as a redundant "id".
            handlebars.RegisterHelper("Property.IsThingId", (context, arguments) =>
            {
                if (arguments.Length != 1 || arguments[0] is not IProperty property)
                {
                    throw new HandlebarsException("{{#Property.IsThingId}} helper must have exactly one IProperty argument");
                }

                return property.Name.Equals("id", System.StringComparison.OrdinalIgnoreCase);
            });

            // Property.QueryIsString (uml4net core) tests whether the property's UML *type* is
            // literally the primitive String - it returns false for Forge's own custom DataTypes that
            // merely *map* to C# string (e.g. SemVer, via UmlCoreDtoGenerator's
            // AddOrOverwriteCSharpTypeMappings), which is the wrong question for deciding how to write
            // a nullable property (string is a reference type - no .HasValue/.Value - regardless of
            // which UML type produced it). Mirrors the check this project's own DTO generation already
            // relies on for the same reason (uml4net.Extensions.PropertyExtensions.QueryIsNullableAndNotString,
            // used by HandleBarHelpers/PropertyHelper.cs) rather than the core QueryIsString helper.
            handlebars.RegisterHelper("Property.QueryIsCSharpString", (context, arguments) =>
            {
                if (arguments.Length != 1 || arguments[0] is not IProperty property)
                {
                    throw new HandlebarsException("{{#Property.QueryIsCSharpString}} helper must have exactly one IProperty argument");
                }

                return property.QueryCSharpTypeName() == "string";
            });
        }
    }
}
