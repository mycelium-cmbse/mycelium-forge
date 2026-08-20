// ------------------------------------------------------------------------------------------------
// <copyright file="IApiKeysViewModel.cs" company="Starion Group S.A.">
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
    /// Defines the view model contract for the API keys management page.
    /// </summary>
    public interface IApiKeysViewModel
    {
        /// <summary>
        /// Gets or sets the collection of API keys displayed in the table.
        /// </summary>
        List<APIKey> ApiKeys { get; set; }

        /// <summary>
        /// Initializes the view model state and populates the API key collection.
        /// </summary>
        void InitializeViewModel();

        /// <summary>
        /// Creates and adds a new API key entry to the collection.
        /// </summary>
        /// <param name="apiKey">The <see cref="APIKey" /> to create.</param>
        void CreateApiKey(APIKey apiKey);

        /// <summary>
        /// Revokes an API key with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the API key to revoke.</param>
        void RevokeApiKey(Guid id);
    }
}
