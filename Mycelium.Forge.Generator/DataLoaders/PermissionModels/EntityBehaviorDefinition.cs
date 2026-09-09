// ------------------------------------------------------------------------------------------------
// <copyright file="EntityBehaviorDefinition.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.DataLoaders.PermissionModels
{
    /// <summary>
    /// Represents a specialized domain permission behavior configuration parsed from configuration.
    /// </summary>
    public class EntityBehaviorDefinition
    {
        /// <summary>
        /// Gets or sets the target entity name.
        /// </summary>
        public string EntityName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the behavior type kind (e.g. "ScopeItem", "ParentDelegation", "InvitationWorkflow", "OrganizationScope").
        /// </summary>
        public string BehaviorType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the key-value dictionary of configuration settings for this behavior.
        /// </summary>
        public Dictionary<string, string> Configuration { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    }
}
