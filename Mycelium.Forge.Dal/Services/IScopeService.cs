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
    using ErrorOr;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Defines read-only domain service operations for <see cref="IScope" /> instances (organizations and accounts).
    /// </summary>
    public interface IScopeService : IReadService<IScope>
    {
        /// <summary>
        /// Asynchronously reads a single <see cref="IScope" /> instance matching the specified scope short name.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="shortName">The short name of the scope.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="ErrorOr{TValue}" /> containing the permitted <see cref="IScope" /> or an error.</returns>
        Task<ErrorOr<IScope>> ReadByShortNameAsync(IUserContext userContext, string shortName, CancellationToken token = default);
    }
}
