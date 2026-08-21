// ------------------------------------------------------------------------------------------------
// <copyright file="IApiKeysViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.ApiKeys
{
    using System;
    using System.Collections.Generic;

    using Mycelium.Forge.Models.ApiKey;
    using Mycelium.Forge.Models.DialogResults;

    /// <summary>
    /// Defines the view model contract for the API keys management page.
    /// </summary>
    public interface IApiKeysViewModel
    {
        /// <summary>
        /// Gets or sets the collection of API keys displayed in the table.
        /// </summary>
        List<ApiKeyModel> ApiKeys { get; set; }

        /// <summary>
        /// Initializes the view model state and populates the API key collection.
        /// </summary>
        void InitializeViewModel();

        /// <summary>
        /// Creates and adds a new API key model entry to the collection.
        /// </summary>
        /// <param name="apiKey">The <see cref="ApiKeyModel" /> to create.</param>
        void CreateApiKey(ApiKeyModel apiKey);

        /// <summary>
        /// Creates and stores a new API key based on the dialog configuration result, returning the created model.
        /// </summary>
        /// <param name="result">The <see cref="CreateApiKeyResult" /> containing the key parameters.</param>
        /// <returns>The created <see cref="ApiKeyModel" /> containing the entity and revealed plain-text secret token.</returns>
        ApiKeyModel CreateApiKey(CreateApiKeyResult result);

        /// <summary>
        /// Revokes an API key with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the API key to revoke.</param>
        void RevokeApiKey(Guid id);
    }
}
