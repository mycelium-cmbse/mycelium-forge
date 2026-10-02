// ------------------------------------------------------------------------------------------------
// <copyright file="BadgeSkeleton.razor.cs" company="Starion Group S.A.">
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
    /// Represents a reusable skeleton loading placeholder component for badges and filter chips.
    /// </summary>
    public partial class BadgeSkeleton : ComponentBase
    {
        /// <summary>
        /// Gets or sets the number of skeleton badges to display.
        /// </summary>
        [Parameter]
        public int Count { get; set; } = 3;

        /// <summary>
        /// Gets or sets the CSS height class for each badge skeleton item.
        /// </summary>
        [Parameter]
        public string HeightClass { get; set; } = "h-6";

        /// <summary>
        /// Gets or sets the CSS width class for each badge skeleton item.
        /// </summary>
        [Parameter]
        public string WidthClass { get; set; } = "w-16";

        /// <summary>
        /// Gets or sets the CSS shape or border radius class for each badge skeleton item.
        /// </summary>
        [Parameter]
        public string ShapeClass { get; set; } = "rounded-md";

        /// <summary>
        /// Gets or sets the CSS gap class between badge skeleton items.
        /// </summary>
        [Parameter]
        public string GapClass { get; set; } = "gap-2";

        /// <summary>
        /// Gets or sets optional custom CSS classes applied to the root container.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets optional custom attributes applied to the root container.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> AdditionalAttributes { get; set; } = new();

        /// <summary>
        /// Gets the computed CSS class string for the root container element.
        /// </summary>
        private string ComputedClass => $"flex flex-row items-center {this.GapClass} {this.Class}".Trim();
    }
}
