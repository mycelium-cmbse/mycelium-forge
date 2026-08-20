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

    /// <summary>
    /// Provides view model state and operations for the API keys management page.
    /// </summary>
    public class ApiKeysViewModel : IApiKeysViewModel
    {
        /// <summary>
        /// The master collection of seed API key entries.
        /// </summary>
        private readonly List<APIKey> seedKeys =
        [
            new()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "ci-publish",
                CreatedAt = new DateTime(2026, 1, 1),
                ExpiresAt = new DateTime(2026, 7, 1),
                LastUsedAt = DateTime.UtcNow.AddDays(-3)
            },
            new()
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "release-bot",
                CreatedAt = new DateTime(2025, 12, 1),
                ExpiresAt = new DateTime(2026, 6, 1),
                LastUsedAt = DateTime.UtcNow.AddDays(-14)
            },
            new()
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "local-dev",
                CreatedAt = new DateTime(2025, 11, 1),
                ExpiresAt = new DateTime(2026, 4, 1),
                LastUsedAt = DateTime.UtcNow.AddDays(-30)
            }
        ];

        /// <summary>
        /// Gets or sets the collection of API keys.
        /// </summary>
        public IReadOnlyList<APIKey> ApiKeys { get; set; } = [];

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
        /// <param name="apiKey">The <see cref="APIKey" /> to create.</param>
        public void CreateApiKey(APIKey apiKey)
        {
            this.seedKeys.Add(apiKey);
            this.ApiKeys = [.. this.seedKeys];
        }

        /// <summary>
        /// Revokes an API key with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the API key to revoke.</param>
        public void RevokeApiKey(Guid id)
        {
            this.seedKeys.RemoveAll(x => x.Id == id);
            this.ApiKeys = [.. this.seedKeys];
        }
    }
}
