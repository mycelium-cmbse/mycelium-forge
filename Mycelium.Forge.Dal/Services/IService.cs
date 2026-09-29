// ------------------------------------------------------------------------------------------------
// <copyright file="IService.cs" company="Starion Group S.A.">
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

    using Npgsql;

    /// <summary>
    /// Base interface for domain entity services bringing validation, permission evaluation, and DAO operations together.
    /// </summary>
    /// <typeparam name="T">The domain entity type implementing <see cref="IThing" />.</typeparam>
    public interface IService<T> : IReadService<T> where T : IThing
    {
        /// <summary>
        /// Asynchronously creates (persists) instances of <typeparamref name="T" /> within an existing transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="transaction">The current <see cref="NpgsqlTransaction" /> to the database.</param>
        /// <param name="dtos">The collection of <typeparamref name="T" /> instances to create.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="ErrorOr{Created}" /> indicating success or failure.</returns>
        Task<ErrorOr<Created>> CreateAsync(IUserContext userContext, NpgsqlTransaction transaction, IEnumerable<T> dtos, CancellationToken token);

        /// <summary>
        /// Asynchronously creates (persists) instances of <typeparamref name="T" /> managing its own connection and transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="dtos">The collection of <typeparamref name="T" /> instances to create.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="ErrorOr{Created}" /> indicating success or failure.</returns>
        Task<ErrorOr<Created>> CreateAsync(IUserContext userContext, IEnumerable<T> dtos, CancellationToken token);

        /// <summary>
        /// Asynchronously updates instances of <typeparamref name="T" /> within an existing transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="transaction">The current <see cref="NpgsqlTransaction" /> to the database.</param>
        /// <param name="dtos">The collection of <typeparamref name="T" /> instances to update.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="ErrorOr{Updated}" /> indicating success or failure.</returns>
        Task<ErrorOr<Updated>> UpdateAsync(IUserContext userContext, NpgsqlTransaction transaction, IEnumerable<T> dtos, CancellationToken token);

        /// <summary>
        /// Asynchronously updates instances of <typeparamref name="T" /> managing its own connection and transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="dtos">The collection of <typeparamref name="T" /> instances to update.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="ErrorOr{Updated}" /> indicating success or failure.</returns>
        Task<ErrorOr<Updated>> UpdateAsync(IUserContext userContext, IEnumerable<T> dtos, CancellationToken token);

        /// <summary>
        /// Asynchronously deletes instances of <typeparamref name="T" /> within an existing transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="transaction">The current <see cref="NpgsqlTransaction" /> to the database.</param>
        /// <param name="iids">The collection of unique identifiers of instances to delete.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="ErrorOr{Deleted}" /> indicating success or failure.</returns>
        Task<ErrorOr<Deleted>> DeleteAsync(IUserContext userContext, NpgsqlTransaction transaction, IEnumerable<Guid> iids, CancellationToken token);

        /// <summary>
        /// Asynchronously deletes instances of <typeparamref name="T" /> managing its own connection and transaction.
        /// </summary>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="iids">The collection of unique identifiers of instances to delete.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="ErrorOr{Deleted}" /> indicating success or failure.</returns>
        Task<ErrorOr<Deleted>> DeleteAsync(IUserContext userContext, IEnumerable<Guid> iids, CancellationToken token);
    }
}
