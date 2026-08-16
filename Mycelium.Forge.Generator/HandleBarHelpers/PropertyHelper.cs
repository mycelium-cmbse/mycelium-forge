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
    using System;
    using System.Globalization;
    using System.Text;

    using Mycelium.Forge.Generator.Extensions;
    
    using HandlebarsDotNet;

    using uml4net.Classification;
    using uml4net.CommonStructure;
    using uml4net.Extensions;
    using uml4net.SimpleClassifiers;
    using uml4net.StructuredClassifiers;

    /// <summary>
    /// A Handlebars block helper for the <see cref="IProperty"/> interface, adapted from
    /// SysML2.NET.CodeGenerator's own local PropertyHelper: DTO reference properties render as the
    /// referenced type's unique identifier (<c>Guid</c>/<c>Guid?</c>/<c>List&lt;Guid&gt;</c>), not as
    /// an embedded object or interface, per the reference-property conventions documented at
    /// https://github.com/STARIONGROUP/uml4net/wiki/code-generation-conventions.
    /// </summary>
    public static class PropertyHelper
    {
        /// <summary>
        /// Registers the <see cref="PropertyHelper"/>
        /// </summary>
        /// <param name="handlebars">
        /// The <see cref="IHandlebars"/> context with which the helper needs to be registered
        /// </param>
        public static void RegisterPropertyHelper(this IHandlebars handlebars)
        {
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

                if (property.Type is IDataType)
                {
                    if (property.QueryIsEnumerable())
                    {
                        sb.Append($"List<{property.QueryCSharpTypeName()}>");
                        sb.Append(' ');
                    }
                    else
                    {
                        sb.Append($"{property.QueryCSharpTypeName()}");

                        if (uml4net.Extensions.PropertyExtensions.QueryIsNullableAndNotString(property))
                        {
                            sb.Append('?');
                        }

                        sb.Append(' ');
                    }
                }
                else
                {
                    if (property.QueryIsEnumerable())
                    {
                        sb.Append("List<Guid>");
                        sb.Append(' ');
                    }
                    else
                    {
                        sb.Append("Guid");

                        if (uml4net.Extensions.PropertyExtensions.QueryIsNullableAndNotString(property))
                        {
                            sb.Append('?');
                        }

                        sb.Append(' ');
                    }
                }

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

                if (property.Type is IDataType)
                {
                    if (property.QueryIsEnumerable())
                    {
                        sb.Append($"List<{property.QueryCSharpTypeName()}>");
                        sb.Append(' ');
                    }
                    else
                    {
                        sb.Append($"{property.QueryCSharpTypeName()}");

                        if (uml4net.Extensions.PropertyExtensions.QueryIsNullableAndNotString(property))
                        {
                            sb.Append('?');
                        }

                        sb.Append(' ');
                    }
                }
                else
                {
                    if (property.QueryIsEnumerable())
                    {
                        sb.Append("List<Guid>");
                        sb.Append(' ');
                    }
                    else
                    {
                        sb.Append("Guid");

                        if (uml4net.Extensions.PropertyExtensions.QueryIsNullableAndNotString(property))
                        {
                            sb.Append('?');
                        }

                        sb.Append(' ');
                    }
                }

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
        /// Gets the getter implementation for an <see cref="IProperty"/> that has been redefined, for DTO generation
        /// </summary>
        /// <param name="redefinedProperty">The redefined property</param>
        /// <param name="redefinition">The property that redefines <paramref name="redefinedProperty"/></param>
        /// <param name="context">Gets the <see cref="IClass"/> context</param>
        /// <returns>The getter implementation</returns>
        private static string GetRedefinedPropertyGetterImplementationForDto(IProperty redefinedProperty, IProperty redefinition, IClass context)
        {
            string redefinitionPropertyName;

            if (redefinition.TryQueryRedefinedByProperty(context, out _))
            {
                var owner = (INamedElement)redefinition.Owner;
                redefinitionPropertyName = $"((I{owner.Name})this).{redefinition.QueryPropertyNameBasedOnUmlProperties()}";
            }
            else
            {
                redefinitionPropertyName = $"this.{redefinition.QueryPropertyNameBasedOnUmlProperties()}";
            }

            if (redefinedProperty.QueryIsEnumerable() && redefinition.QueryIsEnumerable())
            {
                return $"[..{redefinitionPropertyName}];";
            }

            if (redefinedProperty.QueryIsEnumerable() && !redefinition.QueryIsEnumerable())
            {
                return uml4net.Extensions.PropertyExtensions.QueryIsNullableAndNotString(redefinition)
                    ? $"{redefinitionPropertyName}.HasValue ? [{redefinitionPropertyName}.Value] : [];"
                    : $"[{redefinitionPropertyName}];";
            }

            return uml4net.Extensions.PropertyExtensions.QueryIsNullableAndNotString(redefinition)
                ? $"{redefinitionPropertyName}.HasValue ? {redefinitionPropertyName}.Value : {(redefinedProperty.QueryIsReferenceType() ? "Guid.Empty" : "default")};"
                : $"{redefinitionPropertyName};";
        }

        /// <summary>
        /// Gets the setter implementation for an <see cref="IProperty"/> that has been redefined, for DTO generation
        /// </summary>
        /// <param name="redefinedProperty">The redefined property</param>
        /// <param name="redefinition">The property that redefines <paramref name="redefinedProperty"/></param>
        /// <param name="context">Gets the <see cref="IClass"/> context</param>
        /// <returns>The setter implementation</returns>
        private static string GetRedefinedPropertySetterImplementationForDto(IProperty redefinedProperty, IProperty redefinition, IClass context)
        {
            if (redefinition.IsDerived || redefinition.IsDerivedUnion || redefinition.IsReadOnly)
            {
                return string.Empty;
            }

            string redefinitionPropertyName;

            if (redefinition.TryQueryRedefinedByProperty(context, out _))
            {
                var owner = (INamedElement)redefinition.Owner;
                redefinitionPropertyName = $"((I{owner.Name})this).{redefinition.QueryPropertyNameBasedOnUmlProperties()}";
            }
            else
            {
                redefinitionPropertyName = $"this.{redefinition.QueryPropertyNameBasedOnUmlProperties()}";
            }

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
