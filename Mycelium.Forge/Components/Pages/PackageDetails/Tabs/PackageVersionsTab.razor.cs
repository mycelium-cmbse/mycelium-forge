// ------------------------------------------------------------------------------------------------
// <copyright file="PackageVersionsTab.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.PackageDetails.Tabs
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Models.Package;

    /// <summary>
    /// Represents the release versions history tab component for package details.
    /// </summary>
    public partial class PackageVersionsTab : ComponentBase
    {
        /// <summary>
        /// Gets or sets the collection of released versions for the package.
        /// </summary>
        [Parameter]
        public IReadOnlyList<PackageVersionModel> Versions { get; set; } = [];
    }
}
