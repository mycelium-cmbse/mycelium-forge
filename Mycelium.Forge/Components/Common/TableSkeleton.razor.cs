// ------------------------------------------------------------------------------------------------
// <copyright file="TableSkeleton.razor.cs" company="Starion Group S.A.">
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
    /// Represents a reusable skeleton loading placeholder component for tables.
    /// </summary>
    public partial class TableSkeleton : ComponentBase
    {
        /// <summary>
        /// Gets or sets the number of skeleton columns to display.
        /// </summary>
        [Parameter]
        public int ColumnCount { get; set; } = 4;

        /// <summary>
        /// Gets or sets the number of skeleton body rows to display.
        /// </summary>
        [Parameter]
        public int RowCount { get; set; } = 5;

        /// <summary>
        /// Gets or sets the CSS height class for header skeleton items.
        /// </summary>
        [Parameter]
        public string HeaderHeightClass { get; set; } = "h-8";

        /// <summary>
        /// Gets or sets the CSS height class for body row skeleton items.
        /// </summary>
        [Parameter]
        public string RowHeightClass { get; set; } = "h-6";

        /// <summary>
        /// Gets or sets the CSS gap class between columns.
        /// </summary>
        [Parameter]
        public string GapClass { get; set; } = "gap-4";

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
        private string ComputedClass => $"w-full space-y-3 {this.Class}".Trim();
    }
}
