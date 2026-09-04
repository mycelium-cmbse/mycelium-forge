// ------------------------------------------------------------------------------------------------
// <copyright file="PropertyPermissionDefinition.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.DataLoaders.PermissionModels
{
    /// <summary>
    /// Represents a property-level mutation permission requirement parsed from CSV.
    /// </summary>
    public class PropertyPermissionDefinition
    {
        /// <summary>
        /// Gets or sets the target entity name.
        /// </summary>
        public string EntityName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the property name being mutated.
        /// </summary>
        public string Property { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the required permission enum name when this property is mutated.
        /// </summary>
        public string RequiredPermission { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the operation type (e.g. "Update").
        /// </summary>
        public string Operation { get; set; } = string.Empty;
    }
}
