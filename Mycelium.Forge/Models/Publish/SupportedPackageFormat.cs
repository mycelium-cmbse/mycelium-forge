// ------------------------------------------------------------------------------------------------
// <copyright file="SupportedPackageFormat.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.Publish
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extractors.Enums;

    /// <summary>
    /// Represents a supported package file format and its associated artifact kind.
    /// </summary>
    public sealed record SupportedPackageFormat
    {
        /// <summary>
        /// The collection of all supported package formats recognized by the registry.
        /// </summary>
        public static readonly IReadOnlyList<SupportedPackageFormat> All =
        [
            new(PackageFormatConstants.SysMlV2, ".kpar", ArtifactKind.Kpar),
            new(PackageFormatConstants.Cdp4Comet, ".zip", ArtifactKind.EcssC3),
            new(PackageFormatConstants.Capella, ".zip", ArtifactKind.Capella)
        ];

        /// <summary>
        /// Initializes a new instance of the <see cref="SupportedPackageFormat" /> record.
        /// </summary>
        /// <param name="name">The human-readable format name matching the database package type name.</param>
        /// <param name="extension">The supported file extension.</param>
        /// <param name="kind">The associated <see cref="ArtifactKind" />.</param>
        private SupportedPackageFormat(string name, string extension, ArtifactKind kind)
        {
            this.Name = name;
            this.Extension = extension;
            this.Kind = kind;
        }

        /// <summary>
        /// Gets the human-readable format name matching the database package type name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the supported file extension.
        /// </summary>
        public string Extension { get; }

        /// <summary>
        /// Gets the associated <see cref="ArtifactKind" />.
        /// </summary>
        public ArtifactKind Kind { get; }
    }
}
