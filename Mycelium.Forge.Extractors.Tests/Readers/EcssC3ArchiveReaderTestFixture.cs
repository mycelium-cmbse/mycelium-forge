// ------------------------------------------------------------------------------------------------
// <copyright file="EcssC3ArchiveReaderTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Tests.Readers
{
    using System.Text.Json;

    using CDP4Common.CommonData;
    using CDP4Common.DTO;

    using CDP4JsonFileDal;

    using Mycelium.Forge.Extractors.Enums;
    using Mycelium.Forge.Extractors.Models.EcssC3;
    using Mycelium.Forge.Extractors.Readers;
    using Mycelium.Forge.Extractors.Tests.Helpers;
    using Mycelium.Forge.Extractors.Tests.Models;

    using NUnit.Framework;

    using File = File;

    [TestFixture]
    public class EcssC3ArchiveReaderTestFixture
    {
        private const string SampleArchiveName = "LOFT_ECSS-E-TM-10-25_AnnexC.zip";
        private string validHeaderJson;
        private string validSiteDirectoryJson;
        private EcssC3ArchiveReader reader;

        [SetUp]
        public void SetUp()
        {
            this.validHeaderJson = """
                                   {
                                       "mediaType": "application/x-ecss-c3",
                                       "dataModelVersion": "2.4.1",
                                       "exchangeFileFormatVersion": "1.0",
                                       "createdOn": {
                                           "utc": "2026-03-30T10:00:00Z",
                                           "local": "2026-03-30T12:00:00+02:00"
                                       },
                                       "creatorPerson": {
                                           "givenName": "Jane",
                                           "surname": "Doe"
                                       }
                                   }
                                   """;

            this.validSiteDirectoryJson = """
                                          [
                                              {
                                                  "classKind": "SiteDirectory",
                                                  "iid": "00000000-0000-0000-0000-000000000001",
                                                  "revisionNumber": 1,
                                                  "name": "Test Site"
                                              }
                                          ]
                                          """;

            var jsonFileDal = new JsonFileDal();
            this.reader = new EcssC3ArchiveReader(jsonFileDal);
        }

        [Test]
        public void VerifyCanHandle()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.reader.CanHandle(ArtifactKind.EcssC3), Is.True);
                Assert.That(this.reader.CanHandle(ArtifactKind.Kpar), Is.False);
                Assert.That(this.reader.CanHandle(ArtifactKind.Capella), Is.False);
            }
        }

        [Test]
        public void VerifyRead()
        {
            var samplePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources", SampleArchiveName);
            using var sampleStream = File.OpenRead(samplePath);
            var archive = (EcssC3Archive)this.reader.Read(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(archive, Is.Not.Null);
                Assert.That(archive.Header, Is.Not.Null);
                Assert.That(archive.Header.MediaType, Is.EqualTo("application/ecss-e-tm-10-25+json"));
                Assert.That(archive.Header.DataModelVersion, Is.EqualTo("2.5.0"));
                Assert.That(archive.Header.CreatedOn, Is.Not.Null);
                Assert.That(archive.Header.CreatedOn.Utc, Is.EqualTo(DateTime.Parse("2016-03-09T14:18:36.3233254Z").ToUniversalTime()));
                Assert.That(archive.Header.LastModifiedOn, Is.Null);
                Assert.That(archive.SiteDirectoryThings, Has.Count.GreaterThan(0));
                Assert.That(archive.SiteDirectoryThings.Any(thing => thing is SiteDirectory || thing.ClassKind == ClassKind.SiteDirectory), Is.True);
            }

            using var rootArchiveStream = ArchiveTestHelper.CreateZipStream(new ZipEntryModel("Header.json", this.validHeaderJson), new ZipEntryModel("SiteDirectory.json", this.validSiteDirectoryJson));
            var rootArchive = (EcssC3Archive)this.reader.Read(rootArchiveStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(rootArchive, Is.Not.Null);
                Assert.That(rootArchive.Header, Is.Not.Null);
                Assert.That(rootArchive.Header.MediaType, Is.EqualTo("application/x-ecss-c3"));
                Assert.That(rootArchive.Header.CreatedOn, Is.Not.Null);
                Assert.That(rootArchive.Header.CreatedOn.Utc, Is.EqualTo(DateTime.Parse("2026-03-30T10:00:00Z").ToUniversalTime()));
                Assert.That(rootArchive.Header.LastModifiedOn, Is.Null);
                Assert.That(rootArchive.SiteDirectoryThings, Has.Count.EqualTo(1));
            }

            var missingHeaderStream = ArchiveTestHelper.CreateZipStream(new ZipEntryModel("SiteDirectory.json", this.validSiteDirectoryJson));
            Assert.That(() => this.reader.Read(missingHeaderStream), Throws.TypeOf<InvalidDataException>());

            var missingSiteDirectoryStream = ArchiveTestHelper.CreateZipStream(new ZipEntryModel("Header.json", this.validHeaderJson));
            Assert.That(() => this.reader.Read(missingSiteDirectoryStream), Throws.TypeOf<InvalidDataException>());

            var corruptHeaderStream = ArchiveTestHelper.CreateZipStream(new ZipEntryModel("Header.json", "{ not valid json"), new ZipEntryModel("SiteDirectory.json", this.validSiteDirectoryJson));
            Assert.That(() => this.reader.Read(corruptHeaderStream), Throws.TypeOf<JsonException>());

            var corruptSiteDirectoryStream = ArchiveTestHelper.CreateZipStream(new ZipEntryModel("Header.json", this.validHeaderJson), new ZipEntryModel("SiteDirectory.json", "{ not valid json"));
            Assert.That(() => this.reader.Read(corruptSiteDirectoryStream), Throws.TypeOf<JsonException>());

            var nonZipStream = new MemoryStream([0x01, 0x02, 0x03, 0x04]);
            Assert.That(() => this.reader.Read(nonZipStream), Throws.TypeOf<InvalidDataException>());

            var duplicateHeaderStream = ArchiveTestHelper.CreateZipStream(new ZipEntryModel("Header.json", this.validHeaderJson), new ZipEntryModel("sub/Header.json", this.validHeaderJson), new ZipEntryModel("SiteDirectory.json", this.validSiteDirectoryJson));
            Assert.That(() => this.reader.Read(duplicateHeaderStream), Throws.TypeOf<InvalidDataException>());

            var duplicateSiteDirectoryStream = ArchiveTestHelper.CreateZipStream(new ZipEntryModel("Header.json", this.validHeaderJson), new ZipEntryModel("SiteDirectory.json", this.validSiteDirectoryJson), new ZipEntryModel("sub/SiteDirectory.json", this.validSiteDirectoryJson));
            Assert.That(() => this.reader.Read(duplicateSiteDirectoryStream), Throws.TypeOf<InvalidDataException>());

            var otherHeaderStream = ArchiveTestHelper.CreateZipStream(new ZipEntryModel("OtherHeader.json", this.validHeaderJson), new ZipEntryModel("SiteDirectory.json", this.validSiteDirectoryJson));
            Assert.That(() => this.reader.Read(otherHeaderStream), Throws.TypeOf<InvalidDataException>());
        }
    }
}
