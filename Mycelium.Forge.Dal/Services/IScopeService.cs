// ------------------------------------------------------------------------------------------------
// <copyright file="IScopeService.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.Services
{
    using Mycelium.Forge.Common;

    /// <summary>
    /// Defines read-only domain service operations for <see cref="IScope" /> instances (organizations and accounts).
    /// </summary>
    public interface IScopeService : IReadService<IScope>
    {
    }
}
