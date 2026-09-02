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

    using Mycelium.Forge.Generator.DataLoaders.PermissionModels;
    using Mycelium.Forge.Generator.Extensions;

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
        /// Gets the behavior type name handled by this helper.
        /// </summary>
        public string BehaviorType => "ParentDelegation";

        /// <summary>
        /// Determines whether this behavior helper handles the specified operation.
        /// </summary>
        /// <param name="operation">The operation name ("Create", "Read", "Update", "Delete").</param>
        /// <returns><c>true</c> if the behavior handles the operation; otherwise <c>false</c>.</returns>
        public bool HandlesOperation(string operation)
        {
            return true;
        }

        /// <summary>
        /// Determines whether the specified operation requires an asynchronous implementation hook.
        /// </summary>
        /// <param name="operation">The operation name ("Create", "Read", "Update", "Delete").</param>
        /// <returns><c>true</c> if the operation is asynchronous; otherwise <c>false</c>.</returns>
        public bool IsAsyncMethod(string operation)
        {
            return true;
        }

        /// <summary>
        /// Writes fields, constructors, and dependency injection parameters for the entity class.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to write code into.</param>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="behavior">The behavior definition.</param>
        public void WriteFieldsAndConstructors(StringBuilder stringBuilder, IClass @class, EntityBehaviorDefinition behavior)
        {
            var parentEntity = behavior.Configuration.GetValueOrDefault("ParentEntity", "Package");
            var parentServiceType = $"I{parentEntity}Service";
            var parentServiceField = $"{char.ToLowerInvariant(parentEntity[0])}{parentEntity[1..]}Service";
            var parentPermServiceType = $"I{parentEntity}PermissionService";
            var parentPermServiceField = $"{char.ToLowerInvariant(parentEntity[0])}{parentEntity[1..]}PermissionService";

            stringBuilder.AppendLine("        /// <summary>");
            stringBuilder.AppendLine($"        /// The (injected) <see cref=\"{parentServiceType}\" /> domain service.");
            stringBuilder.AppendLine("        /// </summary>");
            stringBuilder.AppendLine($"        private readonly {parentServiceType} {parentServiceField};");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("        /// <summary>");
            stringBuilder.AppendLine($"        /// The (injected) <see cref=\"{parentPermServiceType}\" /> domain service.");
            stringBuilder.AppendLine("        /// </summary>");
            stringBuilder.AppendLine($"        private readonly {parentPermServiceType} {parentPermServiceField};");
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
            stringBuilder.AppendLine($"        /// <param name=\"{parentServiceField}\">The (injected) <see cref=\"{parentServiceType}\" /> domain service.</param>");
            stringBuilder.AppendLine($"        /// <param name=\"{parentPermServiceField}\">The (injected) <see cref=\"{parentPermServiceType}\" /> domain service.</param>");
            stringBuilder.AppendLine($"        public {@class.Name}PermissionService({parentServiceType} {parentServiceField}, {parentPermServiceType} {parentPermServiceField})");
            stringBuilder.AppendLine("        {");
            stringBuilder.AppendLine($"            this.{parentServiceField} = {parentServiceField};");
            stringBuilder.AppendLine($"            this.{parentPermServiceField} = {parentPermServiceField};");
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
        public void WriteIsAllowedToCreate(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior, bool isAsync)
        {
            var parentEntity = behavior.Configuration.GetValueOrDefault("ParentEntity", "Package");
            var parentKey = behavior.Configuration.GetValueOrDefault("ParentKey", "Owner");
            var parentServiceField = $"{char.ToLowerInvariant(parentEntity[0])}{parentEntity[1..]}Service";
            var parentVar = $"{char.ToLowerInvariant(parentEntity[0])}{parentEntity[1..]}";
            var createPerm = behavior.Configuration.TryGetValue("CreatePermission", out var cp) ? cp : definition?.CreatePermission;

            var parentOwnerProps = behavior.Configuration.TryGetValue("ParentOwnerProperties", out var pop)
                ? pop.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                : ["Owner"];

            stringBuilder.AppendLine("            if (userContext is not { IsAuthenticated: true } || !userContext.AccountId.HasValue)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine($"                return Result.Fail(\"Unauthenticated user cannot publish a {@class.Name.ToLowerInvariant()}.\");");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            if (toCreate == null)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine($"                return Result.Fail(\"{@class.Name} cannot be null.\");");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();

            if (!string.IsNullOrWhiteSpace(createPerm))
            {
                stringBuilder.AppendLine($"            var guard = PermissionGuard.GuardPermission(userContext, PermissionKind.{createPerm});");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            if (guard.IsFailed)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                return guard;");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
            }

            stringBuilder.AppendLine($"            var parentResult = await this.{parentServiceField}.ReadAsync(userContext, CancellationToken.None, [toCreate.{parentKey}]);");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            if (parentResult.IsSuccess && parentResult.Value.Count > 0)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine($"                var {parentVar} = parentResult.Value[0];");
            stringBuilder.AppendLine("                var accountId = userContext.AccountId.Value;");
            stringBuilder.AppendLine();

            var ownershipChecks = new List<string> { $"{parentVar}.Owner == accountId" };

            foreach (var prop in parentOwnerProps)
            {
                if (!prop.Equals("Owner", StringComparison.OrdinalIgnoreCase))
                {
                    ownershipChecks.Add($"{parentVar}.{prop}.Contains(accountId)");
                }
            }

            stringBuilder.AppendLine($"                if ({string.Join(" || ", ownershipChecks)})");
            stringBuilder.AppendLine("                {");
            stringBuilder.AppendLine("                    return Result.Ok();");
            stringBuilder.AppendLine("                }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"                return Result.Fail(\"Access denied: user is not an owner or maintainer of the {parentEntity.ToLowerInvariant()}.\");");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.Append("            return Result.Ok();");
        }

        /// <summary>
        /// Writes the read permission verification implementation body.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to write code into.</param>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The behavior definition.</param>
        /// <param name="isAsync">Whether the enclosing method is asynchronous.</param>
        public void WriteIsAllowedToRead(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior, bool isAsync)
        {
            var parentEntity = behavior.Configuration.GetValueOrDefault("ParentEntity", "Package");
            var parentKey = behavior.Configuration.GetValueOrDefault("ParentKey", "Owner");
            var parentServiceField = $"{char.ToLowerInvariant(parentEntity[0])}{parentEntity[1..]}Service";
            var parentPermServiceField = $"{char.ToLowerInvariant(parentEntity[0])}{parentEntity[1..]}PermissionService";

            stringBuilder.AppendLine("            if (thing == null)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine($"                return Result.Fail(\"{@class.Name} cannot be null.\");");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"            var parentResult = await this.{parentServiceField}.ReadAsync(userContext, CancellationToken.None, [thing.{parentKey}]);");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            if (parentResult.IsSuccess && parentResult.Value.Count > 0)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine($"                return await this.{parentPermServiceField}.IsAllowedToRead(userContext, parentResult.Value[0]);");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.Append("            return Result.Ok();");
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
        public void WriteIsAllowedToUpdate(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior, List<PropertyPermissionDefinition> propertyDefinitions, bool isAsync)
        {
            var parentEntity = behavior.Configuration.GetValueOrDefault("ParentEntity", "Package");
            var parentKey = behavior.Configuration.GetValueOrDefault("ParentKey", "Owner");
            var parentServiceField = $"{char.ToLowerInvariant(parentEntity[0])}{parentEntity[1..]}Service";
            var parentVar = $"{char.ToLowerInvariant(parentEntity[0])}{parentEntity[1..]}";
            var stateProp = behavior.Configuration.GetValueOrDefault("StateProperty", "Listed");
            var stateActivePerm = behavior.Configuration.GetValueOrDefault("StateActivePermission", "RelistPackageVersion");
            var stateInactivePerm = behavior.Configuration.GetValueOrDefault("StateInactivePermission", "UnlistPackageVersion");

            var parentOwnerProps = behavior.Configuration.TryGetValue("ParentOwnerProperties", out var pop)
                ? pop.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                : ["Owner"];

            stringBuilder.AppendLine("            if (userContext is not { IsAuthenticated: true } || !userContext.AccountId.HasValue)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine($"                return Result.Fail(\"Unauthenticated user cannot update {@class.Name.ToLowerInvariant()}.\");");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            if (existingThing == null || updatedThing == null)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine($"                return Result.Fail(\"{@class.Name} cannot be null.\");");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();

            if (!string.IsNullOrWhiteSpace(stateProp))
            {
                var immutableProperties = @class.QueryDtoClassProperties()
                    .Select(p => p.Name.CapitalizeFirstLetter())
                    .Where(p => !p.Equals("Id", StringComparison.OrdinalIgnoreCase) && !p.Equals(stateProp, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (immutableProperties.Count > 0)
                {
                    var diffChecks = immutableProperties.Select(p => $"existingThing.{p} != updatedThing.{p}");
                    stringBuilder.AppendLine($"            if ({string.Join(" ||\r\n                ", diffChecks)})");
                    stringBuilder.AppendLine("            {");
                    stringBuilder.AppendLine($"                return Result.Fail(\"{@class.Name}s are immutable; only the {stateProp.ToLowerInvariant()} status may be modified.\");");
                    stringBuilder.AppendLine("            }");
                    stringBuilder.AppendLine();
                }

                if (!string.IsNullOrWhiteSpace(stateActivePerm) && !string.IsNullOrWhiteSpace(stateInactivePerm))
                {
                    stringBuilder.AppendLine($"            if (existingThing.{stateProp} != updatedThing.{stateProp})");
                    stringBuilder.AppendLine("            {");
                    stringBuilder.AppendLine($"                var permission = updatedThing.{stateProp} ? PermissionKind.{stateActivePerm} : PermissionKind.{stateInactivePerm};");
                    stringBuilder.AppendLine("                var guard = PermissionGuard.GuardPermission(userContext, permission);");
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine("                if (guard.IsFailed)");
                    stringBuilder.AppendLine("                {");
                    stringBuilder.AppendLine("                    return guard;");
                    stringBuilder.AppendLine("                }");
                    stringBuilder.AppendLine("            }");
                    stringBuilder.AppendLine();
                }
            }

            stringBuilder.AppendLine($"            var parentResult = await this.{parentServiceField}.ReadAsync(userContext, CancellationToken.None, [existingThing.{parentKey}]);");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            if (parentResult.IsSuccess && parentResult.Value.Count > 0)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine($"                var {parentVar} = parentResult.Value[0];");
            stringBuilder.AppendLine("                var accountId = userContext.AccountId.Value;");
            stringBuilder.AppendLine();

            var ownershipChecks = new List<string> { $"{parentVar}.Owner == accountId" };

            foreach (var prop in parentOwnerProps)
            {
                if (!prop.Equals("Owner", StringComparison.OrdinalIgnoreCase))
                {
                    ownershipChecks.Add($"{parentVar}.{prop}.Contains(accountId)");
                }
            }

            stringBuilder.AppendLine($"                if ({string.Join(" || ", ownershipChecks)})");
            stringBuilder.AppendLine("                {");
            stringBuilder.AppendLine("                    return Result.Ok();");
            stringBuilder.AppendLine("                }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"                return Result.Fail(\"Access denied: user is not an owner or maintainer of the {parentEntity.ToLowerInvariant()}.\");");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.Append("            return Result.Ok();");
        }

        /// <summary>
        /// Writes the delete permission verification implementation body.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to write code into.</param>
        /// <param name="class">The UML <see cref="IClass" /> being generated.</param>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The behavior definition.</param>
        /// <param name="isAsync">Whether the enclosing method is asynchronous.</param>
        public void WriteIsAllowedToDelete(StringBuilder stringBuilder, IClass @class, EntityPermissionDefinition definition, EntityBehaviorDefinition behavior, bool isAsync)
        {
            var parentEntity = behavior.Configuration.GetValueOrDefault("ParentEntity", "Package");
            var parentKey = behavior.Configuration.GetValueOrDefault("ParentKey", "Owner");
            var parentServiceField = $"{char.ToLowerInvariant(parentEntity[0])}{parentEntity[1..]}Service";
            var parentVar = $"{char.ToLowerInvariant(parentEntity[0])}{parentEntity[1..]}";
            var deletePerm = behavior.Configuration.TryGetValue("DeletePermission", out var dp) ? dp : definition?.DeletePermission;

            var parentOwnerProps = behavior.Configuration.TryGetValue("ParentOwnerProperties", out var pop)
                ? pop.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                : ["Owner"];

            stringBuilder.AppendLine("            if (userContext is not { IsAuthenticated: true } || !userContext.AccountId.HasValue)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine($"                return Result.Fail(\"Unauthenticated user cannot erase a {@class.Name.ToLowerInvariant()}.\");");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            if (thing == null)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine($"                return Result.Fail(\"{@class.Name} cannot be null.\");");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();

            if (!string.IsNullOrWhiteSpace(deletePerm))
            {
                stringBuilder.AppendLine($"            var eraseGuard = PermissionGuard.GuardPermission(userContext, PermissionKind.{deletePerm});");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("            if (eraseGuard.IsFailed)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                return eraseGuard;");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine();
            }

            stringBuilder.AppendLine($"            var parentResult = await this.{parentServiceField}.ReadAsync(userContext, CancellationToken.None, [thing.{parentKey}]);");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            if (parentResult.IsSuccess && parentResult.Value.Count > 0)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine($"                var {parentVar} = parentResult.Value[0];");
            stringBuilder.AppendLine("                var accountId = userContext.AccountId.Value;");
            stringBuilder.AppendLine();

            var ownershipChecks = new List<string> { $"{parentVar}.Owner == accountId" };

            foreach (var prop in parentOwnerProps)
            {
                if (!prop.Equals("Owner", StringComparison.OrdinalIgnoreCase))
                {
                    ownershipChecks.Add($"{parentVar}.{prop}.Contains(accountId)");
                }
            }

            stringBuilder.AppendLine($"                if ({string.Join(" || ", ownershipChecks)})");
            stringBuilder.AppendLine("                {");
            stringBuilder.AppendLine("                    return Result.Ok();");
            stringBuilder.AppendLine("                }");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"                return Result.Fail(\"Access denied: only {parentEntity.ToLowerInvariant()} owners can delete {parentEntity.ToLowerInvariant()} versions.\");");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
            stringBuilder.Append("            return Result.Ok();");
        }
    }
}
