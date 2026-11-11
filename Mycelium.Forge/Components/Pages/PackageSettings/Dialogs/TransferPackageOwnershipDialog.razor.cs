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
    using Mycelium.Forge.Dal.Services;
    using Mycelium.Forge.Services;

    /// <summary>
    /// Represents a modal dialog for transferring package ownership to another account or organization.
    /// </summary>
    public partial class TransferPackageOwnershipDialog : ComponentBase
    {
        /// <summary>
        /// Gets or sets the (injected) <see cref="IScopeService" /> used to lookup the target scope.
        /// </summary>
        [Inject]
        public IScopeService ScopeService { get; set; }

        /// <summary>
        /// Gets or sets the (injected) <see cref="IUserService" /> used to retrieve the current user context.
        /// </summary>
        [Inject]
        public IUserService UserService { get; set; }

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
                .Check(nameof(this.ConfirmationPackageName), string.Equals(this.ConfirmationPackageName, this.PackageName, StringComparison.Ordinal), "Confirmation name does not match.")
                .IsValid;

            if (!isValid)
            {
                return;
            }

            var userContext = await this.UserService.GetUserContext();
            var scopeResult = await this.ScopeService.ReadByShortNameAsync(userContext, CancellationToken.None, [this.TargetScope]);

            if (scopeResult.IsError || scopeResult.Value.Count == 0)
            {
                this.ValidationManager.Check(nameof(this.TargetScope), false, $"Scope '{this.TargetScope}' not found.");
                return;
            }

            var scope = scopeResult.Value[0];

            if (string.Equals(this.TargetScope, this.CurrentOwner, StringComparison.OrdinalIgnoreCase))
            {
                this.ValidationManager.Check(nameof(this.TargetScope), false, "You cannot transfer ownership to the current owner scope.");
                return;
            }

            if (scope is IOrganization organization &&
                (!userContext.AccountId.HasValue || !organization.Administrator.Contains(userContext.AccountId.Value)) &&
                !PermissionGuard.HasPermission(userContext, PermissionKind.TransferPackageOwnership))
            {
                this.ValidationManager.Check(nameof(this.TargetScope), false, "You do not have administrative rights on the target organization.");
                return;
            }

            await this.OnResult.InvokeAsync(scope);

            if (this.DialogReference != null)
            {
                await this.DialogReference.CloseAsync(DialogResult.Ok());
            }
        }
    }
}
