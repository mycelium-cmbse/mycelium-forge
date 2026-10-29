// ------------------------------------------------------------------------------------------------
// <copyright file="KparArchiveReaderTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Tests.Readers
{
    using Microsoft.Extensions.Logging.Abstractions;

    using Mycelium.Forge.Extractors.Enums;
    using Mycelium.Forge.Extractors.Models;
    using Mycelium.Forge.Extractors.Models.Kpar;
    using Mycelium.Forge.Extractors.Readers;

    using NUnit.Framework;

    using SysML2.NET.Kpar;
    using SysML2.NET.Kpar.Cryptography;

    [TestFixture]
    public class KparArchiveReaderTestFixture
    {
        private const string SampleArchiveName = "Kernel_Data_Type_Library-1.1.0-dev.20260801.kpar";
        private KparArchiveReader reader;

        [SetUp]
        public void SetUp()
        {
            var checksumService = new ChecksumService();
            var sysmlReader = new Reader(checksumService, NullLogger<Reader>.Instance);
            this.reader = new KparArchiveReader(sysmlReader);
        }

        [Test]
        public void VerifyCanHandle()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.reader.CanHandle(ArtifactKind.Kpar), Is.True);
                Assert.That(this.reader.CanHandle(ArtifactKind.EcssC3), Is.False);
                Assert.That(this.reader.CanHandle(ArtifactKind.Capella), Is.False);
            }
        }

        [Test]
        public void VerifyRead()
        {
            var samplePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources", SampleArchiveName);
            using var sampleStream = File.OpenRead(samplePath);
            var archive = (KparArchive)this.reader.Read(sampleStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(archive, Is.Not.Null);
                Assert.That(archive.Archive, Is.Not.Null);
                Assert.That(archive.Archive.Project, Is.Not.Null);
                Assert.That(archive.Archive.Project.Name, Is.EqualTo("Kernel Data Type Library"));
            }

            using var secondStream = File.OpenRead(samplePath);
            IArchiveReader baseReader = this.reader;
            var genericArchive = baseReader.Read(secondStream);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(genericArchive, Is.Not.Null);
                Assert.That(genericArchive, Is.InstanceOf<IArchive>());
            }

            Assert.That(() =>
            {
                using var corruptStream = new MemoryStream([0x01, 0x02, 0x03, 0x04]);
                this.reader.Read(corruptStream);
            }, Throws.TypeOf<InvalidDataException>());
        }
    }
}
