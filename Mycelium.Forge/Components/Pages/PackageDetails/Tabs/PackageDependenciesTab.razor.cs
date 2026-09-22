// ------------------------------------------------------------------------------------------------
// <copyright file="PackageDependenciesTab.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.PackageDetails.Tabs
{
    using Microsoft.AspNetCore.Components;

    /// <summary>
    /// Represents the direct dependencies list tab component for package details.
    /// </summary>
    public partial class PackageDependenciesTab : ComponentBase
    {
        /// <summary>
        /// Gets or sets the collection of dependency tuples (Name, Summary, IsVerified) required by the package.
        /// </summary>
        [Parameter]
        public IReadOnlyList<(string Name, string Summary, bool IsVerified)> Dependencies { get; set; } = [];
    }
}
