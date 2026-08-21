// ------------------------------------------------------------------------------------------------
// <copyright file="SearchInput.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Common
{
    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Represents a search input form bar for query input and discovery filtering.
    /// </summary>
    public partial class SearchInput : ComponentBase
    {
        /// <summary>
        /// Gets or sets the name of the search query query-string parameter.
        /// </summary>
        [Parameter]
        public string Name { get; set; } = UrlParameterNames.Query;

        /// <summary>
        /// Gets or sets the search input placeholder text.
        /// </summary>
        [Parameter]
        public string Placeholder { get; set; } = "Search models, libraries, units, quantity kinds...";

        /// <summary>
        /// Gets or sets the bound search query value.
        /// </summary>
        [Parameter]
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the event callback invoked when the search value changes.
        /// </summary>
        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a keyboard shortcut badge is displayed.
        /// </summary>
        [Parameter]
        public bool ShowShortcut { get; set; }

        /// <summary>
        /// Gets or sets additional CSS styling classes applied to the search form wrapper.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the search icon size in pixels.
        /// </summary>
        [Parameter]
        public int IconSize { get; set; } = 18;

        /// <summary>
        /// Gets or sets additional CSS classes applied to the inner input element.
        /// </summary>
        [Parameter]
        public string InputClass { get; set; } = "w-full bg-transparent text-md leading-sm text-foreground placeholder:text-muted-foreground border-0 shadow-none focus-visible:ring-0 focus:outline-none p-0 h-auto";

        /// <summary>
        /// Gets or sets the input update timing strategy.
        /// </summary>
        [Parameter]
        public UpdateTiming UpdateTiming { get; set; } = UpdateTiming.Immediate;

        /// <summary>
        /// Gets or sets additional CSS classes applied to the keyboard shortcut badge.
        /// </summary>
        [Parameter]
        public string KbdClass { get; set; } = "hidden sm:inline-flex items-center px-1.5 py-0.5 text-2xs leading-3xs font-medium text-muted-foreground bg-muted border border-border rounded shrink-0";
    }
}
