// ------------------------------------------------------------------------------------------------
// <copyright file="RoleDefinition.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.DataLoaders.PermissionModels
{
    /// <summary>
    /// Represents a role parsed from a CSV row, including its human-readable summary
    /// and the list of permission enum names granted to it.
    /// </summary>
    public class RoleDefinition
    {
        /// <summary>
        /// Gets or sets the name of the role in PascalCase.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the role this role inherits permissions from, if any.
        /// </summary>
        public string Inherits { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the human-readable summary description of the role.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the list of permission enum names granted to this role.
        /// </summary>
        public List<string> GrantedPermissions { get; set; } = [];
    }
}
