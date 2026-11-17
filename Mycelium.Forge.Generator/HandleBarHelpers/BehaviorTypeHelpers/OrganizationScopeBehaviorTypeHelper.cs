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
    using Mycelium.Forge.Generator.Extensions;
    using Mycelium.Forge.Generator.Models;

    using uml4net.StructuredClassifiers;

    /// <summary>
    /// Generates permission verification hooks and dependency injection for entities that can be owned by either
    /// an account (personal scope) or an organization, evaluating scope boundaries and organization membership.
    /// </summary>
    public class OrganizationScopeBehaviorTypeHelper : ScopedBehaviorTypeHelperBase<OrganizationScopeConfiguration>
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
        /// Writes the create permission verification implementation body.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to write code into.</param>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The behavior definition.</param>
        public override void WriteIsAllowedToCreate(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior)
        {
            var config = this.GetConfiguration(definition, behavior);
            var entityLower = @class.Name.ToLowerInvariant();
            var membershipChecks = config.ScopeMemberProperties.Select(prop => $"!organization.{prop}.Contains(userContext.AccountId.Value)");

            stringBuilder.Append($$"""
                                               if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                               {
                                                   return Error.Unauthorized(description: "Unauthenticated user cannot create a {{entityLower}}.");
                                               }

                                               if (toCreate.Owner == userContext.AccountId.Value)
                                               {
                                                   return PermissionGuard.GuardPermission(userContext, PermissionKind.{{config.PersonalCreatePermission}});
                                               }
                                   """);

            if (!string.IsNullOrWhiteSpace(config.AdminPermission))
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine();

                stringBuilder.Append($$"""
                                               if (PermissionGuard.HasPermission(userContext, PermissionKind.{{config.AdminPermission}}))
                                               {
                                                   return Result.Success;
                                               }
                                       """);
            }

            stringBuilder.AppendLine();
            stringBuilder.AppendLine();

            stringBuilder.Append($$"""
                                               var orgResult = await this.{{config.ScopeServiceField}}.ReadAsync(userContext, transaction, CancellationToken.None, [toCreate.Owner]);

                                               if (orgResult.IsError || orgResult.Value.Count == 0)
                                               {
                                                   return Error.Forbidden(description: "Access denied: target {{config.ScopeEntity.ToLowerInvariant()}} was not found or is not accessible.");
                                               }

                                               var organization = orgResult.Value[0];

                                               if ({{string.Join(" && ", membershipChecks)}})
                                               {
                                                   return Error.Forbidden(description: "Access denied: user is not a member of the target {{config.ScopeEntity.ToLowerInvariant()}}.");
                                               }

                                               return Result.Success;
                                   """);
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
            var entityLower = @class.Name.ToLowerInvariant();
            var bypassCheck = PermissionStatementHelper.FormatHasPermissionCheck(config.BypassPermissions);
            var scopeVar = config.ScopeVar;
            var scopeAdminChecks = config.ScopeAdminProperties.Select(prop => $"{scopeVar}.{prop}.Contains(accountId)").ToArray();
            var internalMemberProps = config.ScopeAdminProperties.Length > 0 ? config.ScopeMemberProperties.Where(prop => !config.ScopeAdminProperties.Contains(prop, StringComparer.OrdinalIgnoreCase)).ToArray() : config.ScopeMemberProperties;
            var scopeRoleChecks = internalMemberProps.Select(prop => $"{scopeVar}.{prop}.Contains(accountId)").ToArray();

            var ownershipChecks = new List<string> { $"thing.{PropertyNames.Owner} == accountId" };

            if (!string.IsNullOrWhiteSpace(config.OwnerProperty) && !config.OwnerProperty.Equals(PropertyNames.Owner, StringComparison.OrdinalIgnoreCase))
            {
                ownershipChecks.Add($"thing.{config.OwnerProperty}.Contains(accountId)");
            }

            if (!string.IsNullOrWhiteSpace(definition.MaintainerProperty))
            {
                ownershipChecks.Add($"thing.{definition.MaintainerProperty}.Contains(accountId)");
            }

            stringBuilder.Append($$"""
                                               if (thing.{{config.VisibilityProperty}} == VisibilityKind.PUBLIC)
                                               {
                                                   return Result.Success;
                                               }

                                               if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                               {
                                                   return Error.Unauthorized(description: "Unauthenticated user cannot access non-public {{entityLower}}.");
                                               }

                                               var accountId = userContext.AccountId.Value;

                                               if ({{string.Join(" || ", ownershipChecks)}})
                                               {
                                                   return Result.Success;
                                               }
                                   """);

            if (!string.IsNullOrEmpty(bypassCheck))
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine();

                stringBuilder.Append($$"""
                                               if ({{bypassCheck}})
                                               {
                                                   return Result.Success;
                                               }
                                       """);
            }

            stringBuilder.AppendLine();
            stringBuilder.AppendLine();

            if (config.ScopeAdminProperties.Length > 0)
            {
                var adminCheckExpr = string.Join(" || ", scopeAdminChecks);

                var memberCheckBlock = scopeRoleChecks.Length > 0
                    ? $$"""


                                            if (thing.{{config.VisibilityProperty}} == VisibilityKind.INTERNAL && ({{string.Join(" || ", scopeRoleChecks)}}))
                                            {
                                                return Result.Success;
                                            }
                        """
                    : string.Empty;

                stringBuilder.Append($$"""
                                                   var orgResult = await this.{{config.ScopeServiceField}}.ReadAsync(userContext, transaction, CancellationToken.None, [thing.Owner]);

                                                   if (!orgResult.IsError && orgResult.Value.Count > 0)
                                                   {
                                                       var {{scopeVar}} = orgResult.Value[0];

                                                       if ({{adminCheckExpr}})
                                                       {
                                                           return Result.Success;
                                                       }{{memberCheckBlock}}
                                                   }

                                                   if (thing.{{config.VisibilityProperty}} == VisibilityKind.INTERNAL)
                                                   {
                                                       return Error.Forbidden(description: "Access denied: user is not a member of the owning {{config.ScopeEntity.ToLowerInvariant()}}.");
                                                   }

                                                   return Error.Forbidden(description: "Access denied: cannot view this {{entityLower}}.");
                                       """);
            }
            else
            {
                stringBuilder.Append($$"""
                                                   if (thing.{{config.VisibilityProperty}} == VisibilityKind.INTERNAL)
                                                   {
                                                       var orgResult = await this.{{config.ScopeServiceField}}.ReadAsync(userContext, transaction, CancellationToken.None, [thing.Owner]);

                                                       if (!orgResult.IsError && orgResult.Value.Count > 0)
                                                       {
                                                           var {{scopeVar}} = orgResult.Value[0];

                                                           if ({{string.Join(" || ", scopeRoleChecks)}})
                                                           {
                                                               return Result.Success;
                                                           }
                                                       }

                                                       return Error.Forbidden(description: "Access denied: user is not a member of the owning {{config.ScopeEntity.ToLowerInvariant()}}.");
                                                   }

                                                   return Error.Forbidden(description: "Access denied: cannot view this {{entityLower}}.");
                                       """);
            }
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
            var entityLower = @class.Name.ToLowerInvariant();

            stringBuilder.AppendLine($$"""
                                                   if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                                   {
                                                       return Error.Unauthorized(description: "Unauthenticated user cannot update a {{entityLower}}.");
                                                   }

                                                   var accountId = userContext.AccountId.Value;

                                       """);

            if (definition != null)
            {
                PermissionHelper.WritePropertyOwnershipCheck(stringBuilder, @class, definition.OwnerProperty, "existingThing", true, false);
            }

            if (!string.IsNullOrWhiteSpace(config.AdminPermission))
            {
                stringBuilder.AppendLine($$"""
                                                       if (PermissionGuard.HasPermission(userContext, PermissionKind.{{config.AdminPermission}}))
                                                       {
                                                           return Result.Success;
                                                       }

                                           """);
            }

            WriteScopeAdminCheck(stringBuilder, config, "existingThing");

            if (propertyDefinitions != null)
            {
                PermissionHelper.WritePropertyChangePermissionChecks(stringBuilder, @class, propertyDefinitions, true);
            }

            PermissionHelper.AppendPermissionGuardOrOk(stringBuilder, definition?.UpdatePermission, true);
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
            var entityLower = @class.Name.ToLowerInvariant();

            stringBuilder.AppendLine($$"""
                                                   if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                                   {
                                                       return Error.Unauthorized(description: "Unauthenticated user cannot delete a {{entityLower}}.");
                                                   }

                                                   var accountId = userContext.AccountId.Value;

                                       """);

            if (definition != null)
            {
                PermissionHelper.WritePropertyOwnershipCheck(stringBuilder, @class, definition.OwnerProperty, "thing", true, false);
            }

            if (!string.IsNullOrWhiteSpace(definition?.DeletePermission))
            {
                var deleteCheck = PermissionStatementHelper.FormatHasPermissionCheck(definition.DeletePermission.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));

                stringBuilder.AppendLine($$"""
                                                       if ({{deleteCheck}})
                                                       {
                                                           return Result.Success;
                                                       }

                                           """);
            }

            WriteScopeAdminCheck(stringBuilder, config, "thing");
            stringBuilder.Append($"            return Error.Forbidden(description: \"Access denied: cannot delete this {entityLower}.\");");
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
            var entityName = behavior.EntityName;
            var config = this.GetConfiguration(definition, behavior);

            var ownerChecks = new List<string>
            {
                $"{entityName.ToSnakeCase()}.owner = @callerAccountId"
            };

            if (!config.OwnerProperty.Equals(PropertyNames.Owner, StringComparison.OrdinalIgnoreCase) &&
                !config.OwnerProperty.Equals(PropertyNames.Id, StringComparison.OrdinalIgnoreCase))
            {
                ownerChecks.Add(ManyToManyPredicateBuilder.BuildCallerAccountExistsClause(entityName, config.OwnerProperty, entityName, PropertyNames.Id));
            }

            if (!string.IsNullOrWhiteSpace(definition.MaintainerProperty))
            {
                ownerChecks.Add(ManyToManyPredicateBuilder.BuildCallerAccountExistsClause(entityName, definition.MaintainerProperty, entityName, PropertyNames.Id));
            }

            foreach (var adminProp in config.ScopeAdminProperties)
            {
                ownerChecks.Add(ManyToManyPredicateBuilder.BuildCallerAccountExistsClause(config.ScopeEntity, adminProp, entityName, PropertyNames.Owner));
            }

            var ownerChecksSql = string.Join("\r\n                        OR ", ownerChecks);
            var bypassSql = string.Join(" OR ", config.BypassPermissions.Select(p => $"@can{p} = true"));

            var internalMemberProps = config.ScopeAdminProperties.Length > 0
                ? config.ScopeMemberProperties.Where(p => !config.ScopeAdminProperties.Contains(p, StringComparer.OrdinalIgnoreCase)).ToArray()
                : config.ScopeMemberProperties;

            var scopeLinkChecks = (internalMemberProps.Length > 0 ? internalMemberProps : config.ScopeMemberProperties)
                .Select(prop => ManyToManyPredicateBuilder.BuildCallerAccountExistsClause(config.ScopeEntity, prop, entityName, PropertyNames.Owner));

            var scopeLinksSql = string.Join("\r\n                                        OR ", scopeLinkChecks);

            return $$"""
                                         (thing.data->>'{{config.VisibilityPropertyLower}}' = 'PUBLIC')
                                         OR (@callerAccountId IS NOT NULL AND (
                                             {{ownerChecksSql}}
                                             OR {{bypassSql}}
                                             OR (
                                                 thing.data->>'{{config.VisibilityPropertyLower}}' = 'INTERNAL'
                                                 AND (
                                                     {{scopeLinksSql}}
                                                 )
                                             )
                                         ))
                     """;
        }

        /// <summary>
        /// Factory method to create a new <see cref="OrganizationScopeConfiguration" /> instance.
        /// </summary>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The entity behavior definition.</param>
        /// <returns>A new <see cref="OrganizationScopeConfiguration" /> instance.</returns>
        protected override OrganizationScopeConfiguration CreateConfiguration(EntityPermissionDefinition definition, EntityBehaviorDefinition behavior)
        {
            return new OrganizationScopeConfiguration(definition, behavior);
        }

        /// <summary>
        /// Writes a scope administrator ownership check for the specified target entity parameter when configured.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to write code into.</param>
        /// <param name="config">The organization scope configuration.</param>
        /// <param name="targetEntityName">The parameter name of the target entity instance.</param>
        private static void WriteScopeAdminCheck(StringBuilder stringBuilder, OrganizationScopeConfiguration config, string targetEntityName)
        {
            if (config.ScopeAdminProperties.Length == 0)
            {
                return;
            }

            var scopeVar = config.ScopeVar;
            var adminChecks = string.Join(" || ", config.ScopeAdminProperties.Select(prop => $"{scopeVar}.{prop}.Contains(accountId)"));

            stringBuilder.AppendLine($$"""
                                                   var orgResult = await this.{{config.ScopeServiceField}}.ReadAsync(userContext, transaction, CancellationToken.None, [{{targetEntityName}}.Owner]);

                                                   if (!orgResult.IsError && orgResult.Value.Count > 0)
                                                   {
                                                       var {{scopeVar}} = orgResult.Value[0];

                                                       if ({{adminChecks}})
                                                       {
                                                           return Result.Success;
                                                       }
                                                   }

                                       """);
        }
    }
}
