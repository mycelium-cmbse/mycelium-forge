// ------------------------------------------------------------------------------------------------
// <copyright file="DocumentationTocItemModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.Documentation
{
    /// <summary>
    /// Represents an individual heading link within the on-page table of contents sidebar.
    /// </summary>
    public class DocumentationTocItemModel
    {
        /// <summary>
        /// Gets or sets the display text for the table of contents link.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the HTML element fragment identifier (e.g. what-is-forge).
        /// </summary>
        public string TargetId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the destination hyperlink anchor URL (e.g. #what-is-forge).
        /// </summary>
        public string Href { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this section is currently active in the viewport.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
