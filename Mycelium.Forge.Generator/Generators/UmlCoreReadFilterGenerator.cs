// ------------------------------------------------------------------------------------------------
// <copyright file="UmlCoreReadFilterGenerator.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Generators
{
    using Mycelium.Forge.Generator.DataLoaders;
    using Mycelium.Forge.Generator.DataLoaders.PermissionModels;
    using Mycelium.Forge.Generator.Extensions;
    using Mycelium.Forge.Generator.HandleBarHelpers;

    using uml4net.StructuredClassifiers;
    using uml4net.xmi.Readers;

    /// <summary>
    /// A UML Handlebars based code generator for SQL read visibility filters.
    /// Emits read filter factory classes for all concrete classes deriving from Thing.
    /// </summary>
    public class UmlCoreReadFilterGenerator : UmlHandleBarsGenerator
    {
        /// <summary>
        /// Gets the name of the template used to generate read filter classes.
        /// </summary>
        private const string ReadFilterClassTemplateName = "read-filter-class-uml-template";

        /// <summary>
        /// Initializes a new instance of the <see cref="UmlCoreReadFilterGenerator" /> class.
        /// </summary>
        public UmlCoreReadFilterGenerator()
        {
            this.TryLoadDefaultEntityPermissions();
        }

        /// <summary>
        /// Gets the dictionary of entity permission definitions loaded from CSV.
        /// </summary>
        public Dictionary<string, EntityPermissionDefinition> EntityPermissions { get; private set; } = [];

        /// <summary>
        /// Gets the dictionary of entity behavior definitions loaded from CSV.
        /// </summary>
        public Dictionary<string, EntityBehaviorDefinition> EntityBehaviors { get; private set; } = [];

        /// <summary>
        /// Loads configuration CSVs: entity permissions and entity behaviors.
        /// </summary>
        /// <param name="entityCsvPath">The path to the entity permissions CSV file.</param>
        /// <param name="behaviorCsvPath">The optional path to the entity behaviors CSV file.</param>
        public void LoadConfigurations(string entityCsvPath, string behaviorCsvPath = null)
        {
            var entityLoader = new CsvEntityPermissionsDataLoader();
            this.EntityPermissions = entityLoader.Load(entityCsvPath);

            if (!string.IsNullOrWhiteSpace(behaviorCsvPath) && File.Exists(behaviorCsvPath))
            {
                var behaviorLoader = new CsvEntityBehaviorsDataLoader();
                this.EntityBehaviors = behaviorLoader.Load(behaviorCsvPath);
            }

            ReadFilterHelper.SetConfigurations(this.EntityPermissions, this.EntityBehaviors);
        }

        /// <summary>
        /// Loads the entity permissions from the specified CSV file, and automatically discovers companion behavior CSV.
        /// </summary>
        /// <param name="csvPath">The path to the entity permissions CSV file.</param>
        public void LoadEntityPermissions(string csvPath)
        {
            var directory = Path.GetDirectoryName(csvPath);
            var behaviorCsv = !string.IsNullOrWhiteSpace(directory) ? Path.Combine(directory, "forge-entity-behaviors.csv") : null;

            this.LoadConfigurations(csvPath, behaviorCsv);
        }

        /// <summary>
        /// Generates read filter classes for all eligible model classes and writes them to <paramref name="outputDirectory" />.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        public override async Task GenerateAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            await this.GenerateReadFilterClassesAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Generates read filter classes for all concrete classes deriving from Thing and writes each to
        /// <paramref name="outputDirectory" />.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        public async Task GenerateReadFilterClassesAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            var template = this.Templates[ReadFilterClassTemplateName];
            var classes = QueryFilterClasses(xmiReaderResult);

            foreach (var @class in classes)
            {
                var content = template(@class);
                var filePath = Path.Combine(outputDirectory.FullName, $"{@class.Name}ReadFilter.cs");
                await File.WriteAllTextAsync(filePath, content);
            }
        }

        /// <summary>
        /// Generates a single read filter class for the specified <paramref name="className" /> as a string.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="className">The name of the class to generate.</param>
        /// <returns>The generated C# source code string.</returns>
        public Task<string> GenerateReadFilterClassAsync(XmiReaderResult xmiReaderResult, string className)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);

            if (string.IsNullOrWhiteSpace(className))
            {
                throw new ArgumentException("Class name cannot be null or whitespace.", nameof(className));
            }

            var @class = QueryFilterClasses(xmiReaderResult).SingleOrDefault(x => x.Name == className);

            if (@class == null)
            {
                throw new InvalidOperationException($"The class {className} could not be found in the model.");
            }

            var template = this.Templates[ReadFilterClassTemplateName];
            var generated = template(@class);

            return Task.FromResult(generated);
        }

        /// <summary>
        /// Registers the Handlebars helpers needed for read filter code generation.
        /// </summary>
        protected override void RegisterHelpers()
        {
            ReadFilterHelper.SetConfigurations(this.EntityPermissions, this.EntityBehaviors);
            this.Handlebars.RegisterReadFilterHelper();
        }

        /// <summary>
        /// Registers the Handlebars templates needed for read filter code generation.
        /// </summary>
        protected override void RegisterTemplates()
        {
            this.RegisterTemplate(ReadFilterClassTemplateName);
        }

        /// <summary>
        /// Attempts to locate and load the default <c>forge-entity-permissions.csv</c> resource.
        /// </summary>
        private void TryLoadDefaultEntityPermissions()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var candidates = new[]
            {
                Path.Combine(baseDirectory, "Resources", "forge-entity-permissions.csv"),
                Path.Combine(baseDirectory, "forge-entity-permissions.csv"),
                Path.GetFullPath(Path.Combine(baseDirectory, "../../../../Mycelium.Forge.Generator/Resources/forge-entity-permissions.csv")),
                Path.GetFullPath(Path.Combine(baseDirectory, "../../../Mycelium.Forge.Generator/Resources/forge-entity-permissions.csv"))
            };

            foreach (var candidate in candidates)
            {
                if (File.Exists(candidate))
                {
                    this.LoadEntityPermissions(candidate);
                    return;
                }
            }
        }

        /// <summary>
        /// Queries all concrete classes deriving from Thing from the UML model.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to query.</param>
        /// <returns>A list of <see cref="IClass" /> instances.</returns>
        private static List<IClass> QueryFilterClasses(XmiReaderResult xmiReaderResult)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);

            return QueryAllClasses(xmiReaderResult)
                .Where(x => (x.HasThingClass() || x.IsThingClass()) && !x.IsAbstract)
                .ToList();
        }
    }
}
