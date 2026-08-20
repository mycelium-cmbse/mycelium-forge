// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationMemberModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents a member belonging to an organization along with their display name, handle, avatar initials, and assigned role.
    /// </summary>
    /// <param name="Name">The full display name of the organization member.</param>
    /// <param name="Username">The username handle of the member (e.g., @r.andre).</param>
    /// <param name="Initials">The initials used for avatar representation.</param>
    /// <param name="Role">The role assigned to the member within the organization.</param>
    public record OrganizationMemberModel(
        string Name,
        string Username,
        string Initials,
        string Role);
}
