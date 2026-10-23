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

    using Mycelium.Forge.Extractors.Extensions;
    using Mycelium.Forge.Extractors.Extractors;
    using Mycelium.Forge.Extractors.Models;

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
            var reader = provider.GetService<IReader>();
            var extractors = provider.GetServices<IArtifactManifestExtractor>().ToList();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(checksumService, Is.Not.Null);
                Assert.That(reader, Is.Not.Null);
                Assert.That(extractors, Has.Count.GreaterThanOrEqualTo(1));
                Assert.That(extractors, Has.One.InstanceOf<KparManifestExtractor>());
                Assert.That(extractors.Any(registeredExtractor => registeredExtractor.CanHandle(ArtifactKind.Kpar)), Is.True);
            }
        }
    }
}
