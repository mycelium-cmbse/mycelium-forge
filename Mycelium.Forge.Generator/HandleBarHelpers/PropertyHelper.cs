// ------------------------------------------------------------------------------------------------
// <copyright file="PropertyHelper.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.HandleBarHelpers
{
    using System.Globalization;
    using System.Text;

    using HandlebarsDotNet;

    using Mycelium.Forge.Generator.Extensions;

    using uml4net.Classification;
    using uml4net.CommonStructure;
    using uml4net.Extensions;
    using uml4net.SimpleClassifiers;
    using uml4net.StructuredClassifiers;

    /// <summary>
    /// A Handlebars block helper for the <see cref="IProperty" /> interface, adapted from
    /// SysML2.NET.CodeGenerator's own local PropertyHelper: DTO reference properties render as the
    /// referenced type's unique identifier (<c>Guid</c>/<c>Guid?</c>/<c>List&lt;Guid&gt;</c>), not as
    /// an embedded object or interface, per the reference-property conventions documented at
    /// https://github.com/STARIONGROUP/uml4net/wiki/code-generation-conventions.
    /// </summary>
    public static class PropertyHelper
    {
        /// <summary>
        /// Registers the <see cref="PropertyHelper" />
        /// </summary>
        /// <param name="handlebars">
        /// The <see cref="IHandlebars" /> context with which the helper needs to be registered
        /// </param>
        public static void RegisterPropertyHelper(this IHandlebars handlebars)
        {
            ArgumentNullException.ThrowIfNull(handlebars);

            // uml4net's QueryOwnedAttributeOrdered misses reverse composite association ends; unions owned attributes with owner properties
            handlebars.RegisterHelper("Class.QueryDtoInterfaceProperties", (context, _) =>
            {
                if (context.Value is not IClass @class)
                {
                    throw new ArgumentException("supposed to be IClass");
                }

                return @class.QueryDtoInterfaceProperties();
            });

            // uml4net's QueryAllProperties misses reverse composite association ends; unions full hierarchy properties with superclass owner properties
            handlebars.RegisterHelper("Class.QueryDtoClassProperties", (context, _) =>
            {
                if (context.Value is not IClass @class)
                {
                    throw new ArgumentException("supposed to be IClass");
                }

                return @class.QueryDtoClassProperties();
            });

            // Writes XML documentation for a property, providing a default summary for owner properties
            handlebars.RegisterHelper("Property.WriteDocumentation", (in writer, in options, in context, in arguments) =>
            {
                if (context.Value is not IProperty property)
                {
                    throw new ArgumentException("supposed to be IProperty");
                }

                if (!property.OwnedComment.Any() && property.Name.Equals("owner", StringComparison.OrdinalIgnoreCase))
                {
                    var ownerTypeName = property.Type?.Name
                                        ?? property.Opposite?.Class?.Name
                                        ?? property.Opposite?.Type?.Name
                                        ?? "container";

                    writer.WriteSafeString($"        /// <summary>{Environment.NewLine}        /// The unique identifier of the owning {ownerTypeName}.{Environment.NewLine}        /// </summary>{Environment.NewLine}");
                    return;
                }

                if (handlebars.Configuration.Helpers.TryGetValue("Documentation", out var docHelper))
                {
                    docHelper.Invoke(writer, options, context, arguments);
                }
            });

            // Writes [Implements] attribute on DTO class properties referencing the declaring interface
            handlebars.RegisterHelper("Decorator.WriteImplementsAttribute", (writer, context, _) =>
            {
                if (context.Value is not IProperty property)
                {
                    throw new ArgumentException("supposed to be IProperty");
                }

                var propertyName = property.Name.CapitalizeFirstLetter();

                // Computes the declaring interface name from the property's class, owner, or opposite type
                var className = property.Class?.Name
                                ?? (property.Owner as IClass)?.Name
                                ?? (property.Opposite?.Type as IClass)?.Name
                                ?? (property.Owner as INamedElement)?.Name;

                writer.WriteSafeString($"[Implements(implementation: \"I{className}.{propertyName}\")]{Environment.NewLine}");
            });

            handlebars.RegisterHelper("Property.WriteForDTOInterface", (writer, context, _) =>
            {
                if (context.Value is not IProperty property)
                {
                    throw new ArgumentException("supposed to be IProperty");
                }

                var sb = new StringBuilder();

                if (property.RedefinedProperty.Any(x => x.Name == property.Name))
                {
                    sb.Append("new ");
                }

                sb.Append(QueryDtoPropertyTypeName(property));
                sb.Append(' ');

                var propertyName = property.Name.CapitalizeFirstLetter();

                if (property.IsDerived || property.IsDerivedUnion)
                {
                    propertyName = propertyName.LowerCaseFirstLetter();
                }

                sb.Append(propertyName);
                sb.Append(' ');

                if (property.IsReadOnly || property.IsDerived || property.IsDerivedUnion)
                {
                    sb.Append("{ get; }");
                }
                else
                {
                    sb.Append("{ get; set; }");
                }

                writer.WriteSafeString(sb + Environment.NewLine);
            });

            handlebars.RegisterHelper("Property.WriteForDTOClass", (writer, _, parameters) =>
            {
                if (parameters.Length != 2)
                {
                    throw new HandlebarsException("{{#Property.WriteForDTOClass}} helper must have exactly two arguments");
                }

                var property = (parameters[0] as IProperty)!;
                var classContext = (parameters[1] as IClass)!;

                var sb = new StringBuilder();
                var propertyName = property.Name.CapitalizeFirstLetter();
                var isRedefinedPropertyInContext = property.TryQueryRedefinedByProperty(classContext, out var redefiningProperty);

                if (!isRedefinedPropertyInContext)
                {
                    sb.Append(property.Visibility.ToString().ToLower(CultureInfo.InvariantCulture));
                    sb.Append(' ');
                }

                sb.Append(QueryDtoPropertyTypeName(property));
                sb.Append(' ');

                if (property.IsDerived || property.IsDerivedUnion)
                {
                    propertyName = propertyName.LowerCaseFirstLetter();
                }

                if (isRedefinedPropertyInContext)
                {
                    // Every generated type lives in the single flat Mycelium.Forge.Common namespace
                    // (the AutoGenDto/AutoGenEnum split is folder-only, not reflected in the C#
                    // namespace), so the explicit interface implementation never needs qualifying
                    // with anything beyond the bare interface name.
                    var owner = (INamedElement)property.Owner;
                    propertyName = $"I{owner.Name}.{propertyName}";
                }

                sb.Append(propertyName);
                sb.Append(' ');

                if (property.IsReadOnly || property.IsDerived || property.IsDerivedUnion)
                {
                    if (isRedefinedPropertyInContext)
                    {
                        sb.Append($"=> {GetRedefinedPropertyGetterImplementationForDto(property, redefiningProperty, classContext)}");
                    }
                    else
                    {
                        sb.Append("{ get; internal set; }");

                        if (property.QueryIsEnumerable())
                        {
                            sb.Append(" = [];");
                        }
                    }
                }
                else
                {
                    if (isRedefinedPropertyInContext)
                    {
                        sb.AppendLine("{");
                        sb.AppendLine($"\tget => {GetRedefinedPropertyGetterImplementationForDto(property, redefiningProperty, classContext)}");
                        var setterImplementation = GetRedefinedPropertySetterImplementationForDto(property, redefiningProperty, classContext);

                        if (string.IsNullOrWhiteSpace(setterImplementation))
                        {
                            sb.AppendLine("\tset { }");
                        }
                        else
                        {
                            sb.AppendLine("\tset ");
                            sb.AppendLine("{");
                            sb.AppendLine($"\t{setterImplementation}");
                            sb.Append('}');
                        }

                        sb.Append('}');
                    }
                    else
                    {
                        sb.Append("{ get; set; }");

                        if (property.QueryIsEnumerable())
                        {
                            sb.Append(" = [];");
                        }
                    }
                }

                if (!isRedefinedPropertyInContext)
                {
                    if (property.QueryIsEnumPropertyWithDefaultValue())
                    {
                        sb.Append($" = {property.Type.Name.CapitalizeFirstLetter()}.{property.QueryDefaultValueAsString().CapitalizeFirstLetter()};");
                    }
                    else if (property.QueryIsDefaultValueDifferentThanDefault())
                    {
                        if (property.QueryIsString())
                        {
                            sb.Append($" = \"{property.QueryDefaultValueAsString()}\";");
                        }
                        else
                        {
                            sb.Append($" = {property.QueryDefaultValueAsString()};");
                        }
                    }
                }

                writer.WriteSafeString(sb + Environment.NewLine);
            });
        }

        /// <summary>
        /// Computes the C# type name for a DTO property, mapping DataTypes and entity references to C# types or GUIDs.
        /// </summary>
        /// <param name="property">The <see cref="IProperty" /> to compute the type name for.</param>
        /// <returns>The C# type name string.</returns>
        private static string QueryDtoPropertyTypeName(IProperty property)
        {
            if (property.Type is IDataType)
            {
                if (property.QueryIsEnumerable())
                {
                    return $"List<{property.QueryCSharpTypeName()}>";
                }

                var nullable = property.QueryIsNullableAndNotString() ? "?" : string.Empty;
                return $"{property.QueryCSharpTypeName()}{nullable}";
            }

            if (property.QueryIsEnumerable())
            {
                return "List<Guid>";
            }

            var guidNullable = property.QueryIsNullableAndNotString() ? "?" : string.Empty;
            return $"Guid{guidNullable}";
        }

        /// <summary>
        /// Resolves the property name expression for a redefinition in the specified class context.
        /// </summary>
        /// <param name="redefinition">The redefinition <see cref="IProperty" />.</param>
        /// <param name="context">The <see cref="IClass" /> context.</param>
        /// <returns>The property name expression string.</returns>
        private static string GetRedefinitionPropertyName(IProperty redefinition, IClass context)
        {
            if (redefinition.TryQueryRedefinedByProperty(context, out _))
            {
                var owner = (INamedElement)redefinition.Owner;
                return $"((I{owner.Name})this).{redefinition.QueryPropertyNameBasedOnUmlProperties()}";
            }

            return $"this.{redefinition.QueryPropertyNameBasedOnUmlProperties()}";
        }

        /// <summary>
        /// Gets the getter implementation for an <see cref="IProperty" /> that has been redefined, for DTO generation
        /// </summary>
        /// <param name="redefinedProperty">The redefined property</param>
        /// <param name="redefinition">The property that redefines <paramref name="redefinedProperty" /></param>
        /// <param name="context">Gets the <see cref="IClass" /> context</param>
        /// <returns>The getter implementation</returns>
        private static string GetRedefinedPropertyGetterImplementationForDto(IProperty redefinedProperty, IProperty redefinition, IClass context)
        {
            var redefinitionPropertyName = GetRedefinitionPropertyName(redefinition, context);

            if (redefinedProperty.QueryIsEnumerable() && redefinition.QueryIsEnumerable())
            {
                return $"[..{redefinitionPropertyName}];";
            }

            if (redefinedProperty.QueryIsEnumerable() && !redefinition.QueryIsEnumerable())
            {
                return redefinition.QueryIsNullableAndNotString()
                    ? $"{redefinitionPropertyName}.HasValue ? [{redefinitionPropertyName}.Value] : [];"
                    : $"[{redefinitionPropertyName}];";
            }

            return redefinition.QueryIsNullableAndNotString()
                ? $"{redefinitionPropertyName}.HasValue ? {redefinitionPropertyName}.Value : {(redefinedProperty.QueryIsReferenceType() ? "Guid.Empty" : "default")};"
                : $"{redefinitionPropertyName};";
        }

        /// <summary>
        /// Gets the setter implementation for an <see cref="IProperty" /> that has been redefined, for DTO generation
        /// </summary>
        /// <param name="redefinedProperty">The redefined property</param>
        /// <param name="redefinition">The property that redefines <paramref name="redefinedProperty" /></param>
        /// <param name="context">Gets the <see cref="IClass" /> context</param>
        /// <returns>The setter implementation</returns>
        private static string GetRedefinedPropertySetterImplementationForDto(IProperty redefinedProperty, IProperty redefinition, IClass context)
        {
            if (redefinition.IsDerived || redefinition.IsDerivedUnion || redefinition.IsReadOnly)
            {
                return string.Empty;
            }

            var redefinitionPropertyName = GetRedefinitionPropertyName(redefinition, context);

            if (redefinedProperty.QueryIsEnumerable() == redefinition.QueryIsEnumerable())
            {
                return $"{redefinitionPropertyName} = value;";
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("if(value.Count != 0)");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine($"\t{redefinitionPropertyName} = value[0];");
            stringBuilder.AppendLine("}");
            return stringBuilder.ToString();
        }
    }
}
