// ------------------------------------------------------------------------------------------------
// <copyright file="CreateOrganizationDialog.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.AccountSettings.Dialogs
{
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Models.DialogResults;
    using Mycelium.Forge.Services;

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
        /// Gets the validation manager instance handling field validation states.
        /// </summary>
        public ValidationManager ValidationManager { get; } = new ValidationManager();

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
        /// Handles changes to the organization name input value.
        /// </summary>
        /// <param name="value">The new organization name.</param>
        public void OnOrganizationNameChanged(string value)
        {
            this.OrganizationName = value ?? string.Empty;
            this.ValidationManager.ClearError(nameof(this.OrganizationName));
        }

        /// <summary>
        /// Handles changes to the organization scope input value.
        /// </summary>
        /// <param name="value">The new scope identifier.</param>
        public void OnScopeChanged(string value)
        {
            this.Scope = value ?? string.Empty;
            this.ValidationManager.ClearError(nameof(this.Scope));
        }

        /// <summary>
        /// Handles changes to the billing email input value.
        /// </summary>
        /// <param name="value">The new billing email address.</param>
        public void OnBillingEmailChanged(string value)
        {
            this.BillingEmail = value ?? string.Empty;
            this.ValidationManager.ClearError(nameof(this.BillingEmail));
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
        /// Handles the create organization action, validating and emitting the entered values before closing.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnCreateOrganizationClicked()
        {
            var isValid = this.ValidationManager
                .Check(nameof(this.OrganizationName), !string.IsNullOrWhiteSpace(this.OrganizationName), "Organization name is required.")
                .Check(nameof(this.Scope), !string.IsNullOrWhiteSpace(this.Scope), "Scope is required.")
                .Check(nameof(this.BillingEmail), !string.IsNullOrWhiteSpace(this.BillingEmail), "Billing email is required.")
                .IsValid;

            if (!isValid)
            {
                return;
            }

            var result = new CreateOrganizationResult
            {
                OrganizationName = this.OrganizationName.Trim(),
                Scope = this.Scope.Trim(),
                BillingEmail = this.BillingEmail.Trim()
            };

            await this.OnResult.InvokeAsync(result);

            if (this.DialogReference != null)
            {
                await this.DialogReference.CloseAsync(DialogResult.Ok(result));
            }
        }
    }
}
