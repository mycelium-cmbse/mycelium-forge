// ------------------------------------------------------------------------------------------------
// <copyright file="InvitationWorkflowBehaviorTypeHelper.cs" company="Starion Group S.A.">
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
    /// Generates permission verification hooks and dependency injection for invitation entities governed by an
    /// invitation lifecycle workflow (e.g. OrganizationInvitation, PackageInvitation).
    /// Fully configurable via behavior key-value pairs:
    /// <c>ScopeEntity</c>, <c>ScopeProperty</c>, <c>ScopeRoles</c>, <c>InviteeProperty</c>,
    /// <c>CreatePermission</c>, <c>ReadPermission</c>, <c>AdminPermission</c>.
    /// </summary>
    public class InvitationWorkflowBehaviorTypeHelper : ScopedBehaviorTypeHelperBase<InvitationWorkflowConfiguration>
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
            var scopeRoleChecks = config.ScopeRoles.Select(role => $"{config.ScopeVar}.{role}.Contains(accountId)").ToList();

            stringBuilder.Append($$"""
                                               if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                               {
                                                   return Error.Unauthorized(description: "Unauthenticated user cannot create an invitation.");
                                               }

                                               var accountId = userContext.AccountId.Value;

                                               var guard = PermissionGuard.GuardPermission(userContext, PermissionKind.{{config.CreatePermission}});

                                               if (!guard.IsError)
                                               {
                                                   return Result.Success;
                                               }

                                               var scopeResult = await this.{{config.ScopeServiceField}}.ReadAsync(userContext, transaction, CancellationToken.None, [toCreate.{{config.ScopeProperty}}]);

                                               if (!scopeResult.IsError && scopeResult.Value.Count > 0)
                                               {
                                                   var {{config.ScopeVar}} = scopeResult.Value[0];

                                                   if ({{string.Join(" || ", scopeRoleChecks)}})
                                                   {
                                                       return Result.Success;
                                                   }
                                               }

                                               return Error.Forbidden(description: "Access denied: only {{config.ScopeRoleDescription}}s can invite members.");
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
            var scopeRoleChecks = config.ScopeRoles.Select(role => $"{config.ScopeVar}.{role}.Contains(accountId)").ToList();

            stringBuilder.Append($$"""
                                               if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                               {
                                                   return Error.Unauthorized(description: "Unauthenticated user cannot view an invitation.");
                                               }

                                               var accountId = userContext.AccountId.Value;

                                               if (thing.Owner == accountId || thing.{{config.InviteeProperty}} == accountId)
                                               {
                                                   return Result.Success;
                                               }

                                               var scopeResult = await this.{{config.ScopeServiceField}}.ReadAsync(userContext, transaction, CancellationToken.None, [thing.{{config.ScopeProperty}}]);

                                               if (!scopeResult.IsError && scopeResult.Value.Count > 0)
                                               {
                                                   var {{config.ScopeVar}} = scopeResult.Value[0];

                                                   if ({{string.Join(" || ", scopeRoleChecks)}})
                                                   {
                                                       return Result.Success;
                                                   }
                                               }

                                               return PermissionGuard.GuardPermission(userContext, PermissionKind.{{config.ReadPermission}});
                                   """);
        }

        /// <summary>
        /// Writes the update permission verification implementation body with state machine transitions.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to write code into.</param>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The behavior definition.</param>
        /// <param name="propertyDefinitions">The list of property-level permission definitions for this entity.</param>
        public override void WriteIsAllowedToUpdate(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior, List<PropertyPermissionDefinition> propertyDefinitions)
        {
            var config = this.GetConfiguration(definition, behavior);
            var isAsync = this.IsAsyncMethod(Operations.Update);
            var scopeRoleChecks = config.ScopeRoles.Select(role => $"{config.ScopeVar}.{role}.Contains(accountId)").ToList();
            var revokePermCheck = PermissionStatementHelper.FormatHasPermissionCheck(config.AdminPermission, config.CreatePermission);

            var failUnauth = PermissionStatementHelper.GetUnauthorizedReturn("\"Unauthenticated user cannot respond to an invitation.\"", isAsync);
            var failAlready = PermissionStatementHelper.GetFailReturn("$\"Cannot update invitation that is already {existingThing.Status}.\"", isAsync);
            var failExpired = PermissionStatementHelper.GetFailReturn("\"Cannot respond to an invitation that has expired.\"", isAsync);
            var failModified = PermissionStatementHelper.GetFailReturn("\"Cannot modify invitation details when accepting or rejecting an invitation.\"", isAsync);
            var failNotInvitee = PermissionStatementHelper.GetFailReturn("\"Access denied: only the invited target account can accept or reject the invitation.\"", isAsync);
            var failUnsupported = PermissionStatementHelper.GetFailReturn("$\"Unsupported invitation status transition to {updatedThing.Status}.\"", isAsync);
            var okParty = PermissionStatementHelper.GetOkReturn(isAsync);
            var okAdmin = PermissionStatementHelper.GetOkReturn(isAsync);
            var failNotParty = PermissionStatementHelper.GetFailReturn("\"Access denied: you are not a party to this invitation.\"", isAsync);

            stringBuilder.Append($$"""
                                               if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                               {
                                                   {{failUnauth}}
                                               }

                                               var accountId = userContext.AccountId.Value;

                                               if (existingThing.Status != InvitationStatusKind.PENDING)
                                               {
                                                   {{failAlready}}
                                               }

                                               if (existingThing.Status != updatedThing.Status)
                                               {
                                                   if (updatedThing.Status is InvitationStatusKind.ACCEPTED or InvitationStatusKind.REJECTED)
                                                   {
                                                       if (existingThing.ExperiesAt <= DateTime.UtcNow)
                                                       {
                                                           {{failExpired}}
                                                       }

                                                       if (existingThing.ExperiesAt != updatedThing.ExperiesAt ||
                                                           existingThing.{{config.EntityName}}Kind != updatedThing.{{config.EntityName}}Kind ||
                                                           existingThing.{{config.ScopeProperty}} != updatedThing.{{config.ScopeProperty}} ||
                                                           existingThing.{{config.InviteeProperty}} != updatedThing.{{config.InviteeProperty}} ||
                                                           existingThing.{{config.OwnerProperty}} != updatedThing.{{config.OwnerProperty}})
                                                       {
                                                           {{failModified}}
                                                       }

                                                       if (existingThing.{{config.InviteeProperty}} != accountId && !PermissionGuard.HasPermission(userContext, PermissionKind.{{config.AdminPermission}}))
                                                       {
                                                           {{failNotInvitee}}
                                                       }

                                                       {{okParty}}
                                                   }

                                                   if (updatedThing.Status == InvitationStatusKind.REVOKED)
                                                   {
                                                       if (existingThing.Owner == accountId || {{revokePermCheck}})
                                                       {
                                                           {{okParty}}
                                                       }

                                                       var scopeResult = await this.{{config.ScopeServiceField}}.ReadAsync(userContext, transaction, CancellationToken.None, [existingThing.{{config.ScopeProperty}}]);

                                                       if (!scopeResult.IsError && scopeResult.Value.Count > 0)
                                                       {
                                                           var {{config.ScopeVar}} = scopeResult.Value[0];

                                                           if ({{string.Join(" || ", scopeRoleChecks)}})
                                                           {
                                                               {{okParty}}
                                                           }
                                                       }

                                                       return Error.Forbidden(description: "Access denied: only the invitation creator or {{config.ScopeRoleDescription}}s can revoke the invitation.");
                                                   }

                                                   {{failUnsupported}}
                                               }

                                               if (existingThing.Owner == accountId)
                                               {
                                                   {{okParty}}
                                               }

                                               if (PermissionGuard.HasPermission(userContext, PermissionKind.{{config.AdminPermission}}))
                                               {
                                                   {{okAdmin}}
                                               }

                                               var updateScopeResult = await this.{{config.ScopeServiceField}}.ReadAsync(userContext, transaction, CancellationToken.None, [existingThing.{{config.ScopeProperty}}]);

                                               if (!updateScopeResult.IsError && updateScopeResult.Value.Count > 0)
                                               {
                                                   var {{config.ScopeVar}} = updateScopeResult.Value[0];

                                                   if ({{string.Join(" || ", scopeRoleChecks)}})
                                                   {
                                                       {{okParty}}
                                                   }
                                               }

                                               {{failNotParty}}
                                   """);
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
            var scopeRoleChecks = config.ScopeRoles.Select(role => $"{config.ScopeVar}.{role}.Contains(accountId)").ToList();
            var deletePermCheck = PermissionStatementHelper.FormatHasPermissionCheck(config.AdminPermission, config.CreatePermission);

            stringBuilder.Append($$"""
                                               if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                               {
                                                   return Error.Unauthorized(description: "Unauthenticated user cannot revoke an invitation.");
                                               }

                                               var accountId = userContext.AccountId.Value;

                                               if (thing.{{config.OwnerProperty}} == accountId || {{deletePermCheck}})
                                               {
                                                   return Result.Success;
                                               }

                                               var scopeResult = await this.{{config.ScopeServiceField}}.ReadAsync(userContext, transaction, CancellationToken.None, [thing.{{config.ScopeProperty}}]);

                                               if (!scopeResult.IsError && scopeResult.Value.Count > 0)
                                               {
                                                   var {{config.ScopeVar}} = scopeResult.Value[0];

                                                   if ({{string.Join(" || ", scopeRoleChecks)}})
                                                   {
                                                       return Result.Success;
                                                   }
                                               }

                                               return Error.Forbidden(description: "Access denied: only the invitation creator or {{config.ScopeRoleDescription}}s can revoke invitations.");
                                   """);
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

            string bypassCondition;

            if (behavior.Configuration.TryGetValue(ConfigurationKeys.BypassPermissions, out var bp) && !string.IsNullOrWhiteSpace(bp))
            {
                var perms = bp.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                bypassCondition = string.Join(" OR ", perms.Select(p => $"@can{p} = true"));
            }
            else
            {
                var readPermParts = config.ReadPermission.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                bypassCondition = string.Join(" OR ", readPermParts.Select(p => $"@can{p} = true"));
            }

            var roleChecks = config.ScopeRoles.Select(role => ManyToManyPredicateBuilder.BuildCallerAccountExistsClause(config.ScopeEntity, role, entityName, config.ScopeProperty));

            var roleChecksSql = "\r\n                                OR " + string.Join("\r\n                                OR ", roleChecks);

            return $"""
                                        {bypassCondition}
                                        OR (@callerAccountId IS NOT NULL AND (
                                            {entityName.ToSnakeCase()}.{config.OwnerColumn} = @callerAccountId
                                            OR {entityName.ToSnakeCase()}.{config.InviteeColumn} = @callerAccountId{roleChecksSql}
                                        ))
                    """;
        }

        /// <summary>
        /// Factory method to create a new <see cref="InvitationWorkflowConfiguration" /> instance.
        /// </summary>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The entity behavior definition.</param>
        /// <returns>A new <see cref="InvitationWorkflowConfiguration" /> instance.</returns>
        protected override InvitationWorkflowConfiguration CreateConfiguration(EntityPermissionDefinition definition, EntityBehaviorDefinition behavior)
        {
            return new InvitationWorkflowConfiguration(definition, behavior);
        }
    }
}
