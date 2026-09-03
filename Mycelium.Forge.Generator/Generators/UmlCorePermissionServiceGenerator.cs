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
        /// Gets the dictionary of entity permission definitions loaded from CSV.
        /// </summary>
        public Dictionary<string, EntityPermissionDefinition> EntityPermissions { get; private set; } = [];

        /// <summary>
        /// Gets the dictionary of property permission definitions loaded from CSV, grouped by entity name.
        /// </summary>
        public Dictionary<string, List<PropertyPermissionDefinition>> PropertyPermissions { get; private set; } = [];

        /// <summary>
        /// Gets the dictionary of entity behavior definitions loaded from CSV.
        /// </summary>
        public Dictionary<string, EntityBehaviorDefinition> EntityBehaviors { get; private set; } = [];

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
        /// Loads all configuration CSVs: entity permissions, property permissions, and entity behaviors.
        /// </summary>
        /// <param name="entityCsvPath">The path to the entity permissions CSV file.</param>
        /// <param name="propertyCsvPath">The optional path to the property permissions CSV file.</param>
        /// <param name="behaviorCsvPath">The optional path to the entity behaviors CSV file.</param>
        public void LoadConfigurations(string entityCsvPath, string propertyCsvPath = null, string behaviorCsvPath = null)
        {
            var entityLoader = new CsvEntityPermissionsDataLoader();
            this.EntityPermissions = entityLoader.Load(entityCsvPath);

            if (!string.IsNullOrWhiteSpace(propertyCsvPath) && File.Exists(propertyCsvPath))
            {
                var propertyLoader = new CsvPropertyPermissionsDataLoader();
                this.PropertyPermissions = propertyLoader.Load(propertyCsvPath);
            }

            if (!string.IsNullOrWhiteSpace(behaviorCsvPath) && File.Exists(behaviorCsvPath))
            {
                var behaviorLoader = new CsvEntityBehaviorsDataLoader();
                this.EntityBehaviors = behaviorLoader.Load(behaviorCsvPath);
            }

            PermissionHelper.SetConfigurations(this.EntityPermissions, this.PropertyPermissions, this.EntityBehaviors);
        }

        /// <summary>
        /// Loads the entity permissions from the specified CSV file, and automatically discovers companion property and behavior
        /// CSVs.
        /// </summary>
        /// <param name="csvPath">The path to the entity permissions CSV file.</param>
        public void LoadEntityPermissions(string csvPath)
        {
            var directory = Path.GetDirectoryName(csvPath);
            var propertyCsv = !string.IsNullOrWhiteSpace(directory) ? Path.Combine(directory, "forge-property-permissions.csv") : null;
            var behaviorCsv = !string.IsNullOrWhiteSpace(directory) ? Path.Combine(directory, "forge-entity-behaviors.csv") : null;

            this.LoadConfigurations(csvPath, propertyCsv, behaviorCsv);
        }

        /// <summary>
        /// Validates that all permissions referenced in the entity permissions configuration exist in the role-permission model,
        /// and optionally checks that referenced entity properties exist on the UML classes in <paramref name="xmiReaderResult" />
        /// .
        /// </summary>
        /// <param name="model">The parsed role-permission model.</param>
        /// <param name="xmiReaderResult">The parsed UML model, or null if UML validation is not required.</param>
        public void ValidateConfiguration(RolePermissionModel model, XmiReaderResult xmiReaderResult = null)
        {
            ArgumentNullException.ThrowIfNull(model);

            var validPermissions = model.Permissions.Select(p => p.EnumName).ToHashSet(StringComparer.OrdinalIgnoreCase);
            List<string> errors = [];

            foreach (var keyValuePair in this.EntityPermissions)
            {
                var entityName = keyValuePair.Key;
                var definition = keyValuePair.Value;

                CheckPermissionExists(validPermissions, definition.CreatePermission, entityName, "Create", errors);
                CheckPermissionExists(validPermissions, definition.ReadPermission, entityName, "Read", errors);
                CheckPermissionExists(validPermissions, definition.UpdatePermission, entityName, "Update", errors);
                CheckPermissionExists(validPermissions, definition.DeletePermission, entityName, "Delete", errors);
            }

            foreach (var keyValuePair in this.PropertyPermissions)
            {
                var entityName = keyValuePair.Key;

                foreach (var propDef in keyValuePair.Value)
                {
                    CheckPermissionExists(validPermissions, propDef.RequiredPermission, entityName, $"Property '{propDef.Property}'", errors);
                }
            }

            if (xmiReaderResult != null)
            {
                var classes = QueryPermissionServiceClasses(xmiReaderResult).ToDictionary(c => c.Name, StringComparer.OrdinalIgnoreCase);

                foreach (var keyValuePair in this.EntityPermissions)
                {
                    var entityName = keyValuePair.Key;
                    var definition = keyValuePair.Value;

                    if (!classes.TryGetValue(entityName, out var @class))
                    {
                        continue;
                    }

                    var allProperties = @class.QueryDtoClassProperties().Select(p => p.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

                    CheckPropertyExists(allProperties, definition.OwnerProperty, entityName, nameof(definition.OwnerProperty), errors);
                    CheckPropertyExists(allProperties, definition.MaintainerProperty, entityName, nameof(definition.MaintainerProperty), errors);
                    CheckPropertyExists(allProperties, definition.VisibilityProperty, entityName, nameof(definition.VisibilityProperty), errors);
                }

                foreach (var keyValuePair in this.PropertyPermissions)
                {
                    var entityName = keyValuePair.Key;

                    if (!classes.TryGetValue(entityName, out var @class))
                    {
                        continue;
                    }

                    var allProperties = @class.QueryDtoClassProperties().Select(p => p.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

                    foreach (var propDef in keyValuePair.Value)
                    {
                        CheckPropertyExists(allProperties, propDef.Property, entityName, nameof(PropertyPermissionDefinition.Property), errors);
                    }
                }
            }

            if (errors.Count > 0)
            {
                throw new InvalidOperationException($"Entity permission validation failed with {errors.Count} error(s):{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
            }
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

            this.ValidateConfiguration(model);

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
        /// Generates the permission service class for a single, named <see cref="IClass" />, without necessarily
        /// writing it to disk; the rendered text is returned so it can be verified in tests.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> that contains the UML model to generate from.</param>
        /// <param name="className">The name of the class to generate.</param>
        /// <returns>An awaitable <see cref="Task{String}" /> with the generated code.</returns>
        public Task<string> GeneratePermissionServiceClassAsync(XmiReaderResult xmiReaderResult, string className)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentException.ThrowIfNullOrWhiteSpace(className);

            var template = this.Templates[PermissionServiceClassTemplateName];
            var classToGenerate = QueryPermissionServiceClasses(xmiReaderResult).Single(x => x.Name == className);
            var generated = this.CodeCleanup(template(classToGenerate));

            return Task.FromResult(generated);
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
        /// Verifies that the specified permission name exists in the set of valid permission enum names.
        /// </summary>
        /// <param name="validPermissions">The set of valid permission names.</param>
        /// <param name="permissionName">The permission name to verify.</param>
        /// <param name="entityName">The entity name associated with the permission check.</param>
        /// <param name="operation">The operation or property role being checked.</param>
        /// <param name="errors">The list of error messages to collect.</param>
        private static void CheckPermissionExists(HashSet<string> validPermissions, string permissionName, string entityName, string operation, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(permissionName))
            {
                return;
            }

            var parts = permissionName.Split(['|', '&'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var part in parts)
            {
                if (!validPermissions.Contains(part))
                {
                    errors.Add($"Entity '{entityName}' references unknown {operation} permission '{part}'.");
                }
            }
        }

        /// <summary>
        /// Verifies that the specified property name exists in the set of valid class properties.
        /// </summary>
        /// <param name="allProperties">The set of valid class property names.</param>
        /// <param name="propertyName">The property name to verify.</param>
        /// <param name="entityName">The entity name associated with the property check.</param>
        /// <param name="role">The role or setting of the property being checked.</param>
        /// <param name="errors">The list of error messages to collect.</param>
        private static void CheckPropertyExists(HashSet<string> allProperties, string propertyName, string entityName, string role, List<string> errors)
        {
            if (!string.IsNullOrWhiteSpace(propertyName) && !allProperties.Contains(propertyName))
            {
                errors.Add($"Entity '{entityName}' references property '{propertyName}' for {role}, but property does not exist on UML class.");
            }
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
