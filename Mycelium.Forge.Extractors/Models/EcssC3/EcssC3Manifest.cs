// ------------------------------------------------------------------------------------------------
// <copyright file="EcssC3Manifest.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Models.EcssC3
{
    using Mycelium.Forge.Extractors.Enums;

    /// <summary>
    /// Represents the registry metadata projected from the Header.json of an ECSS-E-TM-10-25 Annex C.3 exchange file.
    /// </summary>
    public class EcssC3Manifest : IArtifactManifest
    {
        /// <summary>
        /// Gets the format of the artifact the manifest was extracted from.
        /// </summary>
        public ArtifactKind Kind => ArtifactKind.EcssC3;

        /// <summary>
        /// Gets or sets the media type of the exchange file.
        /// </summary>
        public string MediaType { get; set; }

        /// <summary>
        /// Gets or sets the data model version of the exchange file.
        /// </summary>
        public string DataModelVersion { get; set; }

        /// <summary>
        /// Gets or sets the exchange file format version.
        /// </summary>
        public string ExchangeFileFormatVersion { get; set; }

        /// <summary>
        /// Gets or sets the creator organization name.
        /// </summary>
        public string CreatorOrganization { get; set; }

        /// <summary>
        /// Gets or sets the creator person display name.
        /// </summary>
        public string CreatorPerson { get; set; }

        /// <summary>
        /// Gets or sets the copyright statement.
        /// </summary>
        public string Copyright { get; set; }

        /// <summary>
        /// Gets or sets the remark or description.
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Gets or sets the application-specific extensions.
        /// </summary>
        public string Extensions { get; set; }

        /// <summary>
        /// Gets or sets the creation timestamp.
        /// </summary>
        public DateTimeOffset CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the last modified timestamp.
        /// </summary>
        public DateTimeOffset? LastModifiedOn { get; set; }
    }
}
