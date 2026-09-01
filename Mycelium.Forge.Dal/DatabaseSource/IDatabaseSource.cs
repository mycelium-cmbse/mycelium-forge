// ------------------------------------------------------------------------------------------------
// <copyright file="IDatabaseSource.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.DatabaseSource
{
    using System.Threading;
    using System.Threading.Tasks;

    using Npgsql;

    /// <summary>
    /// Provides an abstraction for opening database connections.
    /// </summary>
    public interface IDatabaseSource
    {
        /// <summary>
        /// Asynchronously opens a new database connection.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A new open <see cref="NpgsqlConnection"/>.</returns>
        Task<NpgsqlConnection> OpenNewConnectionAsync(CancellationToken cancellationToken = default);
    }
}
