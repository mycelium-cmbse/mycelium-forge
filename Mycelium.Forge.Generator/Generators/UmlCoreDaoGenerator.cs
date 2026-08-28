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
        /// Generates the DAO interface for every non-abstract <see cref="IClass" /> that derives from Thing
        /// in the model and writes each to <paramref name="outputDirectory" />.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        private async Task GenerateDaoInterfacesAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            var template = this.Templates[DaoInterfaceTemplateName];

            var classes = QueryAllClasses(xmiReaderResult)
                .Where(x => (x.HasThingClass() || x.IsThingClass()) && !x.IsAbstract)
                .ToList();

            foreach (var @class in classes)
            {
                var generatedInterface = template(@class);

                generatedInterface = this.CodeCleanup(generatedInterface);

                var fileName = $"I{@class.Name.CapitalizeFirstLetter()}Dao.cs";

                await WriteAsync(generatedInterface, outputDirectory, fileName);
            }
        }

        /// <summary>
        /// Generates the DAO class for every non-abstract <see cref="IClass" /> that derives from Thing
        /// in the model and writes each to <paramref name="outputDirectory" />.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        private async Task GenerateDaoClassesAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            var template = this.Templates[DaoClassTemplateName];

            var classes = QueryAllClasses(xmiReaderResult)
                .Where(x => (x.HasThingClass() || x.IsThingClass()) && !x.IsAbstract)
                .ToList();

            foreach (var @class in classes)
            {
                var generatedClass = template(@class);

                generatedClass = this.CodeCleanup(generatedClass);

                var fileName = $"{@class.Name.CapitalizeFirstLetter()}Dao.cs";

                await WriteAsync(generatedClass, outputDirectory, fileName);
            }
        }
    }
}
