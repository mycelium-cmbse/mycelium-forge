// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeCopyButton.razor.cs" company="Starion Group S.A.">
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
    /// Represents a reusable button component that writes text to the clipboard with visual confirmation feedback.
    /// </summary>
    public partial class ForgeCopyButton : ComponentBase
    {
        /// <summary>
        /// Gets or sets the JavaScript interop service for clipboard access.
        /// </summary>
        [Inject]
        public IJsInterop JsInterop { get; set; }

        /// <summary>
        /// Gets or sets the text content to be written to the clipboard when clicked.
        /// </summary>
        [Parameter]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the text label displayed next to the copy icon. When empty, renders icon-only.
        /// </summary>
        [Parameter]
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether to display a dropdown chevron icon on the right.
        /// </summary>
        [Parameter]
        public bool ShowDropdownChevron { get; set; }

        /// <summary>
        /// Gets or sets the size of the icon glyph in pixels.
        /// </summary>
        [Parameter]
        public int IconSize { get; set; } = 15;

        /// <summary>
        /// Gets or sets custom CSS classes applied to the icon element.
        /// </summary>
        [Parameter]
        public string IconClass { get; set; } = "text-foreground shrink-0";

        /// <summary>
        /// Gets or sets the accessibility label for screen readers.
        /// </summary>
        [Parameter]
        public string AriaLabel { get; set; } = "Copy to clipboard";

        /// <summary>
        /// Gets or sets optional custom CSS classes applied to the button element.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets an event callback invoked after copying to clipboard with a success flag.
        /// </summary>
        [Parameter]
        public EventCallback<bool> OnCopied { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether text was recently copied.
        /// </summary>
        public bool IsCopied { get; set; }

        /// <summary>
        /// Copies the specified text to the user clipboard and updates visual state.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous copy operation.</returns>
        public async Task CopyTextToClipboard()
        {
            if (string.IsNullOrWhiteSpace(this.Text))
            {
                return;
            }

            var success = await this.JsInterop.CopyToClipboard(this.Text);
            this.IsCopied = success;
            await this.OnCopied.InvokeAsync(success);
        }

        /// <summary>
        /// Computes the complete CSS class string for the button element.
        /// </summary>
        /// <returns>The combined CSS class string.</returns>
        private string GetButtonClass()
        {
            if (!string.IsNullOrWhiteSpace(this.Class))
            {
                return this.Class;
            }

            if (string.IsNullOrWhiteSpace(this.Label))
            {
                return "p-1 rounded text-muted-foreground hover:text-foreground transition-colors cursor-pointer focus:outline-none shrink-0";
            }

            return "flex items-center gap-1.5 px-3 py-1.5 h-8 rounded-lg bg-card border border-border text-sm leading-xs font-medium text-foreground hover:bg-muted transition-colors cursor-pointer shrink-0";
        }
    }
}
