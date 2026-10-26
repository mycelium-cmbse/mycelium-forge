// ------------------------------------------------------------------------------------------------
// <copyright file="CapellaArchive.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Models.Capella
{
    using Auriga.Capellamodeller;

    /// <summary>
    /// Encapsulates the descriptors and model elements read from a Capella archive package.
    /// </summary>
    public class CapellaArchive
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CapellaArchive" /> class.
        /// </summary>
        /// <param name="project">The deserialized Capella project or library.</param>
        public CapellaArchive(IProject project)
        {
            this.Project = project;
        }

        /// <summary>
        /// Gets the deserialized Capella project or library.
        /// </summary>
        public IProject Project { get; }
    }
}
