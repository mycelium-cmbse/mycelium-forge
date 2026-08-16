// ------------------------------------------------------------------------------------------------
// <copyright file="UmlCoreEnumProviderGenerator.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Generators
{
    using HandlebarsDotNet;

    using uml4net.Extensions;
    using uml4net.HandleBars;
    using uml4net.xmi.Readers;

    /// <summary>
    /// A UML Handlebars based EnumProvider code generator
    /// </summary>
    public class UmlCoreEnumProviderGenerator : UmlHandleBarsGenerator
    {
        /// <summary>
        /// Generates the Enumeration Providers
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
            await this.GenerateEnumerationProvidersAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Generates Enumerations
        /// </summary>
        /// <param name="xmiReaderResult">
        /// the <see cref="XmiReaderResult" /> that contains the UML model to generate from
        /// </param>
        /// <param name="outputDirectory">
        /// The target <see cref="DirectoryInfo" />
        /// </param>
        /// <returns>
        /// an awaitable task
        /// </returns>
        public Task GenerateEnumerationProvidersAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);

            ArgumentNullException.ThrowIfNull(outputDirectory);

            return this.GenerateEnumerationProvidersInternalAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Generates Enumeration Provider
        /// </summary>
        /// <param name="xmiReaderResult">
        /// the <see cref="XmiReaderResult" /> that contains the UML model to generate from
        /// </param>
        /// <param name="outputDirectory">
        /// The target <see cref="DirectoryInfo" />
        /// </param>
        /// <param name="name">
        /// The name of the Enumeration to generate
        /// </param>
        /// <returns>
        /// an awaitable task
        /// </returns>
        public Task<string> GenerateEnumerationProviderAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory, string name)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);

            ArgumentNullException.ThrowIfNull(outputDirectory);
            ArgumentException.ThrowIfNullOrEmpty(name);

            return this.GenerateEnumerationProviderInternalAsync(xmiReaderResult, outputDirectory, name);
        }

        /// <summary>
        /// Register the custom helpers
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

            HandleBarHelpers.EnumerationLiteralHelper.RegisterTypeNameHelper(this.Handlebars);
            HandleBarHelpers.EnumerationHelper.RegisterEnumerationHelper(this.Handlebars);
        }

        /// <summary>
        /// Register the code templates
        /// </summary>
        protected override void RegisterTemplates()
        {
            this.RegisterTemplate("enumprovider-uml-template");
        }

        /// <summary>
        /// Generates Enumeration
        /// </summary>
        /// <param name="xmiReaderResult">
        /// the <see cref="XmiReaderResult" /> that contains the UML model to generate from
        /// </param>
        /// <param name="outputDirectory">
        /// The target <see cref="DirectoryInfo" />
        /// </param>
        /// <returns>
        /// an awaitable task
        /// </returns>
        private async Task GenerateEnumerationProvidersInternalAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            var template = this.Templates["enumprovider-uml-template"];

            var enumerations = QueryAllEnumerations(xmiReaderResult);

            foreach (var enumeration in enumerations)
            {
                var generatedEnumerationProvider = template(enumeration);

                generatedEnumerationProvider = this.CodeCleanup(generatedEnumerationProvider);

                var fileName = $"{enumeration.Name.CapitalizeFirstLetter()}Provider.cs";

                await WriteAsync(generatedEnumerationProvider, outputDirectory, fileName);
            }
        }

        /// <summary>
        /// Generates EnumerationProvider classes
        /// </summary>
        /// <param name="xmiReaderResult">
        /// the <see cref="XmiReaderResult" /> that contains the UML model to generate from
        /// </param>
        /// <param name="outputDirectory">
        /// The target <see cref="DirectoryInfo" />
        /// </param>
        /// <param name="name">
        /// The name of the Enumeration to generate
        /// </param>
        /// <returns>
        /// an awaitable task
        /// </returns>
        private async Task<string> GenerateEnumerationProviderInternalAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory, string name)
        {
            var template = this.Templates["enumprovider-uml-template"];

            var enumerations = QueryAllEnumerations(xmiReaderResult);

            var enumeration = enumerations.Single(x => x.Name == name);

            var generatedProviderEnumeration = template(enumeration);

            generatedProviderEnumeration = this.CodeCleanup(generatedProviderEnumeration);

            var fileName = $"{enumeration.Name.CapitalizeFirstLetter()}Provider.cs";

            await WriteAsync(generatedProviderEnumeration, outputDirectory, fileName);

            return generatedProviderEnumeration;
        }
    }
}
