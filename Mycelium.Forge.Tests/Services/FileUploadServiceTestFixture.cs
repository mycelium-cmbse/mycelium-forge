// ------------------------------------------------------------------------------------------------
// <copyright file="FileUploadServiceTestFixture.cs" company="Starion Group S.A.">
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

    using ErrorOr;

    using Microsoft.AspNetCore.Components.Forms;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using Moq;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Services;
    using Mycelium.Forge.Storage.Stores;

    [TestFixture]
    public class FileUploadServiceTestFixture
    {
        private Mock<IJsInterop> jsInteropMock;
        private Mock<IFileUploadStagingService> uploadStagingServiceMock;
        private Mock<IArtifactStore> artifactStoreMock;
        private Mock<ILogger<FileUploadService>> loggerMock;
        private IConfiguration configuration;
        private FileUploadService fileUploadService;

        [SetUp]
        public void SetUp()
        {
            this.jsInteropMock = new Mock<IJsInterop>();
            this.uploadStagingServiceMock = new Mock<IFileUploadStagingService>();
            this.artifactStoreMock = new Mock<IArtifactStore>();
            this.loggerMock = new Mock<ILogger<FileUploadService>>();
            this.configuration = new ConfigurationBuilder().Build();

            this.fileUploadService = new FileUploadService(this.jsInteropMock.Object, this.uploadStagingServiceMock.Object, this.artifactStoreMock.Object, this.configuration, this.loggerMock.Object);
        }

        [Test]
        public async Task VerifyUploadFile()
        {
            // Case 1: Null file
            var nullFileResult = await this.fileUploadService.UploadFile(null);
            Assert.That(nullFileResult.IsError, Is.True);

            // Case 2: Zero size file
            var zeroByteFileMock = new Mock<IBrowserFile>();
            zeroByteFileMock.Setup(x => x.Size).Returns(0);
            var zeroByteResult = await this.fileUploadService.UploadFile(zeroByteFileMock.Object);
            Assert.That(zeroByteResult.IsError, Is.True);

            // Case 3: Artifact store error
            var validFileMock = new Mock<IBrowserFile>();
            validFileMock.Setup(x => x.Size).Returns(1024);
            validFileMock.Setup(x => x.ContentType).Returns("application/zip");

            validFileMock.Setup(x => x.OpenReadStream(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .Returns(() => new MemoryStream(new byte[1024]));

            var failureError = Error.Failure(description: "Storage failure.");

            this.artifactStoreMock
                .Setup(x => x.StoreArtifactAsync(It.IsAny<Stream>(), "application/zip", It.IsAny<CancellationToken>()))
                .ReturnsAsync(failureError);

            var failureResult = await this.fileUploadService.UploadFile(validFileMock.Object, "application/zip");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(failureResult.IsError, Is.True);
                Assert.That(failureResult.FirstError.Description, Is.EqualTo("Storage failure."));
            }

            // Case 4: Exception thrown during read
            var throwingFileMock = new Mock<IBrowserFile>();
            throwingFileMock.Setup(x => x.Size).Returns(1024);
            throwingFileMock.Setup(x => x.ContentType).Returns("application/zip");

            throwingFileMock.Setup(x => x.OpenReadStream(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .Throws(new InvalidOperationException("Stream read failed."));

            var exceptionResult = await this.fileUploadService.UploadFile(throwingFileMock.Object, "application/zip");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(exceptionResult.IsError, Is.True);
                Assert.That(exceptionResult.FirstError.Description, Does.Contain("Stream read failed."));
            }

            // Case 5: Successful upload
            this.artifactStoreMock
                .Setup(x => x.StoreArtifactAsync(It.IsAny<Stream>(), "application/zip", It.IsAny<CancellationToken>()))
                .ReturnsAsync("sha256-hash-value");

            var successResult = await this.fileUploadService.UploadFile(validFileMock.Object, "application/zip");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(successResult.IsError, Is.False);
                Assert.That(successResult.Value, Is.EqualTo("sha256-hash-value"));
            }
        }

        [Test]
        public async Task VerifyUploadFileOverHttps()
        {
            // Case 1: Empty inputElementId
            var emptyInputResult = await this.fileUploadService.UploadFileOverHttps(string.Empty);
            Assert.That(emptyInputResult.IsError, Is.True);

            // Case 2: JsInterop throws exception
            this.jsInteropMock
                .Setup(x => x.UploadLargeFile("upload-input", ApiRoutes.UploadFileForStage, "application/octet-stream"))
                .ThrowsAsync(new InvalidOperationException("Browser upload failed."));

            var jsExceptionResult = await this.fileUploadService.UploadFileOverHttps("upload-input");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(jsExceptionResult.IsError, Is.True);
                Assert.That(jsExceptionResult.FirstError.Description, Does.Contain("Browser upload failed."));
            }

            // Case 3: Staging service throws exception
            this.jsInteropMock
                .Setup(x => x.UploadLargeFile("upload-input", ApiRoutes.UploadFileForStage, "application/octet-stream"))
                .ReturnsAsync("valid-upload-token");

            this.uploadStagingServiceMock
                .Setup(x => x.OpenStagedFile("valid-upload-token"))
                .Throws(new FileNotFoundException("Staged file missing."));

            var stagingExceptionResult = await this.fileUploadService.UploadFileOverHttps("upload-input");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(stagingExceptionResult.IsError, Is.True);
                Assert.That(stagingExceptionResult.FirstError.Description, Does.Contain("Staged file missing."));
                this.uploadStagingServiceMock.Verify(x => x.Cleanup("valid-upload-token"), Times.Once);
            }

            // Case 4: Artifact store returns error
            this.uploadStagingServiceMock
                .Setup(x => x.OpenStagedFile("valid-upload-token"))
                .Returns(new MemoryStream(new byte[512]));

            var storeError = Error.Failure(description: "Artifact persistence failed.");

            this.artifactStoreMock
                .Setup(x => x.StoreArtifactAsync(It.IsAny<Stream>(), "application/octet-stream", It.IsAny<CancellationToken>()))
                .ReturnsAsync(storeError);

            var errorResult = await this.fileUploadService.UploadFileOverHttps("upload-input");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(errorResult.IsError, Is.True);
                Assert.That(errorResult.FirstError.Description, Is.EqualTo("Artifact persistence failed."));
            }

            // Case 5: Successful upload
            this.artifactStoreMock
                .Setup(x => x.StoreArtifactAsync(It.IsAny<Stream>(), "application/octet-stream", It.IsAny<CancellationToken>()))
                .ReturnsAsync("sha256-stored-hash");

            var successResult = await this.fileUploadService.UploadFileOverHttps("upload-input");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(successResult.IsError, Is.False);
                Assert.That(successResult.Value, Is.EqualTo("sha256-stored-hash"));
            }
        }
    }
}
