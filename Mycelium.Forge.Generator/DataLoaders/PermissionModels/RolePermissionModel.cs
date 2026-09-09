// ------------------------------------------------------------------------------------------------
// <copyright file="RolePermissionModel.cs" company="Starion Group S.A.">
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
    /// Represents the complete parsed role-permission model extracted from a roles CSV.
    /// </summary>
    public class RolePermissionModel
    {
        /// <summary>
        /// Gets or sets the list of roles defined in the CSV.
        /// </summary>
        public List<RoleDefinition> Roles { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of permissions defined in the CSV column headers.
        /// </summary>
        public List<PermissionDefinition> Permissions { get; set; } = [];

        /// <summary>
        /// Gets or sets the policy mappings (permission -> allowed roles).
        /// </summary>
        public List<PolicyMapping> Policies { get; set; } = [];
    }
}
