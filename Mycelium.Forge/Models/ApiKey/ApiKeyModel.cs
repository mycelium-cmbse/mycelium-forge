// ------------------------------------------------------------------------------------------------
// <copyright file="ApiKeyModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Models.ApiKey
{
    using System;
    using Mycelium.Forge.Common;

    /// <summary>
    /// Represents an API key presentation model wrapping the underlying APIKey entity with display properties.
    /// </summary>
    public class ApiKeyModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiKeyModel" /> class.
        /// </summary>
        public ApiKeyModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiKeyModel" /> class with specified properties.
        /// </summary>
        /// <param name="apiKey">The underlying <see cref="IAPIKey" /> DTO.</param>
        /// <param name="scope">The scope or organization associated with the API key.</param>
        /// <param name="permissionsText">The summary text of granted permissions.</param>
        /// <param name="secretToken">The optional revealed plain-text secret token.</param>
        public ApiKeyModel(
            IAPIKey apiKey,
            string scope = "@starion",
            string permissionsText = "publish",
            string secretToken = "")
        {
            this.ApiKey = apiKey;
            this.Scope = scope;
            this.PermissionsText = permissionsText;
            this.SecretToken = secretToken;
        }

        /// <summary>
        /// Gets or sets the underlying API key entity.
        /// </summary>
        public IAPIKey ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the scope or organization associated with the API key.
        /// </summary>
        public string Scope { get; set; } = "@starion";

        /// <summary>
        /// Gets or sets the summary description of granted permissions.
        /// </summary>
        public string PermissionsText { get; set; } = "publish";

        /// <summary>
        /// Gets or sets the one-time plain-text secret token string.
        /// </summary>
        public string SecretToken { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the human-readable expiration summary text.
        /// </summary>
        public string ExpirationText { get; set; } = string.Empty;

        /// <summary>
        /// Gets the unique identifier of the API key.
        /// </summary>
        public Guid Id => this.ApiKey?.Id ?? Guid.Empty;

        /// <summary>
        /// Gets the name of the API key.
        /// </summary>
        public string Name => this.ApiKey?.Name ?? string.Empty;

        /// <summary>
        /// Gets the creation timestamp of the API key.
        /// </summary>
        public DateTime CreatedAt => this.ApiKey?.CreatedAt ?? DateTime.MinValue;

        /// <summary>
        /// Gets the expiration timestamp of the API key.
        /// </summary>
        public DateTime ExpiresAt => this.ApiKey?.ExpiresAt ?? DateTime.MaxValue;

        /// <summary>
        /// Gets the timestamp when the API key was last used.
        /// </summary>
        public DateTime LastUsedAt => this.ApiKey?.LastUsedAt ?? default;
    }
}
