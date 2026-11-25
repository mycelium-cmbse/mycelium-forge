// ------------------------------------------------------------------------------------------------
// <copyright file="ChangeEmailDialog.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.AccountSettings.Dialogs
{
    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Dal.AutoGenDtoValidator;
    using Mycelium.Forge.Services;

    /// <summary>
    /// Dialog component allowing users to change their account email address.
    /// </summary>
    /// <remarks>
    /// To be replaced in ticket #221 by Keycloak's <c>kc_action=UPDATE_EMAIL</c> OIDC flow.
    /// </remarks>
    public partial class ChangeEmailDialog
    {
        /// <summary>
        /// Gets or sets the dialog reference provided by <see cref="DialogService" />.
        /// </summary>
        [CascadingParameter]
        public IDialogReference DialogReference { get; set; }

        /// <summary>
        /// Gets or sets the user's current email address.
        /// </summary>
        [Parameter]
        public string CurrentEmail { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the event callback invoked with the validated new email address when confirmed.
        /// </summary>
        [Parameter]
        public EventCallback<string> OnResult { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the dialog is cancelled.
        /// </summary>
        [Parameter]
        public EventCallback OnCancel { get; set; }

        /// <summary>
        /// Gets the validation manager instance handling field validation state.
        /// </summary>
        public ValidationManager ValidationManager { get; } = new();

        /// <summary>
        /// Gets or sets the entered new email address.
        /// </summary>
        public string NewEmail { get; set; } = string.Empty;

        /// <summary>
        /// Handles changes to the new email input value and clears active validation errors.
        /// </summary>
        /// <param name="value">The updated email input value.</param>
        private void OnNewEmailChanged(string value)
        {
            this.NewEmail = value ?? string.Empty;
            this.ValidationManager.ClearError(nameof(this.NewEmail));
        }

        /// <summary>
        /// Handles the cancel button click event, invoking <see cref="OnCancel" /> and cancelling the dialog.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnCancelClicked()
        {
            await this.OnCancel.InvokeAsync();

            if (this.DialogReference != null)
            {
                await this.DialogReference.CancelAsync();
            }
        }

        /// <summary>
        /// Validates the entered email address, invokes <see cref="OnResult" />, and closes the dialog.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnConfirmClicked()
        {
            var normalizedEmail = this.NewEmail.Trim().ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(normalizedEmail) || !AccountValidator.EmailFormatRegex().IsMatch(normalizedEmail))
            {
                this.ValidationManager.Check(nameof(this.NewEmail), false, "Please enter a valid email address.");
                return;
            }

            if (string.Equals(normalizedEmail, this.CurrentEmail, StringComparison.OrdinalIgnoreCase))
            {
                this.ValidationManager.Check(nameof(this.NewEmail), false, "New email address must be different from your current email.");
                return;
            }

            await this.OnResult.InvokeAsync(normalizedEmail);

            if (this.DialogReference != null)
            {
                await this.DialogReference.CloseAsync(DialogResult.Ok());
            }
        }
    }
}
