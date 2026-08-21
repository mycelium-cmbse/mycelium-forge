// ------------------------------------------------------------------------------------------------
// <copyright file="DocumentationSidebar.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.Documentation
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Models.Documentation;

    /// <summary>
    /// Code-behind logic for the reusable documentation sidebar navigation panel.
    /// </summary>
    public partial class DocumentationSidebar : ComponentBase
    {
        /// <summary>
        /// The base CSS class applied to all sidebar navigation items.
        /// </summary>
        private const string BaseNavItemClass = "flex items-center px-3 h-7 rounded-md text-sm leading-xs transition-colors";

        /// <summary>
        /// Gets or sets the collection of documentation navigation groups to render.
        /// </summary>
        [Parameter]
        public List<DocumentationNavGroupModel> Groups { get; set; } = [];

        /// <summary>
        /// Computes the CSS classes applied to a sidebar documentation navigation link item.
        /// </summary>
        /// <param name="item">The documentation navigation item model.</param>
        /// <returns>The combined CSS class string.</returns>
        public string GetNavItemClass(DocumentationNavItemModel item)
        {
            if (item == null)
            {
                return string.Empty;
            }

            return item.IsActive
                ? $"{BaseNavItemClass} bg-primary-subtle text-primary font-medium"
                : $"{BaseNavItemClass} text-secondary-text hover:text-foreground hover:bg-muted font-normal";
        }
    }
}
