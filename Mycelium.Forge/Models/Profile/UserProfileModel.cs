// ------------------------------------------------------------------------------------------------
// <copyright file="UserProfileModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.Profile
{
    using Mycelium.Forge.Common;

    /// <summary>
    /// Represents the user profile information displayed on the account settings page, wrapping the account DTO.
    /// </summary>
    public class UserProfileModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileModel" /> class.
        /// </summary>
        public UserProfileModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileModel" /> class with specified properties.
        /// </summary>
        /// <param name="account">The underlying user account DTO.</param>
        /// <param name="company">The company or organization affiliation of the user.</param>
        /// <param name="biography">The personal biography or summary description of the user.</param>
        /// <param name="isEmailVerified">A value indicating whether the primary email address is verified.</param>
        public UserProfileModel(
            IAccount account,
            string company = "",
            string biography = "",
            bool isEmailVerified = true)
        {
            this.Account = account;
            this.Company = company;
            this.Biography = biography;
            this.IsEmailVerified = isEmailVerified;
        }

        /// <summary>
        /// Gets or sets the underlying user account DTO.
        /// </summary>
        public IAccount Account { get; set; }

        /// <summary>
        /// Gets the unique username handle of the account.
        /// </summary>
        public string Username => this.Account != null ? $"@{this.Account.ShortName}" : string.Empty;

        /// <summary>
        /// Gets the primary email address associated with the account.
        /// </summary>
        public string Email => this.Account?.Email ?? string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the primary email address is verified.
        /// </summary>
        public bool IsEmailVerified { get; set; } = true;

        /// <summary>
        /// Gets the full display name of the user.
        /// </summary>
        public string DisplayName => this.Account?.Name ?? string.Empty;

        /// <summary>
        /// Gets or sets the company or organization affiliation of the user.
        /// </summary>
        public string Company { get; set; } = string.Empty;

        /// <summary>
        /// Gets the geographical location of the user.
        /// </summary>
        public string Location => this.Account?.Origin ?? string.Empty;

        /// <summary>
        /// Gets the personal or organization website URL of the user.
        /// </summary>
        public string Website => this.Account?.Website ?? string.Empty;

        /// <summary>
        /// Gets or sets the personal biography or summary description of the user.
        /// </summary>
        public string Biography { get; set; } = string.Empty;
    }
}
