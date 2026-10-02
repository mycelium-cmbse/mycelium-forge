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
        /// <param name="package">The package whose download count is to be computed and set.</param>
        extension(IPackage package)
        {
            /// <summary>
            /// Computes and sets the total download count on the specified <see cref="IPackage" /> as the sum of downloads across the
            /// provided package versions.
            /// </summary>
            /// <param name="packageVersions">The collection of <see cref="IPackageVersion" /> instances associated with the package.</param>
            public void ComputeDownloadCount(IEnumerable<IPackageVersion> packageVersions)
            {
                if (package is Package concretePackage && packageVersions != null)
                {
                    concretePackage.downloadCount = packageVersions
                        .Where(v => v.Owner == package.Id)
                        .Sum(v => v.DownloadCount);
                }
            }
        }
    }
}
