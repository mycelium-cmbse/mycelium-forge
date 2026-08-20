// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationSettingsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels
{
    using Mycelium.Forge.Data;
    using Mycelium.Forge.Models;

    /// <summary>
    /// Provides view model state and operations for managing organization members, invitations, and scope configuration.
    /// </summary>
    public class OrganizationSettingsViewModel : IOrganizationSettingsViewModel
    {
        /// <summary>
        /// The list of available role options for organization members.
        /// </summary>
        private static readonly List<string> AvailableRoles =
        [
            "Organization Administrator",
            "Forge Publisher",
            "Organization Member"
        ];

        /// <summary>
        /// Gets or sets the organization profile details.
        /// </summary>
        public OrganizationModel Organization { get; set; }

        /// <summary>
        /// Gets or sets the current user's role within the organization.
        /// </summary>
        public string CurrentUserRole { get; set; }

        /// <summary>
        /// Gets or sets the collection of members belonging to the organization.
        /// </summary>
        public List<OrganizationMemberModel> Members { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of pending invitations for the organization.
        /// </summary>
        public List<OrganizationInvitationModel> PendingInvitations { get; set; } = [];

        /// <summary>
        /// Gets or sets the available role options for organization members.
        /// </summary>
        public List<string> RoleOptions { get; set; } = AvailableRoles;

        /// <summary>
        /// Initializes the view model state for the specified organization identifier or scope.
        /// </summary>
        /// <param name="id">The unique identifier or slug handle of the organization.</param>
        public void InitializeViewModel(string id)
        {
            this.Organization = SeedData.StarionOrganizationModel;
            this.CurrentUserRole = "Organization Administrator";
            this.Members = [.. SeedData.StarionMembers];

            this.PendingInvitations =
            [
                new OrganizationInvitationModel("a.novak@esa.int", "Organization Administrator", "Sent 2 days ago · expires in 5 days")
            ];
        }

        /// <summary>
        /// Changes the role of the specified organization member.
        /// </summary>
        /// <param name="member">The member whose role is being updated.</param>
        /// <param name="newRole">The new role to assign to the member.</param>
        public void ChangeMemberRole(OrganizationMemberModel member, string newRole)
        {
            member?.Role = newRole;
        }

        /// <summary>
        /// Removes the specified member from the organization.
        /// </summary>
        /// <param name="member">The member to remove.</param>
        public void RemoveMember(OrganizationMemberModel member)
        {
            this.Members.Remove(member);
        }

        /// <summary>
        /// Resends the specified pending invitation.
        /// </summary>
        /// <param name="invitation">The invitation to resend.</param>
        public void ResendInvitation(OrganizationInvitationModel invitation)
        {
        }

        /// <summary>
        /// Revokes the specified pending invitation.
        /// </summary>
        /// <param name="invitation">The invitation to revoke.</param>
        public void RevokeInvitation(OrganizationInvitationModel invitation)
        {
            this.PendingInvitations.Remove(invitation);
        }

        /// <summary>
        /// Handles initiating an organization transfer.
        /// </summary>
        public void TransferOrganization()
        {
        }
    }
}
