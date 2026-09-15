// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeCard.razor.cs" company="Starion Group S.A.">
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
    /// Generic card container styled with standard Forge card styling.
    /// </summary>
    public partial class ForgeCard : ComponentBase
    {
        /// <summary>
        /// Gets or sets the content to be rendered inside the card.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets optional custom CSS classes applied to the card container.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets optional custom inline styles applied to the card container.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> AdditionalAttributes { get; set; } = new();
    }
}
