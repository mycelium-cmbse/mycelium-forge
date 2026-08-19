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
    }
}
