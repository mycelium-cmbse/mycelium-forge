// ------------------------------------------------------------------------------------------------
// <copyright file="CreateOrganizationResult.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.DialogResults
{
    /// <summary>
    /// Represents the result payload containing configuration details when creating a new organization.
    /// </summary>
    public class CreateOrganizationResult
    {
        /// <summary>
        /// Gets or sets the full display name of the organization.
        /// </summary>
        public string OrganizationName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the scope identifier for the organization (e.g., @esa).
        /// </summary>
        public string Scope { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the billing email address for the organization.
        /// </summary>
        public string BillingEmail { get; set; } = string.Empty;
    }
}
