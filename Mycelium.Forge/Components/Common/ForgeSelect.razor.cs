// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeSelect.razor.cs" company="Starion Group S.A.">
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
    /// Represents a styled select dropdown component supporting regular form and filter variations.
    /// </summary>
    /// <typeparam name="TValue">The type of the select value.</typeparam>
    public partial class ForgeSelect<TValue> : ComponentBase
    {
        /// <summary>
        /// Gets or sets the currently selected value.
        /// </summary>
        [Parameter]
        public TValue Value { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when the selected value changes.
        /// </summary>
        [Parameter]
        public EventCallback<TValue> ValueChanged { get; set; }

        /// <summary>
        /// Gets or sets the collection of available select options.
        /// </summary>
        [Parameter]
        public IReadOnlyList<TValue> Items { get; set; } = [];

        /// <summary>
        /// Gets or sets a value indicating whether filter styling should be applied.
        /// </summary>
        [Parameter]
        public bool FilterStyle { get; set; }

        /// <summary>
        /// Gets or sets the optional prefix label displayed before the selected value in filter mode.
        /// </summary>
        [Parameter]
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier applied to the trigger element.
        /// </summary>
        [Parameter]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional CSS classes applied to the select trigger element.
        /// </summary>
        [Parameter]
        public string TriggerClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional CSS classes applied to the select items.
        /// </summary>
        [Parameter]
        public string ItemClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional CSS classes applied to the dropdown content container.
        /// </summary>
        [Parameter]
        public string ContentClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a function to convert an item value into a display string representation.
        /// </summary>
        [Parameter]
        public Func<TValue, string> DisplayTextSelector { get; set; }

        /// <summary>
        /// Gets or sets an optional custom template for the select trigger content.
        /// </summary>
        [Parameter]
        public RenderFragment TriggerContent { get; set; }

        /// <summary>
        /// Gets or sets an optional custom template for rendering each select item.
        /// </summary>
        [Parameter]
        public RenderFragment<TValue> ItemTemplate { get; set; }

        /// <summary>
        /// Handles changes to the selected value and invokes the <see cref="ValueChanged" /> event callback.
        /// </summary>
        /// <param name="newValue">The newly selected value.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnValueChanged(TValue newValue)
        {
            this.Value = newValue;
            await this.ValueChanged.InvokeAsync(newValue);
        }

        /// <summary>
        /// Computes the display string representation for the specified item value.
        /// </summary>
        /// <param name="item">The item value.</param>
        /// <returns>The human-readable string representation.</returns>
        public string GetDisplayText(TValue item)
        {
            if (this.DisplayTextSelector != null)
            {
                return this.DisplayTextSelector(item);
            }

            return item?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Computes the CSS class string for the select trigger button.
        /// </summary>
        /// <returns>The combined CSS class string.</returns>
        public string GetTriggerClass()
        {
            const string commonTriggerClass = "px-3 bg-card border border-border flex items-center text-sm leading-xs cursor-pointer focus:outline-none";

            var styleClass = this.FilterStyle
                ? "h-8 rounded-lg hover:border-input gap-1.5 font-normal transition-colors"
                : "h-10 w-full rounded-md justify-between gap-2 text-foreground";

            var baseClass = $"{commonTriggerClass} {styleClass}";

            return string.IsNullOrWhiteSpace(this.TriggerClass)
                ? baseClass
                : $"{baseClass} {this.TriggerClass}";
        }

        /// <summary>
        /// Computes the CSS class string for the select dropdown content container.
        /// </summary>
        /// <returns>The combined CSS class string.</returns>
        public string GetContentClass()
        {
            const string commonContentClass = "py-1 rounded-lg bg-popover border border-border shadow-md z-20 flex flex-col";
            var widthClass = this.FilterStyle ? "w-40" : "w-full";
            var baseClass = $"{widthClass} {commonContentClass}";

            return string.IsNullOrWhiteSpace(this.ContentClass)
                ? baseClass
                : $"{baseClass} {this.ContentClass}";
        }

        /// <summary>
        /// Computes the CSS class string for an individual select item.
        /// </summary>
        /// <param name="item">The select item value.</param>
        /// <returns>The combined CSS class string.</returns>
        public string GetItemClass(TValue item)
        {
            var isSelected = EqualityComparer<TValue>.Default.Equals(item, this.Value);

            const string filterItemBase = "px-3 py-1.5 text-left text-sm hover:bg-muted transition-colors cursor-pointer";
            string baseClass;

            if (this.FilterStyle)
            {
                baseClass = isSelected
                    ? $"{filterItemBase} font-semibold text-primary data-[focused=true]:!text-primary"
                    : $"{filterItemBase} font-normal text-foreground";
            }
            else
            {
                baseClass = $"{filterItemBase} text-foreground";
            }

            return string.IsNullOrWhiteSpace(this.ItemClass)
                ? baseClass
                : $"{baseClass} {this.ItemClass}";
        }
    }
}
