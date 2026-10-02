// ------------------------------------------------------------------------------------------------
// <copyright file="PointerCacheExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.Decorators
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.Services;
    using Mycelium.Forge.Orm.Helpers;

    using ZiggyCreatures.Caching.Fusion;

    /// <summary>
    /// Provides extension methods on <see cref="IFusionCache" /> for secondary index pointer key caching.
    /// </summary>
    public static class PointerCacheExtensions
    {
        /// <param name="fusionCache">The <see cref="IFusionCache" /> used as a caching layer.</param>
        extension(IFusionCache fusionCache)
        {
            /// <summary>
            /// Attempts to resolve an entity via a secondary index pointer cache key using the provided <see cref="IReadService{T}" />
            /// verifying cache integrity against the expected short name.
            /// </summary>
            /// <typeparam name="TEntity">The entity type, which must implement <see cref="INamespace" />.</typeparam>
            /// <param name="cacheKey">The secondary index cache key mapping to the entity identifier.</param>
            /// <param name="readService">The <see cref="IReadService{T}" /> used to fetch the entity by identifier.</param>
            /// <param name="userContext">The contextual user information and assigned roles.</param>
            /// <param name="expectedShortName">The expected short name to verify cache integrity.</param>
            /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
            /// <returns>The resolved <typeparamref name="TEntity" />, or <c>null</c> if not cached or stale.</returns>
            public async Task<TEntity> TryGetByPointerAsync<TEntity>(string cacheKey, IReadService<TEntity> readService, IUserContext userContext, string expectedShortName, CancellationToken token = default) where TEntity : class, INamespace
            {
                var cachedId = await fusionCache.TryGetAsync<Guid>(cacheKey, token: token);

                if (!cachedId.HasValue)
                {
                    return null;
                }

                var cachedResult = await readService.ReadAsync(userContext, token, [cachedId.Value]);

                if (!cachedResult.IsError && cachedResult.Value.Count > 0)
                {
                    var cached = cachedResult.Value[0];

                    if (string.Equals(cached.ShortName, expectedShortName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return cached;
                    }
                }

                await fusionCache.RemoveAsync(cacheKey, token: token);
                return null;
            }

            /// <summary>
            /// Stores a secondary index pointer key in the cache with the specified tags and standard cache duration.
            /// </summary>
            /// <param name="cacheKey">The secondary index cache key mapping to the entity identifier.</param>
            /// <param name="id">The unique identifier of the entity.</param>
            /// <param name="tags">The cache invalidation tags.</param>
            /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
            /// <returns>An awaitable <see cref="Task" />.</returns>
            public Task SetPointerAsync(string cacheKey, Guid id, IEnumerable<string> tags, CancellationToken token = default)
            {
                return fusionCache.SetAsync(cacheKey, id, tags: tags, duration: DaoHelper.CacheDuration, token: token).AsTask();
            }
        }
    }
}
