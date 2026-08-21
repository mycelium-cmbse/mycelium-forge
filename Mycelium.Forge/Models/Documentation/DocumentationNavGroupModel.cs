// ------------------------------------------------------------------------------------------------
// <copyright file="DocumentationNavGroupModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.Documentation
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a grouped section of documentation navigation links displayed in the sidebar.
    /// </summary>
    public class DocumentationNavGroupModel
    {
        /// <summary>
        /// Gets or sets the group header title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Lucide icon name for the group header.
        /// </summary>
        public string IconName { get; set; } = "chevron-right";

        /// <summary>
        /// Gets or sets the list of navigation items within this group.
        /// </summary>
        public List<DocumentationNavItemModel> Items { get; set; } = [];
    }
}
