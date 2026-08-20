// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationSettings.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.OrganizationSettings
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Models;
    using Mycelium.Forge.ViewModels;

    /// <summary>
    /// Represents the organization settings, membership administration, and scope management view of the Mycelium Forge registry.
    /// </summary>
    public partial class OrganizationSettings : ComponentBase
    {
        /// <summary>
        /// Gets or sets the organization identifier or scope slug supplied from the URL route.
        /// </summary>
        [Parameter]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the view model for the organization settings page.
        /// </summary>
        [Inject]
        public IOrganizationSettingsViewModel ViewModel { get; set; }

        /// <summary>
        /// Handles changing the assigned role of an organization member.
        /// </summary>
        /// <param name="member">The member whose role is changing.</param>
        /// <param name="newRole">The selected new role name.</param>
        public void OnChangeMemberRole(OrganizationMemberModel member, string newRole)
        {
            if (member == null || string.IsNullOrWhiteSpace(newRole))
            {
                return;
            }

            this.ViewModel.ChangeMemberRole(member, newRole);
        }

        /// <summary>
        /// Handles the action to remove a member from the organization.
        /// </summary>
        /// <param name="member">The member to remove.</param>
        public void OnRemoveMember(OrganizationMemberModel member)
        {
            if (member == null)
            {
                return;
            }

            this.ViewModel.RemoveMember(member);
        }

        /// <summary>
        /// Handles the action to resend a pending membership invitation.
        /// </summary>
        /// <param name="invitation">The invitation to resend.</param>
        public void OnResendInvitation(OrganizationInvitationModel invitation)
        {
            if (invitation == null)
            {
                return;
            }

            this.ViewModel.ResendInvitation(invitation);
        }

        /// <summary>
        /// Handles the action to revoke a pending membership invitation.
        /// </summary>
        /// <param name="invitation">The invitation to revoke.</param>
        public void OnRevokeInvitation(OrganizationInvitationModel invitation)
        {
            if (invitation == null)
            {
                return;
            }

            this.ViewModel.RevokeInvitation(invitation);
        }

        /// <summary>
        /// Handles the action to initiate an organization transfer.
        /// </summary>
        public void OnTransferOrganization()
        {
            this.ViewModel.TransferOrganization();
        }

        /// <summary>
        /// Handles component parameter updates and initializes the view model with the organization identifier.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            this.ViewModel.InitializeViewModel(this.Id);
        }
    }
}
