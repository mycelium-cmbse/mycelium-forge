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
    using Mycelium.Forge.Models;

    /// <summary>
    /// Provides view model state and operations for the API keys management page.
    /// </summary>
    public class ApiKeysViewModel : IApiKeysViewModel
    {
        /// <summary>
        /// The master collection of seed API key entries.
        /// </summary>
        private readonly List<ApiKeyModel> seedKeys =
        [
            new("1", "ci-publish", "@starion", "publish", "Jan 2026", "in 5 months", "3 days ago"),
            new("2", "release-bot", "@starion", "publish, unlist", "Dec 2025", "in 4 months", "2 weeks ago"),
            new("3", "local-dev", "@mycelium", "publish", "Nov 2025", "in 2 months", "1 month ago")
        ];

        /// <summary>
        /// Gets or sets the collection of API keys.
        /// </summary>
        public IReadOnlyList<ApiKeyModel> ApiKeys { get; set; } = [];

        /// <summary>
        /// Initializes the view model state and populates the API keys collection.
        /// </summary>
        public void InitializeViewModel()
        {
            this.ApiKeys = [.. this.seedKeys];
        }

        /// <summary>
        /// Creates and adds a new API key entry to the collection.
        /// </summary>
        /// <param name="apiKey">The <see cref="ApiKeyModel" /> to create.</param>
        public void CreateApiKey(ApiKeyModel apiKey)
        {
            this.seedKeys.Add(apiKey);
            this.ApiKeys = [.. this.seedKeys];
        }

        /// <summary>
        /// Revokes an API key with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the API key to revoke.</param>
        public void RevokeApiKey(string id)
        {
            this.seedKeys.RemoveAll(x => x.Id == id);
            this.ApiKeys = [.. this.seedKeys];
        }
    }
}
