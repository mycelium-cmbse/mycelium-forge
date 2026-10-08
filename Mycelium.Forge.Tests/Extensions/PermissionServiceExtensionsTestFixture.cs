// ------------------------------------------------------------------------------------------------
// <copyright file="PermissionServiceExtensionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.AutoGenPermissionService;
    using Mycelium.Forge.Dal.DatabaseSource;
    using Mycelium.Forge.Extensions;

    [TestFixture]
    public class PermissionServiceExtensionsTestFixture
    {
        private Mock<IPackagePermissionService> serviceMock;
        private Mock<IDatabaseSource> databaseSourceMock;
        private UserContext userContext;
        private Package package;

        [SetUp]
        public void SetUp()
        {
            this.serviceMock = new Mock<IPackagePermissionService>();
            this.databaseSourceMock = new Mock<IDatabaseSource>();
            this.userContext = new UserContext { AccountId = Guid.NewGuid() };
            this.package = new Package { Id = Guid.NewGuid() };
        }

        [Test]
        public async Task VerifyIsAllowedToDelete()
        {
            this.databaseSourceMock
                .Setup(x => x.OpenNewConnectionAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Failed to open connection"));

            var failureResult = await this.serviceMock.Object.IsAllowedToDelete(this.databaseSourceMock.Object, this.userContext, this.package, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(failureResult.IsError, Is.True);
                Assert.That(failureResult.FirstError.Description, Is.EqualTo("Failed to open connection"));
            }
        }

        [Test]
        public async Task VerifyIsAllowedToUpdate()
        {
            this.databaseSourceMock
                .Setup(x => x.OpenNewConnectionAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Failed to open connection"));

            var failureResult = await this.serviceMock.Object.IsAllowedToUpdate(this.databaseSourceMock.Object, this.userContext, this.package, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(failureResult.IsError, Is.True);
                Assert.That(failureResult.FirstError.Description, Is.EqualTo("Failed to open connection"));
            }
        }
    }
}
