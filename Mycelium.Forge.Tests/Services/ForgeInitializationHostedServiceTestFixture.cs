// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeInitializationHostedServiceTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using Moq;

    using Mycelium.Forge.Services;

    [TestFixture]
    [SuppressMessage("Performance", "CA1873:Avoid potentially expensive logging", Justification = "Moq expression tree in unit test.")]
    public class ForgeInitializationHostedServiceTestFixture
    {
        private Mock<IForgeContext> forgeContextMock;
        private IConfiguration configuration;
        private Mock<ILogger<ForgeInitializationHostedService>> loggerMock;
        private ForgeInitializationHostedService hostedService;

        [SetUp]
        public void SetUp()
        {
            this.forgeContextMock = new Mock<IForgeContext>();
            this.loggerMock = new Mock<ILogger<ForgeInitializationHostedService>>();

            var inMemorySettings = new Dictionary<string, string?>
            {
                { ForgeInitializationHostedService.RetryIntervalSecondsConfigurationKey, "0" }
            };

            this.configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            this.hostedService = new ForgeInitializationHostedService(this.forgeContextMock.Object, this.configuration, this.loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            this.hostedService.Dispose();
        }

        [Test]
        public void VerifyConstructor()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => new ForgeInitializationHostedService(null, this.configuration, this.loggerMock.Object), Throws.ArgumentNullException);
                Assert.That(() => new ForgeInitializationHostedService(this.forgeContextMock.Object, null, this.loggerMock.Object), Throws.ArgumentNullException);
                Assert.That(() => new ForgeInitializationHostedService(this.forgeContextMock.Object, this.configuration, null), Throws.ArgumentNullException);
            }
        }

        [Test]
        [CancelAfter(10000)]
        public async Task VerifyStartAsync()
        {
            // Case 1: Context is already initialized, initialization is bypassed.
            this.forgeContextMock.Setup(x => x.IsInitialized).Returns(true);
            await this.hostedService.StartAsync(CancellationToken.None);
            await this.hostedService.ExecuteTask!;

            this.forgeContextMock.Verify(x => x.TryInitializeAsync(It.IsAny<CancellationToken>()), Times.Never);

            // Case 2: Context is uninitialized, TryInitializeAsync succeeds on first attempt.
            using var uninitializedService = new ForgeInitializationHostedService(this.forgeContextMock.Object, this.configuration, this.loggerMock.Object);

            this.forgeContextMock.Setup(x => x.IsInitialized).Returns(false);
            this.forgeContextMock.Setup(x => x.TryInitializeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

            await uninitializedService.StartAsync(CancellationToken.None);
            await uninitializedService.ExecuteTask!;

            this.forgeContextMock.Verify(x => x.TryInitializeAsync(It.IsAny<CancellationToken>()), Times.Once);

            // Case 3: Retrying after TryInitializeAsync returns false, then succeeds on next attempt.
            using var retryOnFalseService = new ForgeInitializationHostedService(this.forgeContextMock.Object, this.configuration, this.loggerMock.Object);

            this.forgeContextMock.Setup(x => x.IsInitialized).Returns(false);

            this.forgeContextMock.SetupSequence(x => x.TryInitializeAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(false)
                .ReturnsAsync(true);

            await retryOnFalseService.StartAsync(CancellationToken.None);
            await retryOnFalseService.ExecuteTask!;

            this.forgeContextMock.Verify(x => x.TryInitializeAsync(It.IsAny<CancellationToken>()), Times.Exactly(3));

            // Case 4: Retrying after TryInitializeAsync throws an exception, logs warning, and succeeds on next attempt.
            using var retryOnExceptionService = new ForgeInitializationHostedService(this.forgeContextMock.Object, this.configuration, this.loggerMock.Object);

            this.forgeContextMock.Setup(x => x.IsInitialized).Returns(false);

            this.forgeContextMock.SetupSequence(x => x.TryInitializeAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Temporary database error."))
                .ReturnsAsync(true);

            await retryOnExceptionService.StartAsync(CancellationToken.None);
            await retryOnExceptionService.ExecuteTask!;

            this.forgeContextMock.Verify(x => x.TryInitializeAsync(It.IsAny<CancellationToken>()), Times.Exactly(5));

            // Case 5: TryInitializeAsync throws an exception and service terminates upon cancellation.
            using var cancellationTokenSource = new CancellationTokenSource();
            using var failingService = new ForgeInitializationHostedService(this.forgeContextMock.Object, this.configuration, this.loggerMock.Object);

            this.forgeContextMock.Setup(x => x.IsInitialized).Returns(false);

            this.forgeContextMock.Setup(x => x.TryInitializeAsync(It.IsAny<CancellationToken>()))
                .Callback(cancellationTokenSource.Cancel)
                .ThrowsAsync(new InvalidOperationException("Simulated initialization failure."));

            await failingService.StartAsync(cancellationTokenSource.Token);
            await failingService.ExecuteTask!;

            this.loggerMock.Verify(x => x.Log(LogLevel.Warning, It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => true), It.IsAny<Exception?>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.AtLeastOnce);
        }
    }
}
