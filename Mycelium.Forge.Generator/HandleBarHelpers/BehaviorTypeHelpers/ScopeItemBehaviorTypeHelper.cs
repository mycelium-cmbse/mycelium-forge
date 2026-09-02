// ------------------------------------------------------------------------------------------------
// <copyright file="ScopeItemBehaviorTypeHelper.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.HandleBarHelpers.BehaviorTypeHelpers
{
    using System.Collections.Generic;
    using System.Text;

    using Mycelium.Forge.Generator.DataLoaders.PermissionModels;

    using uml4net.StructuredClassifiers;

    /// <summary>
    /// Generates permission verification hooks and dependency injection for scope-owned child entities
    /// such as addresses and profile links that delegate management to <c>ScopeItemPermissionHelper</c>.
    /// </summary>
    public class ScopeItemBehaviorTypeHelper : IBehaviorTypeHelper
    {
        /// <summary>
        /// Gets the behavior type name handled by this helper.
        /// </summary>
        public string BehaviorType => "ScopeItem";

        /// <summary>
        /// Determines whether the specified operation requires an asynchronous implementation hook.
        /// </summary>
        /// <param name="operation">The operation name ("Create", "Read", "Update", "Delete").</param>
        /// <returns><c>true</c> if the operation is asynchronous; otherwise <c>false</c>.</returns>
        public bool IsAsyncMethod(string operation)
        {
            return true;
        }

        /// <summary>
        /// Writes fields, constructors, and dependency injection parameters for the entity class.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to write code into.</param>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="behavior">The behavior definition.</param>
        public void WriteFieldsAndConstructors(StringBuilder stringBuilder, IClass @class, EntityBehaviorDefinition behavior)
        {
            stringBuilder.AppendLine("        /// <summary>");
            stringBuilder.AppendLine("        /// The (injected) <see cref=\"IOrganizationService\" /> domain service.");
            stringBuilder.AppendLine("        /// </summary>");
            stringBuilder.AppendLine("        private readonly IOrganizationService organizationService;");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("        /// <summary>");
            stringBuilder.AppendLine($"        /// Initializes a new instance of the <see cref=\"{@class.Name}PermissionService\"/> class.");
            stringBuilder.AppendLine("        /// </summary>");
            stringBuilder.AppendLine($"        public {@class.Name}PermissionService()");
            stringBuilder.AppendLine("        {");
            stringBuilder.AppendLine("        }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("        /// <summary>");
            stringBuilder.AppendLine($"        /// Initializes a new instance of the <see cref=\"{@class.Name}PermissionService\"/> class.");
            stringBuilder.AppendLine("        /// </summary>");
            stringBuilder.AppendLine("        /// <param name=\"organizationService\">The (injected) <see cref=\"IOrganizationService\" /> domain service.</param>");
            stringBuilder.AppendLine($"        public {@class.Name}PermissionService(IOrganizationService organizationService)");
            stringBuilder.AppendLine("        {");
            stringBuilder.AppendLine("            this.organizationService = organizationService;");
            stringBuilder.AppendLine("        }");
        }

        /// <summary>
        /// Writes the create permission verification implementation body.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to write code into.</param>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The behavior definition.</param>
        /// <param name="isAsync">Whether the enclosing method is asynchronous.</param>
        /// <returns><c>true</c> if the behavior handled the operation; otherwise <c>false</c>.</returns>
        public bool WriteIsAllowedToCreate(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior, bool isAsync)
        {
            var ownerProp = definition != null && !string.IsNullOrWhiteSpace(definition.OwnerProperty) ? definition.OwnerProperty : "Owner";
            stringBuilder.AppendLine("            if (toCreate == null)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine($"                return Result.Fail(\"{@class.Name} cannot be null.\");");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.Append($"            return await ScopeItemPermissionHelper.IsAllowedToManageScopeItem(userContext, toCreate.{ownerProp}, this.organizationService, \"{@class.Name.ToLowerInvariant()}\");");
            return true;
        }

        /// <summary>
        /// Writes the read permission verification implementation body.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to write code into.</param>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The behavior definition.</param>
        /// <param name="isAsync">Whether the enclosing method is asynchronous.</param>
        /// <returns><c>true</c> if the behavior handled the operation; otherwise <c>false</c>.</returns>
        public bool WriteIsAllowedToRead(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior, bool isAsync)
        {
            var ownerProp = definition != null && !string.IsNullOrWhiteSpace(definition.OwnerProperty) ? definition.OwnerProperty : "Owner";
            stringBuilder.AppendLine("            if (thing == null)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine($"                return Result.Fail(\"{@class.Name} cannot be null.\");");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.Append($"            return await ScopeItemPermissionHelper.IsAllowedToReadScopeItem(userContext, thing.{ownerProp}, this.organizationService);");
            return true;
        }

        /// <summary>
        /// Writes the update permission verification implementation body.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to write code into.</param>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The behavior definition.</param>
        /// <param name="propertyDefinitions">The list of property-level permission definitions for this entity.</param>
        /// <param name="isAsync">Whether the enclosing method is asynchronous.</param>
        /// <returns><c>true</c> if the behavior handled the operation; otherwise <c>false</c>.</returns>
        public bool WriteIsAllowedToUpdate(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior, List<PropertyPermissionDefinition> propertyDefinitions, bool isAsync)
        {
            var ownerProp = definition != null && !string.IsNullOrWhiteSpace(definition.OwnerProperty) ? definition.OwnerProperty : "Owner";
            stringBuilder.AppendLine("            if (existingThing == null || updatedThing == null)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine($"                return Result.Fail(\"{@class.Name} cannot be null.\");");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.Append($"            return await ScopeItemPermissionHelper.IsAllowedToManageScopeItem(userContext, existingThing.{ownerProp}, this.organizationService, \"{@class.Name.ToLowerInvariant()}\");");
            return true;
        }

        /// <summary>
        /// Writes the delete permission verification implementation body.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to write code into.</param>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The behavior definition.</param>
        /// <param name="isAsync">Whether the enclosing method is asynchronous.</param>
        /// <returns><c>true</c> if the behavior handled the operation; otherwise <c>false</c>.</returns>
        public bool WriteIsAllowedToDelete(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior, bool isAsync)
        {
            var ownerProp = definition != null && !string.IsNullOrWhiteSpace(definition.OwnerProperty) ? definition.OwnerProperty : "Owner";
            stringBuilder.AppendLine("            if (thing == null)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine($"                return Result.Fail(\"{@class.Name} cannot be null.\");");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.Append($"            return await ScopeItemPermissionHelper.IsAllowedToManageScopeItem(userContext, thing.{ownerProp}, this.organizationService, \"{@class.Name.ToLowerInvariant()}\");");
            return true;
        }
    }
}
