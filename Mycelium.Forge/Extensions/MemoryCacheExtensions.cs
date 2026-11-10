// ------------------------------------------------------------------------------------------------
// <copyright file="MemoryCacheExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extensions
{
    using Microsoft.Extensions.Caching.Memory;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Provides extension methods for <see cref="IMemoryCache" /> operations.
    /// </summary>
    public static class MemoryCacheExtensions
    {
        /// <param name="memoryCache">The extended <see cref="IMemoryCache" /> instance.</param>
        extension(IMemoryCache memoryCache)
        {
            /// <summary>
            /// Builds the cache key for package details scoped to the current user context and package coordinates.
            /// </summary>
            /// <param name="userContext">The contextual user information.</param>
            /// <param name="scope">The scope short name.</param>
            /// <param name="packageShortname">The package short name.</param>
            /// <returns>The cache key string.</returns>
            public string BuildPackageDetailsCacheKey(IUserContext userContext, string scope, string packageShortname)
            {
                var userIdentifier = userContext.IsAuthenticated && userContext.AccountId.HasValue
                    ? userContext.AccountId.Value
                    : Guid.Empty;

                return $"pkg-details:{scope}:{packageShortname}:{userIdentifier}";
            }

            /// <summary>
            /// Removes the cached package details for the specified user context and package coordinates.
            /// </summary>
            /// <param name="userContext">The contextual user information.</param>
            /// <param name="scope">The scope short name.</param>
            /// <param name="packageShortname">The package short name.</param>
            public void RemovePackageDetails(IUserContext userContext, string scope, string packageShortname)
            {
                var cacheKey = memoryCache.BuildPackageDetailsCacheKey(userContext, scope, packageShortname);
                memoryCache.Remove(cacheKey);
            }
        }
    }
}
