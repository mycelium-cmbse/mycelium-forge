// ------------------------------------------------------------------------------------------------
// <copyright file="MemoryCacheExtensionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Extensions
{
    using System;

    using Microsoft.Extensions.Caching.Memory;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extensions;

    [TestFixture]
    public class MemoryCacheExtensionsTestFixture
    {
        private MemoryCache memoryCache;

        [SetUp]
        public void SetUp()
        {
            this.memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        [TearDown]
        public void TearDown()
        {
            this.memoryCache.Dispose();
        }

        [Test]
        public void VerifyBuildPackageDetailsCacheKey()
        {
            var accountId = Guid.NewGuid();
            var authenticatedContext = new UserContext { AccountId = accountId, Username = "alice" };
            var anonymousContext = new UserContext { Username = "anonymous" };

            // Case 1: Authenticated user produces key with account identifier
            var authenticatedKey = this.memoryCache.BuildPackageDetailsCacheKey(authenticatedContext, "starion", "my-package");
            Assert.That(authenticatedKey, Is.EqualTo($"pkg-details:starion:my-package:{accountId}"));

            // Case 2: Anonymous user produces key with Guid.Empty
            var anonymousKey = this.memoryCache.BuildPackageDetailsCacheKey(anonymousContext, "starion", "my-package");
            Assert.That(anonymousKey, Is.EqualTo($"pkg-details:starion:my-package:{Guid.Empty}"));
        }

        [Test]
        public void VerifyRemovePackageDetails()
        {
            var accountId = Guid.NewGuid();
            var userContext = new UserContext { AccountId = accountId, Username = "alice" };
            var cacheKey = this.memoryCache.BuildPackageDetailsCacheKey(userContext, "starion", "my-package");

            this.memoryCache.Set(cacheKey, "test-data");
            Assert.That(this.memoryCache.TryGetValue(cacheKey, out _), Is.True);

            this.memoryCache.RemovePackageDetails(userContext, "starion", "my-package");
            Assert.That(this.memoryCache.TryGetValue(cacheKey, out _), Is.False);
        }
    }
}
