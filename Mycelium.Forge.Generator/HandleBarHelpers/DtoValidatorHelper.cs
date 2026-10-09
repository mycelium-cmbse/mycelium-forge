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

            handlebars.RegisterHelper("DtoValidator.WriteRulesIfApplicable", (writer, _, arguments) =>
            {
                WriteRulesIfApplicable(writer, arguments);
            });
        }

        /// <summary>
        /// Writes the validation rule for the property, if applicable.
        /// </summary>
        /// <param name="writer">The output text writer.</param>
        /// <param name="arguments">The Handlebars helper arguments containing an <see cref="IProperty" />.</param>
        /// <exception cref="HandlebarsException">Thrown when arguments do not contain exactly one <see cref="IProperty" />.</exception>
        private static void WriteRulesIfApplicable(EncodedTextWriter writer, Arguments arguments)
        {
            var property = arguments.QuerySingle<IProperty>("DtoValidator.WriteRulesIfApplicable");

            var rule = GenerateValidationRule(property);

            if (!string.IsNullOrEmpty(rule))
            {
                writer.WriteSafeString(rule);
            }
        }

        /// <summary>
        /// Generates the FluentValidation rule string for the specified property, or an empty string if no rule applies.
        /// </summary>
        /// <param name="property">The <see cref="IProperty" /> to generate a rule for.</param>
        /// <returns>The generated rule code string.</returns>
        private static string GenerateValidationRule(IProperty property)
        {
            if (ShouldSkipProperty(property))
            {
                return string.Empty;
            }

            var propertyName = property.Name.CapitalizeFirstLetter();

            if (!property.QueryIsDataType())
            {
                return $"            this.RuleFor(x => x.{propertyName}).NotEmpty();{Environment.NewLine}";
            }

            var ruleType = RequiresNotEmpty(property) ? ".NotEmpty();" : ".NotNull();";
            return $"            this.RuleFor(x => x.{propertyName}){ruleType}{Environment.NewLine}";
        }

        /// <summary>
        /// Determines whether validation rule generation should be skipped for the specified property.
        /// </summary>
        /// <param name="property">The <see cref="IProperty" /> to evaluate.</param>
        /// <returns>True if the property should be skipped; otherwise false.</returns>
        private static bool ShouldSkipProperty(IProperty property)
        {
            return property.Lower == 0 ||
                   property.IsDerived ||
                   property.IsDerivedUnion ||
                   property.IsReadOnly ||
                   property.Name.Equals("classKind", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether a data-type property requires a NotEmpty rule instead of a NotNull rule.
        /// </summary>
        /// <param name="property">The data-type <see cref="IProperty" /> to evaluate.</param>
        /// <returns>True if NotEmpty is required; otherwise false.</returns>
        private static bool RequiresNotEmpty(IProperty property)
        {
            var typeName = property.QueryCSharpTypeName();

            return typeName.Equals("string", StringComparison.OrdinalIgnoreCase) ||
                   property.QueryIsString() ||
                   typeName == nameof(DateTime) ||
                   typeName == "DateOnly" ||
                   property.QueryIsEnumerable();
        }
    }
}
