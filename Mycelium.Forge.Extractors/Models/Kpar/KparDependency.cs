// ------------------------------------------------------------------------------------------------
// <copyright file="KparDependency.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Models.Kpar
{
    /// <summary>
    /// Represents one <c>usage[]</c> entry of a <c>.project.json</c>, which is an edge of the dependency graph.
    /// </summary>
    public class KparDependency
    {
        /// <summary>
        /// Gets or sets the IRI of the used project.
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// Gets or sets the optional version constraint on the used project.
        /// </summary>
        public string VersionConstraint { get; set; }
    }
}
