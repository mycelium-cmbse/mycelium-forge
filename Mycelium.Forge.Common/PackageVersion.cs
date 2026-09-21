// ------------------------------------------------------------------------------------------------
// <copyright file="PackageVersion.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    /// <summary>
    /// Partial class implementation for <see cref="PackageVersion" /> providing release content, file integrity attributes, and deprecation properties.
    /// </summary>
    /// <remarks>
    /// This partial class is temporary and will be removed once the domain metamodel is updated to include release README documentation, changelog release notes, artifact file size, cryptographic checksums, digital signatures, and release deprecation attributes in the code-generated DTO.
    /// </remarks>
    public partial class PackageVersion
    {
        /// <summary>
        /// Gets or sets the README markdown content associated with this specific package release.
        /// </summary>
        public string Readme { get; set; } = "Reusable SysML v2 library providing electrical and systems architecture.";

        /// <summary>
        /// Gets or sets the release changelog or release notes in markdown format describing changes in this version.
        /// </summary>
        public string ReleaseNotes { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the binary artifact size in bytes.
        /// </summary>
        /// <remarks>
        /// This property is temporary and will be removed once a dedicated File domain entity is implemented in the model.
        /// </remarks>
        public long SizeInBytes { get; set; } = 150000;

        /// <summary>
        /// Gets or sets the cryptographic SHA-256 checksum of the package artifact file.
        /// </summary>
        /// <remarks>
        /// This property is temporary and will be removed once a dedicated File domain entity is implemented in the model.
        /// </remarks>
        public string Checksum { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this specific package version is deprecated.
        /// </summary>
        /// <remarks>
        /// This property is temporary and will be removed once dedicated deprecation entities are implemented in the model.
        /// </remarks>
        public bool IsDeprecated { get; set; } = false;
    }
}
