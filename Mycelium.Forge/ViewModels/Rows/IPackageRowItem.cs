// ------------------------------------------------------------------------------------------------
// <copyright file="IPackageRowItem.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.Rows
{
    /// <summary>
    /// Defines common display properties for a package row or card item.
    /// </summary>
    public interface IPackageRowItem
    {
        /// <summary>
        /// Gets the package name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the package short name (slug).
        /// </summary>
        string ShortName { get; }

        /// <summary>
        /// Gets the full scoped package identifier in the form '{Publisher}/{Name}'.
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Gets the package description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the format name.
        /// </summary>
        string Format { get; }

        /// <summary>
        /// Gets the publisher namespace or author handle.
        /// </summary>
        string Publisher { get; }

        /// <summary>
        /// Gets the package release version string.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets the tags string for the package.
        /// </summary>
        string Tags { get; }

        /// <summary>
        /// Gets the number of downloads for this version.
        /// </summary>
        int DownloadCount { get; }

        /// <summary>
        /// Gets the number of packages that depend on this package.
        /// </summary>
        int DependentsCount { get; }

        /// <summary>
        /// Gets a value indicating whether the publisher is verified.
        /// </summary>
        bool IsVerified { get; }

        /// <summary>
        /// Gets the relative elapsed time since publication.
        /// </summary>
        string LastPublished { get; }
    }
}
