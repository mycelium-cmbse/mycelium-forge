// ------------------------------------------------------------------------------------------------
// <copyright file="UmlCoreDtoComparerGenerator.cs" company="Starion Group S.A.">
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
    /// A UML Handlebars based DTO comparer code generator. For every non-abstract <see cref="IClass" /> in the
    /// model it emits a DTO comparer class implementing <c>IDtoComparer</c>.
    /// </summary>
    public class UmlCoreDtoComparerGenerator : UmlHandleBarsGenerator
    {
        /// <summary>
        /// Gets the name of the template used to generate DTO Comparers.
        /// </summary>
        private const string DtoComparerTemplateName = "dto-comparer-uml-template";

        /// <summary>
        /// Registers C# type mappings for custom primitive DataTypes.
        /// </summary>
        static UmlCoreDtoComparerGenerator()
        {
            TypedElementExtensions.AddOrOverwriteCSharpTypeMappings(
                ("UUID", "Guid"),
                ("URI", "string"),
                ("SemVer", "string"),
                ("Date", "DateOnly"));
        }

        /// <summary>
        /// Generates the DTO comparers for the non-abstract classes in the model and writes each to
        /// <paramref name="outputDirectory" />.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        public override async Task GenerateAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            await this.GenerateDtoComparersAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Generates the DTO comparer for every non-abstract <see cref="IClass" /> in the model and writes each to
        /// <paramref name="outputDirectory" />.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        public Task GenerateDtoComparersAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            return this.GenerateDtoComparersInternalAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Generates the DTO comparer for a single, named <see cref="IClass" />, returning the generated source code.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="name">The name of the class to generate.</param>
        /// <returns>An awaitable <see cref="Task{String}" /> containing the generated source code.</returns>
        public Task<string> GenerateDtoComparerClassAsync(XmiReaderResult xmiReaderResult, string name)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            return this.GenerateDtoComparerClassInternalAsync(xmiReaderResult, name);
        }

        /// <summary>
        /// Register the custom helpers used by the DTO comparer template.
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

            this.Handlebars.RegisterDtoComparerHelper();
        }

        /// <summary>
        /// Register the code templates.
        /// </summary>
        protected override void RegisterTemplates()
        {
            this.RegisterTemplate(DtoComparerTemplateName);
        }

        /// <summary>
        /// Internal implementation for generating all DTO comparers.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        private async Task GenerateDtoComparersInternalAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            var template = this.Templates[DtoComparerTemplateName];

            var classes = QueryAllClasses(xmiReaderResult).Where(x => !x.IsAbstract).ToList();

            foreach (var @class in classes)
            {
                var generatedDtoComparer = template(@class);

                generatedDtoComparer = this.CodeCleanup(generatedDtoComparer);

                var fileName = $"{@class.Name.CapitalizeFirstLetter()}Comparer.cs";

                await WriteAsync(generatedDtoComparer, outputDirectory, fileName);
            }
        }

        /// <summary>
        /// Internal implementation for generating a single DTO comparer.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="name">The name of the class to generate.</param>
        /// <returns>An awaitable <see cref="Task{String}" /> containing the generated source code.</returns>
        private async Task<string> GenerateDtoComparerClassInternalAsync(XmiReaderResult xmiReaderResult, string name)
        {
            var template = this.Templates[DtoComparerTemplateName];

            var classes = QueryAllClasses(xmiReaderResult);

            var @class = classes.Single(x => x.Name == name);

            var generatedDtoComparer = template(@class);

            generatedDtoComparer = this.CodeCleanup(generatedDtoComparer);

            return await Task.FromResult(generatedDtoComparer);
        }
    }
}
