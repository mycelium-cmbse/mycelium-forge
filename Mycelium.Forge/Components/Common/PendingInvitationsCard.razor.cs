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

    /// <summary>
    /// Displays pending invitations with target names, assigned roles, expiration state, and optional resend/revoke controls.
    /// </summary>
    /// <typeparam name="TInvitation">The concrete invitation type.</typeparam>
    public partial class PendingInvitationsCard<TInvitation> : ComponentBase where TInvitation : IInvitation
    {
        /// <summary>
        /// Gets or sets the pending invitations to display.
        /// </summary>
        [Parameter]
        public IReadOnlyList<TInvitation> Invitations { get; set; } = [];

        /// <summary>
        /// Gets or sets the selector resolving the display short name for an invitation's target account.
        /// </summary>
        [Parameter]
        [EditorRequired]
        public Func<TInvitation, string> TargetNameSelector { get; set; }

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
        /// Gets or sets the CSS class applied to each resend button.
        /// </summary>
        [Parameter]
        public string ResendButtonClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the CSS class applied to each revoke button.
        /// </summary>
        [Parameter]
        public string RevokeButtonClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the event callback invoked when the resend button is clicked.
        /// </summary>
        [Parameter]
        public EventCallback<TInvitation> OnResend { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the revoke button is clicked.
        /// </summary>
        [Parameter]
        public EventCallback<TInvitation> OnRevoke { get; set; }

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
