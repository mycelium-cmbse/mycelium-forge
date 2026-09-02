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
    using System.Collections.Generic;
    using System.Text;

    using Mycelium.Forge.Generator.DataLoaders.PermissionModels;

    using uml4net.StructuredClassifiers;

    /// <summary>
    /// Generates permission verification hooks and dependency injection for entities that can be owned by either
    /// an account (personal scope) or an organization, evaluating scope boundaries and organization membership.
    /// </summary>
    public class OrganizationScopeBehaviorTypeHelper : IBehaviorTypeHelper
    {
        /// <summary>
        /// Gets the behavior type name handled by this helper.
        /// </summary>
        public string BehaviorType => "OrganizationScope";

        /// <summary>
        /// Determines whether the specified operation requires an asynchronous implementation hook.
        /// </summary>
        /// <param name="operation">The operation name ("Create", "Read", "Update", "Delete").</param>
        /// <returns><c>true</c> if the operation is asynchronous; otherwise <c>false</c>.</returns>
        public bool IsAsyncMethod(string operation)
        {
            return operation is "Create" or "Read";
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
            stringBuilder.AppendLine("            if (userContext is not { IsAuthenticated: true } || !userContext.AccountId.HasValue)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine("                return Result.Fail(\"Unauthenticated user cannot create a package.\");");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            if (toCreate == null)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine("                return Result.Fail(\"Package cannot be null.\");");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            if (toCreate.Owner == userContext.AccountId.Value)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine("                return PermissionGuard.GuardPermission(userContext, PermissionKind.PublishPackageToPersonalScope);");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            if (PermissionGuard.HasPermission(userContext, PermissionKind.ManageOrganizations))");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine("                return Result.Ok();");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            var orgGuard = PermissionGuard.GuardPermission(userContext, PermissionKind.PublishPackageToOrganizationScope);");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            if (orgGuard.IsFailed)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine("                return orgGuard;");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            var orgResult = await this.organizationService.ReadAsync(userContext, CancellationToken.None, [toCreate.Owner]);");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            if (orgResult.IsSuccess && orgResult.Value.Count > 0)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine("                var organization = orgResult.Value[0];");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("                if (!organization.Member.Contains(userContext.AccountId.Value) && !organization.Administrator.Contains(userContext.AccountId.Value))");
            stringBuilder.AppendLine("                {");
            stringBuilder.AppendLine("                    return Result.Fail(\"Access denied: user is not a member of the target organization.\");");
            stringBuilder.AppendLine("                }");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.Append("            return Result.Ok();");
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
            stringBuilder.AppendLine("            if (thing == null)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine("                return Result.Fail(\"Package cannot be null.\");");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            if (thing.Visibility == VisibilityKind.PUBLIC)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine("                return Result.Ok();");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            if (userContext is not { IsAuthenticated: true } || !userContext.AccountId.HasValue)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine("                return Result.Fail(\"Unauthenticated user cannot access non-public package.\");");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            var accountId = userContext.AccountId.Value;");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            if (thing.Owner == accountId || thing.PackageOwner.Contains(accountId) || thing.PackageMaintainer.Contains(accountId))");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine("                return Result.Ok();");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            if (thing.Visibility == VisibilityKind.INTERNAL)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine("                if (PermissionGuard.HasPermission(userContext, PermissionKind.ViewAllOrganizations) || PermissionGuard.HasPermission(userContext, PermissionKind.ManageOrganizations))");
            stringBuilder.AppendLine("                {");
            stringBuilder.AppendLine("                    return Result.Ok();");
            stringBuilder.AppendLine("                }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("                var internalPermissionResult = PermissionGuard.GuardPermission(userContext, PermissionKind.ReadInternalPackage);");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("                if (internalPermissionResult.IsFailed)");
            stringBuilder.AppendLine("                {");
            stringBuilder.AppendLine("                    return internalPermissionResult;");
            stringBuilder.AppendLine("                }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("                var orgResult = await this.organizationService.ReadAsync(userContext, CancellationToken.None, [thing.Owner]);");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("                if (orgResult.IsSuccess && orgResult.Value.Count > 0)");
            stringBuilder.AppendLine("                {");
            stringBuilder.AppendLine("                    var organization = orgResult.Value[0];");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("                    if (organization.Member.Contains(accountId) || organization.Administrator.Contains(accountId))");
            stringBuilder.AppendLine("                    {");
            stringBuilder.AppendLine("                        return Result.Ok();");
            stringBuilder.AppendLine("                    }");
            stringBuilder.AppendLine("                }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("                return Result.Fail(\"Access denied: user is not a member of the owning organization.\");");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            if (thing.Visibility == VisibilityKind.PRIVATE)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine("                return PermissionGuard.GuardPermission(userContext, PermissionKind.ReadPrivatePackage);");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.Append("            return Result.Fail(\"Access denied: cannot view this package.\");");
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
            return false;
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
            return false;
        }
    }
}
