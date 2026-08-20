// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationInvitationModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents a pending membership invitation sent to an external email address.
    /// </summary>
    /// <param name="Email">The recipient email address of the invitation.</param>
    /// <param name="Role">The role assigned to the invited member upon acceptance.</param>
    /// <param name="StatusText">The formatted status string detailing when the invitation was sent and when it expires.</param>
    public record OrganizationInvitationModel(
        string Email,
        string Role,
        string StatusText);
}
