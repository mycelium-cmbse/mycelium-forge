// ------------------------------------------------------------------------------------------------
// <copyright file="IAccountDetailsViewModel.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.ViewModels.AccountDetails
{
    using Mycelium.Forge.Common;
    using Mycelium.Forge.ViewModels.Rows;

    /// <summary>
    /// Defines the view model contract for the account and publisher profile page.
    /// </summary>
    public interface IAccountDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the account profile details.
        /// </summary>
        IAccount Account { get; set; }

        /// <summary>
        /// Gets or sets the collection of package rows published by the account.
        /// </summary>
        IReadOnlyList<PackageRowViewModel> Packages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current authenticated user owns this account profile.
        /// </summary>
        bool IsCurrentUser { get; set; }

        /// <summary>
        /// Initializes the account view model state for the specified account short name asynchronously.
        /// </summary>
        /// <param name="shortName">The short name of the account.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous initialization.</returns>
        Task InitializeViewModel(string shortName, CancellationToken cancellationToken = default);
    }
}
