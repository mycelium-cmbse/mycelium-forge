// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeBreadcrumb.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Common
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Models.Common;

    /// <summary>
    /// Displays a standardized breadcrumb navigation trail using <see cref="BreadcrumbItem" /> entries.
    /// </summary>
    public partial class ForgeBreadcrumb : ComponentBase
    {
        /// <summary>
        /// Gets or sets the collection of breadcrumb navigation items to render.
        /// </summary>
        [Parameter]
        public IEnumerable<BreadcrumbItem> Items { get; set; } = [];

        /// <summary>
        /// Gets or sets optional custom CSS classes applied to the breadcrumb container.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = string.Empty;

        /// <summary>
        /// Gets the breadcrumb items materialized as a read-only list for indexed access.
        /// </summary>
        /// <returns>A read-only list containing the breadcrumb items.</returns>
        public IReadOnlyList<BreadcrumbItem> GetItemsList()
        {
            return this.Items as IReadOnlyList<BreadcrumbItem> ?? [.. this.Items];
        }
    }
}
