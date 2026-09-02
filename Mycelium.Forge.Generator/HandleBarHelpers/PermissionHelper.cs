// ------------------------------------------------------------------------------------------------
// <copyright file="PermissionHelper.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.HandleBarHelpers
{
    using System.Text;

    using HandlebarsDotNet;

    using Mycelium.Forge.Generator.DataLoaders.PermissionModels;
    using Mycelium.Forge.Generator.Extensions;
    using Mycelium.Forge.Generator.HandleBarHelpers.BehaviorTypeHelpers;

    using uml4net.Extensions;
    using uml4net.StructuredClassifiers;

    /// <summary>
    /// Handlebars helpers for generating entity-specific permission logic in permission service classes.
    /// </summary>
    public static class PermissionHelper
    {
        /// <summary>
        /// Registry of behavior type helpers keyed by behavior type name.
        /// </summary>
        private static readonly Dictionary<string, IBehaviorTypeHelper> BehaviorTypeHelpers = new()
        {
            { "ScopeItem", new ScopeItemBehaviorTypeHelper() },
            { "ParentDelegation", new ParentDelegationBehaviorTypeHelper() },
            { "OrganizationScope", new OrganizationScopeBehaviorTypeHelper() },
            { "InvitationWorkflow", new InvitationWorkflowBehaviorTypeHelper() }
        };

        /// <summary>
        /// Cache of entity permission definitions loaded from CSV.
        /// </summary>
        private static Dictionary<string, EntityPermissionDefinition> entityPermissions = [];

        /// <summary>
        /// Cache of property-level permission definitions loaded from CSV, grouped by entity name.
        /// </summary>
        private static Dictionary<string, List<PropertyPermissionDefinition>> propertyPermissions = [];

        /// <summary>
        /// Cache of entity behavior definitions loaded from CSV.
        /// </summary>
        private static Dictionary<string, EntityBehaviorDefinition> entityBehaviors = [];

        /// <summary>
        /// Sets all configuration dictionaries: entity permissions, property permissions, and entity behaviors.
        /// </summary>
        /// <param name="entityDefinitions">The dictionary of entity permission definitions.</param>
        /// <param name="propertyDefinitions">The dictionary of property permission definitions grouped by entity name.</param>
        /// <param name="behaviorDefinitions">The dictionary of entity behavior definitions.</param>
        public static void SetConfigurations(
            Dictionary<string, EntityPermissionDefinition> entityDefinitions,
            Dictionary<string, List<PropertyPermissionDefinition>> propertyDefinitions = null,
            Dictionary<string, EntityBehaviorDefinition> behaviorDefinitions = null)
        {
            entityPermissions = entityDefinitions ?? [];
            propertyPermissions = propertyDefinitions ?? [];
            entityBehaviors = behaviorDefinitions ?? [];
        }

        /// <summary>
        /// Determines whether the specified class and operation require an asynchronous implementation hook.
        /// </summary>
        /// <param name="class">The <see cref="IClass" /> being generated.</param>
        /// <param name="operation">The operation name ("Create", "Read", "Update", "Delete").</param>
        /// <returns>True if the operation requires async; otherwise false.</returns>
        public static bool IsAsyncMethod(IClass @class, string operation)
        {
            if (entityBehaviors.TryGetValue(@class.Name, out var behavior) &&
                behavior != null &&
                BehaviorTypeHelpers.TryGetValue(behavior.BehaviorType, out var helper))
            {
                return helper.IsAsyncMethod(operation);
            }

            return false;
        }

        /// <summary>
        /// Registers the permission helpers with the given <see cref="IHandlebars" /> instance.
        /// </summary>
        /// <param name="handlebars">The <see cref="IHandlebars" /> instance to register with.</param>
        public static void RegisterPermissionHelper(this IHandlebars handlebars)
        {
            ArgumentNullException.ThrowIfNull(handlebars);

            handlebars.RegisterHelper("Permission.WriteAsyncModifierCreate", (writer, context, _) =>
            {
                if (context.Value is IClass @class && IsAsyncMethod(@class, "Create"))
                {
                    writer.WriteSafeString("async ");
                }
            });

            handlebars.RegisterHelper("Permission.WriteAsyncModifierRead", (writer, context, _) =>
            {
                if (context.Value is IClass @class && IsAsyncMethod(@class, "Read"))
                {
                    writer.WriteSafeString("async ");
                }
            });

            handlebars.RegisterHelper("Permission.WriteAsyncModifierUpdate", (writer, context, _) =>
            {
                if (context.Value is IClass @class && IsAsyncMethod(@class, "Update"))
                {
                    writer.WriteSafeString("async ");
                }
            });

            handlebars.RegisterHelper("Permission.WriteAsyncModifierDelete", (writer, context, _) =>
            {
                if (context.Value is IClass @class && IsAsyncMethod(@class, "Delete"))
                {
                    writer.WriteSafeString("async ");
                }
            });

            handlebars.RegisterHelper("Permission.WriteFieldsAndConstructors", (writer, context, _) =>
            {
                if (context.Value is not IClass @class)
                {
                    throw new ArgumentException("Context must be an IClass", nameof(context));
                }

                entityBehaviors.TryGetValue(@class.Name, out var behavior);
                var stringBuilder = new StringBuilder();

                if (behavior != null && BehaviorTypeHelpers.TryGetValue(behavior.BehaviorType, out var helper))
                {
                    helper.WriteFieldsAndConstructors(stringBuilder, @class, behavior);
                }
                else
                {
                    stringBuilder.AppendLine("        /// <summary>");
                    stringBuilder.AppendLine($"        /// Initializes a new instance of the <see cref=\"{@class.Name}PermissionService\"/> class.");
                    stringBuilder.AppendLine("        /// </summary>");
                    stringBuilder.AppendLine($"        public {@class.Name}PermissionService()");
                    stringBuilder.AppendLine("        {");
                    stringBuilder.AppendLine("        }");
                }

                writer.WriteSafeString(stringBuilder.ToString());
            });

            handlebars.RegisterHelper("Permission.WriteIsAllowedToCreate", (writer, context, _) =>
            {
                if (context.Value is not IClass @class)
                {
                    throw new ArgumentException("Context must be an IClass", nameof(context));
                }

                entityPermissions.TryGetValue(@class.Name, out var definition);
                entityBehaviors.TryGetValue(@class.Name, out var behavior);
                var isAsync = IsAsyncMethod(@class, "Create");
                var stringBuilder = new StringBuilder();

                if (behavior != null &&
                    BehaviorTypeHelpers.TryGetValue(behavior.BehaviorType, out var helper) &&
                    helper.WriteIsAllowedToCreate(stringBuilder, @class, definition, behavior, isAsync))
                {
                    // Handled by behavior helper
                }
                else if (definition != null && !string.IsNullOrWhiteSpace(definition.CreatePermission))
                {
                    var guardExpr = EmitPermissionGuard(definition.CreatePermission);
                    stringBuilder.Append(isAsync ? $"            return {guardExpr};" : $"            return Task.FromResult({guardExpr});");
                }
                else
                {
                    stringBuilder.Append(isAsync ? "            return Result.Ok();" : "            return Task.FromResult(Result.Ok());");
                }

                writer.WriteSafeString(stringBuilder.ToString());
            });

            handlebars.RegisterHelper("Permission.WriteIsAllowedToRead", (writer, context, _) =>
            {
                if (context.Value is not IClass @class)
                {
                    throw new ArgumentException("Context must be an IClass", nameof(context));
                }

                entityPermissions.TryGetValue(@class.Name, out var definition);
                entityBehaviors.TryGetValue(@class.Name, out var behavior);
                var isAsync = IsAsyncMethod(@class, "Read");
                var stringBuilder = new StringBuilder();

                if (behavior != null &&
                    BehaviorTypeHelpers.TryGetValue(behavior.BehaviorType, out var helper) &&
                    helper.WriteIsAllowedToRead(stringBuilder, @class, definition, behavior, isAsync))
                {
                    // Handled by behavior helper
                }
                else if (definition != null)
                {
                    if (!string.IsNullOrWhiteSpace(definition.VisibilityProperty))
                    {
                        stringBuilder.AppendLine($"            if (thing.{definition.VisibilityProperty} == VisibilityKind.PUBLIC)");
                        stringBuilder.AppendLine("            {");
                        stringBuilder.AppendLine(isAsync ? "                return Result.Ok();" : "                return Task.FromResult(Result.Ok());");
                        stringBuilder.AppendLine("            }");
                        stringBuilder.AppendLine();
                    }

                    WritePropertyOwnershipCheck(stringBuilder, @class, definition.OwnerProperty, "thing", isAsync);

                    if (!string.IsNullOrWhiteSpace(definition.MaintainerProperty))
                    {
                        stringBuilder.AppendLine($"            if (userContext.AccountId.HasValue && thing.{definition.MaintainerProperty}.Contains(userContext.AccountId.Value))");
                        stringBuilder.AppendLine("            {");
                        stringBuilder.AppendLine(isAsync ? "                return Result.Ok();" : "                return Task.FromResult(Result.Ok());");
                        stringBuilder.AppendLine("            }");
                        stringBuilder.AppendLine();
                    }

                    if (!string.IsNullOrWhiteSpace(definition.ReadPermission))
                    {
                        var guardExpr = EmitPermissionGuard(definition.ReadPermission);
                        stringBuilder.Append(isAsync ? $"            return {guardExpr};" : $"            return Task.FromResult({guardExpr});");
                    }
                    else
                    {
                        stringBuilder.Append(isAsync ? "            return Result.Ok();" : "            return Task.FromResult(Result.Ok());");
                    }
                }
                else
                {
                    stringBuilder.Append(isAsync ? "            return Result.Ok();" : "            return Task.FromResult(Result.Ok());");
                }

                writer.WriteSafeString(stringBuilder.ToString());
            });

            handlebars.RegisterHelper("Permission.WriteIsAllowedToUpdate", (writer, context, _) =>
            {
                if (context.Value is not IClass @class)
                {
                    throw new ArgumentException("Context must be an IClass", nameof(context));
                }

                entityPermissions.TryGetValue(@class.Name, out var definition);
                entityBehaviors.TryGetValue(@class.Name, out var behavior);
                propertyPermissions.TryGetValue(@class.Name, out var propertyDefs);
                var isAsync = IsAsyncMethod(@class, "Update");
                var stringBuilder = new StringBuilder();

                if (behavior != null &&
                    BehaviorTypeHelpers.TryGetValue(behavior.BehaviorType, out var helper) &&
                    helper.WriteIsAllowedToUpdate(stringBuilder, @class, definition, behavior, propertyDefs, isAsync))
                {
                    // Handled by behavior helper
                }
                else if (definition != null)
                {
                    if (propertyDefs is { Count: > 0 })
                    {
                        var allProperties = @class.QueryDtoClassProperties().ToList();

                        foreach (var propDef in propertyDefs)
                        {
                            var prop = allProperties.FirstOrDefault(p => p.Name.Equals(propDef.Property, StringComparison.OrdinalIgnoreCase));
                            var isEnumerable = prop != null && prop.QueryIsEnumerable();

                            if (isEnumerable)
                            {
                                stringBuilder.AppendLine($"            if (!existingThing.{propDef.Property}.SequenceEqual(updatedThing.{propDef.Property}))");
                            }
                            else
                            {
                                stringBuilder.AppendLine($"            if (existingThing.{propDef.Property} != updatedThing.{propDef.Property})");
                            }

                            stringBuilder.AppendLine("            {");
                            var propGuardExpr = EmitPermissionGuard(propDef.RequiredPermission);
                            stringBuilder.AppendLine($"                var guard = {propGuardExpr};");
                            stringBuilder.AppendLine();
                            stringBuilder.AppendLine("                if (guard.IsFailed)");
                            stringBuilder.AppendLine("                {");
                            stringBuilder.AppendLine(isAsync ? "                    return guard;" : "                    return Task.FromResult(guard);");
                            stringBuilder.AppendLine("                }");
                            stringBuilder.AppendLine("            }");
                            stringBuilder.AppendLine();
                        }
                    }

                    WritePropertyOwnershipCheck(stringBuilder, @class, definition.OwnerProperty, "existingThing", isAsync);

                    if (!string.IsNullOrWhiteSpace(definition.UpdatePermission))
                    {
                        var guardExpr = EmitPermissionGuard(definition.UpdatePermission);
                        stringBuilder.Append(isAsync ? $"            return {guardExpr};" : $"            return Task.FromResult({guardExpr});");
                    }
                    else
                    {
                        stringBuilder.Append(isAsync ? "            return Result.Ok();" : "            return Task.FromResult(Result.Ok());");
                    }
                }
                else
                {
                    stringBuilder.Append(isAsync ? "            return Result.Ok();" : "            return Task.FromResult(Result.Ok());");
                }

                writer.WriteSafeString(stringBuilder.ToString());
            });

            handlebars.RegisterHelper("Permission.WriteIsAllowedToDelete", (writer, context, _) =>
            {
                if (context.Value is not IClass @class)
                {
                    throw new ArgumentException("Context must be an IClass", nameof(context));
                }

                entityPermissions.TryGetValue(@class.Name, out var definition);
                entityBehaviors.TryGetValue(@class.Name, out var behavior);
                var isAsync = IsAsyncMethod(@class, "Delete");
                var stringBuilder = new StringBuilder();

                if (behavior != null &&
                    BehaviorTypeHelpers.TryGetValue(behavior.BehaviorType, out var helper) &&
                    helper.WriteIsAllowedToDelete(stringBuilder, @class, definition, behavior, isAsync))
                {
                    // Handled by behavior helper
                }
                else if (definition != null)
                {
                    WritePropertyOwnershipCheck(stringBuilder, @class, definition.OwnerProperty, "thing", isAsync);

                    if (!string.IsNullOrWhiteSpace(definition.DeletePermission))
                    {
                        var guardExpr = EmitPermissionGuard(definition.DeletePermission);
                        stringBuilder.Append(isAsync ? $"            return {guardExpr};" : $"            return Task.FromResult({guardExpr});");
                    }
                    else
                    {
                        stringBuilder.Append(isAsync ? "            return Result.Ok();" : "            return Task.FromResult(Result.Ok());");
                    }
                }
                else
                {
                    stringBuilder.Append(isAsync ? "            return Result.Ok();" : "            return Task.FromResult(Result.Ok());");
                }

                writer.WriteSafeString(stringBuilder.ToString());
            });
        }

        /// <summary>
        /// Emits a C# permission guard expression, handling single permissions, OR expressions ('|'), and AND expressions ('&amp;
        /// ').
        /// </summary>
        /// <param name="permissionExpression">The permission expression.</param>
        /// <param name="userContextName">The parameter name for the user context.</param>
        /// <returns>A string representation of the permission guard expression.</returns>
        private static string EmitPermissionGuard(string permissionExpression, string userContextName = "userContext")
        {
            if (string.IsNullOrWhiteSpace(permissionExpression))
            {
                return "Result.Ok()";
            }

            if (permissionExpression.Contains('|'))
            {
                var parts = permissionExpression.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var enumList = string.Join(", ", parts.Select(p => $"PermissionKind.{p}"));
                return $"PermissionGuard.GuardAnyPermission({userContextName}, {enumList})";
            }

            if (permissionExpression.Contains('&'))
            {
                var parts = permissionExpression.Split('&', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var enumList = string.Join(", ", parts.Select(p => $"PermissionKind.{p}"));
                return $"PermissionGuard.GuardAllPermissions({userContextName}, {enumList})";
            }

            return $"PermissionGuard.GuardPermission({userContextName}, PermissionKind.{permissionExpression.Trim()})";
        }

        /// <summary>
        /// Emits a check verifying whether the user's account matches the specified property on the entity.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to append to.</param>
        /// <param name="class">The <see cref="IClass" /> being generated.</param>
        /// <param name="propertyName">The property name to inspect on the entity.</param>
        /// <param name="targetEntityName">The parameter name of the target entity instance.</param>
        /// <param name="isAsync">Whether the enclosing method is asynchronous.</param>
        private static void WritePropertyOwnershipCheck(StringBuilder stringBuilder, IClass @class, string propertyName, string targetEntityName, bool isAsync = false)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return;
            }

            var allProperties = @class.QueryDtoClassProperties();
            var property = allProperties.FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

            var isEnumerable = property != null && property.QueryIsEnumerable();

            if (isEnumerable)
            {
                stringBuilder.AppendLine($"            if (userContext.AccountId.HasValue && {targetEntityName}.{propertyName}.Contains(userContext.AccountId.Value))");
            }
            else
            {
                stringBuilder.AppendLine($"            if (userContext.AccountId.HasValue && {targetEntityName}.{propertyName} == userContext.AccountId.Value)");
            }

            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine(isAsync ? "                return Result.Ok();" : "                return Task.FromResult(Result.Ok());");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
        }
    }
}
