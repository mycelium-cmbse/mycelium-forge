// ------------------------------------------------------------------------------------------------
// <copyright file="CapellaArchiveReaderTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Tests.Readers
{
    using Auriga.Capellamodeller;
    using Auriga.Libraries;

    using Mycelium.Forge.Extractors.Enums;
    using Mycelium.Forge.Extractors.Models.Capella;
    using Mycelium.Forge.Extractors.Readers;
    using Mycelium.Forge.Extractors.Tests.Helpers;
    using Mycelium.Forge.Extractors.Tests.Models;

    using NUnit.Framework;

    [TestFixture]
    public class CapellaArchiveReaderTestFixture
    {
        private const string SampleArchiveName = "coffee-machine-demo.zip";
        private const string InvalidRootXml = "<UnknownRoot id=\"1\" name=\"test\" />";
        private string validCapellaXml;
        private string validMelodyModellerXml;
        private string validLibraryXml;
        private string validVersionWithoutXsiTypeXml;
        private CapellaArchiveReader reader;

        [SetUp]
        public void SetUp()
        {
            this.validCapellaXml = """
                                   <?xml version="1.0" encoding="UTF-8"?>
                                   <!--Capella_Version_6.0.0-->
                                   <org.polarsys.capella.core.data.capellamodeller:Project
                                       xmi:version="2.0"
                                       xmlns:xmi="http://www.omg.org/XMI"
                                       xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                                       xmlns:libraries="http://www.polarsys.org/capella/common/libraries/6.0.0"
                                       xmlns:org.polarsys.capella.core.data.capellamodeller="http://www.polarsys.org/capella/core/modeller/6.0.0"
                                       id="proj-123"
                                       name="Sample Capella Project"
                                       summary="A test summary"
                                       description="A full test description">
                                     <ownedExtensions xsi:type="libraries:ModelInformation" id="model-info-1">
                                       <version xsi:type="libraries:ModelVersion" id="version-1" majorVersionNumber="2" minorVersionNumber="5" lastModifiedFileStamp="1700000000"/>
                                     </ownedExtensions>
                                   </org.polarsys.capella.core.data.capellamodeller:Project>
                                   """;

            this.validMelodyModellerXml = """
                                          <?xml version="1.0" encoding="UTF-8"?>
                                          <org.polarsys.capella.core.data.capellamodeller:Project
                                              xmi:version="2.0"
                                              xmlns:xmi="http://www.omg.org/XMI"
                                              xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                                              xmlns:org.polarsys.capella.core.data.capellamodeller="http://www.polarsys.org/capella/core/modeller/5.0.0"
                                              id="legacy-proj"
                                              name="Legacy Project" />
                                          """;

            this.validLibraryXml = """
                                   <?xml version="1.0" encoding="UTF-8"?>
                                   <org.polarsys.capella.core.data.capellamodeller:Library
                                       xmi:version="2.0"
                                       xmlns:xmi="http://www.omg.org/XMI"
                                       xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                                       xmlns:org.polarsys.capella.core.data.capellamodeller="http://www.polarsys.org/capella/core/modeller/6.0.0"
                                       id="lib-proj"
                                       name="Sample Capella Library" />
                                   """;

            this.validVersionWithoutXsiTypeXml = """
                                                 <?xml version="1.0" encoding="UTF-8"?>
                                                 <org.polarsys.capella.core.data.capellamodeller:Project
                                                     xmi:version="2.0"
                                                     xmlns:xmi="http://www.omg.org/XMI"
                                                     xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                                                     xmlns:libraries="http://www.polarsys.org/capella/common/libraries/6.0.0"
                                                     xmlns:org.polarsys.capella.core.data.capellamodeller="http://www.polarsys.org/capella/core/modeller/6.0.0"
                                                     id="proj-456"
                                                     name="Project With Untyped Version">
                                                   <ownedExtensions xsi:type="libraries:ModelInformation" id="model-info-2">
                                                     <version id="ver-2" majorVersionNumber="4" minorVersionNumber="1" />
                                                   </ownedExtensions>
                                                 </org.polarsys.capella.core.data.capellamodeller:Project>
                                                 """;

            this.reader = new CapellaArchiveReader();
        }

        [Test]
        public void VerifyCanHandle()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.reader.CanHandle(ArtifactKind.Capella), Is.True);
                Assert.That(this.reader.CanHandle(ArtifactKind.Kpar), Is.False);
                Assert.That(this.reader.CanHandle(ArtifactKind.EcssC3), Is.False);
            }
        }

        [Test]
        public void VerifyRead()
        {
            var samplePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources", SampleArchiveName);
            using var sampleStream = File.OpenRead(samplePath);
            var sampleArchive = (CapellaArchive)this.reader.Read(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(sampleArchive, Is.Not.Null);
                Assert.That(sampleArchive.Project, Is.Not.Null);
                Assert.That(sampleArchive.Project.Name, Is.EqualTo("coffee-machine-demo"));
                Assert.That(sampleArchive.Project.Id, Is.EqualTo("5d62564f-8f7f-4179-bcd0-1dc1d213c3e8"));
            }

            using var projectStream = ArchiveTestHelper.CreateZipStream(new ZipEntryModel("model.capella", this.validCapellaXml));
            var projectArchive = (CapellaArchive)this.reader.Read(projectStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(projectArchive, Is.Not.Null);
                Assert.That(projectArchive.Project, Is.Not.Null);
                Assert.That(projectArchive.Project.Name, Is.EqualTo("Sample Capella Project"));
                Assert.That(projectArchive.Project.Summary, Is.EqualTo("A test summary"));
                Assert.That(projectArchive.Project.Description, Is.EqualTo("A full test description"));
                Assert.That(projectArchive.Project.OwnedExtensions, Has.Count.EqualTo(1));

                var modelInfo = (IModelInformation)projectArchive.Project.OwnedExtensions[0];
                Assert.That(modelInfo.Id, Is.EqualTo("model-info-1"));
                Assert.That(modelInfo.Version, Is.Not.Null);
                Assert.That(modelInfo.Version.Id, Is.EqualTo("version-1"));
                Assert.That(modelInfo.Version.MajorVersionNumber, Is.EqualTo(2));
                Assert.That(modelInfo.Version.MinorVersionNumber, Is.EqualTo(5));
                Assert.That(modelInfo.Version.LastModifiedFileStamp, Is.EqualTo(1700000000L));
            }

            using var untypedVersionStream = ArchiveTestHelper.CreateZipStream(new ZipEntryModel("model.capella", this.validVersionWithoutXsiTypeXml));
            var untypedVersionArchive = (CapellaArchive)this.reader.Read(untypedVersionStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(untypedVersionArchive.Project.OwnedExtensions, Has.Count.EqualTo(1));
                var untypedModelInfo = (IModelInformation)untypedVersionArchive.Project.OwnedExtensions[0];
                Assert.That(untypedModelInfo.Version, Is.Not.Null);
                Assert.That(untypedModelInfo.Version.MajorVersionNumber, Is.EqualTo(4));
                Assert.That(untypedModelInfo.Version.MinorVersionNumber, Is.EqualTo(1));
                Assert.That(untypedModelInfo.Version.LastModifiedFileStamp, Is.Null);
            }

            using var fragmentedProjectStream = ArchiveTestHelper.CreateZipStream(new ZipEntryModel("model.capella", this.validCapellaXml), new ZipEntryModel("subsystem.capellafragment", "<fragment />"));
            var fragmentedArchive = (CapellaArchive)this.reader.Read(fragmentedProjectStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(fragmentedArchive, Is.Not.Null);
                Assert.That(fragmentedArchive.Project.Name, Is.EqualTo("Sample Capella Project"));
            }

            using var legacyStream = ArchiveTestHelper.CreateZipStream(new ZipEntryModel("legacy.melodymodeller", this.validMelodyModellerXml));
            var legacyArchive = (CapellaArchive)this.reader.Read(legacyStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(legacyArchive, Is.Not.Null);
                Assert.That(legacyArchive.Project.Name, Is.EqualTo("Legacy Project"));
            }

            using var libraryStream = ArchiveTestHelper.CreateZipStream(new ZipEntryModel("component.capella", this.validLibraryXml));
            var libraryArchive = (CapellaArchive)this.reader.Read(libraryStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(libraryArchive, Is.Not.Null);
                Assert.That(libraryArchive.Project, Is.InstanceOf<ILibrary>());
                Assert.That(libraryArchive.Project.Name, Is.EqualTo("Sample Capella Library"));
            }

            var emptyArchiveStream = ArchiveTestHelper.CreateZipStream();
            Assert.That(() => this.reader.Read(emptyArchiveStream), Throws.TypeOf<InvalidDataException>());

            var multipleModelsStream = ArchiveTestHelper.CreateZipStream(new ZipEntryModel("a.capella", this.validCapellaXml), new ZipEntryModel("b.capella", this.validCapellaXml));
            Assert.That(() => this.reader.Read(multipleModelsStream), Throws.TypeOf<InvalidDataException>());

            var invalidRootStream = ArchiveTestHelper.CreateZipStream(new ZipEntryModel("invalid.capella", InvalidRootXml));
            Assert.That(() => this.reader.Read(invalidRootStream), Throws.TypeOf<InvalidDataException>());
        }
    }
}
