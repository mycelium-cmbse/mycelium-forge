// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeEntityBehaviorsDocument.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.DataLoaders.PermissionModels
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents the root document structure of an entity behaviors JSON file.
    /// </summary>
    public class ForgeEntityBehaviorsDocument
    {
        /// <summary>
        /// Gets or sets the collection of entity behavior items.
        /// </summary>
        [JsonPropertyName("behaviors")]
        public List<EntityBehaviorItem> Behaviors { get; set; } = [];
    }
}
