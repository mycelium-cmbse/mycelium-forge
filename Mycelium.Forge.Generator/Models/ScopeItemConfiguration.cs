// ------------------------------------------------------------------------------------------------
// <copyright file="ScopeItemConfiguration.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Models
{
    using Mycelium.Forge.Generator.Constants;
    using Mycelium.Forge.Generator.DataLoaders.PermissionModels;

    /// <summary>
    /// Configuration class holding resolved values for scope item permission behavior.
    /// </summary>
    public class ScopeItemConfiguration : BehaviorConfigurationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeItemConfiguration" /> class.
        /// </summary>
        /// <param name="definition">The entity permission definition.</param>
        /// <param name="behavior">The entity behavior definition.</param>
        public ScopeItemConfiguration(EntityPermissionDefinition definition, EntityBehaviorDefinition behavior)
            : base(definition, behavior)
        {
            var config = behavior.Configuration;

            if (!config.TryGetValue(ConfigurationKeys.ScopeEntity, out var scopeEntity) || string.IsNullOrWhiteSpace(scopeEntity))
            {
                throw new InvalidOperationException($"Entity '{this.EntityName}' with ScopeItem behavior must configure '{ConfigurationKeys.ScopeEntity}'.");
            }

            var ownerProp = config.TryGetValue(ConfigurationKeys.OwnerProperty, out var op) && !string.IsNullOrWhiteSpace(op) ? op : definition.OwnerProperty;

            if (string.IsNullOrWhiteSpace(ownerProp))
            {
                throw new InvalidOperationException($"Entity '{this.EntityName}' with ScopeItem behavior must configure '{ConfigurationKeys.OwnerProperty}' or have an OwnerProperty in entity permissions.");
            }

            var readBypassRaw = config.TryGetValue(ConfigurationKeys.ReadBypassPermissions, out var rbp) && !string.IsNullOrWhiteSpace(rbp) ? rbp : "ManageOrganizations";

            this.ScopeEntity = scopeEntity;
            this.ScopeServiceField = FormatServiceField(scopeEntity);
            this.OwnerProperty = ownerProp;
            this.OwnerColumn = ownerProp.ToLowerInvariant();
            this.ReadBypassPermissions = SplitValues(readBypassRaw);
        }

        /// <summary>
        /// Gets the scope entity name.
        /// </summary>
        public string ScopeEntity { get; }

        /// <summary>
        /// Gets the injected domain service field name for the scope entity.
        /// </summary>
        public string ScopeServiceField { get; }

        /// <summary>
        /// Gets the owner property name on the scope item.
        /// </summary>
        public string OwnerProperty { get; }

        /// <summary>
        /// Gets the lower-case database column name for the owner property.
        /// </summary>
        public string OwnerColumn { get; }

        /// <summary>
        /// Gets the permissions that bypass membership checks for read access.
        /// </summary>
        public string[] ReadBypassPermissions { get; }
    }
}
