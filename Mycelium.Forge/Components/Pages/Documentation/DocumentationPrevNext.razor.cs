// ------------------------------------------------------------------------------------------------
// <copyright file="DocumentationPrevNext.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.Documentation
{
    using Microsoft.AspNetCore.Components;

    /// <summary>
    /// Code-behind logic for the previous and next topic navigation component.
    /// </summary>
    public partial class DocumentationPrevNext : ComponentBase
    {
        /// <summary>
        /// Gets or sets the display title for the previous topic navigation item.
        /// </summary>
        [Parameter]
        public string PrevTitle { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the destination URL for the previous topic navigation item.
        /// </summary>
        [Parameter]
        public string PrevHref { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the display title for the next topic navigation item.
        /// </summary>
        [Parameter]
        public string NextTitle { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the destination URL for the next topic navigation item.
        /// </summary>
        [Parameter]
        public string NextHref { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets optional custom CSS classes applied to the container.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = string.Empty;
    }
}
