// ------------------------------------------------------------------------------------------------
// <copyright file="ArtifactStoreTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Storage.Tests.Stores
{
    using System.Net;
    using System.Security.Cryptography;

    using Amazon.S3;
    using Amazon.S3.Model;

    using ErrorOr;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Moq;

    using Mycelium.Forge.Storage.Models;
    using Mycelium.Forge.Storage.Stores;

    using NUnit.Framework;

    [TestFixture]
    public class ArtifactStoreTestFixture
    {
        private Mock<IAmazonS3> amazonS3Mock;
        private Mock<ILogger<ArtifactStore>> loggerMock;
        private IOptions<ArtifactStoreOptions> options;
        private ArtifactStore artifactStore;

        [SetUp]
        public void SetUp()
        {
            this.amazonS3Mock = new Mock<IAmazonS3>(MockBehavior.Strict);
            this.loggerMock = new Mock<ILogger<ArtifactStore>>();

            this.options = Options.Create(new ArtifactStoreOptions
            {
                BucketName = "test-bucket",
                ServiceUrl = "http://localhost:3900",
                Region = "garage",
                AccessKey = "test-access",
                SecretKey = "test-secret",
                ForcePathStyle = true
            });

            this.artifactStore = new ArtifactStore(this.amazonS3Mock.Object, this.options, this.loggerMock.Object);
        }

        [Test]
        public async Task VerifyDeleteArtifactAsync()
        {
            var emptyResult = await this.artifactStore.DeleteArtifactAsync(string.Empty, CancellationToken.None);
            Assert.That(emptyResult.IsError, Is.True);

            this.amazonS3Mock.Setup(x => x.DeleteObjectAsync(It.Is<DeleteObjectRequest>(r => r.BucketName == "test-bucket" && r.Key == "hash-to-delete"), It.IsAny<CancellationToken>())).ReturnsAsync(new DeleteObjectResponse());

            var deleteResult = await this.artifactStore.DeleteArtifactAsync("hash-to-delete", CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deleteResult.IsError, Is.False);
                Assert.That(deleteResult.Value, Is.EqualTo(Result.Deleted));
            }

            this.amazonS3Mock.Setup(x => x.DeleteObjectAsync(It.Is<DeleteObjectRequest>(r => r.Key == "hash-error"), It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException("Failed"));

            var errorResult = await this.artifactStore.DeleteArtifactAsync("hash-error", CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(errorResult.IsError, Is.True);
                Assert.That(errorResult.FirstError.Type, Is.EqualTo(ErrorType.Failure));
            }
        }

        [Test]
        public async Task VerifyExistsAsync()
        {
            var emptyResult = await this.artifactStore.ExistsAsync(string.Empty, CancellationToken.None);
            Assert.That(emptyResult.IsError, Is.True);

            this.amazonS3Mock.Setup(x => x.GetObjectMetadataAsync(It.Is<GetObjectMetadataRequest>(r => r.Key == "present-hash"), It.IsAny<CancellationToken>())).ReturnsAsync(new GetObjectMetadataResponse());

            var presentResult = await this.artifactStore.ExistsAsync("present-hash", CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(presentResult.IsError, Is.False);
                Assert.That(presentResult.Value, Is.True);
            }

            this.amazonS3Mock.Setup(x => x.GetObjectMetadataAsync(It.Is<GetObjectMetadataRequest>(r => r.Key == "absent-hash"), It.IsAny<CancellationToken>())).ThrowsAsync(new AmazonS3Exception("NotFound") { StatusCode = HttpStatusCode.NotFound });

            var absentResult = await this.artifactStore.ExistsAsync("absent-hash", CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(absentResult.IsError, Is.False);
                Assert.That(absentResult.Value, Is.False);
            }

            this.amazonS3Mock.Setup(x => x.GetObjectMetadataAsync(It.Is<GetObjectMetadataRequest>(r => r.Key == "no-bucket-hash"), It.IsAny<CancellationToken>())).ThrowsAsync(new AmazonS3Exception("NoSuchBucket") { StatusCode = HttpStatusCode.NotFound });

            var noBucketResult = await this.artifactStore.ExistsAsync("no-bucket-hash", CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(noBucketResult.IsError, Is.False);
                Assert.That(noBucketResult.Value, Is.False);
            }

            this.amazonS3Mock.Setup(x => x.GetObjectMetadataAsync(It.Is<GetObjectMetadataRequest>(r => r.Key == "faulty-hash"), It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException("Network error"));

            var faultyResult = await this.artifactStore.ExistsAsync("faulty-hash", CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(faultyResult.IsError, Is.True);
                Assert.That(faultyResult.FirstError.Type, Is.EqualTo(ErrorType.Failure));
            }
        }

        [Test]
        public async Task VerifyGetArtifactAsync()
        {
            var emptyResult = await this.artifactStore.GetArtifactAsync(string.Empty, CancellationToken.None);
            Assert.That(emptyResult.IsError, Is.True);

            var sampleBytes = "package artifact bytes"u8.ToArray();
            var sampleHash = Convert.ToHexStringLower(SHA256.HashData(sampleBytes));

            var getObjectResponse = new GetObjectResponse
            {
                ResponseStream = new MemoryStream(sampleBytes),
                ContentLength = sampleBytes.Length
            };

            this.amazonS3Mock.Setup(x => x.GetObjectAsync(It.Is<GetObjectRequest>(r => r.BucketName == "test-bucket" && r.Key == sampleHash), It.IsAny<CancellationToken>())).ReturnsAsync(getObjectResponse);

            var happyResult = await this.artifactStore.GetArtifactAsync(sampleHash, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(happyResult.IsError, Is.False);
                Assert.That(happyResult.Value, Is.Not.Null);
            }

            await happyResult.Value.DisposeAsync();

            this.amazonS3Mock.Setup(x => x.GetObjectAsync(It.Is<GetObjectRequest>(r => r.Key == "missing-hash"), It.IsAny<CancellationToken>())).ThrowsAsync(new AmazonS3Exception("Not Found") { StatusCode = HttpStatusCode.NotFound });

            var notFoundResult = await this.artifactStore.GetArtifactAsync("missing-hash", CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(notFoundResult.IsError, Is.True);
                Assert.That(notFoundResult.FirstError.Type, Is.EqualTo(ErrorType.NotFound));
            }

            this.amazonS3Mock.Setup(x => x.GetObjectAsync(It.Is<GetObjectRequest>(r => r.Key == "no-bucket-get"), It.IsAny<CancellationToken>())).ThrowsAsync(new AmazonS3Exception("NoSuchBucket") { StatusCode = HttpStatusCode.NotFound });

            var noBucketGetResult = await this.artifactStore.GetArtifactAsync("no-bucket-get", CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(noBucketGetResult.IsError, Is.True);
                Assert.That(noBucketGetResult.FirstError.Type, Is.EqualTo(ErrorType.NotFound));
            }

            this.amazonS3Mock.Setup(x => x.GetObjectAsync(It.Is<GetObjectRequest>(r => r.Key == "error-hash"), It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException("Connection failed"));

            var failureResult = await this.artifactStore.GetArtifactAsync("error-hash", CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(failureResult.IsError, Is.True);
                Assert.That(failureResult.FirstError.Type, Is.EqualTo(ErrorType.Failure));
            }
        }

        [Test]
        public async Task VerifyListArtifactHashesAsync()
        {
            var listResponse = new ListObjectsV2Response
            {
                NextContinuationToken = "next-token"
            };

            listResponse.S3Objects.Add(new S3Object { Key = "hash-1" });
            listResponse.S3Objects.Add(new S3Object { Key = "hash-2" });

            this.amazonS3Mock.Setup(x => x.ListObjectsV2Async(It.Is<ListObjectsV2Request>(r => r.BucketName == "test-bucket" && r.MaxKeys == 10), It.IsAny<CancellationToken>())).ReturnsAsync(listResponse);

            var listResult = await this.artifactStore.ListArtifactHashesAsync(null, 10, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(listResult.IsError, Is.False);
                Assert.That(listResult.Value.Hashes, Has.Count.EqualTo(2));
                Assert.That(listResult.Value.Hashes[0], Is.EqualTo("hash-1"));
                Assert.That(listResult.Value.Hashes[1], Is.EqualTo("hash-2"));
                Assert.That(listResult.Value.NextContinuationToken, Is.EqualTo("next-token"));
            }

            this.amazonS3Mock.Setup(x => x.ListObjectsV2Async(It.Is<ListObjectsV2Request>(r => r.MaxKeys == 5), It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException("Failed"));

            var failureResult = await this.artifactStore.ListArtifactHashesAsync(null, 5, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(failureResult.IsError, Is.True);
                Assert.That(failureResult.FirstError.Type, Is.EqualTo(ErrorType.Failure));
            }
        }

        [Test]
        public async Task VerifyStoreArtifactAsync()
        {
            var nullStreamResult = await this.artifactStore.StoreArtifactAsync(null, null, CancellationToken.None);
            Assert.That(nullStreamResult.IsError, Is.True);

            var existingBytes = "any-data"u8.ToArray();
            var existingHash = Convert.ToHexStringLower(SHA256.HashData(existingBytes));
            this.amazonS3Mock.Setup(x => x.GetObjectMetadataAsync(It.Is<GetObjectMetadataRequest>(r => r.BucketName == "test-bucket" && r.Key == existingHash), It.IsAny<CancellationToken>())).ReturnsAsync(new GetObjectMetadataResponse());

            using var existingStream = new MemoryStream(existingBytes);
            var deduplicatedResult = await this.artifactStore.StoreArtifactAsync(existingStream, null, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deduplicatedResult.IsError, Is.False);
                Assert.That(deduplicatedResult.Value, Is.EqualTo(existingHash));
            }

            var bytes = "new-artifact-bytes"u8.ToArray();
            var expectedHash = Convert.ToHexStringLower(SHA256.HashData(bytes));

            this.amazonS3Mock.Setup(x => x.GetObjectMetadataAsync(It.Is<GetObjectMetadataRequest>(r => r.Key == expectedHash), It.IsAny<CancellationToken>())).ThrowsAsync(new AmazonS3Exception("Not Found") { StatusCode = HttpStatusCode.NotFound });
            this.amazonS3Mock.Setup(x => x.EnsureBucketExistsAsync("test-bucket")).Returns(Task.CompletedTask);
            this.amazonS3Mock.Setup(x => x.PutObjectAsync(It.Is<PutObjectRequest>(r => r.BucketName == "test-bucket" && r.Key == expectedHash), It.IsAny<CancellationToken>())).ReturnsAsync(new PutObjectResponse());

            using var uploadStream = new MemoryStream(bytes);
            var storeResult = await this.artifactStore.StoreArtifactAsync(uploadStream, "application/octet-stream", CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(storeResult.IsError, Is.False);
                Assert.That(storeResult.Value, Is.EqualTo(expectedHash));
            }

            var failBytes = "fail-bytes"u8.ToArray();
            var failHash = Convert.ToHexStringLower(SHA256.HashData(failBytes));
            this.amazonS3Mock.Setup(x => x.GetObjectMetadataAsync(It.Is<GetObjectMetadataRequest>(r => r.Key == failHash), It.IsAny<CancellationToken>())).ThrowsAsync(new AmazonS3Exception("Not Found") { StatusCode = HttpStatusCode.NotFound });
            this.amazonS3Mock.Setup(x => x.PutObjectAsync(It.Is<PutObjectRequest>(r => r.Key == failHash), It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException("Upload failed"));

            using var failStream = new MemoryStream(failBytes);
            var failureResult = await this.artifactStore.StoreArtifactAsync(failStream, null, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(failureResult.IsError, Is.True);
                Assert.That(failureResult.FirstError.Type, Is.EqualTo(ErrorType.Failure));
            }
        }
    }
}
