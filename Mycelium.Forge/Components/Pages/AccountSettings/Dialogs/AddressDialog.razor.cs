// ------------------------------------------------------------------------------------------------
// <copyright file="AddressDialog.razor.cs" company="Starion Group S.A.">
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

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Services;

    /// <summary>
    /// Dialog component allowing users to create or edit an address.
    /// </summary>
    public partial class AddressDialog
    {
        /// <summary>
        /// Gets or sets the dialog reference provided by <see cref="DialogService" />.
        /// </summary>
        [CascadingParameter]
        public IDialogReference DialogReference { get; set; }

        /// <summary>
        /// Gets or sets the address to edit, or <see langword="null" /> when adding a new address.
        /// </summary>
        [Parameter]
        public Address Address { get; set; }

        /// <summary>
        /// Gets or sets the list of selectable countries.
        /// </summary>
        [Parameter]
        [EditorRequired]
        public IReadOnlyList<ICountry> Countries { get; set; } = [];

        /// <summary>
        /// Gets or sets the event callback invoked with the edited address when the user saves.
        /// </summary>
        [Parameter]
        public EventCallback<Address> OnResult { get; set; }

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
        /// Gets the working copy of the address being edited.
        /// </summary>
        public Address EditedAddress { get; private set; } = new();

        /// <summary>
        /// Gets the currently selected country.
        /// </summary>
        public ICountry SelectedCountry { get; private set; }

        /// <summary>
        /// Initializes the working address copy and selected country.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.EditedAddress = this.Address?.Clone() ?? new Address
            {
                Id = Guid.NewGuid()
            };

            this.SelectedCountry = this.Countries.FirstOrDefault(country => country.Id == this.EditedAddress.Country);
        }

        /// <summary>
        /// Gets the display name for the specified country.
        /// </summary>
        /// <param name="country">The country instance.</param>
        /// <returns>The country name, or a placeholder when no country is selected.</returns>
        private static string GetCountryName(ICountry country)
        {
            return country?.Name ?? "Select a country";
        }

        /// <summary>
        /// Handles changes to the first address line input value.
        /// </summary>
        /// <param name="value">The new address line 1 value.</param>
        private void OnAddressLine1Changed(string value)
        {
            this.EditedAddress.AddressLine1 = value ?? string.Empty;
            this.ValidationManager.ClearError(nameof(this.Address.AddressLine1));
        }

        /// <summary>
        /// Handles changes to the second address line input value.
        /// </summary>
        /// <param name="value">The new address line 2 value.</param>
        private void OnAddressLine2Changed(string value)
        {
            this.EditedAddress.AddressLine2 = value ?? string.Empty;
        }

        /// <summary>
        /// Handles changes to the locality input value.
        /// </summary>
        /// <param name="value">The new locality value.</param>
        private void OnLocalityChanged(string value)
        {
            this.EditedAddress.Locality = value ?? string.Empty;
            this.ValidationManager.ClearError(nameof(this.Address.Locality));
        }

        /// <summary>
        /// Handles changes to the region input value.
        /// </summary>
        /// <param name="value">The new region value.</param>
        private void OnRegionChanged(string value)
        {
            this.EditedAddress.Region = value ?? string.Empty;
        }

        /// <summary>
        /// Handles changes to the postal code input value.
        /// </summary>
        /// <param name="value">The new postal code value.</param>
        private void OnPostalCodeChanged(string value)
        {
            this.EditedAddress.PostalCode = value ?? string.Empty;
        }

        /// <summary>
        /// Handles changes to the selected country.
        /// </summary>
        /// <param name="country">The newly selected country.</param>
        private void OnCountryChanged(ICountry country)
        {
            this.SelectedCountry = country;
            this.EditedAddress.Country = country?.Id ?? Guid.Empty;
            this.ValidationManager.ClearError(nameof(this.Address.Country));
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
        /// Validates the address fields, invokes <see cref="OnResult" /> with the edited address, and closes the dialog.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnSaveClicked()
        {
            var isValid = this.ValidationManager
                .Check(nameof(this.Address.AddressLine1), !string.IsNullOrWhiteSpace(this.EditedAddress.AddressLine1), "Address line 1 is required.")
                .Check(nameof(this.Address.Locality), !string.IsNullOrWhiteSpace(this.EditedAddress.Locality), "City is required.")
                .Check(nameof(this.Address.Country), this.EditedAddress.Country != Guid.Empty, "Country is required.")
                .IsValid;

            if (!isValid)
            {
                return;
            }

            await this.OnResult.InvokeAsync(this.EditedAddress);

            if (this.DialogReference != null)
            {
                await this.DialogReference.CloseAsync(DialogResult.Ok());
            }
        }
    }
}
