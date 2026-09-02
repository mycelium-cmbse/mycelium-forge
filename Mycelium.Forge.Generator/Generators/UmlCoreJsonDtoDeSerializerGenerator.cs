// ------------------------------------------------------------------------------------------------
// <copyright file="UmlCoreJsonDtoDeSerializerGenerator.cs" company="Starion Group S.A.">
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

    using PropertyHelper = Mycelium.Forge.Generator.HandleBarHelpers.PropertyHelper;

    /// <summary>
    /// A UML Handlebars based DTO Json DeSerializer code generator
    /// </summary>
    public class UmlCoreJsonDtoDeSerializerGenerator : UmlHandleBarsGenerator
    {
        /// <summary>
        /// Gets the name of the template used to generate DTO Json DeSerializer
        /// </summary>
        private const string DtoDeSerializerTemplateName = "json-dto-deserializer-uml-template";

        /// <summary>
        /// Gets the name of the template used to generate Json DeSerializer provider
        /// </summary>
        private const string DtoDeSerializerProviderTemplateName = "json-dto-deserialization-provider-uml-template";

        /// <summary>
        /// See <see cref="UmlCoreJsonDtoSerializerGenerator" />'s static constructor for why this is
        /// registered here too, rather than relying on another generator's static constructor having
        /// already run in the same process.
        /// </summary>
        static UmlCoreJsonDtoDeSerializerGenerator()
        {
            TypedElementExtensions.AddOrOverwriteCSharpTypeMappings(
                ("UUID", "Guid"),
                ("URI", "string"),
                ("SemVer", "string"),
                ("Date", "DateOnly"));
        }

        /// <summary>
        /// Generates code specific to the concrete implementation
        /// </summary>
        /// <param name="xmiReaderResult">
        /// the <see cref="XmiReaderResult" /> that contains the UML model to generate from
        /// </param>
        /// <param name="outputDirectory">
        /// The target <see cref="DirectoryInfo" />
        /// </param>
        /// <returns>
        /// an awaitable <see cref="Task" />
        /// </returns>
        public override async Task GenerateAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            await this.GenerateDtoJsonDeSerializerAsync(xmiReaderResult, outputDirectory);
            await this.GenerateDeSerializationProviderAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Generates the DeSerialization Provider class
        /// </summary>
        /// <param name="xmiReaderResult">the <see cref="XmiReaderResult" /> that contains the UML model to generate from</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" /></param>
        /// <returns>
        /// an awaitable <see cref="Task" />
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// In case of null value for <paramref name="xmiReaderResult" /> or
        /// <paramref name="outputDirectory" />
        /// </exception>
        public Task GenerateDeSerializationProviderAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            return this.GenerateDeSerializationProviderInternalAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Generates DTO Json DeSerializer files for every non-abstract <see cref="IClass" /> in the model
        /// </summary>
        /// <param name="xmiReaderResult">the <see cref="XmiReaderResult" /> that contains the UML model to generate from</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" /></param>
        /// <returns>
        /// an awaitable <see cref="Task" />
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// In case of null value for <paramref name="xmiReaderResult" /> or
        /// <paramref name="outputDirectory" />
        /// </exception>
        public Task GenerateDtoJsonDeSerializerAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            return this.GenerateDtoJsonDeSerializerInternalAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Generates the DTO Json DeSerializer for a single, named <see cref="IClass" />, without necessarily
        /// writing it to disk; the rendered text is returned so it can be diffed against a committed
        /// golden file by <c>ExpectedOutputTestFixture</c>.
        /// </summary>
        /// <param name="xmiReaderResult">the <see cref="XmiReaderResult" /> that contains the UML model to generate from</param>
        /// <param name="className">The name of the class to generate</param>
        /// <returns>
        /// an awaitable <see cref="Task" /> with the generated code
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// In case of null value for <paramref name="xmiReaderResult" />
        /// </exception>
        /// <exception cref="ArgumentException">
        /// In case of null or whitespace value for the <paramref name="className" />
        /// </exception>
        public Task<string> GenerateDtoDeSerializerClassAsync(XmiReaderResult xmiReaderResult, string className)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentException.ThrowIfNullOrWhiteSpace(className);

            return this.GenerateDtoDeSerializerClassInternalAsync(xmiReaderResult, className);
        }

        /// <summary>
        /// Register the custom helpers used by the deserializer templates
        /// </summary>
        protected override void RegisterHelpers()
        {
            this.Handlebars.RegisterStringHelper();
            this.Handlebars.RegisterClassHelper();
            uml4net.HandleBars.PropertyHelper.RegisterPropertyHelper(this.Handlebars);
            this.Handlebars.RegisterNamedElementHelper();

            // Registers custom generator property helpers such as Class.QueryDtoClassProperties and Class.QueryDtoInterfaceProperties
            PropertyHelper.RegisterPropertyHelper(this.Handlebars);
            this.Handlebars.RegisterPropertyNameHelper();
        }

        /// <summary>
        /// Register the code templates
        /// </summary>
        protected override void RegisterTemplates()
        {
            this.RegisterTemplate(DtoDeSerializerTemplateName);
            this.RegisterTemplate(DtoDeSerializerProviderTemplateName);
        }

        /// <summary>
        /// Generates the DeSerialization Provider class
        /// </summary>
        private async Task GenerateDeSerializationProviderInternalAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            var template = this.Templates[DtoDeSerializerProviderTemplateName];

            var classes = QueryAllClasses(xmiReaderResult).Where(x => !x.IsAbstract).ToList();

            var generatedDeSerializationProvider = template(classes);

            generatedDeSerializationProvider = this.CodeCleanup(generatedDeSerializationProvider);

            const string fileName = "DeSerializationProvider.cs";

            await WriteAsync(generatedDeSerializationProvider, outputDirectory, fileName);
        }

        /// <summary>
        /// Generates DTO Json DeSerializer files
        /// </summary>
        private async Task GenerateDtoJsonDeSerializerInternalAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            var template = this.Templates[DtoDeSerializerTemplateName];

            var classes = QueryAllClasses(xmiReaderResult).Where(x => !x.IsAbstract).ToList();

            foreach (var @class in classes)
            {
                var generatedJsonDeSerializer = template(@class);

                generatedJsonDeSerializer = this.CodeCleanup(generatedJsonDeSerializer);

                var fileName = $"{@class.Name.CapitalizeFirstLetter()}DeSerializer.cs";

                await WriteAsync(generatedJsonDeSerializer, outputDirectory, fileName);
            }
        }

        /// <summary>
        /// Generates the DTO Json DeSerializer for a single, named <see cref="IClass" />
        /// </summary>
        private async Task<string> GenerateDtoDeSerializerClassInternalAsync(XmiReaderResult xmiReaderResult, string className)
        {
            var template = this.Templates[DtoDeSerializerTemplateName];

            var classToGenerate = QueryAllClasses(xmiReaderResult).Single(x => x.Name == className);

            var generatedJsonDeSerializer = template(classToGenerate);

            generatedJsonDeSerializer = this.CodeCleanup(generatedJsonDeSerializer);

            return await Task.FromResult(generatedJsonDeSerializer);
        }
    }
}
