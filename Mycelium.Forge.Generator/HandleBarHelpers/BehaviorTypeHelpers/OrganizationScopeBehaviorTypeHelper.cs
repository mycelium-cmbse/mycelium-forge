// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationScopeBehaviorTypeHelper.cs" company="Starion Group S.A.">
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
    /// Generates permission verification hooks and dependency injection for entities that can be owned by either
    /// an account (personal scope) or an organization, evaluating scope boundaries and organization membership.
    /// </summary>
    public class OrganizationScopeBehaviorTypeHelper : IBehaviorTypeHelper
    {
        /// <summary>
        /// Determines whether this behavior helper handles the specified operation.
        /// </summary>
        /// <param name="operation">The permission operation.</param>
        /// <returns><c>true</c> if the behavior handles the operation; otherwise <c>false</c>.</returns>
        public bool HandlesOperation(Operations operation)
        {
            return operation is Operations.Create or Operations.Read;
        }

        /// <summary>
        /// Determines whether the specified operation requires an asynchronous implementation hook.
        /// </summary>
        /// <param name="operation">The permission operation.</param>
        /// <returns><c>true</c> if the operation is asynchronous; otherwise <c>false</c>.</returns>
        public bool IsAsyncMethod(Operations operation)
        {
            return operation is Operations.Create or Operations.Read;
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
            var config = new OrganizationScopeConfiguration(definition, behavior);
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
            var config = new OrganizationScopeConfiguration(definition, behavior);
            var entityLower = @class.Name.ToLowerInvariant();
            var membershipChecks = config.ScopeMemberProperties.Select(prop => $"!organization.{prop}.Contains(userContext.AccountId.Value)");

            stringBuilder.Append($$""""
                                               if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                               {
                                                   return Result.Fail("""Unauthenticated user cannot create a {{entityLower}}.""");
                                               }

                                               if (toCreate.Owner == userContext.AccountId.Value)
                                               {
                                                   return PermissionGuard.GuardPermission(userContext, PermissionKind.{{config.PersonalCreatePermission}});
                                               }

                                               if (PermissionGuard.HasPermission(userContext, PermissionKind.ManageOrganizations))
                                               {
                                                   return Result.Ok();
                                               }

                                               var orgGuard = PermissionGuard.GuardPermission(userContext, PermissionKind.{{config.OrgCreatePermission}});

                                               if (orgGuard.IsFailed)
                                               {
                                                   return orgGuard;
                                               }

                                               var orgResult = await this.{{config.ScopeServiceField}}.ReadAsync(userContext, CancellationToken.None, [toCreate.Owner]);

                                               if (orgResult.IsSuccess && orgResult.Value.Count > 0)
                                               {
                                                   var organization = orgResult.Value[0];

                                                   if ({{string.Join(" && ", membershipChecks)}})
                                                   {
                                                       return Result.Fail("""Access denied: user is not a member of the target {{config.ScopeEntity.ToLowerInvariant()}}.""");
                                                   }
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
            var config = new OrganizationScopeConfiguration(definition, behavior);
            var entityLower = @class.Name.ToLowerInvariant();
            var bypassChecks = config.BypassPermissions.Select(p => $"PermissionGuard.HasPermission(userContext, PermissionKind.{p})");
            var scopeRoleChecks = config.ScopeMemberProperties.Select(prop => $"organization.{prop}.Contains(accountId)");

            var ownershipChecks = new List<string> { "thing.Owner == accountId" };

            if (!string.IsNullOrWhiteSpace(config.OwnerProperty) && !config.OwnerProperty.Equals("Owner", StringComparison.OrdinalIgnoreCase))
            {
                ownershipChecks.Add($"thing.{config.OwnerProperty}.Contains(accountId)");
            }

            if (!string.IsNullOrWhiteSpace(definition?.MaintainerProperty))
            {
                ownershipChecks.Add($"thing.{definition.MaintainerProperty}.Contains(accountId)");
            }

            stringBuilder.Append($$""""
                                               if (thing.{{config.VisibilityProperty}} == VisibilityKind.PUBLIC)
                                               {
                                                   return Result.Ok();
                                               }

                                               if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                               {
                                                   return Result.Fail("""Unauthenticated user cannot access non-public {{entityLower}}.""");
                                               }

                                               var accountId = userContext.AccountId.Value;

                                               if ({{string.Join(" || ", ownershipChecks)}})
                                               {
                                                   return Result.Ok();
                                               }

                                               if (thing.{{config.VisibilityProperty}} == VisibilityKind.INTERNAL)
                                               {
                                                   if ({{string.Join(" || ", bypassChecks)}})
                                                   {
                                                       return Result.Ok();
                                                   }

                                                   var internalPermissionResult = PermissionGuard.GuardPermission(userContext, PermissionKind.{{config.InternalReadPermission}});

                                                   if (internalPermissionResult.IsFailed)
                                                   {
                                                       return internalPermissionResult;
                                                   }

                                                   var orgResult = await this.{{config.ScopeServiceField}}.ReadAsync(userContext, CancellationToken.None, [thing.Owner]);

                                                   if (orgResult.IsSuccess && orgResult.Value.Count > 0)
                                                   {
                                                       var organization = orgResult.Value[0];

                                                       if ({{string.Join(" || ", scopeRoleChecks)}})
                                                       {
                                                           return Result.Ok();
                                                       }
                                                   }

                                                   return Result.Fail("""Access denied: user is not a member of the owning {{config.ScopeEntity.ToLowerInvariant()}}.""");
                                               }

                                               if (thing.{{config.VisibilityProperty}} == VisibilityKind.PRIVATE)
                                               {
                                                   return PermissionGuard.GuardPermission(userContext, PermissionKind.{{config.PrivateReadPermission}});
                                               }

                                               return Result.Fail("""Access denied: cannot view this {{entityLower}}.""");
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
            // Empty because OrganizationScope entities do not require custom behavior logic for updates;
            // update permissions are handled by standard entity ownership, property-level permissions, and UpdatePermission in PermissionHelper.
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
            // Empty because OrganizationScope entities do not require custom behavior logic for deletion;
            // delete permissions are handled by standard entity ownership checks and DeletePermission in PermissionHelper.
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
            var config = new OrganizationScopeConfiguration(definition, behavior);

            var ownerChecks = new List<string>
            {
                $"\"{entityName}\".\"owner\" = @callerAccountId"
            };

            if (!config.OwnerProperty.Equals("Owner", StringComparison.OrdinalIgnoreCase) &&
                !config.OwnerProperty.Equals("Id", StringComparison.OrdinalIgnoreCase))
            {
                var ownerPropCamel = char.ToLowerInvariant(config.OwnerProperty[0]) + config.OwnerProperty[1..];
                ownerChecks.Add($"EXISTS (SELECT 1 FROM \"Forge\".\"{entityName}_{ownerPropCamel}__Account\" WHERE \"source{entityName}\" = \"{entityName}\".\"id\" AND \"targetAccount\" = @callerAccountId)");
            }

            if (!string.IsNullOrWhiteSpace(definition?.MaintainerProperty))
            {
                var maintainerPropCamel = char.ToLowerInvariant(definition.MaintainerProperty[0]) + definition.MaintainerProperty[1..];
                ownerChecks.Add($"EXISTS (SELECT 1 FROM \"Forge\".\"{entityName}_{maintainerPropCamel}__Account\" WHERE \"source{entityName}\" = \"{entityName}\".\"id\" AND \"targetAccount\" = @callerAccountId)");
            }

            var ownerChecksSql = string.Join("\r\n                        OR ", ownerChecks);
            var bypassSql = string.Join(" OR ", config.BypassPermissions.Select(p => $"@can{p} = true"));

            var scopeLinkChecks = config.ScopeMemberProperties.Select(prop =>
            {
                var propCamel = char.ToLowerInvariant(prop[0]) + prop[1..];
                return $"EXISTS (SELECT 1 FROM \"Forge\".\"{config.ScopeEntity}_{propCamel}__Account\" WHERE \"source{config.ScopeEntity}\" = \"{entityName}\".\"owner\" AND \"targetAccount\" = @callerAccountId)";
            });

            var scopeLinksSql = string.Join("\r\n                                        OR ", scopeLinkChecks);

            return $$"""
                                         ("Thing"."data"->>'{{config.VisibilityPropertyLower}}' = 'PUBLIC')
                                         OR (@callerAccountId IS NOT NULL AND (
                                             {{ownerChecksSql}}
                                             OR (
                                                 "Thing"."data"->>'{{config.VisibilityPropertyLower}}' = 'INTERNAL'
                                                 AND (
                                                     {{bypassSql}}
                                                     OR (
                                                         @can{{config.InternalReadPermission}} = true AND (
                                                             {{scopeLinksSql}}
                                                         )
                                                     )
                                                 )
                                             )
                                             OR (
                                                 "Thing"."data"->>'{{config.VisibilityPropertyLower}}' = 'PRIVATE'
                                                 AND @can{{config.PrivateReadPermission}} = true
                                             )
                                         ))
                     """;
        }
    }
}
