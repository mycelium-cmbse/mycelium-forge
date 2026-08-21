// ------------------------------------------------------------------------------------------------
// <copyright file="DocumentationNavItemModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.Documentation
{
    /// <summary>
    /// Represents an individual navigation item link within a documentation sidebar group.
    /// </summary>
    public class DocumentationNavItemModel
    {
        /// <summary>
        /// Gets or sets the display title for the navigation item.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the target hyperlink URL for the navigation item.
        /// </summary>
        public string Href { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this navigation item is currently active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
