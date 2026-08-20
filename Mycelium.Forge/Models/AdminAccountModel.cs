// ------------------------------------------------------------------------------------------------
// <copyright file="AdminAccountModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents an account record displayed in the installation administration accounts table.
    /// </summary>
    /// <param name="Id">The unique identifier of the account.</param>
    /// <param name="Name">The full display name of the account user.</param>
    /// <param name="Username">The username handle of the account (e.g., @r.andre).</param>
    /// <param name="Initials">The initials used for avatar representation.</param>
    /// <param name="Email">The primary email address associated with the account.</param>
    /// <param name="IsAdministrator">A value indicating whether the account has installation administrator privileges.</param>
    /// <param name="VerificationStatus">The email verification status of the account (e.g., Verified, Pending, Unverified).</param>
    /// <param name="Organizations">The organization memberships and roles associated with the account.</param>
    /// <param name="Status">The operational status of the account (e.g., Active, Suspended, Deactivated).</param>
    public record AdminAccountModel(
        string Id,
        string Name,
        string Username,
        string Initials,
        string Email,
        bool IsAdministrator,
        string VerificationStatus,
        string Organizations,
        string Status);
}
