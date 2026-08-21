// ------------------------------------------------------------------------------------------------
// <copyright file="ApiKeyCreatedDialog.razor.cs" company="Starion Group S.A.">
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

    /// <summary>
    /// Represents a dialog component for displaying a newly created API authentication key secret token.
    /// </summary>
    public partial class ApiKeyCreatedDialog : ComponentBase
    {
        /// <summary>
        /// Gets or sets the cascading dialog reference used to control and close the dialog.
        /// </summary>
        [CascadingParameter]
        public IDialogReference DialogReference { get; set; }

        /// <summary>
        /// Gets or sets the created API key model containing the secret token and metadata.
        /// </summary>
        [Parameter]
        public ApiKeyModel CreatedKey { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the dialog is dismissed or completed.
        /// </summary>
        [Parameter]
        public EventCallback OnDone { get; set; }

        /// <summary>
        /// Handles the done action, invoking the callback and closing the dialog.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnDoneClicked()
        {
            if (this.DialogReference != null)
            {
                await this.DialogReference.CloseAsync(DialogResult.Ok());
            }

            await this.OnDone.InvokeAsync();
        }
    }
}
