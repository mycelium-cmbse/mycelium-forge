// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeAlert.razor.cs" company="Starion Group S.A.">
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
    /// Represents an alert callout banner displaying prominent contextual messages, notices, and feedback.
    /// </summary>
    public partial class ForgeAlert : ComponentBase
    {
        /// <summary>
        /// Gets or sets the visual style variant controlling the color scheme of the alert.
        /// </summary>
        [Parameter]
        public ForgeAlertVariant Variant { get; set; } = ForgeAlertVariant.Default;

        /// <summary>
        /// Gets or sets the heading text for the alert.
        /// </summary>
        [Parameter]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets optional custom markup for the alert title.
        /// </summary>
        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        /// <summary>
        /// Gets or sets the description or body text of the alert.
        /// </summary>
        [Parameter]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the main body content of the alert.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the optional leading icon rendered in the alert.
        /// </summary>
        [Parameter]
        public RenderFragment Icon { get; set; }

        /// <summary>
        /// Gets or sets the optional action buttons rendered in the alert actions slot.
        /// </summary>
        [Parameter]
        public RenderFragment Actions { get; set; }

        /// <summary>
        /// Gets or sets optional additional CSS classes for the alert container.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = string.Empty;

        /// <summary>
        /// Computes the CSS classes corresponding to the current alert variant.
        /// </summary>
        /// <returns>A string containing CSS utility classes for the variant background, border, and text.</returns>
        private string GetVariantClass()
        {
            return this.Variant switch
            {
                ForgeAlertVariant.Info => "bg-info border-info-border text-info-foreground",
                ForgeAlertVariant.Success => "bg-success border-success-border text-success-foreground",
                ForgeAlertVariant.Warning => "bg-warning border-warning-border text-warning-foreground",
                ForgeAlertVariant.Danger => "bg-destructive/10 border-destructive/20 text-destructive",
                _ => "bg-body-bg border-border text-foreground"
            };
        }
    }
}
