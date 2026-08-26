// ------------------------------------------------------------------------------------------------
// <copyright file="DtoComparerHelper.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.HandleBarHelpers
{
    using System.Text;

    using HandlebarsDotNet;

    using Mycelium.Forge.Generator.Extensions;

    using uml4net.Classification;
    using uml4net.CommonStructure;
    using uml4net.Extensions;
    using uml4net.StructuredClassifiers;

    /// <summary>
    /// A Handlebars helper for generating property comparison statements for DTO comparers.
    /// </summary>
    public static class DtoComparerHelper
    {
        /// <summary>
        /// Registers the <see cref="DtoComparerHelper" /> with the given <see cref="IHandlebars" /> instance.
        /// </summary>
        /// <param name="handlebars">
        /// The <see cref="IHandlebars" /> context with which the helper needs to be registered.
        /// </param>
        public static void RegisterDtoComparerHelper(this IHandlebars handlebars)
        {
            ArgumentNullException.ThrowIfNull(handlebars);

            handlebars.RegisterHelper("forge.DtoComparer.WriteComparisonIfApplicable", (writer, _, arguments) =>
            {
                if (arguments.Length == 0 || arguments[0] is not IProperty property)
                {
                    throw new HandlebarsException("{{#forge.DtoComparer.WriteComparisonIfApplicable}} helper must have at least one IProperty argument");
                }

                if (property.Name.Equals("id", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                var classContext = arguments.Length > 1 ? arguments[1] as IClass : null;
                var propertyName = property.QueryPropertyNameBasedOnUmlProperties();

                var targetType = classContext != null
                    ? $"I{classContext.Name}"
                    : property.Owner is INamedElement owner
                        ? $"I{owner.Name}"
                        : string.Empty;

                var propertyReference = string.IsNullOrEmpty(targetType)
                    ? propertyName
                    : $"{targetType}.{propertyName}";

                var stringBuilder = new StringBuilder();

                if (property.QueryIsEnumerable())
                {
                    stringBuilder.AppendLine($"            var old{propertyName} = oldDto.{propertyName} ?? [];");
                    stringBuilder.AppendLine($"            var new{propertyName} = newDto.{propertyName} ?? [];");
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine($"            if (!old{propertyName}.SequenceEqual(new{propertyName}))");
                    stringBuilder.AppendLine("            {");
                    stringBuilder.AppendLine($"                changes.Add(new PropertyChange(nameof({propertyReference}), oldDto.{propertyName}, newDto.{propertyName}));");
                    stringBuilder.AppendLine("            }");
                    stringBuilder.AppendLine();
                }
                else
                {
                    stringBuilder.AppendLine($"            if (oldDto.{propertyName} != newDto.{propertyName})");
                    stringBuilder.AppendLine("            {");
                    stringBuilder.AppendLine($"                changes.Add(new PropertyChange(nameof({propertyReference}), oldDto.{propertyName}, newDto.{propertyName}));");
                    stringBuilder.AppendLine("            }");
                    stringBuilder.AppendLine();
                }

                writer.WriteSafeString(stringBuilder.ToString());
            });
        }
    }
}
