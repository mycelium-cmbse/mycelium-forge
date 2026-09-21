// ------------------------------------------------------------------------------------------------
// <copyright file="ServiceExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extensions
{
    using System.Collections.Immutable;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.Services;

    /// <summary>
    /// Provides extension methods for <see cref="IService{T}" /> instances.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <param name="service">The <see cref="IService{T}" /> instance.</param>
        /// <typeparam name="T">The domain entity type implementing <see cref="IThing" />.</typeparam>
        extension<T>(IService<T> service) where T : IThing
        {
        /// <summary>
        /// Asynchronously reads instances of <typeparamref name="T" /> managing its own connection and transaction,
        /// returning an empty collection if an error occurs or if no identifiers are specified when required.
        /// </summary>
        /// <typeparam name="T">The domain entity type implementing <see cref="IThing" />.</typeparam>
        /// <param name="service">The <see cref="IService{T}" /> instance.</param>
        /// <param name="userContext">The contextual user information and assigned roles.</param>
        /// <param name="iids">An optional collection of unique identifiers to read.</param>
        /// <param name="emptyWhenNoIds">
        /// A value indicating whether to return an empty collection when <paramref name="iids" /> is null or empty,
        /// or to query all entities when <see langword="false" />.
        /// </param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>
        /// A task representing the asynchronous operation, resolving to an <see cref="ImmutableList{T}" /> of entities,
        /// or an empty collection if an error occurs or no identifiers were supplied when required.
        /// </returns>
            public async Task<ImmutableList<T>> ReadOrEmpty(IUserContext userContext, IEnumerable<Guid> iids = null, bool emptyWhenNoIds = true, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(service);
            ArgumentNullException.ThrowIfNull(userContext);

            var ids = iids?.Distinct().ToArray() ?? [];

            if (ids.Length == 0 && emptyWhenNoIds)
            {
                return [];
            }

            var result = await service.ReadAsync(userContext, token, ids);

            if (result.IsError || result.Value == null)
            {
                return [];
            }

            return result.Value;
        }

            /// <summary>
            /// Asynchronously reads a single instance of <typeparamref name="T" /> by its unique identifier, returning
            /// </summary>
            /// <param name="userContext">The contextual user information and assigned roles.</param>
            /// <param name="iid">The unique identifier of the instance to read.</param>
            /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
            /// <returns>
            /// A task representing the asynchronous operation, resolving to the requested entity or null if not found.
            /// </returns>
            public async Task<T> ReadOrNull(IUserContext userContext, Guid iid, CancellationToken token = default)
            {
                var result = await service.ReadOrEmpty(userContext, [iid], true, token);
                return result.Find(x => x.Id == iid);
            }
        }
    }
}
