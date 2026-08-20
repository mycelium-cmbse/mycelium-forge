// ------------------------------------------------------------------------------------------------
// <copyright file="ApiKeyModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents an API key entry displayed on the API keys management page.
    /// </summary>
    /// <param name="Id">The unique identifier of the API key.</param>
    /// <param name="Name">The name or description of the API key.</param>
    /// <param name="Scope">The organization or user scope the key belongs to.</param>
    /// <param name="Permissions">The permissions granted to the key (e.g., publish, unlist).</param>
    /// <param name="Created">The creation date or relative time string.</param>
    /// <param name="Expires">The expiration description or relative time string.</param>
    /// <param name="LastUsed">The relative elapsed time since the key was last used.</param>
    public record ApiKeyModel(
        string Id,
        string Name,
        string Scope,
        string Permissions,
        string Created,
        string Expires,
        string LastUsed);
}
