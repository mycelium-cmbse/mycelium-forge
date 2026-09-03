// ------------------------------------------------------------------------------------------------
// <copyright file="ReadFilterHelper.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.HandleBarHelpers
{
    using System.Text;
    using System.Text.RegularExpressions;

    using HandlebarsDotNet;

    using Mycelium.Forge.Generator.Constants;
    using Mycelium.Forge.Generator.DataLoaders.PermissionModels;
    using Mycelium.Forge.Generator.HandleBarHelpers.BehaviorTypeHelpers;

    using uml4net.StructuredClassifiers;

    /// <summary>
    /// A Handlebars helper that generates the SQL read visibility filter factories for entities.
    /// </summary>
    public static class ReadFilterHelper
    {
        /// <summary>
        /// The error message used when the Handlebars context is not an <see cref="IClass" />.
        /// </summary>
        private const string ContextMustBeIClass = "context is supposed to be an IClass";

        /// <summary>
        /// Registry of behavior type helpers keyed by behavior type name.
        /// </summary>
        private static readonly Dictionary<string, IBehaviorTypeHelper> BehaviorTypeHelpers = new()
        {
            { BehaviorTypes.ScopeItem, new ScopeItemBehaviorTypeHelper() },
            { BehaviorTypes.ParentDelegation, new ParentDelegationBehaviorTypeHelper() },
            { BehaviorTypes.OrganizationScope, new OrganizationScopeBehaviorTypeHelper() },
            { BehaviorTypes.InvitationWorkflow, new InvitationWorkflowBehaviorTypeHelper() }
        };

        /// <summary>
        /// Cache of entity permission definitions loaded from CSV.
        /// </summary>
        private static Dictionary<string, EntityPermissionDefinition> entityPermissions = [];

        /// <summary>
        /// Cache of entity behavior definitions loaded from CSV.
        /// </summary>
        private static Dictionary<string, EntityBehaviorDefinition> entityBehaviors = [];

        /// <summary>
        /// Sets all configuration dictionaries: entity permissions and entity behaviors.
        /// </summary>
        /// <param name="entityDefinitions">The dictionary of entity permission definitions.</param>
        /// <param name="behaviorDefinitions">The dictionary of entity behavior definitions.</param>
        public static void SetConfigurations(
            Dictionary<string, EntityPermissionDefinition> entityDefinitions,
            Dictionary<string, EntityBehaviorDefinition> behaviorDefinitions = null)
        {
            entityPermissions = entityDefinitions ?? [];
            entityBehaviors = behaviorDefinitions ?? [];
        }

        /// <summary>
        /// Registers the read filter Handlebars helpers with the specified Handlebars context.
        /// </summary>
        /// <param name="handlebars">The <see cref="IHandlebars" /> context.</param>
        public static void RegisterReadFilterHelper(this IHandlebars handlebars)
        {
            ArgumentNullException.ThrowIfNull(handlebars);

            handlebars.RegisterHelper("ReadFilter.WriteFilterFromUserContext", WriteFilterFromUserContext);
        }

        /// <summary>
        /// Builds the SQL read visibility filter predicate for the specified class, or empty if unrestricted.
        /// </summary>
        /// <param name="class">The subject <see cref="IClass" />.</param>
        /// <returns>The SQL predicate string, or empty if unrestricted.</returns>
        public static string BuildVisibilityPredicate(IClass @class)
        {
            ArgumentNullException.ThrowIfNull(@class);

            return BuildVisibilityPredicate(@class.Name, @class);
        }

        /// <summary>
        /// Builds the SQL read visibility filter predicate for the specified entity class name, or empty if unrestricted.
        /// </summary>
        /// <param name="className">The name of the entity class.</param>
        /// <param name="class">The optional <see cref="IClass" /> instance if available.</param>
        /// <returns>The SQL predicate string, or empty if unrestricted.</returns>
        public static string BuildVisibilityPredicate(string className, IClass @class = null)
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                return string.Empty;
            }

            entityBehaviors.TryGetValue(className, out var behavior);
            entityPermissions.TryGetValue(className, out var permission);

            if (behavior != null && BehaviorTypeHelpers.TryGetValue(behavior.BehaviorType, out var helper))
            {
                var predicate = helper.BuildReadFilterPredicate(@class, permission, behavior, name => BuildVisibilityPredicate(name));

                if (!string.IsNullOrWhiteSpace(predicate))
                {
                    return predicate;
                }
            }

            return BuildDefaultVisibilityPredicate(className, permission);
        }

        /// <summary>
        /// Writes the implementation of <c>FromUserContext</c> for the current entity class.
        /// </summary>
        /// <param name="writer">The <see cref="EncodedTextWriter" />.</param>
        /// <param name="context">The Handlebars <see cref="Context" /> containing an <see cref="IClass" />.</param>
        /// <param name="arguments">The Handlebars <see cref="Arguments" />.</param>
        private static void WriteFilterFromUserContext(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            if (context.Value is not IClass @class)
            {
                throw new ArgumentException(ContextMustBeIClass, nameof(context));
            }

            var predicate = BuildVisibilityPredicate(@class);

            if (string.IsNullOrWhiteSpace(predicate))
            {
                writer.WriteSafeString("            return SqlFilter.Empty;");
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine("            return new SqlFilter()");
            builder.AppendLine("                .Where(");
            builder.AppendLine("                    \"\"\"");
            builder.AppendLine(predicate);
            builder.AppendLine("                    \"\"\")");
            builder.AppendLine("                .AddParameter(\"@callerAccountId\", NpgsqlDbType.Uuid, userContext?.AccountId)");

            AppendParameters(builder, predicate);

            var trimmed = builder.ToString().TrimEnd();
            writer.WriteSafeString($"{trimmed};");
        }

        /// <summary>
        /// Builds the default SQL read visibility filter predicate based on entity permissions CSV data.
        /// </summary>
        /// <param name="className">The name of the entity class.</param>
        /// <param name="permission">The entity permission definition.</param>
        /// <returns>The SQL predicate string, or empty if unrestricted.</returns>
        private static string BuildDefaultVisibilityPredicate(string className, EntityPermissionDefinition permission)
        {
            if (permission == null)
            {
                return string.Empty;
            }

            var parts = new List<string>();

            if (!string.IsNullOrWhiteSpace(permission.VisibilityProperty))
            {
                var visibilityPropLower = char.ToLowerInvariant(permission.VisibilityProperty[0]) + permission.VisibilityProperty[1..];
                parts.Add($"(\"Thing\".\"data\"->>'{visibilityPropLower}' = 'PUBLIC')");
            }

            if (!string.IsNullOrWhiteSpace(permission.ReadPermission))
            {
                var readPermParts = permission.ReadPermission.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var permChecks = readPermParts.Select(p => $"@can{p} = true");
                parts.Add(string.Join(" OR ", permChecks));
            }

            var ownershipChecks = new List<string>();

            if (!string.IsNullOrWhiteSpace(permission.OwnerProperty))
            {
                if (permission.OwnerProperty.Equals("Id", StringComparison.OrdinalIgnoreCase))
                {
                    ownershipChecks.Add($"\"{className}\".\"id\" = @callerAccountId");
                }
                else if (permission.OwnerProperty.Equals("Owner", StringComparison.OrdinalIgnoreCase))
                {
                    ownershipChecks.Add($"\"{className}\".\"owner\" = @callerAccountId");
                }
                else
                {
                    var ownerPropCamel = char.ToLowerInvariant(permission.OwnerProperty[0]) + permission.OwnerProperty[1..];
                    ownershipChecks.Add($"EXISTS (SELECT 1 FROM \"Forge\".\"{className}_{ownerPropCamel}__Account\" WHERE \"source{className}\" = \"{className}\".\"id\" AND \"targetAccount\" = @callerAccountId)");
                }
            }

            if (!string.IsNullOrWhiteSpace(permission.MaintainerProperty))
            {
                var maintainerPropCamel = char.ToLowerInvariant(permission.MaintainerProperty[0]) + permission.MaintainerProperty[1..];
                ownershipChecks.Add($"EXISTS (SELECT 1 FROM \"Forge\".\"{className}_{maintainerPropCamel}__Account\" WHERE \"source{className}\" = \"{className}\".\"id\" AND \"targetAccount\" = @callerAccountId)");
            }

            if (ownershipChecks.Count > 0)
            {
                if (ownershipChecks.Count == 1 && !ownershipChecks[0].StartsWith("EXISTS"))
                {
                    parts.Add($"(@callerAccountId IS NOT NULL AND {ownershipChecks[0]})");
                }
                else
                {
                    var checkLines = string.Join("\r\n                        OR ", ownershipChecks);
                    parts.Add($"(@callerAccountId IS NOT NULL AND (\r\n                        {checkLines}\r\n                    ))");
                }
            }

            if (parts.Count == 0)
            {
                return string.Empty;
            }

            return "                    " + string.Join("\r\n                    OR ", parts);
        }

        /// <summary>
        /// Appends SQL filter parameter bindings to the specified builder based on parameters present in the predicate.
        /// </summary>
        /// <param name="builder">The <see cref="StringBuilder" /> to append to.</param>
        /// <param name="predicate">The SQL predicate string containing parameter names.</param>
        private static void AppendParameters(StringBuilder builder, string predicate)
        {
            var matches = Regex.Matches(predicate, "@(can[A-Za-z0-9_]+)");
            var handled = new HashSet<string>();

            foreach (Match match in matches)
            {
                var paramName = match.Groups[1].Value;

                if (handled.Add(paramName))
                {
                    var permName = paramName[3..];
                    builder.AppendLine($"                .AddParameter(\"@{paramName}\", NpgsqlDbType.Boolean, PermissionGuard.HasPermission(userContext, PermissionKind.{permName}))");
                }
            }
        }
    }
}
