// ------------------------------------------------------------------------------------------------
// <copyright file="DaoHelperTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Orm.Tests.Helpers
{
    using Microsoft.Extensions.Logging;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Orm.Helpers;

    using NUnit.Framework;

    using ZiggyCreatures.Caching.Fusion;

    [TestFixture]
    public class DaoHelperTestFixture
    {
        private IFusionCache fusionCache;
        private Mock<ILogger> loggerMock;

        [SetUp]
        public void SetUp()
        {
            this.fusionCache = new FusionCache(new FusionCacheOptions());
            this.loggerMock = new Mock<ILogger>();
        }

        [TearDown]
        public void TearDown()
        {
            this.fusionCache.Dispose();
        }

        [Test]
        public async Task VerifyAddToCacheAsync()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(DaoHelper.CacheDurationInMinutes, Is.EqualTo(60));
                Assert.That(DaoHelper.CacheDuration, Is.EqualTo(TimeSpan.FromMinutes(60)));
            }

            var packageId = Guid.NewGuid();

            var package = new Package
            {
                Id = packageId,
                Name = "test-pkg"
            };

            this.loggerMock.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(true);

            await DaoHelper.AddToCacheAsync(this.fusionCache, this.loggerMock.Object, package, CancellationToken.None);

            var cached = await this.fusionCache.TryGetAsync<Package>($"Package:{packageId}");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(cached.HasValue, Is.True);
                Assert.That(cached.Value.Id, Is.EqualTo(packageId));
                Assert.That(cached.Value.Name, Is.EqualTo("test-pkg"));
            }

            var otherPackageId = Guid.NewGuid();

            var interfacePackage = new Package
            {
                Id = otherPackageId,
                Name = "interface-pkg"
            };

            this.loggerMock.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(false);

            await DaoHelper.AddToCacheAsync<IPackage>(this.fusionCache, this.loggerMock.Object, interfacePackage, CancellationToken.None);

            var cachedInterface = await this.fusionCache.TryGetAsync<IPackage>($"Package:{otherPackageId}");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(cachedInterface.HasValue, Is.True);
                Assert.That(cachedInterface.Value.Id, Is.EqualTo(otherPackageId));
                Assert.That(cachedInterface.Value.Name, Is.EqualTo("interface-pkg"));
            }
        }
    }
}
