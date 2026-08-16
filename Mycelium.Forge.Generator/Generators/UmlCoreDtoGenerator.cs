// ------------------------------------------------------------------------------------------------
// <copyright file="UmlCoreDtoGenerator.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Generators
{
    using uml4net.Extensions;
    using uml4net.HandleBars;
    using uml4net.StructuredClassifiers;
    using uml4net.xmi.Readers;

    /// <summary>
    /// A UML Handlebars based DTO code generator. For every non-abstract <see cref="IClass"/> in the
    /// model it emits a partial DTO class, and for every <see cref="IClass"/> (abstract included) it
    /// emits the matching partial DTO interface.
    /// </summary>
    public class UmlCoreDtoGenerator : UmlHandleBarsGenerator
    {
        /// <summary>
        /// uml4net.Extensions' default C# type mapping only covers UML's own primitive types
        /// (Boolean, Integer, Real, String, UnlimitedNatural). The Forge model's own primitive
        /// DataTypes (defined in the forge/Common package) aren't in that table, so without an
        /// explicit mapping they'd render as their bare UML name - e.g. a property typed UUID would
        /// emit as `UUID Id { get; set; }`, which doesn't compile. This mapping is process-wide
        /// static state on uml4net.Extensions.TypeExtensions, so it's registered exactly once via a
        /// static constructor rather than per generator instance.
        /// </summary>
        static UmlCoreDtoGenerator()
        {
            TypedElementExtensions.AddOrOverwriteCSharpTypeMappings(
                ("UUID", "Guid"),
                ("URI", "string"),
                ("SemVer", "string"),
                ("Date", "DateOnly"));
        }

        /// <summary>
        /// Generates the DTO interfaces and classes for the classes in the model
        /// </summary>
        /// <param name="xmiReaderResult">
        /// the <see cref="XmiReaderResult"/> that contains the UML model to generate from
        /// </param>
        /// <param name="outputDirectory">
        /// The target <see cref="DirectoryInfo"/>
        /// </param>
        /// <returns>
        /// an awaitable <see cref="Task"/>
        /// </returns>
        public override async Task GenerateAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            await this.GenerateDataTransferObjectInterfacesAsync(xmiReaderResult, outputDirectory);
            await this.GenerateDataTransferObjectClassesAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Generates the DTO interface for every <see cref="IClass"/> in the model and writes each to
        /// <paramref name="outputDirectory"/>
        /// </summary>
        public Task GenerateDataTransferObjectInterfacesAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            return this.GenerateDataTransferObjectInterfacesInternalAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Generates the DTO interface for a single, named <see cref="IClass"/>, without necessarily
        /// writing it to disk; the rendered text is returned so it can be diffed against a
        /// committed golden file by <c>ExpectedOutputTestFixture</c>.
        /// </summary>
        public Task<string> GenerateDataTransferObjectInterfaceAsync(XmiReaderResult xmiReaderResult, string name)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            return this.GenerateDataTransferObjectInterfaceInternalAsync(xmiReaderResult, name);
        }

        /// <summary>
        /// Generates the DTO class for every non-abstract <see cref="IClass"/> in the model and writes
        /// each to <paramref name="outputDirectory"/>
        /// </summary>
        public Task GenerateDataTransferObjectClassesAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            return this.GenerateDataTransferObjectClassesInternalAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Generates the DTO class for a single, named <see cref="IClass"/>, without necessarily
        /// writing it to disk; the rendered text is returned so it can be diffed against a
        /// committed golden file by <c>ExpectedOutputTestFixture</c>.
        /// </summary>
        public Task<string> GenerateDataTransferObjectClassAsync(XmiReaderResult xmiReaderResult, string name)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            return this.GenerateDataTransferObjectClassInternalAsync(xmiReaderResult, name);
        }

        /// <summary>
        /// Register the custom helpers used by the DTO class and interface templates
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

            HandleBarHelpers.PropertyHelper.RegisterPropertyHelper(this.Handlebars);
        }

        /// <summary>
        /// Register the code templates
        /// </summary>
        protected override void RegisterTemplates()
        {
            this.RegisterTemplate("dto-class-uml-template");
            this.RegisterTemplate("dto-interface-uml-template");
        }

        private async Task GenerateDataTransferObjectInterfacesInternalAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            var template = this.Templates["dto-interface-uml-template"];

            var classes = QueryAllClasses(xmiReaderResult);

            foreach (var @class in classes)
            {
                var generatedDto = template(@class);

                generatedDto = this.CodeCleanup(generatedDto);

                var fileName = $"I{@class.Name.CapitalizeFirstLetter()}.cs";

                await WriteAsync(generatedDto, outputDirectory, fileName);
            }
        }

        private async Task<string> GenerateDataTransferObjectInterfaceInternalAsync(XmiReaderResult xmiReaderResult, string name)
        {
            var template = this.Templates["dto-interface-uml-template"];

            var classes = QueryAllClasses(xmiReaderResult);

            var @class = classes.Single(x => x.Name == name);

            var generatedDataTransferObjectInterface = template(@class);

            generatedDataTransferObjectInterface = this.CodeCleanup(generatedDataTransferObjectInterface);

            return await Task.FromResult(generatedDataTransferObjectInterface);
        }

        private async Task GenerateDataTransferObjectClassesInternalAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            var template = this.Templates["dto-class-uml-template"];

            var classes = QueryAllClasses(xmiReaderResult).Where(x => !x.IsAbstract).ToList();

            foreach (var @class in classes)
            {
                var generatedDto = template(@class);

                generatedDto = this.CodeCleanup(generatedDto);

                var fileName = $"{@class.Name.CapitalizeFirstLetter()}.cs";

                await WriteAsync(generatedDto, outputDirectory, fileName);
            }
        }

        private async Task<string> GenerateDataTransferObjectClassInternalAsync(XmiReaderResult xmiReaderResult, string name)
        {
            var template = this.Templates["dto-class-uml-template"];

            var classes = QueryAllClasses(xmiReaderResult);

            var @class = classes.Single(x => x.Name == name);

            var generatedDataTransferObjectClass = template(@class);

            generatedDataTransferObjectClass = this.CodeCleanup(generatedDataTransferObjectClass);

            return await Task.FromResult(generatedDataTransferObjectClass);
        }
    }
}
