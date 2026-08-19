// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents the organization or publisher profile information displayed on the organization page.
    /// </summary>
    /// <param name="Name">The display name of the organization.</param>
    /// <param name="Initials">The organization initials used for avatar representation.</param>
    /// <param name="Scope">The scope or namespace handle of the organization.</param>
    /// <param name="Description">The summary description or mission statement of the organization.</param>
    /// <param name="IsVerified">A value indicating whether the organization is a verified publisher.</param>
    /// <param name="PackageCount">The number of packages published by this organization.</param>
    /// <param name="VersionCount">The total number of package versions released.</param>
    /// <param name="ImportCount">The total number of imports across all packages.</param>
    /// <param name="MemberSinceYear">The year when the organization joined or became a member.</param>
    public record OrganizationModel(
        string Name,
        string Initials,
        string Scope,
        string Description,
        bool IsVerified,
        int PackageCount,
        int VersionCount,
        int ImportCount,
        int MemberSinceYear);
}
