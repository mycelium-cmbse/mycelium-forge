// ------------------------------------------------------------------------------------------------
// <copyright file="PackageSection.razor.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Common
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Models.Package;

    /// <summary>
    /// Displays a categorized grid of package cards with a title and action link.
    /// </summary>
    public partial class PackageSection : ComponentBase
    {
        /// <summary>
        /// Gets or sets the display title of the section.
        /// </summary>
        [Parameter]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the link destination URL for the 'View all' action.
        /// </summary>
        [Parameter]
        public string ViewAllHref { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of packages displayed within this section.
        /// </summary>
        [Parameter]
        public IReadOnlyList<PackageModel> Packages { get; set; } = [];

        /// <summary>
        /// Gets or sets optional additional CSS classes for the section container.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = string.Empty;
    }
}
