// ------------------------------------------------------------------------------------------------
// <copyright file="IUserService.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Services
{
    using Mycelium.Forge.Common;

    /// <summary>
    /// Provides access to the identity, profile details, and assigned roles of the currently authenticated user within the
    /// scope of a Blazor circuit.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets the current user account, optionally forcing a reload from the data source.
        /// </summary>
        /// <param name="forceLoad">The value indicating whether to force a reload.</param>
        /// <returns>A <see cref="Task{IAccount}" /> representing the asynchronous operation.</returns>
        Task<IAccount> GetCurrentUser(bool forceLoad = false);

        /// <summary>
        /// Gets the user context, which includes the account ID, username, and current roles of the authenticated user.
        /// </summary>
        /// <param name="forceLoad">The value indicating whether to force a reload.</param>
        /// <returns>A <see cref="Task{IUserContext}" /> representing the asynchronous operation.</returns>
        Task<IUserContext> GetUserContext(bool forceLoad = false);

        /// <summary>
        /// Sets the currently authenticated user by account identifier and optional roles.
        /// </summary>
        /// <param name="userId">The unique identifier of the user account to set as current, or <c>null</c> for unauthenticated.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task SetCurrentUser(Guid? userId, CancellationToken cancellationToken = default);
    }
}
