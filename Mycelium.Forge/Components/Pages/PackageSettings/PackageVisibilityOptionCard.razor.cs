// ------------------------------------------------------------------------------------------------
// <copyright file="PackageVisibilityOptionCard.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.PackageSettings
{
    using Microsoft.AspNetCore.Components;

    /// <summary>
    /// Represents an option card displaying package visibility choices with selection state and role badges.
    /// </summary>
    public partial class PackageVisibilityOptionCard : ComponentBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether this option is the currently active visibility setting.
        /// </summary>
        [Parameter]
        public bool IsCurrent { get; set; }

        /// <summary>
        /// Gets or sets the display title for the visibility option.
        /// </summary>
        [Parameter]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the explanatory description for the visibility option.
        /// </summary>
        [Parameter]
        public string Description { get; set; } = string.Empty;
    }
}
