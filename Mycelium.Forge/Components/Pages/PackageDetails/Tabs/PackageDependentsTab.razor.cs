// ------------------------------------------------------------------------------------------------
// <copyright file="PackageDependentsTab.razor.cs" company="Starion Group S.A.">
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
    /// Represents the reverse dependents and consumers list tab component for package details.
    /// </summary>
    public partial class PackageDependentsTab : ComponentBase
    {
        /// <summary>
        /// Gets or sets the collection of dependent package and project tuples (Name, Summary, IsProject, IsVerified).
        /// </summary>
        [Parameter]
        public IReadOnlyList<(string Name, string Summary, bool IsProject, bool IsVerified)> Dependents { get; set; } = [];
    }
}
