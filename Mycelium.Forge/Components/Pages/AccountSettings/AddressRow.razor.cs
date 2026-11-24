// ------------------------------------------------------------------------------------------------
// <copyright file="AddressRow.razor.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Components.Pages.AccountSettings
{
    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Represents a single address row in the account settings addresses card.
    /// </summary>
    public partial class AddressRow
    {
        /// <summary>
        /// Gets or sets the address to display.
        /// </summary>
        [Parameter]
        [EditorRequired]
        public Address Address { get; set; }

        /// <summary>
        /// Gets or sets the resolved country name for the address.
        /// </summary>
        [Parameter]
        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this address is the primary address.
        /// </summary>
        [Parameter]
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the user sets this address as primary.
        /// </summary>
        [Parameter]
        public EventCallback<Address> OnSetPrimary { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the user edits this address.
        /// </summary>
        [Parameter]
        public EventCallback<Address> OnEdit { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the user deletes this address.
        /// </summary>
        [Parameter]
        public EventCallback<Address> OnDelete { get; set; }

        /// <summary>
        /// Formats the street lines of the address into a single summary string.
        /// </summary>
        /// <returns>The formatted street summary.</returns>
        private string GetStreetSummary()
        {
            string[] parts = [this.Address.AddressLine1, this.Address.AddressLine2];
            return string.Join(", ", parts.Where(part => !string.IsNullOrWhiteSpace(part)));
        }

        /// <summary>
        /// Formats the locality, region, postal code, and country into a single summary string.
        /// </summary>
        /// <returns>The formatted locality summary.</returns>
        private string GetLocalitySummary()
        {
            string[] parts = [this.Address.Locality, this.Address.Region, this.Address.PostalCode, this.CountryName];
            return string.Join(", ", parts.Where(part => !string.IsNullOrWhiteSpace(part)));
        }

        /// <summary>
        /// Invokes the <see cref="OnSetPrimary" /> callback for this address.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private Task OnSetPrimaryClicked()
        {
            return this.OnSetPrimary.InvokeAsync(this.Address);
        }

        /// <summary>
        /// Invokes the <see cref="OnEdit" /> callback for this address.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private Task OnEditClicked()
        {
            return this.OnEdit.InvokeAsync(this.Address);
        }

        /// <summary>
        /// Invokes the <see cref="OnDelete" /> callback for this address.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        private Task OnDeleteClicked()
        {
            return this.OnDelete.InvokeAsync(this.Address);
        }
    }
}
