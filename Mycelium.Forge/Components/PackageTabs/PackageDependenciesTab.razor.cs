// ------------------------------------------------------------------------------------------------
// <copyright file="PackageDependenciesTab.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.PackageTabs
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Models;

    /// <summary>
    /// Represents the direct dependencies list tab component for package details.
    /// </summary>
    public partial class PackageDependenciesTab : ComponentBase
    {
        /// <summary>
        /// Gets or sets the collection of dependencies required by the package.
        /// </summary>
        [Parameter]
        public IReadOnlyList<PackageDependencyModel> Dependencies { get; set; } = [];

        /// <summary>
        /// Gets or sets the additional CSS class names for styling the tab container.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = string.Empty;
    }
}
