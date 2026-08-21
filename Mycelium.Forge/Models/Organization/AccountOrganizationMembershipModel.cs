// ------------------------------------------------------------------------------------------------
// <copyright file="AccountOrganizationMembershipModel.cs" company="Starion Group S.A.">
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
    /// Represents an organization that a user account is a member of, wrapping the organization DTO.
    /// </summary>
    public class AccountOrganizationMembershipModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountOrganizationMembershipModel" /> class.
        /// </summary>
        public AccountOrganizationMembershipModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountOrganizationMembershipModel" /> class with specified properties.
        /// </summary>
        /// <param name="organization">The underlying organization DTO.</param>
        /// <param name="role">The user's role within this organization.</param>
        public AccountOrganizationMembershipModel(
            IOrganization organization,
            OrganizationInvitationKind role = OrganizationInvitationKind.MEMBER)
        {
            this.Organization = organization;
            this.Role = role;
        }

        /// <summary>
        /// Gets or sets the underlying organization DTO.
        /// </summary>
        public IOrganization Organization { get; set; }

        /// <summary>
        /// Gets or sets the user's role within this organization.
        /// </summary>
        public OrganizationInvitationKind Role { get; set; } = OrganizationInvitationKind.MEMBER;

        /// <summary>
        /// Gets a value indicating whether the user is an administrator of the organization.
        /// </summary>
        public bool IsAdministrator => this.Role == OrganizationInvitationKind.ADMINISTRATOR;

        /// <summary>
        /// Gets the organization scope namespace identifier (e.g., @starion).
        /// </summary>
        public string Scope => this.Organization != null ? $"@{this.Organization.ShortName}" : string.Empty;

        /// <summary>
        /// Gets the full display name of the organization.
        /// </summary>
        public string Name => this.Organization?.Name ?? string.Empty;

        /// <summary>
        /// Gets the uppercase initials extracted from the organization name.
        /// </summary>
        public string Initials => (this.Organization?.Name).ToInitials();
    }
}
