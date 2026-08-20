// ------------------------------------------------------------------------------------------------
// <copyright file="IOrganizationSettingsViewModel.cs" company="Starion Group S.A.">
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
    /// Defines the view model contract for managing organization members, pending invitations, and scope settings.
    /// </summary>
    public interface IOrganizationSettingsViewModel
    {
        /// <summary>
        /// Gets or sets the organization profile details.
        /// </summary>
        OrganizationModel Organization { get; set; }

        /// <summary>
        /// Gets or sets the current user's role within the organization.
        /// </summary>
        string CurrentUserRole { get; set; }

        /// <summary>
        /// Gets or sets the collection of members belonging to the organization.
        /// </summary>
        IReadOnlyList<OrganizationMemberModel> Members { get; set; }

        /// <summary>
        /// Gets or sets the collection of pending invitations for the organization.
        /// </summary>
        IReadOnlyList<OrganizationInvitationModel> PendingInvitations { get; set; }

        /// <summary>
        /// Gets or sets the available role options for organization members.
        /// </summary>
        IReadOnlyList<string> RoleOptions { get; set; }

        /// <summary>
        /// Initializes the view model state for the specified organization identifier or scope.
        /// </summary>
        /// <param name="id">The unique identifier or slug handle of the organization.</param>
        void InitializeViewModel(string id);

        /// <summary>
        /// Changes the role of the specified organization member.
        /// </summary>
        /// <param name="member">The member whose role is being updated.</param>
        /// <param name="newRole">The new role to assign to the member.</param>
        void ChangeMemberRole(OrganizationMemberModel member, string newRole);

        /// <summary>
        /// Removes the specified member from the organization.
        /// </summary>
        /// <param name="member">The member to remove.</param>
        void RemoveMember(OrganizationMemberModel member);

        /// <summary>
        /// Resends the specified pending invitation.
        /// </summary>
        /// <param name="invitation">The invitation to resend.</param>
        void ResendInvitation(OrganizationInvitationModel invitation);

        /// <summary>
        /// Revokes the specified pending invitation.
        /// </summary>
        /// <param name="invitation">The invitation to revoke.</param>
        void RevokeInvitation(OrganizationInvitationModel invitation);

        /// <summary>
        /// Handles initiating an organization transfer.
        /// </summary>
        void TransferOrganization();
    }
}
