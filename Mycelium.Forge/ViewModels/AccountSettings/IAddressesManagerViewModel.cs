// ------------------------------------------------------------------------------------------------
// <copyright file="IAddressesManagerViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.AccountSettings
{
    using DynamicData;

    using ErrorOr;

    using Mycelium.Forge.Common;

    /// <summary>
    /// Defines the view model contract for managing the addresses of the user account.
    /// </summary>
    public interface IAddressesManagerViewModel
    {
        /// <summary>
        /// Gets the collection of addresses owned by the user account.
        /// </summary>
        SourceList<Address> Addresses { get; }

        /// <summary>
        /// Gets the countries that can be selected for an address.
        /// </summary>
        IReadOnlyList<ICountry> Countries { get; }

        /// <summary>
        /// Gets or sets the identifier of the primary address of the user account.
        /// </summary>
        Guid? PrimaryAddressId { get; set; }

        /// <summary>
        /// Gets or sets the function invoked to persist a new primary address identifier on the user account.
        /// </summary>
        Func<Guid?, Task<ErrorOr<Success>>> OnPrimaryAddressChanging { get; set; }

        /// <summary>
        /// Initializes the view model state asynchronously for the specified account profile.
        /// </summary>
        /// <param name="profile">The current user profile account.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task InitializeViewModel(Account profile);

        /// <summary>
        /// Creates a new address for the user account asynchronously. The address becomes the primary address when there is none.
        /// </summary>
        /// <param name="address">The address to create.</param>
        /// <returns>A task indicating the success or failure of the operation.</returns>
        Task<ErrorOr<Success>> CreateAddress(Address address);

        /// <summary>
        /// Updates an existing address of the user account asynchronously, and updates the primary address of the account when it
        /// changed.
        /// </summary>
        /// <param name="address">The updated address.</param>
        /// <param name="isPrimary">A value indicating whether the address is the primary address of the account.</param>
        /// <returns>A task indicating the success or failure of the operation.</returns>
        Task<ErrorOr<Success>> UpdateAddress(Address address, bool isPrimary);

        /// <summary>
        /// Deletes an address of the user account asynchronously, clearing the primary address first when needed.
        /// </summary>
        /// <param name="addressId">The identifier of the address to delete.</param>
        /// <returns>A task indicating the success or failure of the operation.</returns>
        Task<ErrorOr<Success>> DeleteAddress(Guid addressId);
    }
}
