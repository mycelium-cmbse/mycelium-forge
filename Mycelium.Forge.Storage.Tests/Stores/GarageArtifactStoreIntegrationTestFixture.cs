// ------------------------------------------------------------------------------------------------
// <copyright file="GarageArtifactStoreIntegrationTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Storage.Tests.Stores
{
    using System.Security.Cryptography;

    using ErrorOr;

    using Mycelium.Forge.Storage.Tests.Helper;

    using NUnit.Framework;

    [TestFixture]
    [Category("Storage")]
    public class GarageArtifactStoreIntegrationTestFixture : BaseGarageIntegrationTestFixture
    {
        private string testContentHash;
        private byte[] testPayloadBytes;

        [SetUp]
        public void SetUp()
        {
            this.testPayloadBytes = [.. "Garage Artifact Storage Verification Payload for Mycelium Forge A-03"u8];
            this.testContentHash = Convert.ToHexStringLower(SHA256.HashData(this.testPayloadBytes));
        }

        [TearDown]
        public async Task TearDown()
        {
            if (this.ArtifactStore != null && !string.IsNullOrEmpty(this.testContentHash))
            {
                await this.ArtifactStore.DeleteArtifactAsync(this.testContentHash, CancellationToken.None);
            }
        }

        [Test]
        public async Task VerifyDeleteArtifactAsync()
        {
            var deletePayload = "Garage Payload to Delete"u8.ToArray();
            var deleteHash = Convert.ToHexStringLower(SHA256.HashData(deletePayload));

            using var uploadStream = new MemoryStream(deletePayload);
            await this.ArtifactStore.StoreArtifactAsync(deleteHash, uploadStream, deletePayload.Length, "application/octet-stream", CancellationToken.None);

            var deleteResult = await this.ArtifactStore.DeleteArtifactAsync(deleteHash, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deleteResult.IsError, Is.False);
                Assert.That(deleteResult.Value, Is.EqualTo(Result.Deleted));
            }

            var postDeleteExists = await this.ArtifactStore.ExistsAsync(deleteHash, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(postDeleteExists.IsError, Is.False);
                Assert.That(postDeleteExists.Value, Is.False);
            }

            var emptyResult = await this.ArtifactStore.DeleteArtifactAsync(string.Empty, CancellationToken.None);
            Assert.That(emptyResult.IsError, Is.True);
        }

        [Test]
        public async Task VerifyExistsAsync()
        {
            using var uploadStream = new MemoryStream(this.testPayloadBytes);
            await this.ArtifactStore.StoreArtifactAsync(this.testContentHash, uploadStream, this.testPayloadBytes.Length, "application/octet-stream", CancellationToken.None);

            var existsResult = await this.ArtifactStore.ExistsAsync(this.testContentHash, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(existsResult.IsError, Is.False);
                Assert.That(existsResult.Value, Is.True);
            }

            var missingResult = await this.ArtifactStore.ExistsAsync("1111111111111111111111111111111111111111111111111111111111111111", CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(missingResult.IsError, Is.False);
                Assert.That(missingResult.Value, Is.False);
            }

            var emptyResult = await this.ArtifactStore.ExistsAsync(string.Empty, CancellationToken.None);
            Assert.That(emptyResult.IsError, Is.True);
        }

        [Test]
        public async Task VerifyGetArtifactAsync()
        {
            using var uploadStream = new MemoryStream(this.testPayloadBytes);
            await this.ArtifactStore.StoreArtifactAsync(this.testContentHash, uploadStream, this.testPayloadBytes.Length, "application/octet-stream", CancellationToken.None);

            var getResult = await this.ArtifactStore.GetArtifactAsync(this.testContentHash, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(getResult.IsError, Is.False);
                Assert.That(getResult.Value, Is.Not.Null);
            }

            using var memoryStream = new MemoryStream();
            await getResult.Value.CopyToAsync(memoryStream);
            var downloadedBytes = memoryStream.ToArray();
            Assert.That(downloadedBytes, Is.EqualTo(this.testPayloadBytes));
            await getResult.Value.DisposeAsync();

            var missingResult = await this.ArtifactStore.GetArtifactAsync("0000000000000000000000000000000000000000000000000000000000000000", CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(missingResult.IsError, Is.True);
                Assert.That(missingResult.FirstError.Type, Is.EqualTo(ErrorType.NotFound));
            }

            var emptyResult = await this.ArtifactStore.GetArtifactAsync(string.Empty, CancellationToken.None);
            Assert.That(emptyResult.IsError, Is.True);
        }

        [Test]
        public async Task VerifyListArtifactHashesAsync()
        {
            using var uploadStream = new MemoryStream(this.testPayloadBytes);
            await this.ArtifactStore.StoreArtifactAsync(this.testContentHash, uploadStream, this.testPayloadBytes.Length, "application/octet-stream", CancellationToken.None);

            var listResult = await this.ArtifactStore.ListArtifactHashesAsync(null, 100, CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(listResult.IsError, Is.False);
                Assert.That(listResult.Value.Hashes, Does.Contain(this.testContentHash));
            }
        }

        [Test]
        public async Task VerifyStoreArtifactAsync()
        {
            using var uploadStream = new MemoryStream(this.testPayloadBytes);
            var storeResult = await this.ArtifactStore.StoreArtifactAsync(this.testContentHash, uploadStream, this.testPayloadBytes.Length, "application/octet-stream", CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(storeResult.IsError, Is.False);
                Assert.That(storeResult.Value, Is.EqualTo(this.testContentHash));
            }

            using var duplicateStream = new MemoryStream(this.testPayloadBytes);
            var deduplicatedResult = await this.ArtifactStore.StoreArtifactAsync(this.testContentHash, duplicateStream, this.testPayloadBytes.Length, "application/octet-stream", CancellationToken.None);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deduplicatedResult.IsError, Is.False);
                Assert.That(deduplicatedResult.Value, Is.EqualTo(this.testContentHash));
            }

            var emptyResult = await this.ArtifactStore.StoreArtifactAsync(string.Empty, new MemoryStream(), 0, null, CancellationToken.None);
            Assert.That(emptyResult.IsError, Is.True);
        }
    }
}
