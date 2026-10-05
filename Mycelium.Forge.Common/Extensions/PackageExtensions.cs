// ------------------------------------------------------------------------------------------------
// <copyright file="PackageExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides extension methods for <see cref="IPackage" /> and <see cref="Package" /> instances.
    /// </summary>
    public static class PackageExtensions
    {
        /// <param name="package">The package whose download count is to be computed.</param>
        extension(IPackage package)
        {
            /// <summary>
            /// Computes the total download count on the specified <see cref="IPackage" /> as the sum of downloads across the
            /// owned package versions.
            /// </summary>
            /// <param name="packageVersions">The collection of <see cref="IPackageVersion" /> instances associated with the package.</param>
            /// <returns>The total number of downloads across all package versions belonging to this package.</returns>
            public int ComputeDownloadCount(IEnumerable<IPackageVersion> packageVersions)
            {
                if (package == null || package.Version.Count == 0 || packageVersions == null)
                {
                    return 0;
                }

                return packageVersions
                    .Where(v => package.Version.Contains(v.Id))
                    .Sum(v => v.DownloadCount);
            }
        }
    }
}
