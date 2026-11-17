// ------------------------------------------------------------------------------------------------
// <copyright file="IInvitationManagerViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.Invitations
{
    using DynamicData;

    using ErrorOr;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Defines the shared view model contract for loading, creating, resending, and revoking pending invitations of a
    /// specific invitation domain interface type.
    /// </summary>
    /// <typeparam name="TInvitation">The invitation domain interface type.</typeparam>
    public interface IInvitationManagerViewModel<TInvitation> where TInvitation : class, IInvitation
    {
        /// <summary>
        /// Gets the reactive source list of pending invitations.
        /// </summary>
        SourceList<TInvitation> PendingInvitations { get; }

        /// <summary>
        /// Initializes the invitation manager state for the specified owning entity asynchronously.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="ownerEntityId">The unique identifier of the owning organization or package.</param>
        /// <param name="canManage">A value indicating whether the current user is allowed to manage invitations.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task InitializeViewModel(IUserContext userContext, Guid ownerEntityId, bool canManage = true, CancellationToken token = default);

        /// <summary>
        /// Sets default status and expiration details on the specified invitation and persists it asynchronously.
        /// </summary>
        /// <param name="invitation">The invitation entity to send.</param>
        /// <param name="target">The invited target account.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A task indicating the success or failure of the operation.</returns>
        Task<ErrorOr<Success>> SendInvitation(TInvitation invitation, IAccount target, CancellationToken token = default);

        /// <summary>
        /// Resends the specified pending invitation with a refreshed expiration timestamp asynchronously.
        /// </summary>
        /// <param name="invitation">The pending invitation to resend.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task ResendInvitation(TInvitation invitation, CancellationToken token = default);

        /// <summary>
        /// Revokes the specified pending invitation asynchronously.
        /// </summary>
        /// <param name="invitation">The pending invitation to revoke.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task RevokeInvitation(TInvitation invitation, CancellationToken token = default);

        /// <summary>
        /// Resolves the display short name for the target account of the specified invitation.
        /// </summary>
        /// <param name="invitation">The invitation whose target account short name is resolved.</param>
        /// <returns>The short name of the target account, or <c>Unknown</c> when unavailable.</returns>
        string GetTargetName(TInvitation invitation);
    }
}
