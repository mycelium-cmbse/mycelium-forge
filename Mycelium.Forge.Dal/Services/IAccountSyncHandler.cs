// ------------------------------------------------------------------------------------------------
// <copyright file="IAccountSyncHandler.cs" company="Starion Group S.A.">
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
    /// Handles synchronizing account changes to an external identity provider.
    /// </summary>
    public interface IAccountSyncHandler
    {
        /// <summary>
        /// Asynchronously synchronizes changes between the existing and updated account to the external identity provider.
        /// </summary>
        /// <param name="existing">The account state prior to the update.</param>
        /// <param name="updated">The updated account state.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An <see cref="ErrorOr{Success}" /> indicating whether the synchronization succeeded.</returns>
        Task<ErrorOr<Success>> SyncAccountAsync(IAccount existing, IAccount updated, CancellationToken token);
    }
}
