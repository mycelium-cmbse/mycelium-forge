// ------------------------------------------------------------------------------------------------
// <copyright file="EntityPermissionDefinition.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.DataLoaders.PermissionModels
{
    /// <summary>
    /// Represents the declarative CRUD permission and ownership property configuration for an entity.
    /// </summary>
    public class EntityPermissionDefinition
    {
        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        public string EntityName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the permission required to create instances of the entity.
        /// </summary>
        public string CreatePermission { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the permission required to read instances of the entity.
        /// </summary>
        public string ReadPermission { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the permission required to update instances of the entity.
        /// </summary>
        public string UpdatePermission { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the permission required to delete instances of the entity.
        /// </summary>
        public string DeletePermission { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the property name on the entity representing the owner or owner collection.
        /// </summary>
        public string OwnerProperty { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the property name on the entity representing maintainers or member collection.
        /// </summary>
        public string MaintainerProperty { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the property name on the entity representing visibility.
        /// </summary>
        public string VisibilityProperty { get; set; } = string.Empty;
    }
}
