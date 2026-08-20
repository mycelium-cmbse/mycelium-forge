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

    /// <summary>
    /// Provides view model state and operations for the API keys management page.
    /// </summary>
    public class ApiKeysViewModel : IApiKeysViewModel
    {
        /// <summary>
        /// Gets or sets the collection of API keys.
        /// </summary>
        public List<APIKey> ApiKeys { get; set; } = [];

        /// <summary>
        /// Initializes the view model state and populates the API keys collection.
        /// </summary>
        public void InitializeViewModel()
        {
            this.ApiKeys = [.. SeedData.ApiKeys];
        }

        /// <summary>
        /// Creates and adds a new API key entry to the collection.
        /// </summary>
        /// <param name="apiKey">The <see cref="APIKey" /> to create.</param>
        public void CreateApiKey(APIKey apiKey)
        {
            this.ApiKeys.Add(apiKey);
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
