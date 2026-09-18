// ------------------------------------------------------------------------------------------------
// <copyright file="IDatabaseSeeder.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Data
{
    /// <summary>
    /// Defines operations for seeding development data into the backing database.
    /// </summary>
    public interface IDatabaseSeeder
    {
        /// <summary>
        /// Asynchronously drops existing schema, applies migrations, and seeds initial development entities.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to abort the seeding operation.</param>
        /// <returns>
        /// A task representing the asynchronous operation, resolving to <see langword="true" /> if seeding
        /// completed successfully; otherwise <see langword="false" />.
        /// </returns>
        Task<bool> SeedAsync(CancellationToken cancellationToken = default);
    }
}
