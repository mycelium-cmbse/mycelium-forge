// ------------------------------------------------------------------------------------------------
// <copyright file="PendingRequestsManager.razor.cs" company="Starion Group S.A.">
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
    using Mycelium.Forge.ViewModels.AccountSettings;

    /// <summary>
    /// Represents the pending organization and package invitations manager component in the account settings page.
    /// </summary>
    public partial class PendingRequestsManager : ComponentBase
    {
        /// <summary>
        /// Gets or sets the CSS class applied to the card.
        /// </summary>
        [Parameter]
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the view model for managing pending requests.
        /// </summary>
        [Parameter]
        [EditorRequired]
        public IPendingRequestsManagerViewModel ViewModel { get; set; }

        /// <summary>
        /// Handles accepting a pending organization or package invitation asynchronously.
        /// </summary>
        /// <param name="invitation">The pending invitation to accept.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnAcceptInvitation(IInvitation invitation)
        {
            await this.ViewModel.AcceptInvitation(invitation);
            await this.InvokeAsync(this.StateHasChanged);
        }

        /// <summary>
        /// Handles rejecting a pending organization or package invitation asynchronously.
        /// </summary>
        /// <param name="invitation">The pending invitation to reject.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnRejectInvitation(IInvitation invitation)
        {
            await this.ViewModel.RejectInvitation(invitation);
            await this.InvokeAsync(this.StateHasChanged);
        }
    }
}
