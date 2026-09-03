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
    using System.Text;

    using Mycelium.Forge.Generator.Constants;
    using Mycelium.Forge.Generator.DataLoaders.PermissionModels;
    using Mycelium.Forge.Generator.Models;

    using uml4net.StructuredClassifiers;

    /// <summary>
    /// Generates permission verification hooks and dependency injection for scope-owned child entities
    /// such as addresses and profile links that delegate management to <c>ScopeItemPermissionHelper</c>.
    /// </summary>
    public class ScopeItemBehaviorTypeHelper : IBehaviorTypeHelper
    {
        /// <summary>
        /// Determines whether this behavior helper handles the specified operation.
        /// </summary>
        /// <param name="operation">The permission operation.</param>
        /// <returns><c>true</c> if the behavior handles the operation; otherwise <c>false</c>.</returns>
        public bool HandlesOperation(Operations operation)
        {
            return true;
        }

        /// <summary>
        /// Determines whether the specified operation requires an asynchronous implementation hook.
        /// </summary>
        /// <param name="operation">The permission operation.</param>
        /// <returns><c>true</c> if the operation is asynchronous; otherwise <c>false</c>.</returns>
        public bool IsAsyncMethod(Operations operation)
        {
            return true;
        }

        /// <summary>
        /// Writes fields, constructors, and dependency injection parameters for the entity class.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to write code into.</param>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The behavior definition.</param>
        public void WriteFieldsAndConstructors(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior)
        {
            var config = new ScopeItemConfiguration(definition, behavior);
            var scopeService = $"I{config.ScopeEntity}Service";

            stringBuilder.AppendLine($$"""
                                               /// <summary>
                                               /// The (injected) <see cref="{{scopeService}}" /> domain service.
                                               /// </summary>
                                               private readonly {{scopeService}} {{config.ScopeServiceField}};

                                               /// <summary>
                                               /// Initializes a new instance of the <see cref="{{@class.Name}}PermissionService"/> class.
                                               /// </summary>
                                               public {{@class.Name}}PermissionService()
                                               {
                                               }

                                               /// <summary>
                                               /// Initializes a new instance of the <see cref="{{@class.Name}}PermissionService"/> class.
                                               /// </summary>
                                               /// <param name="{{config.ScopeServiceField}}">The (injected) <see cref="{{scopeService}}" /> domain service.</param>
                                               public {{@class.Name}}PermissionService({{scopeService}} {{config.ScopeServiceField}})
                                               {
                                                   this.{{config.ScopeServiceField}} = {{config.ScopeServiceField}};
                                               }
                                       """);
        }

        /// <summary>
        /// Writes the create permission verification implementation body.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to write code into.</param>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The behavior definition.</param>
        public void WriteIsAllowedToCreate(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior)
        {
            var config = new ScopeItemConfiguration(definition, behavior);
            stringBuilder.Append($"            return await ScopeItemPermissionHelper.IsAllowedToManageScopeItem(userContext, toCreate.{config.OwnerProperty}, this.{config.ScopeServiceField}, \"\"\"{@class.Name.ToLowerInvariant()}\"\"\");");
        }

        /// <summary>
        /// Writes the read permission verification implementation body.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to write code into.</param>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The behavior definition.</param>
        public void WriteIsAllowedToRead(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior)
        {
            var config = new ScopeItemConfiguration(definition, behavior);
            stringBuilder.Append($"            return await ScopeItemPermissionHelper.IsAllowedToReadScopeItem(userContext, thing.{config.OwnerProperty}, this.{config.ScopeServiceField});");
        }

        /// <summary>
        /// Writes the update permission verification implementation body.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to write code into.</param>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The behavior definition.</param>
        /// <param name="propertyDefinitions">The list of property-level permission definitions for this entity.</param>
        public void WriteIsAllowedToUpdate(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior, List<PropertyPermissionDefinition> propertyDefinitions)
        {
            var config = new ScopeItemConfiguration(definition, behavior);
            stringBuilder.Append($"            return await ScopeItemPermissionHelper.IsAllowedToManageScopeItem(userContext, existingThing.{config.OwnerProperty}, this.{config.ScopeServiceField}, \"\"\"{@class.Name.ToLowerInvariant()}\"\"\");");
        }

        /// <summary>
        /// Writes the delete permission verification implementation body.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to write code into.</param>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The behavior definition.</param>
        public void WriteIsAllowedToDelete(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior)
        {
            var config = new ScopeItemConfiguration(definition, behavior);
            stringBuilder.Append($"            return await ScopeItemPermissionHelper.IsAllowedToManageScopeItem(userContext, thing.{config.OwnerProperty}, this.{config.ScopeServiceField}, \"\"\"{@class.Name.ToLowerInvariant()}\"\"\");");
        }

        /// <summary>
        /// Builds the SQL read filter predicate for an entity configured with this behavior.
        /// </summary>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The behavior definition.</param>
        /// <param name="resolveEntityPredicate">A delegate to resolve the SQL read filter predicate of another entity by name.</param>
        /// <returns>The SQL predicate string, or empty string if unrestricted.</returns>
        public string BuildReadFilterPredicate(IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior, Func<string, string> resolveEntityPredicate)
        {
            var entityName = behavior.EntityName;
            var config = new ScopeItemConfiguration(definition, behavior);
            var bypassSql = string.Join(" OR ", config.ReadBypassPermissions.Select(p => $"@can{p} = true"));

            return $$"""
                                 {{bypassSql}}
                                 OR (@callerAccountId IS NOT NULL AND (
                                     "{{entityName}}"."{{config.OwnerColumn}}" = @callerAccountId
                                     OR EXISTS (SELECT 1 FROM "Forge"."{{config.ScopeEntity}}_member__Account" WHERE "source{{config.ScopeEntity}}" = "{{entityName}}"."{{config.OwnerColumn}}" AND "targetAccount" = @callerAccountId)
                                     OR EXISTS (SELECT 1 FROM "Forge"."{{config.ScopeEntity}}_administrator__Account" WHERE "source{{config.ScopeEntity}}" = "{{entityName}}"."{{config.OwnerColumn}}" AND "targetAccount" = @callerAccountId)
                                 ))
                     """;
        }
    }
}
