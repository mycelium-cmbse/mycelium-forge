// ------------------------------------------------------------------------------------------------
// <copyright file="Package.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Partial class implementation for <see cref="Package" /> providing extended metadata, repository URLs, documentation, tags, verification status, and deprecation attributes.
    /// </summary>
    /// <remarks>
    /// This partial class is temporary and will be removed once the domain metamodel is updated to include repository links, homepage and documentation URLs, SPDX license expressions, tag relationships, verification status, and deprecation lifecycle attributes in the code-generated DTO.
    /// </remarks>
    public partial class Package
    {
        /// <summary>
        /// Gets or sets the SPDX license identifier for the package (e.g., Apache-2.0, MIT, BSD-3-Clause).
        /// </summary>
        public string License { get; set; } = "Apache-2.0";

        /// <summary>
        /// Gets or sets the source code repository URL for the package.
        /// </summary>
        public string RepositoryUrl { get; set; } = "https://github.com/stariongroup/power-bus";

        /// <summary>
        /// Gets or sets the primary project homepage or external website URL.
        /// </summary>
        public string HomepageUrl { get; set; } = "https://stariongroup.eu";

        /// <summary>
        /// Gets or sets the hosted documentation or API reference URL.
        /// </summary>
        public string DocumentationUrl { get; set; } = "https://docs.stariongroup.eu";

        /// <summary>
        /// Gets or sets the collection of tag names associated with the package.
        /// </summary>
        public List<string> Tags { get; set; } = ["sysml-v2", "spacecraft", "power-system", "small-sat", "electrical"];

        /// <summary>
        /// Gets or sets a value indicating whether the package is deprecated.
        /// </summary>
        /// <remarks>
        /// This property is temporary and will be removed once dedicated deprecation lifecycle entities are implemented in the domain model.
        /// </remarks>
        public bool IsDeprecated { get; set; }

        /// <summary>
        /// Gets or sets the human-readable explanation or migration guidance for why the package was deprecated.
        /// </summary>
        /// <remarks>
        /// This property is temporary and will be removed once dedicated deprecation lifecycle entities are implemented in the domain model.
        /// </remarks>
        public string DeprecationReason { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the package publisher has verified credentials and cryptographic signatures.
        /// </summary>
        public bool IsVerified { get; set; } = true;
    }
}
