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
    using ErrorOr;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Enums;

    /// <summary>
    /// Defines the view model contract for managing organization settings, members, and team roles.
    /// </summary>
    public interface IOrganizationSettingsViewModel
    {
        /// <summary>
        /// Gets or sets the organization profile details.
        /// </summary>
        Organization Organization { get; set; }

        /// <summary>
        /// Gets or sets the current user's role within the organization.
        /// </summary>
        OrganizationRole CurrentUserRole { get; set; }

        /// <summary>
        /// Gets the current user context.
        /// </summary>
        IUserContext UserContext { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user is allowed to manage the organization.
        /// </summary>
        bool CanManageOrganization { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the currently authenticated user.
        /// </summary>
        Guid CurrentUserId { get; set; }

        /// <summary>
        /// Gets the collection of members belonging to the organization.
        /// </summary>
        IReadOnlyList<IAccount> Members { get; }

        /// <summary>
        /// Gets the collection of pending invitations for the organization.
        /// </summary>
        IReadOnlyList<OrganizationInvitation> PendingInvitations { get; }

        /// <summary>
        /// Gets the collection of accounts that have been invited to join the organization.
        /// </summary>
        IReadOnlyList<IAccount> InvitedAccounts { get; }

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
        Task ChangeMemberRole(IAccount member, OrganizationRole newRole);

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
        Task ResendInvitation(OrganizationInvitation invitation);

        /// <summary>
        /// Revokes the specified pending invitation asynchronously.
        /// </summary>
        /// <param name="invitation">The invitation to revoke.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task RevokeInvitation(OrganizationInvitation invitation);

        /// <summary>
        /// Transfers ownership of the organization to the specified recipient account asynchronously.
        /// </summary>
        /// <param name="recipient">The recipient account to transfer ownership to.</param>
        /// <returns>A <see cref="Task{ErrorOr}" /> representing the asynchronous operation.</returns>
        Task<ErrorOr<Success>> TransferOrganization(IAccount recipient);

        /// <summary>
        /// Invites an account to join the organization with the specified role asynchronously.
        /// </summary>
        /// <param name="target">The target account being invited.</param>
        /// <param name="role">The invitation role to grant.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A <see cref="Task{ErrorOr}" /> representing the asynchronous operation.</returns>
        Task<ErrorOr<Success>> InviteMember(IAccount target, OrganizationInvitationKind role, CancellationToken token = default);
    }
}
