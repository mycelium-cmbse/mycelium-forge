// ------------------------------------------------------------------------------------------------
// <copyright file="CreateOrganizationDialog.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Common
{
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Models.DialogResults;

    /// <summary>
    /// Represents a dialog component for creating a new organization within Mycelium Forge.
    /// </summary>
    public partial class CreateOrganizationDialog : ComponentBase
    {
        /// <summary>
        /// Gets or sets the cascading dialog reference used to control and close the dialog.
        /// </summary>
        [CascadingParameter]
        public IDialogReference DialogReference { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the user confirms the creation of an organization.
        /// </summary>
        [Parameter]
        public EventCallback<CreateOrganizationResult> OnResult { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the dialog is cancelled or closed.
        /// </summary>
        [Parameter]
        public EventCallback OnCancel { get; set; }

        /// <summary>
        /// Gets or sets the organization name input value.
        /// </summary>
        public string OrganizationName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the organization scope input value.
        /// </summary>
        public string Scope { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the organization billing email input value.
        /// </summary>
        public string BillingEmail { get; set; } = string.Empty;

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
        /// Handles the create organization action, emitting the entered values before closing.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnCreateOrganizationClicked()
        {
            var result = new CreateOrganizationResult
            {
                OrganizationName = this.OrganizationName,
                Scope = this.Scope,
                BillingEmail = this.BillingEmail
            };

            await this.OnResult.InvokeAsync(result);

            if (this.DialogReference != null)
            {
                await this.DialogReference.CloseAsync(DialogResult.Ok(result));
            }
        }
    }
}
