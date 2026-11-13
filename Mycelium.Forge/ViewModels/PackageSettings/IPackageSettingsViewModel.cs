// ------------------------------------------------------------------------------------------------
// <copyright file="IPackageSettingsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.PackageSettings
{
    using ErrorOr;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Defines the view model contract for the Mycelium Forge package settings page.
    /// </summary>
    public interface IPackageSettingsViewModel
    {
        /// <summary>
        /// Gets the authenticated user context resolved on initialization.
        /// </summary>
        IUserContext UserContext { get; }

        /// <summary>
        /// Gets or sets the underlying package DTO.
        /// </summary>
        Package Package { get; set; }

        /// <summary>
        /// Gets or sets the owning scope DTO.
        /// </summary>
        IScope Owner { get; set; }

        /// <summary>
        /// Gets or sets the collection of maintainer accounts for the package.
        /// </summary>
        List<IAccount> Maintainers { get; set; }

        /// <summary>
        /// Gets or sets the collection of owner accounts for the package.
        /// </summary>
        List<IAccount> Owners { get; set; }

        /// <summary>
        /// Gets or sets the collection of pending invitations for the package.
        /// </summary>
        List<PackageInvitation> PendingInvitations { get; set; }

        /// <summary>
        /// Gets or sets the collection of accounts that have pending invitations for the package.
        /// </summary>
        List<IAccount> InvitedAccounts { get; set; }

        /// <summary>
        /// Gets or sets the collection of released package versions.
        /// </summary>
        List<PackageVersion> Versions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user is allowed to manage the package.
        /// </summary>
        bool CanManagePackage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user is allowed to manage the package team.
        /// </summary>
        bool CanManagePackageTeam { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user is allowed to delete the package.
        /// </summary>
        bool CanDeletePackage { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the currently authenticated user.
        /// </summary>
        Guid CurrentUserId { get; set; }

        /// <summary>
        /// Initializes the package settings view model state for the specified package short name and scope asynchronously.
        /// </summary>
        /// <param name="packageShortname">The short name of the package.</param>
        /// <param name="scope">The owning scope or publisher identifier.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous initialization.</returns>
        Task InitializeViewModel(string packageShortname, string scope);

        /// <summary>
        /// Updates the package visibility setting asynchronously.
        /// </summary>
        /// <param name="visibility">The target visibility kind.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task SetVisibility(VisibilityKind visibility, CancellationToken token = default);

        /// <summary>
        /// Unlists the specified package version asynchronously.
        /// </summary>
        /// <param name="version">The package version to unlist.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task UnlistVersion(PackageVersion version, CancellationToken token = default);

        /// <summary>
        /// Relists the specified unlisted package version asynchronously.
        /// </summary>
        /// <param name="version">The package version to relist.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task RelistVersion(PackageVersion version, CancellationToken token = default);

        /// <summary>
        /// Marks the specified package version as deprecated asynchronously.
        /// </summary>
        /// <param name="version">The package version to deprecate.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task DeprecateVersion(PackageVersion version, CancellationToken token = default);

        /// <summary>
        /// Transfers package ownership to the specified recipient scope asynchronously.
        /// </summary>
        /// <param name="targetScope">The recipient target scope.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A task indicating the success or failure of the operation.</returns>
        /// <remarks>
        /// If the target scope is an <see cref="IAccount" />, the target account becomes the sole package owner and previous
        /// owners become maintainers. If the target scope is an <see cref="IOrganization" />, the organization's administrators
        /// become package owners and previous owners become maintainers.
        /// </remarks>
        Task<ErrorOr<Success>> TransferOwnership(IScope targetScope, CancellationToken token = default);

        /// <summary>
        /// Deletes the package asynchronously.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A task indicating the success or failure of the operation.</returns>
        Task<ErrorOr<Success>> DeletePackage(CancellationToken token = default);

        /// <summary>
        /// Invites an account to collaborate on the package with the specified role asynchronously.
        /// </summary>
        /// <param name="target">The target account being invited.</param>
        /// <param name="role">The invitation role to grant.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A task indicating the success or failure of the operation.</returns>
        Task<ErrorOr<Success>> InviteCollaborator(IAccount target, PackageInvitationKind role, CancellationToken token = default);

        /// <summary>
        /// Resends the specified pending package invitation asynchronously.
        /// </summary>
        /// <param name="invitation">The invitation to resend.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task ResendInvitation(PackageInvitation invitation, CancellationToken token = default);

        /// <summary>
        /// Revokes the specified pending package invitation asynchronously.
        /// </summary>
        /// <param name="invitation">The invitation to revoke.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task RevokeInvitation(PackageInvitation invitation, CancellationToken token = default);

        /// <summary>
        /// Changes the role of the specified package collaborator asynchronously.
        /// </summary>
        /// <param name="collaborator">The collaborator account whose role is changing.</param>
        /// <param name="newRole">The new package role to assign.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task ChangeCollaboratorRole(IAccount collaborator, PackageInvitationKind newRole, CancellationToken token = default);

        /// <summary>
        /// Removes the specified collaborator from the package asynchronously.
        /// </summary>
        /// <param name="collaborator">The collaborator account to remove.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task RemoveCollaborator(IAccount collaborator, CancellationToken token = default);
    }
}
