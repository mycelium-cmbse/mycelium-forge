// ------------------------------------------------------------------------------------------------
// <copyright file="UmlCoreEnumGenerator.cs" company="Starion Group S.A.">
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
    using uml4net.SimpleClassifiers;
    using uml4net.xmi.Readers;

    /// <summary>
    /// A UML Handlebars based enum code generator. For every <see cref="IEnumeration"/> in the model
    /// it emits a partial C# enum.
    /// </summary>
    public class UmlCoreEnumGenerator : UmlHandleBarsGenerator
    {
        private const string EnumTemplateName = "enum-uml-template";

        /// <summary>
        /// Generates the enumerations found in the model
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
            await this.GenerateEnumerationsAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Generates the C# enum for every <see cref="IEnumeration"/> in the model and writes each to
        /// <paramref name="outputDirectory"/>
        /// </summary>
        public Task GenerateEnumerationsAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            return this.GenerateEnumerationsInternalAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Generates the C# enum for a single, named <see cref="IEnumeration"/>, without necessarily
        /// writing it to disk; the rendered text is returned so it can be diffed against a committed
        /// golden file by <c>ExpectedOutputTestFixture</c>.
        /// </summary>
        public Task<string> GenerateEnumerationAsync(XmiReaderResult xmiReaderResult, string name)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            return this.GenerateEnumerationInternalAsync(xmiReaderResult, name);
        }

        /// <summary>
        /// Register the custom helpers used by the enum template
        /// </summary>
        protected override void RegisterHelpers()
        {
            this.Handlebars.RegisterDocumentationHelper();
            this.Handlebars.RegisterEnumHelper();

            // uml4net.HandleBars does not ship a helper to write an enumeration literal's name; this
            // is the one small piece hand-rolled here rather than ported wholesale from SysML2.NET's
            // reserved-keyword-aware variant, which is out of scope for this model.
            this.Handlebars.RegisterHelper("EnumerationLiteral.Write", (writer, _, parameters) =>
            {
                if (parameters.Length != 1 || parameters[0] is not IEnumerationLiteral enumerationLiteral)
                {
                    throw new HandlebarsException("{{#EnumerationLiteral.Write}} helper must have exactly one argument");
                }

                writer.WriteSafeString(enumerationLiteral.Name.CapitalizeFirstLetter());
            });
        }

        /// <summary>
        /// Register the code templates
        /// </summary>
        protected override void RegisterTemplates()
        {
            this.RegisterTemplate(EnumTemplateName);
        }

        private async Task GenerateEnumerationsInternalAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            var template = this.Templates[EnumTemplateName];

            var enumerations = QueryAllEnumerations(xmiReaderResult);

            foreach (var enumeration in enumerations)
            {
                var generatedEnumeration = template(enumeration);

                generatedEnumeration = this.CodeCleanup(generatedEnumeration);

                var fileName = $"{enumeration.Name.CapitalizeFirstLetter()}.cs";

                await WriteAsync(generatedEnumeration, outputDirectory, fileName);
            }
        }

        private async Task<string> GenerateEnumerationInternalAsync(XmiReaderResult xmiReaderResult, string name)
        {
            var template = this.Templates[EnumTemplateName];

            var enumerations = QueryAllEnumerations(xmiReaderResult);

            var enumeration = enumerations.Single(x => x.Name == name);

            var generatedEnumeration = template(enumeration);

            generatedEnumeration = this.CodeCleanup(generatedEnumeration);

            return await Task.FromResult(generatedEnumeration);
        }
    }
}
