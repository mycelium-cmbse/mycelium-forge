// ------------------------------------------------------------------------------------------------
// <copyright file="OwnerScopeConfiguration.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Models
{
    using Mycelium.Forge.Generator.Constants;

    /// <summary>
    /// Configuration class holding resolved values for an optional secondary owner scope entity check
    /// (e.g. checking <c>Organization.Administrator</c> for a <c>Package</c> owned by an organization).
    /// </summary>
    public class OwnerScopeConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwnerScopeConfiguration" /> class.
        /// </summary>
        /// <param name="config">The raw behavior configuration dictionary.</param>
        public OwnerScopeConfiguration(Dictionary<string, string> config)
        {
            var entity = config.TryGetValue(ConfigurationKeys.OwnerScopeEntity, out var e) && !string.IsNullOrWhiteSpace(e) ? e : string.Empty;
            var property = config.TryGetValue(ConfigurationKeys.OwnerScopeProperty, out var p) && !string.IsNullOrWhiteSpace(p) ? p : PropertyNames.Owner;
            var rolesRaw = config.TryGetValue(ConfigurationKeys.OwnerScopeRoles, out var r) && !string.IsNullOrWhiteSpace(r) ? r : string.Empty;

            this.Entity = entity;
            this.Property = property;
            this.Roles = string.IsNullOrWhiteSpace(rolesRaw) ? [] : rolesRaw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (!string.IsNullOrWhiteSpace(entity))
            {
                var lowerFirst = $"{char.ToLowerInvariant(entity[0])}{entity[1..]}";
                this.ServiceType = $"I{entity}Service";
                this.ServiceField = $"{lowerFirst}Service";
                this.VarName = lowerFirst;
            }
            else
            {
                this.ServiceType = string.Empty;
                this.ServiceField = string.Empty;
                this.VarName = string.Empty;
            }
        }

        /// <summary>
        /// Gets the owner scope entity name.
        /// </summary>
        public string Entity { get; }

        /// <summary>
        /// Gets the property name on the primary scope/parent entity holding the owner scope identifier.
        /// </summary>
        public string Property { get; }

        /// <summary>
        /// Gets the roles on the owner scope entity that grant access.
        /// </summary>
        public string[] Roles { get; }

        /// <summary>
        /// Gets the domain service interface type name for the owner scope entity.
        /// </summary>
        public string ServiceType { get; }

        /// <summary>
        /// Gets the injected domain service field name for the owner scope entity.
        /// </summary>
        public string ServiceField { get; }

        /// <summary>
        /// Gets the local variable name used for the resolved owner scope entity.
        /// </summary>
        public string VarName { get; }

        /// <summary>
        /// Gets a value indicating whether an owner scope check is configured.
        /// </summary>
        public bool IsConfigured => !string.IsNullOrWhiteSpace(this.Entity) && this.Roles.Length > 0;

        /// <summary>
        /// Builds the asynchronous C# code block that reads the owner scope entity and returns <c>Result.Success</c>
        /// when the caller holds any configured owner scope role.
        /// </summary>
        /// <param name="parentVariableName">The local variable name of the primary scope/parent entity.</param>
        /// <returns>The formatted C# code block, or an empty string if not configured.</returns>
        public string BuildAsyncAccessCheck(string parentVariableName)
        {
            if (!this.IsConfigured)
            {
                return string.Empty;
            }

            var roleChecks = string.Join(" || ", this.Roles.Select(role => $"{this.VarName}.{role}.Contains(accountId)"));

            return $$"""

                                    var {{this.VarName}}Result = await this.{{this.ServiceField}}.ReadAsync(userContext, transaction, CancellationToken.None, [{{parentVariableName}}.{{this.Property}}]);

                                    if (!{{this.VarName}}Result.IsError && {{this.VarName}}Result.Value.Count > 0)
                                    {
                                        var {{this.VarName}} = {{this.VarName}}Result.Value[0];

                                        if ({{roleChecks}})
                                        {
                                            return Result.Success;
                                        }
                                    }
                """;
        }
    }
}
