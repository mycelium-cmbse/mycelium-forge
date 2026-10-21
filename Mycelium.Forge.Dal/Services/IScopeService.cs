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
    using System.Collections.Immutable;

    using ErrorOr;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Defines read-only domain service operations for <see cref="IScope" /> instances (organizations and accounts).
    /// </summary>
    public interface IScopeService : IReadService<IScope>
    {
        /// <summary>
        /// Asynchronously reads instances of <see cref="IScope" /> matching the specified scope short names managing its own
        /// connection and transaction. Organizations are matched and returned first, followed by accounts for any unresolved
        /// short names. The returned order reflects this sequence rather than the input order.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <param name="shortNames">An optional array of short name values to read.</param>
        /// <returns>A <see cref="ErrorOr{TValue}" /> containing the permitted <see cref="IScope" /> instances.</returns>
        Task<ErrorOr<ImmutableList<IScope>>> ReadByShortNameAsync(IUserContext userContext, CancellationToken token, string[] shortNames = null);
    }
}
