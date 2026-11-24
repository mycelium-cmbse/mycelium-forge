// ------------------------------------------------------------------------------------------------
// <copyright file="AddressesManager.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.AccountSettings
{
    using BlazorBlueprint.Components;

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;
    using Mycelium.Forge.Components.Pages.AccountSettings.Dialogs;
    using Mycelium.Forge.Extensions;
    using Mycelium.Forge.ViewModels.AccountSettings;

    /// <summary>
    /// Represents the addresses manager card component in the account settings page.
    /// </summary>
    public partial class AddressesManager : DisposableComponent
    {
        /// <summary>
        /// Gets or sets the dialog service used to display modal dialogs.
        /// </summary>
        [Inject]
        public DialogService DialogService { get; set; }

        /// <summary>
        /// Gets or sets the CSS class applied to the card.
        /// </summary>
        [Parameter]
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the view model for managing addresses.
        /// </summary>
        [Parameter]
        [EditorRequired]
        public IAddressesManagerViewModel ViewModel { get; set; }

        /// <summary>
        /// Subscribes to address collection changes when the component initializes.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.Disposables.Add(this.ViewModel.Addresses.Connect().SubscribeAsync(_ => this.InvokeAsync(this.StateHasChanged)));
        }

        /// <summary>
        /// Determines whether the specified address is the primary address of the account.
        /// </summary>
        /// <param name="address">The address to evaluate.</param>
        /// <returns><c>true</c> if the address is the primary address; otherwise, <c>false</c>.</returns>
        private bool IsPrimary(Address address)
        {
            return this.ViewModel.PrimaryAddressId == address.Id;
        }

        /// <summary>
        /// Resolves the display name of the country with the specified identifier.
        /// </summary>
        /// <param name="countryId">The country identifier.</param>
        /// <returns>The country name, or an empty string if not found.</returns>
        private string GetCountryName(Guid countryId)
        {
            return this.ViewModel.Countries.FirstOrDefault(country => country.Id == countryId)?.Name ?? string.Empty;
        }

        /// <summary>
        /// Opens the address dialog to add a new address.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnAddAddress()
        {
            var onResult = new EventCallbackFactory().Create(this, async (Address address) => await this.ViewModel.CreateAddress(address));

            var parameters = new Dictionary<string, object>
            {
                { nameof(AddressDialog.Countries), this.ViewModel.Countries },
                { nameof(AddressDialog.OnResult), onResult }
            };

            var options = new DialogOpenOptions
            {
                Title = "Add address"
            };

            await this.DialogService.OpenAsync<AddressDialog>(parameters, options);
        }

        /// <summary>
        /// Opens the address dialog to edit an existing address.
        /// </summary>
        /// <param name="address">The address to edit.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnEditAddress(Address address)
        {
            var isPrimary = this.IsPrimary(address);
            var onResult = new EventCallbackFactory().Create(this, async (Address updatedAddress) => await this.ViewModel.UpdateAddress(updatedAddress, isPrimary));

            var parameters = new Dictionary<string, object>
            {
                { nameof(AddressDialog.Address), address },
                { nameof(AddressDialog.Countries), this.ViewModel.Countries },
                { nameof(AddressDialog.OnResult), onResult }
            };

            var options = new DialogOpenOptions
            {
                Title = "Edit address"
            };

            await this.DialogService.OpenAsync<AddressDialog>(parameters, options);
        }

        /// <summary>
        /// Sets the specified address as the primary address asynchronously.
        /// </summary>
        /// <param name="address">The address to mark as primary.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnSetPrimaryAddress(Address address)
        {
            await this.ViewModel.UpdateAddress(address, true);
        }

        /// <summary>
        /// Prompts for confirmation and deletes the specified address asynchronously.
        /// </summary>
        /// <param name="address">The address to delete.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private async Task OnDeleteAddress(Address address)
        {
            var options = new ConfirmDialogOptions
            {
                Destructive = true,
                ConfirmText = "Delete",
                CancelText = "Cancel"
            };

            var result = await this.DialogService.ConfirmAsync("Delete address", "Are you sure you want to delete this address?", options);

            if (result.Confirmed)
            {
                await this.ViewModel.DeleteAddress(address.Id);
            }
        }
    }
}
