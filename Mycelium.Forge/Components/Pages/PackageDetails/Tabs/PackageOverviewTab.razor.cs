// ------------------------------------------------------------------------------------------------
// <copyright file="PackageOverviewTab.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.PackageDetails.Tabs
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Models;

    /// <summary>
    /// Represents the overview and usage README tab component for package details.
    /// </summary>
    public partial class PackageOverviewTab : ComponentBase
    {
        /// <summary>
        /// Gets or sets the package details model data.
        /// </summary>
        [Parameter]
        public PackageDetailsModel Model { get; set; }

        /// <summary>
        /// Gets or sets the additional CSS class names for styling the tab container.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = string.Empty;
    }
}
