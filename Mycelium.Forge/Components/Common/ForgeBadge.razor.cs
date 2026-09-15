// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeBadge.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Common
{
    using Microsoft.AspNetCore.Components;

    /// <summary>
    /// Standardized badge component styled with design tokens and custom CSS classes.
    /// </summary>
    public partial class ForgeBadge : ComponentBase
    {
        /// <summary>
        /// Gets or sets the custom child content rendered inside the badge.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the text label displayed inside the badge when <see cref="ChildContent" /> is null.
        /// </summary>
        [Parameter]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets optional custom CSS classes applied to the badge container.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = string.Empty;
    }
}
