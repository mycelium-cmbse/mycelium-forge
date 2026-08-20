// ------------------------------------------------------------------------------------------------
// <copyright file="OrganizationInvitationModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models
{
    /// <summary>
    /// Represents a pending membership invitation sent to an external collaborator or member.
    /// </summary>
    public class OrganizationInvitationModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationInvitationModel" /> class.
        /// </summary>
        public OrganizationInvitationModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationInvitationModel" /> class with specified properties.
        /// </summary>
        /// <param name="email">The invitee email address.</param>
        /// <param name="role">The assigned role upon accepting the invitation.</param>
        /// <param name="statusText">The expiration or invitation status text.</param>
        public OrganizationInvitationModel(string email, string role, string statusText = "")
        {
            this.Email = email;
            this.Role = role;
            this.StatusText = statusText;
        }

        /// <summary>
        /// Gets or sets the invitee email address.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the assigned role upon accepting the invitation.
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the expiration or invitation status text.
        /// </summary>
        public string StatusText { get; set; } = string.Empty;
    }
}
