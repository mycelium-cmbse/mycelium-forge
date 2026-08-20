// ------------------------------------------------------------------------------------------------
// <copyright file="PackageElementModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents a model element definition contained within a package release.
    /// </summary>
    public class PackageElementModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageElementModel" /> class.
        /// </summary>
        public PackageElementModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageElementModel" /> class with specified properties.
        /// </summary>
        /// <param name="name">The element identifier name.</param>
        /// <param name="kind">The element definition kind tag (e.g., «part def», «port def», «attribute def»).</param>
        /// <param name="category">
        /// The category kind grouping for filtering (e.g., Parts, Attributes, Units, Scales, Types, Templates).
        /// </param>
        /// <param name="attributeSummary">The summary description of the element attributes or typing.</param>
        public PackageElementModel(
            string name,
            string kind,
            string category,
            string attributeSummary)
        {
            this.Name = name;
            this.Kind = kind;
            this.Category = category;
            this.AttributeSummary = attributeSummary;
        }

        /// <summary>
        /// Gets or sets the element identifier name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the element definition kind tag (e.g., «part def», «port def», «attribute def»).
        /// </summary>
        public string Kind { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the category kind grouping for filtering (e.g., Parts, Attributes, Units, Scales, Types, Templates).
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the summary description of the element attributes or typing.
        /// </summary>
        public string AttributeSummary { get; set; } = string.Empty;
    }
}
