// ------------------------------------------------------------------------------------------------
// <copyright file="DocumentationToc.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.Documentation
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;

    using Mycelium.Forge.Models.Documentation;

    /// <summary>
    /// Code-behind logic for the on-page table of contents sidebar component.
    /// </summary>
    public partial class DocumentationToc : ComponentBase
    {
        /// <summary>
        /// The base CSS class applied to all table of contents items.
        /// </summary>
        private const string BaseTocItemClass = "text-sm leading-xs transition-colors";

        /// <summary>
        /// Gets or sets the JavaScript runtime service.
        /// </summary>
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        /// <summary>
        /// Gets or sets the collection of table of contents entries to display.
        /// </summary>
        [Parameter]
        public List<DocumentationTocItemModel> Items { get; set; } = [];

        /// <summary>
        /// Computes the CSS classes applied to an on-page table of contents link item.
        /// </summary>
        /// <param name="item">The table of contents item model.</param>
        /// <returns>The combined CSS class string.</returns>
        public string GetTocItemClass(DocumentationTocItemModel item)
        {
            if (item == null)
            {
                return string.Empty;
            }

            return item.IsActive
                ? $"{BaseTocItemClass} font-medium text-primary hover:underline"
                : $"{BaseTocItemClass} font-normal text-muted-foreground hover:text-foreground";
        }

        /// <summary>
        /// Handles clicking a table of contents item to smoothly scroll to the target section.
        /// </summary>
        /// <param name="item">The table of contents item to scroll to.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnItemClick(DocumentationTocItemModel item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.TargetId))
            {
                return;
            }

            foreach (var tocItem in this.Items)
            {
                tocItem.IsActive = tocItem == item;
            }

            await this.JSRuntime.InvokeVoidAsync("forgeInterop.scrollToElement", item.TargetId);
        }
    }
}
