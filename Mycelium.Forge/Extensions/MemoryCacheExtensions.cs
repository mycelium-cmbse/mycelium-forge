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
            /// <remarks>
            /// Normalizes scope and package short names to lowercase. Cache keys include the caller account identifier to preserve
            /// user-specific authorization boundaries.
            /// </remarks>
            public string BuildPackageDetailsCacheKey(IUserContext userContext, string scope, string packageShortname)
            {
                ArgumentNullException.ThrowIfNull(memoryCache);

                var normalizedScope = scope?.ToLowerInvariant() ?? string.Empty;
                var normalizedPackage = packageShortname?.ToLowerInvariant() ?? string.Empty;

                var userIdentifier = userContext.IsAuthenticated && userContext.AccountId.HasValue
                    ? userContext.AccountId.Value
                    : Guid.Empty;

                return $"pkg-details:{normalizedScope}:{normalizedPackage}:{userIdentifier}";
            }

            /// <summary>
            /// Removes the cached package details for the specified user context and package coordinates.
            /// </summary>
            /// <param name="userContext">The contextual user information.</param>
            /// <param name="scope">The scope short name.</param>
            /// <param name="packageShortname">The package short name.</param>
            /// <remarks>
            /// Clears the cache entry for the publisher's user context. Other users and unauthenticated visitors will observe newly published
            /// details once their respective cached entries reach expiration (configured with a 2-minute absolute expiration window in
            /// <c>PackageDetailsViewModel</c>).
            /// </remarks>
            public void RemovePackageDetails(IUserContext userContext, string scope, string packageShortname)
            {
                var cacheKey = memoryCache.BuildPackageDetailsCacheKey(userContext, scope, packageShortname);
                memoryCache.Remove(cacheKey);
            }
        }
    }
}
