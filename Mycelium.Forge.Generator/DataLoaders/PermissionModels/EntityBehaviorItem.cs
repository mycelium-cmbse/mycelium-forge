// ------------------------------------------------------------------------------------------------
// <copyright file="EntityBehaviorItem.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.DataLoaders.PermissionModels
{
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents an individual entity behavior configuration entry.
    /// </summary>
    public class EntityBehaviorItem
    {
        /// <summary>
        /// Gets or sets the entity name.
        /// </summary>
        [JsonPropertyName("entity")]
        public string Entity { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the alternative entity name property.
        /// </summary>
        [JsonPropertyName("entityName")]
        public string EntityName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the behavior type.
        /// </summary>
        [JsonPropertyName("behaviorType")]
        public string BehaviorType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the behavior configuration settings.
        /// </summary>
        [JsonPropertyName("configuration")]
        public Dictionary<string, JsonElement> Configuration { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    }
}
