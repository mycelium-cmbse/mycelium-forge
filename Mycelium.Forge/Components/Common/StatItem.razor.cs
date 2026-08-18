// ------------------------------------------------------------------------------------------------
// <copyright file="StatItem.razor.cs" company="Starion Group S.A.">
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
    /// Displays a key numeric metric with its corresponding descriptive label.
    /// </summary>
    public partial class StatItem : ComponentBase
    {
        /// <summary>
        /// Gets or sets the numeric value or metric string to display.
        /// </summary>
        [Parameter]
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the descriptive label displayed below the value.
        /// </summary>
        [Parameter]
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets optional additional CSS classes to apply to the container.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = string.Empty;
    }
}
