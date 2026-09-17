// ------------------------------------------------------------------------------------------------
// <copyright file="ServiceExtensionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Extensions
{
    using System;
    using System.Collections.Immutable;
    using System.Threading;
    using System.Threading.Tasks;

    using ErrorOr;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.Services;
    using Mycelium.Forge.Extensions;

    /// <summary>
    /// Suite of tests for the <see cref="ServiceExtensions" /> class.
    /// </summary>
    [TestFixture]
    public class ServiceExtensionsTestFixture
    {
        private Mock<IService<IPackage>> serviceMock;
        private Mock<IUserContext> userContextMock;

        /// <summary>
        /// Sets up the test context before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.serviceMock = new Mock<IService<IPackage>>();
            this.userContextMock = new Mock<IUserContext>();
        }

        /// <summary>
        /// Verifies the behavior of
        /// <see
        ///     cref="ServiceExtensions.ReadOrEmpty{T}(IService{T}, IUserContext, CancellationToken, System.Collections.Generic.IEnumerable{System.Guid}, bool)" />
        /// and its transaction overload under various conditions.
        /// </summary>
        [Test]
        public async Task VerifyReadOrEmpty()
        {
            var testId = Guid.NewGuid();
            var packageMock = new Mock<IPackage>();
            var expectedList = ImmutableList.Create(packageMock.Object);

            this.serviceMock
                .Setup(x => x.ReadAsync(this.userContextMock.Object, It.IsAny<CancellationToken>(), It.Is<Guid[]>(ids => ids != null && ids.Length == 1 && ids[0] == testId)))
                .ReturnsAsync(expectedList);

            this.serviceMock
                .Setup(x => x.ReadAsync(this.userContextMock.Object, It.IsAny<CancellationToken>(), It.Is<Guid[]>(ids => ids != null && ids.Length == 0)))
                .ReturnsAsync(expectedList);

            var resultWithIds = await this.serviceMock.Object.ReadOrEmpty(this.userContextMock.Object, CancellationToken.None, [testId]);
            var resultWithDefaultEmpty = await this.serviceMock.Object.ReadOrEmpty(this.userContextMock.Object, CancellationToken.None);
            var resultWithEmptyArray = await this.serviceMock.Object.ReadOrEmpty(this.userContextMock.Object, CancellationToken.None, []);
            var resultWithQueryAll = await this.serviceMock.Object.ReadOrEmpty(this.userContextMock.Object, CancellationToken.None, null, false);

            this.serviceMock
                .Setup(x => x.ReadAsync(this.userContextMock.Object, It.IsAny<CancellationToken>(), It.Is<Guid[]>(ids => ids != null && ids.Length == 1 && ids[0] == testId)))
                .ReturnsAsync(Error.Failure(description: "Database error"));

            var resultWithError = await this.serviceMock.Object.ReadOrEmpty(this.userContextMock.Object, CancellationToken.None, [testId]);

            this.serviceMock
                .Setup(x => x.ReadAsync(this.userContextMock.Object, It.IsAny<CancellationToken>(), It.Is<Guid[]>(ids => ids != null && ids.Length == 1 && ids[0] == testId)))
                .ReturnsAsync(default(ImmutableList<IPackage>));

            var resultWithNullValue = await this.serviceMock.Object.ReadOrEmpty(this.userContextMock.Object, CancellationToken.None, [testId]);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(resultWithIds, Is.EqualTo(expectedList));
                Assert.That(resultWithDefaultEmpty, Is.Empty);
                Assert.That(resultWithEmptyArray, Is.Empty);
                Assert.That(resultWithQueryAll, Is.EqualTo(expectedList));
                Assert.That(resultWithError, Is.Empty);
                Assert.That(resultWithNullValue, Is.Empty);
                Assert.ThrowsAsync<ArgumentNullException>(() => ServiceExtensions.ReadOrEmpty<IPackage>(null, this.userContextMock.Object, CancellationToken.None));
                Assert.ThrowsAsync<ArgumentNullException>(() => this.serviceMock.Object.ReadOrEmpty(null, CancellationToken.None));
            }
        }
    }
}
