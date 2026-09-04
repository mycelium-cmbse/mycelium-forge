// ------------------------------------------------------------------------------------------------
// <copyright file="DtoValidatorHelper.cs" company="Starion Group S.A.">
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

    using uml4net.Classification;
    using uml4net.Extensions;

    /// <summary>
    /// A Handlebars block helper for generating validation rules in DTO Validator classes.
    /// </summary>
    public static class DtoValidatorHelper
    {
        /// <summary>
        /// Registers the <see cref="DtoValidatorHelper" /> with the given <see cref="IHandlebars" /> context.
        /// </summary>
        /// <param name="handlebars">
        /// The <see cref="IHandlebars" /> context with which this helper needs to be registered.
        /// </param>
        public static void RegisterDtoValidatorHelper(this IHandlebars handlebars)
        {
            ArgumentNullException.ThrowIfNull(handlebars);

            handlebars.RegisterHelper("DtoValidator.WriteRulesIfApplicable", (writer, context, _) =>
            {
                if (context.Value is not IProperty property)
                {
                    throw new ArgumentException("DtoValidator.WriteRulesIfApplicable - context is supposed to be an IProperty");
                }

                if (property.Lower == 0 || 
                    property.IsDerived || 
                    property.IsDerivedUnion || 
                    property.IsReadOnly || 
                    property.Name.Equals("classKind", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                var stringBuilder = new StringBuilder();
                var propertyName = property.Name.CapitalizeFirstLetter();

                if (!property.QueryIsDataType())
                {
                    if (!property.IsComposite)
                    {
                        stringBuilder.AppendLine($"            this.RuleFor(x => x.{propertyName}).NotEmpty();");
                    }
                }
                else
                {
                    stringBuilder.Append($"            this.RuleFor(x => x.{propertyName})");

                    if (property.QueryCSharpTypeName().Equals("string", StringComparison.OrdinalIgnoreCase) || 
                        property.QueryIsString() || 
                        property.QueryCSharpTypeName() == nameof(DateTime) || 
                        property.QueryCSharpTypeName() == "DateOnly" || property.QueryIsEnumerable())
                    {
                        stringBuilder.AppendLine(".NotEmpty();");
                    }
                    else
                    {
                        stringBuilder.AppendLine(".NotNull();");
                    }
                }

                writer.WriteSafeString(stringBuilder.ToString());
            });
        }
    }
}
