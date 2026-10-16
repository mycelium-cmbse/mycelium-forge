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

    using Mycelium.Forge.Generator.Extensions;

    using uml4net.Classification;
    using uml4net.Extensions;
    using uml4net.StructuredClassifiers;

    /// <summary>
    /// A Handlebars block helper for generating validation rules and inheritance in DTO Validator classes.
    /// </summary>
    public static class DtoValidatorHelper
    {
        /// <summary>
        /// Registers the <see cref="DtoValidatorHelper" /> with the given <see cref="IHandlebars" /> context.
        /// </summary>
        /// <param name="handlebars">The <see cref="IHandlebars" /> context with which this helper needs to be registered.</param>
        public static void RegisterDtoValidatorHelper(this IHandlebars handlebars)
        {
            ArgumentNullException.ThrowIfNull(handlebars);

            handlebars.RegisterHelper("DtoValidator.WriteRulesIfApplicable", (writer, _, arguments) => WriteRulesIfApplicable(writer, arguments));
            handlebars.RegisterHelper("DtoValidator.BaseClass", (writer, _, arguments) => WriteBaseClass(writer, arguments));
        }

        /// <summary>
        /// Writes the validation rule for the property, if applicable.
        /// </summary>
        /// <param name="writer">The output text writer.</param>
        /// <param name="arguments">The Handlebars helper arguments containing an <see cref="IProperty" />.</param>
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
        /// Writes the base class declaration for the validator of the specified <see cref="IClass" />.
        /// </summary>
        /// <param name="writer">The output text writer.</param>
        /// <param name="arguments">The Handlebars helper arguments containing an <see cref="IClass" />.</param>
        private static void WriteBaseClass(EncodedTextWriter writer, Arguments arguments)
        {
            var @class = arguments.QuerySingle<IClass>("DtoValidator.BaseClass");
            writer.WriteSafeString(QueryBaseClass(@class));
        }

        /// <summary>
        /// Computes the base class name for a validator.
        /// </summary>
        /// <param name="class">The <see cref="IClass" /> to evaluate.</param>
        /// <returns>The base class declaration string.</returns>
        private static string QueryBaseClass(IClass @class)
        {
            if (@class.IsThingClass())
            {
                return "DtoValidatorBase<T>";
            }

            var directParents = @class.Generalization.Select(generalization => generalization.General).OfType<IClass>().Where(parentClass => parentClass.IsThingClass() || parentClass.HasThingClass()).ToList();

            var parent = directParents.Count switch
            {
                0 => throw new InvalidOperationException($"Class {@class.Name} has no parent in the Thing hierarchy."),
                1 => directParents[0],
                _ => directParents.OrderByDescending(parentClass => parentClass.QueryAllGeneralClassifiers().Count).First()
            };

            var parentName = parent.Name.CapitalizeFirstLetter();

            return @class.IsAbstract ? $"{parentName}Validator<T>" : $"{parentName}Validator<I{@class.Name.CapitalizeFirstLetter()}>";
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
            var typeName = property.QueryCSharpTypeName();
            var isGuid = !property.QueryIsDataType() || typeName.Equals(nameof(Guid), StringComparison.OrdinalIgnoreCase);
            var isString = property.QueryIsString() || typeName.Equals(nameof(String), StringComparison.OrdinalIgnoreCase);

            if (property.QueryIsEnumerable())
            {
                var stringBuilder = new StringBuilder();

                if (property.Lower > 0)
                {
                    stringBuilder.AppendLine($"            this.RuleFor(x => x.{propertyName}).Must(value => value is {{ Count: >= {property.Lower} }});");
                }

                if (isGuid)
                {
                    stringBuilder.AppendLine($"            this.RuleForEach(x => x.{propertyName}).NotEmpty();");
                }
                else if (isString)
                {
                    stringBuilder.AppendLine($"            this.RuleForEach(x => x.{propertyName}).Must(value => !string.IsNullOrWhiteSpace(value));");
                }

                return stringBuilder.ToString();
            }

            if (isGuid)
            {
                return property.Lower == 0 ? $"            this.RuleFor(x => x.{propertyName}).Must(value => !value.HasValue || value.Value != Guid.Empty);{Environment.NewLine}" : $"            this.RuleFor(x => x.{propertyName}).NotEmpty();{Environment.NewLine}";
            }

            if (isString)
            {
                return property.Lower >= 1 ? $"            this.RuleFor(x => x.{propertyName}).Must(value => !string.IsNullOrWhiteSpace(value));{Environment.NewLine}" : string.Empty;
            }

            if (typeName is nameof(DateTime) or nameof(DateOnly))
            {
                return property.Lower >= 1 ? $"            this.RuleFor(x => x.{propertyName}).NotEmpty();{Environment.NewLine}" : string.Empty;
            }

            return property.Lower >= 1 ? $"            this.RuleFor(x => x.{propertyName}).NotNull();{Environment.NewLine}" : string.Empty;
        }

        /// <summary>
        /// Determines whether validation rule generation should be skipped for the specified property.
        /// </summary>
        /// <param name="property">The <see cref="IProperty" /> to evaluate.</param>
        /// <returns>True if the property should be skipped; otherwise false.</returns>
        private static bool ShouldSkipProperty(IProperty property)
        {
            return property.IsDerived ||
                   property.IsDerivedUnion ||
                   property.IsReadOnly ||
                   property.Name.Equals("classKind", StringComparison.OrdinalIgnoreCase);
        }
    }
}
