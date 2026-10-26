// ------------------------------------------------------------------------------------------------
// <copyright file="CapellaManifestExtractorTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Tests.Extractors
{
    using System.Xml;

    using Auriga.Capellamodeller;
    using Auriga.Libraries;

    using Moq;

    using Mycelium.Forge.Extractors.Enums;
    using Mycelium.Forge.Extractors.Errors;
    using Mycelium.Forge.Extractors.Extractors;
    using Mycelium.Forge.Extractors.Models.Capella;
    using Mycelium.Forge.Extractors.Readers;

    using NUnit.Framework;

    [TestFixture]
    public class CapellaManifestExtractorTestFixture
    {
        private const string SampleArchiveName = "coffee-machine-demo.zip";
        private Mock<ICapellaArchiveReader> readerMock;
        private CapellaManifestExtractor extractor;

        [SetUp]
        public void SetUp()
        {
            this.readerMock = new Mock<ICapellaArchiveReader>();
            this.extractor = new CapellaManifestExtractor(this.readerMock.Object);
        }

        [Test]
        public void VerifyCanHandle()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.extractor.CanHandle(ArtifactKind.Capella), Is.True);
                Assert.That(this.extractor.CanHandle(ArtifactKind.Kpar), Is.False);
                Assert.That(this.extractor.CanHandle(ArtifactKind.EcssC3), Is.False);
            }
        }

        [Test]
        public void VerifyExtract()
        {
            var project = new Project
            {
                Id = "proj-1",
                Name = "Capella System Model",
                Summary = "Summary of model",
                Description = "Description of model"
            };

            var modelInformation = new ModelInformation
            {
                Id = "info-1",
                Version = new ModelVersion
                {
                    Id = "ver-1",
                    MajorVersionNumber = 3,
                    MinorVersionNumber = 1,
                    LastModifiedFileStamp = 1234567890L
                }
            };

            project.OwnedExtensions.Add(modelInformation);

            var validArchive = new CapellaArchive(project);
            this.readerMock.Setup(reader => reader.Read(It.IsAny<Stream>())).Returns(validArchive);

            using var sampleStream = new MemoryStream();
            var successResult = this.extractor.Extract(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(successResult.IsError, Is.False);
                Assert.That(successResult.Value.Kind, Is.EqualTo(ArtifactKind.Capella));

                var manifest = (CapellaManifest)successResult.Value;
                Assert.That(manifest.Name, Is.EqualTo("Capella System Model"));
                Assert.That(manifest.Summary, Is.EqualTo("Summary of model"));
                Assert.That(manifest.Description, Is.EqualTo("Description of model"));
                Assert.That(manifest.MajorVersionNumber, Is.EqualTo(3));
                Assert.That(manifest.MinorVersionNumber, Is.EqualTo(1));
                Assert.That(manifest.LastModifiedFileStamp, Is.EqualTo(1234567890L));
            }

            var projectWithoutVersion = new Project
            {
                Id = "proj-2",
                Name = "Unversioned Model"
            };

            this.readerMock.Setup(reader => reader.Read(It.IsAny<Stream>())).Returns(new CapellaArchive(projectWithoutVersion));
            var unversionedResult = this.extractor.Extract(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(unversionedResult.IsError, Is.False);
                var manifest = (CapellaManifest)unversionedResult.Value;
                Assert.That(manifest.Name, Is.EqualTo("Unversioned Model"));
                Assert.That(manifest.MajorVersionNumber, Is.Null);
                Assert.That(manifest.MinorVersionNumber, Is.Null);
                Assert.That(manifest.LastModifiedFileStamp, Is.Null);
            }

            var projectWithBlankName = new Project
            {
                Id = "proj-3",
                Name = "   "
            };

            this.readerMock.Setup(reader => reader.Read(It.IsAny<Stream>())).Returns(new CapellaArchive(projectWithBlankName));
            var blankNameResult = this.extractor.Extract(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(blankNameResult.IsError, Is.True);
                Assert.That(blankNameResult.FirstError.Code, Is.EqualTo(CapellaErrorCodes.MissingName));
            }

            this.readerMock.Setup(reader => reader.Read(It.IsAny<Stream>())).Throws(new InvalidDataException("Archive corrupted"));
            var invalidArchiveResult = this.extractor.Extract(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(invalidArchiveResult.IsError, Is.True);
                Assert.That(invalidArchiveResult.FirstError.Code, Is.EqualTo(CapellaErrorCodes.InvalidArchive));
            }

            this.readerMock.Setup(reader => reader.Read(It.IsAny<Stream>())).Throws(new XmlException("Malformed XML"));
            var invalidDescriptorResult = this.extractor.Extract(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(invalidDescriptorResult.IsError, Is.True);
                Assert.That(invalidDescriptorResult.FirstError.Code, Is.EqualTo(CapellaErrorCodes.InvalidDescriptor));
            }

            this.readerMock.Setup(reader => reader.Read(It.IsAny<Stream>())).Throws(new InvalidOperationException("Unexpected error"));
            var unexpectedErrorResult = this.extractor.Extract(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(unexpectedErrorResult.IsError, Is.True);
                Assert.That(unexpectedErrorResult.FirstError.Code, Is.EqualTo(CapellaErrorCodes.UnexpectedError));
            }
        }

        [Test]
        public void VerifyExtractWithSampleArchive()
        {
            var samplePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources", SampleArchiveName);
            using var sampleStream = File.OpenRead(samplePath);

            var archiveReader = new CapellaArchiveReader();
            var manifestExtractor = new CapellaManifestExtractor(archiveReader);

            var result = manifestExtractor.Extract(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsError, Is.False);
                Assert.That(result.Value.Kind, Is.EqualTo(ArtifactKind.Capella));

                var manifest = (CapellaManifest)result.Value;
                Assert.That(manifest.Name, Is.EqualTo("coffee-machine-demo"));
                Assert.That(manifest.Summary, Is.Null);
                Assert.That(manifest.Description, Is.Null);
                Assert.That(manifest.MajorVersionNumber, Is.Null);
                Assert.That(manifest.MinorVersionNumber, Is.Null);
                Assert.That(manifest.LastModifiedFileStamp, Is.Null);
            }
        }
    }
}
