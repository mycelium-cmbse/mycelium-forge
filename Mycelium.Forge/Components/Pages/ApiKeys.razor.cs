// ------------------------------------------------------------------------------------------------
// <copyright file="ApiKeys.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Models;
    using Mycelium.Forge.ViewModels;

    /// <summary>
    /// Represents the API keys management page, allowing users to view, create, and revoke
    /// authentication tokens for CLI and CI package publication.
    /// </summary>
    public partial class ApiKeys : ComponentBase
    {
        /// <summary>
        /// Gets or sets the view model for the API keys page.
        /// </summary>
        [Inject]
        public IApiKeysViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the navigation manager instance.
        /// </summary>
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Handles the event to create a new API key.
        /// </summary>
        public void OnCreateKey()
        {
            var newKey = new ApiKeyModel(
                "4",
                "deploy-runner",
                "@starion",
                "publish",
                "Feb 2026",
                "in 6 months",
                "never");

            this.ViewModel.CreateApiKey(newKey);
        }

        /// <summary>
        /// Handles the event to revoke an existing API key.
        /// </summary>
        /// <param name="id">The unique identifier of the API key to revoke.</param>
        public void OnRevokeKey(string id)
        {
            this.ViewModel.RevokeApiKey(id);
        }

        /// <summary>
        /// Initializes the component lifecycle and populates the view model state.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.ViewModel.InitializeViewModel();
        }
    }
}
