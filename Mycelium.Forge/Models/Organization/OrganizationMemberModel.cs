// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationMemberModel.cs" company="Starion Group S.A.">
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
    /// Represents a member within an organization team wrapping their user account DTO.
    /// </summary>
    public class OrganizationMemberModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationMemberModel" /> class.
        /// </summary>
        public OrganizationMemberModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationMemberModel" /> class with specified properties.
        /// </summary>
        /// <param name="account">The underlying user account DTO.</param>
        /// <param name="role">The member's organizational role.</param>
        public OrganizationMemberModel(IAccount account, OrganizationInvitationKind role = OrganizationInvitationKind.MEMBER)
        {
            this.Account = account;
            this.Role = role;
        }

        /// <summary>
        /// Gets or sets the underlying user account DTO.
        /// </summary>
        public IAccount Account { get; set; }

        /// <summary>
        /// Gets or sets the member's organizational role.
        /// </summary>
        public OrganizationInvitationKind Role { get; set; } = OrganizationInvitationKind.MEMBER;

        /// <summary>
        /// Gets the full display name from the underlying account.
        /// </summary>
        public string Name => this.Account?.Name ?? string.Empty;

        /// <summary>
        /// Gets the formatted handle with leading at-symbol.
        /// </summary>
        public string Username => this.Account != null ? $"@{this.Account.ShortName}" : string.Empty;

        /// <summary>
        /// Gets the uppercase initials extracted from the account name.
        /// </summary>
        public string Initials => (this.Account?.Name).ToInitials();
    }
}
