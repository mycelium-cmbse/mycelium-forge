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

    using Mycelium.Forge.Models.Package;

    /// <summary>
    /// Represents the direct dependencies list tab component for package details.
    /// </summary>
    public partial class PackageDependenciesTab : ComponentBase
    {
        /// <summary>
        /// Gets or sets the collection of dependencies required by the package.
        /// </summary>
        [Parameter]
        public IReadOnlyList<PackageRelationshipModel> Dependencies { get; set; } = [];
    }
}
