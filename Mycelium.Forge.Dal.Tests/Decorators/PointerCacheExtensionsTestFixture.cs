// ------------------------------------------------------------------------------------------------
// <copyright file="PointerCacheExtensionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Dal.Tests.Decorators
{
    using System.Collections.Immutable;

    using ErrorOr;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.Decorators;
    using Mycelium.Forge.Dal.Services;

    using NUnit.Framework;

    using ZiggyCreatures.Caching.Fusion;

    [TestFixture]
    public class PointerCacheExtensionsTestFixture
    {
        private IFusionCache fusionCache;
        private Mock<IReadService<Organization>> readServiceMock;
        private IUserContext userContext;

        [SetUp]
        public void SetUp()
        {
            this.fusionCache = new FusionCache(new FusionCacheOptions());
            this.readServiceMock = new Mock<IReadService<Organization>>();

            this.userContext = new UserContext
            {
                AccountId = Guid.NewGuid(),
                Username = "testUser",
                CurrentRoles = [RoleKind.Account]
            };
        }

        [TearDown]
        public void TearDown()
        {
            this.fusionCache.Dispose();
        }

        [Test]
        public async Task VerifySetPointerAsync()
        {
            const string cacheKey = "Package:Coordinate:owner/pkg";
            var targetId = Guid.NewGuid();
            var tag = $"Package:{targetId}";

            await this.fusionCache.SetPointerAsync(cacheKey, targetId, [tag], CancellationToken.None);

            var cached = await this.fusionCache.TryGetAsync<Guid>(cacheKey);
            Assert.That(cached.HasValue, Is.True);
            Assert.That(cached.Value, Is.EqualTo(targetId));

            await this.fusionCache.RemoveByTagAsync(tag);

            var cachedAfterTagRemoval = await this.fusionCache.TryGetAsync<Guid>(cacheKey);
            Assert.That(cachedAfterTagRemoval.HasValue, Is.False);
        }

        [Test]
        public async Task VerifyTryGetByPointerAsync()
        {
            const string cacheKey = "Scope:ShortName:test-org";
            var targetId = Guid.NewGuid();

            // Case 1: Pointer not in cache -> returns null
            var resultNoCache = await this.fusionCache.TryGetByPointerAsync(cacheKey, this.readServiceMock.Object, this.userContext, "test-org", CancellationToken.None);
            Assert.That(resultNoCache, Is.Null);

            // Case 2: Pointer in cache, read service returns error -> removes cache key and returns null
            await this.fusionCache.SetPointerAsync(cacheKey, targetId, [$"Scope:{targetId}"], CancellationToken.None);

            this.readServiceMock.Setup(x => x.ReadAsync(this.userContext, It.IsAny<CancellationToken>(), It.Is<Guid[]>(ids => ids.Length == 1 && ids[0] == targetId)))
                .ReturnsAsync(Error.NotFound());

            var resultError = await this.fusionCache.TryGetByPointerAsync(cacheKey, this.readServiceMock.Object, this.userContext, "test-org", CancellationToken.None);
            var cachedAfterError = await this.fusionCache.TryGetAsync<Guid>(cacheKey);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(resultError, Is.Null);
                Assert.That(cachedAfterError.HasValue, Is.False);
            }

            // Case 3: Pointer in cache, read service returns empty list -> removes cache key and returns null
            await this.fusionCache.SetPointerAsync(cacheKey, targetId, [$"Scope:{targetId}"], CancellationToken.None);

            this.readServiceMock.Setup(x => x.ReadAsync(this.userContext, It.IsAny<CancellationToken>(), It.Is<Guid[]>(ids => ids.Length == 1 && ids[0] == targetId)))
                .ReturnsAsync(ImmutableList<Organization>.Empty);

            var resultEmpty = await this.fusionCache.TryGetByPointerAsync(cacheKey, this.readServiceMock.Object, this.userContext, "test-org", CancellationToken.None);
            var cachedAfterEmpty = await this.fusionCache.TryGetAsync<Guid>(cacheKey);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(resultEmpty, Is.Null);
                Assert.That(cachedAfterEmpty.HasValue, Is.False);
            }

            // Case 4: Pointer in cache, read service returns entity with mismatched short name -> removes cache key and returns null
            var mismatchedOrg = new Organization
            {
                Id = targetId,
                ShortName = "other-org"
            };

            await this.fusionCache.SetPointerAsync(cacheKey, targetId, [$"Scope:{targetId}"], CancellationToken.None);

            this.readServiceMock.Setup(x => x.ReadAsync(this.userContext, It.IsAny<CancellationToken>(), It.Is<Guid[]>(ids => ids.Length == 1 && ids[0] == targetId)))
                .ReturnsAsync(ImmutableList.Create(mismatchedOrg));

            var resultMismatch = await this.fusionCache.TryGetByPointerAsync(cacheKey, this.readServiceMock.Object, this.userContext, "test-org", CancellationToken.None);
            var cachedAfterMismatch = await this.fusionCache.TryGetAsync<Guid>(cacheKey);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(resultMismatch, Is.Null);
                Assert.That(cachedAfterMismatch.HasValue, Is.False);
            }

            // Case 5: Pointer in cache, read service returns entity with matching short name (case-insensitive) -> returns entity
            var matchingOrg = new Organization
            {
                Id = targetId,
                ShortName = "TEST-ORG"
            };

            await this.fusionCache.SetPointerAsync(cacheKey, targetId, [$"Scope:{targetId}"], CancellationToken.None);

            this.readServiceMock.Setup(x => x.ReadAsync(this.userContext, It.IsAny<CancellationToken>(), It.Is<Guid[]>(ids => ids.Length == 1 && ids[0] == targetId)))
                .ReturnsAsync(ImmutableList.Create(matchingOrg));

            var resultSuccess = await this.fusionCache.TryGetByPointerAsync(cacheKey, this.readServiceMock.Object, this.userContext, "test-org", CancellationToken.None);
            var cachedAfterSuccess = await this.fusionCache.TryGetAsync<Guid>(cacheKey);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(resultSuccess, Is.Not.Null);
                Assert.That(resultSuccess.Id, Is.EqualTo(targetId));
                Assert.That(resultSuccess.ShortName, Is.EqualTo("TEST-ORG"));
                Assert.That(cachedAfterSuccess.HasValue, Is.True);
                Assert.That(cachedAfterSuccess.Value, Is.EqualTo(targetId));
            }
        }
    }
}
