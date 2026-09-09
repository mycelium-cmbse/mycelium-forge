// ------------------------------------------------------------------------------------------------
// <copyright file="PolicyMapping.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.DataLoaders.PermissionModels
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the mapping of a single permission to the list of roles that grant it.
    /// </summary>
    public class PolicyMapping
    {
        /// <summary>
        /// Gets or sets the PascalCase name of the permission enum.
        /// </summary>
        public string PermissionEnumName { get; set; }

        /// <summary>
        /// Gets or sets the list of role names that grant this permission.
        /// </summary>
        public List<string> AllowedRoles { get; set; } = [];
    }
}
