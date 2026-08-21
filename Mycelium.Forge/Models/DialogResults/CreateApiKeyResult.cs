// ------------------------------------------------------------------------------------------------
// <copyright file="CreateApiKeyResult.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.DialogResults
{
    using System;
    using System.Collections.Generic;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Models;

    /// <summary>
    /// Represents the result payload and created state when configuring and generating a new API key.
    /// </summary>
    public class CreateApiKeyResult
    {
        /// <summary>
        /// Gets or sets the name of the API key.
        /// </summary>
        public string KeyName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the selected scope or organization for the API key.
        /// </summary>
        public string Scope { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of permissions configured for the API key.
        /// </summary>
        public List<ApiKeyPermissionModel> Permissions { get; set; } = [];

        /// <summary>
        /// Gets or sets the human-readable expiration option string.
        /// </summary>
        public string Expiration { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the calculated expiration date and time.
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Gets or sets the generated plain-text secret token string.
        /// </summary>
        public string SecretToken { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the created API key entity instance.
        /// </summary>
        public APIKey CreatedKey { get; set; }

        /// <summary>
        /// Gets or sets the summary description of granted permissions.
        /// </summary>
        public string PermissionsText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the human-readable expiration summary text.
        /// </summary>
        public string ExpirationText { get; set; } = string.Empty;
    }
}
