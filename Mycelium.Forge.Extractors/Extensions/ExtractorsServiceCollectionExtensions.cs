// ------------------------------------------------------------------------------------------------
// <copyright file="ExtractorsServiceCollectionExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Extensions
{
    using CDP4JsonFileDal;

    using Microsoft.Extensions.DependencyInjection;

    using Mycelium.Forge.Extractors.Extractors;
    using Mycelium.Forge.Extractors.Readers;

    using SysML2.NET.Kpar;
    using SysML2.NET.Kpar.Cryptography;

    /// <summary>
    /// Provides extension methods for registering artifact manifest extractors in the service collection.
    /// </summary>
    public static class ExtractorsServiceCollectionExtensions
    {
        /// <summary>
        /// Registers one <see cref="IArtifactManifestExtractor" /> for each supported artifact format.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to configure.</param>
        /// <returns>The configured <see cref="IServiceCollection" /> instance.</returns>
        public static IServiceCollection RegisterArtifactManifestExtractors(this IServiceCollection services)
        {
            services.AddSingleton<IChecksumService, ChecksumService>();
            services.AddSingleton<IReader, Reader>();
            services.AddSingleton<JsonFileDal>();
            services.AddSingleton<IEcssC3ArchiveReader, EcssC3ArchiveReader>();
            services.AddSingleton<IKparArchiveReader, KparArchiveReader>();
            services.AddSingleton<ICapellaArchiveReader, CapellaArchiveReader>();
            services.AddSingleton<IArchiveReader, EcssC3ArchiveReader>();
            services.AddSingleton<IArchiveReader, KparArchiveReader>();
            services.AddSingleton<IArchiveReader, CapellaArchiveReader>();
            services.AddSingleton<IArtifactManifestExtractor, KparManifestExtractor>();
            services.AddSingleton<IArtifactManifestExtractor, EcssC3ManifestExtractor>();
            services.AddSingleton<IArtifactManifestExtractor, CapellaManifestExtractor>();

            return services;
        }
    }
}
