// ------------------------------------------------------------------------------------------------
// <copyright file="ExtractorsServiceCollectionExtensionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Tests.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;

    using Mycelium.Forge.Extractors.Enums;
    using Mycelium.Forge.Extractors.Extensions;
    using Mycelium.Forge.Extractors.Extractors;
    using Mycelium.Forge.Extractors.Readers;

    using NUnit.Framework;

    using SysML2.NET.Kpar;
    using SysML2.NET.Kpar.Cryptography;

    [TestFixture]
    public class ExtractorsServiceCollectionExtensionsTestFixture
    {
        [Test]
        public void VerifyRegisterArtifactManifestExtractors()
        {
            var services = new ServiceCollection();
            services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
            services.RegisterArtifactManifestExtractors();

            using var provider = services.BuildServiceProvider();
            var checksumService = provider.GetService<IChecksumService>();
            var kparReader = provider.GetService<IReader>();
            var ecssReader = provider.GetService<IEcssC3ArchiveReader>();
            var capellaReader = provider.GetService<ICapellaArchiveReader>();
            var extractors = provider.GetServices<IArtifactManifestExtractor>().ToList();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(checksumService, Is.Not.Null);
                Assert.That(kparReader, Is.Not.Null);
                Assert.That(ecssReader, Is.Not.Null);
                Assert.That(capellaReader, Is.Not.Null);
                Assert.That(extractors, Has.Count.GreaterThanOrEqualTo(3));
                Assert.That(extractors, Has.One.InstanceOf<KparManifestExtractor>());
                Assert.That(extractors, Has.One.InstanceOf<EcssC3ManifestExtractor>());
                Assert.That(extractors, Has.One.InstanceOf<CapellaManifestExtractor>());
                Assert.That(extractors.Any(registeredExtractor => registeredExtractor.CanHandle(ArtifactKind.Kpar)), Is.True);
                Assert.That(extractors.Any(registeredExtractor => registeredExtractor.CanHandle(ArtifactKind.EcssC3)), Is.True);
                Assert.That(extractors.Any(registeredExtractor => registeredExtractor.CanHandle(ArtifactKind.Capella)), Is.True);
            }
        }
    }
}
