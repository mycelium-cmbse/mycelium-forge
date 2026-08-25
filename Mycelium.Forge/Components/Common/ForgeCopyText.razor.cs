// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeCopyText.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Common
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Services;

    /// <summary>
    /// Represents a styled text snippet container with built-in clipboard copying capabilities.
    /// </summary>
    public partial class ForgeCopyText : ComponentBase
    {
        /// <summary>
        /// Gets or sets the JavaScript interop service for clipboard interactions.
        /// </summary>
        [Inject]
        public IJsInterop JsInterop { get; set; }

        /// <summary>
        /// Gets or sets the text content to display and copy.
        /// </summary>
        [Parameter]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether to render using the dark code surface variant.
        /// </summary>
        [Parameter]
        public bool Dark { get; set; }

        /// <summary>
        /// Gets or sets optional custom CSS classes applied to the outer container.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets optional custom CSS classes applied to the text element.
        /// </summary>
        [Parameter]
        public string TextClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accessibility label and tooltip for the copy button.
        /// </summary>
        [Parameter]
        public string AriaLabel { get; set; } = "Copy to clipboard";

        /// <summary>
        /// Gets or sets an optional event callback invoked when text is copied.
        /// </summary>
        [Parameter]
        public EventCallback<bool> OnCopied { get; set; }

        /// <summary>
        /// Computes the CSS classes for the outer container based on the selected variant.
        /// </summary>
        /// <returns>The combined CSS class string.</returns>
        private string GetContainerClass()
        {
            const string baseClass = " px-3 py-2";

            return this.Dark
                ? $"{baseClass} bg-code-bg text-code-import"
                : $"{baseClass} bg-card border border-border text-foreground";
        }

        /// <summary>
        /// Computes the CSS classes for the text element based on the selected variant.
        /// </summary>
        /// <returns>The combined CSS class string.</returns>
        private string GetTextClass()
        {
            return this.Dark
                ? "text-code-import"
                : "text-foreground";
        }
    }
}
