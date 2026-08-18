// ------------------------------------------------------------------------------------------------
// <copyright file="Home.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages
{
    using Microsoft.AspNetCore.Components;

    /// <summary>
    /// Represents the home landing page of the Mycelium Forge registry.
    /// </summary>
    public partial class Home : ComponentBase
    {
        /// <summary>
        /// Gets or sets the total published package count displayed in the hero section.
        /// </summary>
        public string PackageCount { get; set; } = "42";

        /// <summary>
        /// Gets or sets the total package version count displayed in the hero section.
        /// </summary>
        public string VersionCount { get; set; } = "128";

        /// <summary>
        /// Gets or sets the total registered publisher count displayed in the hero section.
        /// </summary>
        public string PublisherCount { get; set; } = "6";

        /// <summary>
        /// Gets or sets the total package import count displayed in the hero section.
        /// </summary>
        public string ImportCount { get; set; } = "2,582";
    }
}
