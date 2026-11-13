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
        /// Gets or sets a value indicating whether changing the collaborator's role is disabled.
        /// </summary>
        [Parameter]
        public bool IsRoleChangeDisabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether removing the collaborator is disabled.
        /// </summary>
        [Parameter]
        public bool IsRemoveDisabled { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the change role menu option is clicked.
        /// </summary>
        [Parameter]
        public EventCallback<IAccount> OnChangeRole { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the view profile menu option is clicked.
        /// </summary>
        [Parameter]
        public EventCallback<IAccount> OnViewProfile { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the remove maintainer menu option is clicked.
        /// </summary>
        [Parameter]
        public EventCallback<IAccount> OnRemove { get; set; }
    }
}
