// ------------------------------------------------------------------------------------------------
// <copyright file="AdminAccountModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.Admin
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Extensions;

    /// <summary>
    /// Represents an account entry displayed in the administrator accounts table wrapping the account DTO.
    /// </summary>
    public class AdminAccountModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminAccountModel" /> class.
        /// </summary>
        public AdminAccountModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminAccountModel" /> class with specified properties.
        /// </summary>
        /// <param name="account">The underlying user account DTO.</param>
        /// <param name="isAdministrator">A value indicating whether the user is an installation administrator.</param>
        /// <param name="verificationStatus">The email/identity verification status outcome.</param>
        /// <param name="organizations">The formatted summary of organizations and roles.</param>
        /// <param name="status">The account activity status.</param>
        public AdminAccountModel(
            IAccount account,
            bool isAdministrator = false,
            string verificationStatus = "Verified",
            string organizations = "",
            ScopeStatusKind status = ScopeStatusKind.ACTIVE)
        {
            this.Account = account;
            this.IsAdministrator = isAdministrator;
            this.VerificationStatus = verificationStatus;
            this.Organizations = organizations;
            this.Status = status;
        }

        /// <summary>
        /// Gets or sets the underlying user account DTO.
        /// </summary>
        public IAccount Account { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is an installation administrator.
        /// </summary>
        public bool IsAdministrator { get; set; }

        /// <summary>
        /// Gets or sets the email/identity verification status outcome.
        /// </summary>
        public string VerificationStatus { get; set; } = "Verified";

        /// <summary>
        /// Gets or sets the formatted summary of organizations and roles.
        /// </summary>
        public string Organizations { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the account activity status.
        /// </summary>
        public ScopeStatusKind Status { get; set; } = ScopeStatusKind.ACTIVE;

        /// <summary>
        /// Gets the unique identifier string from the account DTO.
        /// </summary>
        public string Id => this.Account?.Id.ToString() ?? string.Empty;

        /// <summary>
        /// Gets the full display name from the account DTO.
        /// </summary>
        public string Name => this.Account?.Name ?? string.Empty;

        /// <summary>
        /// Gets the formatted handle with leading at-symbol.
        /// </summary>
        public string Username => this.Account != null ? $"@{this.Account.ShortName}" : string.Empty;

        /// <summary>
        /// Gets the email address from the account DTO.
        /// </summary>
        public string Email => this.Account?.Email ?? string.Empty;

        /// <summary>
        /// Gets the uppercase initials extracted from the account name.
        /// </summary>
        public string Initials => (this.Account?.Name).ToInitials();
    }
}
