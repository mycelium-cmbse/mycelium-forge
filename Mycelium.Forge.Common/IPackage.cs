// ------------------------------------------------------------------------------------------------
// <copyright file="IPackage.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    using System.Collections.Generic;

    /// <summary>
    /// Partial interface extending <see cref="IPackage" /> with repository URLs, documentation, license, tags, verification
    /// status, and deprecation attributes.
    /// </summary>
    /// <remarks>
    /// This partial interface is temporary and will be removed once the domain metamodel is updated to include repository
    /// links, homepage and documentation URLs, SPDX license expressions, tag relationships, verification status, and
    /// deprecation lifecycle attributes in the code-generated DTO.
    /// </remarks>
    public partial interface IPackage
    {
        /// <summary>
        /// Gets or sets the SPDX license identifier for the package (e.g., Apache-2.0, MIT, BSD-3-Clause).
        /// </summary>
        string License { get; set; }

        /// <summary>
        /// Gets or sets the source code repository URL for the package.
        /// </summary>
        string RepositoryUrl { get; set; }

        /// <summary>
        /// Gets or sets the primary project homepage or external website URL.
        /// </summary>
        string HomepageUrl { get; set; }

        /// <summary>
        /// Gets or sets the hosted documentation or API reference URL.
        /// </summary>
        string DocumentationUrl { get; set; }

        /// <summary>
        /// Gets or sets the collection of tag names associated with the package.
        /// </summary>
        List<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the package is deprecated.
        /// </summary>
        /// <remarks>
        /// This property is temporary and will be removed once dedicated deprecation lifecycle entities are implemented in the
        /// domain model.
        /// </remarks>
        bool IsDeprecated { get; set; }

        /// <summary>
        /// Gets or sets the human-readable explanation or migration guidance for why the package was deprecated.
        /// </summary>
        /// <remarks>
        /// This property is temporary and will be removed once dedicated deprecation lifecycle entities are implemented in the
        /// domain model.
        /// </remarks>
        string DeprecationReason { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the package publisher has verified credentials and cryptographic signatures.
        /// </summary>
        bool IsVerified { get; set; }
    }
}
