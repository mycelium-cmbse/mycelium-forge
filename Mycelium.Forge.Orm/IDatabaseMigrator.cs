// ------------------------------------------------------------------------------------------------
// <copyright file="IDatabaseMigrator.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Orm
{
    /// <summary>
    /// Defines operations for running database migrations and resetting schema.
    /// </summary>
    public interface IDatabaseMigrator
    {
        /// <summary>
        /// Applies database migrations.
        /// </summary>
        /// <param name="connectionString">The PostgreSQL connection string.</param>
        /// <returns>
        /// <see langword="true" /> if migrations succeeded; otherwise <see langword="false" />.
        /// </returns>
        bool Migrate(string connectionString);

        /// <summary>
        /// Drops the existing schema and reapplies database migrations.
        /// </summary>
        /// <param name="connectionString">The PostgreSQL connection string.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task resolving to <see langword="true" /> if schema was reset and migrations succeeded; otherwise
        /// <see langword="false" />.
        /// </returns>
        Task<bool> ResetDatabaseAsync(string connectionString, CancellationToken cancellationToken = default);
    }
}
