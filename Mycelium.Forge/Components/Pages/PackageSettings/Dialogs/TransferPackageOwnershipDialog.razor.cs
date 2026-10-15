// ------------------------------------------------------------------------------------------------
// <copyright file="TransferPackageOwnershipDialog.razor.cs" company="Starion Group S.A.">
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

    using Mycelium.Forge.Extensions;

    /// <summary>
    /// Represents a modal dialog for transferring package ownership to another account or organization.
    /// </summary>
    public partial class TransferPackageOwnershipDialog : ComponentBase
    {
        /// <summary>
        /// Gets or sets the cascading dialog reference used to control and close the dialog.
        /// </summary>
        [CascadingParameter]
        public IDialogReference DialogReference { get; set; }

        /// <summary>
        /// Gets or sets the package name being transferred.
        /// </summary>
        [Parameter]
        public string PackageName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current owner short name or scope.
        /// </summary>
        [Parameter]
        public string CurrentOwner { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the event callback invoked with the target scope when ownership transfer is confirmed.
        /// </summary>
        [Parameter]
        public EventCallback<string> OnResult { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the dialog is cancelled.
        /// </summary>
        [Parameter]
        public EventCallback OnCancel { get; set; }

        /// <summary>
        /// Gets or sets the target recipient username or organization scope.
        /// </summary>
        public string TargetScope { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the confirmation package name input typed by the user.
        /// </summary>
        public string ConfirmationPackageName { get; set; } = string.Empty;

        /// <summary>
        /// Determines whether the transfer action button is disabled.
        /// </summary>
        /// <returns><see langword="true" /> if transfer should be disabled; otherwise, <see langword="false" />.</returns>
        public bool IsTransferDisabled()
        {
            return string.IsNullOrWhiteSpace(this.TargetScope) || !string.Equals(this.ConfirmationPackageName, this.PackageName, StringComparison.Ordinal);
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
        /// Handles the transfer confirmation action, closing the dialog and invoking the result callback.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnTransferClicked()
        {
            if (this.IsTransferDisabled())
            {
                return;
            }

            var cleanTarget = this.TargetScope.CleanScope();

            await this.OnResult.InvokeAsync(cleanTarget);

            if (this.DialogReference != null)
            {
                await this.DialogReference.CloseAsync(DialogResult.Ok());
            }
        }
    }
}
