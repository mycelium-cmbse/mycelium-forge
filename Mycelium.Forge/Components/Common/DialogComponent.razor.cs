// ------------------------------------------------------------------------------------------------
// <copyright file="DialogComponent.razor.cs" company="Starion Group S.A.">
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
    /// Represents a standardized wrapper layout container for modal dialog content and footer actions.
    /// </summary>
    public partial class DialogComponent : ComponentBase
    {
        /// <summary>
        /// Gets or sets the main body content rendered inside the dialog container.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the optional footer content rendered at the bottom of the dialog container.
        /// </summary>
        [Parameter]
        public RenderFragment Footer { get; set; }

        /// <summary>
        /// Gets or sets optional CSS classes to override the dialog container layout.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets optional CSS classes to override the dialog footer layout.
        /// </summary>
        [Parameter]
        public string FooterClass { get; set; } = string.Empty;

        /// <summary>
        /// Computes the CSS classes applied to the outer dialog container element.
        /// </summary>
        /// <returns>A string containing CSS utility classes for the dialog container.</returns>
        public string GetContainerClass()
        {
            return string.IsNullOrWhiteSpace(this.Class)
                ? "flex flex-col gap-4 px-6 py-3"
                : this.Class;
        }

        /// <summary>
        /// Computes the CSS classes applied to the dialog footer element.
        /// </summary>
        /// <returns>A string containing CSS utility classes for the dialog footer.</returns>
        public string GetFooterClass()
        {
            return string.IsNullOrWhiteSpace(this.FooterClass)
                ? "pt-2 flex items-center justify-end gap-2.5"
                : this.FooterClass;
        }
    }
}
