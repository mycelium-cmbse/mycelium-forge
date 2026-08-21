// ------------------------------------------------------------------------------------------------
// <copyright file="ApiKeysViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.Data;
    using Mycelium.Forge.Models.ApiKey;
    using Mycelium.Forge.Models.DialogResults;

    /// <summary>
    /// Provides view model state and operations for the API keys management page.
    /// </summary>
    public class ApiKeysViewModel : IApiKeysViewModel
    {
        /// <summary>
        /// Gets or sets the collection of API keys.
        /// </summary>
        public List<ApiKeyModel> ApiKeys { get; set; } = [];

        /// <summary>
        /// Initializes the view model state and populates the API keys collection.
        /// </summary>
        public void InitializeViewModel()
        {
            this.ApiKeys = SeedData.ApiKeys
                .Select(x => new ApiKeyModel(x))
                .ToList();
        }

        /// <summary>
        /// Creates and adds a new API key model entry to the collection.
        /// </summary>
        /// <param name="apiKey">The <see cref="ApiKeyModel" /> to create.</param>
        public void CreateApiKey(ApiKeyModel apiKey)
        {
            this.ApiKeys.Add(apiKey);
        }

        /// <summary>
        /// Creates and stores a new API key based on the dialog configuration result, returning the created model.
        /// </summary>
        /// <param name="result">The <see cref="CreateApiKeyResult" /> containing the key parameters.</param>
        /// <returns>The created <see cref="ApiKeyModel" /> containing the entity and revealed plain-text secret token.</returns>
        public ApiKeyModel CreateApiKey(CreateApiKeyResult result)
        {
            var apiKey = new APIKey
            {
                Id = Guid.NewGuid(),
                Name = result.KeyName,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = result.ExpiresAt
            };

            var randomBytes = new byte[18];
            Random.Shared.NextBytes(randomBytes);

            var base64 = Convert.ToBase64String(randomBytes)
                .Replace("+", "x")
                .Replace("/", "y")
                .Replace("=", "");

            var secretToken = $"forge_pat_{base64}";

            var granted = result.Permissions.Where(x => x.IsGranted).Select(x => x.Name).ToList();
            var permissionsText = granted.Count > 0 ? string.Join(", ", granted) : "none";

            var expirationText = result.Expiration == "No expiration" || result.ExpiresAt == DateTime.MaxValue
                ? "never"
                : $"in {result.Expiration}";

            var model = new ApiKeyModel(apiKey, result.Scope, permissionsText, secretToken)
            {
                ExpirationText = expirationText
            };

            this.ApiKeys.Add(model);

            return model;
        }

        /// <summary>
        /// Revokes an API key with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the API key to revoke.</param>
        public void RevokeApiKey(Guid id)
        {
            this.ApiKeys.RemoveAll(x => x.Id == id);
        }
    }
}
