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
    using Mycelium.Forge.Generator.Models;

    using uml4net.StructuredClassifiers;

    /// <summary>
    /// Generates permission verification hooks and dependency injection for invitation entities governed by an
    /// invitation lifecycle workflow (e.g. OrganizationInvitation, PackageInvitation).
    /// Fully configurable via behavior key-value pairs:
    /// <c>ScopeEntity</c>, <c>ScopeProperty</c>, <c>ScopeRoles</c>, <c>InviteeProperty</c>,
    /// <c>CreatePermission</c>, <c>ReadPermission</c>, <c>AcceptPermission</c>, <c>RevokePermission</c>,
    /// <c>AdminPermission</c>.
    /// </summary>
    public class InvitationWorkflowBehaviorTypeHelper : BehaviorTypeHelperBase<InvitationWorkflowConfiguration>
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
            return operation != Operations.Update;
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
        public override void WriteIsAllowedToCreate(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior)
        {
            var config = this.GetConfiguration(definition, behavior);
            var scopeRoleChecks = config.ScopeRoles.Select(role => $"{config.ScopeVar}.{role}.Contains(userContext.AccountId.Value)");

            stringBuilder.Append($$"""
                                               if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                               {
                                                   return Result.Fail("Unauthenticated user cannot create an invitation.");
                                               }

                                               var guard = PermissionGuard.GuardPermission(userContext, PermissionKind.{{config.CreatePermission}});

                                               if (guard.IsSuccess)
                                               {
                                                   return Result.Ok();
                                               }

                                               var scopeResult = await this.{{config.ScopeServiceField}}.ReadAsync(userContext, CancellationToken.None, [toCreate.{{config.ScopeProperty}}]);

                                               if (scopeResult.IsSuccess && scopeResult.Value.Count > 0)
                                               {
                                                   var {{config.ScopeVar}} = scopeResult.Value[0];

                                                   if ({{string.Join(" || ", scopeRoleChecks)}})
                                                   {
                                                       return Result.Ok();
                                                   }
                                               }

                                               return Result.Fail("Access denied: only {{config.ScopeRoleDescription}}s can invite members.");
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
            var scopeRoleChecks = config.ScopeRoles.Select(role => $"{config.ScopeVar}.{role}.Contains(accountId)");

            stringBuilder.Append($$"""
                                               if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                               {
                                                   return Result.Fail("Unauthenticated user cannot view an invitation.");
                                               }

                                               var accountId = userContext.AccountId.Value;

                                               if (thing.Owner == accountId || thing.{{config.InviteeProperty}} == accountId)
                                               {
                                                   return Result.Ok();
                                               }

                                               var scopeResult = await this.{{config.ScopeServiceField}}.ReadAsync(userContext, CancellationToken.None, [thing.{{config.ScopeProperty}}]);

                                               if (scopeResult.IsSuccess && scopeResult.Value.Count > 0)
                                               {
                                                   var {{config.ScopeVar}} = scopeResult.Value[0];

                                                   if ({{string.Join(" || ", scopeRoleChecks)}})
                                                   {
                                                       return Result.Ok();
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

            var failUnauth = PermissionStatementHelper.GetFailReturn("\"Unauthenticated user cannot respond to an invitation.\"", isAsync);
            var failAlready = PermissionStatementHelper.GetFailReturn("$\"Cannot change status of invitation that is already {existingThing.Status}.\"", isAsync);
            var failNotInvitee = PermissionStatementHelper.GetFailReturn("\"Access denied: only the invited target account can accept the invitation.\"", isAsync);
            var acceptGuard = PermissionStatementHelper.GetReturnStatement($"PermissionGuard.GuardPermission(userContext, PermissionKind.{config.AcceptPermission})", isAsync);
            var failNotOwner = PermissionStatementHelper.GetFailReturn("\"Access denied: only the invitation creator can revoke the invitation.\"", isAsync);
            var revokeGuard = PermissionStatementHelper.GetReturnStatement($"PermissionGuard.GuardPermission(userContext, PermissionKind.{config.RevokePermission})", isAsync);
            var failUnsupported = PermissionStatementHelper.GetFailReturn("$\"Unsupported invitation status transition to {updatedThing.Status}.\"", isAsync);
            var okParty = PermissionStatementHelper.GetOkReturn(isAsync);
            var okAdmin = PermissionStatementHelper.GetOkReturn(isAsync);
            var failNotParty = PermissionStatementHelper.GetFailReturn("\"Access denied: you are not a party to this invitation.\"", isAsync);

            stringBuilder.Append($$"""
                                               if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                               {
                                                   {{failUnauth}}
                                               }

                                               if (existingThing.Status != updatedThing.Status)
                                               {
                                                   if (existingThing.Status != InvitationStatusKind.PENDING)
                                                   {
                                                       {{failAlready}}
                                                   }

                                                   if (updatedThing.Status == InvitationStatusKind.ACCEPTED)
                                                   {
                                                       if (existingThing.{{config.InviteeProperty}} != userContext.AccountId.Value && !PermissionGuard.HasPermission(userContext, PermissionKind.{{config.AdminPermission}}))
                                                       {
                                                           {{failNotInvitee}}
                                                       }

                                                       {{acceptGuard}}
                                                   }

                                                   if (updatedThing.Status == InvitationStatusKind.REVOKED)
                                                   {
                                                       if (existingThing.Owner != userContext.AccountId.Value && !PermissionGuard.HasPermission(userContext, PermissionKind.{{config.AdminPermission}}))
                                                       {
                                                           {{failNotOwner}}
                                                       }

                                                       {{revokeGuard}}
                                                   }

                                                   {{failUnsupported}}
                                               }

                                               if (existingThing.Owner == userContext.AccountId.Value || existingThing.{{config.InviteeProperty}} == userContext.AccountId.Value)
                                               {
                                                   {{okParty}}
                                               }

                                               if (PermissionGuard.HasPermission(userContext, PermissionKind.{{config.AdminPermission}}))
                                               {
                                                   {{okAdmin}}
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
            var scopeRoleChecks = config.ScopeRoles.Select(role => $"{config.ScopeVar}.{role}.Contains(accountId)");

            stringBuilder.Append($$"""
                                               if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                               {
                                                   return Result.Fail("Unauthenticated user cannot revoke an invitation.");
                                               }

                                               var guard = PermissionGuard.GuardPermission(userContext, PermissionKind.{{config.CreatePermission}});

                                               if (guard.IsFailed)
                                               {
                                                   return guard;
                                               }

                                               var scopeResult = await this.{{config.ScopeServiceField}}.ReadAsync(userContext, CancellationToken.None, [thing.{{config.ScopeProperty}}]);

                                               if (scopeResult.IsSuccess && scopeResult.Value.Count > 0)
                                               {
                                                   var {{config.ScopeVar}} = scopeResult.Value[0];
                                                   var accountId = userContext.AccountId.Value;

                                                   if ({{string.Join(" || ", scopeRoleChecks)}})
                                                   {
                                                       return Result.Ok();
                                                   }
                                               }

                                               return Result.Fail("Access denied: only {{config.ScopeRoleDescription}}s can revoke invitations.");
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

            var roleChecks = config.ScopeRoles.Select(role =>
            {
                var rolePropCamel = char.ToLowerInvariant(role[0]) + role[1..];
                return $"EXISTS (SELECT 1 FROM \"Forge\".\"{config.ScopeEntity}_{rolePropCamel}__Account\" WHERE \"source{config.ScopeEntity}\" = \"{entityName}\".\"{config.ScopeProperty.ToLowerInvariant()}\" AND \"targetAccount\" = @callerAccountId)";
            });

            var roleChecksSql = "\r\n                                OR " + string.Join("\r\n                                OR ", roleChecks);

            return $"""
                                        {bypassCondition}
                                        OR (@callerAccountId IS NOT NULL AND (
                                            "{entityName}"."{config.OwnerColumn}" = @callerAccountId
                                            OR "{entityName}"."{config.InviteeColumn}" = @callerAccountId{roleChecksSql}
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
