// ------------------------------------------------------------------------------------------------
// <copyright file="InputPromptDialog.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Common
{
    using BlazorBlueprint.Components;

    using ErrorOr;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Services;

    /// <summary>
    /// Represents a reusable modal dialog for collecting and validating a single text input value.
    /// </summary>
    /// <remarks>
    /// Provided instead of BlazorBlueprint's built-in <see cref="DialogService.PromptAsync" /> because
    /// <see cref="PromptDialogOptions" /> only supports <c>DefaultValue</c>, <c>Placeholder</c>, <c>Required</c>, and
    /// <c>MaxLength</c>, with no custom or asynchronous validation hook, and <c>PromptAsync</c> closes before validation can
    /// run and renders outside <see cref="DialogComponent" />. Wrapping <see cref="DialogComponent" /> allows inline
    /// <see cref="ValidationManager" /> feedback via <c>BbFieldError</c> and keeps styling consistent across Forge dialogs.
    /// </remarks>
    public partial class InputPromptDialog
    {
        /// <summary>
        /// Gets or sets the cascading dialog reference used to control and close the dialog.
        /// </summary>
        [CascadingParameter]
        public IDialogReference DialogReference { get; set; }

        /// <summary>
        /// Gets or sets the optional explanatory description displayed above the input field.
        /// </summary>
        [Parameter]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the optional label displayed above the input field.
        /// </summary>
        [Parameter]
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the placeholder text displayed in the input field.
        /// </summary>
        [Parameter]
        public string Placeholder { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the initial input value populated when the dialog initializes.
        /// </summary>
        [Parameter]
        public string InitialValue { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether a non-whitespace value is required.
        /// </summary>
        [Parameter]
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets the confirmation button text.
        /// </summary>
        [Parameter]
        public string ConfirmText { get; set; } = "Save";

        /// <summary>
        /// Gets or sets the cancellation button text.
        /// </summary>
        [Parameter]
        public string CancelText { get; set; } = "Cancel";

        /// <summary>
        /// Gets or sets the optional asynchronous validator invoked with the trimmed input value before confirming.
        /// </summary>
        [Parameter]
        public Func<string, Task<ErrorOr<Success>>> ValidateAsync { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked with the validated, trimmed input value when the user confirms.
        /// </summary>
        [Parameter]
        public EventCallback<string> OnResult { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the dialog is cancelled or closed.
        /// </summary>
        [Parameter]
        public EventCallback OnCancel { get; set; }

        /// <summary>
        /// Gets the validation manager instance handling field validation state.
        /// </summary>
        public ValidationManager ValidationManager { get; } = new();

        /// <summary>
        /// Gets or sets the current input text value.
        /// </summary>
        public string InputValue { get; set; } = string.Empty;

        /// <summary>
        /// Gets a value indicating whether <see cref="Description" /> has non-whitespace text.
        /// </summary>
        public bool HasDescription => !string.IsNullOrWhiteSpace(this.Description);

        /// <summary>
        /// Gets a value indicating whether <see cref="Label" /> has non-whitespace text.
        /// </summary>
        public bool HasLabel => !string.IsNullOrWhiteSpace(this.Label);

        /// <summary>
        /// Initializes the dialog state with the supplied <see cref="InitialValue" />.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.InputValue = this.InitialValue ?? string.Empty;
        }

        /// <summary>
        /// Handles changes to the input value and clears any active validation error.
        /// </summary>
        /// <param name="value">The updated input value.</param>
        public void OnInputValueChanged(string value)
        {
            this.InputValue = value ?? string.Empty;
            this.ValidationManager.ClearError(nameof(this.InputValue));
        }

        /// <summary>
        /// Handles the cancel action, invoking <see cref="OnCancel" /> and cancelling the dialog.
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
        /// Validates the entered input, invokes <see cref="OnResult" /> with the trimmed value, and closes the dialog.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        public async Task OnConfirmClicked()
        {
            if (this.Required && string.IsNullOrWhiteSpace(this.InputValue))
            {
                this.ValidationManager.Check(nameof(this.InputValue), false, "This field is required.");
                return;
            }

            this.InputValue = this.InputValue.Trim();

            if (this.ValidateAsync != null)
            {
                var validationResult = await this.ValidateAsync.Invoke(this.InputValue);

                if (validationResult.IsError)
                {
                    this.ValidationManager.Check(nameof(this.InputValue), false, validationResult.FirstError.Description);
                    return;
                }
            }

            await this.OnResult.InvokeAsync(this.InputValue);

            if (this.DialogReference != null)
            {
                await this.DialogReference.CloseAsync(DialogResult.Ok());
            }
        }
    }
}
