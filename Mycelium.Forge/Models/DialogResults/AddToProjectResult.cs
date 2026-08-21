// ------------------------------------------------------------------------------------------------
// <copyright file="AddToProjectResult.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.DialogResults
{
    /// <summary>
    /// Represents the result payload containing the project selection and version constraint when adding a dependency.
    /// </summary>
    public class AddToProjectResult
    {
        /// <summary>
        /// Gets or sets the name of the destination project.
        /// </summary>
        public string ProjectName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the version constraint expression for the package dependency.
        /// </summary>
        public string VersionConstraint { get; set; } = string.Empty;
    }
}
