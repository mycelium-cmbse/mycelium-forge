// ------------------------------------------------------------------------------------------------
// <copyright file="PackageRelationshipModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.Package
{
    /// <summary>
    /// Represents a related package or project in the dependency or dependent graph.
    /// </summary>
    public class PackageRelationshipModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageRelationshipModel" /> class.
        /// </summary>
        public PackageRelationshipModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageRelationshipModel" /> class with specified properties.
        /// </summary>
        /// <param name="name">The full package or project identifier name.</param>
        /// <param name="href">The relative URL to the package details page or project.</param>
        /// <param name="summary">The description of the relationship requirement or imported version.</param>
        /// <param name="isProject">A value indicating whether the target is a project rather than a package.</param>
        /// <param name="isVerified">A value indicating whether the target publisher is verified.</param>
        public PackageRelationshipModel(
            string name,
            string href,
            string summary,
            bool isProject = false,
            bool isVerified = false)
        {
            this.Name = name;
            this.Href = href;
            this.Summary = summary;
            this.IsProject = isProject;
            this.IsVerified = isVerified;
        }

        /// <summary>
        /// Gets or sets the full package or project identifier name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the relative URL to the package details page or project.
        /// </summary>
        public string Href { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the relationship requirement or imported version.
        /// </summary>
        public string Summary { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the target is a project rather than a package.
        /// </summary>
        public bool IsProject { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the target publisher is verified.
        /// </summary>
        public bool IsVerified { get; set; }
    }
}
