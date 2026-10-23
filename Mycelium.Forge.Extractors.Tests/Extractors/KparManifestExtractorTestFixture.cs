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
    using System.IO.Compression;

    using ErrorOr;

    using Microsoft.Extensions.Logging.Abstractions;

    using Moq;

    using Mycelium.Forge.Extractors.Enums;
    using Mycelium.Forge.Extractors.Errors;
    using Mycelium.Forge.Extractors.Extractors;
    using Mycelium.Forge.Extractors.Models.Kpar;
    using Mycelium.Forge.Extractors.Tests.Models;

    using NUnit.Framework;

    using SysML2.NET.Kpar;
    using SysML2.NET.Kpar.Cryptography;

    [TestFixture]
    public class KparManifestExtractorTestFixture
    {
        private const string SampleArchiveName = "Kernel_Data_Type_Library-1.1.0-dev.20260801.kpar";
        private const string ProjectFileName = ".project.json";
        private const string MetaFileName = ".meta.json";

        private KparManifestExtractor extractor;

        [SetUp]
        public void SetUp()
        {
            var checksumService = new ChecksumService();
            var reader = new Reader(checksumService, NullLogger<Reader>.Instance);
            this.extractor = new KparManifestExtractor(reader);
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
            var samplePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources", SampleArchiveName);
            using var sampleStream = File.OpenRead(samplePath);
            var sampleResult = this.extractor.Extract(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(sampleResult.IsError, Is.False);
                Assert.That(sampleResult.Value.Kind, Is.EqualTo(ArtifactKind.Kpar));

                var manifest = (KparManifest)sampleResult.Value;
                Assert.That(manifest.Name, Is.EqualTo("Kernel Data Type Library"));
                Assert.That(manifest.Version, Is.EqualTo("1.1.0-dev.20260801"));
                Assert.That(manifest.Metamodel, Is.EqualTo("https://www.omg.org/spec/KerML/20250201"));
                Assert.That(manifest.ModelPaths, Has.Count.EqualTo(3));
                Assert.That(manifest.Dependencies, Has.Count.EqualTo(1));
                Assert.That(manifest.Dependencies[0].Resource, Is.EqualTo("https://www.omg.org/spec/KerML/20250201/Semantic-Library.kpar"));
                Assert.That(manifest.Checksums, Has.Count.EqualTo(3));
            }

            using var emptyStream = new MemoryStream();
            var emptyResult = this.extractor.Extract(emptyStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(emptyResult.IsError, Is.True);
                Assert.That(emptyResult.FirstError.Code, Is.EqualTo(KparErrorCodes.InvalidArchive));
            }

            using var invalidZipStream = new MemoryStream([0x01, 0x02, 0x03, 0x04]);
            var invalidZipResult = this.extractor.Extract(invalidZipStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(invalidZipResult.IsError, Is.True);
                Assert.That(invalidZipResult.FirstError.Code, Is.EqualTo(KparErrorCodes.InvalidArchive));
            }

            const string validProject = "{\"name\":\"TestProject\",\"version\":\"1.0.0\",\"usage\":[{\"resource\":\"https://example.com/other.kpar\"}]}";
            const string validMeta = "{\"index\":{\"Pkg\":\"model.sysml\"},\"created\":\"2026-01-01T00:00:00Z\",\"metamodel\":\"https://example.com/meta\"}";

            using var missingProjectStream = CreateZipStream(new ZipEntryModel(MetaFileName, validMeta), new ZipEntryModel("model.sysml", "package Pkg;"));
            var missingProjectResult = this.extractor.Extract(missingProjectStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(missingProjectResult.IsError, Is.True);
                Assert.That(missingProjectResult.FirstError.Code, Is.EqualTo(KparErrorCodes.InvalidArchive));
            }

            using var missingMetaStream = CreateZipStream(new ZipEntryModel(ProjectFileName, validProject), new ZipEntryModel("model.sysml", "package Pkg;"));
            var missingMetaResult = this.extractor.Extract(missingMetaStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(missingMetaResult.IsError, Is.True);
                Assert.That(missingMetaResult.FirstError.Code, Is.EqualTo(KparErrorCodes.InvalidArchive));
            }

            using var duplicateProjectStream = CreateZipStream(new ZipEntryModel(ProjectFileName, validProject), new ZipEntryModel(ProjectFileName, validProject), new ZipEntryModel(MetaFileName, validMeta), new ZipEntryModel("model.sysml", "package Pkg;"));
            var duplicateProjectResult = this.extractor.Extract(duplicateProjectStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(duplicateProjectResult.IsError, Is.True);
                Assert.That(duplicateProjectResult.FirstError.Code, Is.EqualTo(KparErrorCodes.InvalidArchive));
            }

            using var malformedJsonStream = CreateZipStream(new ZipEntryModel(ProjectFileName, "{ malformed json"), new ZipEntryModel(MetaFileName, validMeta), new ZipEntryModel("model.sysml", "package Pkg;"));
            var malformedJsonResult = this.extractor.Extract(malformedJsonStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(malformedJsonResult.IsError, Is.True);
                Assert.That(malformedJsonResult.FirstError.Code, Is.EqualTo(KparErrorCodes.InvalidDescriptor));
            }

            using var missingNameStream = CreateZipStream(new ZipEntryModel(ProjectFileName, "{\"version\":\"1.0.0\"}"), new ZipEntryModel(MetaFileName, validMeta), new ZipEntryModel("model.sysml", "package Pkg;"));
            var missingNameResult = this.extractor.Extract(missingNameStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(missingNameResult.IsError, Is.True);
                Assert.That(missingNameResult.FirstError.Code, Is.EqualTo(KparErrorCodes.MissingName));
            }

            using var missingVersionStream = CreateZipStream(new ZipEntryModel(ProjectFileName, "{\"name\":\"TestProject\"}"), new ZipEntryModel(MetaFileName, validMeta), new ZipEntryModel("model.sysml", "package Pkg;"));
            var missingVersionResult = this.extractor.Extract(missingVersionStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(missingVersionResult.IsError, Is.True);
                Assert.That(missingVersionResult.FirstError.Code, Is.EqualTo(KparErrorCodes.MissingVersion));
            }

            using var invalidVersionStream = CreateZipStream(new ZipEntryModel(ProjectFileName, "{\"name\":\"TestProject\",\"version\":\"v1.0.0\"}"), new ZipEntryModel(MetaFileName, validMeta), new ZipEntryModel("model.sysml", "package Pkg;"));
            var invalidVersionResult = this.extractor.Extract(invalidVersionStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(invalidVersionResult.IsError, Is.True);
                Assert.That(invalidVersionResult.FirstError.Code, Is.EqualTo(KparErrorCodes.InvalidVersion));
            }

            using var missingCreatedStream = CreateZipStream(new ZipEntryModel(ProjectFileName, validProject), new ZipEntryModel(MetaFileName, "{\"index\":{\"Pkg\":\"model.sysml\"}}"), new ZipEntryModel("model.sysml", "package Pkg;"));
            var missingCreatedResult = this.extractor.Extract(missingCreatedStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(missingCreatedResult.IsError, Is.True);
                Assert.That(missingCreatedResult.FirstError.Code, Is.EqualTo(KparErrorCodes.MissingCreated));
            }

            using var emptyIndexStream = CreateZipStream(new ZipEntryModel(ProjectFileName, validProject), new ZipEntryModel(MetaFileName, "{\"index\":{},\"created\":\"2026-01-01T00:00:00Z\"}"), new ZipEntryModel("model.sysml", "package Pkg;"));
            var emptyIndexResult = this.extractor.Extract(emptyIndexStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(emptyIndexResult.IsError, Is.True);
                Assert.That(emptyIndexResult.FirstError.Code, Is.EqualTo(KparErrorCodes.NoModels));
            }

            using var duplicateModelPathStream = CreateZipStream(new ZipEntryModel(ProjectFileName, validProject), new ZipEntryModel(MetaFileName, "{\"index\":{\"PkgA\":\"model.sysml\",\"PkgB\":\"model.sysml\"},\"created\":\"2026-01-01T00:00:00Z\"}"), new ZipEntryModel("model.sysml", "package Pkg;"));
            var duplicateModelPathResult = this.extractor.Extract(duplicateModelPathStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(duplicateModelPathResult.IsError, Is.True);
                Assert.That(duplicateModelPathResult.FirstError.Code, Is.EqualTo(KparErrorCodes.DuplicateModelPath));
            }

            using var missingUsageResourceStream = CreateZipStream(new ZipEntryModel(ProjectFileName, "{\"name\":\"TestProject\",\"version\":\"1.0.0\",\"usage\":[{\"versionConstraint\":\"^1.0.0\"}]}"), new ZipEntryModel(MetaFileName, validMeta), new ZipEntryModel("model.sysml", "package Pkg;"));
            var missingUsageResourceResult = this.extractor.Extract(missingUsageResourceStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(missingUsageResourceResult.IsError, Is.True);
                Assert.That(missingUsageResourceResult.FirstError.Code, Is.EqualTo(KparErrorCodes.MissingUsageResource));
            }

            const string metaWithBadChecksum = "{\"index\":{\"Pkg\":\"model.sysml\"},\"created\":\"2026-01-01T00:00:00Z\",\"checksum\":{\"model.sysml\":{\"value\":\"deadbeef\",\"algorithm\":\"SHA256\"}}}";
            using var badChecksumStream = CreateZipStream(new ZipEntryModel(ProjectFileName, validProject), new ZipEntryModel(MetaFileName, metaWithBadChecksum), new ZipEntryModel("model.sysml", "package Pkg;"));
            var badChecksumResult = this.extractor.Extract(badChecksumStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(badChecksumResult.IsError, Is.True);
                Assert.That(badChecksumResult.FirstError.Code, Is.EqualTo(KparErrorCodes.ChecksumMismatch));
            }

            const string metaWithMissingModelFile = "{\"index\":{\"Pkg\":\"missing_model.sysml\"},\"created\":\"2026-01-01T00:00:00Z\"}";
            using var missingModelFileStream = CreateZipStream(new ZipEntryModel(ProjectFileName, validProject), new ZipEntryModel(MetaFileName, metaWithMissingModelFile));
            var missingModelFileResult = this.extractor.Extract(missingModelFileStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(missingModelFileResult.IsError, Is.True);
                Assert.That(missingModelFileResult.FirstError.Code, Is.EqualTo(KparErrorCodes.InvalidArchive));
            }

            const string multipleErrorsProject = "{\"version\":\"invalid-version\",\"usage\":[{\"versionConstraint\":\"1.0\"}]}";
            const string multipleErrorsMeta = "{\"index\":{}}";
            using var multipleErrorsStream = CreateZipStream(new ZipEntryModel(ProjectFileName, multipleErrorsProject), new ZipEntryModel(MetaFileName, multipleErrorsMeta));
            var multipleErrorsResult = this.extractor.Extract(multipleErrorsStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(multipleErrorsResult.IsError, Is.True);
                var codes = multipleErrorsResult.Errors.Select(error => error.Code).ToList();
                Assert.That(codes, Does.Contain(KparErrorCodes.MissingName));
                Assert.That(codes, Does.Contain(KparErrorCodes.InvalidVersion));
                Assert.That(codes, Does.Contain(KparErrorCodes.MissingUsageResource));
                Assert.That(codes, Does.Contain(KparErrorCodes.MissingCreated));
                Assert.That(codes, Does.Contain(KparErrorCodes.NoModels));
            }
        }

        [Test]
        public void VerifyExtractWhenReaderThrowsUnexpectedException()
        {
            var throwingReader = new Mock<IReader>();
            throwingReader.Setup(r => r.Read(It.IsAny<Stream>(), It.IsAny<ReadOptions>())).Throws(new InvalidOperationException("Simulated unexpected failure."));

            var extractorWithThrowingReader = new KparManifestExtractor(throwingReader.Object);
            using var stream = new MemoryStream();
            var result = extractorWithThrowingReader.Extract(stream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsError, Is.True);
                Assert.That(result.FirstError.Type, Is.EqualTo(ErrorType.Unexpected));
                Assert.That(result.FirstError.Code, Is.EqualTo(KparErrorCodes.UnexpectedError));
                Assert.That(result.FirstError.Description, Is.EqualTo("Simulated unexpected failure."));
            }
        }

        private static MemoryStream CreateZipStream(params ZipEntryModel[] entries)
        {
            var stream = new MemoryStream();

            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true))
            {
                foreach (var entry in entries)
                {
                    var zipEntry = archive.CreateEntry(entry.Name);
                    using var writer = new StreamWriter(zipEntry.Open());
                    writer.Write(entry.Content);
                }
            }

            stream.Position = 0;
            return stream;
        }
    }
}
