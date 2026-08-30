// ------------------------------------------------------------------------------------------------
// <copyright file="UmlCoreJsonDtoSerializerGenerator.cs" company="Starion Group S.A.">
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
    /// A UML Handlebars based DTO Json Serializer code generator
    /// </summary>
    public class UmlCoreJsonDtoSerializerGenerator : UmlHandleBarsGenerator
    {
        /// <summary>
        /// The custom C# type mapping for the Forge model's own primitive DataTypes is process-wide
        /// static state on <see cref="TypedElementExtensions"/> (see <see cref="UmlCoreDtoGenerator"/>'s
        /// own static constructor for why it's needed at all). Registering it here too, rather than
        /// relying on <see cref="UmlCoreDtoGenerator"/> having already run in the same process, is
        /// what makes this generator correct standalone instead of order-dependent on another
        /// generator's static constructor having executed first.
        /// </summary>
        static UmlCoreJsonDtoSerializerGenerator()
        {
            TypedElementExtensions.AddOrOverwriteCSharpTypeMappings(
                ("UUID", "Guid"),
                ("URI", "string"),
                ("SemVer", "string"),
                ("Date", "DateOnly"));
        }

        /// <summary>
        /// Gets the name of the template used to generate DTO Json Serializer
        /// </summary>
        private const string DtoSerializerTemplateName = "json-dto-serializer-uml-template";

        /// <summary>
        /// Gets the name of the template used to generate Json Serializer provider
        /// </summary>
        private const string DtoSerializerProviderTemplateName = "json-dto-serialization-provider-uml-template";

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
            await this.GenerateDtoJsonSerializerAsync(xmiReaderResult, outputDirectory);
            await this.GenerateSerializationProviderAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Register the custom helpers used by the serializer templates
        /// </summary>
        protected override void RegisterHelpers()
        {
            this.Handlebars.RegisterStringHelper();
            this.Handlebars.RegisterClassHelper();
            this.Handlebars.RegisterPropertyHelper();
            this.Handlebars.RegisterNamedElementHelper();

            HandleBarHelpers.PropertyNameHelper.RegisterPropertyNameHelper(this.Handlebars);
            HandleBarHelpers.ReferenceTypeNameHelper.RegisterReferenceTypeNameHelper(this.Handlebars);
        }

        /// <summary>
        /// Register the code templates
        /// </summary>
        protected override void RegisterTemplates()
        {
            this.RegisterTemplate(DtoSerializerTemplateName);
            this.RegisterTemplate(DtoSerializerProviderTemplateName);
        }

        /// <summary>
        /// Generates the Serialization Provider class
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
        public Task GenerateSerializationProviderAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            return this.GenerateSerializationProviderInternalAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Generates the Serialization Provider class
        /// </summary>
        private async Task GenerateSerializationProviderInternalAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            var template = this.Templates[DtoSerializerProviderTemplateName];

            var classes = QueryAllClasses(xmiReaderResult).Where(x => !x.IsAbstract).ToList();

            var generatedSerializationProvider = template(classes);

            generatedSerializationProvider = this.CodeCleanup(generatedSerializationProvider);

            const string fileName = "SerializationProvider.cs";

            await WriteAsync(generatedSerializationProvider, outputDirectory, fileName);
        }

        /// <summary>
        /// Generates DTO Json Serializer files for every non-abstract <see cref="IClass"/> in the model
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
        public Task GenerateDtoJsonSerializerAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            return this.GenerateDtoJsonSerializerInternalAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Generates DTO Json Serializer files
        /// </summary>
        private async Task GenerateDtoJsonSerializerInternalAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            var template = this.Templates[DtoSerializerTemplateName];

            var classes = QueryAllClasses(xmiReaderResult).Where(x => !x.IsAbstract).ToList();

            foreach (var @class in classes)
            {
                var generatedJsonSerializer = template(@class);

                generatedJsonSerializer = this.CodeCleanup(generatedJsonSerializer);

                var fileName = $"{@class.Name.CapitalizeFirstLetter()}Serializer.cs";

                await WriteAsync(generatedJsonSerializer, outputDirectory, fileName);
            }
        }

        /// <summary>
        /// Generates the DTO Json Serializer for a single, named <see cref="IClass"/>, without necessarily
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
        /// <exception cref="ArgumentException">In case of null or whitespace value for the <paramref name="className"/></exception>
        public Task<string> GenerateDtoSerializerClassAsync(XmiReaderResult xmiReaderResult, string className)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentException.ThrowIfNullOrWhiteSpace(className);

            return this.GenerateDtoSerializerClassInternalAsync(xmiReaderResult, className);
        }

        /// <summary>
        /// Generates the DTO Json Serializer for a single, named <see cref="IClass"/>
        /// </summary>
        private async Task<string> GenerateDtoSerializerClassInternalAsync(XmiReaderResult xmiReaderResult, string className)
        {
            var template = this.Templates[DtoSerializerTemplateName];

            var classToGenerate = QueryAllClasses(xmiReaderResult).Single(x => x.Name == className);

            var generatedJsonSerializer = template(classToGenerate);

            generatedJsonSerializer = this.CodeCleanup(generatedJsonSerializer);

            return await Task.FromResult(generatedJsonSerializer);
        }
    }
}
