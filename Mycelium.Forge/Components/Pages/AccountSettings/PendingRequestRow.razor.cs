// ------------------------------------------------------------------------------------------------
// <copyright file="PendingRequestRow.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.AccountSettings
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.ViewModels.Rows;

    /// <summary>
    /// Displays a single pending organization or package invitation request row with accept and reject actions.
    /// </summary>
    public partial class PendingRequestRow : ComponentBase
    {
        /// <summary>
        /// Gets or sets the row view model describing the pending invitation.
        /// </summary>
        [Parameter]
        [EditorRequired]
        public IInvitationRowViewModel Row { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the accept button is clicked.
        /// </summary>
        [Parameter]
        public EventCallback<IInvitation> OnAccept { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the reject button is clicked.
        /// </summary>
        [Parameter]
        public EventCallback<IInvitation> OnReject { get; set; }
    }
}
