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
    using Mycelium.Forge.Models;

    /// <summary>
    /// Provides view model state and operations for managing organization members, invitations, and scope configuration.
    /// </summary>
    public class OrganizationSettingsViewModel : IOrganizationSettingsViewModel
    {
        /// <summary>
        /// The list of available role options for organization members.
        /// </summary>
        private static readonly IReadOnlyList<string> AvailableRoles =
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
        public IReadOnlyList<OrganizationMemberModel> Members { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of pending invitations for the organization.
        /// </summary>
        public IReadOnlyList<OrganizationInvitationModel> PendingInvitations { get; set; } = [];

        /// <summary>
        /// Gets or sets the available role options for organization members.
        /// </summary>
        public IReadOnlyList<string> RoleOptions { get; set; } = AvailableRoles;

        /// <summary>
        /// Initializes the view model state for the specified organization identifier or scope.
        /// </summary>
        /// <param name="id">The unique identifier or slug handle of the organization.</param>
        public void InitializeViewModel(string id)
        {
            var scope = string.IsNullOrWhiteSpace(id) ? "@starion" : id.StartsWith('@') ? id : $"@{id}";

            this.Organization = new OrganizationModel(
                "Starion Group",
                "SG",
                scope,
                "the slug @starion is reserved as this organization’s package scope.",
                true,
                6,
                14,
                390,
                2025);

            this.CurrentUserRole = "Organization Administrator";

            this.Members =
            [
                new OrganizationMemberModel("R. André", "@r.andre", "RA", "Organization Administrator"),
                new OrganizationMemberModel("S. Kramer", "@s.kramer", "SK", "Organization Administrator"),
                new OrganizationMemberModel("J. Klein", "@j.klein", "JK", "Forge Publisher"),
                new OrganizationMemberModel("M. Blanc", "@m.blanc", "MB", "Organization Member")
            ];

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
            this.Members = this.Members
                .Select(m => m == member ? m with { Role = newRole } : m)
                .ToList();
        }

        /// <summary>
        /// Removes the specified member from the organization.
        /// </summary>
        /// <param name="member">The member to remove.</param>
        public void RemoveMember(OrganizationMemberModel member)
        {
            this.Members = this.Members
                .Where(m => m != member)
                .ToList();
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
            this.PendingInvitations = this.PendingInvitations
                .Where(i => i != invitation)
                .ToList();
        }

        /// <summary>
        /// Handles initiating an organization transfer.
        /// </summary>
        public void TransferOrganization()
        {
        }
    }
}
