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
                var targetType = DetermineTargetType(classContext, property);

                var propertyReference = string.IsNullOrEmpty(targetType)
                    ? propertyName
                    : $"{targetType}.{propertyName}";

                var stringBuilder = new StringBuilder();
                AppendPropertyComparison(stringBuilder, propertyName, propertyReference, property.QueryIsEnumerable());

                writer.WriteSafeString(stringBuilder.ToString());
            });
        }

        /// <summary>
        /// Determines the target type name for the specified property within the class context.
        /// </summary>
        /// <param name="classContext">The optional class context.</param>
        /// <param name="property">The property being compared.</param>
        /// <returns>The formatted target interface name, or an empty string if unknown.</returns>
        public static string DetermineTargetType(IClass classContext, IProperty property)
        {
            if (classContext != null)
            {
                return $"I{classContext.Name}";
            }

            if (property.Owner is INamedElement owner)
            {
                return $"I{owner.Name}";
            }

            return string.Empty;
        }

        /// <summary>
        /// Appends the comparison logic for a property to the <see cref="StringBuilder" />.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to append to.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="propertyReference">The nameof reference for the property.</param>
        /// <param name="isEnumerable">A value indicating whether the property is an enumerable collection.</param>
        public static void AppendPropertyComparison(StringBuilder stringBuilder, string propertyName, string propertyReference, bool isEnumerable)
        {
            if (isEnumerable)
            {
                stringBuilder.AppendLine($"            var old{propertyName} = oldDto.{propertyName} ?? [];");
                stringBuilder.AppendLine($"            var new{propertyName} = newDto.{propertyName} ?? [];");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"            if (!old{propertyName}.SequenceEqual(new{propertyName}))");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine($"                changes.Add(new PropertyChange(nameof({propertyReference}), oldDto.{propertyName}, newDto.{propertyName}));");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                return;
            }

            stringBuilder.AppendLine($"            if (oldDto.{propertyName} != newDto.{propertyName})");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine($"                changes.Add(new PropertyChange(nameof({propertyReference}), oldDto.{propertyName}, newDto.{propertyName}));");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
        }
    }
}
