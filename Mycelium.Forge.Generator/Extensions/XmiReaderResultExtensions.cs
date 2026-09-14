// ------------------------------------------------------------------------------------------------
// <copyright file="XmiReaderResultExtensions.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Extensions
{
    using uml4net.xmi.Readers;

    /// <summary>
    /// Provides extension methods for <see cref="XmiReaderResult" />.
    /// </summary>
    public static class XmiReaderResultExtensions
    {
        /// <summary>
        /// The tag name for the package identifier.
        /// </summary>
        private const string PackageIdTagName = "eu.stariongroup.mycelium.packageId";

        /// <summary>
        /// The tag name for the model version.
        /// </summary>
        private const string VersionTagName = "eu.stariongroup.mycelium.version";

        /// <summary>
        /// Queries the model version from the specified <see cref="XmiReaderResult" />.
        /// </summary>
        /// <param name="xmiReaderResult">The <see cref="XmiReaderResult" /> to query.</param>
        /// <param name="packageId">The package identifier to look for. Defaults to "Mycelium.Model.Forge".</param>
        /// <returns>The model version string, or an empty string if not found.</returns>
        public static string QueryModelVersion(this XmiReaderResult xmiReaderResult, string packageId = "Mycelium.Model.Forge")
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);

            if (xmiReaderResult.XmiRoot?.Tags == null || xmiReaderResult.XmiRoot.Tags.Count == 0)
            {
                return string.Empty;
            }

            var tags = xmiReaderResult.XmiRoot.Tags;

            var packageTag = tags.Find(tag =>
                string.Equals(tag.Name, PackageIdTagName, StringComparison.Ordinal) &&
                string.Equals(tag.Value, packageId, StringComparison.Ordinal));

            if (packageTag?.Element == null || packageTag.Element.Count == 0)
            {
                return string.Empty;
            }

            var versionTag = tags.Find(tag => 
                string.Equals(tag.Name, VersionTagName, StringComparison.Ordinal) &&
                tag.Element.Exists(elementId => packageTag.Element.Contains(elementId)));

            return versionTag?.Value ?? string.Empty;
        }
    }
}
