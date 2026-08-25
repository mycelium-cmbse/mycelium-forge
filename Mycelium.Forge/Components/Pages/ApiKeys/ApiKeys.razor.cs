// ------------------------------------------------------------------------------------------------
// <copyright file="ApiKeys.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.ApiKeys
{
    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Components.Pages.ApiKeys.Dialogs;
    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.ViewModels.ApiKeys;

    /// <summary>
    /// Represents the API keys management page, allowing users to view, create, and revoke
    /// authentication tokens for CLI and CI package publication.
    /// </summary>
    public partial class ApiKeys : ComponentBase
    {
        /// <summary>
        /// Gets or sets the dialog service used to display modal dialogs.
        /// </summary>
        [Inject]
        public DialogService DialogService { get; set; }

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
        /// Handles the event to create a new API key by opening the creation dialog.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnCreateKey()
        {
            var onResult = new EventCallbackFactory().Create(this, (CreateApiKeyResult result) => this.HandleCreateApiKey(result));

            var parameters = new Dictionary<string, object>
            {
                { nameof(CreateApiKeyDialog.OnResult), onResult }
            };

            var options = new DialogOpenOptions
            {
                Title = "Create API key",
                Description = "The secret is shown once, right after creation."
            };

            await this.DialogService.OpenAsync<CreateApiKeyDialog>(parameters, options);
        }

        /// <summary>
        /// Handles the result from the create API key dialog, delegates to the viewmodel to create the key,
        /// and opens the success dialog to reveal the generated secret token.
        /// </summary>
        /// <param name="result">The dialog result containing the key configuration.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task HandleCreateApiKey(CreateApiKeyResult result)
        {
            var createdKey = this.ViewModel.CreateApiKey(result);

            var onDone = new EventCallbackFactory().Create(this, () => { });

            var parameters = new Dictionary<string, object>
            {
                { nameof(ApiKeyCreatedDialog.CreatedKey), createdKey },
                { nameof(ApiKeyCreatedDialog.OnDone), onDone }
            };

            var options = new DialogOpenOptions
            {
                Title = "API key created",
                Description = "Copy it now: you will not be able to see it again."
            };

            await this.DialogService.OpenAsync<ApiKeyCreatedDialog>(parameters, options);
        }

        /// <summary>
        /// Handles the event to revoke an existing API key.
        /// </summary>
        /// <param name="id">The unique identifier of the API key to revoke.</param>
        public void OnRevokeKey(Guid id)
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
