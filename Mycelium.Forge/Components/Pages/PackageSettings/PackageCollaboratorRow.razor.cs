// ------------------------------------------------------------------------------------------------
// <copyright file="PackageCollaboratorRow.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.PackageSettings
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Displays a package collaborator item with avatar, verification status, role, and action options.
    /// </summary>
    public partial class PackageCollaboratorRow : ComponentBase
    {
        /// <summary>
        /// Gets or sets the collaborator account details.
        /// </summary>
        [Parameter]
        public IAccount Account { get; set; }

        /// <summary>
        /// Gets or sets the role label displayed for the collaborator (e.g., Owner, Maintainer).
        /// </summary>
        [Parameter]
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the event callback invoked when the options menu button is clicked.
        /// </summary>
        [Parameter]
        public EventCallback<IAccount> OnMenu { get; set; }
    }
}
