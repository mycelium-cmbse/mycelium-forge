// ------------------------------------------------------------------------------------------------
// <copyright file="PackageMaintainerModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents a package maintainer or contributing author.
    /// </summary>
    /// <param name="Name">The display name of the maintainer.</param>
    /// <param name="Initials">The initials of the maintainer used for avatar representation.</param>
    /// <param name="IsVerified">A value indicating whether the maintainer is a verified entity.</param>
    /// <param name="Role">The role or account type description of the maintainer.</param>
    public record PackageMaintainerModel(
        string Name,
        string Initials,
        bool IsVerified = false,
        string Role = "");
}
