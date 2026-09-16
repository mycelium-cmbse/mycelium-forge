// ------------------------------------------------------------------------------------------------
// <copyright file="AccountMenu.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Common
{
    using Microsoft.AspNetCore.Components;

    /// <summary>
    /// Represents the user account dropdown menu triggered from the header avatar.
    /// </summary>
    public partial class AccountMenu : ComponentBase
    {
        /// <summary>
        /// Gets or sets the display name of the logged-in user.
        /// </summary>
        public string Name { get; set; } = "Régis André";

        /// <summary>
        /// Gets or sets the username handle of the logged-in user.
        /// </summary>
        public string Handle { get; set; } = "randre";

        /// <summary>
        /// Gets or sets the primary organization identifier for navigation.
        /// </summary>
        public string Organization { get; set; } = "starion";
    }
}
