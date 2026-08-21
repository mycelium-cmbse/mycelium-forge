// ------------------------------------------------------------------------------------------------
// <copyright file="PackageValidationTab.razor.cs" company="Starion Group S.A.">
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
    /// Represents the automated validation test results tab component for package details.
    /// </summary>
    public partial class PackageValidationTab : ComponentBase
    {
        /// <summary>
        /// Gets or sets the automated validation report model for the package release.
        /// </summary>
        [Parameter]
        public PackageValidationReportModel Report { get; set; }
    }
}
