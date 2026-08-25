// ------------------------------------------------------------------------------------------------
// <copyright file="PackageSortOption.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Enums
{
    /// <summary>
    /// Specifies the available sorting criteria for package search results.
    /// </summary>
    public enum PackageSortOption
    {
        /// <summary>
        /// Sort packages by search relevance.
        /// </summary>
        Relevance,

        /// <summary>
        /// Sort packages by total download count.
        /// </summary>
        Downloads,

        /// <summary>
        /// Sort packages by most recent update timestamp.
        /// </summary>
        RecentlyUpdated,

        /// <summary>
        /// Sort packages alphabetically by name.
        /// </summary>
        Alphabetical
    }
}
