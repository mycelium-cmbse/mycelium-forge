// ------------------------------------------------------------------------------------------------
// <copyright file="DaoHelper.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Orm.Helpers
{
    using Mycelium.Forge.Common;

    using ZiggyCreatures.Caching.Fusion;

    /// <summary>
    /// Provides shared helper utilities for Data Access Object (DAO) implementations.
    /// </summary>
    public static class DaoHelper
    {
        /// <summary>
        /// The cache duration in minutes for cached entities and identifiers.
        /// </summary>
        public const int CacheDurationInMinutes = 60;

        /// <summary>
        /// The cache duration as a <see cref="TimeSpan" /> for cached entities and identifiers.
        /// </summary>
        public static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(CacheDurationInMinutes);

        /// <summary>
        /// Adds an <see cref="IThing" /> instance to the cache.
        /// </summary>
        /// <typeparam name="TThing">The type of the entity to cache, which must implement <see cref="IThing" />.</typeparam>
        /// <param name="fusionCache">The <see cref="IFusionCache" /> used as a caching layer.</param>
        /// <param name="logger">The <see cref="ILogger" /> used for logging.</param>
        /// <param name="thing">The <see cref="IThing" /> instance that is to be added to the cache.</param>
        /// <param name="token">The <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>An awaitable <see cref="Task" />.</returns>
        public static Task AddToCacheAsync<TThing>(IFusionCache fusionCache, ILogger logger, TThing thing, CancellationToken token) where TThing : IThing
        {
            var typeName = typeof(TThing).Name;

            if (typeName.StartsWith('I') && typeName.Length > 1 && char.IsUpper(typeName[1]))
            {
                typeName = typeName[1..];
            }

            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace("Adding {TypeName} {Dto} to the cache", typeName, thing);
            }

            return fusionCache.SetAsync($"{typeName}:{thing.Id}", thing, CacheDuration, token).AsTask();
        }
    }
}
