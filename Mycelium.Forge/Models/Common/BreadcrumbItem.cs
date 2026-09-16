// ------------------------------------------------------------------------------------------------
// <copyright file="BreadcrumbItem.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.Common
{
    /// <summary>
    /// Represents an individual item in a breadcrumb navigation trail.
    /// </summary>
    public class BreadcrumbItem
    {
        /// <summary>
        /// Gets or sets the display name or title of the breadcrumb item.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the optional hyperlink target URL for the breadcrumb item.
        /// </summary>
        public string Link { get; set; } = string.Empty;
    }
}
