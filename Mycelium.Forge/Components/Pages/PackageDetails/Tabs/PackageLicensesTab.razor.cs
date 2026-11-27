// ------------------------------------------------------------------------------------------------
// <copyright file="PackageLicensesTab.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.PackageDetails.Tabs
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Represents the licenses tab component displaying applicable SPDX licenses and full legal texts for package details.
    /// </summary>
    public partial class PackageLicensesTab : ComponentBase
    {
        /// <summary>
        /// Gets or sets the collection of applicable license DTOs for the current package version.
        /// </summary>
        [Parameter]
        public IReadOnlyList<ILicense> Licenses { get; set; } = [];
    }
}
