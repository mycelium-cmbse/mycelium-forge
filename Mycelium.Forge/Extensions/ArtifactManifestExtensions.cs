// ------------------------------------------------------------------------------------------------
// <copyright file="ArtifactManifestExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extensions
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extractors.Models;
    using Mycelium.Forge.Extractors.Models.Capella;
    using Mycelium.Forge.Extractors.Models.EcssC3;
    using Mycelium.Forge.Extractors.Models.Kpar;

    /// <summary>
    /// Provides extension methods for mapping <see cref="IArtifactManifest" /> data onto package DTOs.
    /// </summary>
    public static class ArtifactManifestExtensions
    {
        /// <param name="manifest">The artifact manifest instance.</param>
        extension(IArtifactManifest manifest)
        {
            /// <summary>
            /// Applies the extracted manifest metadata onto the specified package and version instances.
            /// </summary>
            /// <param name="package">The package entity.</param>
            /// <param name="packageVersion">The package version entity.</param>
            /// <param name="isExistingPackage">A value indicating whether an existing package is being versioned.</param>
            public void ApplyTo(IPackage package, IPackageVersion packageVersion, bool isExistingPackage)
            {
                switch (manifest)
                {
                    case KparManifest kpar:
                        ApplyKparManifest(kpar, package, packageVersion, isExistingPackage);
                        break;

                    case CapellaManifest capella:
                        ApplyCapellaManifest(capella, package, packageVersion, isExistingPackage);
                        break;

                    case EcssC3Manifest ecss:
                        ApplyEcssC3Manifest(ecss, package, packageVersion, isExistingPackage);
                        break;
                }
            }

            /// <summary>
            /// Extracts the display dependency tuples declared by the manifest, if any.
            /// </summary>
            /// <returns>A read-only list of dependency tuples (Name, Summary, IsVerified).</returns>
            public IReadOnlyList<(string Name, string Summary, bool IsVerified)> GetDependencies()
            {
                return manifest is KparManifest kpar
                    ? [.. (kpar.Dependencies ?? []).Select(d => (d.Resource, d.VersionConstraint, true))]
                    : [];
            }
        }

        /// <summary>
        /// Maps a SysML v2 kpar manifest onto the package and version DTOs.
        /// </summary>
        /// <param name="kpar">The extracted kpar manifest.</param>
        /// <param name="package">The package entity.</param>
        /// <param name="packageVersion">The package version entity.</param>
        /// <param name="isExistingPackage">A value indicating whether an existing package is being versioned.</param>
        private static void ApplyKparManifest(KparManifest kpar, IPackage package, IPackageVersion packageVersion, bool isExistingPackage)
        {
            if (!isExistingPackage)
            {
                package.Name = kpar.Name;
                package.ShortName = kpar.Name.ToShortName();
                package.Description = kpar.Description;
                package.HomepageUrl = kpar.Website;
            }

            packageVersion.Version = kpar.Version;
        }

        /// <summary>
        /// Maps a Capella archive manifest onto the package and version DTOs.
        /// </summary>
        /// <param name="capella">The extracted Capella manifest.</param>
        /// <param name="package">The package entity.</param>
        /// <param name="packageVersion">The package version entity.</param>
        /// <param name="isExistingPackage">A value indicating whether an existing package is being versioned.</param>
        private static void ApplyCapellaManifest(CapellaManifest capella, IPackage package, IPackageVersion packageVersion, bool isExistingPackage)
        {
            if (!isExistingPackage)
            {
                package.Name = capella.Name;
                package.ShortName = capella.Name.ToShortName();
                package.Description = !string.IsNullOrWhiteSpace(capella.Description) ? capella.Description : capella.Summary;
                package.HomepageUrl = string.Empty;
            }

            packageVersion.Version = capella.MajorVersionNumber.HasValue && capella.MinorVersionNumber.HasValue
                ? $"{capella.MajorVersionNumber.Value}.{capella.MinorVersionNumber.Value}.0"
                : string.Empty;
        }

        /// <summary>
        /// Maps an ECSS-E-TM-10-25 Annex C.3 archive manifest onto the package and version DTOs.
        /// </summary>
        /// <param name="ecss">The extracted ECSS manifest.</param>
        /// <param name="package">The package entity.</param>
        /// <param name="packageVersion">The package version entity.</param>
        /// <param name="isExistingPackage">A value indicating whether an existing package is being versioned.</param>
        private static void ApplyEcssC3Manifest(EcssC3Manifest ecss, IPackage package, IPackageVersion packageVersion, bool isExistingPackage)
        {
            if (!isExistingPackage)
            {
                package.Name = string.Empty;
                package.ShortName = string.Empty;
                package.Description = ecss.Remark;
                package.HomepageUrl = string.Empty;
            }

            packageVersion.Version = string.Empty;
        }
    }
}
