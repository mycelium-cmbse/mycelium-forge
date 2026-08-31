// ------------------------------------------------------------------------------------------------
// <copyright file="UmlCoreSqlSchemaGenerator.cs" company="Starion Group S.A.">
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
    using uml4net.xmi.Readers;

    using PropertyHelper = uml4net.HandleBars.PropertyHelper;

    /// <summary>
    /// A UML Handlebars based SQL schema generator for PostgreSQL database initialization.
    /// Emits table definitions, junction tables, foreign key constraints, autovacuum settings,
    /// delete trigger functions, and triggers for all domain model classes in the UML model.
    /// </summary>
    public class UmlCoreSqlSchemaGenerator : UmlHandleBarsGenerator
    {
        /// <summary>
        /// Gets the name of the template used to generate the core SQL schema.
        /// </summary>
        private const string SqlSchemaTemplateName = "core-sql-schema-template";

        /// <summary>
        /// Registers C# type mappings for custom primitive DataTypes.
        /// </summary>
        static UmlCoreSqlSchemaGenerator()
        {
            TypedElementExtensions.AddOrOverwriteCSharpTypeMappings(
                ("UUID", "Guid"),
                ("URI", "string"),
                ("SemVer", "string"),
                ("Date", "DateOnly"));
        }

        /// <summary>
        /// Generates the SQL schema for the UML model and writes <c>schema.sql</c> into <paramref name="outputDirectory" />.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        public override async Task GenerateAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            await this.GenerateSqlSchemaInternalAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Generates the SQL schema for the UML model and returns the rendered SQL as a string.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <returns>An awaitable <see cref="Task{String}" /> containing the generated SQL schema.</returns>
        public Task<string> GenerateSqlSchemaAsync(XmiReaderResult xmiReaderResult)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);

            return this.GenerateSqlSchemaInternalAsync(xmiReaderResult);
        }

        /// <summary>
        /// Register the custom helpers used by the SQL schema template.
        /// </summary>
        protected override void RegisterHelpers()
        {
            this.Handlebars.RegisterStringHelper();
            this.Handlebars.RegisterEnumerableHelper();
            this.Handlebars.RegisterClassHelper();
            PropertyHelper.RegisterPropertyHelper(this.Handlebars);
            this.Handlebars.RegisterGeneralizationHelper();
            this.Handlebars.RegisterDocumentationHelper();
            this.Handlebars.RegisterNamedElementHelper();

            this.Handlebars.RegisterSqlSchemaHelpers();
        }

        /// <summary>
        /// Register the code templates.
        /// </summary>
        protected override void RegisterTemplates()
        {
            this.RegisterTemplate(SqlSchemaTemplateName);
        }

        /// <summary>
        /// Performs code cleanup on the generated SQL script without applying C# Roslyn formatting.
        /// </summary>
        /// <param name="generatedCode">The generated code that needs to be cleaned.</param>
        /// <returns>The cleaned up SQL code.</returns>
        protected override string CodeCleanup(string generatedCode)
        {
            return generatedCode.Replace("&nbsp;", " ").NormalizeLineEndings();
        }

        /// <summary>
        /// Internal implementation for generating the SQL schema and writing it to disk.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        private async Task GenerateSqlSchemaInternalAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            var generatedSql = await this.GenerateSqlSchemaInternalAsync(xmiReaderResult);

            await WriteAsync(generatedSql, outputDirectory, "schema.sql");
        }

        /// <summary>
        /// Internal implementation for generating the SQL schema string.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <returns>An awaitable <see cref="Task{String}" /> containing the generated SQL schema.</returns>
        private async Task<string> GenerateSqlSchemaInternalAsync(XmiReaderResult xmiReaderResult)
        {
            var template = this.Templates[SqlSchemaTemplateName];

            var classes = QueryAllClasses(xmiReaderResult)
                .Where(x => x.HasThingClass() || x.IsThingClass())
                .OrderBy(x => x.Name)
                .ToList();

            var generatedSql = template(classes);

            generatedSql = this.CodeCleanup(generatedSql);

            return await Task.FromResult(generatedSql);
        }
    }
}
