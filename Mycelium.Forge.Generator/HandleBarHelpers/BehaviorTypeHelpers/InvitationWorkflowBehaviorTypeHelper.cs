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
        /// The default local variable name used for scope lookup results in generated permission checks.
        /// </summary>
        private const string ScopeResultVariableName = "scopeResult";

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
                base.WriteFieldsAndConstructors(stringBuilder, @class, definition, behavior);
                return;
            }

            var scopeService = $"I{config.ScopeEntity}Service";

            stringBuilder.AppendLine($$"""
                                               /// <summary>
                                               /// The (injected) <see cref="{{scopeService}}" /> domain service.
                                               /// </summary>
                                               private readonly {{scopeService}} {{config.ScopeServiceField}};

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
                                               /// <param name="{{config.ScopeServiceField}}">The (injected) <see cref="{{scopeService}}" /> domain service.</param>
                                               /// <param name="{{config.OwnerScope.ServiceField}}">The (injected) <see cref="{{config.OwnerScope.ServiceType}}" /> domain service.</param>
                                               public {{@class.Name}}PermissionService({{scopeService}} {{config.ScopeServiceField}}, {{config.OwnerScope.ServiceType}} {{config.OwnerScope.ServiceField}})
                                               {
                                                   this.{{config.ScopeServiceField}} = {{config.ScopeServiceField}};
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
            var createPermCheck = PermissionStatementHelper.FormatHasPermissionCheck(config.AdminPermission, config.CreatePermission);
            var scopeCheckBlock = BuildScopeAccessBlock(config, $"toCreate.{config.ScopeProperty}", ScopeResultVariableName);

            stringBuilder.Append($$"""
                                               if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                               {
                                                   return Error.Unauthorized(description: "Unauthenticated user cannot create an invitation.");
                                               }

                                               var accountId = userContext.AccountId.Value;

                                               if ({{createPermCheck}})
                                               {
                                                   return Result.Success;
                                               }

                                   {{scopeCheckBlock}}

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
            var scopeCheckBlock = BuildScopeAccessBlock(config, $"thing.{config.ScopeProperty}", ScopeResultVariableName);

            stringBuilder.Append($$"""
                                               if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                               {
                                                   return Error.Unauthorized(description: "Unauthenticated user cannot view an invitation.");
                                               }

                                               var accountId = userContext.AccountId.Value;

                                               if (thing.{{config.InviteeProperty}} == accountId || PermissionGuard.HasPermission(userContext, PermissionKind.{{config.ReadPermission}}))
                                               {
                                                   return Result.Success;
                                               }

                                   {{scopeCheckBlock}}

                                               return Error.Forbidden(description: "Access denied: cannot view this invitation.");
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
            var revokePermCheck = PermissionStatementHelper.FormatHasPermissionCheck(config.AdminPermission, config.CreatePermission);
            var revokeScopeBlock = BuildScopeAccessBlock(config, $"existingThing.{config.ScopeProperty}", ScopeResultVariableName, "                    ");
            var updateScopeBlock = BuildScopeAccessBlock(config, $"existingThing.{config.ScopeProperty}", "updateScopeResult");

            var failUnauth = PermissionStatementHelper.GetUnauthorizedReturn("\"Unauthenticated user cannot respond to an invitation.\"", isAsync);
            var failAlready = PermissionStatementHelper.GetFailReturn("$\"Cannot update invitation that is already {existingThing.Status}.\"", isAsync);
            var failImmutableScopeOrParty = PermissionStatementHelper.GetFailReturn("\"Cannot modify invitation scope, target, or owner.\"", isAsync);
            var failExpired = PermissionStatementHelper.GetFailReturn("\"Cannot respond to an invitation that has expired.\"", isAsync);
            var failModified = PermissionStatementHelper.GetFailReturn("\"Cannot modify invitation details when accepting or rejecting an invitation.\"", isAsync);
            var failKindModified = PermissionStatementHelper.GetFailReturn("\"Cannot change invitation role kind when updating an invitation.\"", isAsync);
            var failNotInvitee = PermissionStatementHelper.GetFailReturn("\"Access denied: only the invited target account can accept or reject the invitation.\"", isAsync);
            var failUnsupported = PermissionStatementHelper.GetFailReturn("$\"Unsupported invitation status transition to {updatedThing.Status}.\"", isAsync);
            var okParty = PermissionStatementHelper.GetOkReturn(isAsync);
            var okAdmin = PermissionStatementHelper.GetOkReturn(isAsync);
            var revokeRoleDescription = FormatRevokeActorDescription(config);
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

                                               if (existingThing.{{config.ScopeProperty}} != updatedThing.{{config.ScopeProperty}} ||
                                                   existingThing.{{config.InviteeProperty}} != updatedThing.{{config.InviteeProperty}} ||
                                                   existingThing.{{config.OwnerProperty}} != updatedThing.{{config.OwnerProperty}})
                                               {
                                                   {{failImmutableScopeOrParty}}
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
                                                           existingThing.{{config.EntityName}}Kind != updatedThing.{{config.EntityName}}Kind)
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
                                                       if ({{revokePermCheck}})
                                                       {
                                                           {{okParty}}
                                                       }

                                   {{revokeScopeBlock}}

                                                       return Error.Forbidden(description: "Access denied: only {{revokeRoleDescription}} can revoke the invitation.");
                                                   }

                                                   {{failUnsupported}}
                                               }

                                               if (existingThing.{{config.EntityName}}Kind != updatedThing.{{config.EntityName}}Kind)
                                               {
                                                   {{failKindModified}}
                                               }

                                               if ({{revokePermCheck}})
                                               {
                                                   {{okAdmin}}
                                               }

                                   {{updateScopeBlock}}

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
            var deletePermCheck = PermissionStatementHelper.FormatHasPermissionCheck(config.AdminPermission, config.CreatePermission);
            var scopeCheckBlock = BuildScopeAccessBlock(config, $"thing.{config.ScopeProperty}", ScopeResultVariableName);
            var revokeRoleDescription = FormatRevokeActorDescription(config);

            stringBuilder.Append($$"""
                                               if (!userContext.IsAuthenticated || !userContext.AccountId.HasValue)
                                               {
                                                   return Error.Unauthorized(description: "Unauthenticated user cannot revoke an invitation.");
                                               }

                                               var accountId = userContext.AccountId.Value;

                                               if ({{deletePermCheck}})
                                               {
                                                   return Result.Success;
                                               }

                                   {{scopeCheckBlock}}

                                               return Error.Forbidden(description: "Access denied: only {{revokeRoleDescription}} can revoke invitations.");
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

            var roleChecks = config.ScopeRoles.Select(role => ManyToManyPredicateBuilder.BuildCallerAccountExistsClause(config.ScopeEntity, role, entityName, config.ScopeProperty)).ToList();

            if (config.OwnerScope.IsConfigured)
            {
                var scopeTable = config.ScopeEntity.ToSnakeCase();
                var entityTable = entityName.ToSnakeCase();
                var scopeColumn = config.ScopeProperty.ToSnakeCase();
                var ownerScopeTable = config.OwnerScope.Entity.ToSnakeCase();
                var ownerScopeColumn = config.OwnerScope.Property.ToSnakeCase();

                foreach (var role in config.OwnerScope.Roles)
                {
                    var junctionTable = $"{ownerScopeTable}_{role.ToSnakeCase()}__account";
                    roleChecks.Add($"EXISTS (SELECT 1 FROM forge.{scopeTable} INNER JOIN forge.{junctionTable} ON source_{ownerScopeTable} = {scopeTable}.{ownerScopeColumn} WHERE {scopeTable}.id = {entityTable}.{scopeColumn} AND target_account = @callerAccountId)");
                }
            }

            var roleChecksSql = "\r\n                                OR " + string.Join("\r\n                                OR ", roleChecks);

            return $"""
                                        {bypassCondition}
                                        OR (@callerAccountId IS NOT NULL AND (
                                            {entityName.ToSnakeCase()}.{config.InviteeColumn} = @callerAccountId{roleChecksSql}
                                        ))
                    """;
        }

        /// <summary>
        /// Builds the asynchronous scope lookup and role/owner-scope verification block.
        /// </summary>
        /// <param name="config">The invitation workflow configuration.</param>
        /// <param name="scopeIdExpression">The C# expression yielding the scope identifier.</param>
        /// <param name="resultVarName">The local variable name for the scope read result.</param>
        /// <param name="indent">The indentation prefix for generated lines.</param>
        /// <returns>The formatted C# scope verification block.</returns>
        private static string BuildScopeAccessBlock(InvitationWorkflowConfiguration config, string scopeIdExpression, string resultVarName, string indent = "            ")
        {
            var scopeRoleChecks = string.Join(" || ", config.ScopeRoles.Select(role => $"{config.ScopeVar}.{role}.Contains(accountId)"));

            var ownerScopeLines = new StringBuilder();

            if (config.OwnerScope.IsConfigured)
            {
                var roleChecks = string.Join(" || ", config.OwnerScope.Roles.Select(role => $"{config.OwnerScope.VarName}.{role}.Contains(accountId)"));
                ownerScopeLines.AppendLine();
                ownerScopeLines.AppendLine();
                ownerScopeLines.AppendLine($"{indent}        var {config.OwnerScope.VarName}Result = await this.{config.OwnerScope.ServiceField}.ReadAsync(userContext, transaction, CancellationToken.None, [{config.ScopeVar}.{config.OwnerScope.Property}]);");
                ownerScopeLines.AppendLine();
                ownerScopeLines.AppendLine($"{indent}        if (!{config.OwnerScope.VarName}Result.IsError && {config.OwnerScope.VarName}Result.Value.Count > 0)");
                ownerScopeLines.AppendLine($"{indent}        {{");
                ownerScopeLines.AppendLine($"{indent}            var {config.OwnerScope.VarName} = {config.OwnerScope.VarName}Result.Value[0];");
                ownerScopeLines.AppendLine();
                ownerScopeLines.AppendLine($"{indent}            if ({roleChecks})");
                ownerScopeLines.AppendLine($"{indent}            {{");
                ownerScopeLines.AppendLine($"{indent}                return Result.Success;");
                ownerScopeLines.AppendLine($"{indent}            }}");
                ownerScopeLines.AppendLine($"{indent}        }}");
            }

            var block = new StringBuilder();
            block.AppendLine($"{indent}var {resultVarName} = await this.{config.ScopeServiceField}.ReadAsync(userContext, transaction, CancellationToken.None, [{scopeIdExpression}]);");
            block.AppendLine();
            block.AppendLine($"{indent}if (!{resultVarName}.IsError && {resultVarName}.Value.Count > 0)");
            block.AppendLine($"{indent}{{");
            block.AppendLine($"{indent}    var {config.ScopeVar} = {resultVarName}.Value[0];");
            block.AppendLine();
            block.AppendLine($"{indent}    if ({scopeRoleChecks})");
            block.AppendLine($"{indent}    {{");
            block.AppendLine($"{indent}        return Result.Success;");
            block.AppendLine($"{indent}    }}{ownerScopeLines.ToString().TrimEnd()}");
            block.Append($"{indent}}}");

            return block.ToString();
        }

        /// <summary>
        /// Formats the human-readable actor description used in invitation revocation error messages.
        /// </summary>
        /// <param name="config">The invitation workflow configuration.</param>
        /// <returns>The formatted actor description.</returns>
        private static string FormatRevokeActorDescription(InvitationWorkflowConfiguration config)
        {
            if (config.ScopeRoles.Length == 1 && config.ScopeRoles[0].Equals("Administrator", StringComparison.OrdinalIgnoreCase))
            {
                return "administrators";
            }

            return $"administrators or {config.ScopeRoleDescription}s";
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
