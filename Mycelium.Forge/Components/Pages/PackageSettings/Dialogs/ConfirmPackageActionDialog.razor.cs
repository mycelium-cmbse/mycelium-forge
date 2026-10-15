// ------------------------------------------------------------------------------------------------
// <copyright file="ConfirmPackageActionDialog.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.PackageSettings.Dialogs
{
    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;

    /// <summary>
    /// Represents a confirmation dialog requiring the user to type the package name before executing a destructive action.
    /// </summary>
    public partial class ConfirmPackageActionDialog : ComponentBase
    {
        /// <summary>
        /// Gets or sets the cascading dialog reference used to control and close the dialog.
        /// </summary>
        [CascadingParameter]
        public IDialogReference DialogReference { get; set; }

        /// <summary>
        /// Gets or sets the target package name to match.
        /// </summary>
        [Parameter]
        public string PackageName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the dialog title.
        /// </summary>
        [Parameter]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the explanatory description displayed in the dialog body.
        /// </summary>
        [Parameter]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the action button display text.
        /// </summary>
        [Parameter]
        public string ActionButtonText { get; set; } = "Confirm";

        /// <summary>
        /// Gets or sets the event callback invoked when the user confirms the action.
        /// </summary>
        [Parameter]
        public EventCallback OnResult { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the dialog is cancelled.
        /// </summary>
        [Parameter]
        public EventCallback OnCancel { get; set; }

        /// <summary>
        /// Gets or sets the confirmation input text typed by the user.
        /// </summary>
        public string ConfirmationInput { get; set; } = string.Empty;

        /// <summary>
        /// Determines whether the confirm action button should be disabled.
        /// </summary>
        /// <returns><see langword="true" /> if confirmation should be disabled; otherwise, <see langword="false" />.</returns>
        public bool IsConfirmDisabled()
        {
            return !string.Equals(this.ConfirmationInput, this.PackageName, StringComparison.Ordinal);
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
        /// Handles the confirm action, closing the dialog and invoking the result callback.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnConfirmClicked()
        {
            if (this.IsConfirmDisabled())
            {
                return;
            }

            await this.OnResult.InvokeAsync();

            if (this.DialogReference != null)
            {
                await this.DialogReference.CloseAsync(DialogResult.Ok());
            }
        }
    }
}
