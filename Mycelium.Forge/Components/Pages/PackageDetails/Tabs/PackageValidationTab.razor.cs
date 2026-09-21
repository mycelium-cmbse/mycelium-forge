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
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extensions;

    /// <summary>
    /// Represents the validation report and quality checks tab component for package details.
    /// </summary>
    public partial class PackageValidationTab : ComponentBase
    {
        /// <summary>
        /// Gets or sets the collection of quality validation check tuples (Title, Detail, Passed).
        /// </summary>
        [Parameter]
        public IReadOnlyList<(string Title, string Detail, bool Passed)> Checks { get; set; } = [];

        /// <summary>
        /// Gets or sets the currently selected package version DTO.
        /// </summary>
        [Parameter]
        public IPackageVersion Version { get; set; }
    }
}
