// ------------------------------------------------------------------------------------------------
// <copyright file="DocumentationSectionCardModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.Documentation
{
    /// <summary>
    /// Represents a key section card on the documentation overview and index page.
    /// </summary>
    public class DocumentationSectionCardModel
    {
        /// <summary>
        /// Gets or sets the Lucide icon name for the section card.
        /// </summary>
        public string IconName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the title of the section.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description text explaining what this section covers.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the destination URL when the card is clicked.
        /// </summary>
        public string Href { get; set; } = string.Empty;
    }
}
