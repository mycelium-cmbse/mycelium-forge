// ------------------------------------------------------------------------------------------------
// <copyright file="SignUpModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents the registration form inputs for creating a new user account.
    /// </summary>
    public class SignUpModel
    {
        /// <summary>
        /// Gets or sets the installation-unique username handle.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the primary email address for the account.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the account password.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
