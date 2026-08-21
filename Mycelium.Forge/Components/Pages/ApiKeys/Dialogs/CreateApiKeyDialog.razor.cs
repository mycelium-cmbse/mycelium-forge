// ------------------------------------------------------------------------------------------------
// <copyright file="CreateApiKeyDialog.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.ApiKeys.Dialogs
{
    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Models.ApiKey;
    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.Services;

    /// <summary>
    /// Represents a dialog component for configuring and submitting a new API authentication key.
    /// </summary>
    public partial class CreateApiKeyDialog : ComponentBase
    {
        /// <summary>
        /// Gets the dictionary mapping expiration label options to their corresponding timespan durations.
        /// </summary>
        private static readonly IReadOnlyDictionary<string, TimeSpan> ExpirationOptions = new Dictionary<string, TimeSpan>
        {
            { "30 days", TimeSpan.FromDays(30) },
            { "60 days", TimeSpan.FromDays(60) },
            { "90 days", TimeSpan.FromDays(90) },
            { "6 months", TimeSpan.FromDays(180) },
            { "1 year", TimeSpan.FromDays(365) },
            { "No expiration", TimeSpan.Zero }
        };

        /// <summary>
        /// Gets or sets the cascading dialog reference used to control and close the dialog.
        /// </summary>
        [CascadingParameter]
        public IDialogReference DialogReference { get; set; }

        /// <summary>
        /// Gets or sets the list of available target scopes or organizations.
        /// </summary>
        [Parameter]
        public IReadOnlyList<string> Scopes { get; set; } =
        [
            "@starion",
            "@esa",
            "Spacecraft Mission",
            "CubeSat Constellation"
        ];

        /// <summary>
        /// Gets or sets the event callback invoked when the user confirms creating the API key.
        /// </summary>
        [Parameter]
        public EventCallback<CreateApiKeyResult> OnResult { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the dialog is cancelled or closed.
        /// </summary>
        [Parameter]
        public EventCallback OnCancel { get; set; }

        /// <summary>
        /// Gets the validation manager instance handling field validation states.
        /// </summary>
        public ValidationManager ValidationManager { get; } = new();

        /// <summary>
        /// Gets or sets the name for the new API key.
        /// </summary>
        public string KeyName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the currently selected scope for the API key.
        /// </summary>
        public string SelectedScope { get; set; } = "@starion";

        /// <summary>
        /// Gets or sets the list of available expiration duration option keys.
        /// </summary>
        public IReadOnlyList<string> ExpirationOptionKeys { get; set; } = [.. ExpirationOptions.Keys];

        /// <summary>
        /// Gets or sets the currently selected expiration duration option.
        /// </summary>
        public string SelectedExpiration { get; set; } = ExpirationOptions.Keys.FirstOrDefault();

        /// <summary>
        /// Gets or sets the list of permission configuration options for the new API key.
        /// </summary>
        public List<ApiKeyPermissionModel> Permissions { get; set; } =
        [
            new()
            {
                Name = "publish",
                Label = "publish (push new packages and versions)",
                IsGranted = true
            },
            new()
            {
                Name = "unlist",
                Label = "unlist (hide a published version)",
                IsGranted = false
            },
            new()
            {
                Name = "manage-keys",
                Label = "manage-keys (create and revoke keys)",
                IsGranted = false
            }
        ];

        /// <summary>
        /// Handles changes to the key name input value.
        /// </summary>
        /// <param name="value">The new key name.</param>
        public void OnKeyNameChanged(string value)
        {
            this.KeyName = value ?? string.Empty;
            this.ValidationManager.ClearError(nameof(this.KeyName));
        }

        /// <summary>
        /// Handles changes to the selected scope value.
        /// </summary>
        /// <param name="value">The newly selected scope.</param>
        public void OnScopeChanged(string value)
        {
            this.SelectedScope = value ?? string.Empty;
            this.ValidationManager.ClearError(nameof(this.SelectedScope));
        }

        /// <summary>
        /// Handles the cancel action, cancelling the dialog and invoking the cancel callback.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnCancelClicked()
        {
            await this.OnCancel.InvokeAsync();

            if (this.DialogReference != null)
            {
                await this.DialogReference.CancelAsync();
            }
        }

        /// <summary>
        /// Handles the create key action, validating and emitting the configured API key details before closing.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnCreateKeyClicked()
        {
            var isValid = this.ValidationManager
                .Check(nameof(this.KeyName), !string.IsNullOrWhiteSpace(this.KeyName), "Key name is required.")
                .Check(nameof(this.SelectedScope), !string.IsNullOrWhiteSpace(this.SelectedScope), "Scope selection is required.")
                .IsValid;

            if (!isValid)
            {
                return;
            }

            var duration = ExpirationOptions.TryGetValue(this.SelectedExpiration, out var timespan)
                ? timespan
                : TimeSpan.FromDays(180);

            var expiresAt = duration == TimeSpan.Zero
                ? DateTime.MaxValue
                : DateTime.UtcNow.Add(duration);

            var result = new CreateApiKeyResult
            {
                KeyName = this.KeyName.Trim(),
                Scope = this.SelectedScope,
                Permissions = this.Permissions,
                Expiration = this.SelectedExpiration,
                ExpiresAt = expiresAt
            };

            if (this.DialogReference != null)
            {
                await this.DialogReference.CloseAsync(DialogResult.Ok(result));
            }

            await this.OnResult.InvokeAsync(result);
        }

        /// <summary>
        /// Initializes default values based on passed parameters when component parameters are initialized.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.SelectedScope = this.Scopes[0];
        }
    }
}
