// ------------------------------------------------------------------------------------------------
// <copyright file="AccountOrganizationMembershipModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents an organization membership associated with the user account.
    /// </summary>
    /// <param name="Scope">The handle or scope of the organization.</param>
    /// <param name="Name">The full display name of the organization.</param>
    /// <param name="Initials">The initials used for the organization avatar.</param>
    /// <param name="Role">The role assigned to the user within the organization.</param>
    /// <param name="IsAdministrator">A value indicating whether the user is an administrator of the organization.</param>
    public record AccountOrganizationMembershipModel(
        string Scope,
        string Name,
        string Initials,
        string Role,
        bool IsAdministrator = false);
}
