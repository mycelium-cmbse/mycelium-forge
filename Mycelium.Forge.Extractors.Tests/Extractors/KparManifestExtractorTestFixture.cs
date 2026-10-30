// ------------------------------------------------------------------------------------------------
// <copyright file="KparManifestExtractorTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Tests.Extractors
{
    using System.Text.Json;

    using ErrorOr;

    using Microsoft.Extensions.Logging.Abstractions;

    using Moq;

    using Mycelium.Forge.Extractors.Enums;
    using Mycelium.Forge.Extractors.Errors;
    using Mycelium.Forge.Extractors.Extractors;
    using Mycelium.Forge.Extractors.Models.Kpar;
    using Mycelium.Forge.Extractors.Readers;

    using NUnit.Framework;

    using SysML2.NET.Kpar;
    using SysML2.NET.Kpar.Cryptography;
    using SysML2.NET.ModelInterchange;

    using File = File;

    [TestFixture]
    public class KparManifestExtractorTestFixture
    {
        private const string SampleArchiveName = "Kernel_Data_Type_Library-1.1.0-dev.20260801.kpar";
        private Mock<IKparArchiveReader> readerMock;
        private KparManifestExtractor extractor;

        [SetUp]
        public void SetUp()
        {
            this.readerMock = new Mock<IKparArchiveReader>();
            this.extractor = new KparManifestExtractor(this.readerMock.Object);
        }

        [Test]
        public void VerifyCanHandle()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.extractor.CanHandle(ArtifactKind.Kpar), Is.True);
                Assert.That(this.extractor.CanHandle(ArtifactKind.EcssC3), Is.False);
                Assert.That(this.extractor.CanHandle(ArtifactKind.Capella), Is.False);
            }
        }

        [Test]
        public void VerifyExtract()
        {
            var createdDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var validProject = new InterchangeProject
            {
                Name = "TestProject",
                Description = "Test description",
                Version = "1.0.0",
                License = "Apache-2.0",
                Website = "https://example.com",
                Maintainer = ["Jane Doe"],
                Topic = ["SysML"],
                Usage = [new InterchangeProjectUsage { Resource = new Uri("https://example.com/other.kpar"), VersionConstraint = "^1.0.0" }]
            };

            var validMetadata = new InterchangeProjectMetadata
            {
                Metamodel = new Uri("https://example.com/meta"),
                Created = createdDate,
                Index = new Dictionary<string, string> { { "Pkg", "model.sysml" } },
                Checksum = new Dictionary<string, InterchangeChecksum> { { "model.sysml", new InterchangeChecksum { Value = "deadbeef" } } }
            };

            var validArchive = new KparArchive(new Archive { Project = validProject, Metadata = validMetadata });
            this.readerMock.Setup(reader => reader.Read(It.IsAny<Stream>())).Returns(validArchive);

            using var sampleStream = new MemoryStream();
            var successResult = this.extractor.Extract(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(successResult.IsError, Is.False);
                Assert.That(successResult.Value.Kind, Is.EqualTo(ArtifactKind.Kpar));

                var manifest = (KparManifest)successResult.Value;
                Assert.That(manifest.Name, Is.EqualTo("TestProject"));
                Assert.That(manifest.Description, Is.EqualTo("Test description"));
                Assert.That(manifest.Version, Is.EqualTo("1.0.0"));
                Assert.That(manifest.License, Is.EqualTo("Apache-2.0"));
                Assert.That(manifest.Website, Is.EqualTo("https://example.com"));
                Assert.That(manifest.Maintainers, Has.Count.EqualTo(1));
                Assert.That(manifest.Maintainers[0], Is.EqualTo("Jane Doe"));
                Assert.That(manifest.Topics, Has.Count.EqualTo(1));
                Assert.That(manifest.Topics[0], Is.EqualTo("SysML"));
                Assert.That(manifest.Metamodel, Is.EqualTo("https://example.com/meta"));
                Assert.That(manifest.Created, Is.EqualTo(new DateTimeOffset(createdDate)));
                Assert.That(manifest.ModelPaths, Has.Count.EqualTo(1));
                Assert.That(manifest.Dependencies, Has.Count.EqualTo(1));
                Assert.That(manifest.Dependencies[0].Resource, Is.EqualTo("https://example.com/other.kpar"));
                Assert.That(manifest.Dependencies[0].VersionConstraint, Is.EqualTo("^1.0.0"));
                Assert.That(manifest.Checksums, Has.Count.EqualTo(1));
            }

            this.readerMock.Setup(reader => reader.Read(It.IsAny<Stream>())).Throws(new InvalidDataException("Corrupted zip."));
            var invalidArchiveResult = this.extractor.Extract(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(invalidArchiveResult.IsError, Is.True);
                Assert.That(invalidArchiveResult.FirstError.Code, Is.EqualTo(KparErrorCodes.InvalidArchive));
            }

            this.readerMock.Setup(reader => reader.Read(It.IsAny<Stream>())).Throws(new JsonException("Corrupted JSON."));
            var invalidDescriptorResult = this.extractor.Extract(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(invalidDescriptorResult.IsError, Is.True);
                Assert.That(invalidDescriptorResult.FirstError.Code, Is.EqualTo(KparErrorCodes.InvalidDescriptor));
            }

            this.readerMock.Setup(reader => reader.Read(It.IsAny<Stream>())).Throws(new InvalidOperationException("Unexpected failure."));
            var unexpectedResult = this.extractor.Extract(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(unexpectedResult.IsError, Is.True);
                Assert.That(unexpectedResult.FirstError.Type, Is.EqualTo(ErrorType.Unexpected));
                Assert.That(unexpectedResult.FirstError.Code, Is.EqualTo(KparErrorCodes.UnexpectedError));
            }

            var missingNameProject = new InterchangeProject { Name = null, Version = "1.0.0" };
            this.readerMock.Setup(reader => reader.Read(It.IsAny<Stream>())).Returns(new KparArchive(new Archive { Project = missingNameProject, Metadata = validMetadata }));
            var missingNameResult = this.extractor.Extract(sampleStream);
            Assert.That(missingNameResult.FirstError.Code, Is.EqualTo(KparErrorCodes.MissingName));

            var missingVersionProject = new InterchangeProject { Name = "TestProject", Version = null };
            this.readerMock.Setup(reader => reader.Read(It.IsAny<Stream>())).Returns(new KparArchive(new Archive { Project = missingVersionProject, Metadata = validMetadata }));
            var missingVersionResult = this.extractor.Extract(sampleStream);
            Assert.That(missingVersionResult.FirstError.Code, Is.EqualTo(KparErrorCodes.MissingVersion));

            var invalidVersionProject = new InterchangeProject { Name = "TestProject", Version = "not-semver" };
            this.readerMock.Setup(reader => reader.Read(It.IsAny<Stream>())).Returns(new KparArchive(new Archive { Project = invalidVersionProject, Metadata = validMetadata }));
            var invalidVersionResult = this.extractor.Extract(sampleStream);
            Assert.That(invalidVersionResult.FirstError.Code, Is.EqualTo(KparErrorCodes.InvalidVersion));

            var missingUsageResourceProject = new InterchangeProject
            {
                Name = "TestProject",
                Version = "1.0.0",
                Usage = [new InterchangeProjectUsage { Resource = null }]
            };

            this.readerMock.Setup(reader => reader.Read(It.IsAny<Stream>())).Returns(new KparArchive(new Archive { Project = missingUsageResourceProject, Metadata = validMetadata }));
            var missingUsageResult = this.extractor.Extract(sampleStream);
            Assert.That(missingUsageResult.FirstError.Code, Is.EqualTo(KparErrorCodes.MissingUsageResource));

            var missingCreatedMetadata = new InterchangeProjectMetadata
            {
                Created = default,
                Index = new Dictionary<string, string> { { "Pkg", "model.sysml" } }
            };

            this.readerMock.Setup(reader => reader.Read(It.IsAny<Stream>())).Returns(new KparArchive(new Archive { Project = validProject, Metadata = missingCreatedMetadata }));
            var missingCreatedResult = this.extractor.Extract(sampleStream);
            Assert.That(missingCreatedResult.FirstError.Code, Is.EqualTo(KparErrorCodes.MissingCreated));

            var emptyIndexMetadata = new InterchangeProjectMetadata
            {
                Created = createdDate,
                Index = new Dictionary<string, string>()
            };

            this.readerMock.Setup(reader => reader.Read(It.IsAny<Stream>())).Returns(new KparArchive(new Archive { Project = validProject, Metadata = emptyIndexMetadata }));
            var emptyIndexResult = this.extractor.Extract(sampleStream);
            Assert.That(emptyIndexResult.FirstError.Code, Is.EqualTo(KparErrorCodes.NoModels));

            var duplicatePathMetadata = new InterchangeProjectMetadata
            {
                Created = createdDate,
                Index = new Dictionary<string, string> { { "PkgA", "model.sysml" }, { "PkgB", "model.sysml" } }
            };

            this.readerMock.Setup(reader => reader.Read(It.IsAny<Stream>())).Returns(new KparArchive(new Archive { Project = validProject, Metadata = duplicatePathMetadata }));
            var duplicatePathResult = this.extractor.Extract(sampleStream);
            Assert.That(duplicatePathResult.FirstError.Code, Is.EqualTo(KparErrorCodes.DuplicateModelPath));

            var checksumMismatchArchive = new Archive
            {
                Project = validProject,
                Metadata = validMetadata,
                ChecksumMismatches = [new ChecksumMismatch { Path = "model.sysml", Expected = "deadbeef", Actual = "badbeef" }]
            };

            this.readerMock.Setup(reader => reader.Read(It.IsAny<Stream>())).Returns(new KparArchive(checksumMismatchArchive));
            var checksumMismatchResult = this.extractor.Extract(sampleStream);
            Assert.That(checksumMismatchResult.FirstError.Code, Is.EqualTo(KparErrorCodes.ChecksumMismatch));

            var multipleErrorsProject = new InterchangeProject { Name = null, Version = "invalid" };
            var multipleErrorsMetadata = new InterchangeProjectMetadata { Created = default, Index = new Dictionary<string, string>() };
            this.readerMock.Setup(reader => reader.Read(It.IsAny<Stream>())).Returns(new KparArchive(new Archive { Project = multipleErrorsProject, Metadata = multipleErrorsMetadata }));
            var multipleErrorsResult = this.extractor.Extract(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(multipleErrorsResult.IsError, Is.True);
                Assert.That(multipleErrorsResult.Errors, Has.Count.EqualTo(4));
            }
        }

        [Test]
        public void VerifyExtractWithSampleArchive()
        {
            var samplePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources", SampleArchiveName);
            using var sampleStream = File.OpenRead(samplePath);

            var checksumService = new ChecksumService();
            var sysmlReader = new Reader(checksumService, NullLogger<Reader>.Instance);
            var archiveReader = new KparArchiveReader(sysmlReader);
            var manifestExtractor = new KparManifestExtractor(archiveReader);

            var result = manifestExtractor.Extract(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsError, Is.False);
                Assert.That(result.Value.Kind, Is.EqualTo(ArtifactKind.Kpar));

                var manifest = (KparManifest)result.Value;
                Assert.That(manifest.Name, Is.EqualTo("Kernel Data Type Library"));
                Assert.That(manifest.Version, Is.EqualTo("1.1.0-dev.20260801"));
                Assert.That(manifest.Metamodel, Is.EqualTo("https://www.omg.org/spec/KerML/20250201"));
                Assert.That(manifest.ModelPaths, Has.Count.EqualTo(3));
                Assert.That(manifest.Dependencies, Has.Count.EqualTo(1));
                Assert.That(manifest.Dependencies[0].Resource, Is.EqualTo("https://www.omg.org/spec/KerML/20250201/Semantic-Library.kpar"));
                Assert.That(manifest.Checksums, Has.Count.EqualTo(3));
            }
        }
    }
}
