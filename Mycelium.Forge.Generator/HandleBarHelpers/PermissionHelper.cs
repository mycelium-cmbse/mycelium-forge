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

    using uml4net.Extensions;
    using uml4net.StructuredClassifiers;

    /// <summary>
    /// Handlebars helpers for generating entity-specific permission logic in permission service classes.
    /// </summary>
    public static class PermissionHelper
    {
        /// <summary>
        /// Cache of entity permission definitions loaded from CSV.
        /// </summary>
        private static Dictionary<string, EntityPermissionDefinition> entityPermissions = [];

        /// <summary>
        /// Sets or initializes the entity permissions dictionary.
        /// </summary>
        /// <param name="definitions">The dictionary of entity permission definitions.</param>
        public static void SetEntityPermissions(Dictionary<string, EntityPermissionDefinition> definitions)
        {
            entityPermissions = definitions ?? [];
        }

        /// <summary>
        /// Registers the permission helpers with the given <see cref="IHandlebars" /> instance.
        /// </summary>
        /// <param name="handlebars">The <see cref="IHandlebars" /> instance to register with.</param>
        public static void RegisterPermissionHelper(this IHandlebars handlebars)
        {
            ArgumentNullException.ThrowIfNull(handlebars);

            handlebars.RegisterHelper("Permission.WriteIsAllowedToCreate", (writer, context, _) =>
            {
                if (context.Value is not IClass @class)
                {
                    throw new ArgumentException("Context must be an IClass", nameof(context));
                }

                entityPermissions.TryGetValue(@class.Name, out var definition);
                var stringBuilder = new StringBuilder();

                if (definition != null && !string.IsNullOrWhiteSpace(definition.CreatePermission))
                {
                    stringBuilder.Append($"            return Task.FromResult(PermissionGuard.GuardPermission(userContext, PermissionKind.{definition.CreatePermission}));");
                }
                else
                {
                    stringBuilder.Append("            return Task.FromResult(Result.Ok());");
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
                var stringBuilder = new StringBuilder();

                if (definition != null)
                {
                    WritePropertyOwnershipCheck(stringBuilder, @class, definition.OwnerProperty, "thing");
                    WritePropertyOwnershipCheck(stringBuilder, @class, definition.MaintainerProperty, "thing");

                    if (!string.IsNullOrWhiteSpace(definition.VisibilityProperty))
                    {
                        stringBuilder.AppendLine($"            if (thing.{definition.VisibilityProperty} == VisibilityKind.PUBLIC)");
                        stringBuilder.AppendLine("            {");
                        stringBuilder.AppendLine("                return Task.FromResult(PermissionGuard.GuardPermission(userContext, PermissionKind.ReadPublicPackage));");
                        stringBuilder.AppendLine("            }");
                        stringBuilder.AppendLine();
                        stringBuilder.AppendLine($"            if (thing.{definition.VisibilityProperty} == VisibilityKind.INTERNAL)");
                        stringBuilder.AppendLine("            {");
                        stringBuilder.AppendLine("                return Task.FromResult(PermissionGuard.GuardPermission(userContext, PermissionKind.ReadOrganizationVisiblePackage));");
                        stringBuilder.AppendLine("            }");
                        stringBuilder.AppendLine();
                        stringBuilder.AppendLine($"            if (thing.{definition.VisibilityProperty} == VisibilityKind.PRIVATE)");
                        stringBuilder.AppendLine("            {");
                        stringBuilder.AppendLine("                return Task.FromResult(PermissionGuard.GuardPermission(userContext, PermissionKind.ReadPrivatePackage));");
                        stringBuilder.AppendLine("            }");
                        stringBuilder.AppendLine();
                    }

                    if (!string.IsNullOrWhiteSpace(definition.ReadPermission))
                    {
                        stringBuilder.Append($"            return Task.FromResult(PermissionGuard.GuardPermission(userContext, PermissionKind.{definition.ReadPermission}));");
                    }
                    else
                    {
                        stringBuilder.Append("            return Task.FromResult(Result.Ok());");
                    }
                }
                else
                {
                    stringBuilder.Append("            return Task.FromResult(Result.Ok());");
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
                var stringBuilder = new StringBuilder();

                if (definition != null)
                {
                    WritePropertyOwnershipCheck(stringBuilder, @class, definition.OwnerProperty, "existingThing");

                    if (!string.IsNullOrWhiteSpace(definition.UpdatePermission))
                    {
                        stringBuilder.Append($"            return Task.FromResult(PermissionGuard.GuardPermission(userContext, PermissionKind.{definition.UpdatePermission}));");
                    }
                    else
                    {
                        stringBuilder.Append("            return Task.FromResult(Result.Ok());");
                    }
                }
                else
                {
                    stringBuilder.Append("            return Task.FromResult(Result.Ok());");
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
                var stringBuilder = new StringBuilder();

                if (definition != null && !string.IsNullOrWhiteSpace(definition.DeletePermission))
                {
                    stringBuilder.Append($"            return Task.FromResult(PermissionGuard.GuardPermission(userContext, PermissionKind.{definition.DeletePermission}));");
                }
                else
                {
                    stringBuilder.Append("            return Task.FromResult(Result.Ok());");
                }

                writer.WriteSafeString(stringBuilder.ToString());
            });
        }

        /// <summary>
        /// Emits a check verifying whether the user's account matches the specified property on the entity.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder" /> to append to.</param>
        /// <param name="class">The <see cref="IClass" /> being generated.</param>
        /// <param name="propertyName">The property name to inspect on the entity.</param>
        /// <param name="targetEntityName">The parameter name of the target entity instance.</param>
        private static void WritePropertyOwnershipCheck(StringBuilder stringBuilder, IClass @class, string propertyName, string targetEntityName)
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
            stringBuilder.AppendLine("                return Task.FromResult(Result.Ok());");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine();
        }
    }
}
