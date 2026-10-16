// ------------------------------------------------------------------------------------------------
// <copyright file="FileUploadStagingServiceTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Services
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using Moq;

    using Mycelium.Forge.Services;

    [TestFixture]
    public class FileUploadStagingServiceTestFixture
    {
        private Mock<ILogger<FileUploadStagingService>> loggerMock;
        private FileUploadStagingService stagingService;

        [SetUp]
        public void SetUp()
        {
            this.loggerMock = new Mock<ILogger<FileUploadStagingService>>();
            this.stagingService = new FileUploadStagingService(this.loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            this.stagingService.Dispose();
        }

        [Test]
        public async Task VerifyCleanup()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => this.stagingService.Cleanup(null), Throws.Nothing);
                Assert.That(() => this.stagingService.Cleanup(string.Empty), Throws.Nothing);
                Assert.That(() => this.stagingService.Cleanup("nonexistent-token"), Throws.Nothing);
            }

            var payload = new byte[] { 7, 8, 9 };
            using var inputStream = new MemoryStream(payload);
            var token = await this.stagingService.StageAsync(inputStream);

            this.stagingService.Cleanup(token);

            Assert.That(() => this.stagingService.OpenStagedFile(token), Throws.TypeOf<FileNotFoundException>());
        }

        [Test]
        public async Task VerifyDispose()
        {
            var localService = new FileUploadStagingService(this.loggerMock.Object);

            var payload = new byte[] { 10, 20, 30 };
            using var inputStream = new MemoryStream(payload);
            var token = await localService.StageAsync(inputStream);

            localService.Dispose();

            Assert.That(() => localService.OpenStagedFile(token), Throws.TypeOf<FileNotFoundException>());
        }

        [Test]
        public async Task VerifyOpenStagedFile()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => this.stagingService.OpenStagedFile(null), Throws.TypeOf<FileNotFoundException>());
                Assert.That(() => this.stagingService.OpenStagedFile(string.Empty), Throws.TypeOf<FileNotFoundException>());
                Assert.That(() => this.stagingService.OpenStagedFile("nonexistent-token"), Throws.TypeOf<FileNotFoundException>());
            }

            var payload = "*+,"u8.ToArray();
            using var inputStream = new MemoryStream(payload);
            var token = await this.stagingService.StageAsync(inputStream);

            var stagedStream = this.stagingService.OpenStagedFile(token);
            using var memoryStream = new MemoryStream();
            await stagedStream.CopyToAsync(memoryStream);
            await stagedStream.DisposeAsync();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => this.stagingService.OpenStagedFile(token), Throws.TypeOf<FileNotFoundException>());
                Assert.That(memoryStream.ToArray(), Is.EqualTo(payload));
            }
        }

        [Test]
        public void VerifyPackageUploadStagingService()
        {
            var stagingDirectory = Path.Combine(Path.GetTempPath(), "mycelium_forge_staged_uploads");
            Directory.CreateDirectory(stagingDirectory);
            var orphanFilePath = Path.Combine(stagingDirectory, "orphan_test.tmp");
            File.WriteAllText(orphanFilePath, "leftover content");

            using var service = new FileUploadStagingService(this.loggerMock.Object);

            Assert.That(File.Exists(orphanFilePath), Is.False);
        }

        [Test]
        public async Task VerifyStageAsync()
        {
            Assert.That(async () => await this.stagingService.StageAsync(null), Throws.TypeOf<ArgumentNullException>());

            var payload = new byte[] { 1, 2, 3, 4, 5 };
            using var inputStream = new MemoryStream(payload);
            var uploadToken = await this.stagingService.StageAsync(inputStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(uploadToken, Is.Not.Null);
                Assert.That(uploadToken, Is.Not.Empty);
            }

            using var cancelledCts = new CancellationTokenSource();
            await cancelledCts.CancelAsync();
            var token = cancelledCts.Token;

            Assert.That(async () =>
            {
                using var stream = new MemoryStream(payload);
                await this.stagingService.StageAsync(stream, token);
            }, Throws.InstanceOf<OperationCanceledException>());
        }
    }
}
