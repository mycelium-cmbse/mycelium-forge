// ------------------------------------------------------------------------------------------------
// <copyright file="Footer.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Layout
{
    using Microsoft.AspNetCore.Components;

    /// <summary>
    /// Represents the global application footer for Mycelium Forge.
    /// </summary>
    public partial class Footer : ComponentBase
    {
        /// <summary>
        /// Gets or sets the copyright legal text displayed in the bottom footer bar.
        /// </summary>
        public string CopyrightText { get; set; } = "© 2026 Starion Group · Mycelium Forge · SysML v2 (OMG formal/25-09-03) · ECSS-E-TM-10-25 · Capella";
    }
}
