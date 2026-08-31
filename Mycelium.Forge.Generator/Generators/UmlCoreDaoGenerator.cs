// ------------------------------------------------------------------------------------------------
// <copyright file="UmlCoreDaoGenerator.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Generators
{
    using Mycelium.Forge.Generator.Extensions;
    using Mycelium.Forge.Generator.HandleBarHelpers;

    using uml4net.Extensions;
    using uml4net.HandleBars;
    using uml4net.StructuredClassifiers;
    using uml4net.xmi.Readers;

    using PropertyHelper = uml4net.HandleBars.PropertyHelper;

    /// <summary>
    /// A UML Handlebars based DAO (Data Access Object) code generator.
    /// Emits DAO classes and interfaces for all concrete classes deriving from Thing.
    /// </summary>
    public class UmlCoreDaoGenerator : UmlHandleBarsGenerator
    {
        /// <summary>
        /// Gets the name of the template used to generate DAO classes.
        /// </summary>
        private const string DaoClassTemplateName = "dao-class-uml-template";

        /// <summary>
        /// Gets the name of the template used to generate DAO interfaces.
        /// </summary>
        private const string DaoInterfaceTemplateName = "dao-interface-uml-template";

        /// <summary>
        /// Registers C# type mappings for custom primitive DataTypes.
        /// </summary>
        static UmlCoreDaoGenerator()
        {
            TypedElementExtensions.AddOrOverwriteCSharpTypeMappings(
                ("UUID", "Guid"),
                ("URI", "string"),
                ("SemVer", "string"),
                ("Date", "DateOnly"));
        }

        /// <summary>
        /// Generates both the DAO interfaces and DAO classes for the model and writes each to
        /// <paramref name="outputDirectory" />.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        public override async Task GenerateAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            await this.GenerateDaoInterfacesAsync(xmiReaderResult, outputDirectory);
            await this.GenerateDaoClassesAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Generates the DAO interface for every non-abstract <see cref="IClass" /> that derives from Thing
        /// in the model and writes each to <paramref name="outputDirectory" />.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        public Task GenerateDaoInterfacesAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            return this.GenerateDaoBatchAsync(xmiReaderResult, outputDirectory, DaoInterfaceTemplateName, true);
        }

        /// <summary>
        /// Generates the DAO class for every non-abstract <see cref="IClass" /> that derives from Thing
        /// in the model and writes each to <paramref name="outputDirectory" />.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        public Task GenerateDaoClassesAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            return this.GenerateDaoBatchAsync(xmiReaderResult, outputDirectory, DaoClassTemplateName, false);
        }

        /// <summary>
        /// Generates the DAO class for a single, named <see cref="IClass" />, without necessarily
        /// writing it to disk; the rendered text is returned so it can be diffed against a
        /// committed golden file by <c>ExpectedOutputTestFixture</c>.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="className">The name of the class to generate.</param>
        /// <returns>An awaitable <see cref="Task" /> with the generated code.</returns>
        public Task<string> GenerateDaoClassAsync(XmiReaderResult xmiReaderResult, string className)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentException.ThrowIfNullOrWhiteSpace(className);

            return this.GenerateSingleDaoAsync(xmiReaderResult, className, DaoClassTemplateName);
        }

        /// <summary>
        /// Register the custom helpers used by the DAO templates.
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

            this.Handlebars.RegisterDaoHelper();
        }

        /// <summary>
        /// Register the code templates.
        /// </summary>
        protected override void RegisterTemplates()
        {
            this.RegisterTemplate(DaoInterfaceTemplateName);
            this.RegisterTemplate(DaoClassTemplateName);
        }

        /// <summary>
        /// Queries all non-abstract classes that derive from or are Thing in the model.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model.</param>
        /// <returns>A list of <see cref="IClass" /> instances.</returns>
        private static List<IClass> QueryDaoClasses(XmiReaderResult xmiReaderResult)
        {
            return QueryAllClasses(xmiReaderResult)
                .Where(x => (x.HasThingClass() || x.IsThingClass()) && !x.IsAbstract)
                .ToList();
        }

        /// <summary>
        /// Generates DAO source files for all matching classes and writes them to the specified directory.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <param name="templateName">The name of the template to execute.</param>
        /// <param name="isInterface">A value indicating whether generating interfaces.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        private async Task GenerateDaoBatchAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory, string templateName, bool isInterface)
        {
            var template = this.Templates[templateName];
            var classes = QueryDaoClasses(xmiReaderResult);

            foreach (var @class in classes)
            {
                var prefix = isInterface ? "I" : string.Empty;
                var fileName = $"{prefix}{@class.Name.CapitalizeFirstLetter()}Dao.cs";
                var generated = this.CodeCleanup(template(@class));

                await WriteAsync(generated, outputDirectory, fileName);
            }
        }

        /// <summary>
        /// Generates a single DAO source string for a named class using the specified template.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="className">The name of the class to generate.</param>
        /// <param name="templateName">The name of the template to execute.</param>
        /// <returns>An awaitable <see cref="Task" /> with the generated code.</returns>
        private Task<string> GenerateSingleDaoAsync(XmiReaderResult xmiReaderResult, string className, string templateName)
        {
            var template = this.Templates[templateName];
            var classToGenerate = QueryAllClasses(xmiReaderResult).Single(x => x.Name == className);
            var generated = this.CodeCleanup(template(classToGenerate));

            return Task.FromResult(generated);
        }
    }
}
