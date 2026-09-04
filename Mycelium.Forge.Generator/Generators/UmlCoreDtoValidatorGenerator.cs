// ------------------------------------------------------------------------------------------------
// <copyright file="UmlCoreDtoValidatorGenerator.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Generators
{
    using Mycelium.Forge.Generator.HandleBarHelpers;

    using uml4net.Extensions;
    using uml4net.HandleBars;
    using uml4net.StructuredClassifiers;
    using uml4net.xmi.Readers;

    using PropertyHelper = uml4net.HandleBars.PropertyHelper;

    /// <summary>
    /// A UML Handlebars based DTO Validator code generator. For every non-abstract <see cref="IClass" /> in the
    /// model it emits a DTO validator class inheriting from <c>DtoValidatorBase</c>.
    /// </summary>
    public class UmlCoreDtoValidatorGenerator : UmlHandleBarsGenerator
    {
        /// <summary>
        /// Gets the name of the template used to generate DTO validators.
        /// </summary>
        private const string DtoValidatorTemplateName = "dto-validator-uml-template";

        /// <summary>
        /// Registers C# type mappings for custom primitive DataTypes.
        /// </summary>
        static UmlCoreDtoValidatorGenerator()
        {
            TypedElementExtensions.AddOrOverwriteCSharpTypeMappings(
                ("UUID", "Guid"),
                ("URI", "string"),
                ("SemVer", "string"),
                ("Date", "DateOnly"));
        }

        /// <summary>
        /// Generates the DTO validators for the non-abstract classes in the model and writes each to
        /// <paramref name="outputDirectory" />.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        public override async Task GenerateAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            await this.GenerateDtoValidatorsAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Generates the DTO validator for every non-abstract <see cref="IClass" /> in the model and writes each to
        /// <paramref name="outputDirectory" />.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        public Task GenerateDtoValidatorsAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            return this.GenerateDtoValidatorsInternalAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Generates the DTO validator for a single, named <see cref="IClass" />, returning the generated source code.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="name">The name of the class to generate.</param>
        /// <returns>An awaitable <see cref="Task{String}" /> containing the generated source code.</returns>
        public Task<string> GenerateDtoValidatorClassAsync(XmiReaderResult xmiReaderResult, string name)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            return this.GenerateDtoValidatorClassInternalAsync(xmiReaderResult, name);
        }

        /// <summary>
        /// Register the custom helpers used by the DTO validator template.
        /// </summary>
        protected override void RegisterHelpers()
        {
            this.Handlebars.RegisterStringHelper();
            this.Handlebars.RegisterEnumerableHelper();
            this.Handlebars.RegisterClassHelper();
            PropertyHelper.RegisterPropertyHelper(this.Handlebars);
            this.Handlebars.RegisterGeneralizationHelper();
            this.Handlebars.RegisterDocumentationHelper();
            this.Handlebars.RegisterEnumHelper();
            this.Handlebars.RegisterDecoratorHelper();
            this.Handlebars.RegisterNamedElementHelper();

            HandleBarHelpers.PropertyHelper.RegisterPropertyHelper(this.Handlebars);
            this.Handlebars.RegisterDtoValidatorHelper();
        }

        /// <summary>
        /// Register the code templates.
        /// </summary>
        protected override void RegisterTemplates()
        {
            this.RegisterTemplate(DtoValidatorTemplateName);
        }

        /// <summary>
        /// Internal implementation for generating all DTO validators.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        private async Task GenerateDtoValidatorsInternalAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            var template = this.Templates[DtoValidatorTemplateName];
            var classes = QueryAllClasses(xmiReaderResult).Where(x => !x.IsAbstract).ToList();

            foreach (var @class in classes)
            {
                var generatedDtoValidator = template(@class);
                generatedDtoValidator = this.CodeCleanup(generatedDtoValidator);

                var fileName = $"{@class.Name.CapitalizeFirstLetter()}Validator.cs";
                await WriteAsync(generatedDtoValidator, outputDirectory, fileName);
            }
        }

        /// <summary>
        /// Internal implementation for generating a single DTO validator.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="name">The name of the class to generate.</param>
        /// <returns>An awaitable <see cref="Task{String}" /> containing the generated source code.</returns>
        private async Task<string> GenerateDtoValidatorClassInternalAsync(XmiReaderResult xmiReaderResult, string name)
        {
            var template = this.Templates[DtoValidatorTemplateName];
            var classes = QueryAllClasses(xmiReaderResult);
            var @class = classes.Single(x => x.Name == name);

            var generatedDtoValidator = template(@class);
            generatedDtoValidator = this.CodeCleanup(generatedDtoValidator);

            return await Task.FromResult(generatedDtoValidator);
        }
    }
}
