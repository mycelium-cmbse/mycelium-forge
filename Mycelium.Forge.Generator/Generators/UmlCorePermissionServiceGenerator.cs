// ------------------------------------------------------------------------------------------------
// <copyright file="UmlCorePermissionServiceGenerator.cs" company="Starion Group S.A.">
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

    using uml4net.Extensions;
    using uml4net.HandleBars;
    using uml4net.StructuredClassifiers;
    using uml4net.xmi.Readers;

    using PropertyHelper = uml4net.HandleBars.PropertyHelper;

    /// <summary>
    /// A UML Handlebars-based Permission Service code generator that generates authorization mapping
    /// artifacts (roles, permissions, map) and per-entity permission services for each concrete Thing in the model.
    /// </summary>
    public class UmlCorePermissionServiceGenerator : UmlHandleBarsGenerator
    {
        /// <summary>
        /// Gets the name of the template used to generate the role enum.
        /// </summary>
        private const string RoleEnumTemplateName = "role-enum-template";

        /// <summary>
        /// Gets the name of the template used to generate the permission enum.
        /// </summary>
        private const string PermissionEnumTemplateName = "permission-enum-template";

        /// <summary>
        /// Gets the name of the template used to generate the role-permission map.
        /// </summary>
        private const string RolePermissionMapTemplateName = "role-permission-map-template";

        /// <summary>
        /// Gets the name of the template used to generate permission service interfaces.
        /// </summary>
        private const string PermissionServiceInterfaceTemplateName = "permission-service-interface-uml-template";

        /// <summary>
        /// Gets the name of the template used to generate permission service classes.
        /// </summary>
        private const string PermissionServiceClassTemplateName = "permission-service-class-uml-template";

        /// <summary>
        /// Initializes a new instance of the <see cref="UmlCorePermissionServiceGenerator" /> class.
        /// </summary>
        public UmlCorePermissionServiceGenerator()
        {
            this.TryLoadDefaultEntityPermissions();
        }

        /// <summary>
        /// Generates the permission services for the model and writes them to <paramref name="outputDirectory" />.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        public override async Task GenerateAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            await this.GeneratePermissionServicesAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Loads the entity permissions from the specified CSV file and configures the helper.
        /// </summary>
        /// <param name="csvPath">The path to the entity permissions CSV file.</param>
        public void LoadEntityPermissions(string csvPath)
        {
            var loader = new CsvEntityPermissionsDataLoader();
            var definitions = loader.Load(csvPath);
            PermissionHelper.SetEntityPermissions(definitions);
        }

        /// <summary>
        /// Generates all authorization mapping files (<c>RoleKind.cs</c>, <c>PermissionKind.cs</c>, <c>RolePermissionMap.cs</c>)
        /// into the specified output directory.
        /// </summary>
        /// <param name="model">The parsed role-permission model.</param>
        /// <param name="outputDirectory">The target directory.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        public async Task GenerateAuthorizationMappingAsync(RolePermissionModel model, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(model);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            var roleTemplate = this.Templates[RoleEnumTemplateName];
            var generatedRole = this.CodeCleanup(roleTemplate(model));
            await WriteAsync(generatedRole, outputDirectory, "RoleKind.cs");

            var permissionTemplate = this.Templates[PermissionEnumTemplateName];
            var generatedPermission = this.CodeCleanup(permissionTemplate(model));
            await WriteAsync(generatedPermission, outputDirectory, "PermissionKind.cs");

            var mapTemplate = this.Templates[RolePermissionMapTemplateName];
            var generatedMap = this.CodeCleanup(mapTemplate(model));
            await WriteAsync(generatedMap, outputDirectory, "RolePermissionMap.cs");
        }

        /// <summary>
        /// Generates both the permission service interfaces and classes for the model and writes each to
        /// <paramref name="outputDirectory" />.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        public async Task GeneratePermissionServicesAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            await this.GeneratePermissionServiceInterfacesAsync(xmiReaderResult, outputDirectory);
            await this.GeneratePermissionServiceClassesAsync(xmiReaderResult, outputDirectory);
        }

        /// <summary>
        /// Generates the permission service interface for every non-abstract <see cref="IClass" /> that derives from Thing
        /// in the model and writes each to <paramref name="outputDirectory" />.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        public Task GeneratePermissionServiceInterfacesAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            return this.GeneratePermissionServiceBatchAsync(xmiReaderResult, outputDirectory, PermissionServiceInterfaceTemplateName, true);
        }

        /// <summary>
        /// Generates the permission service class for every non-abstract <see cref="IClass" /> that derives from Thing
        /// in the model and writes each to <paramref name="outputDirectory" />.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        public Task GeneratePermissionServiceClassesAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            return this.GeneratePermissionServiceBatchAsync(xmiReaderResult, outputDirectory, PermissionServiceClassTemplateName, false);
        }

        /// <summary>
        /// Register the custom helpers used by the permission service templates.
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

            this.Handlebars.RegisterPermissionHelper();
        }

        /// <summary>
        /// Register the code templates.
        /// </summary>
        protected override void RegisterTemplates()
        {
            this.RegisterTemplate(RoleEnumTemplateName);
            this.RegisterTemplate(PermissionEnumTemplateName);
            this.RegisterTemplate(RolePermissionMapTemplateName);
            this.RegisterTemplate(PermissionServiceInterfaceTemplateName);
            this.RegisterTemplate(PermissionServiceClassTemplateName);
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
        /// Queries all non-abstract classes that derive from or are Thing in the model.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model.</param>
        /// <returns>A list of <see cref="IClass" /> instances.</returns>
        private static List<IClass> QueryPermissionServiceClasses(XmiReaderResult xmiReaderResult)
        {
            return QueryAllClasses(xmiReaderResult)
                .Where(x => (x.HasThingClass() || x.IsThingClass()) && !x.IsAbstract)
                .ToList();
        }

        /// <summary>
        /// Generates permission service source files for all matching classes and writes them to the specified directory.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="outputDirectory">The target <see cref="DirectoryInfo" />.</param>
        /// <param name="templateName">The name of the template to execute.</param>
        /// <param name="isInterface">A value indicating whether generating interfaces.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        private async Task GeneratePermissionServiceBatchAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory, string templateName, bool isInterface)
        {
            var template = this.Templates[templateName];
            var classes = QueryPermissionServiceClasses(xmiReaderResult);

            foreach (var @class in classes)
            {
                var prefix = isInterface ? "I" : string.Empty;
                var fileName = $"{prefix}{@class.Name.CapitalizeFirstLetter()}PermissionService.cs";
                var generated = this.CodeCleanup(template(@class));

                await WriteAsync(generated, outputDirectory, fileName);
            }
        }
    }
}
