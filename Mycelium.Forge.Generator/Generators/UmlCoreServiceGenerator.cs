// ------------------------------------------------------------------------------------------------
// <copyright file="UmlCoreServiceGenerator.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Generators
{
    using Mycelium.Forge.Generator.Extensions;

    using uml4net.Extensions;
    using uml4net.HandleBars;
    using uml4net.StructuredClassifiers;
    using uml4net.xmi.Readers;

    /// <summary>
    /// A UML Handlebars based Service code generator that generates domain entity service interfaces,
    /// implementation classes, and service collection registration extensions for all concrete classes deriving from Thing.
    /// </summary>
    public class UmlCoreServiceGenerator : UmlHandleBarsGenerator
    {
        /// <summary>
        /// Gets the name of the template used to generate service classes.
        /// </summary>
        private const string ServiceClassTemplateName = "service-class-uml-template";

        /// <summary>
        /// Gets the name of the template used to generate service interfaces.
        /// </summary>
        private const string ServiceInterfaceTemplateName = "service-interface-uml-template";

        /// <summary>
        /// Gets the name of the template used to generate the service collection registration extension class.
        /// </summary>
        private const string ServiceRegistryClassTemplateName = "service-registry-class-uml-template";

        /// <summary>
        /// Registers C# type mappings for custom primitive DataTypes.
        /// </summary>
        static UmlCoreServiceGenerator()
        {
            TypedElementExtensions.AddOrOverwriteCSharpTypeMappings(
                ("UUID", "Guid"),
                ("URI", "string"),
                ("SemVer", "string"),
                ("Date", "DateOnly"));
        }

        /// <summary>
        /// Generates service interfaces, service classes, and service collection extensions for the model and writes each to
        /// <paramref name="outputDirectory" />.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        public override async Task GenerateAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            await this.GenerateServiceInterfacesAsync(xmiReaderResult, outputDirectory);
            await this.GenerateServiceClassesAsync(xmiReaderResult, outputDirectory);
            await this.GenerateServiceRegistryAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Generates the service interface for every non-abstract <see cref="IClass" /> that derives from Thing
        /// in the model and writes each to <paramref name="outputDirectory" />.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        public Task GenerateServiceInterfacesAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            return this.GenerateServiceBatchAsync(xmiReaderResult, outputDirectory, ServiceInterfaceTemplateName, true);
        }

        /// <summary>
        /// Generates the service class for every non-abstract <see cref="IClass" /> that derives from Thing
        /// in the model and writes each to <paramref name="outputDirectory" />.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        public Task GenerateServiceClassesAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            return this.GenerateServiceBatchAsync(xmiReaderResult, outputDirectory, ServiceClassTemplateName, false);
        }

        /// <summary>
        /// Generates the service collection extensions registry class for all non-abstract <see cref="IClass" /> that derive from
        /// Thing
        /// in the model and writes it to <paramref name="outputDirectory" />.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        public async Task GenerateServiceRegistryAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            var template = this.Templates[ServiceRegistryClassTemplateName];
            var classes = QueryServiceClasses(xmiReaderResult);
            var generated = this.CodeCleanup(template(classes));

            await WriteAsync(generated, outputDirectory, "ServiceCollectionExtensions.cs");
        }

        /// <summary>
        /// Generates the service class for a single, named <see cref="IClass" />, without necessarily
        /// writing it to disk; the rendered text is returned so it can be verified in tests.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="className">The name of the class to generate.</param>
        /// <returns>An awaitable <see cref="Task" /> with the generated code.</returns>
        public Task<string> GenerateServiceClassAsync(XmiReaderResult xmiReaderResult, string className)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentException.ThrowIfNullOrWhiteSpace(className);

            return this.GenerateSingleServiceAsync(xmiReaderResult, className, ServiceClassTemplateName);
        }

        /// <summary>
        /// Register the custom helpers used by the service templates.
        /// </summary>
        protected override void RegisterHelpers()
        {
            this.Handlebars.RegisterStringHelper();
            this.Handlebars.RegisterEnumerableHelper();
            this.Handlebars.RegisterClassHelper();
            this.Handlebars.RegisterPropertyHelper();
            this.Handlebars.RegisterGeneralizationHelper();
            this.Handlebars.RegisterDocumentationHelper();
            this.Handlebars.RegisterEnumHelper();
            this.Handlebars.RegisterDecoratorHelper();
            this.Handlebars.RegisterNamedElementHelper();
        }

        /// <summary>
        /// Register the code templates.
        /// </summary>
        protected override void RegisterTemplates()
        {
            this.RegisterTemplate(ServiceInterfaceTemplateName);
            this.RegisterTemplate(ServiceClassTemplateName);
            this.RegisterTemplate(ServiceRegistryClassTemplateName);
        }

        /// <summary>
        /// Queries all non-abstract classes that derive from or are Thing in the model.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model.</param>
        /// <returns>A list of <see cref="IClass" /> instances.</returns>
        private static List<IClass> QueryServiceClasses(XmiReaderResult xmiReaderResult)
        {
            return QueryAllClasses(xmiReaderResult)
                .Where(x => (x.HasThingClass() || x.IsThingClass()) && !x.IsAbstract)
                .ToList();
        }

        /// <summary>
        /// Generates service source files for all matching classes and writes them to the specified directory.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <param name="templateName">The name of the template to execute.</param>
        /// <param name="isInterface">A value indicating whether generating interfaces.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        private async Task GenerateServiceBatchAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory, string templateName, bool isInterface)
        {
            var template = this.Templates[templateName];
            var classes = QueryServiceClasses(xmiReaderResult);

            foreach (var @class in classes)
            {
                var prefix = isInterface ? "I" : string.Empty;
                var fileName = $"{prefix}{@class.Name.CapitalizeFirstLetter()}Service.cs";
                var generated = this.CodeCleanup(template(@class));

                await WriteAsync(generated, outputDirectory, fileName);
            }
        }

        /// <summary>
        /// Generates a single service source string for a named class using the specified template.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="className">The name of the class to generate.</param>
        /// <param name="templateName">The name of the template to execute.</param>
        /// <returns>An awaitable <see cref="Task" /> with the generated code.</returns>
        private Task<string> GenerateSingleServiceAsync(XmiReaderResult xmiReaderResult, string className, string templateName)
        {
            var template = this.Templates[templateName];
            var classToGenerate = QueryAllClasses(xmiReaderResult).Single(x => x.Name == className);
            var generated = this.CodeCleanup(template(classToGenerate));

            return Task.FromResult(generated);
        }
    }
}
