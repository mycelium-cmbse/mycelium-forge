// ------------------------------------------------------------------------------------------------
// <copyright file="UserProfileModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents the user profile information displayed on the account settings page.
    /// </summary>
    /// <param name="Username">The unique username handle of the account.</param>
    /// <param name="Email">The primary email address associated with the account.</param>
    /// <param name="IsEmailVerified">A value indicating whether the primary email address is verified.</param>
    /// <param name="DisplayName">The full display name of the user.</param>
    /// <param name="Company">The company or organization affiliation of the user.</param>
    /// <param name="Location">The geographical location of the user.</param>
    /// <param name="Website">The personal or organization website URL of the user.</param>
    /// <param name="Biography">The personal biography or summary description of the user.</param>
    public record UserProfileModel(
        string Username,
        string Email,
        bool IsEmailVerified,
        string DisplayName,
        string Company,
        string Location,
        string Website,
        string Biography);
}
