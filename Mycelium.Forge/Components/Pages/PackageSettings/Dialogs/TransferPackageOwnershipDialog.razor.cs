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

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Dal.AutoGenDtoValidator;
    using Mycelium.Forge.Services;
    using Mycelium.Forge.ViewModels.PackageSettings;

    /// <summary>
    /// Represents a modal dialog for transferring package ownership to another account or organization.
    /// </summary>
    public partial class TransferPackageOwnershipDialog : ComponentBase
    {
        /// <summary>
        /// Gets or sets the (injected) <see cref="ITransferPackageOwnershipDialogViewModel" /> used to validate the target scope.
        /// </summary>
        [Inject]
        public ITransferPackageOwnershipDialogViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the cascading dialog reference used to control and close the dialog.
        /// </summary>
        [CascadingParameter]
        public IDialogReference DialogReference { get; set; }

        /// <summary>
        /// Gets or sets the package being transferred.
        /// </summary>
        [Parameter]
        public IPackage Package { get; set; }

        /// <summary>
        /// Gets or sets the current owner short name or scope.
        /// </summary>
        [Parameter]
        public string CurrentOwner { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the event callback invoked with the target scope when ownership transfer is confirmed.
        /// </summary>
        [Parameter]
        public EventCallback<IScope> OnResult { get; set; }

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
        /// Gets the validation manager used to validate dialog inputs.
        /// </summary>
        public ValidationManager ValidationManager { get; } = new();

        /// <summary>
        /// Handles changes to the target scope input and clears previous validation errors.
        /// </summary>
        /// <param name="value">The updated target scope input.</param>
        public void OnTargetScopeChanged(string value)
        {
            this.TargetScope = value ?? string.Empty;
            this.ValidationManager.ClearError(nameof(this.TargetScope));
        }

        /// <summary>
        /// Handles changes to the confirmation package name input and clears previous validation errors.
        /// </summary>
        /// <param name="value">The updated confirmation package name input.</param>
        public void OnConfirmationPackageNameChanged(string value)
        {
            this.ConfirmationPackageName = value ?? string.Empty;
            this.ValidationManager.ClearError(nameof(this.ConfirmationPackageName));
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
        /// Handles the transfer confirmation action, verifying target scope and confirmation before closing the dialog and
        /// invoking
        /// the result callback.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnTransferClicked()
        {
            var isTargetValid = !string.IsNullOrWhiteSpace(this.TargetScope) && AccountValidator.ShortNameFormatRegex().IsMatch(this.TargetScope);

            var isValid = this.ValidationManager
                .Check(nameof(this.TargetScope), isTargetValid, "Target scope must start and end with an alphanumeric character and contain only lowercase alphanumeric characters and hyphens.")
                .Check(nameof(this.ConfirmationPackageName), string.Equals(this.ConfirmationPackageName, this.Package.Name, StringComparison.Ordinal), "Confirmation name does not match.")
                .IsValid;

            if (!isValid)
            {
                return;
            }

            var targetResult = await this.ViewModel.ValidateTargetAsync(this.Package, this.CurrentOwner, this.TargetScope);

            if (targetResult.IsError)
            {
                this.ValidationManager.Check(nameof(this.TargetScope), false, targetResult.FirstError.Description);
                return;
            }

            await this.OnResult.InvokeAsync(targetResult.Value);

            if (this.DialogReference != null)
            {
                await this.DialogReference.CloseAsync(DialogResult.Ok());
            }
        }
    }
}
