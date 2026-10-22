// ------------------------------------------------------------------------------------------------
// <copyright file="KparManifest.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Models
{
    /// <summary>
    /// Represents the registry metadata projected from the <c>.project.json</c> and <c>.meta.json</c> of a kpar.
    /// </summary>
    public class KparManifest : IArtifactManifest
    {
        /// <summary>
        /// Gets the format of the artifact the manifest was extracted from.
        /// </summary>
        public ArtifactKind Kind => ArtifactKind.Kpar;

        /// <summary>
        /// Gets or sets the project name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the project description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the SemVer 2.0.0 project version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the project license.
        /// </summary>
        public string License { get; set; }

        /// <summary>
        /// Gets or sets the project maintainers.
        /// </summary>
        public List<string> Maintainers { get; set; } = [];

        /// <summary>
        /// Gets or sets the project website.
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Gets or sets the project topics.
        /// </summary>
        public List<string> Topics { get; set; } = [];

        /// <summary>
        /// Gets or sets the dependency graph declared by the <c>usage[]</c> list.
        /// </summary>
        public List<KparDependency> Dependencies { get; set; } = [];

        /// <summary>
        /// Gets or sets the metamodel IRI that populates the METAMODEL facet.
        /// </summary>
        public string Metamodel { get; set; }

        /// <summary>
        /// Gets or sets the creation timestamp of the project.
        /// </summary>
        public DateTimeOffset Created { get; set; }

        /// <summary>
        /// Gets or sets the archive path of the model interchange file of each root namespace, keyed by root namespace.
        /// </summary>
        public Dictionary<string, string> ModelPaths { get; set; } = [];

        /// <summary>
        /// Gets or sets the checksums declared for the model interchange files, keyed by archive path.
        /// </summary>
        public Dictionary<string, KparChecksum> Checksums { get; set; } = [];
    }
}
