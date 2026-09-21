// ------------------------------------------------------------------------------------------------
// <copyright file="IPackageVersion.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common
{
    /// <summary>
    /// Partial interface extending <see cref="IPackageVersion" /> with release artifact contents, notes, and file integrity attributes.
    /// </summary>
    /// <remarks>
    /// This partial interface is temporary and will be removed once the domain metamodel is updated to include release README documentation, changelog release notes, artifact file size, cryptographic checksums, digital signatures, and release deprecation attributes in the code-generated DTO.
    /// </remarks>
    public partial interface IPackageVersion
    {
        /// <summary>
        /// Gets or sets the README markdown content associated with this specific package release.
        /// </summary>
        string Readme { get; set; }

        /// <summary>
        /// Gets or sets the release changelog or release notes in markdown format describing changes in this version.
        /// </summary>
        string ReleaseNotes { get; set; }

        /// <summary>
        /// Gets or sets the binary artifact size in bytes.
        /// </summary>
        /// <remarks>
        /// This property is temporary and will be removed once a dedicated File domain entity is implemented in the model.
        /// </remarks>
        long SizeInBytes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this specific package version is deprecated.
        /// </summary>
        /// <remarks>
        /// This property is temporary and will be removed once dedicated deprecation entities are implemented in the model.
        /// </remarks>
        bool IsDeprecated { get; set; }
    }
}
