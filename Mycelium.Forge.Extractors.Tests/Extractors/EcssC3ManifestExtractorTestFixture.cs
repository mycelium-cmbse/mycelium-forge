// ------------------------------------------------------------------------------------------------
// <copyright file="EcssC3ManifestExtractorTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Tests.Extractors
{
    using System.Text.Json;

    using CDP4Common.DTO;

    using CDP4JsonFileDal;
    using CDP4JsonFileDal.Json;

    using ErrorOr;

    using Moq;

    using Mycelium.Forge.Extractors.Enums;
    using Mycelium.Forge.Extractors.Errors;
    using Mycelium.Forge.Extractors.Extractors;
    using Mycelium.Forge.Extractors.Models.EcssC3;
    using Mycelium.Forge.Extractors.Readers;

    using NUnit.Framework;

    using File = File;

    [TestFixture]
    public class EcssC3ManifestExtractorTestFixture
    {
        private const string SampleArchiveName = "LOFT_ECSS-E-TM-10-25_AnnexC.zip";
        private Mock<IEcssC3ArchiveReader> readerMock;
        private EcssC3ManifestExtractor extractor;

        [SetUp]
        public void SetUp()
        {
            this.readerMock = new Mock<IEcssC3ArchiveReader>();
            this.extractor = new EcssC3ManifestExtractor(this.readerMock.Object);
        }

        [Test]
        public void VerifyCanHandle()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.extractor.CanHandle(ArtifactKind.EcssC3), Is.True);
                Assert.That(this.extractor.CanHandle(ArtifactKind.Kpar), Is.False);
                Assert.That(this.extractor.CanHandle(ArtifactKind.Capella), Is.False);
            }
        }

        [Test]
        public void VerifyExtract()
        {
            var createdDate = new DateTimeOffset(2026, 3, 30, 10, 0, 0, TimeSpan.Zero);
            var modifiedDate = new DateTimeOffset(2026, 3, 30, 12, 0, 0, TimeSpan.Zero);

            var validHeader = new ExchangeFileHeader
            {
                MediaType = "application/x-ecss-c3",
                DataModelVersion = "2.4.1",
                ExchangeFileFormatVersion = "1.0",
                CreatedOn = new ExchangeDateTime(createdDate.UtcDateTime),
                LastModifiedOn = new ExchangeDateTime(modifiedDate.UtcDateTime),
                CreatorPerson = new ExchangeFileInitiator { GivenName = "Jane", Surname = "Doe" },
                CreatorOrganization = new OrganizationInfo { Name = "Starion" },
                Copyright = "Copyright 2026",
                Remark = "Test remark",
                Extensions = "Test extensions"
            };

            var validArchive = new EcssC3Archive(validHeader, [new SiteDirectory()]);
            this.readerMock.Setup(r => r.Read(It.IsAny<Stream>())).Returns(validArchive);

            using var sampleStream = new MemoryStream();
            var successResult = this.extractor.Extract(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(successResult.IsError, Is.False);
                Assert.That(successResult.Value.Kind, Is.EqualTo(ArtifactKind.EcssC3));

                var manifest = (EcssC3Manifest)successResult.Value;
                Assert.That(manifest.MediaType, Is.EqualTo("application/x-ecss-c3"));
                Assert.That(manifest.DataModelVersion, Is.EqualTo("2.4.1"));
                Assert.That(manifest.ExchangeFileFormatVersion, Is.EqualTo("1.0"));
                Assert.That(manifest.CreatorPerson, Is.EqualTo("Jane Doe"));
                Assert.That(manifest.CreatorOrganization, Is.EqualTo("Starion"));
                Assert.That(manifest.Copyright, Is.EqualTo("Copyright 2026"));
                Assert.That(manifest.Remark, Is.EqualTo("Test remark"));
                Assert.That(manifest.Extensions, Is.EqualTo("Test extensions"));
                Assert.That(manifest.CreatedOn, Is.EqualTo(createdDate));
                Assert.That(manifest.LastModifiedOn, Is.EqualTo(modifiedDate));
            }

            this.readerMock.Setup(r => r.Read(It.IsAny<Stream>())).Throws(new InvalidDataException("Corrupted zip."));
            var invalidArchiveResult = this.extractor.Extract(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(invalidArchiveResult.IsError, Is.True);
                Assert.That(invalidArchiveResult.FirstError.Code, Is.EqualTo(EcssC3ErrorCodes.InvalidArchive));
            }

            this.readerMock.Setup(r => r.Read(It.IsAny<Stream>())).Throws(new JsonException("Corrupted JSON."));
            var invalidDescriptorResult = this.extractor.Extract(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(invalidDescriptorResult.IsError, Is.True);
                Assert.That(invalidDescriptorResult.FirstError.Code, Is.EqualTo(EcssC3ErrorCodes.InvalidDescriptor));
            }

            this.readerMock.Setup(r => r.Read(It.IsAny<Stream>())).Throws(new InvalidOperationException("Unexpected failure."));
            var unexpectedResult = this.extractor.Extract(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(unexpectedResult.IsError, Is.True);
                Assert.That(unexpectedResult.FirstError.Type, Is.EqualTo(ErrorType.Unexpected));
                Assert.That(unexpectedResult.FirstError.Code, Is.EqualTo(EcssC3ErrorCodes.UnexpectedError));
            }

            var missingMediaTypeHeader = new ExchangeFileHeader
            {
                MediaType = null,
                DataModelVersion = "2.4.1",
                ExchangeFileFormatVersion = "1.0",
                CreatedOn = new ExchangeDateTime(DateTime.UtcNow),
                CreatorPerson = new ExchangeFileInitiator { GivenName = "Jane", Surname = "Doe" }
            };

            this.readerMock.Setup(r => r.Read(It.IsAny<Stream>())).Returns(new EcssC3Archive(missingMediaTypeHeader, [new SiteDirectory()]));
            var missingMediaTypeResult = this.extractor.Extract(sampleStream);
            Assert.That(missingMediaTypeResult.FirstError.Code, Is.EqualTo(EcssC3ErrorCodes.MissingMediaType));

            var missingDataModelVersionHeader = new ExchangeFileHeader
            {
                MediaType = "application/x-ecss-c3",
                DataModelVersion = string.Empty,
                ExchangeFileFormatVersion = "1.0",
                CreatedOn = new ExchangeDateTime(DateTime.UtcNow),
                CreatorPerson = new ExchangeFileInitiator { GivenName = "Jane", Surname = "Doe" }
            };

            this.readerMock.Setup(r => r.Read(It.IsAny<Stream>())).Returns(new EcssC3Archive(missingDataModelVersionHeader, [new SiteDirectory()]));
            var missingDataModelResult = this.extractor.Extract(sampleStream);
            Assert.That(missingDataModelResult.FirstError.Code, Is.EqualTo(EcssC3ErrorCodes.MissingDataModelVersion));

            var missingExchangeFormatHeader = new ExchangeFileHeader
            {
                MediaType = "application/x-ecss-c3",
                DataModelVersion = "2.4.1",
                ExchangeFileFormatVersion = "   ",
                CreatedOn = new ExchangeDateTime(DateTime.UtcNow),
                CreatorPerson = new ExchangeFileInitiator { GivenName = "Jane", Surname = "Doe" }
            };

            this.readerMock.Setup(r => r.Read(It.IsAny<Stream>())).Returns(new EcssC3Archive(missingExchangeFormatHeader, [new SiteDirectory()]));
            var missingExchangeFormatResult = this.extractor.Extract(sampleStream);
            Assert.That(missingExchangeFormatResult.FirstError.Code, Is.EqualTo(EcssC3ErrorCodes.MissingExchangeFileFormatVersion));

            var missingCreatedOnHeader = new ExchangeFileHeader
            {
                MediaType = "application/x-ecss-c3",
                DataModelVersion = "2.4.1",
                ExchangeFileFormatVersion = "1.0",
                CreatedOn = null,
                CreatorPerson = new ExchangeFileInitiator { GivenName = "Jane", Surname = "Doe" }
            };

            this.readerMock.Setup(r => r.Read(It.IsAny<Stream>())).Returns(new EcssC3Archive(missingCreatedOnHeader, [new SiteDirectory()]));
            var missingCreatedOnResult = this.extractor.Extract(sampleStream);
            Assert.That(missingCreatedOnResult.FirstError.Code, Is.EqualTo(EcssC3ErrorCodes.MissingCreatedOn));

            var missingCreatorPersonHeader = new ExchangeFileHeader
            {
                MediaType = "application/x-ecss-c3",
                DataModelVersion = "2.4.1",
                ExchangeFileFormatVersion = "1.0",
                CreatedOn = new ExchangeDateTime(DateTime.UtcNow),
                CreatorPerson = null
            };

            this.readerMock.Setup(r => r.Read(It.IsAny<Stream>())).Returns(new EcssC3Archive(missingCreatorPersonHeader, [new SiteDirectory()]));
            var missingCreatorPersonResult = this.extractor.Extract(sampleStream);
            Assert.That(missingCreatorPersonResult.FirstError.Code, Is.EqualTo(EcssC3ErrorCodes.MissingCreatorPerson));

            this.readerMock.Setup(r => r.Read(It.IsAny<Stream>())).Returns(new EcssC3Archive(validHeader, []));
            var emptySiteResult = this.extractor.Extract(sampleStream);
            Assert.That(emptySiteResult.FirstError.Code, Is.EqualTo(EcssC3ErrorCodes.InvalidSiteDirectory));

            var completelyEmptyHeader = new ExchangeFileHeader
            {
                MediaType = null,
                DataModelVersion = null,
                ExchangeFileFormatVersion = null,
                CreatedOn = null,
                CreatorPerson = null
            };

            this.readerMock.Setup(r => r.Read(It.IsAny<Stream>())).Returns(new EcssC3Archive(completelyEmptyHeader, []));
            var multipleErrorsResult = this.extractor.Extract(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(multipleErrorsResult.IsError, Is.True);
                Assert.That(multipleErrorsResult.Errors, Has.Count.EqualTo(6));
            }
        }

        [Test]
        public void VerifyExtractWithSampleArchive()
        {
            var samplePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources", SampleArchiveName);
            using var sampleStream = File.OpenRead(samplePath);

            var jsonFileDal = new JsonFileDal();
            var archiveReader = new EcssC3ArchiveReader(jsonFileDal);
            var manifestExtractor = new EcssC3ManifestExtractor(archiveReader);

            var result = manifestExtractor.Extract(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsError, Is.False);
                Assert.That(result.Value.Kind, Is.EqualTo(ArtifactKind.EcssC3));

                var manifest = (EcssC3Manifest)result.Value;
                Assert.That(manifest.MediaType, Is.EqualTo("application/ecss-e-tm-10-25+json"));
                Assert.That(manifest.DataModelVersion, Is.EqualTo("2.5.0"));
                Assert.That(manifest.ExchangeFileFormatVersion, Is.EqualTo("1.0.0"));
                Assert.That(manifest.CreatorPerson, Is.EqualTo("The Administrator"));
                Assert.That(manifest.CreatorOrganization, Is.Null);
                Assert.That(manifest.Copyright, Is.EqualTo("Copyright 2016 © RHEA."));
                Assert.That(manifest.Remark, Is.EqualTo("This is an ECSS-E-TM-10-25 exchange file"));
                Assert.That(manifest.Extensions, Is.Null);
                Assert.That(manifest.CreatedOn, Is.EqualTo(DateTimeOffset.Parse("2016-03-09T14:18:36.3233254Z")));
                Assert.That(manifest.LastModifiedOn, Is.Null);
            }
        }
    }
}
