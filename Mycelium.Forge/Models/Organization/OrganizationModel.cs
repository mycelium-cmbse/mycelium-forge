// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.Organization
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extensions;

    /// <summary>
    /// Represents an organization presentation model wrapping the organization DTO and exposing computed attributes.
    /// </summary>
    public class OrganizationModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationModel" /> class.
        /// </summary>
        public OrganizationModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationModel" /> class with specified properties.
        /// </summary>
        /// <param name="organization">The underlying organization DTO.</param>
        /// <param name="initials">The short abbreviation or initials.</param>
        /// <param name="description">The organization description text.</param>
        /// <param name="isVerified">A value indicating whether the organization publisher is verified.</param>
        /// <param name="packageCount">The number of published packages.</param>
        /// <param name="versionCount">The total number of release versions.</param>
        /// <param name="importCount">The total import count across packages.</param>
        /// <param name="memberSinceYear">The year the organization was registered.</param>
        public OrganizationModel(
            IOrganization organization,
            string initials = "",
            string description = "",
            bool isVerified = true,
            int packageCount = 0,
            int versionCount = 0,
            int importCount = 0,
            int memberSinceYear = 2025)
        {
            this.Organization = organization;

            this.Initials = !string.IsNullOrEmpty(initials)
                ? initials
                : (organization?.Name).ToInitials();

            this.Description = !string.IsNullOrEmpty(description)
                ? description
                : organization?.Origin ?? string.Empty;

            this.IsVerified = isVerified;
            this.PackageCount = packageCount;
            this.VersionCount = versionCount;
            this.ImportCount = importCount;
            this.MemberSinceYear = memberSinceYear;
        }

        /// <summary>
        /// Gets or sets the underlying organization DTO.
        /// </summary>
        public IOrganization Organization { get; set; }

        /// <summary>
        /// Gets the organization name from the underlying DTO.
        /// </summary>
        public string Name => this.Organization?.Name ?? string.Empty;

        /// <summary>
        /// Gets the scope namespace identifier (e.g., @starion) computed from the short name.
        /// </summary>
        public string Scope => this.Organization != null ? $"@{this.Organization.ShortName}" : string.Empty;

        /// <summary>
        /// Gets or sets the short abbreviation or initials.
        /// </summary>
        public string Initials { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the organization description text.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the organization publisher is verified.
        /// </summary>
        public bool IsVerified { get; set; }

        /// <summary>
        /// Gets or sets the number of published packages.
        /// </summary>
        public int PackageCount { get; set; }

        /// <summary>
        /// Gets or sets the total number of release versions.
        /// </summary>
        public int VersionCount { get; set; }

        /// <summary>
        /// Gets or sets the total import count across packages.
        /// </summary>
        public int ImportCount { get; set; }

        /// <summary>
        /// Gets or sets the year the organization was registered.
        /// </summary>
        public int MemberSinceYear { get; set; } = 2025;
    }
}
