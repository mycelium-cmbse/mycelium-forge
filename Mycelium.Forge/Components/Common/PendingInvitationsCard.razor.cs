// ------------------------------------------------------------------------------------------------
// <copyright file="PendingInvitationsCard.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Common
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extensions;
    using Mycelium.Forge.ViewModels.Invitations;

    /// <summary>
    /// Displays pending invitations with target names, assigned roles, expiration state, and optional resend/revoke controls.
    /// </summary>
    /// <typeparam name="TInvitation">The invitation domain interface type.</typeparam>
    public partial class PendingInvitationsCard<TInvitation> : DisposableComponent where TInvitation : class, IInvitation
    {
        /// <summary>
        /// Gets or sets the invitation manager view model providing pending invitations and actions.
        /// </summary>
        [Parameter]
        [EditorRequired]
        public IInvitationManagerViewModel<TInvitation> ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the selector resolving the role display text for an invitation.
        /// </summary>
        [Parameter]
        [EditorRequired]
        public Func<TInvitation, string> RoleTextSelector { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether resend and revoke action buttons are visible.
        /// </summary>
        [Parameter]
        public bool CanManage { get; set; } = true;

        /// <summary>
        /// Gets or sets the descriptive alert text displayed at the bottom of the card.
        /// </summary>
        [Parameter]
        public string AlertDescription { get; set; } = string.Empty;

        /// <summary>
        /// Gets a value indicating whether the alert description is populated.
        /// </summary>
        private bool HasAlertDescription => !string.IsNullOrWhiteSpace(this.AlertDescription);

        /// <summary>
        /// Subscribes to reactive pending invitation changes on component initialization.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            this.Disposables.Add(this.ViewModel.PendingInvitations.Connect().SubscribeAsync(_ => this.InvokeAsync(this.StateHasChanged)));
        }

        /// <summary>
        /// Gets the formatted expiration text for the specified invitation.
        /// </summary>
        /// <param name="invitation">The invitation instance.</param>
        /// <returns>The formatted expiration status text.</returns>
        private static string GetExpirationText(IInvitation invitation)
        {
            return invitation.ExperiesAt <= DateTime.UtcNow ? "Expired" : $"Expires in {invitation.ExperiesAt.ToTimeToCome()}";
        }
    }
}
