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
    public class ParentDelegationBehaviorTypeHelper : IBehaviorTypeHelper
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
            var config = new ParentDelegationConfiguration(definition, behavior);

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
            var config = new ParentDelegationConfiguration(definition, behavior);

            var guardCheck = !string.IsNullOrWhiteSpace(config.CreatePermission)
                ? $$"""
                                var guard = PermissionGuard.GuardPermission(userContext, PermissionKind.{{config.CreatePermission}});

                                if (guard.IsFailed)
                                {
                                    return guard;
                                }

                    """
                : string.Empty;

            var ownershipChecks = new List<string> { $"{config.ParentVar}.Owner == accountId" };

            foreach (var prop in config.ParentOwnerProperties)
            {
                if (!prop.Equals("Owner", StringComparison.OrdinalIgnoreCase))
                {
                    ownershipChecks.Add($"{config.ParentVar}.{prop}.Contains(accountId)");
                }
            }

            stringBuilder.Append($$""""
                                               if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                               {
                                                   return Result.Fail("""Unauthenticated user cannot publish a {{@class.Name.ToLowerInvariant()}}.""");
                                               }

                                   {{guardCheck}}            var parentResult = await this.{{config.ParentServiceField}}.ReadAsync(userContext, CancellationToken.None, [toCreate.{{config.ParentKey}}]);

                                               if (parentResult.IsSuccess && parentResult.Value.Count > 0)
                                               {
                                                   var {{config.ParentVar}} = parentResult.Value[0];
                                                   var accountId = userContext.AccountId.Value;

                                                   if ({{string.Join(" || ", ownershipChecks)}})
                                                   {
                                                       return Result.Ok();
                                                   }

                                                   return Result.Fail("""Access denied: user is not an owner or maintainer of the {{config.ParentEntity.ToLowerInvariant()}}.""");
                                               }

                                               return Result.Ok();
                                   """");
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
            var config = new ParentDelegationConfiguration(definition, behavior);

            stringBuilder.Append($$""""
                                               var parentResult = await this.{{config.ParentServiceField}}.ReadAsync(userContext, CancellationToken.None, [thing.{{config.ParentKey}}]);

                                               if (parentResult.IsSuccess && parentResult.Value.Count > 0)
                                               {
                                                   return await this.{{config.ParentPermServiceField}}.IsAllowedToRead(userContext, parentResult.Value[0]);
                                               }

                                               return Result.Ok();
                                   """");
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
            var config = new ParentDelegationConfiguration(definition, behavior);

            var immutableProperties = @class.QueryDtoClassProperties()
                .Select(p => p.Name.CapitalizeFirstLetter())
                .Where(p => !p.Equals("Id", StringComparison.OrdinalIgnoreCase) && !p.Equals(config.StateProperty, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var immutableBlock = string.Empty;

            if (immutableProperties.Count > 0)
            {
                var diffChecks = immutableProperties.Select(p => $"existingThing.{p} != updatedThing.{p}");

                immutableBlock = $$""""
                                               if ({{string.Join(" ||\r\n                    ", diffChecks)}})
                                               {
                                                   return Result.Fail("""{{@class.Name}}s are immutable; only the {{config.StateProperty.ToLowerInvariant()}} status may be modified.""");
                                               }


                                   """";
            }

            var ownershipChecks = new List<string> { $"{config.ParentVar}.Owner == accountId" };

            foreach (var prop in config.ParentOwnerProperties)
            {
                if (!prop.Equals("Owner", StringComparison.OrdinalIgnoreCase))
                {
                    ownershipChecks.Add($"{config.ParentVar}.{prop}.Contains(accountId)");
                }
            }

            stringBuilder.Append($$""""
                                               if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                               {
                                                   return Result.Fail("""Unauthenticated user cannot update {{@class.Name.ToLowerInvariant()}}.""");
                                               }

                                   {{immutableBlock}}            if (existingThing.{{config.StateProperty}} != updatedThing.{{config.StateProperty}})
                                               {
                                                   var stateGuard = updatedThing.{{config.StateProperty}}
                                                       ? PermissionGuard.GuardPermission(userContext, PermissionKind.{{config.StateActivePermission}})
                                                       : PermissionGuard.GuardPermission(userContext, PermissionKind.{{config.StateInactivePermission}});

                                                   if (stateGuard.IsFailed)
                                                   {
                                                       return stateGuard;
                                                   }
                                               }

                                               var parentResult = await this.{{config.ParentServiceField}}.ReadAsync(userContext, CancellationToken.None, [existingThing.{{config.ParentKey}}]);

                                               if (parentResult.IsSuccess && parentResult.Value.Count > 0)
                                               {
                                                   var {{config.ParentVar}} = parentResult.Value[0];
                                                   var accountId = userContext.AccountId.Value;

                                                   if ({{string.Join(" || ", ownershipChecks)}})
                                                   {
                                                       return Result.Ok();
                                                   }

                                                   return Result.Fail("""Access denied: user is not an owner or maintainer of the {{config.ParentEntity.ToLowerInvariant()}}.""");
                                               }

                                               return Result.Ok();
                                   """");
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
            var config = new ParentDelegationConfiguration(definition, behavior);

            var eraseGuardBlock = !string.IsNullOrWhiteSpace(config.DeletePermission)
                ? $$"""
                                var eraseGuard = PermissionGuard.GuardPermission(userContext, PermissionKind.{{config.DeletePermission}});

                                if (eraseGuard.IsFailed)
                                {
                                    return eraseGuard;
                                }

                    """
                : string.Empty;

            var ownershipChecks = new List<string> { $"{config.ParentVar}.Owner == accountId" };

            foreach (var prop in config.ParentOwnerProperties)
            {
                if (!prop.Equals("Owner", StringComparison.OrdinalIgnoreCase))
                {
                    ownershipChecks.Add($"{config.ParentVar}.{prop}.Contains(accountId)");
                }
            }

            stringBuilder.Append($$""""
                                               if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                               {
                                                   return Result.Fail("""Unauthenticated user cannot erase a {{@class.Name.ToLowerInvariant()}}.""");
                                               }

                                   {{eraseGuardBlock}}            var parentResult = await this.{{config.ParentServiceField}}.ReadAsync(userContext, CancellationToken.None, [thing.{{config.ParentKey}}]);

                                               if (parentResult.IsSuccess && parentResult.Value.Count > 0)
                                               {
                                                   var {{config.ParentVar}} = parentResult.Value[0];
                                                   var accountId = userContext.AccountId.Value;

                                                   if ({{string.Join(" || ", ownershipChecks)}})
                                                   {
                                                       return Result.Ok();
                                                   }

                                                   return Result.Fail("""Access denied: only {{config.ParentEntity.ToLowerInvariant()}} owners can delete {{config.ParentEntity.ToLowerInvariant()}} versions.""");
                                               }

                                               return Result.Ok();
                                   """");
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
            var config = new ParentDelegationConfiguration(definition, behavior);

            var parentPredicate = resolveEntityPredicate?.Invoke(config.ParentEntity);

            if (string.IsNullOrWhiteSpace(parentPredicate))
            {
                throw new InvalidOperationException($"Could not resolve visibility predicate for parent entity '{config.ParentEntity}' referenced by '{entityName}'.");
            }

            var replaced = parentPredicate
                .Replace($"\"{config.ParentEntity}\"", $"\"Parent{config.ParentEntity}\"")
                .Replace("\"Thing\"", "\"ParentThing\"");

            var lines = replaced.Split(["\r\n", "\n"], StringSplitOptions.None);
            var indentedLines = lines.Select(line => string.IsNullOrWhiteSpace(line) ? line : $"          {line}");
            var indentedPredicate = string.Join("\r\n", indentedLines);

            return $$"""
                                 EXISTS (
                                     SELECT 1
                                     FROM "Forge"."{{config.ParentEntity}}" AS "Parent{{config.ParentEntity}}"
                                     INNER JOIN "Forge"."Thing" AS "ParentThing" ON "ParentThing"."id" = "Parent{{config.ParentEntity}}"."id"
                                     WHERE "Parent{{config.ParentEntity}}"."id" = "{{entityName}}"."{{config.ParentKeyColumn}}"
                                       AND (
                             {{indentedPredicate}}
                                       )
                                 )
                     """;
        }
    }
}
