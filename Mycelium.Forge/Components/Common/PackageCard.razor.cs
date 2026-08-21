// ------------------------------------------------------------------------------------------------
// <copyright file="PackageCard.razor.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Common
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Models.Package;

    /// <summary>
    /// Displays summary information for a published package library card.
    /// </summary>
    public partial class PackageCard : ComponentBase
    {
        /// <summary>
        /// Gets or sets the package model data to display.
        /// </summary>
        [Parameter]
        public PackageModel Model { get; set; }

        /// <summary>
        /// Gets or sets optional additional CSS classes to apply to the card.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = string.Empty;
    }
}
