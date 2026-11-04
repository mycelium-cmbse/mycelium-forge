// ------------------------------------------------------------------------------------------------
// <copyright file="IPendingRequestsManagerViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.AccountSettings
{
    using ErrorOr;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.ViewModels.Rows;

    /// <summary>
    /// Defines the view model contract for managing pending organization invitations for the user account.
    /// </summary>
    public interface IPendingRequestsManagerViewModel
    {
        /// <summary>
        /// Gets or sets the collection of pending invitations for the user account.
        /// </summary>
        List<OrganizationInvitationRowViewModel> PendingInvitations { get; set; }

        /// <summary>
        /// Gets or sets the action invoked when an organization invitation has been accepted.
        /// </summary>
        Action<IOrganization> OnOrganizationAccepted { get; set; }

        /// <summary>
        /// Initializes the view model state asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task InitializeViewModel();

        /// <summary>
        /// Accepts the specified pending organization invitation asynchronously.
        /// </summary>
        /// <param name="row">The pending invitation row to accept.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A task indicating the success or failure of the operation.</returns>
        Task<ErrorOr<Success>> AcceptInvitation(OrganizationInvitationRowViewModel row, CancellationToken token = default);

        /// <summary>
        /// Rejects the specified pending organization invitation asynchronously.
        /// </summary>
        /// <param name="row">The pending invitation row to reject.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A task indicating the success or failure of the operation.</returns>
        Task<ErrorOr<Success>> RejectInvitation(OrganizationInvitationRowViewModel row, CancellationToken token = default);
    }
}
