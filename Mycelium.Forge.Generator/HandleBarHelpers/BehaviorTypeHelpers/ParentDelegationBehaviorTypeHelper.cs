// ------------------------------------------------------------------------------------------------
// <copyright file="ParentDelegationBehaviorTypeHelper.cs" company="Starion Group S.A.">
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
    using Mycelium.Forge.Generator.Extensions;
    using Mycelium.Forge.Generator.Models;

    using uml4net.Extensions;
    using uml4net.StructuredClassifiers;

    /// <summary>
    /// Generates permission verification hooks and dependency injection for child entities that delegate
    /// authorization to their owning parent entity (e.g., package versions delegating to packages).
    /// Fully configurable via behavior key-value pairs:
    /// <c>ParentEntity</c>, <c>ParentKey</c>, <c>ParentOwnerProperties</c>, <c>CreatePermission</c>,
    /// <c>DeletePermission</c>, <c>StateProperty</c>, <c>StateActivePermission</c>, <c>StateInactivePermission</c>.
    /// </summary>
    public class ParentDelegationBehaviorTypeHelper : BehaviorTypeHelperBase<ParentDelegationConfiguration>
    {
        /// <summary>
        /// Determines whether this behavior helper handles the specified operation.
        /// </summary>
        /// <param name="operation">The permission operation.</param>
        /// <returns><c>true</c> if the behavior handles the operation; otherwise <c>false</c>.</returns>
        public override bool HandlesOperation(Operations operation)
        {
            return true;
        }

        /// <summary>
        /// Determines whether the specified operation requires an asynchronous implementation hook.
        /// </summary>
        /// <param name="operation">The permission operation.</param>
        /// <returns><c>true</c> if the operation is asynchronous; otherwise <c>false</c>.</returns>
        public override bool IsAsyncMethod(Operations operation)
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
        public override void WriteFieldsAndConstructors(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior)
        {
            var config = this.GetConfiguration(definition, behavior);

            if (!config.OwnerScope.IsConfigured)
            {
                stringBuilder.AppendLine($$"""
                                                   /// <summary>
                                                   /// The (injected) <see cref="{{config.ParentServiceType}}" /> domain service.
                                                   /// </summary>
                                                   private readonly {{config.ParentServiceType}} {{config.ParentServiceField}};

                                                   /// <summary>
                                                   /// The (injected) <see cref="{{config.ParentPermServiceType}}" /> domain service.
                                                   /// </summary>
                                                   private readonly {{config.ParentPermServiceType}} {{config.ParentPermServiceField}};

                                                   /// <summary>
                                                   /// Initializes a new instance of the <see cref="{{@class.Name}}PermissionService"/> class.
                                                   /// </summary>
                                                   public {{@class.Name}}PermissionService()
                                                   {
                                                   }

                                                   /// <summary>
                                                   /// Initializes a new instance of the <see cref="{{@class.Name}}PermissionService"/> class.
                                                   /// </summary>
                                                   /// <param name="{{config.ParentServiceField}}">The (injected) <see cref="{{config.ParentServiceType}}" /> domain service.</param>
                                                   /// <param name="{{config.ParentPermServiceField}}">The (injected) <see cref="{{config.ParentPermServiceType}}" /> domain service.</param>
                                                   public {{@class.Name}}PermissionService({{config.ParentServiceType}} {{config.ParentServiceField}}, {{config.ParentPermServiceType}} {{config.ParentPermServiceField}})
                                                   {
                                                       this.{{config.ParentServiceField}} = {{config.ParentServiceField}};
                                                       this.{{config.ParentPermServiceField}} = {{config.ParentPermServiceField}};
                                                   }
                                           """);

                return;
            }

            stringBuilder.AppendLine($$"""
                                               /// <summary>
                                               /// The (injected) <see cref="{{config.ParentServiceType}}" /> domain service.
                                               /// </summary>
                                               private readonly {{config.ParentServiceType}} {{config.ParentServiceField}};

                                               /// <summary>
                                               /// The (injected) <see cref="{{config.ParentPermServiceType}}" /> domain service.
                                               /// </summary>
                                               private readonly {{config.ParentPermServiceType}} {{config.ParentPermServiceField}};

                                               /// <summary>
                                               /// The (injected) <see cref="{{config.OwnerScope.ServiceType}}" /> domain service.
                                               /// </summary>
                                               private readonly {{config.OwnerScope.ServiceType}} {{config.OwnerScope.ServiceField}};

                                               /// <summary>
                                               /// Initializes a new instance of the <see cref="{{@class.Name}}PermissionService"/> class.
                                               /// </summary>
                                               public {{@class.Name}}PermissionService()
                                               {
                                               }

                                               /// <summary>
                                               /// Initializes a new instance of the <see cref="{{@class.Name}}PermissionService"/> class.
                                               /// </summary>
                                               /// <param name="{{config.ParentServiceField}}">The (injected) <see cref="{{config.ParentServiceType}}" /> domain service.</param>
                                               /// <param name="{{config.ParentPermServiceField}}">The (injected) <see cref="{{config.ParentPermServiceType}}" /> domain service.</param>
                                               /// <param name="{{config.OwnerScope.ServiceField}}">The (injected) <see cref="{{config.OwnerScope.ServiceType}}" /> domain service.</param>
                                               public {{@class.Name}}PermissionService({{config.ParentServiceType}} {{config.ParentServiceField}}, {{config.ParentPermServiceType}} {{config.ParentPermServiceField}}, {{config.OwnerScope.ServiceType}} {{config.OwnerScope.ServiceField}})
                                               {
                                                   this.{{config.ParentServiceField}} = {{config.ParentServiceField}};
                                                   this.{{config.ParentPermServiceField}} = {{config.ParentPermServiceField}};
                                                   this.{{config.OwnerScope.ServiceField}} = {{config.OwnerScope.ServiceField}};
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
        public override void WriteIsAllowedToCreate(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior)
        {
            var config = this.GetConfiguration(definition, behavior);

            stringBuilder.Append($$"""
                                               if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                               {
                                                   return Error.Unauthorized(description: "Unauthenticated user cannot publish a {{@class.Name.ToLowerInvariant()}}.");
                                               }
                                   """);

            if (!string.IsNullOrWhiteSpace(config.CreatePermission))
            {
                stringBuilder.Append(ModelConstants.DoubleNewLine);

                stringBuilder.Append($$"""
                                                   var guard = PermissionGuard.GuardPermission(userContext, PermissionKind.{{config.CreatePermission}});

                                                   if (guard.IsError)
                                                   {
                                                       return guard;
                                                   }
                                       """);
            }

            stringBuilder.Append(GetOwnershipBlock(config, Operations.Create));
        }

        /// <summary>
        /// Writes the read permission verification implementation body.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to write code into.</param>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The behavior definition.</param>
        public override void WriteIsAllowedToRead(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior)
        {
            var config = this.GetConfiguration(definition, behavior);

            stringBuilder.Append($$"""
                                               var parentResult = await this.{{config.ParentServiceField}}.ReadAsync(userContext, transaction, CancellationToken.None, [thing.{{config.ParentKey}}]);

                                               if (!parentResult.IsError && parentResult.Value.Count > 0)
                                               {
                                                   return Result.Success;
                                               }

                                               return Error.Forbidden(description: "Access denied: parent {{config.ParentEntity.ToLowerInvariant()}} was not found or is not accessible.");
                                   """);
        }

        /// <summary>
        /// Writes the update permission verification implementation body.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to write code into.</param>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The behavior definition.</param>
        /// <param name="propertyDefinitions">The list of property-level permission definitions for this entity.</param>
        public override void WriteIsAllowedToUpdate(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior, List<PropertyPermissionDefinition> propertyDefinitions)
        {
            var config = this.GetConfiguration(definition, behavior);

            var immutableProperties = @class.QueryDtoClassProperties()
                .Where(p => !p.Name.Equals(PropertyNames.Id, StringComparison.OrdinalIgnoreCase) && !p.Name.Equals(PropertyNames.CreatedAt, StringComparison.OrdinalIgnoreCase) && !p.Name.Equals(PropertyNames.ModifiedAt, StringComparison.OrdinalIgnoreCase) && (string.IsNullOrWhiteSpace(config.StateProperty) || !p.Name.Equals(config.StateProperty, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            stringBuilder.Append($$"""
                                               if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                               {
                                                   return Error.Unauthorized(description: "Unauthenticated user cannot update {{@class.Name.ToLowerInvariant()}}.");
                                               }
                                   """);

            if (immutableProperties.Count > 0 && !string.IsNullOrWhiteSpace(config.StateProperty))
            {
                var diffChecks = immutableProperties.Select(p =>
                {
                    var name = p.Name.CapitalizeFirstLetter();
                    return p.QueryIsEnumerable() ? $"!existingThing.{name}.SequenceEqual(updatedThing.{name})" : $"existingThing.{name} != updatedThing.{name}";
                });

                stringBuilder.Append(ModelConstants.DoubleNewLine);

                stringBuilder.Append($$"""
                                                   if ({{string.Join(" ||\r\n                    ", diffChecks)}})
                                                   {
                                                       return Error.Forbidden(description: "{{@class.Name}}s are immutable; only the {{config.StateProperty.ToLowerInvariant()}} status may be modified.");
                                                   }
                                       """);
            }

            if (!string.IsNullOrWhiteSpace(config.StateProperty) && !string.IsNullOrWhiteSpace(config.StateActivePermission))
            {
                stringBuilder.Append(ModelConstants.DoubleNewLine);

                stringBuilder.Append($$"""
                                                   if (existingThing.{{config.StateProperty}} != updatedThing.{{config.StateProperty}})
                                                   {
                                                       var stateGuard = updatedThing.{{config.StateProperty}}
                                                           ? PermissionGuard.GuardPermission(userContext, PermissionKind.{{config.StateActivePermission}})
                                                           : PermissionGuard.GuardPermission(userContext, PermissionKind.{{config.StateInactivePermission}});

                                                       if (stateGuard.IsError)
                                                       {
                                                           return stateGuard;
                                                       }
                                                   }
                                       """);
            }

            stringBuilder.Append(GetOwnershipBlock(config, Operations.Update));
        }

        /// <summary>
        /// Writes the delete permission verification implementation body.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to write code into.</param>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The behavior definition.</param>
        public override void WriteIsAllowedToDelete(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior)
        {
            var config = this.GetConfiguration(definition, behavior);

            stringBuilder.Append($$"""
                                               if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                               {
                                                   return Error.Unauthorized(description: "Unauthenticated user cannot erase a {{@class.Name.ToLowerInvariant()}}.");
                                               }
                                   """);

            if (!string.IsNullOrWhiteSpace(config.DeletePermission))
            {
                stringBuilder.Append(ModelConstants.DoubleNewLine);

                stringBuilder.Append($$"""
                                                   var eraseGuard = PermissionGuard.GuardPermission(userContext, PermissionKind.{{config.DeletePermission}});

                                                   if (eraseGuard.IsError)
                                                   {
                                                       return eraseGuard;
                                                   }
                                       """);
            }

            stringBuilder.Append(ModelConstants.DoubleNewLine);
            stringBuilder.Append(BuildParentOwnershipBlock(config, $"thing.{config.ParentKey}", "IsAllowedToDelete(userContext, parentResult.Value[0], transaction)", $"Access denied: only {config.ParentEntity.ToLowerInvariant()} owners can delete {config.ParentEntity.ToLowerInvariant()} versions.", !string.IsNullOrWhiteSpace(config.DeletePermission)));
        }

        /// <summary>
        /// Builds the SQL read filter predicate for an entity configured with this behavior.
        /// </summary>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The behavior definition.</param>
        /// <param name="resolveEntityPredicate">A delegate to resolve the SQL read filter predicate of another entity by name.</param>
        /// <returns>The SQL predicate string, or empty string if unrestricted.</returns>
        public override string BuildReadFilterPredicate(IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior, Func<string, string> resolveEntityPredicate)
        {
            return string.Empty;
        }

        /// <summary>
        /// Factory method to create a new <see cref="ParentDelegationConfiguration" /> instance.
        /// </summary>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The entity behavior definition.</param>
        /// <returns>A new <see cref="ParentDelegationConfiguration" /> instance.</returns>
        protected override ParentDelegationConfiguration CreateConfiguration(EntityPermissionDefinition definition, EntityBehaviorDefinition behavior)
        {
            return new ParentDelegationConfiguration(definition, behavior);
        }

        /// <summary>
        /// Builds the ownership verification block for create and update operations.
        /// </summary>
        /// <param name="config">The parent delegation configuration.</param>
        /// <param name="operation">The operation being performed.</param>
        /// <returns>The ownership verification block string.</returns>
        private static string GetOwnershipBlock(ParentDelegationConfiguration config, Operations operation)
        {
            var parentIdExpr = $"{(operation == Operations.Create ? "toCreate" : "existingThing")}.{config.ParentKey}";
            var fallbackExpr = "IsAllowedToUpdate(userContext, parentResult.Value[0], parentResult.Value[0], transaction)";
            var forbiddenMessage = $"Access denied: user is not an owner or maintainer of the {config.ParentEntity.ToLowerInvariant()}.";

            return ModelConstants.DoubleNewLine + BuildParentOwnershipBlock(config, parentIdExpr, fallbackExpr, forbiddenMessage);
        }

        /// <summary>
        /// Builds the asynchronous parent lookup and ownership/owner-scope verification block.
        /// </summary>
        /// <param name="config">The parent delegation configuration.</param>
        /// <param name="parentIdExpression">The C# expression yielding the parent identifier.</param>
        /// <param name="fallbackMethodCall">The delegated permission method call when no direct owner properties are configured.</param>
        /// <param name="forbiddenMessage">The error message returned when ownership verification fails.</param>
        /// <param name="bypassParentOwnershipCheck">Whether to bypass direct parent ownership checks once parent existence is verified.</param>
        /// <returns>The formatted C# parent ownership verification block.</returns>
        private static string BuildParentOwnershipBlock(ParentDelegationConfiguration config, string parentIdExpression, string fallbackMethodCall, string forbiddenMessage, bool bypassParentOwnershipCheck = false)
        {
            var ownershipChecks = BuildOwnershipChecks(config);

            string ownershipBlock;

            if (ownershipChecks.Count > 0)
            {
                if (bypassParentOwnershipCheck)
                {
                    ownershipBlock = "                return Result.Success;";
                }
                else
                {
                    var ownerScopeBlock = config.OwnerScope.IsConfigured
                        ? "\r\n" + config.OwnerScope.BuildAsyncAccessCheck(config.ParentVar)
                        : string.Empty;

                    ownershipBlock = $$"""
                                                       var {{config.ParentVar}} = parentResult.Value[0];
                                                       var accountId = userContext.AccountId.Value;

                                                       if ({{string.Join(" || ", ownershipChecks)}})
                                                       {
                                                           return Result.Success;
                                                       }{{ownerScopeBlock}}

                                                       return Error.Forbidden(description: "{{forbiddenMessage}}");
                                       """;
                }
            }
            else
            {
                ownershipBlock = $"                return await this.{config.ParentPermServiceField}.{fallbackMethodCall};";
            }

            return $$"""
                                 var parentResult = await this.{{config.ParentServiceField}}.ReadAsync(userContext, transaction, CancellationToken.None, [{{parentIdExpression}}]);

                                 if (!parentResult.IsError && parentResult.Value.Count > 0)
                                 {
                     {{ownershipBlock}}
                                 }

                                 return Error.Forbidden(description: "Access denied: parent {{config.ParentEntity.ToLowerInvariant()}} was not found or is not accessible.");
                     """;
        }

        /// <summary>
        /// Builds the list of ownership check expressions for a parent entity.
        /// </summary>
        /// <param name="config">The parent delegation configuration.</param>
        /// <returns>A list of boolean expression strings representing ownership checks.</returns>
        private static List<string> BuildOwnershipChecks(ParentDelegationConfiguration config)
        {
            if (config.ParentOwnerProperties.Length == 0)
            {
                return [];
            }

            return [.. config.ParentOwnerProperties.Select(prop => prop.Equals(PropertyNames.Owner, StringComparison.OrdinalIgnoreCase) ? $"{config.ParentVar}.{PropertyNames.Owner} == accountId" : $"{config.ParentVar}.{prop}.Contains(accountId)")];
        }
    }
}
