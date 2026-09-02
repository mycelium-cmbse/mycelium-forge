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
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Mycelium.Forge.Generator.DataLoaders.PermissionModels;

    using uml4net.StructuredClassifiers;

    /// <summary>
    /// Generates permission verification hooks, state machine transitions, and dependency injection for invitation entities
    /// (<c>OrganizationInvitation</c> and <c>PackageInvitation</c>).
    /// </summary>
    public class InvitationWorkflowBehaviorTypeHelper : IBehaviorTypeHelper
    {
        /// <summary>
        /// Set of invitation entity class names supported by this behavior helper.
        /// </summary>
        private static readonly HashSet<string> SupportedInvitations = ["OrganizationInvitation", "PackageInvitation"];

        /// <summary>
        /// Gets the behavior type name handled by this helper.
        /// </summary>
        public string BehaviorType => "InvitationWorkflow";

        /// <summary>
        /// Validates that the specified class is a supported invitation entity.
        /// </summary>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        private static void ValidateSupportedClass(IClass @class)
        {
            if (!SupportedInvitations.Contains(@class.Name))
            {
                throw new NotSupportedException($"InvitationWorkflow behavior does not support entity '{@class.Name}'. Supported entities are: {string.Join(", ", SupportedInvitations)}.");
            }
        }

        /// <summary>
        /// Determines whether the specified operation requires an asynchronous implementation hook.
        /// </summary>
        /// <param name="operation">The operation name ("Create", "Read", "Update", "Delete").</param>
        /// <returns><c>true</c> if the operation is asynchronous; otherwise <c>false</c>.</returns>
        public bool IsAsyncMethod(string operation)
        {
            return operation is "Create" or "Read" or "Delete";
        }

        /// <summary>
        /// Writes fields, constructors, and dependency injection parameters for the entity class.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to write code into.</param>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="behavior">The behavior definition.</param>
        public void WriteFieldsAndConstructors(StringBuilder stringBuilder, IClass @class, EntityBehaviorDefinition behavior)
        {
            ValidateSupportedClass(@class);

            if (@class.Name == "PackageInvitation")
            {
                stringBuilder.AppendLine("        /// <summary>");
                stringBuilder.AppendLine("        /// The (injected) <see cref=\"IPackageService\" /> domain service.");
                stringBuilder.AppendLine("        /// </summary>");
                stringBuilder.AppendLine("        private readonly IPackageService packageService;");
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
                stringBuilder.AppendLine("        /// <param name=\"packageService\">The (injected) <see cref=\"IPackageService\" /> domain service.</param>");
                stringBuilder.AppendLine($"        public {@class.Name}PermissionService(IPackageService packageService)");
                stringBuilder.AppendLine("        {");
                stringBuilder.AppendLine("            this.packageService = packageService;");
                stringBuilder.AppendLine("        }");
            }
            else
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
            ValidateSupportedClass(@class);

            if (@class.Name == "OrganizationInvitation")
            {
                stringBuilder.AppendLine("            if (userContext is not { IsAuthenticated: true } || !userContext.AccountId.HasValue)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                return Result.Fail(\"Unauthenticated user cannot create an invitation.\");");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            if (toCreate == null)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                return Result.Fail(\"Invitation cannot be null.\");");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            var guard = PermissionGuard.GuardPermission(userContext, PermissionKind.InviteOrganizationMembers);");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            if (guard.IsFailed)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                return guard;");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            var orgResult = await this.organizationService.ReadAsync(userContext, CancellationToken.None, [toCreate.Owner]);");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            if (orgResult.IsSuccess && orgResult.Value.Count > 0)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                var organization = orgResult.Value[0];");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("                if (!organization.Administrator.Contains(userContext.AccountId.Value))");
                stringBuilder.AppendLine("                {");
                stringBuilder.AppendLine("                    return Result.Fail(\"Access denied: only organization administrators can create invitations.\");");
                stringBuilder.AppendLine("                }");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.Append("            return Result.Ok();");
            }
            else
            {
                stringBuilder.AppendLine("            if (userContext is not { IsAuthenticated: true } || !userContext.AccountId.HasValue)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                return Result.Fail(\"Unauthenticated user cannot create an invitation.\");");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            if (toCreate == null)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                return Result.Fail(\"Invitation cannot be null.\");");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            var guard = PermissionGuard.GuardPermission(userContext, PermissionKind.ManagePackageTeam);");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            if (guard.IsFailed)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                return guard;");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            var packageResult = await this.packageService.ReadAsync(userContext, CancellationToken.None, [toCreate.Owner]);");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            if (packageResult.IsSuccess && packageResult.Value.Count > 0)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                var package = packageResult.Value[0];");
                stringBuilder.AppendLine("                var accountId = userContext.AccountId.Value;");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("                if (!package.PackageOwner.Contains(accountId) && !package.PackageMaintainer.Contains(accountId))");
                stringBuilder.AppendLine("                {");
                stringBuilder.AppendLine("                    return Result.Fail(\"Access denied: only package owners or maintainers can invite members.\");");
                stringBuilder.AppendLine("                }");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.Append("            return Result.Ok();");
            }

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
            ValidateSupportedClass(@class);

            var inviteeProp = behavior.Configuration.GetValueOrDefault("InviteeProperty", "Target");

            if (@class.Name == "OrganizationInvitation")
            {
                stringBuilder.AppendLine("            if (thing == null)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                return Result.Fail(\"Invitation cannot be null.\");");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            if (userContext is not { IsAuthenticated: true } || !userContext.AccountId.HasValue)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                return Result.Fail(\"Unauthenticated user cannot view an invitation.\");");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            var accountId = userContext.AccountId.Value;");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"            if (thing.Owner == accountId || thing.{inviteeProp} == accountId)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                return Result.Ok();");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            var orgResult = await this.organizationService.ReadAsync(userContext, CancellationToken.None, [thing.Owner]);");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            if (orgResult.IsSuccess && orgResult.Value.Count > 0)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                var organization = orgResult.Value[0];");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("                if (organization.Administrator.Contains(accountId))");
                stringBuilder.AppendLine("                {");
                stringBuilder.AppendLine("                    return Result.Ok();");
                stringBuilder.AppendLine("                }");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.Append("            return PermissionGuard.GuardPermission(userContext, PermissionKind.ViewOrganizationMemberList);");
            }
            else
            {
                stringBuilder.AppendLine("            if (thing == null)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                return Result.Fail(\"Invitation cannot be null.\");");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            if (userContext is not { IsAuthenticated: true } || !userContext.AccountId.HasValue)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                return Result.Fail(\"Unauthenticated user cannot view an invitation.\");");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            var accountId = userContext.AccountId.Value;");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"            if (thing.Owner == accountId || thing.{inviteeProp} == accountId)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                return Result.Ok();");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            var packageResult = await this.packageService.ReadAsync(userContext, CancellationToken.None, [thing.Owner]);");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            if (packageResult.IsSuccess && packageResult.Value.Count > 0)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                var package = packageResult.Value[0];");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("                if (package.PackageOwner.Contains(accountId) || package.PackageMaintainer.Contains(accountId))");
                stringBuilder.AppendLine("                {");
                stringBuilder.AppendLine("                    return Result.Ok();");
                stringBuilder.AppendLine("                }");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.Append("            return Result.Fail(\"Access denied: user cannot view this invitation.\");");
            }

            return true;
        }

        /// <summary>
        /// Writes the update permission verification implementation body with state machine transitions.
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
            ValidateSupportedClass(@class);

            var inviteeProp = behavior.Configuration.GetValueOrDefault("InviteeProperty", "Target");
            var acceptPerm = behavior.Configuration.GetValueOrDefault("AcceptPermission", "AcceptOrganizationInvitation");
            var revokePerm = behavior.Configuration.GetValueOrDefault("RevokePermission", "RevokeOrganizationInvitation");

            stringBuilder.AppendLine("            if (userContext is not { IsAuthenticated: true } || !userContext.AccountId.HasValue)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine(isAsync ? "                return Result.Fail(\"Unauthenticated user cannot respond to an invitation.\");" : "                return Task.FromResult(Result.Fail(\"Unauthenticated user cannot respond to an invitation.\"));");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            if (existingThing == null || updatedThing == null)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine(isAsync ? "                return Result.Fail(\"Invitation cannot be null.\");" : "                return Task.FromResult(Result.Fail(\"Invitation cannot be null.\"));");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            if (existingThing.Status != updatedThing.Status)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine("                if (existingThing.Status != InvitationStatusKind.PENDING)");
            stringBuilder.AppendLine("                {");
            stringBuilder.AppendLine(isAsync ? "                    return Result.Fail($\"Cannot change status of invitation that is already {existingThing.Status}.\");" : "                    return Task.FromResult(Result.Fail($\"Cannot change status of invitation that is already {existingThing.Status}.\"));");
            stringBuilder.AppendLine("                }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("                if (updatedThing.Status == InvitationStatusKind.ACCEPTED)");
            stringBuilder.AppendLine("                {");
            stringBuilder.AppendLine($"                    if (existingThing.{inviteeProp} != userContext.AccountId.Value)");
            stringBuilder.AppendLine("                    {");
            stringBuilder.AppendLine(isAsync ? "                        return Result.Fail(\"Access denied: only the invited target account can accept the invitation.\");" : "                        return Task.FromResult(Result.Fail(\"Access denied: only the invited target account can accept the invitation.\"));");
            stringBuilder.AppendLine("                    }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(isAsync ? $"                    return PermissionGuard.GuardPermission(userContext, PermissionKind.{acceptPerm});" : $"                    return Task.FromResult(PermissionGuard.GuardPermission(userContext, PermissionKind.{acceptPerm}));");
            stringBuilder.AppendLine("                }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("                if (updatedThing.Status == InvitationStatusKind.REVOKED)");
            stringBuilder.AppendLine("                {");
            stringBuilder.AppendLine("                    if (existingThing.Owner != userContext.AccountId.Value)");
            stringBuilder.AppendLine("                    {");
            stringBuilder.AppendLine(isAsync ? "                        return Result.Fail(\"Access denied: only the invitation creator can revoke the invitation.\");" : "                        return Task.FromResult(Result.Fail(\"Access denied: only the invitation creator can revoke the invitation.\"));");
            stringBuilder.AppendLine("                    }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(isAsync ? $"                    return PermissionGuard.GuardPermission(userContext, PermissionKind.{revokePerm});" : $"                    return Task.FromResult(PermissionGuard.GuardPermission(userContext, PermissionKind.{revokePerm}));");
            stringBuilder.AppendLine("                }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(isAsync ? "                return Result.Fail($\"Unsupported invitation status transition to {updatedThing.Status}.\");" : "                return Task.FromResult(Result.Fail($\"Unsupported invitation status transition to {updatedThing.Status}.\"));");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.Append(isAsync ? "            return Result.Ok();" : "            return Task.FromResult(Result.Ok());");
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
            ValidateSupportedClass(@class);

            if (@class.Name == "OrganizationInvitation")
            {
                stringBuilder.AppendLine("            if (userContext is not { IsAuthenticated: true } || !userContext.AccountId.HasValue)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                return Result.Fail(\"Unauthenticated user cannot revoke an invitation.\");");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            if (thing == null)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                return Result.Fail(\"Invitation cannot be null.\");");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            var guard = PermissionGuard.GuardPermission(userContext, PermissionKind.InviteOrganizationMembers);");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            if (guard.IsFailed)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                return guard;");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            var orgResult = await this.organizationService.ReadAsync(userContext, CancellationToken.None, [thing.Owner]);");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            if (orgResult.IsSuccess && orgResult.Value.Count > 0)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                var organization = orgResult.Value[0];");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("                if (organization.Administrator.Contains(userContext.AccountId.Value))");
                stringBuilder.AppendLine("                {");
                stringBuilder.AppendLine("                    return Result.Ok();");
                stringBuilder.AppendLine("                }");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.Append("            return Result.Fail(\"Access denied: only organization administrators can revoke invitations.\");");
            }
            else
            {
                stringBuilder.AppendLine("            if (userContext is not { IsAuthenticated: true } || !userContext.AccountId.HasValue)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                return Result.Fail(\"Unauthenticated user cannot revoke an invitation.\");");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            if (thing == null)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                return Result.Fail(\"Invitation cannot be null.\");");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            var guard = PermissionGuard.GuardPermission(userContext, PermissionKind.ManagePackageTeam);");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            if (guard.IsFailed)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                return guard;");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            var packageResult = await this.packageService.ReadAsync(userContext, CancellationToken.None, [thing.Owner]);");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            if (packageResult.IsSuccess && packageResult.Value.Count > 0)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                var package = packageResult.Value[0];");
                stringBuilder.AppendLine("                var accountId = userContext.AccountId.Value;");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("                if (package.PackageOwner.Contains(accountId) || package.PackageMaintainer.Contains(accountId))");
                stringBuilder.AppendLine("                {");
                stringBuilder.AppendLine("                    return Result.Ok();");
                stringBuilder.AppendLine("                }");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
                stringBuilder.Append("            return Result.Fail(\"Access denied: only package owners or maintainers can revoke invitations.\");");
            }

            return true;
        }
    }
}
