// ------------------------------------------------------------------------------------------------
// <copyright file="InvitationStatusKind.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    /// <summary>
    /// Enumeration datatype that asserts the status of the invitation
    /// </summary>
    public enum InvitationStatusKind
    {
        /// <summary>
        /// Assertion that the invitation is pending either acceptance or rejection
        /// </summary>
        PENDING,

        /// <summary>
        /// Assertion that the invitation has been accepted.
        /// </summary>
        ACCEPTED,

        /// <summary>
        /// Assertion that the invitation has been rejected.
        /// </summary>
        REJECTED,

        /// <summary>
        /// Cancelled by the inviter before the recipient responded, distinct from the recipient declining it
        /// themselves.
        /// </summary>
        REVOKED,

    }
}

// ------------------------------------------------------------------------------------------------
// --------THIS IS AN AUTOMATICALLY GENERATED FILE. ANY MANUAL CHANGES WILL BE OVERWRITTEN!--------
// ------------------------------------------------------------------------------------------------
