// ------------------------------------------------------------------------------------------------
// <copyright file="IOrganizationSettingsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.OrganizationSettings
{
    using Mycelium.Forge.Common;

    /// <summary>
    /// Defines the view model contract for managing organization settings, members, and team roles.
    /// </summary>
    public interface IOrganizationSettingsViewModel
    {
        /// <summary>
        /// Gets or sets the organization profile details.
        /// </summary>
        IOrganization Organization { get; set; }

        /// <summary>
        /// Gets or sets the current user's role within the organization.
        /// </summary>
        OrganizationInvitationKind CurrentUserRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user is allowed to manage the organization.
        /// </summary>
        bool CanManageOrganization { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the currently authenticated user.
        /// </summary>
        Guid CurrentUserId { get; set; }

        /// <summary>
        /// Gets or sets the collection of members belonging to the organization.
        /// </summary>
        IReadOnlyList<IAccount> Members { get; set; }

        /// <summary>
        /// Gets or sets the collection of pending invitations for the organization.
        /// </summary>
        IReadOnlyList<IOrganizationInvitation> PendingInvitations { get; set; }

        /// <summary>
        /// Gets or sets the available role options for organization members.
        /// </summary>
        IReadOnlyList<OrganizationInvitationKind> RoleOptions { get; set; }

        /// <summary>
        /// Initializes the view model state for the specified organization short name asynchronously.
        /// </summary>
        /// <param name="shortName">The short name of the organization.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous initialization.</returns>
        Task InitializeViewModel(string shortName);

        /// <summary>
        /// Changes the role of the specified organization member asynchronously.
        /// </summary>
        /// <param name="member">The member whose role is being updated.</param>
        /// <param name="newRole">The new role to assign to the member.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task ChangeMemberRole(IAccount member, OrganizationInvitationKind newRole);

        /// <summary>
        /// Removes the specified member from the organization asynchronously.
        /// </summary>
        /// <param name="member">The member to remove.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task RemoveMember(IAccount member);

        /// <summary>
        /// Resends the specified pending invitation asynchronously.
        /// </summary>
        /// <param name="invitation">The invitation to resend.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task ResendInvitation(IOrganizationInvitation invitation);

        /// <summary>
        /// Revokes the specified pending invitation asynchronously.
        /// </summary>
        /// <param name="invitation">The invitation to revoke.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task RevokeInvitation(IOrganizationInvitation invitation);

        /// <summary>
        /// Handles initiating an organization transfer asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task TransferOrganization();
    }
}
