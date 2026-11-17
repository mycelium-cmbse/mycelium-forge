// ------------------------------------------------------------------------------------------------
// <copyright file="IInvitationRowViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.Rows
{
    using Mycelium.Forge.Common;

    /// <summary>
    /// Defines a unified row contract for displaying pending organization and package invitations.
    /// </summary>
    public interface IInvitationRowViewModel
    {
        /// <summary>
        /// Gets the underlying pending invitation entity.
        /// </summary>
        IInvitation Invitation { get; }

        /// <summary>
        /// Gets the human-readable display role name for the invitation.
        /// </summary>
        string RoleName { get; }

        /// <summary>
        /// Gets the relative expiration description string.
        /// </summary>
        string ExpiresIn { get; }
    }
}
