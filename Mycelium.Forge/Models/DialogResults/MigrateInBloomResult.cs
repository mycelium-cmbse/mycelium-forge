// ------------------------------------------------------------------------------------------------
// <copyright file="MigrateInBloomResult.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.DialogResults
{
    /// <summary>
    /// Represents the result payload containing destination project and version constraint when initiating a migration in
    /// Bloom.
    /// </summary>
    public class MigrateInBloomResult
    {
        /// <summary>
        /// Gets or sets the name of the destination project.
        /// </summary>
        public string ProjectName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the version constraint expression for the package migration.
        /// </summary>
        public string VersionConstraint { get; set; } = string.Empty;
    }
}
